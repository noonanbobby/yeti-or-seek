using UnityEngine;

/// <summary>
/// A large snowball that rolls down the course, knocking players back.
/// </summary>
public class RollingSnowball : MonoBehaviour {
    public float speed = 6f;

    void FixedUpdate() {
        transform.Translate(Vector3.back * speed * Time.fixedDeltaTime, Space.World);
    }

    private void OnCollisionEnter(Collision c) {
        var rb = c.rigidbody;
        if (rb != null) {
            rb.AddForce((Vector3.back + Vector3.up * 0.5f) * 8f, ForceMode.VelocityChange);
        }
    }
}
