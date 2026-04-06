using UnityEngine;

public class UI_MainMenu : MonoBehaviour
{
    private void Start()
    {
        transform.root.GetComponentInChildren<UI_FadeScreen>().FadeIn();
    }
    public void PlayBtn()
    {
        AudioManager.instance.PlayGlobalSFX("button_click");
        GameManager.instance.ContinuePlay();
    }

    public void QuitBtn()
    {
        Application.Quit();
    }
}
