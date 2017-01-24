/*
* UniOSC
* Copyright © 2014-2015 Stefan Schlupek
* All rights reserved
* info@monoflow.org
*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using OSCsharp.Data;
using UnityEngine.UI;


namespace UniOSC{


	/// <summary>
	/// Uni OSC scale game object.
	/// </summary>
	//[AddComponentMenu("UniOSC/ScaleGameObject")]
	public class RecvEncoderValue :  UniOSCEventTarget {


		public Text lengthText;
		[HideInInspector]
		public int encoderValue;

		private bool isSetInitEncoderValue = false;
		private int initEncoderValue;
		const float lengthPerValue = 0.043175f ;

		public override void OnEnable(){
			base.OnEnable();
		}

		public override void OnOSCMessageReceived(UniOSCEventArgs args){

			OscMessage msg = (OscMessage)args.Packet;
			if(msg.Data.Count <1) return;

			encoderValue = (int)msg.Data[0];

			if (!isSetInitEncoderValue) {
				initEncoderValue = encoderValue;
				isSetInitEncoderValue = true;
				Debug.Log ("Init Encdoer Value = " + initEncoderValue.ToString ());
			}

			//Debug.Log (encoderValue);
			getLength(encoderValue);

		}

		private void getLength(int value)
		{
			float length;
			length = -1.0f * (value - initEncoderValue) * lengthPerValue;
			lengthText.text = length.ToString ();
			Debug.Log ("Length = " + length.ToString ());
		}
			
	}
}

