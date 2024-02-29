using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;



public class objectShot : MonoBehaviour
{
    public enum Effect {nothing, destroyObject, shoveObject, toggleAnimation, startTimeline, triggerParticles, playAudio, toChair};
    [SerializeField] private Effect effect = Effect.nothing;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Activate(GameObject player, Ray ray, RaycastHit hit){

        Debug.Log("test0");
        //Destroy Effect
        if (effect == Effect.destroyObject){
         Destroy(gameObject);
        }
        //Physics Effect
        else if (effect == Effect.shoveObject){
            Rigidbody rb = hit.transform.gameObject.GetComponent<Rigidbody>();
            if(rb == null){
                rb = hit.transform.gameObject.AddComponent<Rigidbody>();
            }
            rb.AddForce(ray.direction*200);

        }
        //Animation Effect
        else if(effect == Effect.toggleAnimation){
            Animator anim = hit.transform.gameObject.GetComponent<Animator>();
            if (anim != null){
                if (anim.enabled){
                    anim.enabled = false;
                }
                else{
                    anim.enabled = true;
                }
            }
        }
        //Timeline Effect
        else if (effect == Effect.startTimeline){
            PlayableDirector pd = hit.transform.gameObject.GetComponent<PlayableDirector>();
            if (pd != null){
                if(pd.state != PlayState.Playing){
                    pd.Play();
                }
            }
        }
        //Audio Effect
        else if (effect == Effect.playAudio){
            AudioSource audi = hit.transform.gameObject.GetComponent<AudioSource>();
            if(audi != null){
                if(!audi.isPlaying){
                 audi.Play();
                }
            }
        }
        else if (effect == Effect.triggerParticles){
            ParticleSystem ps = hit.transform.gameObject.GetComponent<ParticleSystem>();
            if (ps != null){
                ps.Play();

            }
        }
        else if (effect == Effect.toChair)
        {
            Debug.Log("test");
            CharacterController cc = player.GetComponent<CharacterController>();
            if (cc != null)
            {
                Debug.Log("test1");
                cc.enabled = false;
                player.transform.position = new Vector3((float)-0.4639655, (float)0.1, 6);
            }
        }
    }
}
