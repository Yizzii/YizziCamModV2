using System.Collections.Generic;
using System.IO;
using System.Reflection;
using BepInEx;
using Cinemachine;
using GorillaLocomotion;
using UnityEngine;
using UnityEngine.UI;
using YizziCamModV2.Comps;
#pragma warning disable CS0618

namespace YizziCamModV2
{
    public class CameraController : MonoBehaviour
    {
        public static CameraController Instance;

        public GameObject CameraTablet;
        public GameObject FirstPersonCameraGO;
        public GameObject ThirdPersonCameraGO;
        public GameObject CMVirtualCameraGO;
        public GameObject LeftHandGO;
        public GameObject RightHandGO;
        public GameObject FakeWebCam;
        public GameObject TabletCameraGO;
        public GameObject MainPage;
        public GameObject MiscPage;
        public GameObject LeftGrabCol;
        public GameObject RightGrabCol;
        public GameObject CameraFollower;
        public GameObject TPVBodyFollower;
        public GameObject ColorScreenGO;
        public List<GameObject> Buttons = new List<GameObject>();
        public List<GameObject> ColorButtons = new List<GameObject>();
        public List<Material> ScreenMats = new List<Material>();
        public List<MeshRenderer> meshRenderers = new List<MeshRenderer>();

        public Camera TabletCamera;
        public Camera FirstPersonCamera;
        public Camera ThirdPersonCamera;
        public CinemachineVirtualCamera CMVirtualCamera;

        public Text FovText;
        public Text NearClipText;
        public Text ColorScreenText;
        public Text MinDistText;
        public Text SpeedText;
        public Text SmoothText;
        public Text TPText;
        public Text TPRotText;

        public bool followheadrot = true;
        public bool canbeused;
        public bool flipped;
        public bool tpv;
        public bool fpv;
        public bool fp;
        public bool openedurl;
        public float minDist = 2f;
        float dist;
        public float fpspeed = 0.01f;
        public float smoothing = 0.05f;
        Vector3 targetPosition;
        Vector3 velocity = Vector3.zero;
        public TPVModes TPVMode = TPVModes.BACK;
        bool init;

        void Awake()
        {
            Instance = this;
        }

        public void YizziStart()
        {
            // Add InputManager and UI
            this.gameObject.AddComponent<InputManager>();
            this.gameObject.AddComponent<UI>();

            // Load assets safely
            ColorScreenGO = LoadBundle("ColorScreen", "YizziCamModV2.Assets.colorscreen");
            CameraTablet = LoadBundle("CameraTablet", "YizziCamModV2.Assets.yizzicamera");

            // Get core player objects
            FirstPersonCameraGO = GorillaTagger.Instance.mainCamera;
            ThirdPersonCameraGO = GameObject.Find("Player Objects/Third Person Camera/Shoulder Camera");
            CMVirtualCameraGO = GameObject.Find("Player Objects/Third Person Camera/Shoulder Camera/CM vcam1");
            TPVBodyFollower = GorillaTagger.Instance.bodyCollider.gameObject;

            CMVirtualCamera = CMVirtualCameraGO?.GetComponent<CinemachineVirtualCamera>();
            FirstPersonCamera = FirstPersonCameraGO?.GetComponent<Camera>();
            ThirdPersonCamera = ThirdPersonCameraGO?.GetComponent<Camera>();
            LeftHandGO = GorillaTagger.Instance.leftHandTransform.gameObject;
            RightHandGO = GorillaTagger.Instance.rightHandTransform.gameObject;

            // Adjust CameraTablet
            if (CameraTablet != null) CameraTablet.transform.localScale = Vector3.one * 0.3f;
            CameraFollower = GameObject.Find("Player Objects/Player VR Controller/GorillaPlayer/TurnParent/Main Camera/Camera");

            TabletCameraGO = CameraTablet?.transform.Find("Camera")?.gameObject;
            TabletCamera = TabletCameraGO?.GetComponent<Camera>();
            FakeWebCam = CameraTablet?.transform.Find("FakeCamera")?.gameObject;

            LeftGrabCol = CameraTablet?.transform.Find("LeftGrabCol")?.gameObject;
            RightGrabCol = CameraTablet?.transform.Find("RightGrabCol")?.gameObject;
            LeftGrabCol?.AddComponent<LeftGrabTrigger>();
            RightGrabCol?.AddComponent<RightGrabTrigger>();

            MainPage = CameraTablet?.transform.Find("MainPage")?.gameObject;
            MiscPage = CameraTablet?.transform.Find("MiscPage")?.gameObject;

            // UI Text
            FovText = CameraTablet?.transform.Find("MainPage/Canvas/FovValueText")?.GetComponent<Text>();
            SmoothText = CameraTablet?.transform.Find("MainPage/Canvas/SmoothingValueText")?.GetComponent<Text>();
            NearClipText = CameraTablet?.transform.Find("MainPage/Canvas/NearClipValueText")?.GetComponent<Text>();
            MinDistText = CameraTablet?.transform.Find("MiscPage/Canvas/MinDistValueText")?.GetComponent<Text>();
            SpeedText = CameraTablet?.transform.Find("MiscPage/Canvas/SpeedValueText")?.GetComponent<Text>();
            TPText = CameraTablet?.transform.Find("MiscPage/Canvas/TPText")?.GetComponent<Text>();
            TPRotText = CameraTablet?.transform.Find("MiscPage/Canvas/TPRotText")?.GetComponent<Text>();
            ColorScreenText = ColorScreenGO?.transform.Find("MiscPage/Canvas/ColorScreenText")?.GetComponent<Text>();

            // Buttons
            Buttons.AddRange(new GameObject[] {
                CameraTablet?.transform.Find("MainPage/MiscButton")?.gameObject,
                CameraTablet?.transform.Find("MainPage/FPVButton")?.gameObject,
                CameraTablet?.transform.Find("MainPage/FovUP")?.gameObject,
                CameraTablet?.transform.Find("MainPage/FovDown")?.gameObject,
                CameraTablet?.transform.Find("MainPage/FlipCamButton")?.gameObject,
                CameraTablet?.transform.Find("MainPage/NearClipUp")?.gameObject,
                CameraTablet?.transform.Find("MainPage/NearClipDown")?.gameObject,
                CameraTablet?.transform.Find("MainPage/FPButton")?.gameObject,
                CameraTablet?.transform.Find("MainPage/ControlsButton")?.gameObject,
                CameraTablet?.transform.Find("MainPage/TPVButton")?.gameObject,
                CameraTablet?.transform.Find("MainPage/SmoothingDownButton")?.gameObject,
                CameraTablet?.transform.Find("MainPage/SmoothingUpButton")?.gameObject,
                CameraTablet?.transform.Find("MiscPage/BackButton")?.gameObject,
                CameraTablet?.transform.Find("MiscPage/GreenScreenButton")?.gameObject,
                CameraTablet?.transform.Find("MiscPage/MinDistDownButton")?.gameObject,
                CameraTablet?.transform.Find("MiscPage/MinDistUpButton")?.gameObject,
                CameraTablet?.transform.Find("MiscPage/SpeedDownButton")?.gameObject,
                CameraTablet?.transform.Find("MiscPage/SpeedUpButton")?.gameObject,
                CameraTablet?.transform.Find("MiscPage/TPModeDownButton")?.gameObject,
                CameraTablet?.transform.Find("MiscPage/TPModeUpButton")?.gameObject,
                CameraTablet?.transform.Find("MiscPage/TPRotButton")?.gameObject,
                CameraTablet?.transform.Find("MiscPage/TPRotButton1")?.gameObject
            });

            foreach (var btn in Buttons) btn?.AddComponent<YzGButton>();

            // Color buttons and materials
            ColorButtons.AddRange(new GameObject[] {
                ColorScreenGO?.transform.Find("Stuff/RedButton")?.gameObject,
                ColorScreenGO?.transform.Find("Stuff/GreenButton")?.gameObject,
                ColorScreenGO?.transform.Find("Stuff/BlueButton")?.gameObject
            });

            foreach (var btn in ColorButtons) btn?.AddComponent<YzGButton>();

            ScreenMats.AddRange(new Material[] {
                ColorScreenGO?.transform.Find("Screen1")?.GetComponent<MeshRenderer>()?.material,
                ColorScreenGO?.transform.Find("Screen2")?.GetComponent<MeshRenderer>()?.material,
                ColorScreenGO?.transform.Find("Screen3")?.GetComponent<MeshRenderer>()?.material
            });

            meshRenderers.AddRange(new MeshRenderer[] {
                CameraTablet?.transform.Find("FakeCamera")?.GetComponent<MeshRenderer>(),
                CameraTablet?.transform.Find("Tablet")?.GetComponent<MeshRenderer>(),
                CameraTablet?.transform.Find("Handle")?.GetComponent<MeshRenderer>(),
                CameraTablet?.transform.Find("Handle2")?.GetComponent<MeshRenderer>()
            });

            // Position color screen
            ColorScreenGO?.transform.SetPositionAndRotation(new Vector3(-54.3f, 16.21f, -122.96f), Quaternion.Euler(0, 30, 0));
            ColorScreenGO?.SetActive(false);
            MiscPage?.SetActive(false);

            // Camera settings
            ThirdPersonCamera.nearClipPlane = 0.1f;
            TabletCamera.nearClipPlane = 0.1f;
            FakeWebCam?.transform.Rotate(-180, 180, 0);

            init = true;
        }

        public enum TPVModes
        {
            BACK,
            FRONT
        }

        void LateUpdate()
        {
            if (!init) return;

            // FPV mode
            if (fpv)
            {
                if (MainPage?.activeSelf ?? false)
                {
                    foreach (var mr in meshRenderers) if (mr) mr.enabled = false;
                    MainPage.SetActive(false);
                }
                CameraTablet.transform.position = CameraFollower.transform.position;
                CameraTablet.transform.rotation = Quaternion.Lerp(CameraTablet.transform.rotation, CameraFollower.transform.rotation, smoothing);
            }

            // Exit cameras
            if (InputManager.instance.LeftPrimaryButton && CameraTablet.transform.parent == null)
            {
                fp = fpv = tpv = false;
                if (!(MainPage?.activeSelf ?? false))
                {
                    foreach (var btn in Buttons) btn?.SetActive(true);
                    foreach (var mr in meshRenderers) if (mr) mr.enabled = true;
                    CameraTablet.transform.Rotate(0f, -180f, 0f);
                    MainPage.SetActive(true);
                }
                CameraTablet.transform.position = Player.Instance.headCollider.transform.position + Player.Instance.headCollider.transform.forward;
            }

            // FP camera
            if (fp)
            {
                CameraTablet.transform.LookAt(2f * CameraTablet.transform.position - CameraFollower.transform.position);
                if (!flipped)
                {
                    flipped = true;
                    ThirdPersonCameraGO.transform.Rotate(0.0f, 180f, 0.0f);
                    TabletCameraGO.transform.Rotate(0.0f, 180f, 0.0f);
                    FakeWebCam.transform.Rotate(-180f, 180f, 0.0f);
                }
                dist = Vector3.Distance(CameraFollower.transform.position, CameraTablet.transform.position);
                if (dist > minDist)
                    CameraTablet.transform.position = Vector3.Lerp(CameraTablet.transform.position, CameraFollower.transform.position, fpspeed);
            }

            // TPV camera
            if (tpv)
            {
                if (MainPage?.activeSelf ?? false)
                {
                    foreach (var mr in meshRenderers) if (mr) mr.enabled = false;
                    MainPage.SetActive(false);
                }

                Vector3 offset = TPVMode == TPVModes.BACK ? new Vector3(0.3f, 0.1f, -1.5f) : new Vector3(0.1f, 0.3f, 2.5f);
                targetPosition = (followheadrot ? CameraFollower.transform : TPVBodyFollower.transform).TransformPoint(offset);
                CameraTablet.transform.position = Vector3.SmoothDamp(CameraTablet.transform.position, targetPosition, ref velocity, 0.1f);

                if (TPVMode == TPVModes.BACK)
                    CameraTablet.transform.LookAt(CameraFollower.transform.position);
                else
                    CameraTablet.transform.LookAt(2f * CameraTablet.transform.position - CameraFollower.transform.position);

                if (InputManager.instance.LeftPrimaryButton)
                {
                    CameraTablet.transform.position = Player.Instance.headCollider.transform.position + Player.Instance.headCollider.transform.forward;
                    foreach (var mr in meshRenderers) if (mr) mr.enabled = true;
                    CameraTablet.transform.parent = null;
                    tpv = false;
                }
            }
        }

        GameObject LoadBundle(string goname, string resourcename)
        {
            Stream str = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourcename);
            AssetBundle asb = AssetBundle.LoadFromStream(str);
            GameObject go = Instantiate(asb.LoadAsset<GameObject>(goname));
            asb.Unload(false);
            str.Close();
            return go;
        }
    }
}
