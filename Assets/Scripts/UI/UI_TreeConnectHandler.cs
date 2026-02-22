using UnityEngine;

[System.Serializable]
public class UI_TreeConnectDetails
{
    public UI_TreeConnectHandler childNode;
    public NodeConnectionType direction;
    [Range(100f, 350f)] public float length;
}

public class UI_TreeConnectHandler : MonoBehaviour
{
    [SerializeField] private RectTransform rect;
    [SerializeField] private UI_TreeConnectDetails[] connectDetails;
    [SerializeField] private UI_TreeConnection[] connections;

    private void OnValidate()
    {
        if (rect == null)
        {
            rect = GetComponent<RectTransform>();
        }

        if (connectDetails.Length != connections.Length)
        {
            Debug.LogWarning("Connect detail and connections arrays must have the same length.");
            return;
        }

        UpdateDirections();
    }

    private void UpdateDirections()
    {
        for (int i = 0; i < connectDetails.Length; i++)
        {
            var detail = connectDetails[i];
            var connection = connections[i];
            connection.SetConnection(detail.direction, detail.length);
            Vector2 targetPos = connection.GetConnectionPoint(rect);
            detail.childNode.SetPosition(targetPos);
        }
    }

    public void SetPosition(Vector2 position) => rect.anchoredPosition = position;
}
