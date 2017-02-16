﻿using UnityEngine;
using UniOSC;				/* Add to use OSC's namespace Script */
using System.Collections;

public class FingerForward : MonoBehaviour {

	public RecvPressureValue recvPressureValue;
	private int pressure = 0;
	private int threshold = 0 ;

	void Update(){
		pressure = recvPressureValue.getPressure();
	}

	public bool fingerForward(){
		if (pressure == 0)			// If all ready , change to  ' > threshold '
			return true;
		return false;
	}
}
