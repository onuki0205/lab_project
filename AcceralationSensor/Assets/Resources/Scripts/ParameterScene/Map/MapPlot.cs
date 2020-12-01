using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

public class MapPlot : MonoBehaviour
{
    private GUIStyle labelStyle;
    private bool visible;
    public RouteCalcurator rc;
    public Userstate user;
    public GameObject userarrow;

    private LineRenderer locus = null;
    private int floor;
    public Image m_img;

    private Entity_LabNode es;
    public List<GameObject> Nodes;
    public List<Vector3> pos_v;

    //private LineRenderer circle = null; // 円を描画するための LineRenderer

    // Start is called before the first frame update
    void Start()
    {
        //フォント生成
        this.labelStyle = new GUIStyle();
        this.labelStyle.fontSize = Screen.height / 32;
        this.labelStyle.normal.textColor = Color.red;

        locus = gameObject.GetComponent<LineRenderer>();
        es = Resources.Load("Assets/Lab_NodeList") as Entity_LabNode;
        userarrow = GameObject.Find("UserArrow");

        // 線の幅
        locus.startWidth = 1f;
        locus.endWidth = 1f;

        Nodes = new List<GameObject>();
        pos_v = new List<Vector3>();
        floor = 1;

        //エクセルに入力したノードの追加
        LoadNodeList(1);
        setVisible(false);
    }

    private void Update()
    {
        if (visible == true)
        {
            userarrow.transform.localPosition = new Vector3(user.position.x * 20f, user.position.y * 20f, 0);
            userarrow.transform.rotation = Quaternion.Euler(0, 0, 90 - user.direction);
        }
    }

    //ユーザーの座標更新
    public void AddNewPosition()
    {
        //座標追加
        pos_v.Add(new Vector3(user.position.x * 20f, user.position.y * 20f, 0));
        locus.positionCount++;

        //プロット
        if (visible == true)
        {
            PlotPosition();
        }
    }

    //ユーザーの座標をプロット
    private void PlotPosition()
    {
        //座標セット
        for (int i = 0; i < pos_v.Count; i++)
        {
            locus.SetPosition(i, pos_v[i]);

        }
    }

    public int getVectorCount()
    {
        return pos_v.Count;
    }

    public void setVisible(bool b)
    {
        visible = b;
        if (b == true)
        {
            locus.positionCount = pos_v.Count;
            m_img.enabled = true;
            userarrow.SetActive(true);

            // for (int i = 0; i < Nodes.Count; i++)
            // {
            //     Nodes[i].SetActive(true);
            // }

            LoadNodeList(floor);
            if (floor == user.floor)
            {
                PlotPosition();
            }
        }
        else
        {
            locus.positionCount = 0;
            m_img.enabled = false;
            userarrow.SetActive(false);
            for (int i = 0; i < Nodes.Count; i++)
            {
                GameObject.Destroy(Nodes[i]);
            }

            Nodes.Clear();
        }
    }

    //ノードを追加してマップ上に表示、IDも振る
    private void addNewNode(int id, Vector2 pos)
    {
        GameObject obj = (GameObject)Resources.Load("Prehabs/MapNode");
        obj.GetComponentInChildren<TextMesh>().text = id.ToString();
        Nodes.Add((GameObject)Instantiate(obj, new Vector3(pos.x, pos.y, 0f) * 5.5f, Quaternion.identity));
    }

    //引数の階層のノードをエクセルからまとめてロード
    private void LoadNodeList(int floor)
    {
        for (int i = 0; i < Nodes.Count; i++)
        {
            Destroy(Nodes[i]);
        }
        Nodes.Clear();
        floor--;
        for (int i = 0; i < es.sheets[floor].list.Count; i++)
        {
            addNewNode(es.sheets[floor].list[i].ID, new Vector2(es.sheets[floor].list[i].X, es.sheets[floor].list[i].Y));
        }
    }

    public void changemapimage(int fl)
    {
        floor = fl;
        switch (fl)
        {
            case 1:
                this.GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>("Images/Maps/Lab1F");
                LoadNodeList(1);
                rc.InitNodes(1);
                break;
            case 2:
                this.GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>("Images/Maps/Lab2F");
                LoadNodeList(2);
                rc.InitNodes(2);

                break;
            case 3:
                this.GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>("Images/Maps/Lab3F");
                LoadNodeList(3);
                rc.InitNodes(3);

                break;
        }
        PlotPosition();
        if (user.floor == fl)
        {
            userarrow.SetActive(true);
        }
        else
        {
            userarrow.SetActive(false);

        }
    }
}