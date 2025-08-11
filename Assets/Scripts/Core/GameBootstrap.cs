using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// GameBootstrap sets up the core runtime objects required to run a match of
/// Yeti Or Seek. Depending on the selected GameMode it will either build a
/// procedural race course (Race mode) or a hide‑and‑seek arena (HideSeek mode).
/// It instantiates the HUD, environment, player(s), AI bots, and the relevant
/// manager to coordinate the round. This script runs in the generated scene
/// created by the AutoSceneBuilder and should exist exactly once.
/// </summary>
public class GameBootstrap : MonoBehaviour
{
    [Header("General")]
    public GameMode mode = GameMode.HideSeek;
    public int seed = 12345;
    [Header("Race Settings")]
    public int lengthChunks = 16;
    public int lanes = 3;
    public int botCount = 24;
    [Header("Hide&Seek Settings")]
    public int areaSize = 40;
    public int hideObjectCount = 20;
    public int playerCount = 8;
    public float roundTime = 90f;
    public int seekersCount = 1;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        Random.InitState(seed);

        // Create HUD
        var hudGO = new GameObject("HUD");
        var hud = hudGO.AddComponent<HUDController>();
        hud.SetupBasicHUD();
        DontDestroyOnLoad(hudGO);

        if (mode == GameMode.Race)
        {
            SetupRace(hud);
        }
        else if (mode == GameMode.HideSeek)
        {
            SetupHideSeek(hud);
        }

        // Load persistent systems
        CurrencyManager.Load();
        InventoryManager.Load();
    }

    private void SetupRace(HUDController hud)
    {
        // Course
        var courseGO = new GameObject("Course");
        var course = courseGO.AddComponent<CourseGenerator>();
        course.lanes = lanes;
        course.lengthChunks = lengthChunks;
        course.Generate();

        // Player
        var playerGO = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        playerGO.name = "PlayerYeti";
        playerGO.transform.position = new Vector3(0, 1.1f, 0);
        var motor = playerGO.AddComponent<YetiMotor>();
        motor.moveSpeed = 8f;
        motor.jumpForce = 6.5f;
        playerGO.AddComponent<Knockback>();

        // Camera
        var cam = Camera.main != null ? Camera.main : new GameObject("MainCamera").AddComponent<Camera>();
        var follow = cam.gameObject.AddComponent<CameraFollow>();
        follow.target = playerGO.transform;
        cam.transform.position = playerGO.transform.position + new Vector3(0, 6f, -8f);
        cam.transform.rotation = Quaternion.Euler(20, 0, 0);

        // Round Manager
        var rmGO = new GameObject("RoundManager");
        var rm = rmGO.AddComponent<RoundManager>();
        rm.player = motor;
        rm.hud = hud;

        // Finish registration will occur during CourseGenerator.Generate

        // Bots
        for (int i = 0; i < botCount; i++)
        {
            var bot = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            bot.name = $"Bot_{i:D2}";
            bot.transform.position = new Vector3(Random.Range(-3, 3), 1.1f, Random.Range(-2, 2));
            var bm = bot.AddComponent<BotMotor>();
            bm.moveSpeed = Random.Range(6.2f, 8.4f);
            bm.jumpForce = 6.2f;
            bm.course = course;
            bot.AddComponent<Knockback>();
            rm.RegisterBot(bm);
        }
    }

    private void SetupHideSeek(HUDController hud)
    {
        // Environment
        var arenaGO = new GameObject("HideSeekArena");
        var generator = arenaGO.AddComponent<HideSeekGenerator>();
        generator.areaSize = areaSize;
        generator.hideObjectCount = hideObjectCount;
        generator.Generate();

        // Create players list
        var players = new List<YetiMotor>();
        for (int i = 0; i < playerCount; i++)
        {
            var p = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            p.name = i == 0 ? "PlayerYeti" : $"Bot_{i:D2}";
            // Random spawn inside arena
            float spawnX = Random.Range(-(areaSize * 0.4f), (areaSize * 0.4f));
            float spawnZ = Random.Range(-(areaSize * 0.4f), (areaSize * 0.4f));
            p.transform.position = new Vector3(spawnX, 1.1f, spawnZ);
            var motor = p.AddComponent<YetiMotor>();
            motor.moveSpeed = 6.5f;
            motor.jumpForce = 6.5f;
            p.AddComponent<Knockback>();
            players.Add(motor);
        }

        // Camera on local player
        var cam = Camera.main != null ? Camera.main : new GameObject("MainCamera").AddComponent<Camera>();
        var follow = cam.gameObject.AddComponent<CameraFollow>();
        follow.target = players[0].transform;
        cam.transform.position = players[0].transform.position + new Vector3(0, 8f, -10f);
        cam.transform.rotation = Quaternion.Euler(30, 0, 0);

        // Hide&Seek Manager
        var hsmGO = new GameObject("HideSeekManager");
        var hsm = hsmGO.AddComponent<HideSeekManager>();
        hsm.players = players;
        hsm.hud = hud;
        hsm.totalTime = roundTime;
        hsm.seekerCount = seekersCount;
        hsm.hideObjects = generator.hideObjects;
        hsm.StartRound();
    }
}
