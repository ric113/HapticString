using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;
using Leap.Unity.Attributes;


public class getLeapMotionComponent : MonoBehaviour {

    [AutoFind]
    public LeapServiceProvider provider;

    private Controller controller;
    private Frame frame;
    private List<Hand> hands;
    private Hand leftHand;
    private Hand rightHand;
    private List<Finger> leftHandFingers = new List<Finger>();
    private List<Finger> rightHandFingers = new List<Finger>();
    private Vector3 indexFingerTipPos;
    private bool isLeftHand;


    // Update is called once per frame
    void Update()
    {
        frame = provider.CurrentFixedFrame;
        if (frame != null)
        {

            if (frame.Hands.Count > 0)
            {
                // Debug.Log("Has Hand!");
                setHands();
                setHandSide();

                if (leftHand != null)
                {
                    isLeftHand = true;
                    leftHandFingers = leftHand.Fingers;
                    processFinger(leftHandFingers, isLeftHand);
                    

                }
                else if (rightHand != null)
                {
                    isLeftHand = false;
                    rightHandFingers = rightHand.Fingers;
                    processFinger(rightHandFingers, isLeftHand);
                }

            }
        }
    }

    private void setHands()
    {
        hands = frame.Hands;
    }


    private void setHandSide()
    {
        hands.ForEach(delegate (Hand hand)
        {
            if (hand.IsLeft)
                leftHand = hand;
            else if (hand.IsRight)
                rightHand = hand;
        });
    }

    private void processFinger(List<Finger> fingers, bool isLeftHand) {
        fingers.ForEach(delegate (Finger finger)
        {
            if (finger.Type == Finger.FingerType.TYPE_INDEX)
            {

                float x = finger.TipPosition.x;
                float y = finger.TipPosition.y;
                float z = finger.TipPosition.z;

                indexFingerTipPos = new Vector3(x, y, z);
                //Debug.Log(indexFingerTipPos);
                // LSphere.transform.position = fingerPos;

            }
        });
    }

    public Vector3 getIndexTipPos()
    {
        return indexFingerTipPos;
    }
}
