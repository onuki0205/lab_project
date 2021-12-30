using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ARButtonController : MonoBehaviour
{
    private List<GameObject> blist = new List<GameObject>();
    private GameObject donebutton;
    // Start is called before the first frame update
    void Start()
    {
        //makeButton(new Vector2(Screen.width / 2, Screen.height / 20), 0, 0, "Return");
        //donebutton = makeButton(new Vector2(Screen.width / 2, Screen.height * 2 / 20), 0, 0, "Done");
    }

    public void SelectedButton(int type)
    {
        switch (type)
        {
            case 0:
                GameObject.Find("Canvas1").GetComponentInChildren<ValueSet>().setVisible(true);
                SceneManager.LoadScene("MainScene");
                break;
            case 1:

                break;
        }
    }

    public GameObject makeButton(Vector2 pos, int type, int group, string tex)
    {
        GameObject b = Instantiate(Resources.Load("Prehabs/BaseButton") as GameObject, new Vector3(pos.x, pos.y, 0f), Quaternion.identity);
        b.transform.parent = this.transform;
        b.GetComponentInChildren<Text>().text = tex;
        b.GetComponent<MainBaseButton>().buttontype = type;
        b.GetComponent<MainBaseButton>().buttongroup = group;
        blist.Add(b);

        return b;
    }

    private void setbuttongroupstate(int group, bool show)
    {
        for (int i = 0; i < blist.Count; i++)
        {
            if (blist[i].GetComponent<MainBaseButton>().buttongroup == group)
            {
                blist[i].gameObject.SetActive(show);
            }
        }
    }
}
