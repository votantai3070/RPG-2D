using UnityEngine;

public class UI_MainMenu : MonoBehaviour
{
    public void PlayBtn()
    {
        GameManager.instance.ContinuePlay();
    }

    public void QuitBtn()
    {
        Application.Quit();
    }
}
