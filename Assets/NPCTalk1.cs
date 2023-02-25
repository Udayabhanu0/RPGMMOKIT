using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MultiplayerARPG;

public class NPCTalk1 : MonoBehaviour
{
    public UINpcDialog NpcUi;
    public TextMeshProUGUI textComponent;
    public string lines;
    public float textSpeed;

    private int index;
    // Start is called before the first frame update
    void Start()
    {
        textComponent.text= string.Empty;
        StartDialogue(); 
        Debug.Log(NpcUi.Data.Description);
    }

    // Update is called once per frame
    void Update()
    {
        if (lines == null)
        {
        lines=NpcUi.Data.Description;

        }
        if (textComponent.text == lines)
        {
            NextLine();
        }
        else
        {
            textComponent.text = lines;
            StopAllCoroutines();
        }

    }
    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }
    IEnumerator TypeLine()
    {
        foreach(char c in lines.ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed); 
        }
    } 
    void NextLine()
    {
        if(index<lines.Length-1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
