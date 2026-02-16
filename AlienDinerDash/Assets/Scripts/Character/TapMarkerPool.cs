using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapMarkerPool : MonoBehaviour
{
    [SerializeField] private GameObject tapMarkerPrefab;
    [SerializeField] private int poolSize = 6;

    private Queue<GameObject> _pool = new Queue<GameObject>();

    private void Awake()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(tapMarkerPrefab, transform);
            obj.SetActive(false);
            _pool.Enqueue(obj);
        }
    }
    
    public GameObject GetMarker()
    {
        GameObject obj = _pool.Dequeue();
        obj.SetActive(true);
        _pool.Enqueue(obj);
        return obj;
    }
}
