using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnParameterSceneButton : MonoBehaviour
{
    public void OnClick() {
        SceneManager.LoadScene("ParameterScene");
    }
}
