using UnityEngine;

public class UI_DeathScreen : MonoBehaviour
{
    public void GoToCampBtn()
    {
        GameManager.instance.ChangeScene("Level_0", RespawnType.NoneSpecific);
    }

    public void GoToCheckpoint()
    {
        GameManager.instance.RestartScene();
    }

    public void GoToMainMenu()
    {
        GameManager.instance.ChangeScene("MainMenu", RespawnType.NoneSpecific);
    }
}
