using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class MinimalPlay : MonoBehaviour {
    [Range(0,15)] public int botCount = 6;

    void Start() {
        // Camera
        var cam = Camera.main;
        if (cam == null) { var c=new GameObject("Main Camera"); cam=c.AddComponent<Camera>(); cam.tag="MainCamera"; c.AddComponent<AudioListener>(); c.transform.position=new Vector3(0,10,-14); c.transform.LookAt(Vector3.zero); }
        cam.backgroundColor = Theme.Sky;

        // HUD
        var hud = FindFirstObjectByType<HUDController>() ?? new GameObject("HUD").AddComponent<HUDController>();

        // Manager
        var mgr = HideSeekManager.Instance ?? FindFirstObjectByType<HideSeekManager>() ?? new GameObject("HideSeekManager").AddComponent<HideSeekManager>();
        mgr.roundDuration = 90f; mgr.hidePhaseSeconds = 20f;

        // Player
        var players = new List<YetiMotor>(FindObjectsByType<YetiMotor>(FindObjectsInactive.Exclude, FindObjectsSortMode.None));
        if (players.Count==0){
            var p = GameObject.CreatePrimitive(PrimitiveType.Capsule); p.name="PlayerYeti"; p.transform.position=new Vector3(0,1,0);
            var rb = p.AddComponent<Rigidbody>(); rb.constraints=RigidbodyConstraints.FreezeRotation;
            var m  = p.AddComponent<YetiMotor>();
            var r  = p.GetComponent<Renderer>(); if(r) r.material.color = Theme.Accent;
            players.Add(m);
        }

        // Bots
        for(int i=0;i<botCount;i++){
            var b = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            b.name=$"Bot_{i}";
            b.transform.position = new Vector3(Random.Range(-8f,8f),1,Random.Range(-8f,8f));
            var rb = b.AddComponent<Rigidbody>(); rb.constraints=RigidbodyConstraints.FreezeRotation;
            var m  = b.AddComponent<YetiMotor>();
            var r  = b.GetComponent<Renderer>(); if(r) r.material.color = (i%2==0)? Theme.Primary : Theme.Warn;
            players.Add(m);
        }

        // One bot becomes seeker (AI), rest are hiders (AI)
        if (players.Count>1){
            var allGOs = FindObjectsByType<YetiMotor>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
            // Choose a non-player as seeker
            foreach(var go in allGOs){
                if (go.gameObject.name.StartsWith("Bot_")){ go.gameObject.AddComponent<SeekerBotAI>(); break; }
            }
            foreach(var go in allGOs){
                if (!go.gameObject.GetComponent<SeekerBotAI>()) go.gameObject.AddComponent<SimpleBotAI>();
            }
            mgr.startSeekerCount = 1;
        } else {
            mgr.startSeekerCount = 0; // solo sandbox
        }

        // Camera follow the player
        var follow = cam.GetComponent<CameraFollow>() ?? cam.gameObject.AddComponent<CameraFollow>();
        follow.target = players[0].transform; follow.offset=new Vector3(0,8,-10); follow.smoothTime=0.15f;

        // EventSystem
        if (FindFirstObjectByType<EventSystem>() == null){ var es=new GameObject("EventSystem"); es.AddComponent<EventSystem>(); es.AddComponent<StandaloneInputModule>(); }

        // Arena
        var arena = FindFirstObjectByType<HideSeekGenerator>() ?? new GameObject("Arena").AddComponent<HideSeekGenerator>();
        arena.areaSize = new Vector2(24,24); arena.hideObjectCount=12; arena.Generate();

        // Start round
        mgr.Setup(players, hud);
    }
}
