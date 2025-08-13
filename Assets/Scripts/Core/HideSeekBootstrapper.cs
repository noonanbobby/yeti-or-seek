using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class HideSeekBootstrapper : MonoBehaviour {
    void Start() {
        // HUD (create if missing)
        var hud = Object.FindFirstObjectByType<HUDController>();
        if (hud == null) {
            hud = new GameObject("HUD").AddComponent<HUDController>();
            hud.SetupBasicHUD(); // no-op placeholder, keeps GameBootstrap happy if it calls it
        }

        // Manager (create if missing)
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

        // Arena generator (create + generate if missing)
        var gen = Object.FindFirstObjectByType<HideSeekGenerator>();
        if (gen == null) gen = new GameObject("Arena").AddComponent<HideSeekGenerator>();
        gen.Generate();

        // If only 1 player, don't assign a seeker at round start
        mgr.startSeekerCount = Mathf.Min(mgr.startSeekerCount, Mathf.Max(0, players.Count - 1));

        // Kick off the round
        mgr.Setup(players, hud);
    }
}
