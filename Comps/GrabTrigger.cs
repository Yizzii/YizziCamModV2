using System;
using GorillaLocomotion;
using UnityEngine;
using YizziCamModV2;

namespace YizziCamModV2.Comps
{
    class GrabTrigger : MonoBehaviour
    {
        public InputManager Input;
        public CameraController Main;
        void Start()
        {
            gameObject.layer = 18;
            Input = GameObject.FindObjectOfType<InputManager>().GetComponent<InputManager>();
            Main = GameObject.FindObjectOfType<CameraController>().GetComponent<CameraController>();
        }

        void OnTriggerStay(Collider col)
        {
            if (!Main.fpv)
            {
                if (Input.LeftGrip && Main.CameraTablet.transform.parent != Main.RightHandGO.transform)
                {
                    Main.CameraTablet.transform.parent = Main.LeftHandGO.transform;
                    if (Main.fp) { Main.fp = false; }
                }
                else if (Input.RightGrip && Main.CameraTablet.transform.parent != Main.LeftHandGO.transform)
                {
                    Main.CameraTablet.transform.parent = Main.RightHandGO.transform;
                    if (Main.fp) { Main.fp = false; }
                }
                else
                {
                    Main.CameraTablet.transform.parent = null;
                }
            }
            if (Main.fpv)
            {
                if (Input.LeftStick)
                {
                    Main.fpv = false;
                    foreach (GameObject btns in Main.Buttons)
                    {
                        btns.SetActive(true);
                    }
                    Main.CameraTablet.transform.parent = null;
                    Main.CameraTablet.transform.position = Main.CameraTablet.transform.position + Player.Instance.headCollider.transform.forward;
                    Main.CameraTablet.transform.Rotate(0f, -180f, 0f);
                }
            }
        }
    }
}
