using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeatHighLight : MonoBehaviour
{
    [SerializeField] private GameObject highLightVisual;

    private Table _table;
    private Transform _seat;

    private void Awake()
    {
        _seat = transform;
        _table = GetComponentInParent<Table>();
        
        if(highLightVisual != null)
            highLightVisual.SetActive(false);
    }

    public bool CanHighLight()
    {
        if (_table == null) return false;

        return !_table.IsSeatOccupied(_seat);
    }

    public void Show()
    {
        if(!CanHighLight()) return;
        
        if(highLightVisual != null)
            highLightVisual.SetActive(true);   
    }

    public void Hide()
    {
        if(highLightVisual != null)
            highLightVisual.SetActive(false);
    }
}
