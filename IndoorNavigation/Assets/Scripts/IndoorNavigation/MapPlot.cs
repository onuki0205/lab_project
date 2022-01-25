using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

public class MapPlot : MonoBehaviour
{
    public List<GameObject> Nodes = null;
    public List<GameObject> APs = null;
    public List<Vector3> Ps = null;
    public List<Vector3> ps = null;
    private Vector3[] pl = null;
    public GameObject map;
    private LineRenderer line = null;
    
    float dt = 0;
    int s = 0;
    int e = 0;
    int c = 0;
    bool frag = true;
  
    // Start is called before the first frame update
    void Start()
    {
        addAP();
        addNode();

        // Debug.Log("parent  :" + obj.transform.parent);
        // var o = obj.transform.position;
        // Debug.Log("x : " + o.x.ToString() + "  y : " + o.y.ToString() + "  z : " + o.z.ToString());

        PositionP();
        line = map.GetComponent<LineRenderer>();
        line.transform.parent = map.transform;
        line.startWidth = 0.5f;
        line.endWidth = 0.5f;
        line.positionCount = ps.Count;

        Debug.Log("ps[0]  x : " + ps[0].x.ToString() + "  y: " + ps[0].y.ToString());




        // pl = ps.ToArray();
        // 線を引く場所を指定する
        
        // for(int i = 0; i < ps.Count; i++){
        //     line.SetPosition(i,pl[i]);
        // }
        

    }

    // Update is called once per frame
    void Update()
    {

        dt += Time.deltaTime;
        if (dt > 1)
        {
            if(c <= ps.Count){
                dt = 0.0f;
                Ps.Add(ps[c]);
                pl = Ps.ToArray();
                line.SetPositions(pl);
                c++;
            }
        }
    }

    private void addAP()
    {
        Entity_AP ea = Resources.Load("Assets/Lab_APList") as Entity_AP;
        GameObject AP = new GameObject("APs");
        AP.transform.parent = map.transform;
        AP.transform.localPosition = new Vector3(0,0,0);
        
        for (int i = 0; i < 39; i++)
        {
            var ap = ea.sheets[0].list[i];
            GameObject obj = (GameObject)Resources.Load("Prehabs/MapNode");
            obj.GetComponentInChildren<TextMesh>().text = "AP";

            GameObject t = (GameObject)Instantiate(obj,new Vector3((float)(ap.X/25.634),(float)(ap.Y/25.634),0),Quaternion.identity);
            t.transform.parent = AP.transform;
            t.transform.localScale = new Vector3((float)0.4,(float)0.4,(float)0.15);
            // APs.Add(t);
            // Debug.Log("id : " + ap.ID.ToString() + "  x : " + ap.X.ToString() + "  y: " + ap.Y.ToString());
        }
        AP.transform.localPosition += new Vector3(0,425,2305);
    }

    private void addNode()
    {
        Entity_LabNode el = Resources.Load("Assets/Lab_NodeList") as Entity_LabNode;
        GameObject nodes = new GameObject("Nodes");
        nodes.transform.parent = map.transform;
        nodes.transform.localPosition = new Vector3(0,0,0);
        for (int i = 0; i < el.sheets[0].list.Count; i++)
        {
            var node = el.sheets[0].list[i];
            GameObject obj = (GameObject)Resources.Load("Prehabs/MapNode");
            obj.GetComponentInChildren<TextMesh>().text = node.ID.ToString();

            GameObject t = (GameObject)Instantiate(obj,new Vector3((float)(node.X/25.634),(float)(node.Y/25.634),0),Quaternion.identity);
            t.transform.parent = nodes.transform;
            t.transform.localScale = new Vector3((float)0.4,(float)0.4,(float)0.15);
            Nodes.Add(t);
        }
        nodes.transform.localPosition += new Vector3(0,425,2305);
    }

    private void PositionP()
    {
        Entity_PositionList ep = Resources.Load("Assets/PositionList") as Entity_PositionList;
        GameObject positions = new GameObject("Positions");
        positions.transform.parent = map.transform;
        positions.transform.localPosition = new Vector3(0,0,0);
        
        for (int i = 0; i < ep.sheets[0].list.Count; i++)
        {
            var p = ep.sheets[0].list[i];
            GameObject obj = (GameObject)Resources.Load("Prehabs/P");
            // GameObject t = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            GameObject t = (GameObject)Instantiate(obj,new Vector3((float)(p.X/25.634),(float)(p.Y/25.634),0),Quaternion.identity);

            t.transform.parent = positions.transform;
            // t.transform.localPosition = new Vector3((float)(p.X/25.634),(float)(p.Y/25.634),0);
            // t.transform.localPosition = new Vector3((float)(p.X),(float)(p.Y),0);
            t.transform.localScale = new Vector3(1,1,1);
            var f = t.transform.position;
            // Ps.Add(f);
            // Debug.Log("id : " + ap.ID.ToString() + "  x : " + ap.X.ToString() + "  y: " + ap.Y.ToString());
        }
        positions.transform.localPosition += new Vector3(0,425,2305);
        positions.SetActive(false);
        GetChildren(positions);
    }

    void GetChildren(GameObject obj) {
        Transform children = obj.GetComponentInChildren<Transform>();
        //子要素がいなければ終了
        if (children.childCount == 0) {
            return;
        }
        foreach(Transform ob in children) {
            ps.Add(ob.transform.position);
            Debug.Log("  x : " + ob.transform.position.x.ToString() + "  y: " + ob.transform.position.y.ToString());
        }
    }

}
