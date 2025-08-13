using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour {
    public SimpleJoystick joystick;
    public MobileJumpButton jumpBtn;
    public MobileActionButton actionBtn;

    Text bannerText, infoText, flakesText;
    Font UiFont;

    void Awake() {
        // Use Unity 6 builtin font + fallback
        UiFont = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        if (UiFont == null) { try { UiFont = Font.CreateDynamicFontFromOSFont("Helvetica", 16);} catch {} }

        // Canvas
        var canvasGO = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        canvasGO.transform.SetParent(transform, false);
        var canvas = canvasGO.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        var scaler = canvasGO.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);

        // Banner
        var bannerBg = Box(canvasGO.transform, new Color(0.06f,0.20f,0.55f,0.95f), anchorMin:new Vector2(0,1), anchorMax:new Vector2(1,1), size:new Vector2(0,72), pos:new Vector2(0,-36));
        bannerText = MakeText(bannerBg.transform, "Yeti & Seek", 34, TextAnchor.MiddleCenter, Color.white, UiFont, outline:true);

        // Info (top-left) + Flakes (top-right)
        var infoBG = Box(canvasGO.transform, new Color(1,1,1,0.7f), new Vector2(0,1), new Vector2(0,1), new Vector2(520,56), new Vector2(16,-96));
        infoText   = MakeText(infoBG.transform, "Move: WASD/←→↑↓ | Space: Jump | E: Hide", 20, TextAnchor.MiddleLeft, Color.black, UiFont, outline:false);

        var flakesBG = Box(canvasGO.transform, new Color(1,1,1,0.7f), new Vector2(1,1), new Vector2(1,1), new Vector2(220,56), new Vector2(-16,-96));
        flakesText = MakeText(flakesBG.transform, "❄ 0", 22, TextAnchor.MiddleRight, Color.black, UiFont, outline:false);

        // Controls
        joystick = SimpleJoystick.Create(canvas, new Vector2(0,0), new Vector2(170,170));
        jumpBtn  = MobileJumpButton.Create(canvas, new Vector2(1,0), new Vector2(-150,170));
        actionBtn= MobileActionButton.Create(canvas, new Vector2(1,0), new Vector2(-320,170));
    }

    RectTransform Box(Transform parent, Color c, Vector2 anchorMin, Vector2 anchorMax, Vector2 size, Vector2 pos) {
        var go = new GameObject("Box", typeof(Image));
        go.transform.SetParent(parent, false);
        var img = go.GetComponent<Image>(); img.color = c;
        var rt = go.GetComponent<RectTransform>(); rt.anchorMin=anchorMin; rt.anchorMax=anchorMax; rt.sizeDelta=size; rt.anchoredPosition=pos;
        return rt;
    }

    Text MakeText(Transform parent, string txt, int size, TextAnchor anchor, Color color, Font font, bool outline) {
        var go = new GameObject("Text", typeof(Text));
        go.transform.SetParent(parent, false);
        var t = go.GetComponent<Text>();
        t.text = txt; t.fontSize = size; t.alignment = anchor; t.color = color; t.font = font;
        var rt = go.GetComponent<RectTransform>(); rt.anchorMin=Vector2.zero; rt.anchorMax=Vector2.one; rt.offsetMin=Vector2.zero; rt.offsetMax=Vector2.zero;
        if (outline) go.AddComponent<Outline>().effectDistance = new Vector2(2, -2);
        return t;
    }

    public void UpdateInfo(string s){ if(infoText) infoText.text = s; }
    public void ShowBanner(string s){ if(bannerText) bannerText.text = s; }
    public void ShowResult(string s){ ShowBanner(s); }
    public void UpdateFlakes(int amount){ if(flakesText) flakesText.text = $"❄ {amount}"; }
    public void SetupBasicHUD() { /* compatibility hook */ }
}
