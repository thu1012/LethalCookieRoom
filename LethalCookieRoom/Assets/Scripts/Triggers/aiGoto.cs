using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class aiGoto : MonoBehaviour
{  
    Camera cam;
    NavMeshAgent agent;
    private RaycastHit[] Hits = new RaycastHit[1];
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null){
            Debug.Log("Missing Navmesh Component");
        }
        cam = Camera.main;
        if (cam == null){
            Debug.Log("No main camera detected.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)){
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.RaycastNonAlloc(ray, Hits) > 0){
                agent.SetDestination(Hits[0].point);
            }
        }
        
    }
}
