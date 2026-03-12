using UnityEngine;

public class UI_SkillTree : MonoBehaviour
{
    [SerializeField] private int skillPoints;
    [SerializeField] private UI_TreeConnectHandler[] parentNodes;
    public Player_SkillManager skillManager;

    private void Awake()
    {
        skillManager = FindAnyObjectByType<Player_SkillManager>();
    }

    private void Start()
    {
        UpdateAllConnection();
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
