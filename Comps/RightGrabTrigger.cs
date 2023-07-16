using GorillaExtensions;
using GorillaLocomotion;
using System.Linq;
using UnityEngine;
#pragma warning disable CS0618
namespace YizziCamModV2.Comps
{
    class RightGrabTrigger : MonoBehaviour
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
            if (col.name.Contains("Right"))
            {
                if (Input.RightGrip & !Main.fpv)
                {
                    Main.CameraTablet.transform.parent = Main.RightHandGO.transform;
                    if (Main.fp) { Main.fp = false; }
                }
            }
            if (!Input.RightGrip & Main.CameraTablet.transform.parent == Main.RightHandGO.transform)
            {
                Main.CameraTablet.transform.parent = null;
            }
        }
    }
}
