using System.Collections.Generic;
using UnityEngine;

public class Player_QuestManager : MonoBehaviour
{
    public List<QuestData> activeQuests;

    public void AddProgrres(string questTargetId, int amount = 1)
    {
        foreach (var quest in activeQuests)
        {
            if (quest.questDataSO.questTargetId != questTargetId) continue;

            quest.AddQuestProgress(amount);
        }
    }

    public void AcceptQuest(QuestDataSO questSO)
    {
        activeQuests.Add(new QuestData(questSO));
    }

    public bool QuestIsActive(QuestDataSO questToCheck)
    {
        if (questToCheck == null) return false;

        return activeQuests.Find(q => q.questDataSO == questToCheck) != null;
    }
}