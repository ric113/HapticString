using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO.Ports;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class SendToArdnScript : MonoBehaviour {

	public Slider slider;
	public InputField inputField;
	public SerialHandler serialHandler;

	private int value;
	private int preValue;
	private Text valueTex;

	//private SerialPort sp = new SerialPort("/dev/cu.usbmodem1411",115200);
	// Use this for initialization

	void Start () {
		value = 0;
		preValue = -1;
		valueTex = GetComponent<Text> ();


		//sp.Open ();
		//sp.WriteTimeout = 10;
		//sp.ReadTimeout = 5;
		//StartCoroutine(SendData());
	}



	/* 方法 1 */
	void Update()
	{
		value = (int)slider.value;
		valueTex.text = value.ToString();
		//Debug.Log("Strength : " + value.ToString());

		if (preValue != value) {		//當停留在某值時 , 不會一直送 
			preValue = value;
			if(serialHandler != null){
				//print("Send :" + value.ToString());
				serialHandler.Write (value.ToString ());

			}

		}
	}


	/* 方法 2 */
	IEnumerator SendData()
	{
		while (true) 
		{
			value = (int)slider.value;
			valueTex.text = value.ToString();

			if (/*sp.IsOpen && */preValue != value) {		/* 當停留在某值時 , 不會一直送 */
				preValue = value;
				Debug.Log (value);
				serialHandler.Write (value.ToString ());
			}
			else
				yield return new WaitForSeconds (0.1f);		/* 執行此行會先return(讓出CPU) , 0.1秒後才會回來 */
		}
	}

}
