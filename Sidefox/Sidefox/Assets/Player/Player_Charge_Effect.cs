using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Charge_Effect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Player.PlayerPosition;
    }
}
