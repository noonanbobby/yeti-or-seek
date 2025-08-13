using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HideSeekManager : MonoBehaviour {
    public static HideSeekManager Instance { get; private set; }

    [Header("Round")]
    public float roundDuration = 90f;          // total time
    public float hidePhaseSeconds = 20f;       // time to hide first
    public int   startSeekerCount = 1;

    List<YetiMotor> players = new();
    HUDController hud;
    float timeLeft;
    enum State { Lobby, Hide, Seek, Results }
    State state;

    void Awake(){ if(Instance && Instance!=this){ Destroy(gameObject); return; } Instance=this; }

    public void Setup(List<YetiMotor> list, HUDController hudCtrl){
        players = list; hud = hudCtrl;
        AssignRoles();
        StopAllCoroutines();
        StartCoroutine(RunRound());
    }

    void AssignRoles(){
        foreach (var p in players) p.role = PlayerRole.Hider;
        int seekers = Mathf.Clamp(startSeekerCount, 0, Mathf.Max(0, players.Count-1));
        var shuffled = new List<YetiMotor>(players);
        for (int i=0;i<seekers;i++){
            var s = shuffled[i];
            s.BecomeSeeker();
        }
        UpdateHUD();
    }

    IEnumerator RunRound(){
        state = State.Hide;
        timeLeft = hidePhaseSeconds;
        hud?.SetObjective("HIDE!");
        while (timeLeft > 0){ timeLeft -= Time.deltaTime; hud?.SetTimer(Mathf.CeilToInt(timeLeft)); yield return null; }

        state = State.Seek;
        timeLeft = Mathf.Max(5f, roundDuration - hidePhaseSeconds);
        hud?.SetObjective("SEEK!");
        while (timeLeft > 0){
            timeLeft -= Time.deltaTime;
            hud?.SetTimer(Mathf.CeilToInt(timeLeft));
            if (CountHiders() == 0) break;
            yield return null;
        }

        state = State.Results;
        var winTxt = CountHiders()==0 ? "Seekers Win!" : "Hiders Win!";
        hud?.SetObjective(winTxt);
        hud?.SetTimer(0);
        yield return new WaitForSeconds(3f);

        // reset roles for replay in editor
        AssignRoles();
        StartCoroutine(RunRound());
    }

    void UpdateHUD(){
        hud?.SetHiders(CountHiders());
    }

    int CountHiders()=> players.Count(p=>p.role==PlayerRole.Hider && !p.IsHidden);

    public void NotifyFound(YetiMotor who){
        UpdateHUD();
    }
}
