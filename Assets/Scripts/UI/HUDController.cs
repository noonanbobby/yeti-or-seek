using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour {
    public SimpleJoystick joystick;
    public MobileJumpButton jumpBtn;
    public MobileActionButton actionBtn;

    Text bannerText, infoText, flakesText;

    void Awake() {
        Debug.Log("[HUD] HUDController.Awake()");

        // Canvas
        var canvasGO = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        canvasGO.transform.SetParent(transform, false);
        var canvas = canvasGO.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        var scaler = canvasGO.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);

        // A translucent root so you can SEE the UI is present
        var root = new GameObject("UIRoot", typeof(Image));
        root.transform.SetParent(canvasGO.transform, false);
        var rootImg = root.GetComponent<Image>();
        rootImg.color = new Color(0f, 0f, 0f, 0.05f);
        var rrt = root.GetComponent<RectTransform>();
        rrt.anchorMin = Vector2.zero; rrt.anchorMax = Vector2.one; rrt.offsetMin = Vector2.zero; rrt.offsetMax = Vector2.zero;

        // Fixed banner at top
        var bannerBg = new GameObject("BannerBG", typeof(Image));
        bannerBg.transform.SetParent(root.transform, false);
        var bbRT = bannerBg.GetComponent<RectTransform>();
        bbRT.anchorMin = new Vector2(0, 1); bbRT.anchorMax = new Vector2(1, 1);
        bbRT.sizeDelta = new Vector2(0, 64); bbRT.anchoredPosition = new Vector2(0, -32);
        bannerBg.GetComponent<Image>().color = new Color(0.05f, 0.2f, 0.5f, 0.8f);

        bannerText = CreateText(bannerBg.transform, "HUD ONLINE", 30, TextAnchor.MiddleCenter, Color.white);

        // Info (top-left) + Flakes (top-right)
        var infoBG = new GameObject("InfoBG", typeof(Image)); infoBG.transform.SetParent(root.transform, false);
        var ibRT = infoBG.GetComponent<RectTransform>(); ibRT.anchorMin=new Vector2(0,1); ibRT.anchorMax=new Vector2(0,1); ibRT.anchoredPosition = new Vector2(180,-100); ibRT.sizeDelta = new Vector2(700,50);
        infoBG.GetComponent<Image>().color = new Color(1,1,1,0.6f);
        infoText = CreateText(infoBG.transform, "", 22, TextAnchor.MiddleLeft, Color.black);

        var flakesBG = new GameObject("FlakesBG", typeof(Image)); flakesBG.transform.SetParent(root.transform, false);
        var fbRT = flakesBG.GetComponent<RectTransform>(); fbRT.anchorMin=new Vector2(1,1); fbRT.anchorMax=new Vector2(1,1); fbRT.anchoredPosition = new Vector2(-160,-100); fbRT.sizeDelta = new Vector2(200,50);
        flakesBG.GetComponent<Image>().color = new Color(1,1,1,0.6f);
        flakesText = CreateText(flakesBG.transform, "❄ 0", 22, TextAnchor.MiddleRight, Color.black);

        // Controls (large & opaque)
        joystick = SimpleJoystick.Create(canvas, new Vector2(0,0), new Vector2(160,160));
        jumpBtn  = MobileJumpButton.Create(canvas, new Vector2(1,0), new Vector2(-140,160));
        actionBtn= MobileActionButton.Create(canvas, new Vector2(1,0), new Vector2(-300,160));

        Debug.Log("[HUD] HUD built (joystick + buttons + labels)");
    }

    Text CreateText(Transform parent, string txt, int size, TextAnchor anchor, Color color) {
        var go = new GameObject("Text", typeof(Text));
        go.transform.SetParent(parent, false);
        var t = go.GetComponent<Text>();
        t.text = txt; t.fontSize = size; t.alignment = anchor; t.font = Resources.GetBuiltinResource<Font>("Arial.ttf"); t.color = color;
        var rt = go.GetComponent<RectTransform>(); rt.anchorMin = Vector2.zero; rt.anchorMax = Vector2.one; rt.offsetMin = Vector2.zero; rt.offsetMax = Vector2.zero;
        return t;
    }

    public void UpdateInfo(string s){ if(infoText) infoText.text = s; }
    public void ShowBanner(string s){ if(bannerText) bannerText.text = s; }
    public void ShowResult(string s){ ShowBanner(s); }
    public void UpdateFlakes(int amount){ if(flakesText) flakesText.text = $"❄ {amount}"; }
    public void SetupBasicHUD() { /* compatibility hook */ }
}
