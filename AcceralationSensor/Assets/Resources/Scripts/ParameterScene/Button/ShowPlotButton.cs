using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowPlotButton : MonoBehaviour
{
    public WalkcounterText wtex;
    public MapPlot mplot;

    public void OnClick()
    {
        wtex.setVisible(false);
        mplot.setVisible(true);
    }
}
