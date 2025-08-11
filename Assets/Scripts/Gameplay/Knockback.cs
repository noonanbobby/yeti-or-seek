using UnityEngine;

public class Knockback : MonoBehaviour
{
    public float force = 8f;
    void OnCollisionEnter(Collision c)
    {
        var rb = c.rigidbody;
        if (rb != null)
        {
            var dir = (c.transform.position - transform.position).normalized;
            dir.y = 0.35f;
            rb.AddForce(dir * force, ForceMode.VelocityChange);
        }
    }
}
