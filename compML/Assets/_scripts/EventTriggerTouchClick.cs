﻿using UnityEngine;
using System.Collections.Generic;

namespace Lean.Touch
{
    public class EventTriggerTouchClick : MonoBehaviour
    {

        private OpenCVManager openCVManager;
        private GameVisualManager gameManagerNotOpenCV;
        private InferenceNNfomDATABASE inferenceNNfomDATABASE;
        private InferenceCompositionML inferenceCompositionML;


        protected virtual void OnEnable()
        {
            // Hook into the events we need
            LeanTouch.OnFingerDown += OnFingerDown;
            LeanTouch.OnFingerSet += OnFingerSet;
            LeanTouch.OnFingerUp += OnFingerUp;
            LeanTouch.OnFingerTap += OnFingerTap;
            LeanTouch.OnFingerSwipe += OnFingerSwipe;
            LeanTouch.OnGesture += OnGesture;
        }

        private void Start()
        {
            openCVManager = FindObjectOfType<OpenCVManager>();
            gameManagerNotOpenCV = FindObjectOfType<GameVisualManager>();
            inferenceNNfomDATABASE = FindObjectOfType<InferenceNNfomDATABASE>();
            inferenceCompositionML = FindObjectOfType<InferenceCompositionML>();
        }

        protected virtual void OnDisable()
        {
            // Unhook the events
            LeanTouch.OnFingerDown -= OnFingerDown;
            LeanTouch.OnFingerSet -= OnFingerSet;
            LeanTouch.OnFingerUp -= OnFingerUp;
            LeanTouch.OnFingerTap -= OnFingerTap;
            LeanTouch.OnFingerSwipe -= OnFingerSwipe;
            LeanTouch.OnGesture -= OnGesture;
        }

        public void OnFingerDown(LeanFinger finger)
        {
            //openCVManager.CallTOCalculateAreaLeftRight();
            //Debug.Log("Finger " + finger.Index + " began touching the screen");
        }

        public void OnFingerSet(LeanFinger finger)
        {
            openCVManager.CallForOpenCVCalculationUpdates();
            gameManagerNotOpenCV.CallTOCalculateNOTOpenCVScores();
            inferenceNNfomDATABASE.CallTOCalculateNNFrontTopcore();
            inferenceCompositionML.CallTOCalculateMobileNetScore();

            //Debug.Log("Finger " + finger.Index + " is still touching the screen");
        }

        public void OnFingerUp(LeanFinger finger)
        {
            //openCVManager.CallTOCalculateAreaLeftRight();
            //Debug.Log("Finger " + finger.Index + " finished touching the screen");
        }

        public void OnFingerTap(LeanFinger finger)
        {
            //Debug.Log("Finger " + finger.Index + " tapped the screen");
        }

        public void OnFingerSwipe(LeanFinger finger)
        {
            //Debug.Log("Finger " + finger.Index + " swiped the screen");
        }

        public void OnGesture(List<LeanFinger> fingers)
        {
            //Debug.Log("Gesture with " + fingers.Count + " finger(s)");
            //Debug.Log("    pinch scale: " + LeanGesture.GetPinchScale(fingers));
            //Debug.Log("    twist degrees: " + LeanGesture.GetTwistDegrees(fingers));
            //Debug.Log("    twist radians: " + LeanGesture.GetTwistRadians(fingers));
            //Debug.Log("    screen delta: " + LeanGesture.GetScreenDelta(fingers));
        }
    }
}