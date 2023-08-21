using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

// TODO order manager's initialize
public class GameManager : MonoBehaviour
{
    public StateManager stateManager;
    public EventManager eventManager;
    public UnitManager unitManager;
    public TileManager tileManager;
    public UIcontroller uIcontroller;
    public CameraManager cameraManager;
    public AttackManager attackManager;
    public GameObject ui;
    /// <summary>
    /// unitがunitManagerに追加される
    /// tileがtileManagerに追加される
    /// EventManagerで最初の実行順が決められる
    /// Awakeで書くオブジェクトが必要なオブジェクトの参照を得る
    /// OnE
    /// </summary>
    void Awake()
    {
        Initialize();
    }
    // Start is called before the first frame update
    void Start()
    {
        unitManager.Initialize();

        StartGame();
    }

    void Initialize()
    {
        
        // Get references of managaer objects
        stateManager = GameObject.Find("StateManager").GetComponent<StateManager>();
        if(stateManager == null)
        {
            Debug.Log("stateManager didnt find");

        }
        eventManager = GameObject.Find("EventManager").GetComponent<EventManager>();
        if(eventManager == null)
        {
            Debug.Log("eventManager didnt find");

        }
        unitManager = GameObject.Find("UnitManager").GetComponent<UnitManager>();
        tileManager = GameObject.Find("TileManager").GetComponent<TileManager>();
        cameraManager = GameObject.Find("CameraManager").GetComponent<CameraManager>();
        uIcontroller = GameObject.Find("UI").GetComponent<UIcontroller>();
        attackManager = GameObject.Find("AttackManager").GetComponent<AttackManager>();
        

        ///<summary>
        ///
        ///この時点でレベルの初期設定と各ユニットの配置、１ターン目にアクティブになるユニットが決まっている状態
        /// 
        ///

    }
    void StartGame()
    {
        unitManager.CollectUnit();
        eventManager.InitializeUnitOrder();
        StartInitialPhase();
    }
    // Active Unit の設定、その他イベントの発生管理
    void StartInitialPhase()
    {
        stateManager.gamePhase = StateManager.GamePhase.InitialPhase;
        stateManager.activeUnit = eventManager.GetNextUnit();
        cameraManager.ChangeTarget(stateManager.activeUnit.followTransform);
        StartPreparePhase();

    }
    void StartPreparePhase()
    {
        stateManager.gamePhase = StateManager.GamePhase.PreparePhase;
        stateManager.actionPhase = StateManager.ActionPhase.Default;
        // show movable place
        uIcontroller.ChangeUIMode(UIcontroller.UiMode.SelectAction);
        uIcontroller.ShowUi(stateManager.activeUnit);
        tileManager.SetReachableTiles(stateManager.activeUnit);
        //tileManager.ShowReachableTiles();
        //move unit if notify selected is called
        //do action
    }
    public void NotifyActionPhaseChange()
    {
        // switch(stateManager.actionPhase)
        // {
        //     case StateManager.ActionPhase.Move:
        //     {

        //         break;
        //     }

        //     case default:
        //     break;
        // }
    }
    
    public void NotifyTileSelected()
    {
        if(stateManager.actionPhase == StateManager.ActionPhase.Move)
        {
            MoveUnitMode();
        }
        if(stateManager.actionPhase == StateManager.ActionPhase.Action)
        {
            AttackMode();
        }
    }
    public async void MoveUnitMode()
    {
        stateManager.gamePhase = StateManager.GamePhase.PerformePhase;
        Tile moveTile = tileManager.tileSelected;
        if(tileManager.unitReach.Contains(moveTile))
        {
            tileManager.ResetReachableTiles();
            
            unitManager.MoveUnit(stateManager.activeUnit,
            tileManager.FindPath(stateManager.activeUnit.tilePosition, moveTile.position));
            while(true)
            {
                if(unitManager.isUnitMoved)
                {
                    break;
                }
                await Task.Delay(100);
            }
            EndActionPhase();
        }
    }
    public async void AttackMode()
    {
        stateManager.gamePhase = StateManager.GamePhase.PerformePhase;
        Tile attackTile = tileManager.tileSelected;
        if(tileManager.attackReach.Contains(attackTile))
        {
            tileManager.ResetAttackableTiles();
            attackManager.PerformAttack(stateManager.activeUnit, 0);
            await Task.Delay(100);
            while(true)
            {
                if(attackManager.CheckAttackAnimCompletion())
                {
                    Debug.Log("Completed");
                    break;
                }
                await Task.Delay(100);
            }
            stateManager.activeUnit._direction = stateManager.activeUnit.direction;
            EndActionPhase();
        }
        else
        {
            Debug.Log("Select valid tile");
        }
        
    }
    public void EndActionPhase()
    {
        
        StartPreparePhase();
    }
    public void EndTurn()
    {
        tileManager.ResetReachableTiles();
        StartInitialPhase();
    }
    // public async void ChooseTileToMove()
    // {
    //     await NotifyTileSelected();
    // }
    void StartPerformPhase()
    {

    }

    void StartAfterPerformPhase()
    {

    }
}
