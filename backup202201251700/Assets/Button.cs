using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{
    private List<Canvas> list;
    public void Clicked2DMap(){
        SceneManager.LoadScene("WiFiNavigation");
    }
}
