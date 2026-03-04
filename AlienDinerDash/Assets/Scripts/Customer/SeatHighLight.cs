using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeatHighLight : MonoBehaviour
{
    [SerializeField] private GameObject highlightVisual;

    public void Show()
    {
        highlightVisual.SetActive(true);
        Debug.Log("Highlight ON");
    }

    public void Hide()
    {
        highlightVisual.SetActive(false);
    }
}
