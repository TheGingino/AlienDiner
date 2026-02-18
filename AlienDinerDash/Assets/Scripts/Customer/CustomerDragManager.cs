using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
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
        if (Input.GetMouseButtonDown(0))
            StartDrag(Input.mousePosition);

        if (Input.GetMouseButton(0) && _draggedCustomer != null)
            Drag(Input.mousePosition);

        if (Input.GetMouseButtonUp(0) && _draggedCustomer != null)
            EndDrag(Input.mousePosition);
    }

    private void StartDrag(Vector3 screenpos)
    {
        Ray ray = _camera.ScreenPointToRay(screenpos);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, customerLayer)) // distance need to be tweaked for prefomance
        {
            CustomerSeating customer = hit.collider.GetComponentInParent<CustomerSeating>(); // still working on a good script name
            
            if (customer != null && !customer.IsSeated) 
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

        bool seated = false;

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, tableLayer)) // distance need to be tweaked for performance
        {
            Table table = hit.collider.GetComponent<Table>(); // if the structeur of the table changes instead of the collider on the table model but as a child change it to child  

            if (table != null && table.HasFreeSeat()) // bool still needs to be added in table script 
            {
                table.TrySeatCustomer(_draggedCustomer);
                seated = true;
            }
        }

        if (!seated)
        {
            _draggedCustomer.ReturnToOrigin();
        }

        _draggedCustomer = null;
    }
}
