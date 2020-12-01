using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BaseButton : MonoBehaviour
{
    public int buttontype;
    public int buttongroup;
    public void OnClick()
    {
        this.GetComponentInParent<ButtonContoroller>().SelectedButton(buttontype);
    }
}
