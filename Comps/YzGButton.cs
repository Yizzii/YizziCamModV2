using UnityEngine;

#pragma warning disable CS0618
namespace YizziCamModV2.Comps
{
    class YzGButton : MonoBehaviour
    {
        void Start()
        {
            gameObject.layer = 18;
        }

        void OnEnable()
        {
            Invoke(nameof(ButtonTimer), 1f);
        }

        void OnDisable()
        {
            if (CameraController.Instance != null)
                CameraController.Instance.canbeused = false;
        }

        void ButtonTimer()
        {
            if (CameraController.Instance == null) return;

            if (!enabled)
            {
                CameraController.Instance.canbeused = false;
                return;
            }

            CameraController.Instance.canbeused = true;
        }

        void OnTriggerEnter(Collider col)
        {
            if (CameraController.Instance == null) return;

            if (CameraController.Instance.canbeused &&
                (col.name == "RightHandTriggerCollider" || col.name == "LeftHandTriggerCollider"))
            {
                CameraController.Instance.canbeused = false;
                Invoke(nameof(ButtonTimer), 1f);

                switch (name)
                {
                    case "BackButton":
                        CameraController.Instance.MainPage.SetActive(true);
                        CameraController.Instance.MiscPage.SetActive(false);
                        break;

                    case "ControlsButton":
                        if (!CameraController.Instance.openedurl)
                        {
                            Application.OpenURL("https://github.com/Yizzii/YizziCamModV2#controls");
                            CameraController.Instance.openedurl = true;
                        }
                        break;

                    case "SmoothingDownButton":
                        CameraController.Instance.smoothing -= 0.01f;
                        if (CameraController.Instance.smoothing < 0.05f)
                            CameraController.Instance.smoothing = 0.11f;

                        CameraController.Instance.SmoothText.text = CameraController.Instance.smoothing.ToString();
                        CameraController.Instance.canbeused = true;
                        break;

                    case "SmoothingUpButton":
                        CameraController.Instance.smoothing += 0.01f;
                        if (CameraController.Instance.smoothing > 0.11f)
                            CameraController.Instance.smoothing = 0.05f;

                        CameraController.Instance.SmoothText.text = CameraController.Instance.smoothing.ToString();
                        CameraController.Instance.canbeused = true;
                        break;

                    case "TPVButton":

                        if (CameraController.Instance.TPVMode == CameraController.TPVModes.BACK)
                        {
                            if (CameraController.Instance.flipped)
                            {
                                CameraController.Instance.flipped = false;
                                RotateCameras();
                            }
                        }
                        else if (CameraController.Instance.TPVMode == CameraController.TPVModes.FRONT)
                        {
                            if (!CameraController.Instance.flipped)
                            {
                                CameraController.Instance.flipped = true;
                                RotateCameras();
                            }
                        }

                        CameraController.Instance.fp = false;
                        CameraController.Instance.fpv = false;
                        CameraController.Instance.tpv = true;

                        break;

                    case "FPVButton":

                        if (CameraController.Instance.flipped)
                        {
                            CameraController.Instance.flipped = false;
                            RotateCameras();
                        }

                        CameraController.Instance.fp = false;
                        CameraController.Instance.fpv = true;

                        break;

                    case "FlipCamButton":

                        CameraController.Instance.flipped = !CameraController.Instance.flipped;
                        RotateCameras();

                        break;

                    case "FovDown":

                        CameraController.Instance.TabletCamera.fieldOfView -= 5f;

                        if (CameraController.Instance.TabletCamera.fieldOfView < 20)
                        {
                            CameraController.Instance.TabletCamera.fieldOfView = 130f;
                            CameraController.Instance.ThirdPersonCamera.fieldOfView = 130f;
                        }

                        CameraController.Instance.ThirdPersonCamera.fieldOfView = CameraController.Instance.TabletCamera.fieldOfView;
                        CameraController.Instance.FovText.text = CameraController.Instance.TabletCamera.fieldOfView.ToString();

                        CameraController.Instance.canbeused = true;

                        break;

                    case "FovUP":

                        CameraController.Instance.TabletCamera.fieldOfView += 5f;

                        if (CameraController.Instance.TabletCamera.fieldOfView > 130)
                        {
                            CameraController.Instance.TabletCamera.fieldOfView = 20f;
                            CameraController.Instance.ThirdPersonCamera.fieldOfView = 20f;
                        }

                        CameraController.Instance.ThirdPersonCamera.fieldOfView = CameraController.Instance.TabletCamera.fieldOfView;
                        CameraController.Instance.FovText.text = CameraController.Instance.TabletCamera.fieldOfView.ToString();

                        CameraController.Instance.canbeused = true;

                        break;

                    case "MiscButton":

                        CameraController.Instance.MainPage.SetActive(false);
                        CameraController.Instance.MiscPage.SetActive(true);

                        break;

                    case "NearClipDown":

                        CameraController.Instance.TabletCamera.nearClipPlane -= 0.01f;

                        if (CameraController.Instance.TabletCamera.nearClipPlane < 0.01f)
                        {
                            CameraController.Instance.TabletCamera.nearClipPlane = 1f;
                            CameraController.Instance.ThirdPersonCamera.nearClipPlane = 1f;
                        }

                        CameraController.Instance.ThirdPersonCamera.nearClipPlane = CameraController.Instance.TabletCamera.nearClipPlane;
                        CameraController.Instance.NearClipText.text = CameraController.Instance.TabletCamera.nearClipPlane.ToString();

                        CameraController.Instance.canbeused = true;

                        break;

                    case "NearClipUp":

                        CameraController.Instance.TabletCamera.nearClipPlane += 0.01f;

                        if (CameraController.Instance.TabletCamera.nearClipPlane > 1.0f)
                        {
                            CameraController.Instance.TabletCamera.nearClipPlane = 0.01f;
                            CameraController.Instance.ThirdPersonCamera.nearClipPlane = 0.01f;
                        }

                        CameraController.Instance.ThirdPersonCamera.nearClipPlane = CameraController.Instance.TabletCamera.nearClipPlane;
                        CameraController.Instance.NearClipText.text = CameraController.Instance.TabletCamera.nearClipPlane.ToString();

                        CameraController.Instance.canbeused = true;

                        break;

                    case "GreenScreenButton":

                        bool state = !CameraController.Instance.ColorScreenGO.activeSelf;
                        CameraController.Instance.ColorScreenGO.SetActive(state);

                        if (CameraController.Instance.ColorScreenGO.activeSelf)
                            CameraController.Instance.ColorScreenText.text = "(ENABLED)";
                        else
                            CameraController.Instance.ColorScreenText.text = "(DISABLED)";

                        break;

                    case "RedButton":

                        foreach (Material mat in CameraController.Instance.ScreenMats)
                            mat.color = Color.red;

                        break;

                    case "GreenButton":

                        foreach (Material mat in CameraController.Instance.ScreenMats)
                            mat.color = Color.green;

                        break;

                    case "BlueButton":

                        foreach (Material mat in CameraController.Instance.ScreenMats)
                            mat.color = Color.blue;

                        break;
                }
            }
        }

        void RotateCameras()
        {
            CameraController.Instance.ThirdPersonCameraGO.transform.Rotate(0f, 180f, 0f);
            CameraController.Instance.TabletCameraGO.transform.Rotate(0f, 180f, 0f);
            CameraController.Instance.FakeWebCam.transform.Rotate(-180f, 180f, 0f);
        }
    }
}
