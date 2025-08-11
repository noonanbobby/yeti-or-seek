using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class YetiMotor : MonoBehaviour
{
    public float moveSpeed = 8f;
    public float jumpForce = 6.5f;
    public float airControl = 0.6f;

    private Rigidbody rb;
    private HUDController hud;

    private bool grounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.mass = 1.2f;
        hud = FindObjectOfType<HUDController>();
    }

    void FixedUpdate()
    {
        Vector2 input = hud != null && hud.joystick != null ? hud.joystick.Value : Vector2.zero;
        Vector3 dir = new Vector3(input.x, 0, input.y);
        float control = grounded ? 1f : airControl;
        Vector3 velocity = new Vector3(dir.x * moveSpeed * control, rb.velocity.y, dir.z * moveSpeed * control);
        rb.velocity = Vector3.Lerp(rb.velocity, velocity, 0.25f);

        if (hud != null && hud.jumpBtn != null && hud.jumpBtn.Consume() && grounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
        }

        // Face move direction
        var face = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        if (face.sqrMagnitude > 0.05f) transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(face), 0.2f);
    }

    void OnCollisionStay(Collision c)
    {
        foreach (var cp in c.contacts)
        {
            if (Vector3.Dot(cp.normal, Vector3.up) > 0.5f) { grounded = true; return; }
        }
        grounded = false;
    }

    void OnCollisionExit(Collision c) { grounded = false; }
}
