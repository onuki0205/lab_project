using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveStairButton : MonoBehaviour
{
    public void OnClick()
    {
        GameObject.Find("GuideText").GetComponent<GuideText>().MoveStairDone();
    }
}
