using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MobileActionButton : MonoBehaviour, IPointerDownHandler {
    bool pressed;
    public static MobileActionButton Create(Canvas parent, Vector2 anchor, Vector2 offset){
        var go=new GameObject("ActionButton", typeof(Image)); go.transform.SetParent(parent.transform,false);
        go.GetComponent<Image>().color = Theme.Warn;
        var rt=go.GetComponent<RectTransform>(); rt.anchorMin=anchor; rt.anchorMax=anchor; rt.anchoredPosition=offset; rt.sizeDelta=new Vector2(140,140);
        var label=new GameObject("Label", typeof(Text)); label.transform.SetParent(go.transform,false);
        var t=label.GetComponent<Text>(); t.text="HIDE"; t.font=Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf"); t.color=Color.white; t.fontSize=28; t.alignment=TextAnchor.MiddleCenter;
        label.GetComponent<RectTransform>().sizeDelta=rt.sizeDelta;
        return go.AddComponent<MobileActionButton>();
    }
    public void OnPointerDown(PointerEventData e){ pressed=true; }
    public bool Consume(){ if(pressed){ pressed=false; return true;} return false; }
    void Update(){ #if UNITY_EDITOR if(Input.GetKeyDown(KeyCode.E)) pressed=true; #endif }
}
