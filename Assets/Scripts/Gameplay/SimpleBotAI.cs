using UnityEngine;

/// Super-simple bot: wander randomly, occasionally hide near props.
[RequireComponent(typeof(YetiMotor))]
public class SimpleBotAI : MonoBehaviour {
    YetiMotor motor;
    Vector3 target;
    float retargetAt;

    void Awake(){ motor = GetComponent<YetiMotor>(); PickTarget(); }

    void Update(){
        if (Time.time >= retargetAt){ PickTarget(); }
        // Move towards target
        Vector3 to = (target - transform.position);
        to.y = 0;
        Vector2 input = new Vector2(to.normalized.x, to.normalized.z);
        // Simulate joystick by nudging Rigidbody directly via motor's movement
        // (motor reads joystick each FixedUpdate; we emulate by setting velocity direction)
        // Lightweight approach: just rotate towards target; movement handled by motor physics.
        if (to.sqrMagnitude < 4f) PickTarget();

        // Fake joystick by writing to rigidbody velocity direction a bit
        var rb = GetComponent<Rigidbody>();
        var desired = new Vector3(input.x * motor.moveSpeed * 0.9f, rb.velocity.y, input.y * motor.moveSpeed * 0.9f);
        rb.velocity = Vector3.Lerp(rb.velocity, desired, 0.15f);

        // Occasionally try to hide
        if (Random.value < 0.003f) SendMessage("TryHide", SendMessageOptions.DontRequireReceiver);
    }

    void PickTarget(){
        target = new Vector3(Random.Range(-10f,10f), 0, Random.Range(-10f,10f));
        retargetAt = Time.time + Random.Range(2f, 4.5f);
    }
}
