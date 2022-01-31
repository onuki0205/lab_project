using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public Button button;
    private SelectDestination sd;


    public void ShiftPage()
    {
        switch(button.name){

            case "ChangeLeftButton":
                Debug.Log("Left");
                // sd.PageChange(0);
                break;

            case "ChangeRightButton":
                Debug.Log("Right");
                // sd.PageChange(1);
                break;

            case "Enter":
                // sd.setDestination();
                break;

            default:
                break;
        }
        return;
    } 
}
