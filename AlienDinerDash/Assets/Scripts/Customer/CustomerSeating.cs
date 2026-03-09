using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CustomerSeating : MonoBehaviour
{
    private Transform _currentSeat;
    private Table _currentTable;

    private Vector3 _originPosition;
    private Quaternion _originRotation;

    private bool _canBeDragged = true;
    
    [SerializeField] private UnityEvent hasBeenSeated;

    private void Start()
    {
        _originPosition = transform.position;
        _originRotation = transform.rotation;
    }

    private void Update() // seating test to see if the seat is given free when customer leaves
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LeaveSeat();
            ReturnToOrigin();
            Debug.Log("seat freed!");
        }
    }

    public bool CanBeDragged()
    {
        return _canBeDragged;
    }

    public void SetDraggedPosition(Vector3 _pos)
    {
        transform.position = _pos;

        transform.position = new Vector3(_pos.x, _pos.y + 1.5f, _pos.z); // Drag height can be changed higher or lower for now this so it does not dragg trough the ground
    }
    public void SnapToSeat(Transform seat, Table table)
    {
        _currentSeat = seat;
        _currentTable = table;

        transform.position = seat.position;
        transform.rotation = seat.rotation;

        _canBeDragged = false;
        hasBeenSeated.Invoke();
    }

    public void ReturnToOrigin()
    {
        transform.position = _originPosition;
        transform.rotation = _originRotation;
    }

    public void LeaveSeat()
    {
        if (_currentTable != null && _currentSeat != null)
        {
            _currentTable.FreeSeat(_currentSeat); 
           
            _currentSeat = null;
            _currentTable = null;
                                                 
            _canBeDragged = false;
        }
    }
}
