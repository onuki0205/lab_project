using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WiFiScan : MonoBehaviour
{
    private Entity_WiFi ew;
    public Canvas canvas;
    private List<GameObject> aAPs;
    private GameObject aAP;

    
    public float span = 3f;
    private float currentTime = 0f;

    float offset = Screen.height * 2 / 5 - 150;

    int count = 0;

    

    void Start()
    {
        ew = Resources.Load("Assets/WiFiScan") as Entity_WiFi;
        aAPs = new List<GameObject>();
        aAP = new GameObject("ActiveAP");
        aAP.transform.SetParent(canvas.transform, false);
        GameObject t = Instantiate(Resources.Load("Prehabs/AP") as GameObject, new Vector3(0f,offset,0f), Quaternion.identity);
        Text[] cobj = t.GetComponentsInChildren<Text>();
        
        foreach(var h in cobj){
            Debug.Log(h.text);
        }

        t.transform.SetParent(aAP.transform, false);
        aAPs.Add(t);
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        int shift = 0;

        if(currentTime > span){
            Debug.LogFormat ("{0}秒経過", span);
            currentTime = 0f;

            for (int k = 0; k < aAPs.Count; k++)
            {
                Destroy(aAPs[k]);
            }
            aAPs.Clear();
            for (int i = 0; i < ew.sheets[count].list.Count; i++)
            {
                GameObject t = Instantiate(Resources.Load("Prehabs/AP") as GameObject, new Vector3(0, offset - shift * 300, 0f), Quaternion.identity);
                t.transform.SetParent(aAP.transform, false);
                // t.GetComponentInChildren<Text>().text = es.sheets[j].list[i].Name;
                Text[] cobj = t.GetComponentsInChildren<Text>();
                cobj[0].text = ew.sheets[count].list[i].SSID + " (" + ew.sheets[count].list[i].MAC + ")";
                cobj[1].text = ew.sheets[count].list[i].RSSI.ToString() + " dBm";
                cobj[2].text = ew.sheets[count].list[i].DIS.ToString() + " m";
                aAPs.Add(t);
                shift++;
            }
            count++;
            if(count > 4) count = 0;
        }


    }
}
