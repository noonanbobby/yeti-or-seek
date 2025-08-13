using UnityEngine;
using System.Linq;

[RequireComponent(typeof(YetiMotor))]
public class SeekerBotAI : MonoBehaviour {
    YetiMotor motor;
    Rigidbody rb;

    void Awake(){ motor=GetComponent<YetiMotor>(); rb=GetComponent<Rigidbody>(); motor.BecomeSeeker(); }

    void Update(){
        // Track closest visible hider
        var all = Object.FindObjectsByType<YetiMotor>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        var target = all.Where(p=>p!=motor && p.role==PlayerRole.Hider && !p.IsHidden)
                        .OrderBy(p=> (p.transform.position - transform.position).sqrMagnitude)
                        .FirstOrDefault();

        Vector3 dir = Vector3.zero;
        if (target!=null){
            var to = target.transform.position - transform.position; to.y=0;
            dir = to.normalized;
            // Tag if close
            if (to.magnitude < 1.6f){
                target.FoundBySeeker();
            }
        } else {
            // Patrol if nobody visible
            float t = Time.time * 0.6f;
            dir = new Vector3(Mathf.PerlinNoise(t,0f)-0.5f, 0, Mathf.PerlinNoise(0f,t)-0.5f).normalized;
        }

        var desired = new Vector3(dir.x * motor.moveSpeed, rb.velocity.y, dir.z * motor.moveSpeed);
        rb.velocity = Vector3.Lerp(rb.velocity, desired, 0.18f);
    }
}
