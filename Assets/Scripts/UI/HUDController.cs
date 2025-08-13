using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour {
    public SimpleJoystick joystick;
    public MobileJumpButton jumpBtn;
    public MobileActionButton actionBtn;

    Text bannerText, infoText, flakesText;

    void Awake() {
        // Canvas
        var canvasGO = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        var canvas = canvasGO.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        var scaler = canvasGO.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);

        // Labels
        bannerText = CreateText(canvas.transform, "Yeti or Seek", 36, TextAnchor.UpperCenter, new Vector2(0.5f,1f), new Vector2(0.5f,1f), new Vector2(0,-30));
        infoText   = CreateText(canvas.transform, "", 22, TextAnchor.UpperLeft, new Vector2(0f,1f), new Vector2(0f,1f), new Vector2(16,-16));
        flakesText = CreateText(canvas.transform, "❄ 0", 22, TextAnchor.UpperRight, new Vector2(1f,1f), new Vector2(1f,1f), new Vector2(-16,-16));

        // Controls (BIG + OPAQUE)
        joystick = SimpleJoystick.Create(canvas, new Vector2(0f,0f), new Vector2(160,160));     // bottom-left
        jumpBtn  = MobileJumpButton.Create(canvas, new Vector2(1f,0f), new Vector2(-140,160));  // bottom-right
        actionBtn= MobileActionButton.Create(canvas, new Vector2(1f,0f), new Vector2(-300,160));// left of jump
    }

    Text CreateText(Transform parent, string txt, int size, TextAnchor anchor, Vector2 min, Vector2 max, Vector2 anchored) {
        var go = new GameObject("Text", typeof(Text));
        go.transform.SetParent(parent, false);
        var t = go.GetComponent<Text>();
        t.text = txt; t.fontSize = size; t.alignment = anchor; t.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        t.color = Color.black;
        var rt = go.GetComponent<RectTransform>();
        rt.anchorMin = min; rt.anchorMax = max; rt.anchoredPosition = anchored; rt.sizeDelta = new Vector2(800, 80);
        return t;
    }

    public void UpdateInfo(string s) { if (infoText) infoText.text = s; }
    public void ShowBanner(string s) { if (bannerText) bannerText.text = s; }
    public void ShowResult(string s) { ShowBanner(s); }
    public void UpdateFlakes(int amount) { if (flakesText) flakesText.text = $"❄ {amount}"; }
    public void SetupBasicHUD() { /* compatibility hook */ }
}
