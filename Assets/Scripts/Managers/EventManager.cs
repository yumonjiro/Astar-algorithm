using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public UnitManager unitManager;
    public Queue<Unit> unitTurnQueue; 
    
    public void Awake()
    {
        unitManager = GameObject.Find("UnitManager").GetComponent<UnitManager>();
        unitTurnQueue = new();
    }
    public void InitializeUnitOrder()
    {
        foreach(Unit u in unitManager.units)
        {
            Debug.Log(u.name + " " + u.speed);
            unitTurnQueue.Enqueue(u);
        }
    }
    public void UnitEnqueue(Unit unit)
    {
        if(!unitTurnQueue.Contains(unit))
        {
            unitTurnQueue.Enqueue(unit);
        }
        
    }

    public Unit GetNextUnit()
    {
        Unit unit = unitTurnQueue.Dequeue();
        if(unitTurnQueue.Count == 0)
        {
            InitializeUnitOrder();
        }
        return unit;
        
    }
}
