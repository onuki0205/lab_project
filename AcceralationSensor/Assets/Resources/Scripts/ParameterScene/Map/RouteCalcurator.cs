using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class RouteCalcurator : MonoBehaviour
{

    private Entity_LabNode esnode;
    private Entity_LabRoom esroom;
    private int NodeNum;

    private List<Node> nodes = new List<Node>();

    private Userstate testuserstate = new Userstate(new Vector2(-1.5f, 27.7f), 0, 1);

    void Start()
    {
        esnode = Resources.Load("Assets/Lab_NodeList") as Entity_LabNode;
        esroom = Resources.Load("Assets/Lab_RoomList") as Entity_LabRoom;
        NodeNum = esnode.sheets[0].list.Count;

        for (int i = 0; i < NodeNum; i++)
        {
            nodes.Add(new Node(esnode.sheets[0].list[i].connectID));
        }

        log();
    }

    private void log()
    {
        // List<int> route = dikstra(46, 49);
        // string route_str = "";
        // foreach (int r in route)
        // {
        //     route_str += r.ToString() + " ";
        // }
        // Debug.Log(route_str);

        //closeststaris(testuserstate, "3");
        routecalcrate(esroom.sheets[1].list[70]);
    }

    private List<int> dikstra(int from, int to, int floor)
    {
        List<int> route = new List<int>();
        List<int> queue = new List<int>();

        nodes[from].distance = 0;
        nodes[from].fromnodeID = -1;
        queue.Add(from);
        floor--;

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
                float tmpdist = nodes[minid].distance + Mathf.Abs(Mathf.Pow(esnode.sheets[floor].list[tmpid].X - esnode.sheets[floor].list[minid].X, 2) + Mathf.Pow(esnode.sheets[floor].list[tmpid].Y - esnode.sheets[floor].list[minid].Y, 2));
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
        for (int i = 0; i < nodes.Count; i++)
        {
            nodes[i].resetstate();
        }

        return route;
    }

    private Entity_LabRoom.Param closeststaris(Userstate u, string toID)
    {
        float mindist = float.PositiveInfinity;
        int minindex = 0;
        for (int i = esroom.sheets[u.floor - 1].list.Count - 1; i >= 0; i--)
        {
            if (esroom.sheets[u.floor - 1].list[i].Name.Contains("階段") == true)
            {
                if (toID[0] == '3' && esroom.sheets[u.floor - 1].list[i].Name.Contains("階段H")) continue;
                int node = esroom.sheets[u.floor - 1].list[i].ClosestNode;
                Vector2 tmp = new Vector2(esnode.sheets[u.floor - 1].list[node].X, esnode.sheets[u.floor - 1].list[node].Y);
                float tmpdist = Mathf.Sqrt(Mathf.Pow(u.position.x - tmp.x, 2) + Mathf.Pow(u.position.y - tmp.y, 2));
                if (mindist > tmpdist)
                {
                    mindist = tmpdist;
                    minindex = i;
                }
            }
        }

        return esroom.sheets[u.floor - 1].list[minindex];
    }

    public void routecalcrate(Entity_LabRoom.Param to)
    {
        Userstate user = testuserstate;
        List<int> route;
        string route_str = "";

        float mindist = float.PositiveInfinity;
        int userclosestnode = 0;
        for (int i = 0; i < esnode.sheets[user.floor - 1].list.Count; i++)
        {
            Vector2 tmp = new Vector2(esnode.sheets[user.floor - 1].list[i].X, esnode.sheets[user.floor - 1].list[i].Y);
            float tmpdist = Mathf.Sqrt(Mathf.Pow(user.position.x - tmp.x, 2) + Mathf.Pow(user.position.y - tmp.y, 2));
            if (mindist > tmpdist)
            {
                mindist = tmpdist;
                userclosestnode = i;
            }
        }
        if (user.floor == int.Parse(to.ID[0].ToString()))
        {
            route = dikstra(userclosestnode, to.ClosestNode, user.floor);
            foreach (int r in route)
            {
                route_str += r.ToString() + " ";
            }
            Debug.Log(route_str);
        }
        else
        {
            Entity_LabRoom.Param stair = closeststaris(user, to.ID);
            route = dikstra(userclosestnode, stair.ClosestNode, user.floor);
            foreach (int r in route)
            {
                route_str += r.ToString() + " ";
            }
            route_str += "-" + to.ID[0].ToString() + " ";

            int tofloor = int.Parse(to.ID[0].ToString()) - 1;
            for (int i = esroom.sheets[tofloor].list.Count - 1; i >= 0; i--)
            {
                if (esroom.sheets[tofloor].list[i].Name.CompareTo(stair.Name) == 0)
                {
                    stair = esroom.sheets[tofloor].list[i];
                    break;
                }
            }

            route = dikstra(stair.ClosestNode, to.ClosestNode, tofloor + 1);

            foreach (int r in route)
            {
                route_str += r.ToString() + " ";
            }
            Debug.Log(route_str);
        }
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

    public void resetstate()
    {
        state = false;
        fromnodeID = -1;
        distance = float.PositiveInfinity;

    }

    public List<int> getconnectlist()
    {
        return connectlist;
    }
}