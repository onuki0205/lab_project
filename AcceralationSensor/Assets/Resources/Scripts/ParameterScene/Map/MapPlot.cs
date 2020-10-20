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

    private Entity_Lab_1F es;
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
        es = Resources.Load("Assets/MapNode") as Entity_Lab_1F;

        // 線の幅
        locus.startWidth = 1f;
        locus.endWidth = 1f;

        AddNewPosition(new Vector2(0.0f, 0.0f));
        for (int i = 0; i < es.sheets[0].list.Count; i++)
        {
            addNewNode(new Vector2(es.sheets[0].list[i].X, es.sheets[0].list[i].Y));
        }
        setVisible(false);
    }

    private void Update()
    {
    }

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
        else
        {
            //locus.SetPosition(0, pos_v[0]);
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

    private void addNewNode(Vector2 pos)
    {
        GameObject obj = (GameObject)Resources.Load("Prehabs/MapNode");
        Nodes.Add((GameObject)Instantiate(obj, new Vector3(pos.x, pos.y, 0f) * 5.5f, Quaternion.identity));
    }
}