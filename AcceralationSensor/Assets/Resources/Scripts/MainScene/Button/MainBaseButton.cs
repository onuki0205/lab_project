using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MainBaseButton : MonoBehaviour
{
    public int buttontype;
    public int buttongroup;
    public void OnClick()
    {
        this.GetComponentInParent<MainButtonController>().SelectedButton(buttontype);
    }
}
