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

    public bool TrySeatCustomer(CustomerSeating customer, Vector3 dropPosition)
    {
        int bestIndex = -1;
        float bestDistance = float.MaxValue;
        
        for (int i = 0; i < seats.Count; i++)
        {
            if (_occupied[i]) continue;

            float dist = (dropPosition - seats[i].position).sqrMagnitude;

            if (dist < bestDistance)
            {
                bestDistance = dist;
                bestIndex = i;
            }
           
        }
        if (bestIndex == -1)
            return false; 
       
        _occupied[bestIndex] = true;
        customer.SnapToSeat(seats[bestIndex], this);
        return true;
    }

    public bool IsSeatOccupied(Transform seat)
    {
        int index = seats.IndexOf(seat);

        if (index >= 0)
        {
            return _occupied[index];
        }

        return true;
    }

    public void FreeSeat(Transform seat)
    {
        int index = seats.IndexOf(seat);
        if (index >= 0)
            _occupied[index] = false;
    }
}
