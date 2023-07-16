using System.IO;
using System.Reflection;
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
        public GameObject CameraTablet;
        public GameObject CamFollower;
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
        public GameObject ColorScreenGO;
        public FastList<GameObject> Buttons = new FastList<GameObject>();
        public FastList<GameObject> ColorButtons = new FastList<GameObject>();
        public FastList<Material> ScreenMats = new FastList<Material>();
        public FastList<MeshRenderer> meshRenderers = new FastList<MeshRenderer>();

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
        Vector3 velocity = Vector3.zero;

        InputManager Input;
        void Start()
        {
            Input = GameObject.CreatePrimitive(PrimitiveType.Cube).AddComponent<InputManager>();
            Input.transform.name = "YzGInputManager";
            ColorScreenGO = LoadBundle("ColorScreen", "YizziCamModV2.Assets.colorscreen");
            CameraTablet = LoadBundle("CameraTablet", "YizziCamModV2.Assets.yizzicamera");
            CamFollower = GameObject.Find("Player VR Controller/GorillaPlayer/TurnParent/Main Camera/Camera Follower");
            FirstPersonCameraGO = GameObject.Find("Player VR Controller/GorillaPlayer/TurnParent/Main Camera");
            ThirdPersonCameraGO = GameObject.Find("Global/Third Person Camera/Shoulder Camera");
            CMVirtualCameraGO = GameObject.Find("Global/Third Person Camera/Shoulder Camera/CM vcam1");
            FirstPersonCamera = GameObject.Find("Player VR Controller/GorillaPlayer/TurnParent/Main Camera").GetComponent<Camera>();
            ThirdPersonCamera = GameObject.Find("Global/Third Person Camera/Shoulder Camera").GetComponent<Camera>();
            CMVirtualCamera = CMVirtualCameraGO.GetComponent<CinemachineVirtualCamera>();
            LeftHandGO = GameObject.Find("Player VR Controller/GorillaPlayer/TurnParent/LeftHand Controller");
            RightHandGO = GameObject.Find("Player VR Controller/GorillaPlayer/TurnParent/RightHand Controller");
            CameraTablet.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            CameraFollower = GameObject.Find("Player VR Controller/GorillaPlayer/TurnParent/Main Camera/Camera Follower");
            TabletCameraGO = GameObject.Find("CameraTablet(Clone)/Camera");
            TabletCamera = TabletCameraGO.GetComponent<Camera>();
            FakeWebCam = GameObject.Find("CameraTablet(Clone)/FakeCamera");
            LeftGrabCol = GameObject.Find("CameraTablet(Clone)/LeftGrabCol");
            RightGrabCol = GameObject.Find("CameraTablet(Clone)/RightGrabCol");
            LeftGrabCol.AddComponent<LeftGrabTrigger>();
            RightGrabCol.AddComponent<RightGrabTrigger>();
            MainPage = GameObject.Find("CameraTablet(Clone)/MainPage");
            MiscPage = GameObject.Find("CameraTablet(Clone)/MiscPage");
            FovText = GameObject.Find("CameraTablet(Clone)/MainPage/Canvas/FovValueText").GetComponent<Text>();
            SmoothText = GameObject.Find("CameraTablet(Clone)/MainPage/Canvas/SmoothingValueText").GetComponent<Text>();
            NearClipText = GameObject.Find("CameraTablet(Clone)/MainPage/Canvas/NearClipValueText").GetComponent<Text>();
            MinDistText = GameObject.Find("CameraTablet(Clone)/MiscPage/Canvas/MinDistValueText").GetComponent<Text>();
            SpeedText = GameObject.Find("CameraTablet(Clone)/MiscPage/Canvas/SpeedValueText").GetComponent<Text>();
            Buttons.AddUnique(GameObject.Find("CameraTablet(Clone)/MainPage/MiscButton"));
            Buttons.AddUnique(GameObject.Find("CameraTablet(Clone)/MainPage/FPVButton"));
            Buttons.AddUnique(GameObject.Find("CameraTablet(Clone)/MainPage/FovUP"));
            Buttons.AddUnique(GameObject.Find("CameraTablet(Clone)/MainPage/FovDown"));
            Buttons.AddUnique(GameObject.Find("CameraTablet(Clone)/MainPage/FlipCamButton"));
            Buttons.AddUnique(GameObject.Find("CameraTablet(Clone)/MainPage/NearClipUp"));
            Buttons.AddUnique(GameObject.Find("CameraTablet(Clone)/MainPage/NearClipDown"));
            Buttons.AddUnique(GameObject.Find("CameraTablet(Clone)/MainPage/FPButton"));
            Buttons.AddUnique(GameObject.Find("CameraTablet(Clone)/MainPage/ControlsButton"));
            Buttons.AddUnique(GameObject.Find("CameraTablet(Clone)/MainPage/TPVButton"));
            Buttons.AddUnique(GameObject.Find("CameraTablet(Clone)/MainPage/SmoothingDownButton"));
            Buttons.AddUnique(GameObject.Find("CameraTablet(Clone)/MainPage/SmoothingUpButton"));
            Buttons.AddUnique(GameObject.Find("CameraTablet(Clone)/MiscPage/BackButton"));
            Buttons.AddUnique(GameObject.Find("CameraTablet(Clone)/MiscPage/GreenScreenButton"));
            Buttons.AddUnique(GameObject.Find("CameraTablet(Clone)/MiscPage/MinDistDownButton"));
            Buttons.AddUnique(GameObject.Find("CameraTablet(Clone)/MiscPage/MinDistUpButton"));
            Buttons.AddUnique(GameObject.Find("CameraTablet(Clone)/MiscPage/SpeedDownButton"));
            Buttons.AddUnique(GameObject.Find("CameraTablet(Clone)/MiscPage/SpeedUpButton"));
            foreach (GameObject btns in Buttons)
            {
                btns.AddComponent<YzGButton>();
                btns.GetComponent<YzGButton>().Main = this;
            }
            CMVirtualCamera.enabled = false;
            ThirdPersonCameraGO.transform.SetParent(CameraTablet.transform, true);
            CameraTablet.transform.position = new Vector3(-65, 12, -82);
            ThirdPersonCameraGO.transform.position = TabletCamera.transform.position;
            ThirdPersonCameraGO.transform.rotation = TabletCamera.transform.rotation;
            CameraTablet.transform.Rotate(0, 180, 0);
            ColorScreenText = GameObject.Find("CameraTablet(Clone)/MiscPage/Canvas/ColorScreenText").GetComponent<Text>();
            ColorButtons.AddUnique(GameObject.Find("ColorScreen(Clone)/Stuff/RedButton"));
            ColorButtons.AddUnique(GameObject.Find("ColorScreen(Clone)/Stuff/GreenButton"));
            ColorButtons.AddUnique(GameObject.Find("ColorScreen(Clone)/Stuff/BlueButton"));
            foreach (GameObject btns in ColorButtons)
            {
                btns.AddComponent<YzGButton>();
                btns.GetComponent<YzGButton>().Main = this;
            }
            ScreenMats.AddUnique(GameObject.Find("ColorScreen(Clone)/Screen1").GetComponent<MeshRenderer>().material);
            ScreenMats.AddUnique(GameObject.Find("ColorScreen(Clone)/Screen2").GetComponent<MeshRenderer>().material);
            ScreenMats.AddUnique(GameObject.Find("ColorScreen(Clone)/Screen3").GetComponent<MeshRenderer>().material);
            meshRenderers.AddUnique(GameObject.Find("CameraTablet(Clone)/FakeCamera").GetComponent<MeshRenderer>());
            meshRenderers.AddUnique(GameObject.Find("CameraTablet(Clone)/Tablet").GetComponent<MeshRenderer>());
            meshRenderers.AddUnique(GameObject.Find("CameraTablet(Clone)/Handle").GetComponent<MeshRenderer>());
            meshRenderers.AddUnique(GameObject.Find("CameraTablet(Clone)/Handle2").GetComponent<MeshRenderer>());
            ColorScreenGO.transform.position = new Vector3(-54.3f, 16.21f, -122.96f);
            ColorScreenGO.transform.Rotate(0, 30, 0);
            ColorScreenGO.SetActive(false);
            MiscPage.SetActive(false);
            ThirdPersonCamera.nearClipPlane = 0.1f;
            TabletCamera.nearClipPlane = 0.1f;
            FakeWebCam.transform.Rotate(-180,180,0);
        }
        void LateUpdate()
        {
            if (fpv)
            {
                if (MainPage.active)
                {
                    foreach (MeshRenderer mr in meshRenderers)
                    {
                        mr.enabled = false;
                    }
                    MainPage.active = false;
                }
                CameraTablet.transform.position = CameraFollower.transform.position;
                CameraTablet.transform.rotation = Quaternion.Lerp(CameraTablet.transform.rotation, CameraFollower.transform.rotation, smoothing);
            }
            else if (Input.LeftPrimaryButton &&!tpv&& !fp &&CameraTablet.transform.parent == null)
            {
                CameraTablet.transform.position = Player.Instance.headCollider.transform.position + Player.Instance.headCollider.transform.forward;
            }
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
                {
                    CameraTablet.transform.position = Vector3.Lerp(CameraTablet.transform.position, CameraFollower.transform.position, fpspeed);
                }
            }
            if (tpv)
            {
                Vector3 targetPosition = CameraFollower.transform.TransformPoint(new Vector3(0.3f, 0, -1.5f));
                CameraTablet.transform.position = Vector3.SmoothDamp(CameraTablet.transform.position,targetPosition, ref velocity, 0.1f);
                CameraTablet.transform.rotation = Quaternion.Lerp(CameraTablet.transform.rotation,CameraFollower.transform.rotation,0.1f);
                if (Input.LeftPrimaryButton)
                {
                    CameraTablet.transform.position = Player.Instance.headCollider.transform.position + Player.Instance.headCollider.transform.forward;
                    tpv = false;
                }
            }
        }
        GameObject LoadBundle(string goname, string resourcename)
        {
            Stream str = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourcename);
            AssetBundle asb = AssetBundle.LoadFromStream(str);
            GameObject go = Instantiate<GameObject>(asb.LoadAsset<GameObject>(goname));
            asb.Unload(false);
            str.Close();
            return go;
        }
    }
}
