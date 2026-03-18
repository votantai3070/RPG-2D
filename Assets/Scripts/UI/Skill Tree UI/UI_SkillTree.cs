using UnityEngine;

public class UI_SkillTree : MonoBehaviour
{
    [SerializeField] private int skillPoints;
    [SerializeField] private UI_TreeConnectHandler[] parentNodes;
    private UI_TreeNode[] allTreeNodes;
    public Player_SkillManager skillManager { get; private set; }

    private void Start()
    {
        UpdateAllConnection();
    }

    public void UnlockDefaultSkills()
    {
        allTreeNodes = GetComponentsInChildren<UI_TreeNode>(true);
        skillManager = FindAnyObjectByType<Player_SkillManager>();

        foreach (var node in allTreeNodes)
            node.UnlockDefaultSkills();
    }

    [ContextMenu("Refund All Skills")]
    public void RefundAllSkills()
    {
        UI_TreeNode[] skillNodes = GetComponentsInChildren<UI_TreeNode>();

        foreach (var node in skillNodes)
        {
            node.Refund();
        }
    }

    public bool EnoughSkillPoint(int cost) => skillPoints >= cost;

    public void RemoveSkillPoint(int cost) => skillPoints -= cost;
    public void AddSkillPoint(int cost) => skillPoints += cost;

    [ContextMenu("Update All Connection")]
    public void UpdateAllConnection()
    {
        foreach (var node in parentNodes)
        {
            node.UpdateAllConnection();
        }
    }
}
