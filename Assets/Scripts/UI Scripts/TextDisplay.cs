using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TextDisplay : MonoBehaviour
{
    private Text textToDisplay;

    void Start()
    {
        textToDisplay = GetComponent<Text>();
        textToDisplay.text = "0";
    }

    public void UpdateDisplay(int value)
    {
        textToDisplay.text = value.ToString();
    }
}