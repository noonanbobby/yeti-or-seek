using UnityEngine;

public class SpinnerObstacle : MonoBehaviour
{
    public int armCount = 4;
    public float radius = 4f;
    public float speed = 110f; // deg/sec

    private Transform rotor;

    void Start()
    {
        rotor = new GameObject("Rotor").transform;
        rotor.SetParent(transform, false);
        for (int i = 0; i < armCount; i++)
        {
            var arm = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
            arm.SetParent(rotor, false);
            arm.localScale = new Vector3(radius * 2f, 0.3f, 0.3f);
            arm.localPosition = Vector3.zero;
            arm.localRotation = Quaternion.Euler(0, (360f / armCount) * i, 0);
            var kb = arm.gameObject.AddComponent<Knockback>();
            kb.force = 10f;
        }
    }

    void Update()
    {
        rotor.Rotate(0, speed * Time.deltaTime, 0, Space.Self);
    }
}
