using UnityEngine;

public class TL_DisableUI : MonoBehaviour
{
    //UI Gameobjects
    public GameObject StartButton;
    public GameObject InstructionsButton;
    public GameObject OptionsButton;
    public GameObject CreditsButton;
    public GameObject BackButton;
    public GameObject InstructionsText;
    public GameObject CreditsText;
    public GameObject OptionsMenu;

    //Boolean toggle to switch on/off UI
    private bool Toggle = true;


    public void ToggleBoolean()
    {
        Toggle = !Toggle;
    }

    public void ToggleTextObject(GameObject TextObject)
    {
        TextObject.SetActive(!Toggle);
    }

    void ToggleButtons()
    {
        StartButton.SetActive(Toggle);
        InstructionsButton.SetActive(Toggle);
        OptionsButton.SetActive(Toggle);
        CreditsButton.SetActive(Toggle);
        BackButton.SetActive(!Toggle);
    }

    void Update()
    {
        ToggleButtons();
    }

}
