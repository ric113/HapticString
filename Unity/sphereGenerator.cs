using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sphereGenerator : MonoBehaviour {

	/* ref : https://zh.scribd.com/document/14819165/Regressions-coniques-quadriques-circulaire-spherique*/


	const float TWOPI = 6.2831853071f;
	const float PI = 3.1415926535897f;
	const float PID2 = 1.5707963267948f;

	static public int pointCount = 10 ;		/* 取樣點數 */

	private float[ , ] matrixArray = new float[ 4, 4];
	private float[] y = new float[4];
	private Vector3[] points = new Vector3[pointCount];
	private Matrix4x4 matrix = new Matrix4x4 ();
	private Vector4 yVec4,xVec4;
	private Vector3 sphereCenter;
	private float sphereRadius;

	/* 目標 ： -yVec4 = matrix * xVec4 , 求解 xVec4 */

	void Start(){
		
		pointsSeed();
		generateMatrix ();
		generateY();
		calculateX ();
		getSphere ();
		print ("Center :" + sphereCenter + ", Radius:" + sphereRadius );
		GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		sphere.transform.position = sphereCenter;
		sphere.transform.localScale = new Vector3 (sphereRadius, sphereRadius, sphereRadius);

	}


	/* 產生 'matrix' */
	private void generateMatrix(){
		matrixArray [0, 0] = pointCount;
		float p;

		for (int i = 0; i < pointCount; i++) {
			matrixArray [1, 1] += Mathf.Pow (points [i].x, 2);
			matrixArray [2, 2] += Mathf.Pow (points [i].y, 2);
			matrixArray [3, 3] += Mathf.Pow (points [i].z, 2);

			matrixArray [0, 1] += points [i].x;
			matrixArray [1, 0] += points [i].x;
			matrixArray [0, 2] += points [i].y;
			matrixArray [2, 0] += points [i].y;
			matrixArray [0, 3] += points [i].z;
			matrixArray [3, 0] += points [i].z;

			matrixArray [1, 2] += points [i].x * points [i].y ;
			matrixArray [2, 1] += points [i].x * points [i].y ;
			matrixArray [1, 3] += points [i].x * points [i].z ;
			matrixArray [3, 1] += points [i].x * points [i].z ;
			matrixArray [2, 3] += points [i].z * points [i].y ;
			matrixArray [3, 2] += points [i].z * points [i].y ;

			p = Mathf.Pow(points [i].x,2) + Mathf.Pow(points [i].y,2) + Mathf.Pow(points [i].z,2);
			y [0] += p;
			y [1] += p * points [i].x;
			y [2] += p * points [i].y;
			y [3] += p * points [i].z;
		}

		for (int i = 0; i < 4; i++) {
			matrix.SetColumn (i, arrayToVec4Column (i));
		}

	}
		
	private void generateY(){
		yVec4 = new Vector4 (-1*y [0], -1*y [1], -1*y [2], -1*y [3]);
	}

	private void calculateX(){
		xVec4 =  matrix.inverse * yVec4;
	}

	private void getSphere(){
		float a = (float)0.5 *-1* xVec4.y;
		float b = (float)0.5 *-1* xVec4.z;
		float c = (float)0.5 *-1* xVec4.w;

		//print ("a:" + a + " b:" + b + " c:" + c);
		//print (Mathf.Pow (a, 2) + Mathf.Pow (b, 2) + Mathf.Pow (c, 2) - xVec4.x);

		sphereRadius = Mathf.Sqrt (Mathf.Pow (a, 2) + Mathf.Pow (b, 2) + Mathf.Pow (c, 2) - xVec4.x);
		sphereCenter = new Vector3 (a, b, c);
	}
		

	/* Convert 4*4 Array to Matirx Type */
	private Vector4 arrayToVec4Column(int colNum){
		Vector4 col = new Vector4 (matrixArray[colNum,0],matrixArray[colNum,1],matrixArray[colNum,2],matrixArray[colNum,3]);
		return col;
	}

	/* 測試用 : 亂數產生球面上的點 */
	private void pointsSeed(){

		/* 圓心和半徑可自行設定測試 */
		Vector3 seedCenter = new Vector3 (0.2f,0.4f,0f);
		float seedRadius = 1.3f;

		float theta,phi;


		/* 
		 * 依據所設定的球,隨機產生球面上n個點 .
		 * ref : http://jekel.me/2015/Least-Squares-Sphere-Fit/
		 */
		for (int i=0 ; i<pointCount ; i++) {
			theta = Random.Range(0.0f,1.0f) * TWOPI;
			phi = Random.Range(0.0f,1.0f) * PI - PID2;

			points [i] = new Vector3 (seedCenter.x + seedRadius * Mathf.Cos(phi) * Mathf.Sin(theta),seedCenter.y+ seedRadius * Mathf.Cos(phi) * Mathf.Cos(theta),seedCenter.z + seedRadius * Mathf.Sin(phi));
			print (points [i]);
		}
			
			
	}

}

