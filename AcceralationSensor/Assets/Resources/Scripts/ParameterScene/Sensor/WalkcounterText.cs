using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * メモ
 * 
 * ・三角関数の計算は弧度法(radian)で行われるので
 * 　度数法(degree)での値を使うときはMathf.Deg2Radで直す
 **/


public class WalkcounterText : MonoBehaviour
{

    private GUIStyle labelStyle;    //テキスト表示のためのラベル
    private int digit;              //少数をいくつまで表示するか
    public MapPlot mapplot;         //MapPlotに値を渡す
    public RouteCalcurator rc;
    private bool visible = true;


    //センサー関連の情報を保存する
    private Vector3 gravity;        //重力加速度
    private Vector3 euler;          //端末の姿勢(オイラー角、重力加速度から)
    private Vector3 acceleration;   //端末の加速度
    private Vector3 accvertical;    //端末の鉛直方向の加速度
    private float absaccvertical;
    private Vector3 geomagnetism;   //地磁気センサーの3次元ベクトル

    private float compass = 0;        //コンパス(0:北)

    //StepDitectionに使う変数たち
    private int step = 0;//歩数
    // //オープンソースなので自分が分かる範囲だけコメント記述
    // private float[] mLastValues = new float[3 * 2];//最後の加速度(x,y,z平均)の値
    // private float[] mScale = new float[2];//不明
    // private float mYOffset;//不明
    // private float[] mLastDirections = new float[3 * 2];//最後の鉛直方向の加速度が上か下か
    // private float[][] mLastExtremes = { new float[3 * 2], new float[3 * 2] };//最後の極大値, 極小値
    // private float[] mLastDiff = new float[3 * 2];//最後の極大, 極小値の差(絶対値)
    // private int mLastMatch = -1;//歩行検知時の加速方向の向き
    // private float diff = 0;//極大, 極小値の差(絶対値)
    // private float mLimit;//diff下限(この値未満は無視)
    // private float lLimit;//diff上限(この値より上は無視)
    // private static long now = 0;//検知したときの時間
    // private static long last = 0;//前検知したときの時間
    // private int stepcooltime;//連続検知回避用クールタイム

    //StepDitection2に使う
    bool threshold = false;
    float acc;
    float maxlimit = 0.09f;
    float minlimit = 0.04f;
    float maxoverlimit = 0.2f;
    float minoverlimit = 0.03f;
    int _step = 0;

    //距離計測用
    private float height = 170;
    private float stride = 170 * 0.45f;//cm
    public static Vector2 distance = new Vector2(-1.5f, 27.7f);
    public Userstate user = new Userstate();

    //TestCompass
    private Vector3 H;


    // Start is called before the first frame update
    void Start()
    {
        //gameObject.SetActive(false);

        //フォント生成
        this.labelStyle = new GUIStyle();
        this.labelStyle.fontSize = Screen.height / 22;
        this.labelStyle.normal.textColor = Color.white;

        Debug.Log(string.Format("<b>精度</b>：{0}", Input.compass.headingAccuracy));
        Debug.Log(string.Format("<b>タイムスタンプ</b>：{0}", Input.compass.timestamp));

        Input.compass.enabled = true;
        Input.gyro.enabled = true;
        this.acceleration = Input.gyro.userAcceleration;
        this.geomagnetism = Input.compass.rawVector;

        //InitStepDitection();

        user.position = new Vector2(-1.5f, 27.7f);
        user.floor = 1;

    }

    // Update is called once per frame
    void Update()
    {
        this.gravity = Input.acceleration - Input.gyro.userAcceleration;

        filterAccelValue();
        filterAccelGeomagnetism();

        CalcAttitude();
        CalcAccvertical();
        //StepDitection();
        StepDitection2();
        TestCompass();

    }

    void OnGUI()
    {
        if (visible == true)
        {
            digit = 8;
            if (gravity != null)
            {
                float x = Screen.width / 20;
                float y = 0;
                float w = Screen.width * 8 / 10;
                float h = Screen.height / 20;

                for (int i = 0; i <= 20; i++)
                {
                    y = h * i;
                    string text = string.Empty;

                    switch (i)
                    {
                        case 0:
                            text = string.Format("gravity-X:{0}", System.Math.Round(this.gravity.x, digit));
                            break;
                        case 1:
                            text = string.Format("gravity-Y:{0}", System.Math.Round(this.gravity.y, digit));
                            break;
                        case 2:
                            text = string.Format("gravity-Z:{0}", System.Math.Round(this.gravity.z, digit));
                            break;
                        case 3:
                            text = string.Format("rV-X:{0}", System.Math.Round(Input.compass.rawVector.x, digit));
                            break;
                        case 4:
                            text = string.Format("rV-Y:{0}", System.Math.Round(Input.compass.rawVector.y, digit));
                            break;
                        case 5:
                            text = string.Format("rV-Z:{0}", System.Math.Round(Input.compass.rawVector.z, digit));
                            break;
                        case 6:
                            text = string.Format("H-X:{0}", H.x);
                            break;
                        case 7:
                            text = string.Format("H-Y:{0}", H.y);
                            break;
                        case 8:
                            text = string.Format("H-Z:{0}", H.z);
                            break;
                        case 9:
                            text = string.Format("accv:{0}", absaccvertical);
                            break;
                        case 10:
                            text = string.Format("Scompass:{0}", System.Math.Round(Input.compass.magneticHeading, digit));
                            break;
                        case 11:
                            text = string.Format("compass:{0}", System.Math.Round(user.direction, digit));
                            break;
                        case 12:
                            text = string.Format("Step:{0}", step);
                            break;
                        case 13:
                            text = string.Format("pos-X:{0}", user.position.x);
                            break;
                        case 14:
                            text = string.Format("pos-Y:{0}", user.position.y);
                            break;
                        case 15:
                            text = string.Format("acc:{0}", acc);
                            break;
                        default:
                            break;
                    }

                    GUI.Label(new Rect(x, y, w, h), text, this.labelStyle);
                }
            }
        }
    }

    //端末の鉛直方向の重力加速度計算
    private void CalcAccvertical()
    {
        accvertical.x = acceleration.x * Mathf.Sin(Mathf.Deg2Rad * euler.x);
        accvertical.y = acceleration.y * Mathf.Sin(Mathf.Deg2Rad * euler.y);
        accvertical.z = acceleration.z * Mathf.Sin(Mathf.Deg2Rad * euler.z);

        //absaccvertical = Mathf.Sqrt(Mathf.Pow(accvertical.x, 2) + Mathf.Pow(accvertical.y, 2) + Mathf.Pow(accvertical.z, 2));
        absaccvertical = accvertical.x + accvertical.y + accvertical.z;
    }

    //端末の姿勢を計算
    private void CalcAttitude()
    {
        euler.x = Mathf.Atan2(gravity.x, Mathf.Sqrt(Mathf.Pow(gravity.y, 2) + Mathf.Pow(gravity.z, 2))) * 180 / (float)3.14;
        euler.y = Mathf.Atan2(gravity.y, Mathf.Sqrt(Mathf.Pow(gravity.x, 2) + Mathf.Pow(gravity.z, 2))) * 180 / (float)3.14;
        euler.z = Mathf.Atan2(-gravity.z, Mathf.Sqrt(Mathf.Pow(gravity.x, 2) + Mathf.Pow(gravity.y, 2))) * 180 / (float)3.14;
    }

    //引数のVector3の絶対値を返す
    private float CalcVectorAbs(Vector3 v)
    {
        //acceleration = Input.gyro.userAcceleration;
        return Mathf.Sqrt(Mathf.Pow(v.x, 2) + Mathf.Pow(v.y, 2) + Mathf.Pow(v.z, 2));
        //acceleration = acceleration * 0.9f + Input.gyro.userAcceleration * 0.1f;
    }

    //加速度センサーのノイズ除去
    private void filterAccelValue()
    {
        float k = 0.8f;

        acceleration = Input.gyro.userAcceleration - (Input.gyro.userAcceleration * k) + acceleration * (1 - k);
    }

    //地磁気センサーのノイズ除去
    private void filterAccelGeomagnetism()
    {
        float k = 0.8f;

        geomagnetism = Input.compass.rawVector - (Input.compass.rawVector * k) + geomagnetism * (1 - k);
    }

    //方位計算
    // private void CorrectCompus()
    // {

    //     //compass = Input.compass.trueHeading;      //Unityでサポートされてる方法で方位取得
    //     //compass = (360 + 270 + Mathf.Atan2(geomagnetism.y, geomagnetism.x) * Mathf.Rad2Deg) % 360;   //3次元地磁気ベクトルから計算(傾き補正なし)

    //     /*
    //     端末の傾きによるズレを補正する
    //     下記サイト参考
    //     https://myenigma.hatenablog.com/entry/2016/04/10/211919#3%E8%BB%B8%E5%9C%B0%E7%A3%81%E6%B0%97%E3%82%BB%E3%83%B3%E3%82%B5%E3%81%AB%E3%81%8A%E3%81%91%E3%82%8B%E6%96%B9%E4%BD%8D%E8%A8%88%E7%AE%97%E3%81%AE%E6%96%B9%E6%B3%95

    //     https://www.nxp.com/docs/en/application-note/AN4248.pdf
    //      */
    //     float roll = Mathf.Atan2(gravity.y, gravity.z);
    //     float pitch = Mathf.Atan2(-gravity.x, gravity.y * Mathf.Sin(roll) + gravity.z * Mathf.Cos(roll));

    //     float _y = geomagnetism.z * Mathf.Sin(roll) - geomagnetism.y * Mathf.Cos(roll);
    //     float _x = geomagnetism.x * Mathf.Cos(pitch) + geomagnetism.y * Mathf.Sin(roll) * Mathf.Sin(pitch) + geomagnetism.z * Mathf.Sin(pitch) * Mathf.Cos(roll);


    //     compass = (270f + Mathf.Atan2(_y, _x) * Mathf.Rad2Deg) % 360;     //地磁気ベクトルから計算(傾き補正あり)
    // }

    public void TestCompass()
    {
        Vector3 Vao = new Vector3(0.0372f, 0.0510f, -0.9412f);
        Vector3 Va = new Vector3(gravity.x, gravity.y, gravity.z);

        float alpha = -Mathf.Acos(Vector3.Dot(Va, Vao) / (Va.magnitude * Vao.magnitude));
        Vector3 Vcro = Vector3.Cross(Va, Vao);
        Vector3 n = Vcro / Vcro.magnitude;

        float[,] mat = new float[3, 3];
        float tmp = 1 - Mathf.Cos(alpha);

        mat[0, 0] = n.x * n.x * tmp + Mathf.Cos(alpha);
        mat[0, 1] = n.x * n.y * tmp + n.z * Mathf.Sin(alpha);
        mat[0, 2] = n.x * n.z * tmp + n.y * Mathf.Sin(alpha);
        mat[1, 0] = n.y * n.x * tmp + n.z * Mathf.Sin(alpha);
        mat[1, 1] = n.y * n.y * tmp + Mathf.Cos(alpha);
        mat[1, 2] = n.y * n.z * tmp + n.x * Mathf.Sin(alpha);
        mat[2, 0] = n.z * n.x * tmp + n.y * Mathf.Sin(alpha);
        mat[2, 1] = n.z * n.y * tmp + n.x * Mathf.Sin(alpha);
        mat[2, 2] = n.z * n.z * tmp + Mathf.Cos(alpha);

        H = Vector3.zero;

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                H[i] += mat[i, j] * Input.compass.rawVector[j];
            }
        }

        user.direction = (270f + Mathf.Atan2(H.y, H.x) * Mathf.Rad2Deg) % 360;     //地磁気ベクトルから計算(傾き補正あり)
    }

    //StepDitectionで使う変数を初期化
    // private void InitStepDitection()
    // {
    //     int h = 480;
    //     mYOffset = h * 0.5f;
    //     //float STANDARD_GRAVITY = 9.80665f;//定数
    //     float MAGNETIC_FIELD_EARTH_MAX = 60.0f;//定数
    //                                            //mScale[0] = -(h * 0.5f * (1.0f / (STANDARD_GRAVITY * 2)));//不明
    //     mScale[1] = -(h * 0.5f * (1.0f / (MAGNETIC_FIELD_EARTH_MAX)));//不明
    //     mLimit = 0.6f;//diff下限(要調整)
    //     lLimit = 2.0f;//diff上限(要調整)
    //     stepcooltime = 150; //連続検知回避用クールタイム(要調整)
    // }

    //歩行検知関数(GutHubより)
    /*
     https://github.com/bagilevi/android-pedometer
     src/name/bagi/levente/pedometer/StepDisplayer.java
     javaで書かれてたのでC#様に書き換え、その他いろいろ調整
         */
    //分かる範囲でコメント記述(間違えている可能性あり)
    //private void StepDitection()
    //{
    //    //加速度取得
    //    Vector3 acc = Input.acceleration;

    //    //加速度x,y,zの(おそらく鉛直方向の)合計
    //    float vSum = 0;
    //    for (int i = 0; i < 3; i++)
    //    {
    //        vSum += mYOffset + acc[i] * mScale[1];
    //    }

    //    int k = 0;
    //    //平均をとる
    //    float v = vSum / 3;

    //    //直前の加速度よりvが大きい場合(上向きに加速している場合?)1,　
    //    //小さい場合(下向きに加速している場合?) -1,
    //    //同じなら0
    //    //三項演算子で書いてる
    //    float direction = (v > mLastValues[k] ? 1 : (v < mLastValues[k] ? -1 : 0));

    //    //加速方向が変化しているか
    //    if (direction == -mLastDirections[k])
    //    {
    //        // 変化している

    //        // minumum or maximum?
    //        //0…極小値, 1…極大値
    //        int extType = (direction > 0 ? 0 : 1);

    //        //極小or極大の値を更新
    //        mLastExtremes[extType][k] = mLastValues[k];
    //        //極小値, 極大値の差を絶対値で取得
    //        diff = Mathf.Abs(mLastExtremes[extType][k] - mLastExtremes[1 - extType][k]);

    //        //差が下限より上, 上限未満か判定
    //        if (diff > mLimit && diff < lLimit)
    //        {
    //            //歩行してるかの判定(誤検知回避)
    //            //場合によっては調整

    //            bool isAlmostAsLargeAsPrevious = diff > (mLastDiff[k] * 2 / 3);//diffは直前と同じぐらい十分大きいか
    //            bool isPreviousLargeEnough = mLastDiff[k] > (diff / 3);//直前のdiffは十分大きいか
    //            bool isNotContra = (mLastMatch != 1 - extType);//前回歩数カウントした時とは違う加速方法であるか

    //            if (isAlmostAsLargeAsPrevious && isPreviousLargeEnough && isNotContra)
    //            {
    //                //検知した瞬間の時間を取得
    //                now = System.Environment.TickCount;
    //                //連続検知回避用クールタイム(要調整)
    //                if (now - last > stepcooltime && absaccvertical > 0.05)
    //                {
    //                    //歩いた判定
    //                    //歩数カウント, 加速方向, 瞬間の時間を記録
    //                    step++;
    //                    mLastMatch = extType;
    //                    last = now;

    //                    //移動距離計算
    //                    CalcDistance();
    //                }
    //            }
    //            else
    //            {
    //                //次はどっちの加速方向でも構わない
    //                mLastMatch = -1;
    //            }
    //        }
    //        //今回のdiffを保存
    //        mLastDiff[k] = diff;
    //    }
    //    //今回のdirection, vを保存
    //    mLastDirections[k] = direction;
    //    mLastValues[k] = v;
    //}

    private void StepDitection2()
    {
        acc = CalcVectorAbs(acceleration);

        if (threshold == false && acc > maxlimit)
        {
            step++;
            CalcDistance();
            threshold = true;
        }
        else if (threshold == true && acc < minlimit)
        {
            threshold = false;
        }
    }

    //距離計算
    private void CalcDistance()
    {
        float compass_offset = 180;

        distance.x += stride * Mathf.Cos(Mathf.Deg2Rad * (compass + compass_offset)) / 100;
        distance.y -= stride * Mathf.Sin(Mathf.Deg2Rad * (compass + compass_offset)) / 100;

        mapplot.AddNewPosition(distance);
    }

    public void setVisible(bool b)
    {
        visible = b;
    }
}

public class Userstate
{
    public Vector2 position { get; set; }
    public float direction { get; set; }
    public int floor { get; set; }

    public Userstate()
    {
    }

    public Userstate(Vector2 pos, float dir, int fl)
    {
        position = pos;
        direction = dir;
        floor = fl;
    }
}