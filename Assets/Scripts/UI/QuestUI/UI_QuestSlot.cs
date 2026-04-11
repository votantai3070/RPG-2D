using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_QuestSlot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI questName;
    [SerializeField] private Image[] rewardQuickPreviewSlots;

    public QuestDataSO questInSlot { get; private set; }
    private UI_QuestPreview questPreview;

    public void SetupQuestSlot(QuestDataSO quest)
    {
        questPreview = transform.root.GetComponentInChildren<UI_Quest>().GetQuestPreview();

        questInSlot = quest;
        questName.text = quest.questName;

        foreach (var previewIcon in rewardQuickPreviewSlots)
        {
            previewIcon.gameObject.SetActive(false);
        }

        for (int i = 0; i < questInSlot.rewardItems.Length; i++)
        {
            if (quest.rewardItems[i] == null || quest.rewardItems[i].itemData == null) continue;

            Image slot = rewardQuickPreviewSlots[i];

            slot.gameObject.SetActive(true);
            slot.sprite = quest.rewardItems[i].itemData.itemIcon;
            slot.GetComponentInChildren<TextMeshProUGUI>().text = quest.rewardItems[i].stackSize.ToString();
        }
    }

    public void UpdateQuestPreview()
    {
        questPreview.SetupQuestPreview(questInSlot);
    }
}
