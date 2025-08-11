using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// HideSeekManager orchestrates a hide‑and‑seek round. It assigns roles to
/// players, tracks the remaining time and number of hiders, updates the HUD
/// with the timer and score, and ends the round when conditions are met.
/// Players discovered by seekers are converted into seekers for the remainder
/// of the round.
/// </summary>
public class HideSeekManager : MonoBehaviour
{
    public static HideSeekManager Instance { get; private set; }

    public List<YetiMotor> players = new List<YetiMotor>();
    public List<HideableObject> hideObjects = new List<HideableObject>();
    public HUDController hud;
    public float totalTime = 90f;
    public int seekerCount = 1;

    private float timeRemaining;
    private int hidersRemaining;
    private bool roundOver = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    public void StartRound()
    {
        timeRemaining = totalTime;
        // Assign seekers randomly
        var available = new List<YetiMotor>(players);
        hidersRemaining = players.Count;
        for (int i = 0; i < seekerCount && available.Count > 0; i++)
        {
            int idx = Random.Range(0, available.Count);
            var seeker = available[idx];
            available.RemoveAt(idx);
            seeker.role = PlayerRole.Seeker;
            // Colour seekers
            var renderer = seeker.GetComponent<Renderer>();
            if (renderer != null) renderer.material.color = new Color(1f, 0.5f, 0.5f);
            hidersRemaining--;
        }
        // Colour hiders (blue-ish)
        foreach (var p in players)
        {
            if (p.role == PlayerRole.Hider)
            {
                var renderer = p.GetComponent<Renderer>();
                if (renderer != null) renderer.material.color = new Color(0.6f, 0.8f, 1f);
            }
        }
        UpdateHUD();
    }

    private void Update()
    {
        if (roundOver) return;
        timeRemaining -= Time.deltaTime;
        if (timeRemaining < 0f) timeRemaining = 0f;
        UpdateHUD();
        if (timeRemaining <= 0f || hidersRemaining <= 0)
        {
            EndRound();
        }
    }

    /// <summary>Called by a hider when discovered by a seeker.</summary>
    public void NotifyFound(YetiMotor player)
    {
        if (roundOver) return;
        // Only update if this player was a hider
        if (player.role == PlayerRole.Seeker) return;
        hidersRemaining--;
        // Convert to seeker handled in YetiMotor
        UpdateHUD();
        if (hidersRemaining <= 0)
        {
            EndRound();
        }
    }

    private void UpdateHUD()
    {
        if (hud == null) return;
        int hidersLeft = Mathf.Max(hidersRemaining, 0);
        hud.SetInfoText($"Time: {timeRemaining:F0}s   Hiders left: {hidersLeft}");
    }

    private void EndRound()
    {
        roundOver = true;
        // Determine if player (index 0) won
        var local = players.Count > 0 ? players[0] : null;
        bool playerWon = false;
        if (local != null)
        {
            if (local.role == PlayerRole.Seeker)
            {
                // Player seeker wins if all hiders found before time
                playerWon = (hidersRemaining <= 0);
            }
            else
            {
                // Player hider wins if time runs out and they were not found
                playerWon = (timeRemaining <= 0f);
            }
        }
        if (hud != null)
        {
            hud.ShowResult(playerWon ? "You won!" : "You lost!");
        }
        // Award flakes for winning
        if (playerWon)
        {
            CurrencyManager.AddFlakes(50);
        }
        CurrencyManager.Save();
    }
}
