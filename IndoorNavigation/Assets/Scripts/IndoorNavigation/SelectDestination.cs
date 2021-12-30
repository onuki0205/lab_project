using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SelectDestination : MonoBehaviour
{
    public Canvas canvas;

    private Entity_LabRoom es;
    private bool visible = false;
    private int floor = 1;
    private int maxfloor = 3;
    private int page = 0;
    int[] count;
    int groupnum = 10;
    private List<GameObject> toggles = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        es = Resources.Load("Xls/Lab_RoomList") as Entity_LabRoom;

        float offset = Screen.height * 17 / 20;
        int shift = 0;
        count = new int[maxfloor];

        for (int j = 0; j < maxfloor; j++)
        {

            for (int i = 0; i < es.sheets[j].list.Count; i++)
            {
                if (es.sheets[j].list[i].Name.CompareTo("null") == 0) continue;
                if (es.sheets[j].list[i].Name.CompareTo("研究室") == 0) continue;
                if (es.sheets[j].list[i].Name.CompareTo("実験室") == 0) continue;
                if (es.sheets[j].list[i].Name.CompareTo("図書ラウンジ") == 0) continue;
                if (es.sheets[j].list[i].Name.Contains("階段") == true) continue;
                GameObject t = Instantiate(Resources.Load("Prehabs/Toggle") as GameObject, new Vector3(Screen.width * 2 / 5 + Screen.width * (count[j] / groupnum), offset - shift * 80 + j * Screen.height, 0f), Quaternion.identity);
                // t.transform.parent = this.transform;
                t.transform.SetParent(canvas.transform,false);
                t.GetComponentInChildren<Text>().text = es.sheets[j].list[i].ID + "  " + es.sheets[j].list[i].Name;
                t.GetComponent<Toggle>().group = this.GetComponent<ToggleGroup>();
                toggles.Add(t);
                if (++shift == groupnum)
                {
                    shift = 0;
                }
                count[j]++;
            }
            shift = 0;
        }
        // setVisible(false);
    }




    // Start is called before the first frame update
    // void Start()
    // {
    //     el = Resources.Load("csvFile/Lab_RoomList") as Entity_LabRoom;
        

    //     GameObject obj = (GameObject)Resources.Load("Prehabs/Toggle");
    //     GameObject toggle = (GameObject)Instantiate(obj,new Vector3(0.0f,0.0f,0.0f),Quaternion.identity);
    //     toggle.transform.SetParent(canvas.transform,false);
    //     // toggle.GetComponentInChildren<Text>().text = el.sheets[j].list[i].ID + "  " + es.sheets[j].list[i].Name;
    //     toggle.GetComponentInChildren<Text>().text = "text";
    //     toggle.GetComponent<Toggle>().group = this.GetComponent<ToggleGroup>();
    //     toggles.Add(toggle);
    // }

}
