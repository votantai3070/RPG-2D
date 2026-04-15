using TMPro;
using UnityEngine;

public class UI_ActiveQuestPreview : MonoBehaviour
{
    private Player_QuestManager questManager;

    [SerializeField] private TextMeshProUGUI questName;
    [SerializeField] private TextMeshProUGUI questDescription;
    [SerializeField] private TextMeshProUGUI questProgress;
    [SerializeField] private UI_QuestRewardSlot[] questRewardSlots;

    public void SetupQuestPreview(QuestData questData)
    {
        questManager = Player.instance.questManager;
        QuestDataSO questSO = questData.questDataSO;

        questName.text = questSO.questName;
        questDescription.text = questSO.questDescription;
        questProgress.text = questSO.questGoal + " " + questManager.GetQuestProgress(questData) + "/" + questSO.requiredAmount;

        foreach (var obj in questRewardSlots)
            obj.gameObject.SetActive(false);

        for (int i = 0; i < questSO.rewardItems.Length; i++)
        {
            var slot = questRewardSlots[i];

            slot.gameObject.SetActive(true);
            slot.UpdateSlot(questSO.rewardItems[i]);
        }
    }
}
