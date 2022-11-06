using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform target;

    public float smoothSpeed = 0.125f;

    protected void LateUpdate()
    {
        transform.position = target.position; 
    }
}
