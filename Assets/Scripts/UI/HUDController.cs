using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour {
    public SimpleJoystick joystick;
    public MobileJumpButton jumpBtn;
    public MobileActionButton actionBtn;

    Text titleText, objectiveText, timerText, hidersText, flakesText;
    Font UiFont;

    void Awake() {
        UiFont = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

        var canvasGO = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        canvasGO.transform.SetParent(transform, false);
        var canvas = canvasGO.GetComponent<Canvas>(); canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        var scaler = canvasGO.GetComponent<CanvasScaler>(); scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize; scaler.referenceResolution=new Vector2(1920,1080);

        // Top bar
        var topBar = Box(canvasGO.transform, Theme.Primary, new Vector2(0,1), new Vector2(1,1), new Vector2(0,88), new Vector2(0,-44));
        titleText     = MakeText(topBar.transform, "Yeti & Seek", 40, TextAnchor.MiddleLeft, Color.white, UiFont, new Vector2(24,0), alignRight:false);
        timerText     = MakeText(topBar.transform, "90", 40, TextAnchor.MiddleRight, Color.white, UiFont, new Vector2(-24,0), alignRight:true);

        // Objective + Hiders left (below bar)
        var infoStrip = Box(canvasGO.transform, Theme.UIBack, new Vector2(0.5f,1), new Vector2(0.5f,1), new Vector2(960,60), new Vector2(0,-140));
        objectiveText = MakeText(infoStrip.transform, "HIDE PHASE", 26, TextAnchor.MiddleCenter, Theme.UIText, UiFont, Vector2.zero, false);

        var hiderPanel = Box(canvasGO.transform, Theme.UIBack, new Vector2(0,1), new Vector2(0,1), new Vector2(300,60), new Vector2(180,-140));
        hidersText = MakeText(hiderPanel.transform, "Hiders: -", 24, TextAnchor.MiddleCenter, Theme.UIText, UiFont, Vector2.zero, false);

        var flakesPanel = Box(canvasGO.transform, Theme.UIBack, new Vector2(1,1), new Vector2(1,1), new Vector2(220,60), new Vector2(-180,-140));
        flakesText = MakeText(flakesPanel.transform, "❄ 0", 24, TextAnchor.MiddleCenter, Theme.UIText, UiFont, Vector2.zero, false);

        // Controls
        joystick = SimpleJoystick.Create(canvas, new Vector2(0,0), new Vector2(180,180));
        jumpBtn  = MobileJumpButton.Create(canvas, new Vector2(1,0), new Vector2(-160,180));
        actionBtn= MobileActionButton.Create(canvas, new Vector2(1,0), new Vector2(-330,180));
    }

    RectTransform Box(Transform parent, Color c, Vector2 min, Vector2 max, Vector2 size, Vector2 pos){
        var go = new GameObject("Box", typeof(Image));
        go.transform.SetParent(parent,false);
        var img = go.GetComponent<Image>(); img.color = c;
        var rt = go.GetComponent<RectTransform>();
        rt.anchorMin=min; rt.anchorMax=max; rt.sizeDelta=size; rt.anchoredPosition=pos;
        return rt;
    }
    Text MakeText(Transform parent,string txt,int size,TextAnchor anchor,Color color,Font font,Vector2 pad,bool alignRight){
        var go = new GameObject("Text", typeof(Text));
        go.transform.SetParent(parent,false);
        var t = go.GetComponent<Text>(); t.text=txt; t.fontSize=size; t.alignment=anchor; t.color=color; t.font=font;
        var rt = go.GetComponent<RectTransform>(); rt.anchorMin=Vector2.zero; rt.anchorMax=Vector2.one; rt.offsetMin=new Vector2(16+pad.x,8); rt.offsetMax=new Vector2(-16+pad.x,-8);
        if(alignRight){ var o = go.AddComponent<Outline>(); o.effectDistance=new Vector2(2,-2); }
        return t;
    }

    public void SetObjective(string s){ if(objectiveText) objectiveText.text=s; }
    public void SetTimer(int seconds){ if(timerText) timerText.text = seconds.ToString(); }
    public void SetHiders(int n){ if(hidersText) hidersText.text=$"Hiders: {n}"; }
    public void UpdateFlakes(int amount){ if(flakesText) flakesText.text=$"❄ {amount}"; }
    public void SetupBasicHUD() {}
}
