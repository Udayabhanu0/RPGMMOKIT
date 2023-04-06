using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public Image backgroundImage;
    public Image loadingCircle;
    

    private bool isRotating = false;

    void Start()
    {
       
    }

    void Update()
    {
        // If the circle is currently rotating, rotate it around the Z axis
        if (isRotating)
        {
            loadingCircle.transform.Rotate(0f, 0f, Time.deltaTime * 360f / 5f);
        }
    }

    public void Click()
    {
        // Set the background image to active and make it fill the screen
        backgroundImage.gameObject.SetActive(true);
        backgroundImage.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);

        // Set the loading circle to active and position it in the center of the screen
        loadingCircle.gameObject.SetActive(true);
        loadingCircle.rectTransform.position = new Vector2(Screen.width / 2f, Screen.height / 2f);

        // Start rotating the circle
        isRotating = true;

        // Destroy the loading circle after 5 seconds
        Destroy(loadingCircle.gameObject, 5f);
    }
}
