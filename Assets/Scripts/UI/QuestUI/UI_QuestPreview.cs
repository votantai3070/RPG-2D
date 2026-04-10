using TMPro;
using UnityEngine;

public class UI_QuestPreview : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI questName;
    [SerializeField] private TextMeshProUGUI questDescription;
    [SerializeField] private TextMeshProUGUI questGoal;
    [SerializeField] private UI_QuestRewardSlot[] questRewards;

    [SerializeField] private GameObject[] additionalObjects;

    public void SetupQuestPreview(QuestDataSO questDataSO)
    {
        EnableAdditionalObjects(true);
        EnableQuestRewardObjects(false);

        questName.text = questDataSO.questName;
        questDescription.text = questDataSO.questDescription;
        questGoal.text = $"Goal: {questDataSO.questGoal}";

        for (int i = 0; i < questDataSO.rewardItems.Length; i++)
        {
            Inventory_Item rewardItem = new(questDataSO.rewardItems[i].itemData);
            rewardItem.stackSize = questDataSO.rewardItems[i].stackSize;

            questRewards[i].gameObject.SetActive(true);
            questRewards[i].UpdateSlot(rewardItem);
        }
    }

    public void MakeQuestPreviewEmpty()
    {
        questName.text = "";
        questDescription.text = "";

        EnableAdditionalObjects(false);
        EnableQuestRewardObjects(false);
    }

    private void EnableAdditionalObjects(bool enable)
    {
        foreach (var obj in additionalObjects)
            obj.SetActive(enable);
    }

    private void EnableQuestRewardObjects(bool enable)
    {
        foreach (var obj in questRewards)
        {
            obj.gameObject.SetActive(enable);
        }
    }
}
