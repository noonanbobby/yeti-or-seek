using UnityEngine;

/// <summary>
/// Handles player movement, jumping, hiding/unhiding, and role changes.
/// </summary>
[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class YetiMotor : MonoBehaviour {
    public float moveSpeed = 8f;
    public float jumpForce = 6.5f;
    public float airControl = 0.6f;

    public PlayerRole role = PlayerRole.Hider;

    private Rigidbody rb;
    private Collider col;
    private Renderer[] renderers;
    private HUDController hud;
    private bool grounded;
    private bool isHidden;
    private HideableObject currentHideable;

    void Awake() {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.mass = 1.2f;
        col = GetComponent<Collider>();
        renderers = GetComponentsInChildren<Renderer>(true);
        hud = Object.FindFirstObjectByType<HUDController>();
    }

    void FixedUpdate() {
        if (!isHidden) {
            Vector2 input = (hud != null && hud.joystick != null) ? hud.joystick.Value : Vector2.zero;
            Vector3 dir = new Vector3(input.x, 0, input.y);
            float control = grounded ? 1f : airControl;
            Vector3 velocity = new Vector3(dir.x * moveSpeed * control, rb.velocity.y, dir.z * moveSpeed * control);
            rb.velocity = Vector3.Lerp(rb.velocity, velocity, 0.2f);

            if (hud != null && hud.jumpBtn != null && hud.jumpBtn.Consume() && grounded) {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
            }

            Vector3 face = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            if (face.sqrMagnitude > 0.05f) {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(face), 0.2f);
            }
        }

        // Action button for hide/unhide
        if (hud != null && hud.actionBtn != null && hud.actionBtn.Consume()) {
            if (isHidden) Unhide();
            else TryHide();
        }
    }

    private void TryHide() {
        if (role != PlayerRole.Hider) return;
        float minDist = 1.5f;
        HideableObject best = null;
        foreach (var ho in Object.FindObjectsByType<HideableObject>(FindObjectsInactive.Exclude, FindObjectsSortMode.None)) {
            float d = Vector3.Distance(transform.position, ho.transform.position);
            if (d < minDist) { minDist = d; best = ho; }
        }
        if (best != null && best.TryHide(this)) {
            currentHideable = best;
            isHidden = true;
            foreach (var r in renderers) r.enabled = false;
            if (col != null) col.enabled = false;
            rb.velocity = Vector3.zero;
        }
    }

    public void Unhide() {
        if (!isHidden) return;
        isHidden = false;
        if (currentHideable != null) { currentHideable.Unhide(); currentHideable = null; }
        foreach (var r in renderers) r.enabled = true;
        if (col != null) col.enabled = true;
    }

    /// <summary>Converts this player to a seeker role.</summary>
    public void BecomeSeeker() {
        if (role == PlayerRole.Seeker) return;
        if (isHidden) Unhide();
        role = PlayerRole.Seeker;
        var rend = GetComponent<Renderer>();
        if (rend) rend.material.color = new Color(1f, 0.5f, 0.5f); // red tint
    }

    /// <summary>Called when a seeker collides with the hideable.</summary>
    public void FoundBySeeker() {
        if (role == PlayerRole.Hider) {
            BecomeSeeker();
            HideSeekManager.Instance?.NotifyFound(this);
        }
    }

    void OnCollisionStay(Collision c) {
        grounded = false;
        foreach (var cp in c.contacts) {
            if (Vector3.Dot(cp.normal, Vector3.up) > 0.5f) { grounded = true; break; }
        }
    }

    void OnCollisionExit(Collision c) { grounded = false; }
}
