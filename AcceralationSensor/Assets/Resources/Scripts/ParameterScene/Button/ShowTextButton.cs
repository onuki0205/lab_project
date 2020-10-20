using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowTextButton : MonoBehaviour
{
    public WalkcounterText wtex;
    public MapPlot mplot;

    public void OnClick()
    {
        wtex.setVisible(true);
        mplot.setVisible(false);
    }
}
