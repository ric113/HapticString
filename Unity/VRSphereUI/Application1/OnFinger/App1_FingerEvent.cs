using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class App1_FingerEvent : MonoBehaviour {

	public GameObject surfaceOne;
	public GameObject surfaceTwo;
	Renderer surfaceOneRenderer;
	Renderer surfaceTwoRenderer;

    private Vector3 surface2TouchPointPosition;
    private GameObject surface2TouchPoint;
    private float verticalAngel, horizontalAngel;
    private GameObject onHandObj = null;


    // Use this for initialization
    void Start () {
		surfaceOneRenderer = surfaceOne.GetComponent<Renderer> ();
		surfaceTwoRenderer = surfaceTwo.GetComponent<Renderer> ();
		//surfaceOneRenderer.material.SetColor ("_Color", new Vector4 (255, 255, 255, 0));
	}
	
	void OnCollisionEnter(Collision collision)
	{
		/* 使surface1透明 , surface2顯現 */
         if (collision.gameObject.tag == "middleRegion") 
        {
            surfaceOneRenderer.material
                .SetColor ("_Color", new Color(surfaceOneRenderer.material.color.r,surfaceOneRenderer.material.color.g,surfaceOneRenderer.material.color.b,0.1f));
            setObjOnSurfaceAlpha (surfaceOne, 0.1f);

            surfaceTwoRenderer.material
                .SetColor ("_Color", new Color(surfaceTwoRenderer.material.color.r,surfaceTwoRenderer.material.color.g,surfaceTwoRenderer.material.color.b,0.3f));
            setObjOnSurfaceAlpha (surfaceTwo, 0.3f);
        }

        /* (surface1 init時是顯現)使surface1顯現 , surface2透明 */
        if (collision.gameObject.tag == "surface1") 
        {
            surfaceOneRenderer.material
                .SetColor ("_Color", new Color(surfaceOneRenderer.material.color.r,surfaceOneRenderer.material.color.g,surfaceOneRenderer.material.color.b,0.3f));
            setObjOnSurfaceAlpha (surfaceOne, 0.3f);

            surfaceTwoRenderer.material
                .SetColor ("_Color", new Color(surfaceTwoRenderer.material.color.r,surfaceTwoRenderer.material.color.g,surfaceTwoRenderer.material.color.b,0.1f));
            setObjOnSurfaceAlpha (surfaceTwo, 0.1f);
        }

        /* Object放置在surface2上 */
        if (collision.gameObject.tag == "surface2")
        {
            if (onHandObj != null)
            {
                surface2TouchPointPosition = transform.position;
                generateTouchPointGameObject();
                getTouchPointAngel();
            }
        }


	}

    public void setOnHandObj(GameObject obj)
    {
        onHandObj = obj;
    }

    void generateTouchPointGameObject()
    {
        surface2TouchPoint = new GameObject();
        surface2TouchPoint.name = "TouchPoint";
        surface2TouchPoint.transform.position = surface2TouchPointPosition;
        surface2TouchPoint.transform.parent = surfaceTwo.transform;
    }

    /* 相對於local x軸,y軸的夾角 */
    void getTouchPointAngel()
    {
        Vector3 touchPointVector = surface2TouchPoint.transform.localPosition;
        //Vector3 projectToZ = new Vector3(touchPointVector.x, touchPointVector.y, 0f);
        Vector3 projectToY = new Vector3(touchPointVector.x, 0f, touchPointVector.z);
        //bool verticalUp = (touchPointVector.y >= 0); /* -90 ~ 90 */
        bool horizontalRight = (touchPointVector.z >= 0); /* -180 ~ -180 */

        /*
        verticalAngel = (projectToZ == Vector3.zero) ? 0f : Vector3.Angle(projectToZ, surfaceTwo.transform.right);
        if (verticalAngel > 90)
            verticalAngel = 180 - verticalAngel;
        if (!verticalUp)
            verticalAngel = -1 * verticalAngel;
        */
       
       verticalAngel = Vector3.Angel(touchPointVector, -1 * surfaceTwo.transform.up);
       verticalAngel -= 90;

        horizontalAngel = (projectToY == Vector3.zero) ? 0f : Vector3.Angle(projectToY, surfaceTwo.transform.right);
        if (horizontalRight)
            horizontalAngel = -1 * horizontalAngel;


        //print (verticalAngel);
        //print (horizontalAngel);

        onHandObj.GetComponent<App1_ButtonTouchEvent>().setButtonOnSurface2(verticalAngel,horizontalAngel);
        onHandObj = null;
        Destroy(surface2TouchPoint);
    }

    void setObjOnSurfaceAlpha(GameObject surface ,float alpha){

        Renderer onSurfaceObjRenderer;
        foreach (Transform child in surface.transform)
        {
            //child is your child transform
            foreach(Transform _child in child.transform){
                if (_child.tag == "onSurfaceObj") {
                    onSurfaceObjRenderer = child.GetComponentInChildren<Renderer> ();
                    onSurfaceObjRenderer.material.SetColor ("_Color", new Color (onSurfaceObjRenderer.material.color.r, onSurfaceObjRenderer.material.color.g, onSurfaceObjRenderer.material.color.b, alpha));
                }
            }
        }

    }
}
