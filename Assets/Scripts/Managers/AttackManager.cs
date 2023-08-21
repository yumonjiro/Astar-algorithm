using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor.PackageManager;
using Unity.VisualScripting.Antlr3.Runtime;

//TODO 
public class AttackManager : MonoBehaviour
{
    public bool isAttackPerformed;
    public Animator animator;
    public TileManager tileManager;
    void Start()
    {
        tileManager = GameObject.Find("TileManager").GetComponent<TileManager>();
    }
    public void PerformAttack(Unit unit, int attackId)
    {
        Tile attackTile = tileManager.tileSelected;
        Attack attack = new(attackId);
        // Define Attack Range
        // switch(unit.direction)
        // {
        //     case Unit.Direction.pX:
        //     break;
        //     case Unit.Direction.pZ:
        //     attack.attackArea = attack.attackArea.Select(pos => new Vector3Int(-pos.z, pos.y, pos.x)).ToList();

        //     break;
        //     case Unit.Direction.mX:
        //     attack.attackArea = attack.attackArea.Select(pos => new Vector3Int(-pos.x, pos.y, -pos.z)).ToList();

        //     break;
        //     case Unit.Direction.mZ:
        //     attack.attackArea = attack.attackArea.Select(pos => new Vector3Int(pos.z, pos.y, -pos.x)).ToList();

        //     break;
        //     default:
        //     Debug.LogWarning("" + unit.direction);
        //     break;
        // }
        Vector3 dirVector = (attackTile.transform.position - tileManager.level[unit.tilePosition].position).normalized;
        unit.unitBody.right = dirVector;
        animator = unit.animator;
        
        // foreach(var pos in attack.attackArea)
        // {
        //     Vector3Int attackPos = unit.tilePosition + pos;
        //     if(tileManager.level.ContainsKey(attackPos))
        //     {
                
        //         Tile tile = tileManager.level[attackPos];
        //         Debug.Log($"Attack on tile {tile.position}");
        //         if(tile.UnitOn != null)
        //         {
        //             Debug.Log($"Attack to {unit.name} on {tile.position}");
        //             tile.UnitOn.TakeDamage(10);
        //         }
        //     }
        // }
        Debug.Log($"Attacking on {attackTile.position}");
        if(attackTile.UnitOn != null)
        {
            Debug.Log($"Attacking to {attackTile.UnitOn.name}");
            attackTile.UnitOn.TakeDamage(30);
        }
        animator.Play("Attack");
        
        
    }
    public bool CheckAttackAnimCompletion()
    {
        // TODO 謎すぎる
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {

            return true;
        }
        return false;
    }
        
    // Update is called once per frame
    void Update()
    {
        
    }
}
