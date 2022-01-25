using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SelectDestination : MonoBehaviour
{
    private Entity_LabRoom es;
    // private RouteCalcurator rc;
    public Canvas canvas;
    public Text text;

    private bool visible = false;
    private int floor = 1;
    private int maxfloor = 3;
    // private int page = 0;
    int[] count;
    int groupnum = 10;
    private List<GameObject> toggles = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        es = Resources.Load("Assets/Lab_RoomList") as Entity_LabRoom;

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
                GameObject t = Instantiate(Resources.Load("Prehabs/Toggle") as GameObject, new Vector3(Screen.width * 26 / 120 + Screen.width * (count[j] / groupnum) * 23 / 60, offset - shift * 80 + j * Screen.height, 0f), Quaternion.identity);
                t.transform.parent = canvas.transform;
                // t.transform.SetParent(canvas.transform, false);
                t.GetComponentInChildren<Text>().text = es.sheets[j].list[i].Name;
                // t.GetComponentInChildren<Text>().text = es.sheets[j].list[i].ID + "  " + es.sheets[j].list[i].Name;
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

    private void Update()
    {
        if (visible == true)
        {
        }
    }

    public void Shif1F(){
        Debug.Log("clicked Shift1F");
        if (floor == 1) return;
        for (int i = 0; i < toggles.Count; i++)
        {
            toggles[i].transform.position += new Vector3(0, Screen.height * (floor - 1), 0);
        }
        floor = 1;
    }
    public void Shif2F(){
        Debug.Log("clicked Shift2F");
        if (floor == 2) return;
        for (int i = 0; i < toggles.Count; i++)
        {
            toggles[i].transform.position += new Vector3(0, Screen.height * (floor - 2), 0);
        }
        floor = 2;
    }
    public void Shif3F(){
        Debug.Log("clicked Shift3F");
        if (floor == 3) return;
        for (int i = 0; i < toggles.Count; i++)
        {
            toggles[i].transform.position += new Vector3(0, Screen.height * (floor - 3), 0);
        }
        floor = 3;
    }

    public void setDestination()
    {
        if (this.GetComponent<ToggleGroup>().AnyTogglesOn() == false) return;
        Toggle activetoggle = this.GetComponent<ToggleGroup>().ActiveToggles().First<Toggle>();
        // string destiID = activetoggle.GetComponentInChildren<Text>().text.Split(' ')[0];
        text.text = "Destination   :   " + activetoggle.GetComponentInChildren<Text>().text;

        // Entity_LabRoom.Param desti = null;

        // for (int i = 0; i < es.sheets[floor - 1].list.Count; i++)
        // {
        //     if (es.sheets[floor - 1].list[i].ID.CompareTo(destiID) == 0)
        //     {
        //         desti = es.sheets[floor - 1].list[i];
        //         // Userstate u = GameObject.Find("ValueSet").GetComponent<ValueSet>().user;
        //         // u.destination = desti;
        //         // u.route = rc.routecalclate(u);
        //         // this.GetComponentInParent<MainButtonController>().SelectedButton(7);
        //         // this.GetComponentInParent<MainButtonController>().SelectedButton(2);
        //         return;
        //     }

        // }
    }
    // public void ChangeFloor(int fl)
    // {
    //     if (floor == fl) return;
    //     for (int i = 0; i < toggles.Count; i++)
    //     {
    //         toggles[i].transform.position += new Vector3(0, Screen.height * (floor - fl), 0);
    //     }
    //     floor = fl;

    // }

    // public void setVisible(bool b)
    // {
    //     visible = b;
    //     if (b == true)
    //     {
    //         for (int i = 0; i < toggles.Count; i++)
    //         {
    //             toggles[i].SetActive(true);
    //         }
    //     }
    //     else
    //     {
    //         for (int i = 0; i < toggles.Count; i++)
    //         {
    //             toggles[i].SetActive(false);
    //         }
    //     }
    // }
}
