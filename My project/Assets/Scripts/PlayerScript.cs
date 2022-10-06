using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerScript : MonoBehaviour
{
    // Start is called before the first frame update
    //private char direction = 'D';
    public TextMeshProUGUI MoneyHolder;
    public InventoryManager inventory;
    public DialogueManager dm;
    public Animator animator; 
    public bool canMove;
    public bool inInventory = false;
    public bool inQuestMenu = false;
    public bool inDialogue = false;
    public HoeFunction pt;
    public int currency;
    public QuestHandler quests;
    
    void Start()
    {
        canMove = false;
        quests.Player(gameObject.GetComponent<PlayerScript>());
        pt = this.GetComponent<HoeFunction>();
        inventory = GameObject.Find("InventorySystem").GetComponent<InventoryManager>();
        animator = this.GetComponent<Animator>();
        // inventoryRect = inventory.slotHolder.transform.GetChild(0).GetComponent<RectTransform>();
        // questRect = quests.transform.GetChild(0).GetComponent<RectTransform>();
    }
    #region LerpValues
    public RectTransform inventoryRect;
    public RectTransform questRect;
    float lerpDuration = 0.5f;
    float startValueInven = -200;
    float startValueQuest = -300;
    float endValue = 0;
    float valueToLerp;
    public enum Axis {Horizontal, Vertical}
    public bool moving;
    public string direction;

    IEnumerator LerpIn(Axis direction)
    {
        float timeElapsed = 0;
        while (timeElapsed < lerpDuration)
        {
            //if (!moving) {moving = true;}
            if(Axis.Horizontal == direction){
                questRect.anchoredPosition = new Vector3(Mathf.SmoothStep(startValueQuest, endValue, timeElapsed / lerpDuration), 0, 0);
            }else if (Axis.Vertical == direction) {
                inventoryRect.anchoredPosition = new Vector3(0, Mathf.SmoothStep(startValueInven, endValue, timeElapsed / lerpDuration), 0);
            }
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        valueToLerp = endValue;
        moving = false;
        pt.tilling = false;
    }

    IEnumerator LerpOut(Axis direction)
    {
        float timeElapsed = 0;
        while (timeElapsed < lerpDuration)
        {
            //if (!moving) {moving = true;}
            if(Axis.Horizontal == direction){
                questRect.anchoredPosition = new Vector3(Mathf.Lerp(endValue, startValueQuest, timeElapsed / lerpDuration), 0, 0);
            }else if (Axis.Vertical == direction) {
                inventoryRect.anchoredPosition = new Vector3(0, Mathf.Lerp(endValue, startValueInven, timeElapsed / lerpDuration), 0);
            }
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        if(Axis.Horizontal == direction){
            questRect.anchoredPosition = new Vector3(startValueQuest, 0, 0);
        }else if (Axis.Vertical == direction) {
            inventoryRect.anchoredPosition = new Vector3(0, startValueInven, 0);
        }
        moving = false;
    }
    #endregion
    // Update is called once per frame
    void Update()
    {
        //MOVEMENT ANIMATIONS
        int tempX = 0;
        int tempY = 0;
        if(canMove){
            this.transform.Translate(
                (Input.GetAxis("Horizontal") * 4) * Time.deltaTime,
                (Input.GetAxis("Vertical")   * 4) * Time.deltaTime, 0);
            tempX = (int) Mathf.Round(Input.GetAxis("Horizontal") * 1000 * Time.deltaTime); 
            tempY = (int) Mathf.Round(Input.GetAxis("Vertical")   * 1000 * Time.deltaTime);
            if (Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S)
            && !Input.GetKey(KeyCode.D)){  animator.Play("WalkUp"); tempX = 0; direction = "Up";};
            if (Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.W)
            && !Input.GetKey(KeyCode.D)){  animator.Play("Walk"); tempX = 0; direction = "Down";};
            if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D)
            && !Input.GetKey(KeyCode.W)){  animator.Play("WalkLeft"); tempY = 0; direction = "Left";};
            if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.A)
            && !Input.GetKey(KeyCode.W)){  animator.Play("WalkRight"); tempY = 0; direction = "Right";};
        // }else{//Dialogue
        //     if(Input.GetKeyDown(KeyCode.E) && inDialogue){
        //         dm.NextDialogue(this);
        //     }
        }
            animator.SetInteger("Xdirection", tempX);
            animator.SetInteger("Ydirection", tempY);
        
        //money
        MoneyHolder.text = "Currency: " + currency.ToString();
        //inventory UI
        if(canMove && !moving && !inDialogue){
            if (Input.GetKeyDown(KeyCode.Space) && inInventory == false){ 
                inInventory = true;
                StartCoroutine(LerpIn(Axis.Vertical));
            }else if(Input.GetKeyDown(KeyCode.Space) && inInventory == true) {
                inInventory = false;
                StartCoroutine(LerpOut(Axis.Vertical));
            }
        }
        //quest list UI
        if(canMove && !moving){
            if (Input.GetKeyDown(KeyCode.Q) && inQuestMenu == false){ 
                inQuestMenu = true;
                quests.QuestRefresh();
                StartCoroutine(LerpIn(Axis.Horizontal));
            }else if(Input.GetKeyDown(KeyCode.Q) && inQuestMenu == true) {
                inQuestMenu = false;
                StartCoroutine(LerpOut(Axis.Horizontal));
            }
        }
        //tool usage
        if(!inInventory && inventory.selectedItem != null && !inDialogue && !inQuestMenu){
            if(inventory.selectedItem.GetTool() != null){
                if(inventory.selectedItem.GetTool().toolType == ToolClass.ToolType.hoe ){ pt.tilling = true; }
                if(Input.GetMouseButtonDown(0)){ inventory.selectedItem.Use(this); }
                //if(Input.GetMouseButton(1)){ inventory.selectedItem.Undo(this); }
            }else{ pt.tilling = false;}
            if(Input.GetMouseButtonDown(0) && inventory.selectedItem.GetMisc() != null){
                inventory.selectedItem.Use(this);
            }
            quests.QuestRefresh();
        }else{
            pt.tilling = false;
        }
    }
}
