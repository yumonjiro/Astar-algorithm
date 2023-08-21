using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveTimeSystem : MonoBehaviour
{
    private GameManager gameManager;
    private StateManager stateManager;
    public List<Unit> unitList = new();

    

    // Start is called before the first frame update
    void Start()
    {
        
    }
    void SpeedUpdate(Unit unit)
    {
        unit.speed = 0;
        // stateManager.NextTurn(unit);
        return;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
