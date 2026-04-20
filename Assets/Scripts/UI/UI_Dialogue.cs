using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Dialogue : MonoBehaviour
{
    private UI ui;

    [SerializeField] private Image speakerPortait;
    [SerializeField] private TextMeshProUGUI speakerName;
    [SerializeField] private TextMeshProUGUI dialogueSpeakerText;
    [SerializeField] private TextMeshProUGUI dialogueChoices;

    [Space]
    [SerializeField] private float textSpeed = .1f;
    private string fullTextToShow;
    private Coroutine textTypeCo;

    private DialogueLineSO currentLine;
    private bool waitingToConfirm;
    private bool canInteract;

    private void Awake()
    {
        ui = GetComponentInParent<UI>();
    }

    public void PlayDialogueLine(DialogueLineSO lineSO)
    {
        currentLine = lineSO;
        canInteract = true;

        speakerPortait.sprite = lineSO.speaker.speakerPortrait;
        speakerName.text = lineSO.speaker.speakerName;

        fullTextToShow = lineSO.GetRandomLine();
        textTypeCo = StartCoroutine(TypeText(fullTextToShow));
        StartCoroutine(EnableInteractionCo());
    }

    private void HandleNextAction()
    {
        switch (currentLine.actionType)
        {
            case DialogueActionType.OpenShop:
                ui.SwitchToIngameUI();
                ui.OpenMerchantUI(true);
                break;

            default:
                break;
        }
    }

    private void CompleteTyping()
    {
        if (textTypeCo != null)
        {
            StopCoroutine(textTypeCo);
            dialogueSpeakerText.text = fullTextToShow;
            textTypeCo = null;
        }
    }

    public void DialogueInteraction()
    {
        if (canInteract == false)
            return;

        if (textTypeCo != null)
        {
            CompleteTyping();
            waitingToConfirm = true;
            return;
        }

        if (waitingToConfirm)
        {
            waitingToConfirm = false;
            HandleNextAction();
        }
    }

    private IEnumerator TypeText(string text)
    {
        dialogueSpeakerText.text = "";

        foreach (char c in text)
        {
            dialogueSpeakerText.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        waitingToConfirm = true;
        textTypeCo = null;
    }

    private IEnumerator EnableInteractionCo()
    {
        yield return null;
        canInteract = true;
    }
}
