using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// HideSeekGenerator constructs a simple arena for hide‑and‑seek. It creates
/// a flat ground plane and spawns a number of HideableObject instances
/// randomly within the arena. The generator exposes the list of spawned
/// hideables so that the HideSeekManager can reference them when assigning
/// hiding spots. Additional decorative elements or obstacles could be added
/// here to vary the environment.
/// </summary>
public class HideSeekGenerator : MonoBehaviour
{
    /// <summary>Half‑width/height of the square arena.</summary>
    public int areaSize = 40;
    /// <summary>Number of hideable objects to spawn.</summary>
    public int hideObjectCount = 20;

    /// <summary>List of all hideable objects created by Generate().</summary>
    public List<HideableObject> hideObjects = new List<HideableObject>();

    public void Generate()
    {
        // Create ground plane
        var ground = GameObject.CreatePrimitive(PrimitiveType.Cube);
        ground.name = "Ground";
        float size = areaSize;
        ground.transform.localScale = new Vector3(size * 2f, 1f, size * 2f);
        ground.transform.position = new Vector3(0f, -0.5f, 0f);
        ground.GetComponent<Renderer>().material.color = new Color(0.9f, 0.95f, 1f);

        // Spawn hideable objects
        for (int i = 0; i < hideObjectCount; i++)
        {
            var h = new GameObject($"Hideable_{i:D2}");
            h.transform.position = new Vector3(Random.Range(-size * 0.8f, size * 0.8f), 0.5f, Random.Range(-size * 0.8f, size * 0.8f));
            var shapeChoice = Random.Range(0, 3);
            GameObject prim;
            switch (shapeChoice)
            {
                case 0:
                    prim = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                    break;
                case 1:
                    prim = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                    break;
                default:
                    prim = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    break;
            }
            prim.transform.SetParent(h.transform, false);
            prim.transform.localScale = new Vector3(Random.Range(1f, 2.5f), Random.Range(1.5f, 3f), Random.Range(1f, 2.5f));
            prim.GetComponent<Renderer>().material.color = new Color(Random.Range(0.7f, 0.95f), Random.Range(0.7f, 0.95f), Random.Range(0.7f, 0.95f));
            var hideable = h.AddComponent<HideableObject>();
            hideObjects.Add(hideable);
        }

        // Perimeter walls
        float wallHeight = 4f;
        float wallThickness = 0.5f;
        var left = GameObject.CreatePrimitive(PrimitiveType.Cube);
        left.transform.localScale = new Vector3(wallThickness, wallHeight, size * 2f + 2f);
        left.transform.position = new Vector3(-size - 1f, wallHeight / 2f, 0f);
        var right = GameObject.CreatePrimitive(PrimitiveType.Cube);
        right.transform.localScale = new Vector3(wallThickness, wallHeight, size * 2f + 2f);
        right.transform.position = new Vector3(size + 1f, wallHeight / 2f, 0f);
        var front = GameObject.CreatePrimitive(PrimitiveType.Cube);
        front.transform.localScale = new Vector3(size * 2f + 2f, wallHeight, wallThickness);
        front.transform.position = new Vector3(0f, wallHeight / 2f, size + 1f);
        var back = GameObject.CreatePrimitive(PrimitiveType.Cube);
        back.transform.localScale = new Vector3(size * 2f + 2f, wallHeight, wallThickness);
        back.transform.position = new Vector3(0f, wallHeight / 2f, -size - 1f);
        foreach (var w in new[] { left, right, front, back })
        {
            w.GetComponent<Renderer>().material.color = new Color(0.85f, 0.9f, 1f);
        }
    }
}
