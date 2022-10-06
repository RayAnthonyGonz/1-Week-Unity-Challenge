using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "Quests/Collecting")]
public class CollectingClass : QuestClass
{
    [Header("CollectingQuest")]
    public int required;
    public ItemClass collectable; 

    public override void Evaluate(PlayerScript caller){
        if (caller.inventory.inInventory(collectable) >= required) {
            this.Complete(caller, true);
        }else{
            this.Complete(caller, false);
        }

        //Debug.Log("Collecting" + collectable);
    } 
    public override CollectingClass GetCollect() { return this; }  
}