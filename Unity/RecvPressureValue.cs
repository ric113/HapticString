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
	public class RecvPressureValue :  UniOSCEventTarget {


		public Text lengthText;
		[HideInInspector]
		private int pressureValue;


		public override void OnEnable(){
			base.OnEnable();
		}

		public override void OnOSCMessageReceived(UniOSCEventArgs args){

			OscMessage msg = (OscMessage)args.Packet;
			if(msg.Data.Count <1) return;

			pressureValue = (int)msg.Data[0];
			lengthText.text = "Pressure:" + pressureValue.ToString ();

		}

		public int getPressure(){
			return pressureValue;
		}
			
	}
}

