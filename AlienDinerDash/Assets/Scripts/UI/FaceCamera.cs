using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    private Camera _cam;
    [SerializeField] Canvas _canvas;

    void Start()
    {
        _cam = Camera.main;
    }

    void LateUpdate()
    {
        _canvas.transform.LookAt(_cam.transform);
    }
}