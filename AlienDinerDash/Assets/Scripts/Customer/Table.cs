using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    [SerializeField] private List<Transform> seats = new List<Transform>();

    private bool[] _occupied;

    private void Awake()
    {
        _occupied = new bool[seats.Count];
    }

    public bool HasFreeSeat()
    {
        for (int i = 0; i < _occupied.Length; i++)
        {
            if (!_occupied[i])
                return true;
        }

        return false;
    }

    public bool TrySeatCustomer(CustomerSeating customer)
    {
        for (int i = 0; i < seats.Count; i++)
        {
            if (_occupied[i]) continue;

            _occupied[i] = true;
            customer.SnapToSeat(seats[i], this);
            return true;
        }

        return false;
    }

    public void FreeSeat(Transform seat)
    {
        int index = seats.IndexOf(seat);
        if (index >= 0)
            _occupied[index] = false;
    }
}
