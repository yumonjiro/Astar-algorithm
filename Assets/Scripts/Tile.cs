using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector3Int position;
    public TileManager tileManager;
    public Unit UnitOn = null;
    // Sub variables
    public GameObject _hoverHighlight;
    public GameObject _reachableHighlight;

    // Start is called before the first frame update
    void Start()
    {
        this._hoverHighlight.SetActive(false);
        // Find tile manager reference and add self to tile list
        tileManager = GameObject.Find("TileManager").GetComponent<TileManager>();
        this.position = Vector3Int.FloorToInt(transform.localPosition);
        tileManager.AddTile(this.position, this);
    }

    // On Mouse hover
    public void OnMouseEnter()
    {
        this._hoverHighlight.SetActive(true);
        tileManager.HoverTile(this);
    }

    public void OnMouseDown()
    {
        tileManager.NotifyTileSelected(this);
    }
    public void OnMouseExit()
    {
        this._hoverHighlight.SetActive(false);
    }
}
