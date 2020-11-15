using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceParameterText : MonoBehaviour
{
    private GUIStyle labelStyle;    //テキスト表示のためのラベル
    private int digit = 8;              //少数をいくつまで表示するか
    public Userstate user = null;
    // Start is called before the first frame update
    void Start()
    {
        this.labelStyle = new GUIStyle();
        this.labelStyle.fontSize = Screen.height / 22;
        this.labelStyle.normal.textColor = Color.white;

       user = GameObject.Find("WalkcounterText").GetComponent<WalkcounterText>().user;
       user.position = new Vector2(0,0);
    }

    // Update is called once per frame
    void OnGUI()
    {

        float x = Screen.width / 20;
        float y = 0;
        float w = Screen.width * 8 / 10;
        float h = Screen.height / 20;


        for (int i = 0; i <= 1; i++)
        {
            y = h * i;
            string text = string.Empty;

            switch (i)
            {
                case 0:
                    text = string.Format("distance-X:{0}", System.Math.Round(user.position.x, digit));
                    break;
                case 1:
                    text = string.Format("distance-Y:{0}", System.Math.Round(user.position.y, digit));
                    break;
            }


            GUI.Label(new Rect(x, y, w, h), text, this.labelStyle);
        }

    }
}
