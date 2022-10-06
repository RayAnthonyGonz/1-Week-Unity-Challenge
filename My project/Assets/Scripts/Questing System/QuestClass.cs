using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QuestClass : ScriptableObject
{
    // Start is called before the first frame update
    public string questName;
    public string questDesc;
    public Sprite questIcon;
    public string giver;
    [SerializeField] private bool complete;

    public int currencyReward;

    public virtual void Evaluate(PlayerScript caller){ 
        //Debug.Log("Checking Progress "+ this.questName + " " + this);
    }

    public virtual bool QuestStatus(){
        return complete;
    }

    public virtual int QuestReward(){
        return currencyReward;
    }

    public virtual void Complete(PlayerScript caller, bool status){complete = status;}

    public virtual void Reward(PlayerScript caller){
        caller.currency += currencyReward;
    }

    public virtual QuestClass GetQuest() { return this; }
    public virtual CollectingClass GetCollect() { return null; }  
}
