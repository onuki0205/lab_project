using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueSet : MonoBehaviour
{

    public List<GameObject> APs;
    public GameObject user;
    float dt = 0;

    void Update()
    {
        // dt += Time.deltaTime;
        // if (dt > 1)
        // {
        //     dt = 0.0f;
        //     Debug.Log(dt.ToString());
        // }
    }

    // Start is called before the first frame update
    void Start()
    {
        // user.transform.localPosition = new Vector3(0,0,0);
    }

}
