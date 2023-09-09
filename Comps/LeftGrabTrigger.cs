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
                if (InputManager.instance.LeftGrip & !CameraController.Instance.fpv)
                {
                    CameraController.Instance.CameraTablet.transform.parent = CameraController.Instance.LeftHandGO.transform;
                    if (CameraController.Instance.fp) { CameraController.Instance.fp = false; }
                }
            }
            if (!InputManager.instance.LeftGrip & CameraController.Instance.CameraTablet.transform.parent == CameraController.Instance.LeftHandGO.transform)
            {
                CameraController.Instance.CameraTablet.transform.parent = null;
            }
        }
    }
}
