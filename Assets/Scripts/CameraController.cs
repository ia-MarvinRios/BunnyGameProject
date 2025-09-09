using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    [Header("Controller Settings")]
    [SerializeField] Transform _cameraFollow;
    [SerializeField] Vector3 _offset = Vector3.zero;

    void LateUpdate()
    {
        if (_cameraFollow != null)
        {
            transform.position = _cameraFollow.position + _offset;
        }
    }
}
