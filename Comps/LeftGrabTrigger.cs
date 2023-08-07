using UnityEngine;
#pragma warning disable CS0618
namespace YizziCamModV2.Comps
{
    class LeftGrabTrigger : MonoBehaviour
    {
        void Start()
        {
            gameObject.layer = 18;
        }
        void OnTriggerStay(Collider col)
        {
            if (col.name.Contains("Left"))
            {
                if (InputManager.instance.LeftGrip & !Main.Instance.fpv)
                {
                    Main.Instance.CameraTablet.transform.parent = Main.Instance.LeftHandGO.transform;
                    if (Main.Instance.fp) { Main.Instance.fp = false; }
                }
            }
            if (!InputManager.instance.LeftGrip & Main.Instance.CameraTablet.transform.parent == Main.Instance.LeftHandGO.transform)
            {
                Main.Instance.CameraTablet.transform.parent = null;
            }
        }
    }
}
