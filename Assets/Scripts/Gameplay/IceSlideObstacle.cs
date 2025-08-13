using UnityEngine;

/// <summary>
/// Slippery patch that accelerates players forward.
/// </summary>
public class IceSlideObstacle : MonoBehaviour {
    public float slideForce = 12f;

    private void OnCollisionStay(Collision c) {
        var rb = c.rigidbody;
        if (rb != null) {
            rb.AddForce(Vector3.forward * slideForce * Time.deltaTime, ForceMode.Acceleration);
        }
    }
}
