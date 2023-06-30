using UnityEngine;
namespace YizziCamModV2.Comps
{
    class YzGButton : MonoBehaviour
    {
        public Main Main;
        void Start()
        {
            this.gameObject.layer = 18;
            Main = GameObject.FindObjectOfType<Main>().GetComponent<Main>();
        }
        void OnEnable(){Invoke("ButtonTimer", 1f);}
        void OnDisable(){ Main.canbeused = false;}
        void ButtonTimer()
        {
            if (!this.enabled)
            {
                Main.canbeused = false;
            }
            Main.canbeused = true;
        }
        void OnTriggerEnter(Collider col)
        {
            if (Main.canbeused && col.name == "RightHandTriggerCollider" | col.name == "LeftHandTriggerCollider")
            {
                Main.canbeused = false;
                Invoke("ButtonTimer", 1f);
                switch (this.name)
                {
                    case "BackButton":
                        Main.MainPage.SetActive(true);
                        Main.MiscPage.SetActive(false);
                        break;
                    case "FPVButton":
                        if (Main.flipped)
                        {
                            Main.flipped = false;
                            Main.ThirdPersonCameraGO.transform.Rotate(0.0f, 180f, 0.0f);
                            Main.TabletCameraGO.transform.Rotate(0.0f, 180f, 0.0f);
                            Main.FakeWebCam.transform.Rotate(-180f, 180f, 0.0f);
                        }
                        Main.fpv = true;
                        break;
                    case "FlipCamButton":
                        Main.flipped = !Main.flipped;
                        Main.ThirdPersonCameraGO.transform.Rotate(0.0f, 180f, 0.0f);
                        Main.TabletCameraGO.transform.Rotate(0.0f, 180f, 0.0f);
                        Main.FakeWebCam.transform.Rotate(-180f, 180f, 0.0f);
                        break;
                    case "FovDown":
                        Main.TabletCamera.fieldOfView -= 5f;
                        if (Main.TabletCamera.fieldOfView < 20)
                        {
                            Main.TabletCamera.fieldOfView = 130f;
                            Main.ThirdPersonCamera.fieldOfView = 130f;
                        }
                        Main.ThirdPersonCamera.fieldOfView = Main.TabletCamera.fieldOfView;
                        Main.FovText.text = Main.TabletCamera.fieldOfView.ToString();
                        Main.canbeused = true;
                        break;
                    case "FovUP":
                        Main.TabletCamera.fieldOfView += 5f;
                        if (Main.TabletCamera.fieldOfView > 130)
                        {
                            Main.TabletCamera.fieldOfView = 20f;
                            Main.ThirdPersonCamera.fieldOfView = 20f;
                        }
                        Main.ThirdPersonCamera.fieldOfView = Main.TabletCamera.fieldOfView;
                        Main.FovText.text = Main.TabletCamera.fieldOfView.ToString();
                        Main.canbeused = true;
                        break;
                    case "MiscButton":
                        Main.MainPage.SetActive(false);
                        Main.MiscPage.SetActive(true);
                        break;
                    case "NearClipDown":
                        Main.TabletCamera.nearClipPlane -= 0.01f;
                        if (Main.TabletCamera.nearClipPlane < 0.01)
                        {
                            Main.TabletCamera.nearClipPlane = 1f;
                            Main.ThirdPersonCamera.nearClipPlane = 1f;
                        }
                        Main.ThirdPersonCamera.nearClipPlane = Main.TabletCamera.nearClipPlane;
                        Main.NearClipText.text = Main.TabletCamera.nearClipPlane.ToString();
                        Main.canbeused = true;
                        break;
                    case "NearClipUp":
                        Main.TabletCamera.nearClipPlane += 0.01f;
                        if (Main.TabletCamera.nearClipPlane > 1.0)
                        {
                            Main.TabletCamera.nearClipPlane = 0.01f;
                            Main.ThirdPersonCamera.nearClipPlane = 0.01f;
                        }
                        Main.ThirdPersonCamera.nearClipPlane = Main.TabletCamera.nearClipPlane;
                        Main.NearClipText.text = Main.TabletCamera.nearClipPlane.ToString();
                        Main.canbeused = true;
                        break;
                    case "FPButton":
                        Main.fp = !Main.fp;
                        break;
                    case "MinDistDownButton":
                        Main.minDist -= 0.1f;
                        if (Main.minDist < 1)
                        {
                            Main.minDist = 1;
                        }
                        Main.MinDistText.text = Main.minDist.ToString();
                        Main.canbeused = true;
                        break;
                    case "MinDistUpButton":
                        Main.minDist += 0.1f;
                        if (Main.minDist > 10)
                        {
                            Main.minDist = 10;
                        }
                        Main.MinDistText.text = Main.minDist.ToString();
                        Main.canbeused = true;
                        break;
                    case "SpeedUpButton":
                        Main.fpspeed += 0.01f;
                        if (Main.fpspeed > 0.1)
                        {
                            Main.fpspeed = 0.1f;
                        }
                        Main.SpeedText.text = Main.fpspeed.ToString();
                        Main.canbeused = true;
                        break;
                    case "SpeedDownButton":
                        Main.fpspeed -= 0.01f;
                        if (Main.fpspeed < 0.01)
                        {
                            Main.fpspeed = 0.01f;
                        }
                        Main.SpeedText.text = Main.fpspeed.ToString();
                        Main.canbeused = true;
                        break;
                    case "GreenScreenButton":
                        Main.ColorScreenGO.active = !Main.ColorScreenGO.active;
                        if (Main.ColorScreenGO.active)
                        {
                            Main.ColorScreenText.text = "(ENABLED)";
                        }
                        else
                        {
                            Main.ColorScreenText.text = "(DISABLED)";
                        }
                        break;
                    case "RedButton":
                        foreach (Material mat in Main.ScreenMats)
                        {
                            mat.color = Color.red;
                        }
                        break;
                    case "GreenButton":
                        foreach (Material mat in Main.ScreenMats)
                        {
                            mat.color = Color.green;
                        }
                        break;
                    case "BlueButton":
                        foreach (Material mat in Main.ScreenMats)
                        {
                            mat.color = Color.blue;
                        }
                        break;
                }
            }
        }
    }
}
