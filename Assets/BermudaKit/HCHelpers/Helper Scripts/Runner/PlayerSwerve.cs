using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ali.Helper.Runner
{
    public class PlayerSwerve : MonoBehaviour
    {
        [SerializeField] private float _speedMultiplier = 1f;

        public event System.Action<Vector2> OnSwerve;
        void Start()
        {
            LeanTouch.OnFingerUpdate += LeanTouch_OnFingerUpdate;
        }

        private void LeanTouch_OnFingerUpdate(LeanFinger finger)
        {
            if (Input.GetMouseButton(0))
            {
                OnSwerve?.Invoke(finger.ScaledDelta * _speedMultiplier);
            }
        }

        private void OnDestroy()
        {
            LeanTouch.OnFingerUpdate -= LeanTouch_OnFingerUpdate;
        }

    }
}