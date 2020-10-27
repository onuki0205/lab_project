using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;


public class RouteCalcurator : MonoBehaviour
{

    private Entity_LabNode es;
    private int NodeNum;

    private List<Node> nodes = new List<Node>();

    void Start()
    {
        es = Resources.Load("Assets/Lab_Data") as Entity_LabNode;
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
        List<int> route = dikstra(0, 54);
        foreach (int r in route)
        {
            Debug.Log(r + " ");
        }
    }

    private List<int> dikstra(int from, int to)
    {
        List<int> route = new List<int>();
        List<int> queue = new List<int>();

        nodes[from].distance = 0;
        nodes[from].fromnodeID = -1;
        queue.Add(from);

        while (queue.Count > 0)
        {
            int minid = -1;
            float mindist = float.PositiveInfinity;
            for (int i = 0; i < queue.Count; i++)
            {
                if (mindist > nodes[queue[i]].distance)
                {
                    mindist = nodes[queue[i]].distance;
                    minid = queue[i];
                }
            }
            queue.Remove(minid);

            nodes[minid].state = true;
            List<int> ls = nodes[minid].getconnectlist();

            for (int i = 0; i < ls.Count; i++)
            {
                int tmpid = ls[i];
                if (nodes[tmpid].state == true) continue;
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
        route.Add(to);
        while (nodes[tmp].fromnodeID != -1)
        {
            route.Add(nodes[tmp].fromnodeID);
            tmp = nodes[tmp].fromnodeID;
        }
        route.Reverse();

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