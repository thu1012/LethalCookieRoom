#if UNITY_EDITOR
namespace HyTeKGames.SRPMaterialConverter
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.Rendering;

    public static class SRPMC_Utilities
    {
        // Detects broken materials by checking if they failed to compile.
        public static List<Material> FindBrokenMaterials()
        {
            List<Material> m = FindAllMaterials();
            List<Material> brokenMaterials = m.Where(x => x.shader.name == "Hidden/InternalErrorShader").ToList();
            return brokenMaterials;
        }

        // Find every materials in the project.
        public static List<Material> FindAllMaterials()
        {
            List<Material> assets = new List<Material>();
            string[] guids = AssetDatabase.FindAssets("t:material");
            for (int i = 0; i < guids.Length; i++)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                Material asset = AssetDatabase.LoadAssetAtPath<Material>(assetPath);
                if (asset != null)
                    assets.Add(asset);
            }
            return assets;
        }

        // Checks if textures are power of 2 (256,512,1024 etc..)
        public static bool IsPowerOfTwo(this Texture2D tex)
        {
            return (tex.width > 0 && (tex.width & (tex.width - 1)) == 0) && (tex.height > 0 && (tex.height & (tex.height - 1)) == 0);
        }

        public static Texture2D ConvertToChannel(Texture2D originalTexture, int channel)
        {
            Texture2D newTexture = new Texture2D(originalTexture.width, originalTexture.height);
            Color[] originalPixels = originalTexture.GetPixels();
            Color[] newPixels = new Color[originalPixels.Length];
            for (int i = 0; i < originalPixels.Length; i++)
            {
                float grayValue = (originalPixels[i].r + originalPixels[i].g + originalPixels[i].b) / 3f;
                switch (channel)
                {
                    case 0:
                        newPixels[i] = new Color(grayValue, 0, 0, 1);
                        break;
                    case 1:
                        newPixels[i] = new Color(0, grayValue, 0, 1);
                        break;
                    case 2:
                        newPixels[i] = new Color(0, 0, grayValue, 1);
                        break;
                    case 3:
                        newPixels[i] = new Color(0, 0, 0, grayValue);
                        break;
                }
            }

            newTexture.SetPixels(newPixels);
            newTexture.Apply();
            return newTexture;
        }

        // Rescale a texture.
        public static Texture2D RescaleTexture(Texture2D src, int width, int height, FilterMode mode = FilterMode.Trilinear)
        {
            Rect texRect = new(0, 0, width, height);

            RenderTexture renderTexture = new RenderTexture(src.width, src.height, 0);
            Graphics.Blit(src, renderTexture);
            RenderTexture.active = renderTexture;
            Texture2D texture = new Texture2D(renderTexture.width, renderTexture.height);
            texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            texture.Apply();

            GPUScale(texture, width, height, mode);

            //Get rendered data back to a new texture
            Texture2D result = new(width, height, TextureFormat.ARGB32, true);
            result.Reinitialize(width, height);
            result.ReadPixels(texRect, 0, 0, true);

            RenderTexture.active = null;
            renderTexture.Release();
            GameObject.DestroyImmediate(renderTexture);
            return result;
        }

        // Scale the texture using the GPU if possible.
        private static void GPUScale(Texture2D src, int width, int height, FilterMode fmode)
        {
            src.filterMode = fmode;
            src.Apply(true);
            RenderTexture rtt = new(width, height, 32);
            Graphics.SetRenderTarget(rtt);
            GL.LoadPixelMatrix(0, 1, 1, 0);
            GL.Clear(true, true, new Color(0, 0, 0, 0));
            Graphics.DrawTexture(new Rect(0, 0, 1, 1), src);
        }

        // Checks if a texture is 2D.
        public static bool IsTexture2D(this Texture tex)
        {
            TextureImporter importer = ((TextureImporter)AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(tex)));
            if (!importer) return false;
            TextureImporterShape shape = importer.textureShape;
            return shape == TextureImporterShape.Texture2D;
        }

        // Extracts HDRP MaskMap channels.
        public static List<byte[]> ExtractChannels(this Texture originalTexture)
        {
            List<byte[]> channels = new List<byte[]>();
            for (int i = 0; i < 4; i++)
            {
                RenderTexture renderTexture = new RenderTexture(originalTexture.width, originalTexture.height, 0);
                Graphics.Blit(originalTexture, renderTexture);
                Texture2D channelTexture = ExtractChannels(renderTexture, i);
                channels.Add(channelTexture.EncodeToPNG());
                RenderTexture.active = null;
                renderTexture.Release();
                GameObject.DestroyImmediate(renderTexture);
                GameObject.DestroyImmediate(channelTexture);
            }
            return channels;
        }

        // HDRP MaskMap channels names.
        public static List<string> channelsNames = new List<string>()
        {
            "Metallic",
            "AO",
            "DetailMask",
            "Smoothness"
        };

        // Extracts HDRP MaskMap channels.
        private static Texture2D ExtractChannels(RenderTexture renderTexture, int channelIndex)
        {
            RenderTexture.active = renderTexture;
            Texture2D texture = new Texture2D(renderTexture.width, renderTexture.height);
            texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            texture.Apply();

            Color[] pixels = texture.GetPixels();

            for (int i = 0; i < pixels.Length; i++)
            {
                switch (channelIndex)
                {
                    case 0:
                        pixels[i] = new Color(pixels[i].r, pixels[i].r, pixels[i].r, pixels[i].r);
                        break;
                    case 1:
                        pixels[i] = new Color(pixels[i].g, pixels[i].g, pixels[i].g, pixels[i].g);
                        break;
                    case 2:
                        pixels[i] = new Color(pixels[i].b, pixels[i].b, pixels[i].b, pixels[i].b);
                        break;
                    case 3:
                        pixels[i] = new Color(pixels[i].a, pixels[i].a, pixels[i].a, pixels[i].a);
                        break;
                }
            }

            texture.SetPixels(pixels);
            texture.Apply();

            return texture;
        }

        // We need to parse most of the materials values from the .mat file since Unity can't load the shader when its from another Render Pipeline.
        public static ParsedMaterial ParseMaterial(Material mat)
        {
            string file = AssetDatabase.GetAssetPath(mat);
            if (!File.Exists(file)) return null;
            string content = File.ReadAllText(file);
            // For some reasons, sometimes unity can extract .unitypackage files in binary (unity still recognize them, but have to convert them for the parser to work).
            if (!content.StartsWith("%YAML"))
            {
                bool oldValue = mat.enableInstancing;
                mat.enableInstancing = !mat.enableInstancing;
                AssetDatabase.SaveAssetIfDirty(mat);
                content = File.ReadAllText(file);
                if (!content.StartsWith("%YAML"))
                    return null;
                else
                    mat.enableInstancing = oldValue;
                AssetDatabase.SaveAssetIfDirty(mat);
            }
            ParsedMaterial parsedMat = new ParsedMaterial();
            parsedMat.gpuInstancing = mat.enableInstancing;
            string[] lines = content.Split("\n");

            // Parse value m_TexEnvs
            int texEnvsIndex = Array.FindIndex(lines, line => line.Contains("m_TexEnvs:"));
            if (texEnvsIndex != -1)
            {
                string lastParent = "";
                for (int subLine = texEnvsIndex + 1; subLine < lines.Length; subLine++)
                {
                    string lineContent = lines[subLine];
                    if (lineContent.StartsWith("    -"))
                    {
                        lastParent = lineContent.Remove(0, "    - ".Length).TrimEnd(':');
                        ParsedTexture parsedTex = new ParsedTexture();
                        Texture tex = mat.GetTexture(lastParent);
                        if (tex)
                        {
                            parsedTex.texture = tex;
                            parsedTex.scale = mat.GetTextureScale(lastParent);
                            parsedTex.offset = mat.GetTextureOffset(lastParent);
                        }
                        parsedMat.m_TexEnvs.Add(lastParent, parsedTex);
                    }
                    else if (lineContent.StartsWith("        "))
                    {
                        // These lines contains:
                        // m_Texture (fileID, guid, type)
                        // m_Scale (x,y)
                        // m_Offset (x,y)
                    }
                    else
                        break;
                }
            }

            // Parse Keywords
            int keywordsIndex = Array.FindIndex(lines, line => line.Contains("m_ValidKeywords:"));
            if (keywordsIndex != -1)
            {
                for (int subLine = keywordsIndex + 1; subLine < lines.Length; subLine++)
                {
                    string lineContent = lines[subLine];
                    if (lineContent.StartsWith("  - "))
                        parsedMat.m_ValidKeywords.Add(lineContent.Remove(0, "  - ".Length));
                    else
                        break;
                }
            }

            // Parse Floats
            int floatsIndex = Array.FindIndex(lines, line => line.Contains("m_Floats:"));
            if (floatsIndex != -1)
            {
                for (int subLine = floatsIndex + 1; subLine < lines.Length; subLine++)
                {
                    string lineContent = lines[subLine];
                    if (lineContent.StartsWith("    - "))
                    {
                        string[] stripped = lineContent.Remove(0, "    - ".Length).Split(":");
                        string name = stripped[0];
                        float val = ParseFloat(stripped[1]);
                        parsedMat.m_Floats.Add(name, val);
                    }
                    else
                        break;
                }
            }

            // Parse Colors
            int colorsIndex = Array.FindIndex(lines, line => line.Contains("m_Colors:"));
            if (colorsIndex != -1)
            {
                for (int subLine = colorsIndex + 1; subLine < lines.Length; subLine++)
                {
                    string lineContent = lines[subLine];
                    if (lineContent.StartsWith("    - "))
                    {
                        string[] stripped = lineContent.Remove(0, "    - ".Length).Split(":");
                        string name = stripped[0];
                        List<string> color = stripped.ToList();
                        color.RemoveAt(0);
                        Color val = ParseColor(string.Join(":", color).TrimStart(' '));
                        parsedMat.m_Colors.Add(name, val);
                    }
                    else
                        break;
                }
            }

            // Transparency & Alpha Clipping
            parsedMat.alphaClippingValue = parsedMat.m_Floats.ContainsKey("_Cutoff") ? parsedMat.m_Floats["_Cutoff"] : 0.5f;
            parsedMat.alphaClipping = parsedMat.m_ValidKeywords.Contains("_ALPHATEST_ON");
            parsedMat.transparent = mat.renderQueue == (int)RenderQueue.Transparent;

            // Emission
            int lightmapFlagsIndex = Array.FindIndex(lines, line => line.Contains("m_LightmapFlags:"));
            switch (SRPMC_Editor.originalPipeline)
            {
                case SRPMC_Pipeline.HDRP:
                    parsedMat.emissive = parsedMat.m_Floats.ContainsKey("_UseEmissiveIntensity") && parsedMat.m_Floats["_UseEmissiveIntensity"] == 1;
                    break;
                case SRPMC_Pipeline.URP:
                    if (lightmapFlagsIndex != -1)
                    {
                        string lineContent = lines[lightmapFlagsIndex];
                        float val = ParseFloat(lineContent.Remove(0, "  m_LightmapFlags: ".Length));
                        parsedMat.emissive = val != 4;
                    }
                    break;
                case SRPMC_Pipeline.Builtin:
                    if (lightmapFlagsIndex != -1)
                    {
                        string lineContent = lines[lightmapFlagsIndex];
                        float val = ParseFloat(lineContent.Remove(0, "  m_LightmapFlags: ".Length));
                        parsedMat.emissive = val != 4;
                    }
                    break;
            }

            // Render Face :
            parsedMat.renderFace = RenderFace.Front;
            float cull = 0;
            switch (SRPMC_Editor.originalPipeline)
            {
                case SRPMC_Pipeline.HDRP:
                    if (parsedMat.m_Floats.ContainsKey("_CullMode"))
                    {
                        cull = parsedMat.m_Floats["_CullMode"];
                        parsedMat.renderFace = cull == 2 ? RenderFace.Front : (cull == 1 ? RenderFace.Back : RenderFace.Both);
                    }
                    break;
                case SRPMC_Pipeline.URP:
                    if (parsedMat.m_Floats.ContainsKey("_Cull"))
                    {
                        cull = parsedMat.m_Floats["_Cull"];
                        parsedMat.renderFace = cull == 2 ? RenderFace.Front : (cull == 1 ? RenderFace.Back : RenderFace.Both);
                    }
                    break;
                default:
                    // Builtin does not have a renderFace feature.
                    break;
            }
            return parsedMat;
        }

        // Parse color from .mat file.
        public static Color ParseColor(string input)
        {
            // Define a regex pattern to match the input format
            string pattern = @"\{r:\s*([\d.]+),\s*g:\s*([\d.]+),\s*b:\s*([\d.]+),\s*a:\s*([\d.]+)\}";

            Match match = Regex.Match(input, pattern);

            if (match.Success)
            {
                float r = ParseFloat(match.Groups[1].Value);
                float g = ParseFloat(match.Groups[2].Value);
                float b = ParseFloat(match.Groups[3].Value);
                float a = ParseFloat(match.Groups[4].Value);

                return new Color(r, g, b, a);
            }
            return Color.white;
        }

        // Parse float from .mat file.
        public static float ParseFloat(string input)
        {
            // Define both dot and comma as possible decimal separators
            char[] possibleSeparators = { '.', ',' };

            // Try parsing with InvariantCulture first
            if (float.TryParse(input, NumberStyles.Float, CultureInfo.InvariantCulture, out float result))
            {
                return result;
            }

            // If parsing with InvariantCulture fails, try with other possible separators
            foreach (var separator in possibleSeparators)
            {
                if (separator != CultureInfo.InvariantCulture.NumberFormat.CurrencyDecimalSeparator[0])
                {
                    // Skip the separator that matches the current culture
                    if (float.TryParse(input.Replace(separator, CultureInfo.InvariantCulture.NumberFormat.CurrencyDecimalSeparator[0]), out result))
                    {
                        return result;
                    }
                }
            }

            return 0;
        }

        // Get the type of a texture.
        public static TextureType GetTextureType(this SRPMC_Pipeline pipeline, string name)
        {
            foreach (var kvp in pipelinesMatPropNames[pipeline])
                if (kvp.Value.Contains(name))
                    return kvp.Key;

            foreach (var kvp in customMatPropNames)
                if (kvp.Value.Contains(name))
                    return kvp.Key;

            return TextureType.Unknown;
        }

        // Checks the current pipeline for the target pipeline.
        public static SRPMC_Pipeline GetCurrentPipeline()
        {
            if (GraphicsSettings.currentRenderPipeline)
            {
                if (GraphicsSettings.currentRenderPipeline.GetType().ToString().Contains("HighDefinition"))
                    return SRPMC_Pipeline.HDRP;
                else
                    return SRPMC_Pipeline.URP;
            }
            else
                return SRPMC_Pipeline.Builtin;
        }

        // Default shaders.
        public static Dictionary<SRPMC_Pipeline, string> pipelinesDefaultShader = new Dictionary<SRPMC_Pipeline, string>()
        {
            { SRPMC_Pipeline.Builtin, "Standard" },
            { SRPMC_Pipeline.URP, "Universal Render Pipeline/Lit" },
            { SRPMC_Pipeline.HDRP, "HDRP/Lit" }
        };

        // Tiny Support for custom (non-unity) shaders.
        public static Dictionary<TextureType, List<string>> customMatPropNames = new Dictionary<TextureType, List<string>>()
        {
            { TextureType.Albedo, new List<string>(){ "MainTex", "_BaseColorMap", "_MainTexture" } },
            { TextureType.Metallic, new List<string>(){  } },
            { TextureType.NormalMap, new List<string>(){  } },
            { TextureType.HeightMap, new List<string>(){  } },
            { TextureType.Occlusion, new List<string>(){  } }
        };

        // List of props names in .mat files.
        public static Dictionary<SRPMC_Pipeline, Dictionary<TextureType, List<string>>> pipelinesMatPropNames = new Dictionary<SRPMC_Pipeline, Dictionary<TextureType, List<string>>>()
        {
            // Built-in Render Pipeline :
            {
                SRPMC_Pipeline.Builtin,
                new Dictionary<TextureType, List<string>>()
                {
                    { TextureType.Albedo, new List<string>(){ "_MainTex" } },
                    { TextureType.Metallic, new List<string>(){ "_MetallicGlossMap" } },
                    { TextureType.NormalMap, new List<string>(){ "_BumpMap" } },
                    { TextureType.HeightMap, new List<string>(){ "_ParallaxMap" } },
                    { TextureType.Occlusion, new List<string>(){ "_OcclusionMap" } },
                    { TextureType.Emission, new List<string>(){ "_EmissionMap" } },
                    // Only used by HDRP
                    { TextureType.MaskMap, new List<string>(){ "" } },
                    { TextureType.DetailMap, new List<string>(){ "" } }

                }
            },
            // URP :
            {
                SRPMC_Pipeline.URP,
                new Dictionary<TextureType,  List<string>>()
                {
                    { TextureType.Albedo, new List<string>(){ "_MainTex", "_BaseMap" } },
                    { TextureType.Metallic, new List<string>(){ "_MetallicGlossMap" } },
                    { TextureType.NormalMap, new List<string>(){ "_BumpMap", "_NormalMap" } },
                    { TextureType.HeightMap, new List<string>(){ "_ParallaxMap" } },
                    { TextureType.Occlusion, new List<string>(){ "_OcclusionMap" } },
                    { TextureType.Emission, new List<string>(){ "_EmissionMap" } },
                    // Only used by HDRP
                    { TextureType.MaskMap, new List<string>(){ "" } },
                    { TextureType.DetailMap, new List<string>(){ "" } }
                }
            },
            // HDRP :
            {
                // TODO :
                // Metallic, Occlusion & Smoothness for HDRP needs a custom texture converter : 
                // https://docs.unity3d.com/Packages/com.unity.render-pipelines.high-definition@12.1/manual/Mask-Map-and-Detail-Map.html
                SRPMC_Pipeline.HDRP,
                new Dictionary<TextureType,  List<string>>()
                {
                    { TextureType.Albedo, new List<string>(){ "_MainTex", "_BaseColorMap" } },
                    { TextureType.Metallic, new List<string>() },
                    { TextureType.NormalMap, new List<string>(){ "_BumpMap", "_NormalMap" } },
                    { TextureType.HeightMap, new List<string>(){ "_HeightMap" } },
                    { TextureType.Occlusion, new List<string>() },
                    { TextureType.Emission, new List<string>(){ "_EmissiveColorMap" } },
                    { TextureType.MaskMap, new List<string>(){ "_MaskMap" } },
                    { TextureType.DetailMap, new List<string>(){ "" } },
                }
            },
        };

        // List of primary color variable names
        public static List<string> primaryColorNames = new List<string>()
        {
            "_BaseColor",
            "_Color"
        };

        // List of words albedo textures usually contains.
        public static List<string> albedoNamesContains = new List<string>()
        {
            "albedo",
            "color",
            "base"
        };

        // List of strings that albedo textures usually ends with.
        public static List<string> albedoNamesEndsWith = new List<string>()
        {
            "_bc", // Base Color
            "_al", // Albedo
            "_c", // Color
            "_a" // Albedo
        };

        // Class for a parsed material after reading the .mat file.
        public class ParsedMaterial
        {
            public bool gpuInstancing = false;
            public bool alphaClipping = false;
            public float alphaClippingValue = 0f;
            public bool transparent = false;
            public bool emissive = false;
            public RenderFace renderFace = RenderFace.Front;
            public List<string> m_ValidKeywords = new List<string>();
            public Dictionary<string, ParsedTexture> m_TexEnvs = new Dictionary<string, ParsedTexture>();
            public Dictionary<string, int> m_Ints = new Dictionary<string, int>();
            public Dictionary<string, float> m_Floats = new Dictionary<string, float>();
            public Dictionary<string, Color> m_Colors = new Dictionary<string, Color>();
        }

        // Parsed texture which contains the texture values.
        public class ParsedTexture
        {
            public string guid;
            public Texture texture;
            public Vector2 scale;
            public Vector2 offset;
        }

        // Rendering/Cull face enum.
        public enum RenderFace
        {
            Front,
            Back,
            Both
        }

        // TextureType enum.
        public enum TextureType
        {
            Albedo,
            Metallic,
            NormalMap,
            HeightMap,
            Occlusion,
            Emission,
            MaskMap,
            DetailMap,
            Unknown
        }

        // Pipeline enum for the UI dropdown.
        public enum SRPMC_Pipeline
        {
            Builtin,
            URP,
            HDRP
        }
    }
}
#endif
