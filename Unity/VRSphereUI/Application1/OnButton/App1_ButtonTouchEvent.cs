using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class App1_ButtonTouchEvent : MonoBehaviour {


	public GameObject LfingerTip;
	private bool fingerTouch = false;
	private bool onSecondSurface = false;
	private Vector3 fingerToButtonOffset;
	private ButtonCurvedEffect btnCurverdEffect;

	public GameObject surface2 ;
	private float verticalAngel,horizontalAngel ;
    public App1_FingerEvent fingerEvent;

	// Use this for initialization
	void Start () {
		btnCurverdEffect = GetComponent<ButtonCurvedEffect> ();
		verticalAngel = btnCurverdEffect.angle_vertical;
		horizontalAngel = btnCurverdEffect.angle_horizontal;
	}

	private void FixedUpdate()
	{
		if (fingerTouch) 
		{
			transform.position = LfingerTip.transform.position + fingerToButtonOffset;
		}
	}


	void OnCollisionEnter(Collision collision)
	{

		if (collision.gameObject.tag == "fingerTip" && !onSecondSurface) 
		{
			fingerToButtonOffset = transform.position - LfingerTip.transform.position;
			fingerTouch = true;

            fingerEvent.setOnHandObj(transform.gameObject);
		}
        
	}

	/* Call by FingerEvent.cs */
	public void setButtonOnSurface2(float verticalAngel,float horizontalAngel)
	{
        fingerTouch = false;
        onSecondSurface = true;

        btnCurverdEffect.angle_vertical = verticalAngel;
		btnCurverdEffect.angle_horizontal = horizontalAngel;

		btnCurverdEffect.resetButtonOnSurface (surface2);
	}
}
