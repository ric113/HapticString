using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shootableObj : MonoBehaviour {

	Renderer myRenderer;
	bool beShot = false;

	void Start(){
		myRenderer = GetComponent<Renderer> ();
	}

	void Update(){
		if (beShot) {
			myRenderer.material.color = Color.red;
			beShot = false;
		} else {
			myRenderer.material.color = Color.white;
		}
	}

	public void informBeShot(){
		beShot = true;
	}

}
