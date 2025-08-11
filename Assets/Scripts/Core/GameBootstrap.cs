using UnityEngine;

public class GameBootstrap : MonoBehaviour
{
    [Header("Course")]
    public int seed = 12345;
    public int lengthChunks = 16; // course length
    public int lanes = 3;

    [Header("Bots")] public int botCount = 24;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        Random.InitState(seed);

        // UI
        var hudGO = new GameObject("HUD");
        var hud = hudGO.AddComponent<HUDController>();
        hud.SetupBasicHUD();
        DontDestroyOnLoad(hudGO);

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

        // Finish line listener
        var rmGO = new GameObject("RoundManager");
        var rm = rmGO.AddComponent<RoundManager>();
        rm.player = motor;
        rm.hud = hud;

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

        // Currency / Inventory stubs
        CurrencyManager.Load();
        InventoryManager.Load();
    }
}
