using UnityEngine;

public class HideSeekGenerator : MonoBehaviour {
    public Vector2 areaSize = new Vector2(20f, 20f);
    public int hideObjectCount = 6;

    public void Generate() {
        // Ground sized by areaSize (Plane is 10x10 units at scale 1)
        var ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
        ground.name = "Ground";
        ground.transform.position = Vector3.zero;
        ground.transform.localScale = new Vector3(areaSize.x / 10f, 1f, areaSize.y / 10f);
        ground.GetComponent<Renderer>().material.color = new Color(0.9f, 0.95f, 1f);

        // Simple cylindrical hideables
        for (int i = 0; i < hideObjectCount; i++) {
            var b = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            b.name = $"Hideable_{i}";
            float x = Random.Range(-areaSize.x * 0.5f, areaSize.x * 0.5f);
            float z = Random.Range(-areaSize.y * 0.5f, areaSize.y * 0.5f);
            b.transform.position = new Vector3(x, 0.5f, z);
            b.transform.localScale = new Vector3(1.1f, 1.0f, 1.1f);
            b.AddComponent<HideableObject>();
        }
    }
}
