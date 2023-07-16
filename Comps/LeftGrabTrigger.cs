using GorillaLocomotion;
using UnityEngine;
#pragma warning disable CS0618
namespace YizziCamModV2.Comps
{
    class LeftGrabTrigger : MonoBehaviour
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
            if (col.name.Contains("Left"))
            {
                if (Input.LeftGrip & !Main.fpv)
                {
                    Main.CameraTablet.transform.parent = Main.LeftHandGO.transform;
                    if (Main.fp) { Main.fp = false; }
                }
                if (Main.fpv)
                {
                    if (Input.LeftPrimaryButton)
                    {
                        Main.fpv = false;
                        foreach (GameObject btns in Main.Buttons)
                        {
                            btns.SetActive(true);
                        }
                        if (!Main.MainPage.active)
                        {
                            foreach (MeshRenderer mr in Main.meshRenderers)
                            {
                                mr.enabled = true;
                            }
                            Main.MainPage.active = true;
                        }
                        Main.CameraTablet.transform.position = Player.Instance.headCollider.transform.position + Player.Instance.headCollider.transform.forward;
                        Main.CameraTablet.transform.Rotate(0f, -180f, 0f);
                    }
                }
            }
            if (!Input.LeftGrip & Main.CameraTablet.transform.parent == Main.LeftHandGO.transform)
            {
                Main.CameraTablet.transform.parent = null;
            }
        }
    }
}
