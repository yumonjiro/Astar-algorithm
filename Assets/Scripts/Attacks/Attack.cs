using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack
{
    public int attackId;
    //　攻撃範囲はキャラクターの正面を+X方向、真上を+Y方向とした相対座標で表している
    public List<Vector3Int> attackArea;
    public int damage;

    public Attack(int _attackId)
    {
        attackId = _attackId;
        attackArea = new();
        
        attackArea.Add(new Vector3Int(1, 0, 0));
        damage = 10;
    }
    
    
    
}
