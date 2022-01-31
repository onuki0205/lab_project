using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoValue : MonoBehaviour
{
    public List<Vector2> positions;

    public void SetPositions(){
        positions.Add(new Vector2(-16,700));
        positions.Add(new Vector2(-16,650));
        positions.Add(new Vector2(-16,600));
    }
}
