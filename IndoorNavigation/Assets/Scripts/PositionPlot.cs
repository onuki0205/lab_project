using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionPlot : MonoBehaviour
{
    public List<Vector3> Ps = null;
    public GameObject map,line;

    // Start is called before the first frame update
    void Start()
    {
        // PositionP();
        // var lineRenderer = line.AddComponent<LineRenderer>();

        // Vector3[] pl = Ps.ToArray();
        // // 線を引く場所を指定する
        
        // for(int i = 0; i < Ps.Count; i++){
        //     lineRenderer.SetPosition(i,pl[i]);
        // }
    }

    // Update is called once per frame
    void Update()
    {
        
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
            Ps.Add(t.transform.position);
            // Debug.Log("id : " + ap.ID.ToString() + "  x : " + ap.X.ToString() + "  y: " + ap.Y.ToString());
        }
        positions.transform.localPosition += new Vector3(0,425,2305);
    }
}
