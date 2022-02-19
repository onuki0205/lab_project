using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARBaseButton : MonoBehaviour
{
    // Start is called before the first frame update
    public int buttontype;
    public int buttongroup;
    public void OnClick()
    {
        this.GetComponentInParent<ARButtonController>().SelectedButton(buttontype);
    }
}
