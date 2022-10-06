using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New ToolClass" , menuName = "Inventory Data/Tool")]
public class ToolClass : ItemClass
{
    [Header("Tool")]
    public ToolType toolType;
    public enum ToolType{
        axe, hoe
    }
    //private InventoryManager inventory;

    public override ToolClass GetTool() { return this; }
    public override void Use(PlayerScript caller) { 
        if(toolType == ToolType.hoe){
            caller.pt.Til(caller.inventory);
            //caller.inventory.Add(,1);
        }
    }
    public override void Undo(PlayerScript caller){
        if(toolType == ToolType.hoe){
            //Debug.Log("m1");
            caller.pt.UnTil();
        }
    }
}
