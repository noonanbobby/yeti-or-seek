using UnityEngine;

[RequireComponent(typeof(YetiMotor))]
public class SimpleBotAI : MonoBehaviour {
    YetiMotor motor;
    Rigidbody rb;
    float nextChoose;
    Vector3 target;

    void Awake(){ motor=GetComponent<YetiMotor>(); rb=GetComponent<Rigidbody>(); Choose(); }

    void Update(){
        if (Time.time>=nextChoose) Choose();
        var to = target - transform.position; to.y=0;
        if (to.sqrMagnitude < 4f) Choose();
        var dir = to.normalized;
        var desired = new Vector3(dir.x * motor.moveSpeed * 0.8f, rb.velocity.y, dir.z * motor.moveSpeed * 0.8f);
        rb.velocity = Vector3.Lerp(rb.velocity, desired, 0.15f);

        // Occasionally hide if near an object
        if (motor.role==PlayerRole.Hider && Random.value<0.0025f) SendMessage("TryHide", SendMessageOptions.DontRequireReceiver);
    }

    void Choose(){
        target = new Vector3(Random.Range(-10f,10f), 0, Random.Range(-10f,10f));
        nextChoose = Time.time + Random.Range(2f,4f);
    }
}
