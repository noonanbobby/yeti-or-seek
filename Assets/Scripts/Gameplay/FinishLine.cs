using UnityEngine;
using System;

public class FinishLine : MonoBehaviour
{
    public event Action<GameObject> onReached;

    void Start()
    {
        var poleL = GameObject.CreatePrimitive(PrimitiveType.Cube);
        var poleR = GameObject.CreatePrimitive(PrimitiveType.Cube);
        poleL.transform.SetParent(transform, false);
        poleR.transform.SetParent(transform, false);
        poleL.transform.localScale = poleR.transform.localScale = new Vector3(0.3f, 3f, 0.3f);
        poleL.transform.localPosition = new Vector3(-3.5f, 1.5f, 0);
        poleR.transform.localPosition = new Vector3( 3.5f, 1.5f, 0);

        var tape = GameObject.CreatePrimitive(PrimitiveType.Cube);
        tape.transform.SetParent(transform, false);
        tape.transform.localScale = new Vector3(7.5f, 0.2f, 0.2f);
        tape.transform.localPosition = new Vector3(0, 1.2f, 0);
        tape.GetComponent<Renderer>().material.color = new Color(1f, 0.9f, 0.4f);

        var trigger = gameObject.AddComponent<BoxCollider>();
        trigger.isTrigger = true;
        trigger.center = new Vector3(0, 1.0f, 0);
        trigger.size = new Vector3(7.5f, 2.0f, 1.0f);
    }

    void OnTriggerEnter(Collider other)
    {
        onReached?.Invoke(other.gameObject);
    }
}
