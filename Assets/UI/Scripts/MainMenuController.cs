using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour {
    private string currentMenu;
    private int currentOption;

    private bool inputEnabled = true;
    private float inputTimestamp = 0f;
    private float inputSpacing = 0.5f;

    public Color selectedColor;
    public Color deselectedColor;

    public Text[] mainMenuOption;

    void Awake () {

    }

    // Use this for initialization
    void Start () {
        LoadMenu("Main Menu");
        SelectOption(0);
    }
    
    // Update is called once per frame
    void Update () {
        // Process input for selecting between options:
        if (Mathf.Abs(Input.GetAxis("Vertical")) <= 0.1f || Time.time >= inputTimestamp + inputSpacing) {
            inputEnabled = true;
        }
        if (inputEnabled == true && Input.GetAxis("Vertical") <= -0.1f) {
            SelectNextOption();
            inputEnabled = false;
            inputTimestamp = Time.time;
        } else if (inputEnabled == true && Input.GetAxis("Vertical") >= 0.1f) {
            SelectPreviousOption();
            inputEnabled = false;
            inputTimestamp = Time.time;
        }

        // Process input for choosing an option:
        if (Input.GetButtonDown("Jump")) {
            ChooseOption();
        }
    }

    // ------------------------ //
    // NAVIGATING BETWEEN MENUS //
    // ------------------------ //

    void LoadMenu (string menuName) {
        foreach (Transform child in transform) {
            if (child.gameObject.name == menuName) {
                child.gameObject.SetActive(true);
                currentMenu = child.gameObject.name;
            } else {
                child.gameObject.SetActive(false);
            }
        }
    }

    // ----------------------- //
    // NAVIGATING WITHIN MENUS //
    // ----------------------- //

    void SelectOption (int selected) {
        currentOption = selected;

        if (currentMenu == "Main Menu") {
            for (int i = 0; i < mainMenuOption.Length; i++) {
                if (i == selected) {
                    mainMenuOption[i].color = selectedColor;
                } else {
                    mainMenuOption[i].color = deselectedColor;
                }
            }
        }
    }

    void SelectNextOption () {
        int nextOption;

        int optionCount = 0;
        if (currentMenu == "Main Menu") {
            optionCount = mainMenuOption.Length;
        }

        nextOption = currentOption + 1;
        if (nextOption >= optionCount) {
            nextOption = 0;
        }

        SelectOption(nextOption);
    }

    void SelectPreviousOption () {
        int previousOption;

        int optionCount = 0;
        if (currentMenu == "Main Menu") {
            optionCount = mainMenuOption.Length;
        }

        previousOption = currentOption - 1;
        if (previousOption < 0) {
            previousOption = optionCount - 1;
        }

        SelectOption(previousOption);
    }

    // --------------------- //
    // CHOOSING MENU OPTIONS //
    // --------------------- //

    void ChooseOption () {
        if (currentMenu == "Main Menu") {
            if (currentOption == 0) {

            } else if (currentOption == 1) {

            } else if (currentOption == 2) {

            }
        }
    }
}
