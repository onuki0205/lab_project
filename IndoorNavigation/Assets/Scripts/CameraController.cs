using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    float scale = 100f;
    private Camera cam;
    private bool frozen = false;

    int _beforeTouchCount = 0;
    bool _firstTouch = true;
    Vector3 _beforeVec = Vector3.zero;
    float _beforeDist = 0;
    float _now_angle = 0;
    // Start is called before the first frame update
    void Start()
    {
        cam = transform.parent.gameObject.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (frozen == false)
        {
            Controller();
        }
    }

    private void Controller()
    {
        if (Application.isEditor)
        {
            if (!Input.GetMouseButton(0))
            {
                _firstTouch = true;

            }
            MoveCamera(Input.mousePosition);

            if (_firstTouch)
            {
                _firstTouch = false;
            }
            float zoom = Input.GetAxis("Mouse ScrollWheel");
            scale = scale - zoom * 30;
            if (scale < 0.1f)
            {
                scale = 0.1f;
            }
            else if (scale > 160)
            {
                scale = 160f;
            }
            cam.fieldOfView = scale;
        }
        else
        {
            //直前とタッチしている指の数が違った場合
            //_firstTouchのフラグを戻す
            if (Input.touchCount != _beforeTouchCount)
            {
                _firstTouch = true;
            }

            switch (Input.touchCount)
            {
                case 1:
                    //タッチ数が1
                    //座標移動の処理
                    MoveCamera(Input.touches[0].position);
                    break;

                case 2:
                    //タッチ数が2
                    //回転、スケール値変更の処理
                    RotateCamera(Input.touches[0].position, Input.touches[1].position);
                    ZoomCamera(Input.touches[0].position, Input.touches[1].position);
                    break;

                default:
                    break;
            }

            if (_firstTouch)
            {
                _firstTouch = false;
            }
            //タッチ数保存
            _beforeTouchCount = Input.touchCount;
        }
    }

    private void MoveCamera(Vector3 pos1)
    {
        //最初のタッチだった場
        //現在の座標を保存し、終了
        if (_firstTouch)
        {
            _beforeVec = pos1;
            return;
        }

        //直前の座標との差を計算
        Vector3 diff = (pos1 - _beforeVec) / 4;

        //現在のタッチ座標を保存
        _beforeVec = pos1;

        //座標移動の処理
        var tmppos = cam.transform.position;
        cam.transform.position = transform.position - transform.rotation * diff;

        //移動限界の設定
        if (cam.transform.position.x < -400 || cam.transform.position.x > 400)
        {
            cam.transform.position = new Vector3(tmppos.x, cam.transform.position.y, cam.transform.position.z);
        }
        if (cam.transform.position.y < -300 || cam.transform.position.y > 300)
        {
            cam.transform.position = new Vector3(cam.transform.position.x, tmppos.y, cam.transform.position.z);
        }
    }

    private void RotateCamera(Vector3 pos1, Vector3 pos2)
    {
        //最初のタッチだった場
        //2点の座標の差を取得し、終了
        if (_firstTouch)
        {
            _beforeVec = pos1 - pos2;
            return;
        }

        //二点の角度を取得
        float angle = Vector3.Angle(_beforeVec, pos1 - pos2);

        //pos1, pos2の場所に応じて角度修正(?)
        //ネットのソースなので詳しくは不明
        Vector3 cross = Vector3.Cross(_beforeVec, pos1 - pos2);
        if (cross.z < 0) angle *= -1;

        //座標の差を保存
        _beforeVec = pos1 - pos2;
        _now_angle += angle;

        //回転処理
        cam.transform.Rotate(new Vector3(0, 0, 1), -angle);
    }

    private void ZoomCamera(Vector3 pos1, Vector3 pos2)
    {
        //最初のタッチだった場
        //2点の座標の距離を取得し、終了
        if (_firstTouch)
        {
            _beforeDist = Vector3.Distance(pos1, pos2) / 500;
            return;
        }

        //2点の座標の距離を取得
        float dist = Vector3.Distance(pos1, pos2) / 500;
        //「直前の距離」に対する「現在の距離」の割合を計算し、スケール値設定
        scale = scale / (dist / _beforeDist);

        //スケール下限、上限設定
        if (scale < 0.1f)
        {
            scale = 0.1f;
        }
        else if (scale > 160)
        {
            scale = 160f;
        }

        //処理
        _beforeDist = dist;
        cam.fieldOfView = scale;
    }

    public void setFrozen(bool b)
    {
        frozen = b;
    }
}
