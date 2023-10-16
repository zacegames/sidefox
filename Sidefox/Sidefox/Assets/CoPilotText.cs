using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CoPilotText : MonoBehaviour
{
    public float delay = 0.9f;
    public string fullText;
    public string currentText = "";
    public TextMeshProUGUI dialogueText;
    [TextArea(3, 10)]
    public string[] sentences;
    public int FTLength;

    public static int sentenceIndex;


    // Start is called before the first frame update
    void Start()
    {
        FTLength = sentences.Length;
        StartCoroutine(waitForKeyPress());
        dialogueText.SetText(sentences[sentenceIndex]);
    }

    private IEnumerator waitForKeyPress()
    {
        while (sentenceIndex != 3)
        {
            while (!Input.GetKeyDown(KeyCode.Space))
            {
                yield return null;
            }

            //while (sentenceIndex <= sentences.Length)
            //{
            sentenceIndex += 1;
            dialogueText.SetText(sentences[sentenceIndex]);
            //}

            //if (sentenceIndex > sentences.Length)
            //{
            //yield break;  
            //}
            Debug.Log("In Enumerator");

            yield return null;
        }
    }
    
    void Update()
    {
        //Debug.Log(sentenceIndex);
    
    } 
    
}

