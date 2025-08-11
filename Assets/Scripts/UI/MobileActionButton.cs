using UnityEngine;
using UnityEngine.UI;

public class MobileActionButton : MonoBehaviour
{
    public bool Pressed { get; private set; }

    public static MobileActionButton Create(Canvas parent)
    {
        var go = new GameObject("ActionButton", typeof(Image));
        go.transform.SetParent(parent.transform, false);
        var img = go.GetComponent<Image>();
        img.color = new Color(1, 1, 1, 0.25f);

        var rect = go.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(1, 0);
        rect.anchorMax = new Vector2(1, 0);
        rect.pivot = new Vector2(1, 0);
        rect.anchoredPosition = new Vector2(-180, 30);
        rect.sizeDelta = new Vector2(140, 140);

        var btn = go.AddComponent<Button>();
        var comp = go.AddComponent<MobileActionButton>();
        btn.onClick.AddListener(() => comp.Pressed = true);
        return comp;
    }

    public bool Consume()
    {
        if (!Pressed) return false;
        Pressed = false;
        return true;
    }
}
