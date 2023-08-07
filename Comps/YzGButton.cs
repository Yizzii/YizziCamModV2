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
        void OnDisable() { Main.Instance.canbeused = false; }
        void ButtonTimer()
        {
            if (!this.enabled)
            {
                Main.Instance.canbeused = false;
            }
            Main.Instance.canbeused = true;
        }
        void OnTriggerEnter(Collider col)
        {
            if (Main.Instance.canbeused && col.name == "RightHandTriggerCollider" | col.name == "LeftHandTriggerCollider")
            {
                Main.Instance.canbeused = false;
                Invoke("ButtonTimer", 1f);
                switch (this.name)
                {
                    case "BackButton":
                        Main.Instance.MainPage.SetActive(true);
                        Main.Instance.MiscPage.SetActive(false);
                        break;
                    case "ControlsButton":
                        if (!Main.Instance.openedurl)
                        {
                            Application.OpenURL("https://github.com/Yizzii/YizziCamModV2#controls");
                            Main.Instance.openedurl = true;
                        }
                        break;
                    case "SmoothingDownButton":
                        Main.Instance.smoothing -= 0.01f;
                        if (Main.Instance.smoothing < 0.05f)
                        {
                            Main.Instance.smoothing = 0.11f;
                        }
                        Main.Instance.SmoothText.text = Main.Instance.smoothing.ToString();
                        Main.Instance.canbeused = true;
                        break;
                    case "SmoothingUpButton":
                        Main.Instance.smoothing += 0.01f;
                        if (Main.Instance.smoothing > 0.11f)
                        {
                            Main.Instance.smoothing = 0.05f;
                        }
                        Main.Instance.SmoothText.text = Main.Instance.smoothing.ToString();
                        Main.Instance.canbeused = true;
                        break;
                    case "TPVButton":
                        if (Main.Instance.TPVMode == Main.TPVModes.BACK)
                        {
                            if (Main.Instance.flipped)
                            {
                                Main.Instance.flipped = false;
                                Main.Instance.ThirdPersonCameraGO.transform.Rotate(0.0f, 180f, 0.0f);
                                Main.Instance.TabletCameraGO.transform.Rotate(0.0f, 180f, 0.0f);
                                Main.Instance.FakeWebCam.transform.Rotate(-180f, 180f, 0.0f);
                            }
                        }
                        else if (Main.Instance.TPVMode == Main.TPVModes.FRONT)
                        {
                            if (!Main.Instance.flipped)
                            {
                                Main.Instance.flipped = true;
                                Main.Instance.ThirdPersonCameraGO.transform.Rotate(0.0f, 180f, 0.0f);
                                Main.Instance.TabletCameraGO.transform.Rotate(0.0f, 180f, 0.0f);
                                Main.Instance.FakeWebCam.transform.Rotate(-180f, 180f, 0.0f);
                            }
                        }
                        Main.Instance.fp = false;
                        Main.Instance.fpv = false;
                        Main.Instance.tpv = true;
                        break;
                    case "FPVButton":
                        if (Main.Instance.flipped)
                        {
                            Main.Instance.flipped = false;
                            Main.Instance.ThirdPersonCameraGO.transform.Rotate(0.0f, 180f, 0.0f);
                            Main.Instance.TabletCameraGO.transform.Rotate(0.0f, 180f, 0.0f);
                            Main.Instance.FakeWebCam.transform.Rotate(-180f, 180f, 0.0f);
                        }
                        Main.Instance.fp = false;
                        Main.Instance.fpv = true;
                        break;
                    case "FlipCamButton":
                        Main.Instance.flipped = !Main.Instance.flipped;
                        Main.Instance.ThirdPersonCameraGO.transform.Rotate(0.0f, 180f, 0.0f);
                        Main.Instance.TabletCameraGO.transform.Rotate(0.0f, 180f, 0.0f);
                        Main.Instance.FakeWebCam.transform.Rotate(-180f, 180f, 0.0f);
                        break;
                    case "FovDown":
                        Main.Instance.TabletCamera.fieldOfView -= 5f;
                        if (Main.Instance.TabletCamera.fieldOfView < 20)
                        {
                            Main.Instance.TabletCamera.fieldOfView = 130f;
                            Main.Instance.ThirdPersonCamera.fieldOfView = 130f;
                        }
                        Main.Instance.ThirdPersonCamera.fieldOfView = Main.Instance.TabletCamera.fieldOfView;
                        Main.Instance.FovText.text = Main.Instance.TabletCamera.fieldOfView.ToString();
                        Main.Instance.canbeused = true;
                        break;
                    case "FovUP":
                        Main.Instance.TabletCamera.fieldOfView += 5f;
                        if (Main.Instance.TabletCamera.fieldOfView > 130)
                        {
                            Main.Instance.TabletCamera.fieldOfView = 20f;
                            Main.Instance.ThirdPersonCamera.fieldOfView = 20f;
                        }
                        Main.Instance.ThirdPersonCamera.fieldOfView = Main.Instance.TabletCamera.fieldOfView;
                        Main.Instance.FovText.text = Main.Instance.TabletCamera.fieldOfView.ToString();
                        Main.Instance.canbeused = true;
                        break;
                    case "MiscButton":
                        Main.Instance.MainPage.SetActive(false);
                        Main.Instance.MiscPage.SetActive(true);
                        break;
                    case "NearClipDown":
                        Main.Instance.TabletCamera.nearClipPlane -= 0.01f;
                        if (Main.Instance.TabletCamera.nearClipPlane < 0.01)
                        {
                            Main.Instance.TabletCamera.nearClipPlane = 1f;
                            Main.Instance.ThirdPersonCamera.nearClipPlane = 1f;
                        }
                        Main.Instance.ThirdPersonCamera.nearClipPlane = Main.Instance.TabletCamera.nearClipPlane;
                        Main.Instance.NearClipText.text = Main.Instance.TabletCamera.nearClipPlane.ToString();
                        Main.Instance.canbeused = true;
                        break;
                    case "NearClipUp":
                        Main.Instance.TabletCamera.nearClipPlane += 0.01f;
                        if (Main.Instance.TabletCamera.nearClipPlane > 1.0)
                        {
                            Main.Instance.TabletCamera.nearClipPlane = 0.01f;
                            Main.Instance.ThirdPersonCamera.nearClipPlane = 0.01f;
                        }
                        Main.Instance.ThirdPersonCamera.nearClipPlane = Main.Instance.TabletCamera.nearClipPlane;
                        Main.Instance.NearClipText.text = Main.Instance.TabletCamera.nearClipPlane.ToString();
                        Main.Instance.canbeused = true;
                        break;
                    case "FPButton":
                        Main.Instance.fp = !Main.Instance.fp;
                        break;
                    case "MinDistDownButton":
                        Main.Instance.minDist -= 0.1f;
                        if (Main.Instance.minDist < 1)
                        {
                            Main.Instance.minDist = 1;
                        }
                        Main.Instance.MinDistText.text = Main.Instance.minDist.ToString();
                        Main.Instance.canbeused = true;
                        break;
                    case "MinDistUpButton":
                        Main.Instance.minDist += 0.1f;
                        if (Main.Instance.minDist > 10)
                        {
                            Main.Instance.minDist = 10;
                        }
                        Main.Instance.MinDistText.text = Main.Instance.minDist.ToString();
                        Main.Instance.canbeused = true;
                        break;
                    case "SpeedUpButton":
                        Main.Instance.fpspeed += 0.01f;
                        if (Main.Instance.fpspeed > 0.1)
                        {
                            Main.Instance.fpspeed = 0.1f;
                        }
                        Main.Instance.SpeedText.text = Main.Instance.fpspeed.ToString();
                        Main.Instance.canbeused = true;
                        break;
                    case "SpeedDownButton":
                        Main.Instance.fpspeed -= 0.01f;
                        if (Main.Instance.fpspeed < 0.01)
                        {
                            Main.Instance.fpspeed = 0.01f;
                        }
                        Main.Instance.SpeedText.text = Main.Instance.fpspeed.ToString();
                        Main.Instance.canbeused = true;
                        break;
                    case "TPModeDownButton":
                        if (Main.Instance.TPVMode == Main.TPVModes.BACK)
                        {
                            Main.Instance.TPVMode = Main.TPVModes.FRONT;
                        }
                        else
                        {
                            Main.Instance.TPVMode = Main.TPVModes.BACK;
                        }
                        Main.Instance.TPText.text = Main.Instance.TPVMode.ToString();
                        break;
                    case "TPModeUpButton":
                        if (Main.Instance.TPVMode == Main.TPVModes.BACK)
                        {
                            Main.Instance.TPVMode = Main.TPVModes.FRONT;
                        }
                        else
                        {
                            Main.Instance.TPVMode = Main.TPVModes.BACK;
                        }
                        Main.Instance.TPText.text = Main.Instance.TPVMode.ToString();
                        break;
                    case "TPRotButton":
                        Main.Instance.followheadrot = !Main.Instance.followheadrot;
                        Main.Instance.TPRotText.text = Main.Instance.followheadrot.ToString().ToUpper();
                        break;
                    case "TPRotButton1":
                        Main.Instance.followheadrot = !Main.Instance.followheadrot;
                        Main.Instance.TPRotText.text = Main.Instance.followheadrot.ToString().ToUpper();
                        break;
                    case "GreenScreenButton":
                        Main.Instance.ColorScreenGO.active = !Main.Instance.ColorScreenGO.active;
                        if (Main.Instance.ColorScreenGO.active)
                        {
                            Main.Instance.ColorScreenText.text = "(ENABLED)";
                        }
                        else
                        {
                            Main.Instance.ColorScreenText.text = "(DISABLED)";
                        }
                        break;
                    case "RedButton":
                        foreach (Material mat in Main.Instance.ScreenMats)
                        {
                            mat.color = Color.red;
                        }
                        break;
                    case "GreenButton":
                        foreach (Material mat in Main.Instance.ScreenMats)
                        {
                            mat.color = Color.green;
                        }
                        break;
                    case "BlueButton":
                        foreach (Material mat in Main.Instance.ScreenMats)
                        {
                            mat.color = Color.blue;
                        }
                        break;
                }
            }
        }
    }
}
