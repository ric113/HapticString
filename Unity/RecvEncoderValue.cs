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


namespace UniOSC{


	/// <summary>
	/// Uni OSC scale game object.
	/// </summary>
	//[AddComponentMenu("UniOSC/ScaleGameObject")]
	public class RecvEncoderValue :  UniOSCEventTarget {

		//public TouchManager touchManager ;

		[HideInInspector]
		public int encoderValue;

		public override void OnEnable(){
			base.OnEnable();
		}

		public override void OnOSCMessageReceived(UniOSCEventArgs args){

			OscMessage msg = (OscMessage)args.Packet;
			if(msg.Data.Count <1) return;

			encoderValue = (int)msg.Data[0];

			//touchManager.encoderValue = encoderValue;

			Debug.Log (encoderValue);

		}



	}
}

