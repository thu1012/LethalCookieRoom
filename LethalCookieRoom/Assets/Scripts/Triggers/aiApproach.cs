using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class aiApproach : MonoBehaviour
{
    [SerializeField] GameObject target;
    NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null){
            Debug.Log("Missing Navmesh Component");
        }
        agent.destination = target.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        agent.destination = target.transform.position;
    }
}
