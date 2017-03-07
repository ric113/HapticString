using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sphereGenerator : MonoBehaviour
{

    /* ref : https://zh.scribd.com/document/14819165/Regressions-coniques-quadriques-circulaire-spherique*/


    const float TWOPI = 6.2831853071f;
    const float PI = 3.1415926535897f;
    const float PID2 = 1.5707963267948f;
    const float RADIUS_TO_SCALE_CONSTANS = 0.4898552f;

    static public int pointCount = 4;      /* 取樣點數 */

    private float[,] matrixArray = new float[4, 4];
    private float[] y = new float[4];
    //private Vector3[] points = new Vector3[pointCount];
    private List<Vector3> points = new List<Vector3>();
    private Matrix4x4 matrix = new Matrix4x4();
    private Vector4 yVec4, xVec4;
    private Vector3 sphereCenter;
    private float sphereRadius;


    public getLeapMotionComponent getIndexFingerInfo;
    public GameObject SphereUI;
    public GameObject LeapMotionController;
    public GameObject[] Buttons;
    private int currentPointCount = 0;
    private Vector3 offsetToEye = new Vector3(0f,0f,0f);

    private float samplingPeroid = 0.01f;
    private float samplingTime = 5f;
    private float timer = 0.0f;
    private float currentTime = 0.0f;
    private bool Sampling = false;

    /* 目標 ： -yVec4 = matrix * xVec4 , 求解 xVec4 */

    void Update()
    {
        if (Sampling)
        {
            timer += Time.deltaTime;

            if (timer >= samplingPeroid)
                getSamplePoint();

            if (currentTime >= samplingTime)
                samplingComplete();
        }
        else
        {
            if (Input.GetKeyDown("space"))
            {
                print("Start sampling!");
                Sampling = true;
            }

            if (Input.GetKeyDown("return"))
            {
                print("ReSampling!");
                resetSphereUI();
                Sampling = true;
            }
        }


        /*
        if (Input.GetKeyDown("space") && currentPointCount < 4)
        {
            points[currentPointCount] = getIndexFingerInfo.getIndexTipPos();
            currentPointCount++;

            if (currentPointCount == 4) {
                generateMatrix();
                generateY();
                calculateX();
                generateSphere();
                print("Center :" + sphereCenter + ", Radius:" + sphereRadius);

                if (invalidRadiusDetect())
                {

                    // Recollect the sample points !
                    Debug.Log("Sphere generate fail , Please re-sampling points!");
                    currentPointCount = 0;

                }
                else
                {

                    
                    // GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    // sphere.transform.position = sphereCenter;
                    // sphere.transform.localScale = new Vector3(sphereRadius, sphereRadius, sphereRadius);
                    

                    offsetToEye = sphereCenter - LeapMotionController.transform.position;
                    SphereUI.transform.localScale = new Vector3(sphereRadius / RADIUS_TO_SCALE_CONSTANS, sphereRadius / RADIUS_TO_SCALE_CONSTANS, sphereRadius / RADIUS_TO_SCALE_CONSTANS);
                    SphereUI.transform.rotation = LeapMotionController.transform.rotation;
                    SphereUI.SetActive(true);
                    Button.GetComponent<ButtonCurvedEffect>().setButton();

                }

            }

        }
        */
        SphereUI.transform.position = LeapMotionController.transform.position + offsetToEye;
        
    } 
    
    /* 產生 'matrix' */
    private void generateMatrix()
    {
        matrixArray[0, 0] = pointCount;
        float p;

        for (int i = 0; i < pointCount; i++)
        {
            matrixArray[1, 1] += Mathf.Pow(points[i].x, 2);
            matrixArray[2, 2] += Mathf.Pow(points[i].y, 2);
            matrixArray[3, 3] += Mathf.Pow(points[i].z, 2);

            matrixArray[0, 1] += points[i].x;
            matrixArray[1, 0] += points[i].x;
            matrixArray[0, 2] += points[i].y;
            matrixArray[2, 0] += points[i].y;
            matrixArray[0, 3] += points[i].z;
            matrixArray[3, 0] += points[i].z;

            matrixArray[1, 2] += points[i].x * points[i].y;
            matrixArray[2, 1] += points[i].x * points[i].y;
            matrixArray[1, 3] += points[i].x * points[i].z;
            matrixArray[3, 1] += points[i].x * points[i].z;
            matrixArray[2, 3] += points[i].z * points[i].y;
            matrixArray[3, 2] += points[i].z * points[i].y;

            p = Mathf.Pow(points[i].x, 2) + Mathf.Pow(points[i].y, 2) + Mathf.Pow(points[i].z, 2);
            y[0] += p;
            y[1] += p * points[i].x;
            y[2] += p * points[i].y;
            y[3] += p * points[i].z;
        }

        for (int i = 0; i < 4; i++)
        {
            matrix.SetColumn(i, arrayToVec4Column(i));
        }

    }

    private void generateY()
    {
        yVec4 = new Vector4(-1 * y[0], -1 * y[1], -1 * y[2], -1 * y[3]);
    }

    private void calculateX()
    {
        xVec4 = matrix.inverse * yVec4;
    }

    private void generateSphere()
    {
        float a = (float)0.5 * -1 * xVec4.y;
        float b = (float)0.5 * -1 * xVec4.z;
        float c = (float)0.5 * -1 * xVec4.w;

        sphereRadius = Mathf.Sqrt(Mathf.Pow(a, 2) + Mathf.Pow(b, 2) + Mathf.Pow(c, 2) - xVec4.x);
        sphereCenter = new Vector3(a, b, c);
    }


    /* Convert 4*4 Array to Matirx Type */
    private Vector4 arrayToVec4Column(int colNum)
    {
        Vector4 col = new Vector4(matrixArray[colNum, 0], matrixArray[colNum, 1], matrixArray[colNum, 2], matrixArray[colNum, 3]);
        return col;
    }

    private bool invalidRadiusDetect()
    {
        if  (System.Single.IsNaN(sphereRadius) || sphereRadius == 0)
            return true;
        return false;
    }

    private void getSamplePoint() {
        points.Add(getIndexFingerInfo.getIndexTipPos());
        currentTime += timer;
        // print(currentTime);
        timer = 0;
    }

    private void samplingComplete() {
        Sampling = false;
        print(points.Count);
        pointCount = points.Count;

        generateMatrix();
        generateY();
        calculateX();
        generateSphere();
        print("Center :" + sphereCenter + ", Radius:" + sphereRadius);

        if (invalidRadiusDetect())
        {
            // Recollect the sample points !
            Debug.Log("Sphere generate fail , Please re-sampling points!");
            resetSphereUI();
        }
        else
            enableSphereUI();
        
    }

    private void enableSphereUI() {

        offsetToEye = sphereCenter - LeapMotionController.transform.position;
        SphereUI.transform.localScale = 1.001f * (new Vector3(sphereRadius / RADIUS_TO_SCALE_CONSTANS, sphereRadius / RADIUS_TO_SCALE_CONSTANS, sphereRadius / RADIUS_TO_SCALE_CONSTANS));
        //SphereUI.transform.rotation = LeapMotionController.transform.rotation;
        SphereUI.SetActive(true);
        for (int i = 0; i < Buttons.Length; i++)
        {
            Buttons[i].GetComponent<ButtonCurvedEffect>().setButton();
        }
    }

    private void resetSphereUI() {
        print("Reset Sphere UI !");
        SphereUI.SetActive(false);
        points.Clear();
        print(points.Count);
        timer = 0.0f;
        currentTime = 0.0f;
        initMatrixArray();
        initYArray();
    }

    private void initMatrixArray() {
        for (int i = 0; i < 4; i++) {
            for (int j = 0; j < 4; j++)
                matrixArray[i, j] = 0.0f;
        }
    }

    private void initYArray() {
        for (int i = 0; i < 4; i++) {
            y[i] = 0.0f;
        }
    }

    /* 測試用 : 亂數產生球面上的點 */
    private void pointsSeed()
    {

        /* 圓心和半徑可自行設定測試 */
        Vector3 seedCenter = new Vector3(0.2f, 0.4f, 0f);
        float seedRadius = 1.3f;

        float theta, phi;


        /* 
		 * 依據所設定的球,隨機產生球面上n個點 .
		 * ref : http://jekel.me/2015/Least-Squares-Sphere-Fit/
		 */
        for (int i = 0; i < pointCount; i++)
        {
            theta = Random.Range(0.0f, 1.0f) * TWOPI;
            phi = Random.Range(0.0f, 1.0f) * PI - PID2;

            points[i] = new Vector3(seedCenter.x + seedRadius * Mathf.Cos(phi) * Mathf.Sin(theta), seedCenter.y + seedRadius * Mathf.Cos(phi) * Mathf.Cos(theta), seedCenter.z + seedRadius * Mathf.Sin(phi));
            //print(points[i]);
        }


    }

}