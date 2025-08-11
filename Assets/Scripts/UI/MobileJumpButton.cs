using UnityEngine;
using UnityEngine.UI;

public class MobileJumpButton : MonoBehaviour
{
    public bool Pressed { get; private set; }

    public static MobileJumpButton Create(Canvas parent)
    {
        var go = new GameObject("JumpButton", typeof(Image));
        go.transform.SetParent(parent.transform, false);
        var img = go.GetComponent<Image>();
        img.color = new Color(1, 1, 1, 0.25f);

        var rect = go.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(1, 0);
        rect.anchorMax = new Vector2(1, 0);
        rect.pivot = new Vector2(1, 0);
        rect.anchoredPosition = new Vector2(-30, 30);
        rect.sizeDelta = new Vector2(140, 140);

        var btn = go.AddComponent<Button>();
        var comp = go.AddComponent<MobileJumpButton>();
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
