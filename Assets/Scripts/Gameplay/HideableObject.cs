using UnityEngine;

public class HideableObject : MonoBehaviour {
    YetiMotor occupant;

    public bool TryHide(YetiMotor m){
        if (occupant != null) return false;
        occupant = m;
        m.transform.position = transform.position + Vector3.up * 0.1f;
        return true;
    }

    public void Unhide(){ occupant = null; }
}
