using UnityEngine;

public class BumperPad : MonoBehaviour
{
    public float bounce = 8f;
    void OnCollisionEnter(Collision c)
    {
        var rb = c.rigidbody;
        if (rb != null)
        {
            rb.AddForce(Vector3.up * bounce, ForceMode.VelocityChange);
        }
    }
}
