using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;

public class LeapManager : MonoBehaviour {

    public GameObject leftFingerTip;
    public GameObject rightFingerTip;

    private Controller controller;
    private Frame frame;
    private List<Hand> hands;
    private Hand leftHand;
    private Hand rightHand;
    private List<Finger> leftHandFingers = new List<Finger>();
    private List<Finger> rightHandFingers = new List<Finger>();

    // Use this for initialization
    void Start () {
        controller = new Controller();
	}
	
	// Update is called once per frame
	void Update () {
        frame = controller.Frame();
        InitAllObj();

        if(frame.Hands.Count > 0)
        {
            setHands();
            setHandSide();

            if (leftHand != null)
            {
                Debug.Log(leftFingerTip.transform.position);
                
                leftHandFingers = leftHand.Fingers;
                leftHandFingers.ForEach(delegate (Finger finger)
                {
                    if (finger.Type == Finger.FingerType.TYPE_INDEX)
                        Debug.Log(finger.TipPosition.ToVector3());
                        
                });
                
            }
            else if (rightHand != null)
            {
                //rightHandFingers = rightHand.Fingers;
            }
            

        }

	}

    private void InitAllObj()
    {
        leftHand = null;
        rightHand = null;

        leftHandFingers.Clear();
        rightHandFingers.Clear();


    }

    private void setHands()
    {
        hands = frame.Hands ;
    }


    private void setHandSide()
    {
        hands.ForEach(delegate (Hand hand)
        {
            if (hand.IsLeft)
                leftHand = hand;
            else if(hand.IsRight)
                 rightHand = hand;
        });
    }

   



}
