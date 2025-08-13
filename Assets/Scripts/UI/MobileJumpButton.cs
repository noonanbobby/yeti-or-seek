using UnityEngine;
using UnityEngine.UI;

public class MobileJumpButton : MonoBehaviour {
    bool pressed;

    public static MobileJumpButton Create(Canvas parent, Vector2 anchor, Vector2 offset) {
        var go = new GameObject("JumpButton", typeof(Image));
        go.transform.SetParent(parent.transform, false);
        var img = go.GetComponent<Image>();
        img.color = new Color(0.1f, 0.6f, 1f, 0.9f);
        var rt = go.GetComponent<RectTransform>();
        rt.anchorMin = anchor; rt.anchorMax = anchor; rt.anchoredPosition = offset; rt.sizeDelta = new Vector2(130, 130);

        // Label
        var tgo = new GameObject("Label", typeof(Text));
        tgo.transform.SetParent(go.transform, false);
        var t = tgo.GetComponent<Text>();
        t.text = "JUMP"; t.alignment = TextAnchor.MiddleCenter; t.fontSize = 28; t.color = Color.white;
        t.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        var trt = tgo.GetComponent<RectTransform>(); trt.sizeDelta = rt.sizeDelta;

        return go.AddComponent<MobileJumpButton>();
    }

    public bool Consume(){ if(pressed){ pressed=false; return true; } return false; }
    void Update(){
        #if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Space)) pressed = true;
        #endif
        if (Input.GetMouseButtonDown(0)) pressed = true;
    }
}
