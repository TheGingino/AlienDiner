using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    private NavMeshAgent _agent;
    private Camera _camera;
    
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private TapMarkerPool _tapMarkerPool;
    

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _camera = Camera.main;
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
        Ray ray = _camera.ScreenPointToRay(screenPosistion);
        if (Physics.Raycast(ray, out RaycastHit hit,100f, groundLayer)) // navMesh radius can be made smaller when you know the size of the playing feeld (smaller is better optimalisation)
        {
            if (NavMesh.SamplePosition(hit.point, out NavMeshHit navMeshHit, 100f, NavMesh.AllAreas))//navMesh radius can be made smaller when you know the size of the playing feeld
            {
                _agent.SetDestination(navMeshHit.position);
                
                //Spawn TapMarker
                if (_tapMarkerPool != null)
                {
                    GameObject marker = _tapMarkerPool.GetMarker();
                    marker.transform.position = navMeshHit.position + Vector3.up * 0.01f;
                }
            }
        }
    }
}
