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
        public bool LeftPrimaryButton; // x
        public bool RightPrimaryButton;
        // gamepad
        public Vector2 GPLeftStick;
        public Vector2 GPRightStick;

        void Start()
        {
            instance = this;
        }
        void Update()
        {
            LeftGrip = ControllerInputPoller.instance.leftGrab;
            RightGrip = ControllerInputPoller.instance.rightGrab;
            LeftPrimaryButton = ControllerInputPoller.instance.leftControllerPrimaryButton;
            RightPrimaryButton = ControllerInputPoller.instance.rightControllerPrimaryButton;

            if (Gamepad.current != null)
            {
                GPLeftStick = Gamepad.current.leftStick.ReadValue();
                GPRightStick = Gamepad.current.rightStick.ReadValue();
            }
        }
    }
}
