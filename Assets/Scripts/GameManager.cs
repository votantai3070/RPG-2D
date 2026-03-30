using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private Vector3 lastDeathPosition;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetLastDeathPosition(Vector3 position) => lastDeathPosition = position;

    public void RestartScene()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        ChangeScene(sceneName, RespawnType.NoneSpecific);
    }

    public void ChangeScene(string sceneName, RespawnType respawnType)
    {
        SaveManager.instance.SaveGame();
        StartCoroutine(ChangeSceneCo(sceneName, respawnType));
    }

    private IEnumerator ChangeSceneCo(string sceneName, RespawnType respawnType)
    {
        // Fade effect

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(sceneName);

        yield return new WaitForSeconds(1);

        Vector3 position = GetNewPlayerPosition(respawnType);

        if (position != Vector3.zero)
            Player.instance.TeleportPlayer(position);
    }

    private Vector3 GetNewPlayerPosition(RespawnType type)
    {
        if (type == RespawnType.Portal)
        {
            Object_Portal portal = Object_Portal.instance;

            Vector3 position = portal.GetPosition();

            portal.SetTrigger(false);
            portal.DisableIfNeeded();
            return position;
        }

        if (type == RespawnType.NoneSpecific)
        {
            var data = SaveManager.instance.GetGameData();
            var checkpoints = FindObjectsByType<Object_Checkpoint>(FindObjectsSortMode.None);
            var unlockedCheckpoints = checkpoints
                .Where(c => data.unlockedCheckpoints.TryGetValue(c.GetCheckpointId(), out bool unlocked) && unlocked)
                .Select(c => c.GetPosition())
                .ToList();

            var enterWaypoints = FindObjectsByType<Object_Waypoint>(FindObjectsSortMode.None)
                .Where(wp => wp.GetWaypointType() == RespawnType.Enter)
                .Select(wp => wp.GetPositionAndSetTriggerFalse())
                .ToList();

            var selectedPosition = unlockedCheckpoints.Concat(enterWaypoints).ToList(); // combine both lists

            if (selectedPosition.Count == 0)
                return Vector3.zero;

            return selectedPosition.OrderBy(pos => Vector3.Distance(pos, lastDeathPosition)).First(); // return the closest position to the last death position
        }

        return GetWaypointPosition(type);
    }

    private Vector3 GetWaypointPosition(RespawnType type)
    {
        var waypoints = FindObjectsByType<Object_Waypoint>(FindObjectsSortMode.None);

        foreach (var point in waypoints)
        {
            if (point.GetWaypointType() == type)
                return point.GetPositionAndSetTriggerFalse();
        }

        return Vector3.zero;
    }
}
