using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ImageController : MonoBehaviour,IPointerClickHandler,IPointerDownHandler
{
    bool isPinch =false;
  float time=0;      //時間計測
  Vector3 sPos,nPos; //タッチした座標,座標間距離
//   Quaternion sRot;   //タッチしたときの回転
  float vMin = 0.1f , vMax = 3.0f, sDist = 0.0f, nDist = 0.0f; //倍率制限,距離変数
  static Vector3 initScale; //最初の大きさ
  static float v = 1.0f; //現在倍率
 
  float V2NmlzCross(Vector2 v1,Vector2 v2){
    return v1.normalized.x*v2.normalized.y - v1.normalized.y*v2.normalized.x;
  }
  void Update(){
    time += Time.deltaTime;
    if(Input.touchCount == 1) { //ドラッグ
      if (Input.GetTouch(0).phase == TouchPhase.Began || isPinch){
        sPos = Input.mousePosition; isPinch = false;
      } else {
        this.transform.position += (Input.mousePosition - sPos) * 1/30;
        sPos = Input.mousePosition;
      }
    }else if(Input.touchCount >= 2) { //ピンチイン ピンチアウト
      isPinch = true;
      time=99;
      Touch t1 = Input.GetTouch (0), t2 = Input.GetTouch (1);
      if (t2.phase == TouchPhase.Began) {
        initScale = this.transform.localScale; v=1f;
        sDist = Vector2.Distance (t1.position, t2.position);
        // sRot  = this.transform.rotation;
        sPos  = t2.position - t1.position;
      } else if ((t1.phase == TouchPhase.Moved||t1.phase == TouchPhase.Stationary) &&
                 (t2.phase == TouchPhase.Moved||t2.phase == TouchPhase.Stationary) ) {
        nDist = Vector2.Distance (t1.position, t2.position);
        v = Mathf.Clamp(v + (nDist - sDist) / Screen.width , vMin , vMax);
        this.transform.localScale = initScale * v;
        sDist = nDist;
 
        nPos=t2.position-t1.position;
        // this.transform.Rotate(
        //   new Vector3(0,0,Vector2.Angle(sPos,nPos) * Mathf.Sign( V2NmlzCross(sPos,nPos) ) )
        // );
        sPos=nPos;
      }
    }
  }
  public void OnPointerDown( PointerEventData data ){ time=0; }
  public void OnPointerClick( PointerEventData data ){
    if(Input.touchCount<2&&time<0.2f){
      /* TAP Event */
    }
  }
}
