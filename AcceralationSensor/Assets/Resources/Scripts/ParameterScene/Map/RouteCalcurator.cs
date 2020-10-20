using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;


public class RouteCalcurator : MonoBehaviour
{

    private Entity_Lab_1F es;
    private int NodeNum;

    private List<Node> nodes = new List<Node>();

    void Start()
    {
        es = Resources.Load("Assets/MapNode") as Entity_Lab_1F;
        NodeNum = es.sheets[0].list.Count;

        for (int i = 0; i < NodeNum; i++)
        {
            nodes.Add(new Node(es.sheets[0].list[i].connectID));
        }

        log();
    }

    private void log()
    {
        Debug.Log("numberの１行目：" + es.sheets[0].list[0].ID);
        Debug.Log("nameの１行目：" + es.sheets[0].list[0].X);
        Debug.Log("numberの2行目：" + es.sheets[0].list[1].Y);
        Debug.Log("numberの2行目：" + NodeNum);

        Debug.Log(dikstra(0, 54));
    }

    private string dikstra(int from, int to)
    {
        string route = "";
        var queue = new List<int>();

        nodes[from].distance = 0;
        nodes[from].fromnodeID = -1;
        queue.Add(from);

        while (queue.Count > 0)
        {
            int minid = -1;
            float mindist = float.PositiveInfinity;
            for (int i = 0; i < queue.Count; i++)
            {
                Debug.Log("q = " + queue[i]);
                if (mindist > nodes[queue[i]].distance)
                {
                    mindist = nodes[queue[i]].distance;
                    minid = queue[i];
                }
            }
            queue.Remove(minid);

            Debug.Log(minid + " " + mindist);
            nodes[minid].state = true;
            List<int> ls = nodes[minid].getconnectlist();

            for (int i = 0; i < ls.Count; i++)
            {
                int tmpid = ls[i];
                if (nodes[tmpid].state == true) continue;
                Debug.Log(minid + " " + tmpid);
                float tmpdist = nodes[minid].distance + Mathf.Abs(Mathf.Pow(es.sheets[0].list[tmpid].X - es.sheets[0].list[minid].X, 2) + Mathf.Pow(es.sheets[0].list[tmpid].Y - es.sheets[0].list[minid].Y, 2));
                if (nodes[tmpid].distance > tmpdist)
                {
                    nodes[tmpid].distance = tmpdist;
                    nodes[tmpid].fromnodeID = minid;

                    queue.Add(tmpid);
                }
            }
        }

        int tmp = to;
        route += to + " ";
        while (nodes[tmp].fromnodeID != -1)
        {
            route += nodes[tmp].fromnodeID.ToString() + " ";
            tmp = nodes[tmp].fromnodeID;
        }
        return route;
    }
}

class Node
{
    public int fromnodeID { get; set; }
    public bool state { get; set; }
    public float distance { get; set; }

    private List<int> connectlist = new List<int>();

    public Node(string connectIDs)
    {
        string[] s = connectIDs.Split(',');
        for (int i = 0; i < s.Length; i++)
        {
            connectlist.Add(int.Parse(s[i]));
        }

        state = false;
        fromnodeID = -1;
        distance = float.PositiveInfinity;
    }

    public List<int> getconnectlist()
    {
        return connectlist;
    }
}