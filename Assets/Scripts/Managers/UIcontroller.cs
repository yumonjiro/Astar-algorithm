using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIcontroller : MonoBehaviour
{
    public GameObject actionBox;
    public GameManager gameManager;
    public StateManager stateManager;
    public TileManager tileManager;
    public UiMode uiMode;
    public void ChangeUIMode(UiMode uimode)
    {
        this.uiMode = uimode;
        switch(uiMode)
        {
            case UiMode.SelectAction:
            {
                actionBox.SetActive(true);
                break;
            }
            case UiMode.SeleceTileToMove:
            {
                tileManager.ShowReachableTiles();
                stateManager.ChangeActionPhase(StateManager.ActionPhase.Move);

                actionBox.SetActive(false);
                break;
            }
            case UiMode.SelectTileToAction:
            {
                tileManager.SetAttackaleTiles(stateManager.activeUnit, new Attack(0));
                tileManager.ShowAttackableTiles();
                stateManager.ChangeActionPhase(StateManager.ActionPhase.Action);
                actionBox.SetActive(false);
                break;
            }
            default:
            break;
        }
    }
    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        stateManager = GameObject.Find("StateManager").GetComponent<StateManager>();
        tileManager = GameObject.Find("TileManager").GetComponent<TileManager>();
    }
    public void OnMoveButtonClick()
    {
        ChangeUIMode(UiMode.SeleceTileToMove);
    }
    public void OnAttackButtonClick(int attackId)
    {
        ChangeUIMode(UiMode.SelectTileToAction);
    }
    public void OnEndButtonClick()
    {
        gameManager.EndTurn();
    }
    public void ShowUi(Unit unit)
    {
        actionBox.SetActive(true);
    }
    public enum UiMode
    {
        SelectAction,
        SeleceTileToMove,
        SelectTileToAction,
    }
}
