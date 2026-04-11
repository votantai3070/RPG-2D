using System;

[Serializable]
public class QuestData
{
    public QuestDataSO questDataSO;
    public int currentAmount;
    public bool canGetReward;

    public void AddQuestProgress(int amount)
    {
        currentAmount += amount;
        canGetReward = CanGetReward();
    }

    public bool CanGetReward()
    {
        return currentAmount >= questDataSO.requiredAmount;
    }

    public QuestData(QuestDataSO questSO)
    {
        questDataSO = questSO;
        currentAmount = 0;
        canGetReward = false;
    }
}
