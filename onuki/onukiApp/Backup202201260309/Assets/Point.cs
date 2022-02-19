using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Point : MonoBehaviour
{
    public GameObject obj;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("local  " + obj.transform.position.x.ToString() + "  " + obj.transform.position.y.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
