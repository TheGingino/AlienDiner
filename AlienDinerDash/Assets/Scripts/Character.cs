using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Character : MonoBehaviour
{
    private NavMeshAgent _agent;
    
    [SerializeField] private LayerMask groundLayer;
    

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // for now mouse for testing, switch the input to touch for android. (not sure yet if that works!!!)
        {
            // create ray from the Cam to the mouse pointer
            Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(_ray, out RaycastHit hit, groundLayer))
            {
                if (NavMesh.SamplePosition(hit.point, out NavMeshHit navMeshHit, 100f, NavMesh.AllAreas))
                {
                    _agent.SetDestination(navMeshHit.position);
                }
            }
        }
    }
}
