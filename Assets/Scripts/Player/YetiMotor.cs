using UnityEngine;

/// Player movement + hide/unhide.
/// Now supports proper input reset, button events, and reliable hide radius.
[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class YetiMotor : MonoBehaviour {
    public float moveSpeed = 8f;
    public float jumpForce = 6.5f;
    public float airControl = 0.6f;
    public float hideRadius = 2.0f;

    public PlayerRole role = PlayerRole.Hider;

    Rigidbody rb; Collider col; Renderer[] rends;
    HUDController hud;
    bool grounded, isHidden;
    HideableObject currentHideable;

    void Awake(){
        rb = GetComponent<Rigidbody>(); rb.constraints = RigidbodyConstraints.FreezeRotation; rb.mass = 1.2f;
        col = GetComponent<Collider>();
        rends = GetComponentsInChildren<Renderer>(true);
        hud = Object.FindFirstObjectByType<HUDController>();
    }

    void Update(){
        // Queue actions from buttons in Update
        if (hud != null && hud.actionBtn != null && hud.actionBtn.Consume()){
            if (isHidden) Unhide(); else TryHide();
        }
        if (hud != null && hud.jumpBtn != null && hud.jumpBtn.Consume() && grounded){
            rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
        }
    }

    void FixedUpdate(){
        if (isHidden) { rb.velocity = Vector3.zero; return; }

        Vector2 v = (hud && hud.joystick != null) ? hud.joystick.Value : Vector2.zero;
        float control = grounded ? 1f : airControl;
        Vector3 wish = new Vector3(v.x * moveSpeed * control, rb.velocity.y, v.y * moveSpeed * control);
        rb.velocity = Vector3.Lerp(rb.velocity, wish, 0.25f);

        Vector3 face = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        if (face.sqrMagnitude > 0.05f) {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(face), 0.2f);
        }
    }

    void OnCollisionStay(Collision c){
        grounded = false;
        foreach (var cp in c.contacts) { if (Vector3.Dot(cp.normal, Vector3.up) > 0.5f) { grounded = true; break; } }
    }
    void OnCollisionExit(Collision c){ grounded = false; }

    // HIDE / UNHIDE
    void TryHide(){
        if (role != PlayerRole.Hider) return;
        var cols = Physics.OverlapSphere(transform.position, hideRadius);
        HideableObject best=null; float bestD=float.MaxValue;
        foreach (var cc in cols){
            var ho = cc.GetComponent<HideableObject>();
            if (ho == null) continue;
            float d = Vector3.SqrMagnitude(cc.transform.position - transform.position);
            if (d < bestD){ bestD=d; best=ho; }
        }
        if (best != null && best.TryHide(this)){
            currentHideable = best; isHidden = true;
            foreach (var r in rends) r.enabled = false; if (col) col.enabled = false;
            rb.velocity = Vector3.zero;
        }
    }
    public void Unhide(){
        if (!isHidden) return;
        isHidden = false;
        currentHideable?.Unhide(); currentHideable=null;
        foreach (var r in rends) r.enabled = true; if (col) col.enabled = true;
    }

    public void BecomeSeeker(){
        if (role == PlayerRole.Seeker) return;
        if (isHidden) Unhide();
        role = PlayerRole.Seeker;
        var r = GetComponent<Renderer>(); if (r) r.material.color = new Color(1f,0.5f,0.5f);
    }
    public void FoundBySeeker(){
        if (role == PlayerRole.Hider){
            BecomeSeeker();
            HideSeekManager.Instance?.NotifyFound(this);
        }
    }
}
