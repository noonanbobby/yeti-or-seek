using UnityEngine;
using System.Collections.Generic;

public class RoundManager : MonoBehaviour {
    public int finishCount;
    HUDController hud;

    void Awake() {
        hud = Object.FindFirstObjectByType<HUDController>();
    }

    // Called by generators/bootstrappers to register a finish line
    public void RegisterFinish(FinishLine f) {
        if (f == null) return;
        f.onReached -= OnAnyFinish; // avoid duplicate subscription
        f.onReached += OnAnyFinish;
    }

    // This MUST match Action<YetiMotor>
    private void OnAnyFinish(YetiMotor motor) {
        finishCount++;
        // Reward & feedback so you can see it working
        CurrencyManager.AddFlakes(10);
        CurrencyManager.Save();
        hud?.ShowBanner($"Finish reached! (+10 flakes)  Total finishes: {finishCount}");
    }

    void OnDestroy() {
        // Clean up subscriptions to avoid leaks when reloading scenes
        foreach (var fl in Object.FindObjectsByType<FinishLine>(FindObjectsInactive.Exclude, FindObjectsSortMode.None)) {
            fl.onReached -= OnAnyFinish;
        }
    }
}
