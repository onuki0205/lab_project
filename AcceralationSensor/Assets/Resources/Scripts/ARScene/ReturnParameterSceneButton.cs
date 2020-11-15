using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnParameterSceneButton : MonoBehaviour
{
    public void OnClick()
    {
        GameObject.Find("Canvas1").GetComponentInChildren<WalkcounterText>().setVisible(true);
        SceneManager.LoadScene("ParameterScene");
    }
}
