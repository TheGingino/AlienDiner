using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerVisualFeedback : MonoBehaviour
{
    [SerializeField] private float dragScaleMultiplier = 2f;
    [SerializeField] private float scaleSpeed = 10f;

    private Vector3 _OrginalScale;
    private Coroutine _scaleroutine;

    private void Awake()
    {
        _OrginalScale = transform.localScale;
    }

    public void OnDragStart()
    {
        AnimateScale(_OrginalScale * dragScaleMultiplier);
    }

    public void OnDragEnd()
    {
        AnimateScale(_OrginalScale);
    }

    private void AnimateScale(Vector3 target)
    {
        if(_scaleroutine != null)
            StopCoroutine(_scaleroutine);

        _scaleroutine = StartCoroutine(ScaleRoutine(target));
    }

    private IEnumerator ScaleRoutine(Vector3 target)
    {
        while (Vector3.Distance(transform.localScale, target) > 0.01f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, target, Time.deltaTime * scaleSpeed);
            yield return null;
        }

        transform.localScale = target;
    }
}
