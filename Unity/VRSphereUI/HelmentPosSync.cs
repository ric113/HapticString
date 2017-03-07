using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelmentPosSync : MonoBehaviour {

    public Transform eyePos;
    public float Z_offset = 0.0f;
	// Use this for initialization
	void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(eyePos.position.x, eyePos.position.y, eyePos.position.z + Z_offset);
    }
}
