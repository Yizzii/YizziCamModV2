using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

namespace YizziCamModV2.Comps
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager instance;
        private XRNode lHandNode = XRNode.LeftHand;
        private XRNode rHandNode = XRNode.RightHand;
        public bool LeftGrip;
        public bool RightGrip;
        public bool LeftTrigger;
        public bool RightTrigger;
        public bool LeftStick;
        public bool RightStick;
        public bool LeftPrimaryButton; // x
        // gamepad
        public Vector2 GPLeftStick;
        public Vector2 GPRightStick;

        void Start()
        {
            instance = this;
        }
        void Update()
        {
            InputDevices.GetDeviceAtXRNode(lHandNode).TryGetFeatureValue(UnityEngine.XR.CommonUsages.gripButton, out LeftGrip);
            InputDevices.GetDeviceAtXRNode(rHandNode).TryGetFeatureValue(UnityEngine.XR.CommonUsages.gripButton, out RightGrip);
            InputDevices.GetDeviceAtXRNode(lHandNode).TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out LeftTrigger);
            InputDevices.GetDeviceAtXRNode(rHandNode).TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out RightTrigger);
            InputDevices.GetDeviceAtXRNode(lHandNode).TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxisClick, out LeftStick);
            InputDevices.GetDeviceAtXRNode(rHandNode).TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxisClick, out RightStick);
            InputDevices.GetDeviceAtXRNode(lHandNode).TryGetFeatureValue(UnityEngine.XR.CommonUsages.primaryButton, out LeftPrimaryButton);
            if (Gamepad.current != null)
            {
                GPLeftStick = Gamepad.current.leftStick.ReadValue();
                GPRightStick = Gamepad.current.rightStick.ReadValue();
            }
        }
    }
}
