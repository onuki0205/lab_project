using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShareData : MonoBehaviour
{

    private bool created = false;
    // Start is called before the first frame update
    void Start()
    {
        if (!created)
        {
            // this is the first instance -make it persist
            if (GameObject.FindGameObjectsWithTag("DontDestroyObj").Length > 1)
            {
                Debug.Log("delete");
                Destroy(this.gameObject);

                GameObject.FindWithTag("FixedCanvas").GetComponent<ButtonContoroller>().setCanvas();
            }
            DontDestroyOnLoad(this);
            created = true;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
