using UnityEngine;
using System.Collections.Generic;

public class RoundManager : MonoBehaviour
{
    public YetiMotor player;
    public HUDController hud;
    public FinishLine finishLine;

    private List<BotMotor> bots = new();
    private bool roundOver = false;

    public void RegisterBot(BotMotor bot) => bots.Add(bot);

    public void RegisterFinish(FinishLine f)
    {
        finishLine = f;
        f.onReached += OnAnyFinish;
    }

    private void OnAnyFinish(GameObject who)
    {
        if (roundOver) return;
        roundOver = true;
        bool playerWon = (who == player.gameObject);
        hud.ShowResult(playerWon ? "You finished!" : "A bot finished first!");
        if (playerWon) CurrencyManager.AddFlakes(50);
        CurrencyManager.Save();
    }
}
