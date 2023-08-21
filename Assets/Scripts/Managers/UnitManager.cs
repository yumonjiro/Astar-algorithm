using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class UnitManager : MonoBehaviour
{
    public List<Unit> units;
    public TileManager tileManager;

    //TODO refactoring
    public bool isUnitMoved = true;
    public void Initialize()
    {
        if(units.Count > 0)
        {
            Debug.Log($"There is {units.Count} units");
        }
        else
        {
            Debug.Log("There is no units");
        }
    }
    public void Awake()
    {
        units = new();
        tileManager = GameObject.Find("TileManager").GetComponent<TileManager>();
    }
    public void CollectUnit()
    {
        var unitsC = GameObject.FindGameObjectsWithTag("Unit");
        foreach(var unit in unitsC)
        {
            var u = unit.GetComponent<Unit>();
            this.AddUnit(u);
            tileManager.level[u.tilePosition].UnitOn = u;
        }
    }
    public void AddUnit(Unit unit)
    {
        Debug.Log($"Unit {unit.name} Added to units");
        units.Add(unit);
        Debug.Log($"Unit Count is {units.Count}");
    }

    public void UpdateChargeTime()
    {
        foreach (Unit unit in units)
        {
            unit.IncrementChargeTime();
        }
    }

    public void MoveUnit(Unit unit, List<Vector3Int> path)
    {
        isUnitMoved = false;
        Debug.Log($"Moving active unit from {unit.tilePosition} to {path[path.Count -1]}");
        StartCoroutine(Step(unit, path));
        Debug.Log("Unit moved");
    }
    IEnumerator Step(Unit unit, List<Vector3Int> path)
    {
        
        int pathIndex = 0;
        Vector3 dir = path[pathIndex] - unit.transform.position;
        Vector3Int dirInt = Vector3Int.RoundToInt(dir);
        
        Debug.Log(dirInt);
        if(dirInt.x > 0)
        {
            unit.direction = Unit.Direction.pX;
        }
        else if(dirInt.x < 0)
        {
            unit.direction = Unit.Direction.mX;
        }
        else if(dirInt.x == 0)
        {
            if(dirInt.z > 0)
            {
                unit.direction = Unit.Direction.pZ;
            }
            else
            {
                unit.direction = Unit.Direction.mZ;
            }
        }
        while(true)
        {
            dir = path[pathIndex] - unit.transform.position;
            if(dir.magnitude < 0.1)
            {
                if(tileManager.level[unit.tilePosition].UnitOn == unit)
                {
                    tileManager.level[unit.tilePosition].UnitOn = null;
                }
                
                //tile position update
                unit.tilePosition = path[pathIndex];
                if(tileManager.level[unit.tilePosition].UnitOn == null)
                {
                    tileManager.level[unit.tilePosition].UnitOn = unit;
                }
                
                Debug.Log($"{tileManager.level[unit.tilePosition].UnitOn.name} is on tile {unit.tilePosition}");
                if(pathIndex == path.Count -1)
                {
                    
                    Debug.Log("Reached destination");
                
                    isUnitMoved = true;
                    yield break;
                }
                else
                {
                    
                    Debug.Log($"Active unit reached point {path[pathIndex]}");
                    pathIndex += 1;
                    dirInt = Vector3Int.RoundToInt(path[pathIndex] - unit.transform.position);
                    Debug.Log(dirInt);
                    if(dirInt.x > 0)
                    {
                        unit.direction = Unit.Direction.pX;
                    }
                    else if(dirInt.x < 0)
                    {
                        unit.direction = Unit.Direction.mX;
                    }
                    else if(dirInt.x == 0)
                    {
                        if(dirInt.z > 0)
                        {
                            unit.direction = Unit.Direction.pZ;
                        }
                        else
                        {
                            unit.direction = Unit.Direction.mZ;
                        }
                    }
                }
            }
            
            unit.transform.position += unit.moveSpeed * Time.deltaTime * dir.normalized;
            yield return null;
        }
    }
}
