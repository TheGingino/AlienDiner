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

    [SerializeField] private Animator _animator;

    [SerializeField] private AudioSource walkingSFX;
    

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();
        _camera = Camera.main;
    }

    private void Update()
    {   
        bool isMoving = _agent.velocity.sqrMagnitude > 0.01f;
        _animator.SetBool("Walk", isMoving);

        if (isMoving)
        {
            if (!walkingSFX.isPlaying)
            {
                walkingSFX.loop = true;
                walkingSFX.Play();
            }
        }
        else
        {
            if (walkingSFX.isPlaying)
            {
                walkingSFX.Stop();
            }
        }

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

    void MoveToTarget(Transform target, Action OnArrive)
    {
        if (_agent == null) return;



        _agent.isStopped = false; 
        StopAllCoroutines();   

        _agent.SetDestination(target.position);

        StartCoroutine(CheckArrival(target, OnArrive));
    }

    IEnumerator CheckArrival(Transform target, Action OnArrive)
    {
        while (_agent.pathPending)
            yield return null;

        while (_agent.remainingDistance > _agent.stoppingDistance)
            yield return null;

        while (_agent.velocity.sqrMagnitude > 0.01f)
            yield return null;

        _agent.Warp(target.position);
        _agent.isStopped = true;

        OnArrive?.Invoke();
    }
    
    public void LockPlayerMovement(bool enabled)
    {
        if (_agent == null) return;

        _agent.isStopped = !enabled;

    }
    
    public void MoveToInteraction(InteractableObject station)
    {
        MoveToTarget(station.InteractionWaypoint, () =>
        {
            GetComponent<PlayerInteraction>().StartInteraction(station);
        });
    }
    
    public void MoveToTable(Transform servepoint, Transform tableTransform)
    {
        MoveToTarget(servepoint, () =>
        {
            GetComponent<PlayerInteraction>().TryServeCustomersAtTable(tableTransform);
        });
    }
}
