using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New MiscClass" , menuName = "Inventory Data/Misc")]
public class MiscClass : ItemClass
{
    //[Header("Misc")]

    
    public override MiscClass GetMisc() {return this;}
    public override void Use(PlayerScript caller) { 
        caller.inventory.ConsumeSelected(); // remove item from
    }
}
