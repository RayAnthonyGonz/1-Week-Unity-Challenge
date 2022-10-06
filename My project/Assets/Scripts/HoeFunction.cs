using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using System.Linq;

[System.Serializable]
public class HoeFunction : MonoBehaviour
{
    [SerializeField] private GridLayout gl;
    [SerializeField] private Tilemap affectingGrid;
    [SerializeField] private Tilemap walkableGrid;
    [SerializeField] private Tilemap mushroomGrid;
    [SerializeField] private PlayerScript player;
    public Tile[] mushrooms;
    [SerializeField] private Tilemap foliageGrid;
    [SerializeField] private Tilemap[] blockingGrids;
    [SerializeField] private RuleTile dirt;
    [SerializeField] private GameObject Selector;
    
    public bool tilling;
    private TileData tile;
    private Vector3Int mouseGridLocation;
    private Vector3 location;
    public float xOffset;
    public float yOffset;
    public float offsetDivisor;
    private bool tillable;
    
    public MiscClass mushroomItem;

    public void setNewShroomGrid(Tilemap newShrooms){
        Destroy(mushroomGrid.gameObject);
        mushroomGrid = Instantiate(newShrooms, gl.transform) as Tilemap;
    }

    public bool Til(InventoryManager inventory){
        
        for (int i = 0; i < blockingGrids.Length; i++)
        {
            if(mushrooms.Contains(mushroomGrid.GetTile(gl.WorldToCell(location))))
            {
                if(mushroomGrid.GetTile(gl.WorldToCell(location)) && tillable){
                    mushroomGrid.SetTile(gl.WorldToCell(location), null);
                    inventory.Add(mushroomItem,1);
                }
                if(player.direction == "Up"){ player.animator.Play("TillUp");}
                if(player.direction == "Down"){ player.animator.Play("Till");}
                if(player.direction == "Left"){ player.animator.Play("TillLeft");}
                if(player.direction == "Right"){ player.animator.Play("TillRight");}
            }
            // else
            // {
            //     if(!affectingGrid.GetTile(gl.WorldToCell(location)) && tillable){
            //         affectingGrid.SetTile(gl.WorldToCell(location), dirt);
            //     }
            // }
        }
        //tillable = true;
        return true;
    }

    public bool UnTil(){
        //Debug.Log("Untilling");
        for (int i = 0; i < blockingGrids.Length; i++)
        {
            //if(blockingGrids[i].GetTile(gl.WorldToCell(location))){tillable = false;}
            if(!affectingGrid.GetTile(gl.WorldToCell(location)) && tillable){
                
                affectingGrid.SetTile(gl.WorldToCell(location), null);
            }
        }
        //tillable = true;
        return true;
    }
    void Update()
    {
        tillable = true;
        //if(!walkableGrid.GetTile(gl.WorldToCell(location))){tillable = false;}
        // if(blockingGrids[0].GetTile(gl.WorldToCell(location))){tillable = false;}
        // if(blockingGrids[1].GetTile(gl.WorldToCell(location))){tillable = false;}
        // if(blockingGrids[2].GetTile(gl.WorldToCell(location))){tillable = false;}
        if(!mushroomGrid.GetTile(gl.WorldToCell(location))){tillable = false;}
        if(foliageGrid.GetTile(gl.WorldToCell(location))){tillable = false;}

        xOffset = GameObject.Find("Character").transform.position.x/offsetDivisor;
        yOffset = GameObject.Find("Character").transform.position.y/offsetDivisor;

        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        location = new Vector3(pos.x, pos.y, 0);
        Selector.SetActive(tilling);
        
        if(tillable){
            Selector.transform.GetChild(0).GetComponent<Image>().color = Color.green;
        }else{
            Selector.transform.GetChild(0).GetComponent<Image>().color = Color.red;
        }
        //Cursor.visible = !tilling;
        Selector.transform.position = Camera.main.WorldToScreenPoint(affectingGrid.GetCellCenterWorld(gl.WorldToCell(location))) - new Vector3(xOffset,yOffset,0);
    }

}
