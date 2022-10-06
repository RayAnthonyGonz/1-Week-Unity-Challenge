using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowNPC : MonoBehaviour
{
    [SerializeField] private QuestClass quest;
    [SerializeField] private bool questGiven;
    [SerializeField] private bool dialogueEnded;
    [SerializeField] private bool inside;
    [SerializeField] private TextAsset dialogue;
    [SerializeField] private PlayerScript player;

    private DialogueManager dm;
    void Awake()
    {
        dialogue = Resources.Load("CowDialogue") as TextAsset;
        dm = GameObject.Find("DialogueHolder").GetComponent<DialogueManager>();
        dm.Render(dialogue);
        questGiven = false;
        dm.dialogueBox.gameObject.SetActive(false);
        dialogueEnded = false;
        inside = false;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        this.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        inside = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        this.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
        inside = true;
        player = other.GetComponent<PlayerScript>();
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && inside){
            if(player != null){
                player.canMove = false;
                player.inDialogue = true;
                dm.dialogueBox.gameObject.SetActive(true);
                StartCoroutine(dm.moveDialogue(dm.endLoc));
                if(!questGiven){
                    dialogueEnded = dm.LoadQuestDialogue(1, player);
                    if(dialogueEnded){
                        StartCoroutine(player.quests.GiveQuest(player, quest));
                        questGiven = true;
                        
                    }
                }else{
                    if(questGiven){
                        bool hasQuest = false;
                        bool isDone = false;
                        if(player.quests.HasQuestFrom("Cow")){
                            foreach(var quest in player.quests.QuestList()){
                                if(quest.GetQuest().giver.Equals("Cow")){
                                    hasQuest = true;
                                    if(quest.GetQuest().QuestStatus()){
                                        isDone = true;
                                    }
                                }
                            }
                            if(hasQuest){
                                if(!isDone){
                                    dialogueEnded = dm.LoadQuestDialogue(2, player);
                                }else{
                                    dialogueEnded = dm.LoadQuestDialogue(3, player);
                                    if(dialogueEnded){
                                        foreach (var questCheck in player.quests.QuestList())
                                        {
                                            if(questCheck.GetQuest().QuestStatus()){
                                                StartCoroutine(player.quests.RemoveQuest(player, quest));
                                                player.inventory.Remove(quest.GetCollect().collectable, quest.GetCollect().required);
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }else{
                            dialogueEnded = dm.LoadQuestDialogue(4, player);
                        }
                    }
                }
            }
            
        }
    }
}
