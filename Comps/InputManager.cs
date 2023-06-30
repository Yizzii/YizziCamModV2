using System;
using UnityEngine;
using UnityEngine.XR;

namespace YizziCamModV2.Comps
{
    class InputManager : MonoBehaviour
    {
        private XRNode lHandNode = XRNode.LeftHand;
        private XRNode rHandNode = XRNode.RightHand;
        public bool LeftGrip;
        public bool RightGrip;
        public bool LeftTrigger;
        public bool RightTrigger;
        public bool LeftStick;
        public bool RightStick;

        void Update()
        {
            InputDevices.GetDeviceAtXRNode(lHandNode).TryGetFeatureValue(CommonUsages.gripButton, out LeftGrip);
            InputDevices.GetDeviceAtXRNode(rHandNode).TryGetFeatureValue(CommonUsages.gripButton, out RightGrip);
            InputDevices.GetDeviceAtXRNode(lHandNode).TryGetFeatureValue(CommonUsages.triggerButton, out LeftTrigger);
            InputDevices.GetDeviceAtXRNode(rHandNode).TryGetFeatureValue(CommonUsages.triggerButton, out RightTrigger);
            InputDevices.GetDeviceAtXRNode(lHandNode).TryGetFeatureValue(CommonUsages.primary2DAxisClick, out LeftStick);
            InputDevices.GetDeviceAtXRNode(rHandNode).TryGetFeatureValue(CommonUsages.primary2DAxisClick, out RightStick);
        }
    }
}
