using System.Collections;
using UnityEngine;
using TMPro;
using MultiplayerARPG;

public class MessageController : MonoBehaviour
{
    public float speed = 0.5f;
    public float yOffset = 1.0f;

    private RectTransform rectTransform;
    private TextMeshProUGUI textMeshPro;
    private Transform npcTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        textMeshPro = GetComponent<TextMeshProUGUI>();

        rectTransform.anchoredPosition = new Vector2(0, -rectTransform.rect.height - yOffset);
        textMeshPro.alpha = 0;

        npcTransform = transform.parent;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerCharacterEntity>() != null)
        {
            StartCoroutine(PopUpMessage());
        }
    }

    private IEnumerator PopUpMessage()
    {
        float t = 0;

        while (t < 1)
        {
            t += speed * Time.deltaTime;

            Vector3 worldPos = npcTransform.position + Vector3.up * yOffset;
            Vector2 screenPos = Camera.main.WorldToScreenPoint(worldPos);
            Vector2 anchoredPos = screenPos - new Vector2(Screen.width / 2, Screen.height / 2);

            float yPos = Mathf.Lerp(-rectTransform.rect.height - yOffset, anchoredPos.y, t);
            float alpha = Mathf.Lerp(0, 1, t);

            rectTransform.anchoredPosition = new Vector2(0, yPos);
            textMeshPro.alpha = alpha;

            yield return null;
        }
    }
}
