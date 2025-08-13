using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Generates a linear race course with various obstacles.
/// </summary>
public class CourseGenerator : MonoBehaviour {
    public int lanes = 3;
    public int lengthChunks = 16;
    public float laneWidth = 3f;
    public List<Vector3> waypoints = new();

    public void Generate() {
        var ground = GameObject.CreatePrimitive(PrimitiveType.Cube);
        ground.name = "Ground";
        ground.transform.localScale = new Vector3(lanes * laneWidth + 6, 1f, lengthChunks * 8 + 20);
        ground.transform.position = new Vector3(0, -0.5f, (lengthChunks * 8) * 0.5f);
        ground.GetComponent<Renderer>().material.color = new Color(0.9f, 0.95f, 1f);

        float z = 8f;
        for (int i = 0; i < lengthChunks; i++) {
            int type = Random.Range(0, 5); // 0 spinner, 1 bumper row, 2 gap, 3 snowball, 4 ice slide
            if (type == 0) {
                var sp = new GameObject($"Spinner_{i}");
                sp.transform.position = new Vector3(0, 1.25f, z);
                var so = sp.AddComponent<SpinnerObstacle>();
                so.armCount = 4;
                so.radius = laneWidth * lanes * 0.5f - 0.5f;
            } else if (type == 1) {
                for (int l = 0; l < lanes; l++) {
                    var b = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                    b.name = $"Bumper_{i}_{l}";
                    b.transform.position = new Vector3((l - (lanes - 1) * 0.5f) * laneWidth, 0.5f, z);
                    b.transform.localScale = new Vector3(0.8f, 0.5f, 0.8f);
                    b.AddComponent<BumperPad>();
                }
            } else if (type == 2) {
                int safeLane = Random.Range(0, lanes);
                var hole = GameObject.CreatePrimitive(PrimitiveType.Cube);
                hole.transform.localScale = new Vector3(lanes * laneWidth + 2, 1f, 6f);
                hole.transform.position = new Vector3(0, -3f, z);
                hole.GetComponent<Renderer>().material.color = new Color(0.8f, 0.9f, 1f);

                var bridge = GameObject.CreatePrimitive(PrimitiveType.Cube);
                bridge.transform.localScale = new Vector3(laneWidth * 0.8f, 0.3f, 6f);
                bridge.transform.position = new Vector3((safeLane - (lanes - 1) * 0.5f) * laneWidth, 0.15f, z);
                bridge.GetComponent<Renderer>().material.color = new Color(0.75f, 0.85f, 1f);
            } else if (type == 3) {
                var snowball = new GameObject($"Snowball_{i}");
                snowball.transform.position = new Vector3(0f, 1f, z);
                snowball.transform.localScale = Vector3.one * 2f;
                var rb = snowball.AddComponent<Rigidbody>();
                rb.useGravity = false;
                rb.mass = 5f;
                var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                sphere.transform.SetParent(snowball.transform, false);
                sphere.transform.localScale = Vector3.one;
                sphere.GetComponent<Renderer>().material.color = new Color(0.9f, 0.95f, 1f);
                snowball.AddComponent<RollingSnowball>();
            } else if (type == 4) {
                var slideGO = new GameObject($"IceSlide_{i}");
                slideGO.transform.position = new Vector3(0f, 0f, z);
                var col = slideGO.AddComponent<BoxCollider>();
                col.size = new Vector3(lanes * laneWidth + 2f, 0.2f, 6f);
                col.isTrigger = false;
                slideGO.AddComponent<IceSlideObstacle>();

                var vis = GameObject.CreatePrimitive(PrimitiveType.Cube);
                vis.transform.SetParent(slideGO.transform, false);
                vis.transform.localScale = new Vector3(lanes * laneWidth + 2f, 0.1f, 6f);
                vis.GetComponent<Renderer>().material.color = new Color(0.7f, 0.85f, 1f);
            }

            waypoints.Add(new Vector3(0, 0, z + 4f));
            z += 8f;
        }

        var finishGO = new GameObject("FinishLine");
        finishGO.transform.position = new Vector3(0, 0f, z + 2f);
        var fl = finishGO.AddComponent<FinishLine>();

        // Add trigger collider for finish detection
        var bc = finishGO.AddComponent<BoxCollider>();
        bc.isTrigger = true;
        bc.size = new Vector3(lanes * laneWidth, 2f, 1f);

        var rm = Object.FindFirstObjectByType<RoundManager>();
        rm?.RegisterFinish(fl);

        var left = GameObject.CreatePrimitive(PrimitiveType.Cube);
        left.transform.localScale = new Vector3(0.5f, 4f, z + 8f);
        left.transform.position = new Vector3(-(lanes * laneWidth) * 0.5f - 1.5f, 2f, z * 0.5f);
        var right = GameObject.CreatePrimitive(PrimitiveType.Cube);
        right.transform.localScale = new Vector3(0.5f, 4f, z + 8f);
        right.transform.position = new Vector3((lanes * laneWidth) * 0.5f + 1.5f, 2f, z * 0.5f);
        left.GetComponent<Renderer>().material.color = right.GetComponent<Renderer>().material.color = new Color(0.85f, 0.9f, 1f);
    }
}
