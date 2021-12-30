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

    void Start()
    {
        esnode = Resources.Load("Assets/csvFiles/Lab_NodeList") as Entity_LabNode;
        esroom = Resources.Load("Assets/csvFiles/Lab_RoomList") as Entity_LabRoom;

        InitNodes(1);

        log();
    }

    private void log()
    {
    }


    private List<int> dikstra(int from, int to, int floor)
    {
        List<int> route = new List<int>();
        List<int> queue = new List<int>();

        InitNodes(floor);

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

    private float calcroutedistance(List<int> route, int floor)
    {
        float dist = 0;
        for (int i = 0; i < route.Count - 1; i++)
        {
            Vector2 from = new Vector2(esnode.sheets[floor - 1].list[route[i]].X, esnode.sheets[floor - 1].list[route[i]].Y);
            Vector2 to = new Vector2(esnode.sheets[floor - 1].list[route[i + 1]].X, esnode.sheets[floor - 1].list[route[i + 1]].Y);
            dist += Mathf.Sqrt(Mathf.Pow(to.x - from.x, 2) + Mathf.Pow(to.y - from.y, 2));
        }
        return dist;
    }

    public int calcclosestnode(Userstate u)
    {
        int closest = 0;
        float closestdist = float.PositiveInfinity;

        InitNodes(u.floor);

        if (u.closestnode == -1)
        {
            for (int i = 0; i < esnode.sheets[u.floor - 1].list.Count; i++)
            {
                Vector2 tmp = new Vector2(esnode.sheets[u.floor - 1].list[i].X, esnode.sheets[u.floor - 1].list[i].Y);
                float tmpdist = Mathf.Sqrt(Mathf.Pow(u.position.x - tmp.x, 2) + Mathf.Pow(u.position.y - tmp.y, 2));
                if (tmpdist < closestdist)
                {
                    closestdist = tmpdist;
                    closest = esnode.sheets[u.floor - 1].list[i].ID;
                }
            }
        }
        else
        {
            Vector2 tmp = new Vector2(esnode.sheets[u.floor - 1].list[u.closestnode].X, esnode.sheets[u.floor - 1].list[u.closestnode].Y);
            float tmpdist = Mathf.Sqrt(Mathf.Pow(u.position.x - tmp.x, 2) + Mathf.Pow(u.position.y - tmp.y, 2));
            closestdist = tmpdist;
            closest = u.closestnode;

            List<int> li = nodes[u.closestnode].getconnectlist();
            for (int i = 0; i < nodes[u.closestnode].getconnectlist().Count; i++)
            {
                tmp = new Vector2(esnode.sheets[u.floor - 1].list[li[i]].X, esnode.sheets[u.floor - 1].list[li[i]].Y);
                tmpdist = Mathf.Sqrt(Mathf.Pow(u.position.x - tmp.x, 2) + Mathf.Pow(u.position.y - tmp.y, 2));
                if (tmpdist < closestdist)
                {
                    closestdist = tmpdist;
                    closest = esnode.sheets[u.floor - 1].list[li[i]].ID;
                }
            }

        }
        return closest;
    }

    private Entity_LabRoom.Param calccloseststaris(Userstate u, string toID)
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

    public List<int> routecalclate(Userstate user)
    {
        Entity_LabRoom.Param to = user.destination;
        List<int> route;
        string route_str = "";

        if (user.floor == int.Parse(to.ID[0].ToString()))
        {
            route = dikstra(user.closestnode, to.ClosestNode, user.floor);
            user.distance = calcroutedistance(route, user.floor);
            foreach (int r in route)
            {
                route_str += r.ToString() + " ";
            }
        }
        else
        {
            Entity_LabRoom.Param stair = calccloseststaris(user, to.ID);
            route = dikstra(user.closestnode, stair.ClosestNode, user.floor);
            user.distance = calcroutedistance(route, user.floor);
            route.Add(-int.Parse(to.ID[0].ToString()));

            int tofloor = int.Parse(to.ID[0].ToString()) - 1;
            for (int i = esroom.sheets[tofloor].list.Count - 1; i >= 0; i--)
            {
                if (esroom.sheets[tofloor].list[i].Name.CompareTo(stair.Name) == 0)
                {
                    stair = esroom.sheets[tofloor].list[i];
                    break;
                }
            }
            List<int> route2 = dikstra(stair.ClosestNode, to.ClosestNode, tofloor + 1);
            user.distance += calcroutedistance(route2, tofloor + 1);
            route.AddRange(route2);
            foreach (int r in route)
            {
                route_str += r.ToString() + " ";
            }
        }
        Debug.Log(route_str);
        return route;
    }
    public Vector2 correctposition(Userstate u)
    {
        Vector2 clonodepos = new Vector2(esnode.sheets[u.floor - 1].list[u.closestnode].X, esnode.sheets[u.floor - 1].list[u.closestnode].Y);

        float mindist = float.PositiveInfinity;
        int closestnode = 0;
        List<int> li = nodes[u.closestnode].getconnectlist();
        for (int i = 0; i < li.Count; i++)
        {
            float tmpdist = calcdistance(u.position, new Vector2(esnode.sheets[u.floor - 1].list[u.closestnode].X, esnode.sheets[u.floor - 1].list[u.closestnode].Y), new Vector2(esnode.sheets[u.floor - 1].list[li[i]].X, esnode.sheets[u.floor - 1].list[li[i]].Y));
            if (tmpdist < mindist)
            {
                mindist = tmpdist;
                closestnode = esnode.sheets[u.floor - 1].list[li[i]].ID;
            }
        }

        //Debug.Log(u.closestnode + "   " + closestnode);
        return nearest(u.position, new Vector2(esnode.sheets[u.floor - 1].list[u.closestnode].X, esnode.sheets[u.floor - 1].list[u.closestnode].Y), new Vector2(esnode.sheets[u.floor - 1].list[closestnode].X, esnode.sheets[u.floor - 1].list[closestnode].Y));
    }

    public float correctdirection(Userstate u)
    {
        Vector2 clonodepos = new Vector2(esnode.sheets[u.floor - 1].list[u.closestnode].X, esnode.sheets[u.floor - 1].list[u.closestnode].Y);
        float clodir = u.direction + 10;
        float diff = float.PositiveInfinity;

        List<int> li = nodes[u.closestnode].getconnectlist();
        Debug.Log(u.direction);
        for (int i = 0; i < li.Count; i++)
        {
            Vector2 tmppos = new Vector2(esnode.sheets[u.floor - 1].list[li[i]].X, esnode.sheets[u.floor - 1].list[li[i]].Y);
            float tmpang = getAngle(clonodepos, tmppos);
            float tmpdiff = Mathf.Abs(Mathf.Cos(u.direction * Mathf.Deg2Rad) - Mathf.Cos(tmpang * Mathf.Deg2Rad)) + Mathf.Abs(Mathf.Sin(u.direction * Mathf.Deg2Rad) - Mathf.Sin(tmpang * Mathf.Deg2Rad));
            if (calcdistance(u.position, clonodepos, tmppos) > 3.0f) continue;

            if (tmpdiff < diff)
            {
                clodir = tmpang;
                diff = tmpdiff;
            }
            tmpang = (tmpang + 180) % 360;
            tmpdiff = Mathf.Abs(Mathf.Cos(u.direction * Mathf.Deg2Rad) - Mathf.Cos(tmpang * Mathf.Deg2Rad)) + Mathf.Abs(Mathf.Sin(u.direction * Mathf.Deg2Rad) - Mathf.Sin(tmpang * Mathf.Deg2Rad));

            if (tmpdiff < diff)
            {
                clodir = tmpang;
                diff = tmpdiff;
            }
        }

        Debug.Log("clo:  " + clodir);
        if (Mathf.Min(Mathf.Abs(clodir - (u.direction + 10)), Mathf.Abs(clodir - (u.direction + 10 + 360))) < 30)
        {
            return clodir;
        }
        else
        {
            return u.direction + 10;
        }

    }

    private float calcdistance(Vector2 p, Vector2 a, Vector2 b)
    {
        float _x = b.x - a.x;
        float _y = b.y - a.y;
        float _x2 = _x * _x;
        float _y2 = _y * _y;
        float r2 = _x2 + _y2;
        float tt = -(_x * (a.x - p.x) + _y * (a.y - p.y));
        if (tt < 0)
        {
            return (a.x - p.x) * (a.x - p.x) + (a.y - p.y) * (a.y - p.y);
        }
        if (tt > r2)
        {
            return (b.x - p.x) * (b.x - p.x) + (b.y - p.y) * (b.y - p.y);
        }
        float f1 = _x * (a.y - p.y) - _y * (a.x - p.x);
        return f1 * f1 / r2;
    }

    float getAngle(Vector2 start, Vector2 target)
    {
        Vector2 dt = target - start;
        float rad = Mathf.Atan2(dt.x, dt.y);
        float degree = rad * Mathf.Rad2Deg + 90;

        if (degree < 0)
        {
            degree += 360;
        }
        return degree;
    }

    private Vector2 nearest(Vector2 P, Vector2 A, Vector2 B)
    {
        Vector2 a, b;
        float r;

        a.x = B.x - A.x;
        a.y = B.y - A.y;
        b.x = P.x - A.x;
        b.y = P.y - A.y;

        // 内積 ÷ |a|^2
        r = (a.x * b.x + a.y * b.y) / (a.x * a.x + a.y * a.y);

        if (r <= 0)
        {
            return A;
        }
        else if (r >= 1)
        {
            return B;
        }
        else
        {
            Vector2 result;
            result.x = A.x + r * a.x;
            result.y = A.y + r * a.y;
            return result;
        }
    }

    public void InitNodes(int floor)
    {
        nodes.Clear();

        floor--;
        NodeNum = esnode.sheets[floor].list.Count;

        for (int i = 0; i < NodeNum; i++)
        {
            nodes.Add(new Node(esnode.sheets[floor].list[i].connectID));
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