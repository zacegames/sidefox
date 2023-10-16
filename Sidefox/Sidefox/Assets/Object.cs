using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{
    

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (TestSceneController.StartLevel == true)
        {
           // Debug.Log("global enemy speed = " + TestSceneController.globalEnemySpeed);
            transform.position += Vector3.left * (TestSceneController.globalEnemySpeed * Time.deltaTime);
        }
        /*
        if (transform.position.x < -TestSceneController.SceneDimensions.x - 5)
        {
            Destroy(this.gameObject);
        }
        */
    }
}
