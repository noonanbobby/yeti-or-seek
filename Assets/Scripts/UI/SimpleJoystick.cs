using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SimpleJoystick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public RectTransform knob;
    public RectTransform background;
    public Vector2 Value { get; private set; }

    private Canvas canvas;
    private float radius;

    public static SimpleJoystick Create(Canvas parent)
    {
        var go = new GameObject("Joystick");
        go.transform.SetParent(parent.transform, false);
        var j = go.AddComponent<SimpleJoystick>();

        var bg = new GameObject("BG", typeof(Image)).GetComponent<Image>();
        bg.transform.SetParent(go.transform, false);
        bg.color = new Color(1, 1, 1, 0.15f);
        bg.raycastTarget = true;
        j.background = bg.rectTransform;

        var kb = new GameObject("Knob", typeof(Image)).GetComponent<Image>();
        kb.transform.SetParent(bg.transform, false);
        kb.color = new Color(1, 1, 1, 0.35f);
        j.knob = kb.rectTransform;

        var rect = j.GetComponent<RectTransform>() ?? go.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(0, 0);
        rect.anchorMax = new Vector2(0, 0);
        rect.pivot = new Vector2(0, 0);
        rect.anchoredPosition = new Vector2(30, 30);
        rect.sizeDelta = new Vector2(160, 160);

        j.background.anchorMin = Vector2.zero;
        j.background.anchorMax = Vector2.one;
        j.background.offsetMin = Vector2.zero;
        j.background.offsetMax = Vector2.zero;

        j.knob.sizeDelta = new Vector2(60, 60);
        j.knob.anchoredPosition = Vector2.zero;

        j.canvas = parent;
        j.radius = 70f;
        return j;
    }

    public void OnPointerDown(PointerEventData eventData) => OnDrag(eventData);

    public void OnDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(background, eventData.position, canvas.worldCamera, out var pos);
        var v = Vector2.ClampMagnitude(pos / radius, 1f);
        Value = v;
        knob.anchoredPosition = v * radius;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Value = Vector2.zero;
        knob.anchoredPosition = Vector2.zero;
    }
}
