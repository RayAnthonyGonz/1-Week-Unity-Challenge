using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public GameObject slotHolder;
    [SerializeField] private SlotClass[] startingItems;
    [SerializeField] private SlotClass[] items;

    public int toAdd;
    public ItemClass itemToAdd;
    public int toRemove; 
    public ItemClass itemToRemove;
    
    [SerializeField] private GameObject[] slots;
    [SerializeField] private SlotClass movingSlot;
    [SerializeField] private SlotClass tempSlot;
    [SerializeField] private SlotClass originalSlot;
    [SerializeField] private bool isMovingItem;
    [SerializeField] private GameObject itemCursor;

    [SerializeField] private GameObject hotbarSlotHolder;
    [SerializeField] private GameObject[] hotbarSlots;
    [SerializeField] private int selectedSlotIndex;
    [SerializeField] private GameObject hotbarSelector;
    public ItemClass selectedItem;

    private void Start()
    {
        //inventory
        slots = new GameObject[slotHolder.transform.childCount];
        items = new SlotClass[slots.Length];
        //hotbar
        hotbarSlots = new GameObject[hotbarSlotHolder.transform.childCount];
        for (int i = 0; i < hotbarSlots.Length; i++)
        {
            hotbarSlots[i] = hotbarSlotHolder.transform.GetChild(i).gameObject;
        }
        for (int i = 0; i < items.Length; i++) //initialize all slots
        {
            items[i] = new SlotClass();
        }
        
        for (int i = 0; i < startingItems.Length; i++) //initialize all slots
        {
            items[i] = startingItems[i];
        }

        for (int i = 0; i < slotHolder.transform.childCount; i++)
        {
            slots[i] = slotHolder.transform.GetChild(i).gameObject;
        }
        RefreshInventoryUI();
        // Add(itemToAdd, toAdd);
        // Remove(itemToRemove, toRemove);
    }

    private void Update()
    {
        itemCursor.SetActive(isMovingItem);
        itemCursor.transform.position = Input.mousePosition;

        //Debug.Log("Moving item: " + isMovingItem);
        if(isMovingItem){
            //Debug.Log(movingSlot.GetItem().itemName);
            itemCursor.transform.GetChild(0).GetComponent<Image>().sprite = movingSlot.GetItem().itemSprite;
            itemCursor.transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = movingSlot.GetQuantity().ToString();
            //itemCursor.transform.GetChild(0).transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = movingSlot.GetItem().itemName;
        }

        if(Input.GetMouseButtonDown(0)){
            if(!isMovingItem){
                BeginItemMove();
            }else{
                EndItemMove();
            }
            //Debug.Log(Input.mousePosition);
        }else{
            if(Input.GetMouseButtonDown(1)){
                if(!isMovingItem){
                    BeginItemMove_Half();
                }else{
                    EndItemMove_Single();
                }
            }
        }

        if (Input.GetAxis("Mouse ScrollWheel") != 0){
            if(Input.GetAxis("Mouse ScrollWheel") > 0){
                selectedSlotIndex = Mathf.Clamp(selectedSlotIndex - 1 , -1, hotbarSlots.Length );
                
            }else{
                selectedSlotIndex = Mathf.Clamp(selectedSlotIndex + 1 , -1, hotbarSlots.Length);
            }
        }
        if (selectedSlotIndex < 0) { selectedSlotIndex = hotbarSlots.Length - 1; }
        if (selectedSlotIndex > hotbarSlots.Length - 1) { selectedSlotIndex = 0; }

        hotbarSelector.transform.position = hotbarSlots[selectedSlotIndex].transform.position;
        selectedItem = items[selectedSlotIndex].GetItem();
    }
    #region Inventory
    public void RefreshInventoryUI(){
        for (int i = 0; i < slots.Length; i++)
        {
            try{
                slots[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
                slots[i].transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().enabled = true;
                slots[i].transform.GetChild(0).GetChild(1 ).GetComponent<TextMeshProUGUI>().enabled = true;
                slots[i].transform.GetChild(0).GetComponent<Image>().sprite = items[i].GetItem().itemSprite;
                if (items[i].GetItem() is ToolClass){
                    slots[i].transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().enabled = false;
                }else{
                    slots[i].transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = items[i].GetQuantity().ToString();}
                slots[i].transform.GetChild(0).GetChild(1 ).GetComponent<TextMeshProUGUI>().text = items[i].GetItem().itemName;
            }
            catch{
                slots[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
                slots[i].transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().enabled = false;
                slots[i].transform.GetChild(0).GetChild(1 ).GetComponent<TextMeshProUGUI>().enabled = false;
            }
        }
        RefreshHotbar();
    } 

    public void RefreshHotbar(){
        for (int i = 0; i < hotbarSlots.Length; i++)
        {
            try{
                hotbarSlots[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
                hotbarSlots[i].transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().enabled = true;
                hotbarSlots[i].transform.GetChild(0).GetChild(1 ).GetComponent<TextMeshProUGUI>().enabled = true;
                hotbarSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = items[i].GetItem().itemSprite;
                if (items[i].GetItem() is ToolClass){
                    hotbarSlots[i].transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().enabled = false;
                }else{
                    hotbarSlots[i].transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = items[i].GetQuantity().ToString();}
                hotbarSlots[i].transform.GetChild(0).GetChild(1 ).GetComponent<TextMeshProUGUI>().text = items[i].GetItem().itemName;
            }
            catch{
                hotbarSlots[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
                hotbarSlots[i].transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().enabled = false;
                hotbarSlots[i].transform.GetChild(0).GetChild(1 ).GetComponent<TextMeshProUGUI>().enabled = false;
            }
        }
    }

    public bool Add(ItemClass item, int toAdd){
        while (toAdd > 0)
        {
            SlotClass slot = Contains(item);
            //Debug.Log("Found in slot: " + slot);
            if(slot != null && slot.GetItem().isStackable){
                slot.AddQuantity(1);
                //Debug.Log("adding to quantity");
            }else{
                for (int i = 0; i < items.Length; i++)
                {
                    if(items[i].GetItem() == null){
                        items[i].AddNewItem(item, 1);
                        //Debug.Log("adding new item");
                        break;
                    }
                }
            }
            toAdd--;
            
        }
        
        RefreshInventoryUI();
        return true;
    }

    public bool Remove(ItemClass item, int toRemove){
        SlotClass temp = Contains(item);
        if(temp != null){
            if(toRemove < temp.GetQuantity()){
                temp.RemoveQuantity(toRemove);
            }else{
                if(toRemove > temp.GetQuantity()){
                    toRemove -= temp.GetQuantity();
                    temp.RemoveQuantity(temp.GetQuantity());
                    Debug.Log(toRemove);
                    Remove(item, toRemove);
                }else{
                    int slotToRemoveIndex = 0;
                    for (int i = 0; i < items.Length; i++)
                    {
                        if(items[i].GetItem() == item){
                            slotToRemoveIndex = i;
                            break;
                        }
                    }
                    items[slotToRemoveIndex].ClearValues();
                }
                
            }
        }else{
            return false;
        }    
        RefreshInventoryUI();
        return true;
    }

    public void ConsumeSelected(){
        items[selectedSlotIndex].RemoveQuantity(1);
        RefreshInventoryUI();
    }

    public SlotClass Contains(ItemClass item){
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].GetItem() == item ){
                return items[i];
            }
        }
        
        return null;
    }

    public int inInventory(ItemClass item){
        int count = 0;
        for (int i = 0; i < slots.Length; i++)
        {
            if (items[i].GetItem() == item){
                count += items[i].GetQuantity();
            }
        }
        //Debug.Log(count);
        return count;
    }

    #endregion
    
    #region MouseDrag
    
    private bool BeginItemMove(){
        
        originalSlot = GetClosestSlot();
        if (originalSlot == null || originalSlot.GetItem() == null){
            return false;
        }

        movingSlot = new SlotClass(originalSlot.GetItem(), originalSlot.GetQuantity());
        originalSlot.ClearValues();
        RefreshInventoryUI();
        isMovingItem = true;
        return true;
    }

    private bool BeginItemMove_Half(){
        originalSlot = GetClosestSlot();
        if(originalSlot.GetItem().GetTool() == true){
            BeginItemMove();
        }else{
            if (originalSlot == null || originalSlot.GetItem() == null){
                return false;
            }
            if (originalSlot.GetQuantity() == 1){
                movingSlot = new SlotClass(originalSlot.GetItem(), 1);
                originalSlot.ClearValues();
            }else{
                movingSlot = new SlotClass(originalSlot.GetItem(), Mathf.CeilToInt(originalSlot.GetQuantity()/2));
                originalSlot.RemoveQuantity(Mathf.CeilToInt(originalSlot.GetQuantity()/2));
            }

            if(originalSlot.GetQuantity() ==  0){
                originalSlot.ClearValues();
            }
        }

        RefreshInventoryUI();
        isMovingItem = true;
        return true;
    }
    
    private bool EndItemMove(){
        originalSlot = GetClosestSlot();
        //Debug.Log(originalSlot.GetItem().itemName + " " + originalSlot.GetQuantity());
        if (originalSlot == null)
        {
            Add(movingSlot.GetItem(), movingSlot.GetQuantity());
            movingSlot.ClearValues();
        }
        else
        {
            if(originalSlot.GetItem() != null)
            {
                if(originalSlot.GetItem() == movingSlot.GetItem())
                {
                    if (originalSlot.GetItem().isStackable)
                    {
                        originalSlot.AddQuantity(movingSlot.GetQuantity());
                        movingSlot.ClearValues();
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    tempSlot = new SlotClass(originalSlot.GetItem(), originalSlot.GetQuantity());
                        //Debug.Log("place original in temp");
                    originalSlot.AddNewItem(movingSlot.GetItem(), movingSlot.GetQuantity());
                        //Debug.Log("place moving in original");
                    movingSlot.AddNewItem(tempSlot.GetItem(), tempSlot.GetQuantity());
                        //Debug.Log("place temp in moving");
                    RefreshInventoryUI();
                    return true;
                }
            }
            else
            {
                originalSlot.AddNewItem(movingSlot.GetItem(), movingSlot.GetQuantity());
                movingSlot.ClearValues();
            }
        }

        RefreshInventoryUI();
        isMovingItem = false;
        return true;

    }

    private bool EndItemMove_Single(){

        originalSlot = GetClosestSlot();
        if (originalSlot == null){
            return false;
        }
        if (originalSlot.GetItem() != null && originalSlot.GetItem() != movingSlot.GetItem()){
            return false;
        }

        movingSlot.RemoveQuantity(1);

        if (originalSlot.GetItem() != null && originalSlot.GetItem() == movingSlot.GetItem()){
            originalSlot.AddQuantity(1);
        }else{
            originalSlot.AddNewItem(movingSlot.GetItem(), 1);
        }

        if (movingSlot.GetQuantity() < 1){
            isMovingItem = false;
            movingSlot.ClearValues();
        }else{
            isMovingItem = true;
        }
        RefreshInventoryUI();
        return true;
    }
    private SlotClass GetClosestSlot(){
        //Debug.Log(Input.mousePosition);
        for (int i = 0; i < slots.Length; i++)
        {
            //Debug.Log(Vector2.Distance(slots[i].transform.position, Input.mousePosition));
            if (Vector2.Distance(slots[i].transform.position, Input.mousePosition) <= 45){
                
                return items[i];
            }   
        }
        return null;
    }
    #endregion
}
