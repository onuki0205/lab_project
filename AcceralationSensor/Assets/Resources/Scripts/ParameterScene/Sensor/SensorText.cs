using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcceralatorText : MonoBehaviour
{
    private Vector3 gravity;        //重力加速度
    private Vector3 acceleration;   //端末の加速度
    private Compass compass;        //コンパス(北)
    //private Quaternion gyro;        //端末の姿勢(クォータニオン)
    //private Vector3 gyroacc;        //角速度
    private Vector3 euler;          //端末の姿勢(オイラー角、重力加速度から)
    private GUIStyle labelStyle;
    private int digit;

    float AccelerometerUpdateInterval = 1.0f / 30.0f;
    float LowPassKernelWidthInSeconds = 1.0f;
    float LowPassFilterFactor = 0;
    Vector3 lowPassValue = Vector3.zero;

    private Vector3 movedist;
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

        //Filter Accelerometer
        LowPassFilterFactor = AccelerometerUpdateInterval / LowPassKernelWidthInSeconds;
        acceleration = Input.gyro.userAcceleration;

        CalcAttitude();
        Input.compass.enabled = true;
        Input.gyro.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        this.gravity = Input.acceleration - Input.gyro.userAcceleration;
        //this.acceleration = Input.gyro.userAcceleration;
        this.compass = Input.compass;
        //this.gyro = Input.gyro.attitude;
        //this.gyroacc = Input.gyro.rotationRateUnbiased;
        CalcAttitude();
        filterAccelValue();
        CalcDistance();
    }

    void OnGUI()
    {
        digit = 8;
        if (gravity != null)
        {
            float x = Screen.width / 20;
            float y = 0;
            float w = Screen.width * 8 / 10;
            float h = Screen.height / 20;

            for (int i = 0; i <= 17; i++)
            {
                y = h * i;
                string text = string.Empty;

                switch (i)
                {
                    case 0://X
                        text = string.Format("gravity-X:{0}", System.Math.Round(this.gravity.x, digit));
                        break;
                    case 1://Y
                        text = string.Format("gravity-Y:{0}", System.Math.Round(this.gravity.y, digit));
                        break;
                    case 2://Z
                        text = string.Format("gravity-Z:{0}", System.Math.Round(this.gravity.z, digit));
                        break;
                    case 3://X
                        text = string.Format("accel-X:{0}", System.Math.Round(this.acceleration.x, digit));
                        break;
                    case 4://Y
                        text = string.Format("accel-Y:{0}", System.Math.Round(this.acceleration.y, digit));
                        break;
                    case 5://Z
                        text = string.Format("accel-Z:{0}", System.Math.Round(this.acceleration.z, digit));
                        break;
                    //case 3://X
                    //    text = string.Format("comps-X:{0}", System.Math.Round(this.compass.rawVector.x, digit));
                    //    break;
                    //case 4://Y
                    //    text = string.Format("comps-Y:{0}", System.Math.Round(this.compass.rawVector.y, digit));
                    //    break;
                    //case 5://Z
                    //    text = string.Format("comps-Z:{0}", System.Math.Round(this.compass.rawVector.z, digit));
                    //    break;
                    case 6://Z
                        text = string.Format("magHeading:{0}", System.Math.Round(this.compass.magneticHeading, digit));
                        break;
                    case 7://Z
                        text = string.Format("trueHeading:{0}", System.Math.Round(this.compass.trueHeading, digit));
                        break;
                    //case 6://X
                    //    text = string.Format("gyroacc-x:{0}", System.Math.Round(this.gyroacc.x, digit));
                    //    break;
                    //case 7://Y
                    //    text = string.Format("gyroacc-y:{0}", System.Math.Round(this.gyroacc.y, digit));
                    //    break;
                    //case 8://Y
                    //    text = string.Format("gyroacc-z:{0}", System.Math.Round(this.gyroacc.z, digit));
                    //    break;
                    //case 9://X
                    //    text = string.Format("gyroQ-x:{0}", System.Math.Round(this.gyro.x, digit));
                    //    break;
                    //case 10://Y
                    //    text = string.Format("gyroQ-y:{0}", System.Math.Round(this.gyro.y, digit));
                    //    break;
                    //case 11://Y
                    //    text = string.Format("gyroQ-z:{0}", System.Math.Round(this.gyro.z, digit));
                    //    break;
                    //case 12://Y
                    //    text = string.Format("gyroQ-w:{0}", System.Math.Round(this.gyro.w, digit));
                    //    break;
                    case 8://X
                        text = string.Format("gyroE-x:{0}", System.Math.Round(this.euler.x, digit));
                        break;
                    case 9://Y
                        text = string.Format("gyroE-y:{0}", System.Math.Round(this.euler.y, digit));
                        break;
                    case 10://Y
                        text = string.Format("gyroE-z:{0}", System.Math.Round(this.euler.z, digit));
                        break;
                    case 11://Y
                        text = string.Format("movedist-x:{0}", System.Math.Round(this.movedist.x, digit));
                        break;
                    case 12://Y
                        text = string.Format("movedist-y:{0}", System.Math.Round(this.movedist.y, digit));
                        break;
                    case 13://Y
                        text = string.Format("movedist-y:{0}", System.Math.Round(this.movedist.z, digit));
                        break;

                    default:
                        throw new System.InvalidOperationException();
                }

                GUI.Label(new Rect(x, y, w, h), text, this.labelStyle);
            }
        }
    }

    //private Vector3 QuaternionToEular(Quaternion q)
    //{

    //    float x = q.x;
    //    float y = q.y;
    //    float z = q.z;
    //    float w = q.w;

    //    float x2 = x * x;
    //    float y2 = y * y;
    //    float z2 = z * z;

    //    float xy = x * y;
    //    float xz = x * z;
    //    float yz = y * z;
    //    float wx = w * x;
    //    float wy = w * y;
    //    float wz = w * z;

    //    1 - 2y ^ 2 - 2z ^ 2
    //    float m00 = 1f - (2f * y2) - (2f * z2);

    //    2xy + 2wz
    //    float m01 = (2f * xy) + (2f * wz);

    //    2xy - 2wz
    //    float m10 = (2f * xy) - (2f * wz);

    //    1 - 2x ^ 2 - 2z ^ 2
    //    float m11 = 1f - (2f * x2) - (2f * z2);

    //    2xz + 2wy
    //    float m20 = (2f * xz) + (2f * wy);

    //    2yz + 2wx
    //    float m21 = (2f * yz) - (2f * wx);

    //    1 - 2x ^ 2 - 2y ^ 2
    //    float m22 = 1f - (2f * x2) - (2f * y2);


    //    float tx, ty, tz;

    //    if (Mathf.Approximately(m21, 1f))
    //    {
    //        tx = Mathf.PI / 2f;
    //        ty = 0;
    //        tz = Mathf.Atan2(m10, m00);
    //    }
    //    else if (Mathf.Approximately(m21, -1f))
    //    {
    //        tx = -Mathf.PI / 2f;
    //        ty = 0;
    //        tz = Mathf.Atan2(m10, m00);
    //    }
    //    else
    //    {
    //        tx = Mathf.Asin(-m21);
    //        ty = Mathf.Atan2(m20, m22);
    //        tz = Mathf.Atan2(m01, m11);
    //    }

    //    tx *= Mathf.Rad2Deg;
    //    ty *= Mathf.Rad2Deg;
    //    tz *= Mathf.Rad2Deg;

    //    return new Vector3(tx, ty, tz);
    //}

    private void CalcAttitude()
    {
        euler.x = Mathf.Atan2(gravity.x, Mathf.Sqrt(Mathf.Pow(gravity.y, 2) + Mathf.Pow(gravity.z, 2))) * 180 / (float)3.14;
        euler.y = Mathf.Atan2(gravity.y, Mathf.Sqrt(Mathf.Pow(gravity.x, 2) + Mathf.Pow(gravity.z, 2))) * 180 / (float)3.14;
        euler.z = Mathf.Atan2(-gravity.z, Mathf.Sqrt(Mathf.Pow(gravity.x, 2) + Mathf.Pow(gravity.y, 2))) * 180 / (float)3.14;
    }

    private void filterAccelValue()
    {
        //acceleration = Input.gyro.userAcceleration;
        acceleration = Vector3.Lerp(acceleration, Input.gyro.userAcceleration, LowPassFilterFactor);
        //acceleration = acceleration * 0.9f + Input.gyro.userAcceleration * 0.1f;
    }

    private void CalcDistance()
    {
        if (Mathf.Abs(acceleration.x) > 0.01f)
        {
            movedist.x += acceleration.x;
        }
        if (Mathf.Abs(acceleration.y) > 0.01f)
        {
            movedist.y += acceleration.y;
        }
        if (Mathf.Abs(acceleration.z) > 0.01f)
        {
            movedist.z += acceleration.z;
        }
    }
}
