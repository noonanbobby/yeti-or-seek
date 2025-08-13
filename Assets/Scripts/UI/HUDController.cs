using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour {
    public SimpleJoystick joystick;
    public MobileJumpButton jumpBtn;
    public MobileActionButton actionBtn;
    Text bannerText, infoText;

    void Awake(){
        // Create Canvas
        var canvasGO = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        var canvas = canvasGO.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        var scaler = canvasGO.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920,1080);

        bannerText = CreateText(canvas.transform, "Yeti or Seek", 28, TextAnchor.UpperCenter, new Vector2(0.5f,1f), new Vector2(0.5f,1f), new Vector2(0,-30));
        infoText   = CreateText(canvas.transform, "", 18, TextAnchor.UpperLeft, new Vector2(0,1f), new Vector2(0,1f), new Vector2(12,-12));

        // Controls
        joystick = SimpleJoystick.Create(canvas);
        jumpBtn  = MobileJumpButton.Create(canvas, new Vector2(1f,0), new Vector2(-120,120));
        actionBtn= MobileActionButton.Create(canvas, new Vector2(1f,0), new Vector2(-260,120));
    }

    public void SetupBasicHUD() {
        // Provided for compatibility with GameBootstrap; our HUD builds itself in Awake.
        // You can add extra labels/buttons here later if GameBootstrap expects more.
    }

    Text CreateText(Transform parent,string txt,int size,TextAnchor anchor,Vector2 min,Vector2 max,Vector2 anchored){
        var go = new GameObject("Text", typeof(Text));
        go.transform.SetParent(parent,false);
        var t = go.GetComponent<Text>();
        t.text = txt; t.fontSize = size; t.alignment = anchor; t.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        var rt = go.GetComponent<RectTransform>(); rt.anchorMin=min; rt.anchorMax=max; rt.anchoredPosition=anchored; rt.sizeDelta=new Vector2(700,60);
        return t;
    }

    public void UpdateInfo(string s){ if(infoText) infoText.text = s; }
    public void ShowBanner(string s){ if(bannerText) bannerText.text = s; }
    public void ShowResult(string s){ ShowBanner(s); }
}
