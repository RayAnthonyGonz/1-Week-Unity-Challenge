using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public RectTransform dialogueBox;
    [SerializeField] private List<string> dialogueQuest;    // 1
    [SerializeField] private List<string> dialogueNotDone;  // 2
    [SerializeField] private List<string> dialogueDone;     // 3
    [SerializeField] private List<string> dialogueAfter;    // 4
    private int currDialogue;
    private int dq = 0;
    private int dnd = 0;
    private int dd = 0;
    private int da = 0;
    [SerializeField] private string NPCName;
    void Start()
    {
        startLoc = (int) this.gameObject.GetComponent<RectTransform>().anchoredPosition.y;
    }

    public void Render(TextAsset file)
    {
        // try{
            //string path = "Assets/Resources/" +file.name+".txt";
            //Debug.Log(path);
            var textFile = Resources.Load<TextAsset>(file.name);
            List<string> tempFile = new List<string>(textFile.text.Split('\n'));
            string dialogueType = "";
            for (int i = 0; i < tempFile.Count; i++){
                string line = tempFile[i];
                //Debug.Log(line);
                if (i==0){NPCName = line;}
                if (line.Contains("_Quest")){
                    dialogueType = "_Quest";
                    continue;
                }
                if (line.Contains("_NotDone")){
                    dialogueType = "_NotDone";
                    continue;
                }
                if (line.Contains("_Done")){
                    dialogueType = "_Done";
                    continue;
                }
                if (line.Contains("_After")){
                    dialogueType = "_After";
                    continue;
                }
                switch(dialogueType){
                    case "_Quest": dialogueQuest.Add(line); break;
                    case "_NotDone": dialogueNotDone.Add(line); break;
                    case "_Done": dialogueDone.Add(line); break;
                    case "_After": dialogueAfter.Add(line); break;
                    default: break;
                }
            }
        // }
        // catch
        // {
        //     Debug.Log("The file could not be read:");
        // }
    }
    #region lerp
    public int endLoc = 10;
    public int startLoc = -250;

    public IEnumerator moveDialogue(int targetPosition)
    {
        endLoc = 10;
        startLoc = -250;
        float time = 0;
        Vector2 startPosition = dialogueBox.anchoredPosition;
        while (time < 0.5f)
        {
            dialogueBox.anchoredPosition = Vector2.Lerp(startPosition, new Vector2(0, targetPosition), time / 0.5f);
            time += Time.deltaTime;
            //Debug.Log(Time.deltaTime);
            yield return null;
        }
        dialogueBox.anchoredPosition = new Vector2(0, targetPosition);
    }
    #endregion

    public bool LoadQuestDialogue(int type,PlayerScript player){
        currDialogue = type;
        //Debug.Log(dq);
        switch(currDialogue){
            case 1: 
                if (dialogueQuest.Count > dq){
                    
                    dialogueBox.GetChild(2).GetComponent<TextMeshProUGUI>().text = dialogueQuest[dq++];
                    
                    return false;
                }else{
                    StartCoroutine(moveDialogue(startLoc));
                    player.inDialogue = false;
                    player.canMove = true;
                    dq = 0;
                    return true;
                }
                break;
            case 2:if (dialogueNotDone.Count > dnd){
                    dialogueBox.GetChild(2).GetComponent<TextMeshProUGUI>().text = dialogueNotDone[dnd++];
                    return false;
                }else{
                    StartCoroutine(moveDialogue(startLoc));
                    player.inDialogue = false;
                    player.canMove = true;
                    dnd=0;
                    return true;
                }
                break;
            case 3:if (dialogueDone.Count > dd){
                    dialogueBox.GetChild(2).GetComponent<TextMeshProUGUI>().text = dialogueDone[dd++];
                    return false;
                }else{
                    StartCoroutine(moveDialogue(startLoc));
                    player.inDialogue = false;
                    player.canMove = true;
                    dd=0;
                    return true;
                }
                break;
            case 4:if (dialogueAfter.Count > da){
                    dialogueBox.GetChild(2).GetComponent<TextMeshProUGUI>().text = dialogueAfter[da++];
                    return false;
                }else{
                    StartCoroutine(moveDialogue(startLoc));
                    player.inDialogue = false;
                    player.canMove = true;
                    da=0;
                    return true;
                }
                break;
            default: return true; break;
        }
    }
}
