using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class createRoomList : MonoBehaviour
{
    public Canvas canvas;
    // Start is called before the first frame update
    void Start()
    {

        GameObject obj = (GameObject)Resources.Load("Prehabs/Toggle") as GameObject;
        GameObject instance = Instantiate(obj,new Vector3(0.0f,0.0f,0.0f),Quaternion.identity);
        instance.transform.SetParent(canvas.transform, false);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
