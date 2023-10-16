using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class uiControllerStartLevel : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        //Debug.Log(this.gameObject.GetComponentInChildren<CoPilotText>().FTLength);
        if (CoPilotText.sentenceIndex > 2
            && !TestSceneController.StartLevel
            && (Input.GetKey("joystick button 1") || Input.GetKey(KeyCode.Space)))
        {
            TestSceneController.StartLevel = true;
            gameObject.SetActive(false);            
        }

        
    }
}
