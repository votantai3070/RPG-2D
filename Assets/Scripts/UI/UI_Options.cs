using UnityEngine;
using UnityEngine.UI;

public class UI_Options : MonoBehaviour
{
    private Player player;
    [SerializeField] private Toggle healthbarToggle;

    private void Awake()
    {
        player = FindFirstObjectByType<Player>();

        healthbarToggle.onValueChanged.AddListener(OnHealthbarToggleChange);
    }

    public void OnHealthbarToggleChange(bool isOn)
    {
        player.playerHealth.EnableHealthBar(isOn);
    }

    public void GoMainMenuBtn() => GameManager.instance.ChangeScene("MainMenu", RespawnType.NoneSpecific);
}
