using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class CustomerSeating : MonoBehaviour
{
    private Transform _currentSeat;
    private Table _currentTable;

    private Vector3 _originPosition;
    private Quaternion _originRotation;

    private bool _isSeated = false;
    public bool IsSeated => _isSeated;
    

    private void Start() // remembers spawnlocation for invalit drop
    {
        _originPosition = transform.position;
        _originRotation = transform.rotation;
    }

    public void SetDraggedPosition(Vector3 _pos)
    {
        transform.position = new Vector3(_pos.x ,_pos.y + 1.5f, _pos.z); // small drag height change so you dont drag them in to the floor
    }
    public void SnapToSeat(Transform seat, Table table)
    {
        _currentSeat = seat;
        _currentTable = table;

        transform.position = seat.position;
        transform.rotation = seat.rotation; // cutomer look at table dunno what it will do with proper models

        _isSeated = true;
    }

    public void ReturnToOrigin()
    {
        _currentSeat = null;
        _currentTable = null;

        transform.position = _originPosition;
        transform.rotation = _originRotation;
    }

    public void leavesSeat() // add this to the customer when they leave then the seat can be re-used
    {
        if (_currentTable != null && _currentSeat != null)
        {
            _currentTable.FreeSeat(_currentSeat);
        }
        _currentSeat = null;
        _currentTable = null;
    }
}
