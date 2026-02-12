using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    private NavMeshAgent _agent;
    
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private GameObject TapmarkerPrefab;   
    

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
        Ray ray = Camera.main.ScreenPointToRay(screenPosistion);
        if (Physics.Raycast(ray, out RaycastHit hit,100f, groundLayer)) // navMesh radius kleiner maken voor Mobile preformance (maxDistance 2f)?
        {
            if (NavMesh.SamplePosition(hit.point, out NavMeshHit navMeshHit, 100f, NavMesh.AllAreas))// navMesh radius kleiner maken voor Mobile preformance (maxDistance 2f)?
            {
                _agent.SetDestination(navMeshHit.position);
                
                //Spawn TapMarker
                if (TapmarkerPrefab != null)
                {
                    Instantiate(TapmarkerPrefab, navMeshHit.position + Vector3.up * 0.05f, Quaternion.identity);
                }
            }
        }
    }
}
