using System;
using UnityEngine;

public class FinishLine : MonoBehaviour {
    // RoundManager subscribes to this. We'll invoke it from a trigger.
    public event Action<YetiMotor> onReached;

    private void OnTriggerEnter(Collider other) {
        var motor = other.GetComponent<YetiMotor>();
        if (motor != null) {
            onReached?.Invoke(motor);
        }
    }
}
