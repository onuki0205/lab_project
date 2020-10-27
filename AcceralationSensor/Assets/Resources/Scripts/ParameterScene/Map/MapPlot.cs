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

    private LineRenderer locus = null;
    public Image m_img;

    private Entity_LabNode es;
    private List<GameObject> Nodes = new List<GameObject>();
    private List<Vector3> pos_v = new List<Vector3>();

    //private LineRenderer circle = null; // 円を描画するための LineRenderer

    // Start is called before the first frame update
    void Start()
    {
        //フォント生成
        this.labelStyle = new GUIStyle();
        this.labelStyle.fontSize = Screen.height / 32;
        this.labelStyle.normal.textColor = Color.red;

        locus = gameObject.GetComponent<LineRenderer>();
        es = Resources.Load("Assets/Lab_Data") as Entity_LabNode;

        // 線の幅
        locus.startWidth = 1f;
        locus.endWidth = 1f;

        //ユーザーの初期位置をセットしておく
        AddNewPosition(new Vector2(0.0f, 0.0f));

        //エクセルに入力したノードの追加
        LoadNodeList(2);
        setVisible(false);
    }

    //ユーザーの座標更新
    public void AddNewPosition(Vector2 newpos)
    {
        //座標追加
        pos_v.Add(new Vector3(newpos.x * 20f, newpos.y * 20f, 0));
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
        if (visible == true)
        {
            //座標セット
            for (int i = 0; i < pos_v.Count; i++)
            {
                locus.SetPosition(i, pos_v[i]);
            }
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
            for (int i = 0; i < Nodes.Count; i++)
            {
                Nodes[i].SetActive(true);
            }
        }
        else
        {
            locus.positionCount = 0;
            m_img.enabled = false;
            for (int i = 0; i < Nodes.Count; i++)
            {
                Nodes[i].SetActive(false);
            }
        }

        PlotPosition();
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
        floor--;
        for (int i = 0; i < es.sheets[floor].list.Count; i++)
        {
            addNewNode(es.sheets[floor].list[i].ID, new Vector2(es.sheets[floor].list[i].X, es.sheets[floor].list[i].Y));
        }
    }
}