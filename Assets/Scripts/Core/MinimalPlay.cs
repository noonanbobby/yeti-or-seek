using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class MinimalPlay : MonoBehaviour {
    void Start() {
        // HUD
        var hud = Object.FindFirstObjectByType<HUDController>();
        if (hud == null) {
            hud = new GameObject("HUD").AddComponent<HUDController>();
            hud.SetupBasicHUD();
        }

        // Manager
        var mgr = HideSeekManager.Instance ?? Object.FindFirstObjectByType<HideSeekManager>();
        if (mgr == null) mgr = new GameObject("HideSeekManager").AddComponent<HideSeekManager>();
        mgr.roundDuration = 90f;
        mgr.startSeekerCount = 0; // single-player sanity

        // Player
        var players = new List<YetiMotor>(Object.FindObjectsByType<YetiMotor>(FindObjectsInactive.Exclude, FindObjectsSortMode.None));
        if (players.Count == 0) {
            var p = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            p.name = "PlayerYeti";
            p.transform.position = new Vector3(0, 1, 0);
            var rb = p.AddComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            var motor = p.AddComponent<YetiMotor>();
            players.Add(motor);
        }

        // Camera
        var cam = Camera.main;
        if (cam == null) {
            var c = new GameObject("Main Camera");
            cam = c.AddComponent<Camera>();
            cam.tag = "MainCamera";
            c.AddComponent<AudioListener>();
            c.transform.position = new Vector3(0, 8, -10);
            c.transform.LookAt(Vector3.zero);
        }
        var follow = cam.GetComponent<CameraFollow>() ?? cam.gameObject.AddComponent<CameraFollow>();
        follow.target = players[0].transform;
        follow.offset = new Vector3(0, 7, -8);
        follow.smoothTime = 0.15f;

        // EventSystem
        if (Object.FindFirstObjectByType<EventSystem>() == null) {
            var es = new GameObject("EventSystem");
            es.AddComponent<EventSystem>();
            es.AddComponent<StandaloneInputModule>();
        }

        // Tiny hide-and-seek arena (no finish line)
        var arena = Object.FindFirstObjectByType<HideSeekGenerator>();
        if (arena == null) arena = new GameObject("Arena").AddComponent<HideSeekGenerator>();
        arena.areaSize = new Vector2(20f, 20f);
        arena.hideObjectCount = 6;
        arena.Generate();

        // Start round
        mgr.Setup(players, hud);
    }
}
