using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestsSystem : MonoBehaviour
{
    [System.Serializable]
    public class Quest
    {
        public string questName;
        public GameObject questTutorialUI;
    }

    public Quest[] quests;
    public int questIndex;
    
    [SerializeField] private Text txt_QuestName;
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>(); 
    }

    void Start()
    {
        questIndex = -1;
        CompleteQuestAnim();
    }

    public void CompleteQuestAnim()
    {
        Debug.Log("Mision completada...");

        anim.SetTrigger("ChangeQuest");
        questIndex++;
    }

    public void CompleteQuest()
    {
        ChangeQuest(questIndex);
    }

    private void ChangeQuest(int i)
    {
        foreach(Quest q in quests)
        {
            if (q.questTutorialUI == null)
                continue;

            q.questTutorialUI.SetActive(false);
        }
        txt_QuestName.text = "";

        if (questIndex == quests.Length)
            return;

        txt_QuestName.text = quests[i].questName;
        if (quests[i].questTutorialUI != null)
        {
            quests[i].questTutorialUI.SetActive(true);
        }
    }
}