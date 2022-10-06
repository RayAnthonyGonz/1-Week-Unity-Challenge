using System.Collections;
using UnityEngine;

[System.Serializable]
public class SlotClass
{
    [SerializeField] private ItemClass item;
    [SerializeField] private int quantity;
    
    public SlotClass(){
        item = null;
        quantity = 0;
    }

    public SlotClass(ItemClass _item, int _quantity){
        item = _item;
        quantity = _quantity;
    }
    public void ClearValues(){
        this.item = null;
        this.quantity = 0;
    }
    public ItemClass GetItem(){return item;}
    public int GetQuantity(){return quantity;}
    public void AddQuantity(int _quantity ){ quantity += _quantity;}
    public void RemoveQuantity(int _quantity ){
        quantity -= _quantity;
            if(quantity <= 0){
                ClearValues();
            }
        }
    public void AddNewItem(ItemClass _item, int _quantity ){ this.item = _item; this.quantity = _quantity; }
    public void RemoveItem(ItemClass _item, int _quantity ){ this.item = _item; this.quantity = _quantity; }
    
}