#if UNITY_EDITOR
namespace HyTeKGames.SRPMaterialConverter
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using UnityEditor;
    using UnityEditor.UIElements;
    using UnityEngine;
    using UnityEngine.UIElements;
    using static SRPMC_Utilities;

    public class SRPMC_Editor : EditorWindow
    {
        // List of materials to be converted.
        public List<Material> materials = new List<Material>();

        // Values
        public static SRPMC_Pipeline originalPipeline = SRPMC_Pipeline.HDRP;
        public static SRPMC_Pipeline targetPipeline = SRPMC_Pipeline.URP;
        private static string AssetPath = "Assets/HyTeKGames/SRPMaterialConverter/";
        public List<string> pipelines = new List<string>()
        {
            "Builtin",
            "URP",
            "HDRP"
        };

        // UI Fields
        private SerializedObject serializedObject;
        private DropdownField originalPipelineDropdown;
        private DropdownField targetPipelineDropdown;
        private Label arrowLabel;
        private Label errorLabel;
        private Toggle overrideOriginalMaterials;
        private Button convertBtn;
        private ProgressBar progressBar;


        [MenuItem("Tools/HyTeKGames/SRP Material Converter")]
        static void OpenEditorWindow()
        {
            SRPMC_Editor wnd = GetWindow<SRPMC_Editor>();
            wnd.titleContent = new GUIContent("SRP Material Converter");
            wnd.maxSize = new Vector2(350, 500);
            wnd.minSize = wnd.maxSize;
        }

        // Creating the main window.
        void CreateGUI()
        {
            VisualElement root = rootVisualElement;
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(Path.Combine(AssetPath, "Resources/UI Documents/SRPMC_Window.uxml"));

            VisualElement tree = visualTree.Instantiate();
            root.Add(tree);

            if (serializedObject == null)
                serializedObject = new SerializedObject(this);

            SerializedProperty serialProp = serializedObject.FindProperty("materials");

            root.Q<PropertyField>("materials").BindProperty(serialProp);
            root.Q<Label>("pipeline").text = "Your project is configured to use " + (GetCurrentPipeline() == SRPMC_Pipeline.Builtin ? "the Built-in render pipeline" : GetCurrentPipeline().ToString()) + ".";
            Button findBtn = root.Q<Button>("find-btn");
            findBtn.clicked += () => FindBrokenMaterials();
            progressBar = root.Q<ProgressBar>("progress");
            progressBar.style.display = DisplayStyle.None;
            arrowLabel = root.Q<Label>("result-label");
            errorLabel = root.Q<Label>("error-label");
            convertBtn = root.Q<Button>("convert-btn");
            convertBtn.clicked += () => StartConvert();
            overrideOriginalMaterials = root.Q<Toggle>("override");

            originalPipelineDropdown = root.Q<DropdownField>("new-original-pipeline");
            originalPipelineDropdown.choices = pipelines;
            originalPipelineDropdown.RegisterValueChangedCallback(OnOriginalPipelineChanged);
            originalPipelineDropdown.value = originalPipeline.ToString();

            targetPipelineDropdown = root.Q<DropdownField>("new-target-pipeline");
            targetPipelineDropdown.choices = pipelines;
            targetPipelineDropdown.RegisterValueChangedCallback(OnTargetPipelineChanged);
            targetPipelineDropdown.value = targetPipeline.ToString();

            SetTargetPipeline(GetCurrentPipeline());
            SetOriginalPipeline(Enum.GetValues(typeof(SRPMC_Pipeline)).Cast<SRPMC_Pipeline>().Where(x => x != targetPipeline).LastOrDefault());

            /*
            -> Old enum (not supported on LTS 2021 so switched to dropdown)
            originalPipelineEnum = root.Q<EnumField>("original-pipeline");
            originalPipelineEnum.RegisterValueChangedCallback(OnOriginalPipelineChanged);
            originalPipelineEnum.value = originalPipeline;
            targetPipelineEnum = root.Q<EnumField>("target-pipeline");
            targetPipelineEnum.RegisterValueChangedCallback(OnTargetPipelineChanged);
            targetPipelineEnum.value = targetPipeline;*/

            root.Q<Button>("rate-btn").clicked += () => { Application.OpenURL("https://assetstore.unity.com/packages/tools/utilities/srp-material-converter-hdrp-to-urp-hdrp-to-built-in-more-243595#reviews"); };
            UpdatePipelineLabels();
        }

        public void SetTargetPipeline(SRPMC_Pipeline val)
        {
            targetPipeline = val;
            targetPipelineDropdown.value = val.ToString();
        }

        public void SetOriginalPipeline(SRPMC_Pipeline val)
        {
            originalPipeline = val;
            originalPipelineDropdown.value = val.ToString();
        }

        // Starts the conversion.
        async void StartConvert()
        {
            AssetDatabase.SaveAssets();
            if (!Directory.Exists(Path.Combine(AssetPath, "ConvertedMaterials")))
                Directory.CreateDirectory(Path.Combine(AssetPath, "ConvertedMaterials"));

            progressBar.title = $"0/{materials.Count} materials converted (0%)";
            progressBar.value = 0;
            progressBar.style.display = DisplayStyle.Flex;
            await Task.Delay(5);
            int max = materials.Count;
            int current = 0;

            foreach (Material mat in materials.Where(x => x != null))
            {
                /*try
                {*/
                    ParsedMaterial parsedMat = ParseMaterial(mat);
                    if (parsedMat != null)
                    {
                        Shader shader = Shader.Find(pipelinesDefaultShader[targetPipeline]);
                        if (shader)
                        {
                            Material converted = null;

                            if (!overrideOriginalMaterials.value)
                            {
                                converted = new Material(shader);
                                AssetDatabase.CreateAsset(converted, Path.Combine(AssetPath, "ConvertedMaterials/" + mat.name + ".mat"));
                                Material _mat = AssetDatabase.LoadAssetAtPath<Material>(Path.Combine(AssetPath, "ConvertedMaterials/" + mat.name + ".mat"));
                                PostProcessMaterial(_mat, parsedMat);
                            }
                            else
                            {
                                mat.shader = shader;
                                // Some values are kept so we reset them. Sadly unity doesn't offer a .Reset() on Material like in the inspector.
                                foreach (var word in mat.enabledKeywords)
                                    mat.DisableKeyword(word);
                                switch (targetPipeline)
                                {
                                    case SRPMC_Pipeline.URP:
                                    case SRPMC_Pipeline.Builtin:
                                        mat.globalIlluminationFlags = (MaterialGlobalIlluminationFlags)4;
                                        break;
                                }
                                mat.DisableKeyword("_EMISSION");

                                AssetDatabase.SaveAssets();
                                PostProcessMaterial(mat, parsedMat);
                            }
                        }
                    }
                /*}
                catch (Exception ex)
                {
                    Debug.LogError("[SRPMaterialConverter] An error occured while converting material \"" + mat.name + "\"! Error: " + ex.ToString());
                }*/
                current++;
                int progress = (int)((float)current / max * 100);
                progressBar.title = $"{current}/{materials.Count} materials converted ({progress}%)";
                progressBar.value = progress;
                await Task.Delay(1);
            }
            progressBar.style.display = DisplayStyle.None;
            AssetDatabase.Refresh();
            EditorUtility.DisplayDialog("Materials Successfully Converted!", "Your " + materials.Count + " materials have been successfully converted to " + (targetPipeline == SRPMC_Pipeline.Builtin ? "the Built-in Render Pipeline" : targetPipeline.ToString()) + ".", "Ok");

        }

        // Post process the materials (conversion)
        void PostProcessMaterial(Material converted, ParsedMaterial parsedMat)
        {
            if (!Directory.Exists(Path.Combine(AssetPath, "ConvertedMaterials/Maps")))
                Directory.CreateDirectory(Path.Combine(AssetPath, "ConvertedMaterials/Maps"));
            // Transparency & Alpha Clipping
            if (parsedMat.alphaClipping)
            {
                switch (targetPipeline)
                {
                    case SRPMC_Pipeline.HDRP:
                        converted.SetFloat("_AlphaCutoffEnable", 1);
                        converted.SetFloat("_AlphaCutoff", parsedMat.alphaClippingValue);
                        break;
                    case SRPMC_Pipeline.URP:
                        converted.SetFloat("_AlphaClip", 1);
                        converted.SetFloat("_Cutoff", parsedMat.alphaClippingValue);
                        break;
                    default:
                        converted.SetFloat("_Mode", 1);
                        converted.SetFloat("_Cutoff", parsedMat.alphaClippingValue);
                        break;
                }
            }

            if (parsedMat.transparent)
            {
                switch (targetPipeline)
                {
                    case SRPMC_Pipeline.HDRP:
                        converted.SetFloat("_SurfaceType", 1);
                        break;
                    case SRPMC_Pipeline.URP:
                        converted.SetFloat("_Surface", 1);
                        break;
                    default:
                        converted.SetFloat("_Mode", 3);
                        break;
                }
            }

            Texture2D metallicMap = null;
            Texture2D occlusionMap = null;

            // Textures
            bool extractedMaskMap = false;
            bool foundAlbedo = false;
            bool foundNormal = false;
            foreach (var kvp in parsedMat.m_TexEnvs)
            {
                if (kvp.Value.texture != null)
                {
                    if (!kvp.Value.texture.IsTexture2D()) continue;
                    TextureType type = originalPipeline.GetTextureType(kvp.Key);
                    if (type == TextureType.Unknown)
                        continue;
                    if (type == TextureType.Metallic)
                        metallicMap = kvp.Value.texture as Texture2D;
                    if (type == TextureType.Occlusion)
                        occlusionMap = kvp.Value.texture as Texture2D;
                    if (type == TextureType.MaskMap && !extractedMaskMap)
                    {
                        extractedMaskMap = true;
                        List<byte[]> maps = kvp.Value.texture.ExtractChannels();


                        string mapsPath = Application.dataPath + "/HyTeKGames/SRPMaterialConverter/ConvertedMaterials/Maps";
                        if (!Directory.Exists(mapsPath))
                            Directory.CreateDirectory(mapsPath);
                        for (int i = 0; i < maps.Count; i++)
                        {
                            foreach (var kvp2 in pipelinesMatPropNames[targetPipeline])
                            {
                                if ((kvp2.Key == TextureType.Metallic && i == 0) || (kvp2.Key == TextureType.Occlusion && i == 1))
                                {
                                    string texturePath = "";
                                    Texture texture = null;
                                    foreach (string texName in kvp2.Value)
                                    {
                                        if (converted.HasTexture(texName))
                                        {
                                            if (texturePath == "")
                                            {
                                                texturePath = mapsPath + $"/{converted.name}_" + channelsNames[i] + ".png";
                                                File.WriteAllBytes(texturePath, maps[i]);
                                                AssetDatabase.Refresh();
                                                texture = AssetDatabase.LoadAssetAtPath<Texture>(texturePath.Replace(Application.dataPath, "Assets"));
                                            }
                                            if (texture != null)
                                                converted.SetTexture(texName, texture);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (pipelinesMatPropNames.ContainsKey(targetPipeline) && pipelinesMatPropNames[targetPipeline].ContainsKey(type))
                        {
                            foreach (string texName in pipelinesMatPropNames[targetPipeline][type])
                            {
                                if (converted.HasTexture(texName))
                                {
                                    if (type == TextureType.Albedo)
                                        foundAlbedo = true;
                                    if (type == TextureType.NormalMap)
                                        foundNormal = true;

                                    converted.SetTexture(texName, kvp.Value.texture);
                                    converted.SetTextureScale(texName, kvp.Value.scale);
                                    converted.SetTextureOffset(texName, kvp.Value.offset);
                                }
                            }
                        }
                        if (customMatPropNames.ContainsKey(type))
                        {
                            foreach (string texName in customMatPropNames[type])
                            {
                                if (converted.HasTexture(texName))
                                {
                                    converted.SetTexture(texName, kvp.Value.texture);
                                    converted.SetTextureScale(texName, kvp.Value.scale);
                                    converted.SetTextureOffset(texName, kvp.Value.offset);
                                }
                            }
                        }
                    }
                }
            }

            Texture2D convertedMaskMap = null;
            // If we are converting to HDRP and we have atleast 1 map.
            if (targetPipeline == SRPMC_Pipeline.HDRP && (metallicMap || occlusionMap))
            {
                int width = 0;
                int height = 0;
                // If both maps are present only process them if they are both the same size.
                if ((metallicMap == null || occlusionMap == null) || (metallicMap != null && occlusionMap != null && metallicMap.width + metallicMap.height == occlusionMap.width + occlusionMap.height))
                {
                    if (metallicMap)
                    {
                        width = metallicMap.width;
                        height = metallicMap.height;
                    }
                    else
                    {
                        width = occlusionMap.width;
                        height = occlusionMap.height;
                    }

                    if (metallicMap)
                        metallicMap = ConvertToChannel(RescaleTexture(metallicMap, width, height), 0);
                    if (occlusionMap)
                        occlusionMap = ConvertToChannel(RescaleTexture(occlusionMap, width, height), 1);

                    Texture2D _maskMap = new Texture2D(width, height);
                    // Merge textures into maskMap
                    for (int y = 0; y < width; y++)
                    {
                        for (int x = 0; x < height; x++)
                        {
                            Color mColor = metallicMap ? metallicMap.GetPixel(x, y) : Color.black;
                            Color aoColor = occlusionMap ? occlusionMap.GetPixel(x, y) : Color.black;

                            // Set red channel from metallic and green channel from ambient occlusion
                            _maskMap.SetPixel(x, y, new Color(mColor.r, aoColor.g, 0f, 1f));
                        }
                    }
                    _maskMap.Apply();

                    string mapsPath = Application.dataPath + "/HyTeKGames/SRPMaterialConverter/ConvertedMaterials/Maps";
                    string texturePath = mapsPath + $"/{converted.name}_ConvertedMaskMap.png";
                    File.WriteAllBytes(texturePath, _maskMap.EncodeToPNG());
                    AssetDatabase.Refresh();
                    convertedMaskMap = AssetDatabase.LoadAssetAtPath<Texture2D>(texturePath.Replace(Application.dataPath, "Assets"));
                }
            }

            if (convertedMaskMap)
            {
                foreach (string texName in pipelinesMatPropNames[targetPipeline][TextureType.MaskMap])
                {
                    if (converted.HasTexture(texName))
                        converted.SetTexture(texName, convertedMaskMap);
                }
            }

            // If no albedo were found, try to find it through all textures.
            if (!foundAlbedo)
            {
                Dictionary<ParsedTexture, int> score = new Dictionary<ParsedTexture, int>();
                foreach (var kvp in parsedMat.m_TexEnvs)
                {
                    if (kvp.Value.texture != null)
                    {
                        TextureImporter importer = ((TextureImporter)AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(kvp.Value.texture)));
                        if (!importer) continue;
                        TextureImporterType type = importer.textureType;
                        TextureImporterShape shape = importer.textureShape;

                        if (type != TextureImporterType.NormalMap && kvp.Value.texture.IsTexture2D())
                        {
                            if (kvp.Value.texture.name.ToLower().Contains("bent")) continue;
                            if (!score.ContainsKey(kvp.Value))
                                score.Add(kvp.Value, 0);
                            string texName = kvp.Value.texture.name.ToLower();
                            foreach (string s in albedoNamesContains)
                            {
                                if (texName.Contains(s))
                                    score[kvp.Value] += 2;
                            }
                            foreach (string s in albedoNamesEndsWith)
                            {
                                if (texName.EndsWith(s))
                                    score[kvp.Value] += 3;
                            }
                        }
                    }
                }
                ParsedTexture firstTex = parsedMat.m_TexEnvs.Values.Where(x => x.texture != null && x.texture.IsTexture2D()).FirstOrDefault();
                if (firstTex != null)
                {
                    try
                    {
                        if (!score.ContainsKey(firstTex))
                            score.Add(firstTex, 0);
                        else
                            score[firstTex] += 1;
                    }
                    catch
                    { // No output since its not needed.
                    }
                }

                var texWithHighestScore = score.OrderByDescending(x => x.Value).FirstOrDefault();
                if (texWithHighestScore.Key != null)
                {
                    foreach (string texName in pipelinesMatPropNames[targetPipeline][TextureType.Albedo])
                    {
                        if (converted.HasTexture(texName))
                        {
                            converted.SetTexture(texName, texWithHighestScore.Key.texture);
                            converted.SetTextureScale(texName, texWithHighestScore.Key.scale);
                            converted.SetTextureOffset(texName, texWithHighestScore.Key.offset);
                        }
                    }
                }
            }

            // If no normals were found, try to find it through all textures.
            if (!foundNormal)
            {
                foreach (var kvp in parsedMat.m_TexEnvs)
                {
                    if (kvp.Value.texture != null)
                    {
                        TextureImporter importer = ((TextureImporter)AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(kvp.Value.texture)));
                        if (!importer) continue;
                        if (importer.textureType != TextureImporterType.NormalMap)
                            continue;
                        foreach (string texName in pipelinesMatPropNames[targetPipeline][TextureType.NormalMap])
                        {
                            if (converted.HasTexture(texName))
                            {
                                converted.SetTexture(texName, kvp.Value.texture);
                                converted.SetTextureScale(texName, kvp.Value.scale);
                                converted.SetTextureOffset(texName, kvp.Value.offset);
                            }
                        }
                    }
                }
            }

            // Colors (Primary variable names doesn't change much accross pipelines)
            foreach (var kvp in parsedMat.m_Colors)
            {
                if (converted.HasColor(kvp.Key))
                    converted.SetColor(kvp.Key, kvp.Value);
                if (primaryColorNames.Contains(kvp.Key))
                {
                    foreach (string name in primaryColorNames)
                    {
                        if (converted.HasColor(name))
                            converted.SetColor(name, kvp.Value);
                    }
                }
            }

            // Emission
            if (parsedMat.emissive)
            {
                bool colorFound = parsedMat.m_Colors.ContainsKey("_EmissiveColor") || parsedMat.m_Colors.ContainsKey("_EmissionColor");
                Color color = parsedMat.m_Colors.ContainsKey("_EmissiveColor") ? parsedMat.m_Colors["_EmissiveColor"] : (parsedMat.m_Colors.ContainsKey("_EmissionColor") ? parsedMat.m_Colors["_EmissionColor"] : Color.white);
                if (targetPipeline == SRPMC_Pipeline.HDRP)
                {
                    converted.SetFloat("_UseEmissiveIntensity", 1);
                    if (colorFound)
                    {
                        converted.SetColor("_EmissiveColorLDR", color);
                        converted.SetColor("_EmissiveColor", color);
                    }
                }
                else
                {
                    converted.EnableKeyword("_EMISSION");
                    if (colorFound)
                        converted.SetColor("_EmissionColor", color);
                }
                converted.globalIlluminationFlags = (MaterialGlobalIlluminationFlags)6;
            }

            // Render Face
            float val = parsedMat.renderFace == RenderFace.Front ? 2 : (parsedMat.renderFace == RenderFace.Back ? 1 : 0);
            switch (targetPipeline)
            {
                case SRPMC_Pipeline.HDRP:
                    converted.SetFloat("_OpaqueCullMode", val);
                    converted.SetFloat("_DoubleSidedEnable", parsedMat.renderFace == RenderFace.Both ? 1 : 0);
                    break;
                case SRPMC_Pipeline.URP:
                    converted.SetFloat("_Cull", val);
                    break;
            }

            // Metallic & Smoothness
            float metallic = parsedMat.m_Floats.ContainsKey("_Metallic") ? parsedMat.m_Floats["_Metallic"] : 0;
            float smoothness = originalPipeline == SRPMC_Pipeline.Builtin ? (parsedMat.m_Floats.ContainsKey("_Glossiness") ? parsedMat.m_Floats["_Glossiness"] : 0.5f) : (parsedMat.m_Floats.ContainsKey("_Smoothness") ? parsedMat.m_Floats["_Smoothness"] : 0.5f);
            converted.SetFloat("_Metallic", metallic);
            if (targetPipeline == SRPMC_Pipeline.Builtin)
                converted.SetFloat("_Glossiness", smoothness);
            else
                converted.SetFloat("_Smoothness", smoothness);
        }

        // Triggered when the user changes original pipeline in the editor window.
        void OnOriginalPipelineChanged(ChangeEvent<string> val)
        {
            Enum.TryParse(val.newValue, true, out originalPipeline);
            UpdatePipelineLabels();
        }

        // Triggered when the user changes target pipeline in the editor window.
        void OnTargetPipelineChanged(ChangeEvent<string> val)
        {
            Enum.TryParse(val.newValue, true, out targetPipeline);
            UpdatePipelineLabels();
        }

        // Updates the labels.
        void UpdatePipelineLabels()
        {
            arrowLabel.text = originalPipeline.ToString() + " ➝ " + targetPipeline.ToString();
            convertBtn.text = $"Convert from {originalPipeline} to {targetPipeline} !";
        }

        // Checks for mistakes and display users warnings.
        void OnGUI()
        {
            if (convertBtn == null) return;
            bool materialsValid = materials.Count > 0 && !materials.Any(x => x == null);
            bool pipelinesValid = originalPipeline != targetPipeline;
            convertBtn.SetEnabled(materialsValid && pipelinesValid);
            if (!materialsValid)
                errorLabel.text = "Select at least 1 material to start conversion.";
            if (!pipelinesValid)
                errorLabel.text = "Original and target pipelines must be different.";
            errorLabel.visible = !materialsValid || !pipelinesValid;
            errorLabel.style.display = errorLabel.visible ? DisplayStyle.Flex : DisplayStyle.None;
        }

        // Triggered when clicking the "Find broken materials" button.
        void FindBrokenMaterials()
        {
            materials.Clear();
            materials.AddRange(SRPMC_Utilities.FindBrokenMaterials());
            EditorUtility.DisplayDialog(materials.Count == 0 ? "No Broken Materials Found" : "Broken Materials Found",
                materials.Count == 0 ? "No materials found. If you have broken materials due to pipeline changes, drag & drop them into the materials field." : ("Found " + materials.Count + " broken materials. Please select the proper pipelines and click 'Convert' to proceed."), "Ok");
        }
    }
}
#endif