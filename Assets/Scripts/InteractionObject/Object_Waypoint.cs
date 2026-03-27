using UnityEngine;
using UnityEngine.SceneManagement;

public class Object_Waypoint : MonoBehaviour
{
    [SerializeField] private string transferToScene;
    public RespawnType waypointType;
    [SerializeField] private RespawnType connectedWaypoint;
    [SerializeField] private bool canBeTrigger = true;

    private void OnValidate()
    {
        gameObject.name = "Object_Waypoint - " + waypointType.ToString() + " - " + transferToScene;

        if (waypointType == RespawnType.Enter)
            connectedWaypoint = RespawnType.Exit;

        if (waypointType == RespawnType.Exit)
            connectedWaypoint = RespawnType.Enter;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SaveManager.instance.SaveGame();
        SceneManager.LoadScene(transferToScene);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        canBeTrigger = true;
    }
}
