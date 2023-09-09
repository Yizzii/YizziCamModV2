using UnityEngine;
#pragma warning disable CS0618
namespace YizziCamModV2.Comps
{
    class YzGButton : MonoBehaviour
    {
        void Start()
        {
            this.gameObject.layer = 18;
        }
        void OnEnable() { Invoke("ButtonTimer", 1f); }
        void OnDisable() { CameraController.Instance.canbeused = false; }
        void ButtonTimer()
        {
            if (!this.enabled)
            {
                CameraController.Instance.canbeused = false;
            }
            CameraController.Instance.canbeused = true;
        }
        void OnTriggerEnter(Collider col)
        {
            if (CameraController.Instance.canbeused && col.name == "RightHandTriggerCollider" | col.name == "LeftHandTriggerCollider")
            {
                CameraController.Instance.canbeused = false;
                Invoke("ButtonTimer", 1f);
                switch (this.name)
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
                        {
                            CameraController.Instance.smoothing = 0.11f;
                        }
                        CameraController.Instance.SmoothText.text = CameraController.Instance.smoothing.ToString();
                        CameraController.Instance.canbeused = true;
                        break;
                    case "SmoothingUpButton":
                        CameraController.Instance.smoothing += 0.01f;
                        if (CameraController.Instance.smoothing > 0.11f)
                        {
                            CameraController.Instance.smoothing = 0.05f;
                        }
                        CameraController.Instance.SmoothText.text = CameraController.Instance.smoothing.ToString();
                        CameraController.Instance.canbeused = true;
                        break;
                    case "TPVButton":
                        if (CameraController.Instance.TPVMode == CameraController.TPVModes.BACK)
                        {
                            if (CameraController.Instance.flipped)
                            {
                                CameraController.Instance.flipped = false;
                                CameraController.Instance.ThirdPersonCameraGO.transform.Rotate(0.0f, 180f, 0.0f);
                                CameraController.Instance.TabletCameraGO.transform.Rotate(0.0f, 180f, 0.0f);
                                CameraController.Instance.FakeWebCam.transform.Rotate(-180f, 180f, 0.0f);
                            }
                        }
                        else if (CameraController.Instance.TPVMode == CameraController.TPVModes.FRONT)
                        {
                            if (!CameraController.Instance.flipped)
                            {
                                CameraController.Instance.flipped = true;
                                CameraController.Instance.ThirdPersonCameraGO.transform.Rotate(0.0f, 180f, 0.0f);
                                CameraController.Instance.TabletCameraGO.transform.Rotate(0.0f, 180f, 0.0f);
                                CameraController.Instance.FakeWebCam.transform.Rotate(-180f, 180f, 0.0f);
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
                            CameraController.Instance.ThirdPersonCameraGO.transform.Rotate(0.0f, 180f, 0.0f);
                            CameraController.Instance.TabletCameraGO.transform.Rotate(0.0f, 180f, 0.0f);
                            CameraController.Instance.FakeWebCam.transform.Rotate(-180f, 180f, 0.0f);
                        }
                        CameraController.Instance.fp = false;
                        CameraController.Instance.fpv = true;
                        break;
                    case "FlipCamButton":
                        CameraController.Instance.flipped = !CameraController.Instance.flipped;
                        CameraController.Instance.ThirdPersonCameraGO.transform.Rotate(0.0f, 180f, 0.0f);
                        CameraController.Instance.TabletCameraGO.transform.Rotate(0.0f, 180f, 0.0f);
                        CameraController.Instance.FakeWebCam.transform.Rotate(-180f, 180f, 0.0f);
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
                        if (CameraController.Instance.TabletCamera.nearClipPlane < 0.01)
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
                        if (CameraController.Instance.TabletCamera.nearClipPlane > 1.0)
                        {
                            CameraController.Instance.TabletCamera.nearClipPlane = 0.01f;
                            CameraController.Instance.ThirdPersonCamera.nearClipPlane = 0.01f;
                        }
                        CameraController.Instance.ThirdPersonCamera.nearClipPlane = CameraController.Instance.TabletCamera.nearClipPlane;
                        CameraController.Instance.NearClipText.text = CameraController.Instance.TabletCamera.nearClipPlane.ToString();
                        CameraController.Instance.canbeused = true;
                        break;
                    case "FPButton":
                        CameraController.Instance.fp = !CameraController.Instance.fp;
                        break;
                    case "MinDistDownButton":
                        CameraController.Instance.minDist -= 0.1f;
                        if (CameraController.Instance.minDist < 1)
                        {
                            CameraController.Instance.minDist = 1;
                        }
                        CameraController.Instance.MinDistText.text = CameraController.Instance.minDist.ToString();
                        CameraController.Instance.canbeused = true;
                        break;
                    case "MinDistUpButton":
                        CameraController.Instance.minDist += 0.1f;
                        if (CameraController.Instance.minDist > 10)
                        {
                            CameraController.Instance.minDist = 10;
                        }
                        CameraController.Instance.MinDistText.text = CameraController.Instance.minDist.ToString();
                        CameraController.Instance.canbeused = true;
                        break;
                    case "SpeedUpButton":
                        CameraController.Instance.fpspeed += 0.01f;
                        if (CameraController.Instance.fpspeed > 0.1)
                        {
                            CameraController.Instance.fpspeed = 0.1f;
                        }
                        CameraController.Instance.SpeedText.text = CameraController.Instance.fpspeed.ToString();
                        CameraController.Instance.canbeused = true;
                        break;
                    case "SpeedDownButton":
                        CameraController.Instance.fpspeed -= 0.01f;
                        if (CameraController.Instance.fpspeed < 0.01)
                        {
                            CameraController.Instance.fpspeed = 0.01f;
                        }
                        CameraController.Instance.SpeedText.text = CameraController.Instance.fpspeed.ToString();
                        CameraController.Instance.canbeused = true;
                        break;
                    case "TPModeDownButton":
                        if (CameraController.Instance.TPVMode == CameraController.TPVModes.BACK)
                        {
                            CameraController.Instance.TPVMode = CameraController.TPVModes.FRONT;
                        }
                        else
                        {
                            CameraController.Instance.TPVMode = CameraController.TPVModes.BACK;
                        }
                        CameraController.Instance.TPText.text = CameraController.Instance.TPVMode.ToString();
                        break;
                    case "TPModeUpButton":
                        if (CameraController.Instance.TPVMode == CameraController.TPVModes.BACK)
                        {
                            CameraController.Instance.TPVMode = CameraController.TPVModes.FRONT;
                        }
                        else
                        {
                            CameraController.Instance.TPVMode = CameraController.TPVModes.BACK;
                        }
                        CameraController.Instance.TPText.text = CameraController.Instance.TPVMode.ToString();
                        break;
                    case "TPRotButton":
                        CameraController.Instance.followheadrot = !CameraController.Instance.followheadrot;
                        CameraController.Instance.TPRotText.text = CameraController.Instance.followheadrot.ToString().ToUpper();
                        break;
                    case "TPRotButton1":
                        CameraController.Instance.followheadrot = !CameraController.Instance.followheadrot;
                        CameraController.Instance.TPRotText.text = CameraController.Instance.followheadrot.ToString().ToUpper();
                        break;
                    case "GreenScreenButton":
                        CameraController.Instance.ColorScreenGO.active = !CameraController.Instance.ColorScreenGO.active;
                        if (CameraController.Instance.ColorScreenGO.active)
                        {
                            CameraController.Instance.ColorScreenText.text = "(ENABLED)";
                        }
                        else
                        {
                            CameraController.Instance.ColorScreenText.text = "(DISABLED)";
                        }
                        break;
                    case "RedButton":
                        foreach (Material mat in CameraController.Instance.ScreenMats)
                        {
                            mat.color = Color.red;
                        }
                        break;
                    case "GreenButton":
                        foreach (Material mat in CameraController.Instance.ScreenMats)
                        {
                            mat.color = Color.green;
                        }
                        break;
                    case "BlueButton":
                        foreach (Material mat in CameraController.Instance.ScreenMats)
                        {
                            mat.color = Color.blue;
                        }
                        break;
                }
            }
        }
    }
}
