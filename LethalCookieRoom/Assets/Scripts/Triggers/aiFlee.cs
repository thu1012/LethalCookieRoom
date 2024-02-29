using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class aiFlee : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] float noticeDistance = 8f;
    [SerializeField] float fleeDistance = 3f;
    NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null){
            Debug.Log("Missing Navmesh Component");
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 runTo = transform.position + ((transform.position - target.transform.position) * fleeDistance);
        float distance = Vector3.Distance(transform.position, target.transform.position);
        if (distance < noticeDistance) agent.SetDestination(runTo);
    }
}

