using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Character : MonoBehaviour
{
    private NavMeshAgent _agent;

    [Header("Movement Settings")] public float moveSpeed = 10f;

    [Header("input setting")] 
    [SerializeField] private float sampleDistance = 0.5f; //distance to point clicked
    [SerializeField] private LayerMask groundLayer;

    public static event System.Action<Vector3> OnGroundTouch;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();

        _agent.speed = moveSpeed;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // for now mouse for testing, switch the input to touch for android. (not sure yet if that works!!!)
        {
            // create ray from the Cam to the mouse pointer
            Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(_ray, out RaycastHit hit, groundLayer))
            {
                if (NavMesh.SamplePosition(hit.point, out NavMeshHit navMeshHit, sampleDistance, NavMesh.AllAreas))
                {
                    _agent.SetDestination(navMeshHit.position);
                                    
                    OnGroundTouch?.Invoke(navMeshHit.position);
                }
                else
                    Debug.Log("clicked point is not an walkable area.");
            }
        }
    }
}
