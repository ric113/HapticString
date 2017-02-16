using UnityEngine;
using UniOSC;				/* Add to use OSC's namespace Script */
using System.Collections;

public class TouchManager : MonoBehaviour {

	public RecvEncoderValue recvEncoderValue;

	private float length;

	// Update is called once per frame
	void Update () {
		length = recvEncoderValue.getLength ();
	}

	public bool isTouch(){				/* need to be modify */
		if (length > 100f)
			return true;
		return false;
	}

	public int getRelativePWM(){		/* need to be modify */
		return 200;
	}

}
