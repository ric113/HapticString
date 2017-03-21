using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fingerEvent : MonoBehaviour {

	Ray ray;
	RaycastHit rayHit;
	float rayLength = 100f;
	int shootableMask;
	GameObject hitObject;

	LineRenderer rayLine;

	bool handOnSurface = false;
	GameObject onHandObj = null;
	Vector3 objOffsetToHand = Vector3.zero;

	void Awake()
	{
		shootableMask = LayerMask.GetMask ("Shootable"); /* shootable Mask ; */
	}

	void Start()
	{
		rayLine = GetComponent<LineRenderer>();
	}

	void FixedUpdate()
	{
		rayLine.SetPosition (0, transform.position);

		if (onHandObj == null) {

			ray = new Ray (transform.position, transform.up);

			if (Physics.Raycast (ray, out rayHit, rayLength, shootableMask)) {
				hitObject = rayHit.collider.gameObject;

				if (hitObject != null) {
					if (handOnSurface) {

						rayLine.SetPosition (1, rayHit.point);
						rayLine.enabled = true;

						/* 綁定和手的相對位置 */
						onHandObj = hitObject;
						objOffsetToHand = onHandObj.transform.position - transform.position;
						// Disable 被射到Obj的Rigidbody .


					} else {
						/* Hover的特效 */
						hitObject.GetComponent<shootableObj>().informBeShot();
					}
				}
			} else {	/* 沒打到東西 */
				if (handOnSurface) {
					rayLine.SetPosition (1, ray.origin + transform.up * 100);
					rayLine.enabled = true;
				} else {
					rayLine.enabled = false;
				}
			}
		} else {	/* 手上已有東西 */
			if (handOnSurface) {	/* Obj跟著手移動 */
				onHandObj.transform.position = transform.position + objOffsetToHand;
				rayLine.SetPosition (1, onHandObj.transform.position);
			} else {	/* 放下Obj */
				onHandObj = null;
				objOffsetToHand = Vector3.zero;
				rayLine.enabled = false;
				// enable 被射到Obj的Rigidbody .
			}
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "surface1" && !handOnSurface) 
		{
			//print ("Col!");
			handOnSurface = true;
		}
	}

	void OnCollisionExit(Collision collision)
	{
		if (collision.gameObject.tag == "surface1" && handOnSurface) 
		{
			//print ("Exit!");
			handOnSurface = false;
		}
	}
}
