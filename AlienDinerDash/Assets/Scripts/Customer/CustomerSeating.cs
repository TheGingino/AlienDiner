using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSeating : MonoBehaviour
{
    private Transform _currentSeat;
    private Table _currentTable;
    public void SetDraggedPosition(Vector3 _pos)
    {
        transform.position = _pos;
    }
    public void SnapToSeat(Transform seat, Table table)
    {
        _currentSeat = seat;
        _currentTable = table;

        transform.position = seat.position;
        transform.rotation = seat.rotation;
    }

    public void leaveSeat()
    {
        if (_currentTable != null && _currentSeat != null)
        {
            _currentTable.FreeSeat(_currentSeat);
        }
        _currentSeat = null;
        _currentTable = null;
    }
}
