using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Unit : MonoBehaviour
{
    public string unitName;
    public int unitID;
    public UnitManager unitManager;
    public EventManager eventManager;
    public LifeGauge lifeGauge;
    // variables to define unit state
    public Vector3Int tilePosition;

    public Direction _direction;
    public Direction direction
    {
        get { return _direction; }
        set 
        {
            _direction = value;
            this.unitBody.transform.eulerAngles = new Vector3(0, -90 * ((int)_direction % 4), 0);
        }
    }

    public Animator animator;
    // Status
    public int maxHP = 100;
    public int hitPoint = 100;
    // chargeTime is charged by speed every second;
    public int speed;
    public int chargeTime;
    public int moveSpeed;
    public int movePower;


    // For cinemachine
    public Transform unitBody;
    public Transform followTransform;
    void Awake()
    {
        Initialize();
        

    }
    // Start is called before the first frame update
    void OnEnable()
    {
        //unitManager.AddUnit(this);
    }
    void Start()
    {
        LifeGaugeContainer.Instance.Add(this);
    }
    void Initialize()
    {
        unitManager = GameObject.Find("UnitManager").GetComponent<UnitManager>();
        
        if(unitManager == null)
        {
            Debug.Log("UnitManager didnt find");
        }

        transform.position = tilePosition;
    }
    public void IncrementChargeTime()
    {
        this.chargeTime += speed;
        if(chargeTime >= 100)
        {
            eventManager.UnitEnqueue(this);
            this.chargeTime = 0;
        }
    }
    public void TakeDamage(int damage)
    {
        this.hitPoint -= damage;
        //ダメージアニメーションの追加
        if(hitPoint <= 0)
        {
            this.gameObject.SetActive(false);
        }
    }

    public List<Vector3Int> moveRange()
    {
        List<Vector3Int> available = new();
        for (int x = 0; x < movePower*2+1; x++)
        {
            for(int z = 0; z < movePower*2+1; z++)
            {
                for(int y = 0; y < movePower*2+1; y++)
                {
                    Vector3Int p1 = this.tilePosition + new Vector3Int(x, y, z) - new Vector3Int(movePower, movePower, movePower);
                    available.Add(p1);
                }
            }
        }


        return available;
    }
    public enum Direction
    {
        pX = 0,
        pZ = 1,
        mX = 2,
        mZ = 3,
    }
}
