using System.Collections;
using UnityEngine;

public abstract class ItemClass : ScriptableObject
{
    public string itemName;
    public Sprite itemSprite;
    public bool isStackable;
    public virtual void Use(PlayerScript caller) { Debug.Log("Using "+ this + " item" ); }
    public virtual void Undo(PlayerScript caller) { }  
    public virtual ItemClass GetItem() { return this; }
    public virtual ToolClass GetTool() { return null; }
    public virtual MiscClass GetMisc() { return null; }  
}
