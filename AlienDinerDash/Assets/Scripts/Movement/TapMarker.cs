using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TapMarker : MonoBehaviour
{
    [SerializeField] private float lifetime = 1f;
    [SerializeField] private AnimationCurve scaleCurve;

    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;

        float t = timer / lifetime;
        transform.localScale = Vector3.one * scaleCurve.Evaluate(t);

        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }
}
