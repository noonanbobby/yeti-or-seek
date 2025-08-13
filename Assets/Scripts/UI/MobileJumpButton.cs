using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MobileJumpButton : MonoBehaviour, IPointerDownHandler {
    bool pressed;

    public static MobileJumpButton Create(Canvas parent, Vector2 anchor, Vector2 offset) {
        var go = new GameObject("JumpButton", typeof(Image));
        go.transform.SetParent(parent.transform, false);
        var img = go.GetComponent<Image>(); img.color = new Color(0.1f, 0.6f, 1f, 0.95f);
        var rt = go.GetComponent<RectTransform>();
        rt.anchorMin=anchor; rt.anchorMax=anchor; rt.anchoredPosition=offset; rt.sizeDelta=new Vector2(140,140);

        var labelGO = new GameObject("Label", typeof(Text)); labelGO.transform.SetParent(go.transform,false);
        var t = labelGO.GetComponent<Text>(); t.text="JUMP"; t.alignment=TextAnchor.MiddleCenter; t.fontSize=28; t.color=Color.white;
        t.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        labelGO.GetComponent<RectTransform>().sizeDelta = rt.sizeDelta;

        return go.AddComponent<MobileJumpButton>();
    }

    public void OnPointerDown(PointerEventData e){ pressed = true; }
    public bool Consume(){ if(pressed){ pressed=false; return true; } return false; }

    void Update(){
        #if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Space)) pressed = true;
        #endif
    }
}
