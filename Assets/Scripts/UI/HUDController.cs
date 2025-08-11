using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    private Canvas canvas;
    public SimpleJoystick joystick;
    public MobileJumpButton jumpBtn;

    private Text topText;

    public void SetupBasicHUD()
    {
        canvas = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster)).GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        var scaler = canvas.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1080, 1920);

        joystick = SimpleJoystick.Create(canvas);
        jumpBtn = MobileJumpButton.Create(canvas);

        // Top text banner
        var txtGO = new GameObject("TopText", typeof(Text));
        txtGO.transform.SetParent(canvas.transform, false);
        topText = txtGO.GetComponent<Text>();
        topText.alignment = TextAnchor.MiddleCenter;
        topText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        topText.fontSize = 48;
        var rt = txtGO.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0, 1);
        rt.anchorMax = new Vector2(1, 1);
        rt.pivot = new Vector2(0.5f, 1);
        rt.anchoredPosition = new Vector2(0, -30);
        rt.sizeDelta = new Vector2(0, 80);
        ShowBanner("Yeti and Seek");
    }

    public void ShowBanner(string msg)
    {
        if (topText) topText.text = msg;
    }

    public void ShowResult(string msg)
    {
        ShowBanner(msg + "  (+50 flakes if you!)");
    }
}
