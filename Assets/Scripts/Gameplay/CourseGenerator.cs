using UnityEngine;
using System.Collections.Generic;

public class CourseGenerator : MonoBehaviour
{
    public int lanes = 3;
    public int lengthChunks = 16;
    public float laneWidth = 3f;

    public List<Vector3> waypoints = new();

    public void Generate()
    {
        // Flat ground
        var ground = GameObject.CreatePrimitive(PrimitiveType.Cube);
        ground.name = "Ground";
        ground.transform.localScale = new Vector3(lanes * laneWidth + 6, 1, lengthChunks * 8 + 20);
        ground.transform.position = new Vector3(0, -0.5f, (lengthChunks * 8) * 0.5f);
        ground.GetComponent<Renderer>().material.color = new Color(0.9f, 0.95f, 1f);

        // Procedural obstacles per chunk
        float z = 8f;
        for (int i = 0; i < lengthChunks; i++)
        {
            int type = Random.Range(0, 3); // 0 spinner, 1 bumper row, 2 gap
            if (type == 0)
            {
                var sp = new GameObject($"Spinner_{i}");
                sp.transform.position = new Vector3(0, 1.25f, z);
                var so = sp.AddComponent<SpinnerObstacle>();
                so.armCount = 4;
                so.radius = laneWidth * lanes * 0.5f - 0.5f;
            }
            else if (type == 1)
            {
                for (int l = 0; l < lanes; l++)
                {
                    var b = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                    b.name = $"Bumper_{i}_{l}";
                    b.transform.position = new Vector3((l - (lanes - 1) * 0.5f) * laneWidth, 0.5f, z);
                    b.transform.localScale = new Vector3(0.8f, 0.5f, 0.8f);
                    b.AddComponent<BumperPad>();
                }
            }
            else
            {
                // gap — add a narrow bridge in random lane
                int safeLane = Random.Range(0, lanes);
                var hole = GameObject.CreatePrimitive(PrimitiveType.Cube);
                hole.transform.localScale = new Vector3(lanes * laneWidth + 2, 1f, 6f);
                hole.transform.position = new Vector3(0, -3f, z);
                hole.GetComponent<Renderer>().material.color = new Color(0.8f, 0.9f, 1f);

                var bridge = GameObject.CreatePrimitive(PrimitiveType.Cube);
                bridge.transform.localScale = new Vector3(laneWidth * 0.8f, 0.3f, 6f);
                bridge.transform.position = new Vector3((safeLane - (lanes - 1) * 0.5f) * laneWidth, 0.15f, z);
                bridge.GetComponent<Renderer>().material.color = new Color(0.75f, 0.85f, 1f);
            }

            // Waypoint roughly each chunk
            waypoints.Add(new Vector3(0, 0, z + 4f));
            z += 8f;
        }

        // Finish line
        var finishGO = new GameObject("FinishLine");
        finishGO.transform.position = new Vector3(0, 0f, z + 2f);
        var fl = finishGO.AddComponent<FinishLine>();
        FindObjectOfType<RoundManager>()?.RegisterFinish(fl);

        // Decorative walls
        var left = GameObject.CreatePrimitive(PrimitiveType.Cube);
        left.transform.localScale = new Vector3(0.5f, 4f, z + 8f);
        left.transform.position = new Vector3(-(lanes * laneWidth) * 0.5f - 1.5f, 2f, (z) * 0.5f);
        var right = GameObject.CreatePrimitive(PrimitiveType.Cube);
        right.transform.localScale = new Vector3(0.5f, 4f, z + 8f);
        right.transform.position = new Vector3((lanes * laneWidth) * 0.5f + 1.5f, 2f, (z) * 0.5f);
        left.GetComponent<Renderer>().material.color = right.GetComponent<Renderer>().material.color = new Color(0.85f, 0.9f, 1f);
    }
}
