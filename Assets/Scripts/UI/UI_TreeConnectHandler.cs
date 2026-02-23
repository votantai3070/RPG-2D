using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class UI_TreeConnectDetails
{
    public UI_TreeConnectHandler childNode;
    public NodeConnectionType direction;
    [Range(100f, 350f)] public float length;
    [Range(-50f, 50f)] public float rotation;
}

public class UI_TreeConnectHandler : MonoBehaviour
{
    [SerializeField] private RectTransform rect;
    [SerializeField] private UI_TreeConnectDetails[] connectDetails;
    [SerializeField] private UI_TreeConnection[] connections;

    private Image connectionImage;
    private Color origionalColor;

    private void Awake()
    {
        if (connectionImage != null)
            origionalColor = connectionImage.color;
    }

    private void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    private void OnValidate()
    {
        if (connectDetails.Length <= 0)
            return;

        if (connectDetails.Length != connections.Length)
        {
            Debug.LogWarning("Connect detail and connections arrays must have the same length.");
            return;
        }

        UpdateConnections();
    }

    private void UpdateConnections()
    {
        for (int i = 0; i < connectDetails.Length; i++)
        {
            var detail = connectDetails[i];
            var connection = connections[i];
            Vector2 targetPos = connection.GetConnectionPoint(rect);
            Image connectionImage = connection.GetConnectionImage();

            connection.SetDirectConnection(detail.direction, detail.length, detail.rotation);

            if (detail.childNode == null)
                continue;

            detail.childNode.SetPosition(targetPos);
            detail.childNode.SetConnectionImage(connectionImage);
            detail.childNode.transform.SetAsLastSibling();
        }
    }

    public void UpdateAllConnection()
    {
        UpdateConnections();

        foreach (var node in connectDetails)
        {
            if (node.childNode == null) continue;
            node.childNode.UpdateConnections();
        }
    }

    public void ConnectionImageUnlocked(bool unlocked)
    {
        if (connectionImage == null)
            return;

        connectionImage.color = unlocked ? Color.white : origionalColor;
    }

    public void SetConnectionImage(Image img) => connectionImage = img;
    public void SetPosition(Vector2 position) => rect.anchoredPosition = position;
}
