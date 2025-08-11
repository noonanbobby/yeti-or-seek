using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class BotMotor : MonoBehaviour
{
    public float moveSpeed = 7.5f;
    public float jumpForce = 6.2f;
    public CourseGenerator course;

    private Rigidbody rb;
    private int nextIndex = 0;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.mass = 1.2f;
    }

    void FixedUpdate()
    {
        if (course == null || course.waypoints.Count == 0) return;
        var target = course.waypoints[Mathf.Clamp(nextIndex, 0, course.waypoints.Count - 1)];
        Vector3 to = (target - transform.position);
        to.y = 0;
        float dist = to.magnitude;
        Vector3 dir = to.normalized;

        // Simple avoidance jitter
        dir += new Vector3(Mathf.PerlinNoise(Time.time, transform.position.x) - 0.5f, 0, Mathf.PerlinNoise(transform.position.z, Time.time) - 0.5f) * 0.2f;
        dir.Normalize();

        Vector3 vel = new Vector3(dir.x * moveSpeed, rb.velocity.y, dir.z * moveSpeed);
        rb.velocity = Vector3.Lerp(rb.velocity, vel, 0.2f);

        if (dist < 2.5f) nextIndex++;
    }
}
