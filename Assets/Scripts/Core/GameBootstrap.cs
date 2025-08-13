using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameBootstrap : MonoBehaviour {
    [Header("Arena")]
    public float area = 20f;
    public int hideCount = 6;

    [Header("Round")]
    public float totalTimeSeconds = 90f;
    public int seekersAtStart = 1;

    void Start() {
        // HUD
        var hud = Object.FindFirstObjectByType<HUDController>();
        if (hud == null) {
            hud = new GameObject("HUD").AddComponent<HUDController>();
            hud.SetupBasicHUD(); // no-op placeholder
        }

        // Manager
        var mgr = HideSeekManager.Instance ?? Object.FindFirstObjectByType<HideSeekManager>();
        if (mgr == null) mgr = new GameObject("HideSeekManager").AddComponent<HideSeekManager>();

        // Players (spawn 1 if none)
        var players = new List<YetiMotor>(Object.FindObjectsByType<YetiMotor>(FindObjectsInactive.Exclude, FindObjectsSortMode.None));
        if (players.Count == 0) {
            var playerGO = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            playerGO.name = "PlayerYeti";
            playerGO.transform.position = new Vector3(0, 1, 0);
            var rb = playerGO.AddComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            var motor = playerGO.AddComponent<YetiMotor>();
            players.Add(motor);
        }

        // Camera follow
        var cam = Camera.main;
        if (cam == null) {
            var camGO = new GameObject("Main Camera");
            cam = camGO.AddComponent<Camera>();
            cam.tag = "MainCamera";
            camGO.AddComponent<AudioListener>();
            camGO.transform.position = new Vector3(0, 8, -10);
            camGO.transform.LookAt(Vector3.zero);
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

        // Generator
        var gen = Object.FindFirstObjectByType<HideSeekGenerator>();
        if (gen == null) gen = new GameObject("Arena").AddComponent<HideSeekGenerator>();
        gen.areaSize = new Vector2(area, area); // <-- fixes the int → Vector2 error
        gen.hideObjectCount = hideCount;
        gen.Generate();

        // Round config (compat with your HideSeekManager fields)
        mgr.roundDuration = totalTimeSeconds;
        mgr.startSeekerCount = seekersAtStart;

        // If single-player, don't assign a seeker at start
        mgr.startSeekerCount = Mathf.Min(mgr.startSeekerCount, Mathf.Max(0, players.Count - 1));

        // Start round
        mgr.Setup(players, hud);
    }
}
