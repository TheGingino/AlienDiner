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
        
        // Ignore Station Clicks
        if (Physics.Raycast(ray, out RaycastHit stationHit))
        {
            if (stationHit.collider.GetComponent<InteractableObject>() != null)
            {
                return; // station handles its own click
            }
        }
        
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

    public void MoveToInteraction(InteractableObject station)
    {
        if (_agent == null) return;

        _agent.SetDestination(station.InteractionWaypoint.position);

        StartCoroutine(CheckArrival(station));
    }

    IEnumerator CheckArrival(InteractableObject station)
    {
        while (_agent.pathPending)
            yield return null;
        
        while (_agent.remainingDistance > _agent.stoppingDistance)
            yield return null;
        
        while (_agent.velocity.sqrMagnitude > 0.01f)
            yield return null;
        
        _agent.Warp(station.InteractionWaypoint.position);
        _agent.isStopped = true;
        GetComponent<PlayerInteraction>().StartInteraction(station);
    }
    
    public void LockPlayerMovement(bool enabled)
    {
        if (_agent == null) return;

        _agent.isStopped = !enabled;

    }
}
