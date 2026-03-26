using UnityEngine;

public class Object_Checkpoint : MonoBehaviour, ISaveable
{
    private Object_Checkpoint[] allCheckPoints;
    private Animator anim;
    private Player player;

    private void Awake()
    {
        allCheckPoints = FindObjectsByType<Object_Checkpoint>(FindObjectsSortMode.None);
    }

    public void ActivativeCheckpoint(bool active)
    {
        anim.SetBool("isActive", active);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach (var point in allCheckPoints)
            point.ActivativeCheckpoint(false);

        SaveManager.instance.GetGameData().savedCheckpoint = transform.position;
        ActivativeCheckpoint(true);
    }

    public void LoadData(GameData data)
    {
        bool active = data.savedCheckpoint == transform.position;
        ActivativeCheckpoint(active);

        if (active)
            Player.instance.TeleportPlayer(transform.position);
    }

    public void SaveData(ref GameData data)
    {
    }
}
