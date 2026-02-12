using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

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
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                MoveToPosision(touch.position);
            }
            
        }
        if (Input.GetMouseButtonDown(0)) // Pc/ editor testing
        {
            MoveToPosision(Input.mousePosition);
        }
    }

    private void MoveToPosision(Vector3 screenPosistion)
    {
        Ray _ray = Camera.main.ScreenPointToRay(screenPosistion);
        if (Physics.Raycast(_ray, out RaycastHit hit, groundLayer))
        {
            if (NavMesh.SamplePosition(hit.point, out NavMeshHit navMeshHit, 100f, NavMesh.AllAreas))
            {
                _agent.SetDestination(navMeshHit.position);
            }
        }
    }
}
