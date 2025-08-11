using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothTime = 0.15f;
    private Vector3 velocity;

    void LateUpdate()
    {
        if (!target) return;
        var desired = target.position + new Vector3(0, 6f, -8f);
        transform.position = Vector3.SmoothDamp(transform.position, desired, ref velocity, smoothTime);
        transform.LookAt(target.position + Vector3.up * 1.0f);
    }
}
