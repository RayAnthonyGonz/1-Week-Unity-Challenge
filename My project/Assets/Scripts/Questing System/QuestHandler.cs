using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class QuestHandler : MonoBehaviour
{
    [SerializeField] private List<QuestClass> questList;

    [SerializeField] private GameObject questPanel;
    [SerializeField] private GameObject panelSlotPrefab;
    [SerializeField] private PlayerScript player;
    
    public void Player(PlayerScript _player){
        player = _player;
    }
    private void Start()
    {
        QuestRefresh();
    }

    public void QuestRefresh(){
        Clear();
        for (int i = 0; i < questList.Count; i++)
        {
            Instantiate(panelSlotPrefab, questPanel.transform);
        }
        foreach (Transform child in questPanel.transform) { //Deletes Extra spawns that somehow come in
            if(questPanel.transform.childCount > questList.Count){
                GameObject.DestroyImmediate(child.gameObject, true);
            }
        }
        for (int i = 0; i < questList.Count; i++)
        {
            questList[i].GetCollect().Evaluate(player);
            //Debug.Log(player.inventory.inInventory(questList[i].GetCollect().collectable).ToString());
            questPanel.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = questList[i].GetQuest().questName;
            questPanel.transform.GetChild(i).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = questList[i].GetQuest().questDesc;
            questPanel.transform.GetChild(i).GetChild(2).GetChild(0).GetComponent<Image>().sprite = questList[i].GetQuest().questIcon;
            //Debug.Log(questPanel.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text);
            
            if (questList[i].GetQuest().QuestStatus()){
                questPanel.transform.GetChild(i).GetChild(5).gameObject.SetActive(true);
                questPanel.transform.GetChild(i).GetChild(4).gameObject.SetActive(false);
                questPanel.transform.GetChild(i).GetChild(3).gameObject.SetActive(false);
            }else{
                questPanel.transform.GetChild(i).GetChild(5).gameObject.SetActive(false);
                if (questList[i] is CollectingClass){
                    questPanel.transform.GetChild(i).GetChild(3).gameObject.SetActive(true);
                    questPanel.transform.GetChild(i).GetChild(3).GetComponent<TextMeshProUGUI>().text = player.inventory.inInventory(questList[i].GetCollect().collectable).ToString();
                    questPanel.transform.GetChild(i).GetChild(4).gameObject.SetActive(true);
                    questPanel.transform.GetChild(i).GetChild(4).GetComponent<TextMeshProUGUI>().text = questList[i].GetCollect().required.ToString();
                }else{
                    questPanel.transform.GetChild(i).GetChild(3).gameObject.SetActive(false);
                    questPanel.transform.GetChild(i).GetChild(4).gameObject.SetActive(false);
                }
            }
        }
    }

    public void Clear(){
        //int i = 0;
        foreach (Transform child in questPanel.transform) {
            // Debug.Log(child.GetChild(0).GetComponent<TextMeshProUGUI>().text);
            //Debug.Log(i++);
            GameObject.DestroyImmediate(child.gameObject, true);
            
        }
        
    }

    public void Add(QuestClass newQuest){
        questList.Add(newQuest);
        QuestRefresh();
    }

    public void Remove(QuestClass doneQuest){
        questList.Remove(doneQuest);
        doneQuest.Reward(player);
        QuestRefresh();
    }

    public bool HasQuestFrom(string questGiver){
        foreach (var quest in questList)
        {
            if (quest.GetQuest().giver == questGiver){
                //Debug.Log(quest.GetQuest().giver);
                return true;
            }
        }
        return false;
    }

    public List<QuestClass> QuestList(){
        return questList;
    }

    public IEnumerator GiveQuest(PlayerScript player, QuestClass newQuest){
        float timeElapsed = 0;
        float lerpDuration = 1;
        player.quests.Add(newQuest);
        while (timeElapsed < lerpDuration)
        {
            yield return null;
        }
        
    }

    public IEnumerator RemoveQuest(PlayerScript player, QuestClass doneQuest){
        float timeElapsed = 0;
        float lerpDuration = 1;
        player.quests.Remove(doneQuest);
        while (timeElapsed < lerpDuration)
        {
            yield return null;
        }
        
    }
}
