using UnityEngine;
using UnityEngine.UI;

public class SimpleJoystick : MonoBehaviour {
    public Vector2 Value { get; private set; }
    RectTransform knob, bg;
    bool dragging;

    public static SimpleJoystick Create(Canvas parent, Vector2 anchor, Vector2 offset) {
        var root = new GameObject("Joystick", typeof(Image));
        root.transform.SetParent(parent.transform,false);
        root.GetComponent<Image>().color = new Color(0,0,0,0.25f);
        var rt = root.GetComponent<RectTransform>(); rt.anchorMin=anchor; rt.anchorMax=anchor; rt.anchoredPosition=offset; rt.sizeDelta=new Vector2(220,220);

        var knobGO = new GameObject("Knob", typeof(Image));
        knobGO.transform.SetParent(root.transform,false);
        knobGO.GetComponent<Image>().color = Theme.Accent;
        var krt = knobGO.GetComponent<RectTransform>(); krt.sizeDelta=new Vector2(96,96); krt.anchoredPosition=Vector2.zero;

        var j = root.AddComponent<SimpleJoystick>(); j.bg=rt; j.knob=krt; return j;
    }

    void Update(){
        #if UNITY_EDITOR
        var kx=Input.GetAxisRaw("Horizontal"); var ky=Input.GetAxisRaw("Vertical");
        if (Mathf.Abs(kx)>0.01f || Mathf.Abs(ky)>0.01f){ Value=new Vector2(kx,ky).normalized; knob.anchoredPosition = Value*(bg.sizeDelta*0.5f*0.8f); return; }
        else if(!dragging){ Value=Vector2.zero; knob.anchoredPosition=Vector2.zero; }
        #endif

        if (Input.GetMouseButtonDown(0)){ if(RectTransformUtility.RectangleContainsScreenPoint(bg, Input.mousePosition)) dragging=true; }
        if (Input.GetMouseButtonUp(0)){ dragging=false; Value=Vector2.zero; knob.anchoredPosition=Vector2.zero; }
        if (dragging){
            RectTransformUtility.ScreenPointToLocalPointInRectangle(bg, Input.mousePosition, null, out var lp);
            var radius = bg.sizeDelta*0.5f*0.8f; lp = Vector2.Max(-radius, Vector2.Min(radius, lp));
            knob.anchoredPosition=lp; Value = radius.sqrMagnitude>0.001f ? lp/radius : Vector2.zero;
        }
    }
}
