using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideText : MonoBehaviour
{
    private GUIStyle labelStyle;    //テキスト表示のためのラベル
    private int digit = 8;              //少数をいくつまで表示するか
    public Userstate user = null;
    public MoveStairButton moveStairButton;
    // Start is called before the first frame update
    void Start()
    {
        this.labelStyle = new GUIStyle();
        this.labelStyle.fontSize = Screen.height / 22;
        this.labelStyle.normal.textColor = Color.white;
        moveStairButton.gameObject.SetActive(false);

        user = GameObject.Find("ValueSet").GetComponent<ValueSet>().user;
    }

    private void Update()
    {
        if (user.route != null)
        {
            if (user.route.Count >= 3)
            {
                if (user.route[1] < 0)
                {
                    moveStairButton.gameObject.SetActive(true);
                }
            }
        }
    }

    // Update is called once per frame
    void OnGUI()
    {

        float x = Screen.width / 20 * 3;
        float y = Screen.height * 6 / 20;
        float w = Screen.width * 8 / 10;
        float h = Screen.height / 20;

        if (user != null)
        {
            if (user.route != null)
            {
                string text = string.Empty;
                this.labelStyle.fontSize = Screen.height / 30;
                y = Screen.height / 20;
                text = string.Format("目的地:\n" + user.destination.Name);
                GUI.Label(new Rect(x, y, w, h), text, this.labelStyle);

                y = Screen.height *3/ 20;
                text = string.Format("距離:約" + Mathf.Round(user.distance) + "m");
                GUI.Label(new Rect(x, y, w, h), text, this.labelStyle);

                this.labelStyle.fontSize = Screen.height / 22;
                x = Screen.width / 20 * 3;
                y = Screen.height * 11 / 20;


                if (user.route.Count == 1)
                {
                    text = "目的地周辺に\n到着しました。";
                }
                else
                {
                    if (user.route[1] >= 0)
                    {
                        text = "矢印の方向に\n進んでください。";
                    }
                    else
                    {
                        x = Screen.width / 20 * 1;
                        text = string.Format("階段で{0}Fに\n移動してください。\n移動後、Doneボタンを\n押してください", -user.route[1]);
                    }
                }

                GUI.Label(new Rect(x, y, w, h), text, this.labelStyle);
            }
        }
        else
        {
            string text = string.Empty;
            text = string.Format("null");

            GUI.Label(new Rect(x, y, w, h), text, this.labelStyle);
        }
    }
    public void MoveStairDone()
    {
        List<int> tmproute = user.route;

        user.floor = -tmproute[1];
        Entity_LabNode esnode = Resources.Load("Assets/Lab_NodeList") as Entity_LabNode;
        user.position = new Vector2(esnode.sheets[user.floor - 1].list[tmproute[2]].X, esnode.sheets[user.floor - 1].list[tmproute[2]].Y);
        Debug.Log(user.floor + "  " + user.position);
        user.route = GameObject.Find("RouteCalcutator").GetComponent<RouteCalcurator>().routecalclate(user);

        moveStairButton.gameObject.SetActive(false);
        GameObject.Find("MapPlot").GetComponent<MapPlot>().pos_v.Clear();

    }

}
