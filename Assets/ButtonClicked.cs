using UnityEngine;
using UnityEngine.UI;

public class ButtonClicked : MonoBehaviour
{
    // Whether the button has been clicked
    public bool clicked = false;

    // The button component to listen to
    private Button button;

    private void Start()
    {
        // Get the button component
        button = GetComponent<Button>();

        // Add an onClick listener to the button
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        // Set clicked to true
        clicked = true;
    }
}
