using UnityEngine;
#pragma warning disable CS0618
namespace YizziCamModV2.Comps
{
    class RightGrabTrigger : MonoBehaviour
    {
        void Start()
        {
            gameObject.layer = 18;
        }
        void OnTriggerStay(Collider col)
        {
            if (col.name.Contains("Right"))
            {
                if (InputManager.instance.RightGrip & !Main.Instance.fpv)
                {
                    Main.Instance.CameraTablet.transform.parent = Main.Instance.RightHandGO.transform;
                    if (Main.Instance.fp) { Main.Instance.fp = false; }
                }
            }
            if (!InputManager.instance.RightGrip & Main.Instance.CameraTablet.transform.parent == Main.Instance.RightHandGO.transform)
            {
                Main.Instance.CameraTablet.transform.parent = null;
            }
        }
    }
}
