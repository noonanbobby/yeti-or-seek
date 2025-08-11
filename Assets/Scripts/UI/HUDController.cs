using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    private Canvas canvas;
    public SimpleJoystick joystick;
    public MobileJumpButton jumpBtn;
    public MobileActionButton actionBtn;

    private Text bannerText;
    private Text infoText;

    /// <summary>
    /// Initializes the HUD with all UI elements: joystick, jump button, action button, banner, and info text.
    /// </summary>
    public void SetupBasicHUD()
    {
        // Create Canvas
        canvas = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster)).GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        var scaler = canvas.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1080, 1920);

        // Create joystick and buttons
        joystick = SimpleJoystick.Create(canvas);
        jumpBtn = MobileJumpButton.Create(canvas);
        actionBtn = MobileActionButton.Create(canvas);

        // Banner text at top of screen
        var bannerGO = new GameObject("BannerText", typeof(Text));
        bannerGO.transform.SetParent(canvas.transform, false);
        bannerText = bannerGO.GetComponent<Text>();
        bannerText.alignment = TextAnchor.MiddleCenter;
        bannerText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        bannerText.fontSize = 48;
        var brt = bannerGO.GetComponent<RectTransform>();
        brt.anchorMin = new Vector2(0f, 1f);
        brt.anchorMax = new Vector2(1f, 1f);
        brt.pivot = new Vector2(0.5f, 1f);
        brt.anchoredPosition = new Vector2(0, -30);
        brt.sizeDelta = new Vector2(0, 80);

        // Info text below banner
        var infoGO = new GameObject("InfoText", typeof(Text));
        infoGO.transform.SetParent(canvas.transform, false);
        infoText = infoGO.GetComponent<Text>();
        infoText.alignment = TextAnchor.MiddleCenter;
        infoText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        infoText.fontSize = 32;
        var irt = infoGO.GetComponent<RectTransform>();
        irt.anchorMin = new Vector2(0f, 1f);
        irt.anchorMax = new Vector2(1f, 1f);
        irt.pivot = new Vector2(0.5f, 1f);
        irt.anchoredPosition = new Vector2(0, -100);
        irt.sizeDelta = new Vector2(0, 60);

        ShowBanner("Yeti and Seek");
        UpdateInfo("");
    }

    public void ShowBanner(string msg)
    {
        if (bannerText != null) bannerText.text = msg;
    }

    public void UpdateInfo(string msg)
    {
        if (infoText != null) infoText.text = msg;
    }

    public void ShowResult(string msg)
    {
        // Show final result message (used by RoundManager or HideSeekManager)
        ShowBanner(msg);
    }
}
