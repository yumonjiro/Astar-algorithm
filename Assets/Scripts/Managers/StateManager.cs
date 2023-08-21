using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This Class manages game state.
/// </summary>

public class StateManager : MonoBehaviour
{
    public GamePhase gamePhase;
    public GameManager gameManager;
    public ActionPhase actionPhase;
    public void ChangeActionPhase(ActionPhase actionphase)
    {
        this.actionPhase = actionphase;
        gameManager.NotifyActionPhaseChange();
    }
    public Unit activeUnit;

    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public enum GamePhase
    {
        InitialPhase,
        PreparePhase,
        PerformePhase,
        AfterPerform,
    }
    public enum ActionPhase
    {
        // Show Action box
        Default,
        // select tile to move
        Move,
        // select action
        Action,
    }
    // Which input ui is wating
    
}
