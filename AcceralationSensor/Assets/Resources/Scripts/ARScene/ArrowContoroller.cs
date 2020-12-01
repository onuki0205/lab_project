﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowContoroller : MonoBehaviour
{
    public Userstate user = null;

    // Start is called before the first frame update
    void Start()
    {
        user = GameObject.Find("ValueSet").GetComponent<ValueSet>().user;
    }

    // Update is called once per frame
    void Update()
    {
        if (user != null && user.route != null)
        {
            SetArrowDirection();
        }
    }

    void SetArrowDirection()
    {
        if (user.route.Count < 2)
        {
            return;
        }
        if (user.route[1] < 0)
        {
            return;
        }

        Entity_LabNode esnode = Resources.Load("Assets/Lab_NodeList") as Entity_LabNode;
        Vector2 from = new Vector2(esnode.sheets[user.floor - 1].list[user.route[0]].X, esnode.sheets[user.floor - 1].list[user.route[0]].Y);
        Vector2 to = new Vector2(esnode.sheets[user.floor - 1].list[user.route[1]].X, esnode.sheets[user.floor - 1].list[user.route[1]].Y);
        Vector2 dt = to - from;
        float rad = Mathf.Atan2(dt.x, dt.y);
        float degree = rad * Mathf.Rad2Deg;

        if (degree < 0)
        {
            degree += 360;
        }
        degree += 90;

        this.transform.rotation = Quaternion.Euler(60, user.direction - degree, 0);
    }
}
