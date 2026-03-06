using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SeatHoverManager : MonoBehaviour
{
    [SerializeField] private LayerMask seatLayer;
    private Camera _cam;
    private SeatHighLight _currentHighLight;

    private void Awake()
    {
        _cam = Camera.main;
    }

    public void UpdateHover(Vector3 screenPosition)
    {
        Ray ray = _cam.ScreenPointToRay(screenPosition);
        
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, seatLayer))
        {
            SeatHighLight hover = hit.collider.GetComponent<SeatHighLight>();

            if (hover != null && hover.CanHighLight())
            {
                if(_currentHighLight != hover)
                {
                    if(_currentHighLight != null)
                        _currentHighLight.Hide();
                    
                    hover.Show();
                    _currentHighLight = hover;
                }
            }
            else
            {
                ClearHighLight();
            }
        }
        else
        {
            ClearHighLight();
        }
    }

    public void ClearHighLight()
    {
        if(_currentHighLight != null)
        {
            _currentHighLight.Hide();
            _currentHighLight = null;
        }
    }
}
