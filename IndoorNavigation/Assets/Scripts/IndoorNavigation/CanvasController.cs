using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasController : MonoBehaviour
{
    private int currentPage = 0;
    private int maxPage;
    public GameObject[] canvasSet;

    private void Awake(){
        maxPage = canvasSet.Length;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ShowDemo(int canvas){
        int setSize = canvasSet.Length;
        for(int i = 0; i < setSize; i++){
            GameObject obj = canvasSet.GetValue(i) as GameObject;
            if(canvas == i){
                obj.SetActive(true);
            } else {
                obj.SetActive(false);
            }
        }
    }

    public void ClickedARButton(){
        SceneManager.LoadScene("MainScene");
    }

    public void ClickedMapButton(){
        currentPage = 2;
        ShowDemo(currentPage);
    }

    public void ClickedDestButton(){
        currentPage = 1;
        ShowDemo(currentPage);
    }
}
