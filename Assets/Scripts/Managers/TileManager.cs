using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.Threading.Tasks;
public class TileManager : MonoBehaviour
{
    public GameManager gameManager;
    public Dictionary<Vector3Int, Tile> level;

    /// <summary>
    /// TileManagerは全てのタイルの状態の管理を行う。
    /// 
    /// </summary>
    
    public Tile tileHovered;
    public Tile tileSelected;
    
    private bool isTileSelected;
    
    private AStar aStar;

    // tiles that shows reach of unit movement
    public List<Tile> unitReach;
    public List<Tile> attackReach;

    void Awake()
    {
        level = new();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        aStar = new(this);
    }

    #region Basic functions
    public void AddTile(Vector3Int position, Tile tile)
    {
        this.level.Add(position ,tile);
        return;
    }
    public void HoverTile(Tile tile)
    {
        this.tileHovered = tile;
        return;
    }
    public async Task<Tile> SelectTile()
    {
        await TileSelectedCheck();
        return this.tileSelected;
    }
    private Task TileSelectedCheck()
    {
        while(true)
        {
            if(isTileSelected)
            {
                isTileSelected = false;
            }
            
        }
    }
    public void NotifyTileSelected(Tile tile)
    {
        this.tileSelected = tile;
        this.isTileSelected = true;
        gameManager.NotifyTileSelected();
    }
    public void TileUnHovered()
    {
        return;
    }
    #endregion

    #region Functions for showing reachable tile

    public void SetReachableTiles(Unit unit)
    {
        List<Vector3Int> searchRange = new();
        foreach(var i in unit.moveRange())
        {
            if(level.ContainsKey(i))
            {
                //Debug.Log($"{i} is added to search range");
                searchRange.Add(i);
            }
        }

        // TODO Definition of movePower
        foreach (Vector3Int pos in searchRange)
        {
            var path = aStar.AStarSearch(unit.tilePosition, pos, 1000);
            if(path.Count == 0)
            {
                continue;
            }
            if(aStar.costSoFar[pos] <= unit.movePower)
            {
                Tile reachTile = level.GetValueOrDefault(pos);
                if(reachTile.UnitOn == null)
                {
                    unitReach.Add(reachTile);
                }
                
            }
            
        }
    }
    public void SetAttackaleTiles(Unit unit, Attack attack)
    {
        List<Vector3Int> areaCheck = new();
        areaCheck.AddRange(attack.attackArea);
        areaCheck.AddRange(attack.attackArea.Select(pos => new Vector3Int(-pos.z, pos.y, pos.x)).ToList());

        areaCheck.AddRange(attack.attackArea.Select(pos => new Vector3Int(-pos.x, pos.y, -pos.z)).ToList());
        
        areaCheck.AddRange(attack.attackArea.Select(pos => new Vector3Int(pos.z, pos.y, -pos.x)).ToList());
        areaCheck = areaCheck.Distinct().ToList();
        foreach(var pos in areaCheck)
        {
            if(level.ContainsKey(unit.tilePosition + pos) && !level.ContainsKey(unit.tilePosition + pos + new Vector3Int(0, 1, 0)))
            {
                attackReach.Add(level[unit.tilePosition + pos]);
            }
        }
    }
    public void ShowAttackableTiles()
    {
        foreach(Tile tile in attackReach)
        {
            tile._reachableHighlight.SetActive(true);
        }
    }
    public void HideAttackableTiles()
    {
        foreach(Tile tile in attackReach)
        {
            tile._reachableHighlight.SetActive(false);
        }
    }
    public void ResetAttackableTiles()
    {
        foreach(var tile in attackReach)
        {
            tile._reachableHighlight.SetActive(false);
        }
        attackReach = new();
    }
    public void ShowReachableTiles()
    {
        foreach(Tile tile in unitReach)
        {
                tile._reachableHighlight.SetActive(true);
        }
    }
    
    public void HideReachableTiles()
    {
        foreach(Tile tile in unitReach)
        {
                tile._reachableHighlight.SetActive(false);
        }
    }
    public void ResetReachableTiles()
    {
        foreach(var tile in unitReach)
        {
            tile._reachableHighlight.SetActive(false);
        }
        unitReach = new();
    }
    #endregion

    #region Functions for AStar path finding
    public List<Vector3Int> FindPath(Vector3Int start, Vector3Int goal)
    {
        return aStar.AStarSearch(start, goal, 100);
    }

    public double Cost(Vector3Int p1, Vector3Int p2)
    {
        return 1;
    }
    
    public List<Vector3Int> Neighbors(Vector3Int position)
    {
        List<Vector3Int> dirPlane = new List<Vector3Int>() {
            new Vector3Int(1,0, 0),new Vector3Int(-1,0, 0),new Vector3Int(0,0, 1),new Vector3Int(0,0, -1)};
        List<Vector3Int> neighbors = new List<Vector3Int>();

        foreach(Vector3Int dir in dirPlane)
        {
            Vector3Int neighborPlace = position + dir;
            // xz座標が等しく、y座標が異なるマスを検索する
            for (int y = 0; y < 3; y++)
            {
                Vector3Int p = neighborPlace;
                p.y -= y;
                if (level.ContainsKey(p) && Passable(position, p))
                {
                    neighbors.Add(p);
                }
                p.y += 2 * y;
                if (level.ContainsKey(p) && Passable(position, p))
                {
                    neighbors.Add(p);
                }
            }
        }

        return neighbors;
    }

    // Check if unit can pass through between p1 and p2
    public bool Passable(Vector3Int p1, Vector3Int p2)
    {
        //p1, p2はx,zざ表の差の合計が１となっている。y座標は０〜２離れている。
        //まず、真上と真下は除外（真上が引数に来ることは多分ないけど）
        if(p1.x == p2.x && p1.z == p2.z && (int)Math.Abs(p1.y - p2.y) == 1)
        {
            return false;
        }
        //目的地の上にタイルがあったらだめ
        if(level.ContainsKey(p2 + new Vector3Int(0, 1, 0)))
        {
            return false;
        }
        //斜め移動の場合、途中に障害物がないか確認
        if((p2-p1).magnitude > 1.1)
        {
            //斜め上
            //自分の上にタイルがあったら辿り着けない
            if((p2.y - p1.y) > 0)
            {
                int height = (int)(p2.y - p1.y);
                for(int i = 0; i < height; i++)
                {
                    if(level.ContainsKey(p1 + new Vector3Int(0, i+2, 0)))
                    {
                        return false;
                    }
                }
                
            }
            //斜め下
            //x,z座標の差はどちらかが1,どちらかが0であることを利用して、目的地のタイルの２つ上から出発地点の高さの一つ上までの高さにタイルがないかを調べる
            //p2.y +2 ~ p1.y+1
            if((p1.y - p2.y) > 0)
            {
                int height = (int)(p1.y - p2.y);
                for(int i = 0; i < height; i++)
                {
                    if(level.ContainsKey(p2 + new Vector3Int(0, 2 + i, 0)))
                    {
                        return false;
                    }
                }
            }
        }
        //TODO 判定するアルゴリズム実装
        return true;
    }

    // return manhattan distance of two position
    public double Heuristic(Vector3Int a, Vector3Int b)
    {
        
        return Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y) + Math.Abs(a.z - b.z);
    }
    #endregion
}
