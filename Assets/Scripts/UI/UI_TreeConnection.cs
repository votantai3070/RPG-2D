using UnityEngine;

public class UI_TreeConnection : MonoBehaviour
{
    [SerializeField] private RectTransform connectionPoint;
    [SerializeField] private RectTransform connectionLength;
    [SerializeField] private RectTransform childNodeConnectionPoint;

    public void SetConnection(NodeConnectionType direction, float length)
    {
        bool isEmpty = direction == NodeConnectionType.None;

        float finalLength = isEmpty ? 0f : length;
        float angle = GetDirectionAngle(direction);
        connectionPoint.localRotation = Quaternion.Euler(0f, 0f, angle);
        connectionLength.sizeDelta = new Vector2(finalLength, connectionLength.sizeDelta.y);
    }

    public Vector2 GetConnectionPoint(RectTransform rect)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle
            (rect.parent as RectTransform, childNodeConnectionPoint.position, null, out Vector2 localPoint);

        return localPoint;
    }

    private float GetDirectionAngle(NodeConnectionType direction)
    {
        switch (direction)
        {
            case NodeConnectionType.Left:
                return 180f;
            case NodeConnectionType.Right:
                return 0f;
            case NodeConnectionType.Up:
                return 90f;
            case NodeConnectionType.Down:
                return 270f;
            case NodeConnectionType.UpLeft:
                return 135f;
            case NodeConnectionType.UpRight:
                return 45f;
            case NodeConnectionType.DownLeft:
                return 225f;
            case NodeConnectionType.DownRight:
                return 315f;
            default:
                return 0f;
        }
    }
}

public enum NodeConnectionType
{
    None,
    Left,
    Right,
    Up,
    UpLeft,
    UpRight,
    Down,
    DownLeft,
    DownRight
}
