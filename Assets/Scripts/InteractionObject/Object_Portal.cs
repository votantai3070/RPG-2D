using UnityEngine;
using UnityEngine.SceneManagement;

public class Object_Portal : MonoBehaviour, ISaveable
{
    public static Object_Portal instance;
    public bool isActivated { get; private set; }

    [SerializeField] private Vector2 defaultPosition; // where portal appears in the town scene
    [SerializeField] private string townSceneName;

    [SerializeField] private Transform respawnPoint;
    [SerializeField] private bool canBeTriggered;

    private string currentSceneName;
    private string returnSceneName;
    private bool returningFromTown;

    private void Awake()
    {
        instance = this;

        currentSceneName = SceneManager.GetActiveScene().name;
        transform.position = new Vector3(9999, 9999); // Hide the portal in the beginning
    }

    public void ActivatePortal(Vector3 position, int facingDir = 1)
    {
        isActivated = true;
        transform.position = position;
        SaveManager.instance.GetGameData().inScencePortals.Clear(); // Clear other portals' data when activating a new one

        if (facingDir == -1)
            transform.Rotate(0, 180, 0);
    }

    public void DisableIfNeeded()
    {
        if (returningFromTown == false)
            return;

        SaveManager.instance.GetGameData().inScencePortals.Remove(currentSceneName);

        isActivated = false;
        transform.position = new Vector3(9999, 9999); // Hide the portal
    }

    private void UseTeleport()
    {
        string destination = IsTown() ? returnSceneName : townSceneName;
        GameManager.instance.ChangeScene(destination, RespawnType.Portal);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canBeTriggered == false)
            return;

        UseTeleport();
    }

    public void SetTrigger(bool trigger) => canBeTriggered = trigger;

    public Vector3 GetPosition() => respawnPoint != null ? respawnPoint.position : transform.position;

    private bool IsTown() => currentSceneName == townSceneName;

    public void LoadData(GameData data)
    {
        if (IsTown() && data.inScencePortals.Count > 0)
        {
            transform.position = defaultPosition;
            isActivated = true;
        }
        else if (data.inScencePortals.TryGetValue(currentSceneName, out Vector3 portalPosition))
        {
            transform.position = portalPosition;
            isActivated = true;
        }

        returningFromTown = data.returningFromTown;
        returnSceneName = data.portalDestinationSceneName;
    }

    public void SaveData(ref GameData data)
    {
        data.returningFromTown = IsTown();

        if (isActivated && IsTown() == false)
        {
            data.inScencePortals[currentSceneName] = transform.position;
            data.portalDestinationSceneName = currentSceneName;
        }
        else
            data.inScencePortals.Remove(currentSceneName);
    }
}
