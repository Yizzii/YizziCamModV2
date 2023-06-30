using System;
using System.IO;
using System.Reflection;
using BepInEx;
using Cinemachine;
using GorillaLocomotion;
using UnityEngine;
using UnityEngine.UI;
using Utilla;
using YizziCamModV2.Comps;

namespace YizziCamModV2
{
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Main : BaseUnityPlugin
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

        public Camera TabletCamera;
        public Camera FirstPersonCamera;
        public Camera ThirdPersonCamera;
        public CinemachineVirtualCamera CMVirtualCamera;
        
        public Text FovText;
        public Text NearClipText;
        public Text ColorScreenText;
        public Text MinDistText;
        public Text SpeedText;

        public bool canbeused;
        public bool flipped;
        public bool fpv;
        public bool fp;
        public float minDist = 2f;
        float dist;
        public float fpspeed = 0.01f;

        InputManager Input;
        void Start()
        {
            Utilla.Events.GameInitialized += OnGameInitialized;
        }

        void OnGameInitialized(object sender, EventArgs e)
        {
            Input = GameObject.CreatePrimitive(PrimitiveType.Cube).AddComponent<InputManager>();
            Input.transform.name = "YzGInputManager";
            ColorScreenGO = LoadBundle("ColorScreen", "YizziCamModV2.Assets.colorscreen");
            CameraTablet = LoadBundle("CameraTablet","YizziCamModV2.Assets.yizzicamera");
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
            LeftGrabCol.AddComponent<GrabTrigger>();
            RightGrabCol.AddComponent<GrabTrigger>();
            MainPage = GameObject.Find("CameraTablet(Clone)/MainPage");
            MiscPage = GameObject.Find("CameraTablet(Clone)/MiscPage");
            FovText = GameObject.Find("CameraTablet(Clone)/MainPage/Canvas/FovText").GetComponent<Text>();
            MinDistText = GameObject.Find("CameraTablet(Clone)/MiscPage/Canvas (1)/MinDistText").GetComponent<Text>();
            SpeedText = GameObject.Find("CameraTablet(Clone)/MiscPage/Canvas (1)/SpeedText").GetComponent<Text>();
            NearClipText = GameObject.Find("CameraTablet(Clone)/MainPage/Canvas/NearClipText").GetComponent<Text>();
            Buttons.AddUnique(GameObject.Find("CameraTablet(Clone)/MainPage/MiscButton"));
            Buttons.AddUnique(GameObject.Find("CameraTablet(Clone)/MainPage/FPVButton"));
            Buttons.AddUnique(GameObject.Find("CameraTablet(Clone)/MainPage/FovUP"));
            Buttons.AddUnique(GameObject.Find("CameraTablet(Clone)/MainPage/FovDown"));
            Buttons.AddUnique(GameObject.Find("CameraTablet(Clone)/MainPage/FlipCamButton"));
            Buttons.AddUnique(GameObject.Find("CameraTablet(Clone)/MainPage/FlipCamButton"));
            Buttons.AddUnique(GameObject.Find("CameraTablet(Clone)/MainPage/NearClipUp"));
            Buttons.AddUnique(GameObject.Find("CameraTablet(Clone)/MainPage/NearClipDown"));
            Buttons.AddUnique(GameObject.Find("CameraTablet(Clone)/MainPage/FPButton"));
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
            ThirdPersonCameraGO.transform.SetParent(CameraTablet.transform,true);
            CameraTablet.transform.position = new Vector3(-65, 12, -82);
            ThirdPersonCameraGO.transform.position = TabletCamera.transform.position;
            ThirdPersonCameraGO.transform.rotation = TabletCamera.transform.rotation;
            CameraTablet.transform.Rotate(0,180,0);
            ColorScreenText = GameObject.Find("CameraTablet(Clone)/MiscPage/Canvas (1)/ColorScreenText").GetComponent<Text>();
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
            ColorScreenGO.transform.position = new Vector3(-54.3f, 16.21f, -122.96f);
            ColorScreenGO.transform.Rotate(0,30,0);
            ColorScreenGO.SetActive(false);
            MiscPage.SetActive(false);
        }
        void FixedUpdate()
        {
            if (fpv)
            {
                if (CameraTablet.transform.parent == null)
                {
                    foreach (GameObject btns in Buttons)
                    {
                        btns.SetActive(false);
                    }
                    CameraTablet.transform.position = CameraFollower.transform.position;
                    CameraTablet.transform.rotation = Player.Instance.headCollider.transform.rotation;
                    CameraTablet.transform.Rotate(0f, -180f, 0f);
                    TabletCamera.nearClipPlane = 0.1f;
                    ThirdPersonCamera.nearClipPlane = 0.1f;
                    CameraTablet.transform.SetParent(CameraFollower.transform);
                }
            }
            if (fp)
            {
                CameraTablet.transform.LookAt(CameraFollower.transform.position);
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
                    CameraTablet.transform.position = Vector3.Lerp(CameraTablet.transform.position,CameraFollower.transform.position,fpspeed);
                }
            }
        }
        GameObject LoadBundle(string goname,string resourcename)
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
