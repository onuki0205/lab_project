using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonContoroller : MonoBehaviour
{
    public Canvas c;
    public Camera cam;
    public SelectDestination sd;
    private List<GameObject> blist = new List<GameObject>();

    private void Start()
    {

        makeButton(new Vector2(Screen.width / 6, Screen.height / 20), 0, "Text");
        makeButton(new Vector2(Screen.width * 3 / 6, Screen.height / 20), 1, "Plot");
        makeButton(new Vector2(Screen.width * 5 / 6, Screen.height / 20), 2, "AR");
        makeButton(new Vector2(Screen.width * 3 / 6, Screen.height * 3 / 20), 3, "Desti");

        makeButton(new Vector2(Screen.width / 6, Screen.height * 3 / 20), 4, "Left");
        makeButton(new Vector2(Screen.width * 5 / 6, Screen.height * 3 / 20), 5, "Right");
        makeButton(new Vector2(Screen.width * 3 / 6, Screen.height * 3 / 20), 6, "OK");
        makeButton(new Vector2(Screen.width * 3 / 6, Screen.height / 20), 7, "Cancel");
        makeButton(new Vector2(Screen.width / 6, Screen.height * 5 / 20), 8, "1F");
        makeButton(new Vector2(Screen.width * 3 / 6, Screen.height * 5 / 20), 9, "2F");
        makeButton(new Vector2(Screen.width * 5 / 6, Screen.height * 5 / 20), 10, "3F");

        for (int i = 0; i < blist.Count; i++)
        {
            if (blist[i].GetComponent<BaseButton>().buttontype < 4)
            {
                blist[i].gameObject.SetActive(true);
            }
            else
            {
                blist[i].gameObject.SetActive(false);
            }
        }
    }

    public void setCanvas()
    {

        c = GameObject.FindWithTag("DontDestroyObj").GetComponent<Canvas>();

    }

    public void SelectedButton(int type)
    {
        switch (type)
        {
            //TextButton
            case 0:
                c.GetComponentInChildren<WalkcounterText>().setVisible(true);
                c.GetComponentInChildren<MapPlot>().setVisible(false);
                this.GetComponentInChildren<SelectDestination>().setVisible(false);

                cam.GetComponentInChildren<CameraController>().setFrozen(true);


                break;
            //PlotButton
            case 1:
                c.GetComponentInChildren<WalkcounterText>().setVisible(false);
                c.GetComponentInChildren<MapPlot>().setVisible(true);
                this.GetComponentInChildren<SelectDestination>().setVisible(false);

                cam.GetComponentInChildren<CameraController>().setFrozen(false);
                break;
            //ARButton
            case 2:
                c.GetComponentInChildren<WalkcounterText>().setVisible(false);
                c.GetComponentInChildren<MapPlot>().setVisible(false);
                this.GetComponentInChildren<SelectDestination>().setVisible(false);
                cam.GetComponentInChildren<CameraController>().setFrozen(false);

                SceneManager.LoadScene("ARScene");
                break;
            //selectdestinationButtton
            case 3:
                for (int i = 0; i < blist.Count; i++)
                {
                    if (blist[i].GetComponent<BaseButton>().buttontype >= 4 && blist[i].GetComponent<BaseButton>().buttontype <= 10)
                    {
                        blist[i].gameObject.SetActive(true);
                    }
                    else
                    {
                        blist[i].gameObject.SetActive(false);
                    }
                }

                c.GetComponentInChildren<WalkcounterText>().setVisible(false);
                c.GetComponentInChildren<MapPlot>().setVisible(false);
                this.GetComponentInChildren<SelectDestination>().setVisible(true);

                cam.GetComponentInChildren<CameraController>().setFrozen(true);
                break;
            //目的地設定画面 LeftButton
            case 4:
                sd.PageChange(0);
                break;
            //目的地設定画面 RightButton
            case 5:
                sd.PageChange(1);
                break;
            //目的地設定画面 OKButton
            case 6:
                sd.setDestination();
                break;
            //目的地設定画面　CancelButton
            case 7:
                for (int i = 0; i < blist.Count; i++)
                {
                    if (blist[i].GetComponent<BaseButton>().buttontype < 4)
                    {
                        blist[i].gameObject.SetActive(true);
                    }
                    else
                    {
                        blist[i].gameObject.SetActive(false);
                    }
                }

                c.GetComponentInChildren<WalkcounterText>().setVisible(true);
                c.GetComponentInChildren<MapPlot>().setVisible(false);
                this.GetComponentInChildren<SelectDestination>().setVisible(false);

                cam.GetComponentInChildren<CameraController>().setFrozen(true);
                break;
            //目的地選択画面 1FButton
            case 8:
                sd.ChangeFloor(1);
                break;
            //目的地選択画面 2FButton

            case 9:
                sd.ChangeFloor(2);
                break;
            //目的地選択画面 3FButton
            case 10:
                sd.ChangeFloor(3);
                break;
            default:
                break;
        }
    }

    public void makeButton(Vector2 pos, int type, string tex)
    {
        GameObject b = Instantiate(Resources.Load("Prehabs/BaseButton") as GameObject, new Vector3(pos.x, pos.y, 0f), Quaternion.identity);
        b.transform.parent = this.transform;
        b.GetComponentInChildren<Text>().text = tex;
        b.GetComponent<BaseButton>().buttontype = type;
        blist.Add(b);
    }
}
