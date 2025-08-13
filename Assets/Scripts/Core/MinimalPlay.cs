using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class MinimalPlay : MonoBehaviour {
    [Range(0,8)] public int botCount = 3;

    void Start() {
        // Camera
        var cam = Camera.main;
        if (cam == null) {
            var c = new GameObject("Main Camera");
            cam = c.AddComponent<Camera>(); cam.tag="MainCamera";
            c.AddComponent<AudioListener>();
            c.transform.position = new Vector3(0, 8, -10); c.transform.LookAt(Vector3.zero);
        }
        cam.backgroundColor = new Color(0.72f, 0.75f, 0.80f);

        // HUD
        var hud = Object.FindFirstObjectByType<HUDController>() ?? new GameObject("HUD").AddComponent<HUDController>();

        // Manager
        var mgr = HideSeekManager.Instance ?? Object.FindFirstObjectByType<HideSeekManager>() ?? new GameObject("HideSeekManager").AddComponent<HideSeekManager>();
        mgr.roundDuration = 90f;

        // Player
        var players = new List<YetiMotor>(Object.FindObjectsByType<YetiMotor>(FindObjectsInactive.Exclude, FindObjectsSortMode.None));
        if (players.Count == 0) {
            var p = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            p.name = "PlayerYeti"; p.transform.position = new Vector3(0, 1, 0);
            var rb = p.AddComponent<Rigidbody>(); rb.constraints = RigidbodyConstraints.FreezeRotation;
            var motor = p.AddComponent<YetiMotor>();
            var r = p.GetComponent<Renderer>(); if (r) r.material.color = new Color(0.6f, 0.8f, 1f);
            players.Add(motor);
        }

        // Optional bots (simple wandering for now)
        for (int i=0;i<botCount;i++){
            var b = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            b.name = $"Bot_{i}"; b.transform.position = new Vector3(Random.Range(-4f,4f),1,Random.Range(-4f,4f));
            var rb = b.AddComponent<Rigidbody>(); rb.constraints = RigidbodyConstraints.FreezeRotation;
            var m  = b.AddComponent<YetiMotor>();
            var r = b.GetComponent<Renderer>(); if (r) r.material.color = new Color(0.85f, 0.9f, 1f);
            b.AddComponent<SimpleBotAI>();
            players.Add(m);
        }

        // Camera follow human
        var follow = cam.GetComponent<CameraFollow>() ?? cam.gameObject.AddComponent<CameraFollow>();
        follow.target = players[0].transform; follow.offset=new Vector3(0,7,-8); follow.smoothTime=0.15f;

        // EventSystem
        if (Object.FindFirstObjectByType<EventSystem>() == null) { var es = new GameObject("EventSystem"); es.AddComponent<EventSystem>(); es.AddComponent<StandaloneInputModule>(); }

        // Arena
        var arena = Object.FindFirstObjectByType<HideSeekGenerator>() ?? new GameObject("Arena").AddComponent<HideSeekGenerator>();
        arena.areaSize = new Vector2(24f,24f); arena.hideObjectCount = 10; arena.Generate();

        // Round
        mgr.startSeekerCount = Mathf.Clamp(players.Count > 1 ? 1 : 0, 0, players.Count-1);
        mgr.Setup(players, hud);
    }
}
