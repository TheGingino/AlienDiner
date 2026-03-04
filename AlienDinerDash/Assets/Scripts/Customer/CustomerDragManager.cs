using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerDragManager : MonoBehaviour
{
    [SerializeField] private LayerMask customerLayer;
    [SerializeField] private LayerMask tableLayer;
    [SerializeField] private LayerMask groundLayer;

    private Camera _camera;
    private CustomerSeating _draggedCustomer;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        // PC input
        if (Input.GetMouseButtonDown(0))
            StartDrag(Input.mousePosition);

        if (Input.GetMouseButton(0) && _draggedCustomer != null)
            Drag(Input.mousePosition);

        if (Input.GetMouseButtonUp(0) && _draggedCustomer != null)
            EndDrag(Input.mousePosition);
        
        // Mobile input

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    StartDrag(touch.position);
                    break;
                
                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                    if(_draggedCustomer != null)
                        Drag(touch.position);
                    break;
                
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    if(_draggedCustomer != null)
                        EndDrag(touch.position);
                    break;
            }
        }
    }

    private void StartDrag(Vector3 screenpos)
    {
        Ray ray = _camera.ScreenPointToRay(screenpos);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, customerLayer)) // distance need to be tweaked for prefomance
        {
            CustomerSeating customer = hit.collider.GetComponentInParent<CustomerSeating>(); // still working o na good script name 

            if (customer != null && customer.CanBeDragged())
            {
                _draggedCustomer = customer;
            }
        }
    }

    private void Drag(Vector3 screenpos)
    {
        Ray ray = _camera.ScreenPointToRay(screenpos);
        
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundLayer)) // distance need to be tweaked for prefomance
        {
            _draggedCustomer.SetDraggedPosition(hit.point);  // SetDraggedPosistion need to be made in customerseating
        }
    }

    private void EndDrag(Vector3 screenpos)
    {
        Ray ray = _camera.ScreenPointToRay(screenpos);
        
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, tableLayer)) // distance need to be tweaked for prefomance
        {
            Table table = hit.collider.GetComponent<Table>();

            if (table != null && table.HasFreeSeat())
            {
                bool seated = table.TrySeatCustomer(_draggedCustomer , hit.point);
                
                if (!seated)
                {
                    _draggedCustomer.ReturnToOrigin();
                }
            }
            else
            {
                _draggedCustomer.ReturnToOrigin();
            }
        }
        else
        {
            _draggedCustomer.ReturnToOrigin();
        }
        _draggedCustomer = null;
    }
}
