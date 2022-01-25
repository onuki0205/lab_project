using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishApp : MonoBehaviour
{
    public void Onclick(){
        // UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
}
