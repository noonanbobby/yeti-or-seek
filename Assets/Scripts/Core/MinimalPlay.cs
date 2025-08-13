using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class MinimalPlay : MonoBehaviour {
    void Start() {
        Debug.Log("[Boot] MinimalPlay.Start()");

        // Camera setup and nicer background
        var cam = Camera.main;
        if (cam == null) {
            var c = new GameObject("Main Camera");
            cam = c.AddComponent<Camera>();
            cam.tag = "MainCamera";
            c.AddComponent<AudioListener>();
            c.transform.position = new Vector3(0, 8, -10);
            c.transform.LookAt(Vector3.zero);
        }
        cam.backgroundColor = new Color(0.72f, 0.75f, 0.80f);

        // HUD
        var hud = Object.FindFirstObjectByType<HUDController>();
        if (hud == null) { hud = new GameObject("HUD").AddComponent<HUDController>(); hud.SetupBasicHUD(); }

        // Manager
        var mgr = HideSeekManager.Instance ?? Object.FindFirstObjectByType<HideSeekManager>();
        if (mgr == null) mgr = new GameObject("HideSeekManager").AddComponent<HideSeekManager>();
        mgr.roundDuration = 90f;
        mgr.startSeekerCount = 0;

        // Player
        var players = new List<YetiMotor>(Object.FindObjectsByType<YetiMotor>(FindObjectsInactive.Exclude, FindObjectsSortMode.None));
        if (players.Count == 0) {
            var p = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            p.name = "PlayerYeti";
            p.transform.position = new Vector3(0, 1, 0);
            var rb = p.AddComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            var motor = p.AddComponent<YetiMotor>();
            var r = p.GetComponent<Renderer>(); if (r) r.material.color = new Color(0.6f, 0.8f, 1f);
            players.Add(motor);
        }

        // Camera follow
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

        // Arena
        var arena = Object.FindFirstObjectByType<HideSeekGenerator>();
        if (arena == null) arena = new GameObject("Arena").AddComponent<HideSeekGenerator>();
        arena.areaSize = new Vector2(22f, 22f);
        arena.hideObjectCount = 8;
        arena.Generate();

        // Start round
        mgr.Setup(players, hud);
    }
}
