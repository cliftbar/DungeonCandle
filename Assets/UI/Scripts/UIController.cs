using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
    private SceneController sc;

    // Textbox variables:
    private GameObject textbox;
    private Text currentText;

    private string[] textArray;
    private bool displayingText;
    private bool writingText;
    private int currentCharacterCount;
    private int currentTextIndex;

    private float textTimestamp;
    public float defaultTextSpeed;
    private float currentTextSpeed;

    private Message currentMessageSource;

    // Lifebar variables:
    private RectTransform lifebarTransform;
    private RectTransform lifeTransform;
    public float lifebarScale;
    public float lifebarX;
    public float lifebarY;

    void Awake () {
        lifebarTransform = transform.Find("Lifebar").GetComponent<RectTransform>();
        lifeTransform = transform.Find("Lifebar").Find("Life").GetComponent<RectTransform>();

        textbox = GameObject.Find("Textbox");
        textbox.SetActive(false);
        currentText = GameObject.Find("UI Canvas/Text").GetComponent<Text>();
        currentText.gameObject.SetActive(false);
    }

    // Use this for initialization
    void Start () {
        sc = GameObject.Find("Scene Controller").GetComponent<SceneController>();
    }

    // Update is called once per frame
    void Update () {
        if (displayingText == true && writingText == true && Time.time >= textTimestamp + 1f / currentTextSpeed) {
            UpdateText();
        } else if (displayingText == true && writingText == false && Input.GetButtonDown("Interact")) {
            NextText();
        }
    }

    // ------------- //
    // PLAYER STATUS //
    // ------------- //

    public void UpdateLife(int currentLife, int maxLife) {
        lifebarTransform.sizeDelta = new Vector2((maxLife + 1) * lifebarScale * 2, lifebarScale * 4);
        lifebarTransform.anchoredPosition = new Vector2(lifebarX + maxLife * lifebarScale, lifebarY);

        lifeTransform.sizeDelta = new Vector2(currentLife * lifebarScale * 2, lifebarScale * 2);
        lifeTransform.anchoredPosition = new Vector2(currentLife * lifebarScale + lifebarScale, 0f);
    }

    // ------- //
    // Text UI //
    // ------- //

    public void DisplayText (Message source, string[] inputText, float textSpeed = -1f) {
        if (textSpeed == -1f) {
            currentTextSpeed = Mathf.Max(defaultTextSpeed, 1f);
        } else {
            currentTextSpeed = Mathf.Max(textSpeed, 1f);
        }
        sc.paused = true;

        writingText = true;
        displayingText = true;

        textbox.SetActive(true);
        currentText.gameObject.SetActive(true);

        textArray = inputText;
        currentTextIndex = 0;
        currentCharacterCount = 0;

        currentMessageSource = source;

        UpdateText();
    }

    private void UpdateText () {
        if (currentCharacterCount >= textArray[currentTextIndex].Length) {
            writingText = false;
        } else {
            textTimestamp = Time.time;
            currentCharacterCount += 1;
            currentText.text = textArray[currentTextIndex].Substring(0, currentCharacterCount);

            for (int i = currentCharacterCount; i < textArray[currentTextIndex].Length; i++) {
                if (textArray[currentTextIndex][i] == ' ') {
                    currentText.text += " ";
                } else {
                    currentText.text += "\u00A0";
                }
            }
        }
    }

    private void NextText () {
        writingText = true;

        currentTextIndex += 1;
        currentCharacterCount = 0;

        if (currentTextIndex >= textArray.Length) {
            HideText();
        } else {
            UpdateText();
        }
    }

    private void HideText () {
        sc.paused = false;

        writingText = false;
        displayingText = false;

        textbox.SetActive(false);
        currentText.gameObject.SetActive(false);

        StartCoroutine(currentMessageSource.FinishMessage());
    }
}
