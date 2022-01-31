using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixCanvas : MonoBehaviour
{
    public Canvas canvas;
    public GameObject user;
    public GameObject map;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("user : x "+ user.transform.position.x.ToString() + " y "+ user.transform.position.y.ToString());
        Debug.Log("map : x "+ map.transform.position.x.ToString() + " y "+ map.transform.position.y.ToString());

        canvas.transform.position -= user.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
