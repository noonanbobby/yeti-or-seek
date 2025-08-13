using UnityEngine;
using UnityEngine.UI;

public class SimpleJoystick : MonoBehaviour {
    public Vector2 Value { get; private set; }
    RectTransform knob, bg;
    bool dragging;

    public static SimpleJoystick Create(Canvas parent, Vector2 anchor, Vector2 offset) {
        var root = new GameObject("Joystick", typeof(Image));
        root.transform.SetParent(parent.transform, false);
        var bgImg = root.GetComponent<Image>(); bgImg.color = new Color(0,0,0,0.25f);
        var rt = root.GetComponent<RectTransform>();
        rt.anchorMin = anchor; rt.anchorMax = anchor; rt.anchoredPosition = offset; rt.sizeDelta = new Vector2(210,210);

        var knobGO = new GameObject("Knob", typeof(Image));
        knobGO.transform.SetParent(root.transform, false);
        var kImg = knobGO.GetComponent<Image>(); kImg.color = new Color(1,1,1,0.95f);
        var krt = knobGO.GetComponent<RectTransform>(); krt.sizeDelta=new Vector2(92,92);

        var j = root.AddComponent<SimpleJoystick>();
        j.bg = rt; j.knob = krt;
        return j;
    }

    void Update() {
        // Keyboard fallback in Editor
        #if UNITY_EDITOR
        var kx = Input.GetAxisRaw("Horizontal");
        var ky = Input.GetAxisRaw("Vertical");
        if (Mathf.Abs(kx) > 0.01f || Mathf.Abs(ky) > 0.01f) {
            Value = new Vector2(kx, ky).normalized;
            knob.anchoredPosition = Value * (bg.sizeDelta * 0.5f * 0.8f);
            return;
        } else if (!dragging) {
            // IMPORTANT: reset when no keys AND not dragging
            Value = Vector2.zero;
            if (knob) knob.anchoredPosition = Vector2.zero;
        }
        #endif

        if (Input.GetMouseButtonDown(0)) {
            Vector2 m = Input.mousePosition;
            if (RectTransformUtility.RectangleContainsScreenPoint(bg, m)) dragging = true;
        }
        if (Input.GetMouseButtonUp(0)) {
            dragging = false; Value = Vector2.zero;
            if (knob) knob.anchoredPosition = Vector2.zero;
        }
        if (dragging) {
            Vector2 m = Input.mousePosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(bg, m, null, out var lp);
            var radius = bg.sizeDelta * 0.5f * 0.8f;
            lp = Vector2.Max(-radius, Vector2.Min(radius, lp));
            knob.anchoredPosition = lp;
            Value = radius.sqrMagnitude > 0.001f ? lp / radius : Vector2.zero;
        }
    }
}
