using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MultiplayerARPG;

public class NPCTalk1 : MonoBehaviour
{
    public UINpcDialog NpcUi;
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;

    private int index;
    // Start is called before the first frame update
    void Start()
    {
        lines[0]=NpcUi.Data.Description;
        textComponent.text= string.Empty;
        StartDialogue(); 
        Debug.Log(NpcUi.Data.Description);
    }

    // Update is called once per frame
    void Update()
    {
     /*       if (textComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                textComponent.text = lines[index];
                StopAllCoroutines();
            }
     */
    }
    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }
    IEnumerator TypeLine()
    {
        foreach(char c in lines[index].ToCharArray())
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
