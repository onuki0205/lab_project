using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SelectDestination : MonoBehaviour
{
    private Entity_LabRoom es;
    public RouteCalcurator rc;

    private bool visible = false;
    private int floor = 1;
    private int page = 0;
    int count = 0;
    int groupnum = 10;
    private List<GameObject> toggles = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        es = Resources.Load("Assets/Lab_RoomList") as Entity_LabRoom;
        Debug.Log("gegegegegege");

        float offset = Screen.height * 17 / 20;

        int shift = 0;

        for (int i = 0; i < es.sheets[floor - 1].list.Count; i++)
        {
            if (es.sheets[floor - 1].list[i].Name.CompareTo("null") == 0) continue;
            if (es.sheets[floor - 1].list[i].Name.CompareTo("研究室") == 0) continue;
            if (es.sheets[floor - 1].list[i].Name.CompareTo("実験室") == 0) continue;
            if (es.sheets[floor - 1].list[i].Name.CompareTo("図書ラウンジ") == 0) continue;
            if (es.sheets[floor - 1].list[i].Name.Contains("階段") == true) continue;
            GameObject t = Instantiate(Resources.Load("Prehabs/Toggle") as GameObject, new Vector3(Screen.width * 2 / 5 + Screen.width * (count / groupnum), offset - shift * 80, 0f), Quaternion.identity);
            t.transform.parent = this.transform;
            Debug.Log(es.sheets[floor - 1].list[i].Name);
            t.GetComponentInChildren<Text>().text = es.sheets[floor - 1].list[i].ID + "  " + es.sheets[floor - 1].list[i].Name;
            t.GetComponent<Toggle>().group = this.GetComponent<ToggleGroup>();
            toggles.Add(t);
            if (++shift == groupnum)
            {
                shift = 0;
            }
            count++;
        }
        setVisible(false);
    }

    private void Update()
    {
        if (visible == true)
        {
        }
    }

    public void buttonpressed(int type)
    {
        switch (type)
        {
            //LeftButton
            case 0:
                if (page > 0)
                {
                    page--;

                    for (int i = 0; i < toggles.Count; i++)
                    {
                        toggles[i].transform.position += new Vector3(Screen.width, 0, 0);
                    }
                }
                break;
            //RightButton
            case 1:
                if (page < count / groupnum)
                {
                    page++;
                    for (int i = 0; i < toggles.Count; i++)
                    {
                        toggles[i].transform.position -= new Vector3(Screen.width, 0, 0);
                    }
                }
                break;
            //OKButton
            case 2:
                Toggle activetoggle = this.GetComponent<ToggleGroup>().ActiveToggles().First<Toggle>();
                string destiID = activetoggle.GetComponentInChildren<Text>().text.Split(' ')[0];

                Entity_LabRoom.Param desti = null;
                for (int i = 0; i < es.sheets[floor - 1].list.Count; i++)
                {
                    if (es.sheets[floor - 1].list[i].ID.CompareTo(destiID) == 0)
                    {
                        desti = es.sheets[floor - 1].list[i];
                        break;
                    }
                }
                rc.routecalcrate(desti);
                break;
        }

    }

    public void setVisible(bool b)
    {
        visible = b;
        if (b == true)
        {
            for (int i = 0; i < toggles.Count; i++)
            {
                toggles[i].SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < toggles.Count; i++)
            {
                toggles[i].SetActive(false);
            }
        }
    }
}
