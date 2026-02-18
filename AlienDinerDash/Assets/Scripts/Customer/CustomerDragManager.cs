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

        if (Physics.Raycast(ray, out RaycastHit hit, 50f, customerLayer))
        {
            _draggedCustomer = hit.collider.GetComponent<CustomerSeating>(); // still working o na good script name 
        }
    }

    private void Drag(Vector3 screenpos)
    {
        Ray ray = _camera.ScreenPointToRay(screenpos);
        
        if (Physics.Raycast(ray, out RaycastHit hit, 50f, groundLayer))
        {
            _draggedCustomer.SetDraggedPosition(hit.point);  // SetDraggedPosistion need to be made in customerseating
        }
    }

    private void EndDrag(Vector3 screenpos)
    {
        Ray ray = _camera.ScreenPointToRay(screenpos);

        if (Physics.Raycast(ray, out RaycastHit hit, 50f, tableLayer))
        {
            Table table = hit.collider.GetComponent<Table>();

            if (table != null && table.FreeSeat()) // bool still needs to be added in table script 
            {
                table.TrySeatCustomer(_draggedCustomer);
            }
        }

        _draggedCustomer = null;
    }
}
