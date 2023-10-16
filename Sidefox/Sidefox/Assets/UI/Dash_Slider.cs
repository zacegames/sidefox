using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dash_Slider : MonoBehaviour
{
    //Used to set the slider value
    public Slider Dash;

    //Used to change the colour of the slider value
    public Image FillColour;

    public Color UseColour;

    public Color RechargeColour;

    private void Start()
    {
        FillColour.color = UseColour;
    }

    // Update is called once per frame
    void Update()
    {
        Dash.value = Player.DashBarUpdate;

        if (Player.DashRecharge)
        {
            FillColour.color = RechargeColour;
        }

        if(FillColour.color != UseColour && !Player.DashRecharge)
        {
            FillColour.color = UseColour;
        }


    }
}
