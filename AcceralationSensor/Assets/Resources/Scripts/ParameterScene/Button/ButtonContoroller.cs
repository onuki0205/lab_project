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
    private Button[] blist;

    private void Start()
    {
        blist = this.GetComponentsInChildren<Button>();

        for (int i = 0; i < blist.Length; i++)
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
                SceneManager.LoadScene("ARScene");
                break;
            //selectdestinationButtton
            case 3:
                c.GetComponentInChildren<WalkcounterText>().setVisible(false);
                c.GetComponentInChildren<MapPlot>().setVisible(false);

                for (int i = 0; i < blist.Length; i++)
                {
                    if (blist[i].GetComponent<BaseButton>().buttontype >= 4 && blist[i].GetComponent<BaseButton>().buttontype <= 6)
                    {
                        blist[i].gameObject.SetActive(true);
                    }
                    else
                    {
                        blist[i].gameObject.SetActive(false);
                    }
                }

                this.GetComponentInChildren<SelectDestination>().setVisible(true);
                cam.GetComponentInChildren<CameraController>().setFrozen(true);
                break;
            //目的地設定画面 LeftButton
            case 4:
                sd.buttonpressed(0);
                break;
            //目的地設定画面 RightButton
            case 5:
                sd.buttonpressed(1);
                break;
            //目的地設定画面 OKButton
            case 6:
                sd.buttonpressed(2);
                break;
            default:
                break;
        }

    }
}
