using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages a hide-and-seek round: assigns seekers, tracks time, updates HUD,
/// and awards flakes to winners. Hiders win if time runs out; seekers win if all hiders are found.
/// </summary>
public class HideSeekManager : MonoBehaviour {
    public static HideSeekManager Instance { get; private set; }

    public float roundDuration = 90f;
    public int startSeekerCount = 1;
    public HUDController hud;
    public List<YetiMotor> players = new();

    private float timeRemaining;
    private int hidersLeft;
    private bool roundOver;

    void Awake() {
        Instance = this;
    }

    public void Setup(List<YetiMotor> playerList, HUDController hudController) {
        players = playerList;
        hud = hudController;
        timeRemaining = roundDuration;
        AssignInitialRoles();
        UpdateHUD();
        hud.ShowBanner("Hide & Seek!");
    }

    private void AssignInitialRoles() {
        hidersLeft = players.Count;
        var pool = new List<YetiMotor>(players);
        int assigned = 0;
        while (assigned < startSeekerCount && pool.Count > 0) {
            int idx = Random.Range(0, pool.Count);
            var seeker = pool[idx];
            pool.RemoveAt(idx);
            seeker.BecomeSeeker();
            hidersLeft--;
            assigned++;
        }
        foreach (var p in players) {
            if (p.role == PlayerRole.Hider) {
                var rend = p.GetComponent<Renderer>();
                if (rend) rend.material.color = new Color(0.6f, 0.8f, 1f);
            }
        }
        roundOver = false;
    }

    void Update() {
        if (roundOver) return;
        timeRemaining -= Time.deltaTime;
        if (timeRemaining <= 0f) {
            EndRound(seekersWin: false);
        }
        UpdateHUD();
    }

    public void NotifyFound(YetiMotor hider) {
        if (roundOver || hider.role != PlayerRole.Hider) return;
        hider.BecomeSeeker();
        hidersLeft--;
        UpdateHUD();
        if (hidersLeft <= 0) {
            EndRound(seekersWin: true);
        }
    }

    private void EndRound(bool seekersWin) {
        roundOver = true;
        var local = players.Count > 0 ? players[0] : null;
        bool playerWon = false;
        if (local != null) {
            if (local.role == PlayerRole.Seeker) playerWon = seekersWin;
            else playerWon = !seekersWin;
        }

        if (!seekersWin) {
            foreach (var p in players) {
                if (p.role == PlayerRole.Hider) CurrencyManager.AddFlakes(30);
            }
        } else {
            foreach (var p in players) {
                if (p.role == PlayerRole.Seeker) CurrencyManager.AddFlakes(30);
            }
        }
        CurrencyManager.Save();
        hud.ShowResult(playerWon ? "You won!" : "You lost!");
    }

    private void UpdateHUD() {
        if (hud && !roundOver) {
            int hidersLeftClamped = Mathf.Max(hidersLeft, 0);
            hud.UpdateInfo($"Time: {timeRemaining:F0}s   Hiders left: {hidersLeftClamped}");
        }
    }
}
