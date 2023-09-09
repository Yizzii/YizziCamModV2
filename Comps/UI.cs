using Photon.Pun;
using UnityEngine;
using GorillaLocomotion;
using UnityEngine.InputSystem;
#pragma warning disable CS0618
namespace YizziCamModV2.Comps
{
    class UI : MonoBehaviour
    {
        public GameObject forest;
        public GameObject cave;
        public GameObject canyon;
        public GameObject mountain;
        public GameObject city;
        public GameObject clouds;
        public GameObject cloudsbottom;
        public GameObject beach;
        public GameObject beachthing;
        public GameObject basement;
        public GameObject citybuildings;

        GameObject rigcache;
        bool keyp;
        bool uiopen;
        bool specui;
        bool freecam;
        bool spectating;
        bool controllerfreecam;
        bool speclookat;
        bool controloffset;
        GameObject followobject;
        float freecamspeed = 0.1f;
        float freecamsens = 1f;
        float rotX;
        float rotY;
        float posY;
        Vector3 specoffset = new Vector3(0.3f, 0.1f, -1.5f);
        Vector3 velocity = Vector3.zero;
        void Start()
        {
            rigcache = GameObject.Find("Player Objects/RigCache/Rig Parent");
            forest = GameObject.Find("Environment Objects/LocalObjects_Prefab/Forest");
            city = GameObject.Find("Environment Objects/LocalObjects_Prefab/City");
            canyon = GameObject.Find("Environment Objects/LocalObjects_Prefab/Canyon");
            cave = GameObject.Find("Environment Objects/LocalObjects_Prefab/Cave_Main_Prefab");
            mountain = GameObject.Find("Environment Objects/LocalObjects_Prefab/Mountain");
            clouds = GameObject.Find("Environment Objects/LocalObjects_Prefab/skyjungle");
            cloudsbottom = GameObject.Find("Environment Objects/LocalObjects_Prefab/Forest/Sky Jungle Bottom (1)/CloudSmall (22)");
            beach = GameObject.Find("Environment Objects/LocalObjects_Prefab/Beach");
            beachthing = GameObject.Find("Environment Objects/LocalObjects_Prefab/ForestToBeach");
            basement = GameObject.Find("Environment Objects/LocalObjects_Prefab/Basement");
            citybuildings = GameObject.Find("Environment Objects/LocalObjects_Prefab/City/CosmeticsRoomAnchor/rain");
        }
        void OnGUI()
        {
            if (uiopen)
            {
                GUI.Box(new Rect(30f, 50f, 170f, 270f), "Yizzi's Camera Mod");
                if (GUI.Button(new Rect(35f, 70f, 160f, 20f), "FreeCam"))
                {
                    if (spectating)
                    {
                        spectating = false;
                        followobject = null;
                    }
                    if (freecam)
                    {
                        CameraController.Instance.CameraTablet.transform.position = Player.Instance.headCollider.transform.position + Player.Instance.headCollider.transform.forward;
                    }
                    if (!CameraController.Instance.flipped)
                    {
                        CameraController.Instance.flipped = true;
                        CameraController.Instance.ThirdPersonCameraGO.transform.Rotate(0.0f, 180f, 0.0f);
                        CameraController.Instance.TabletCameraGO.transform.Rotate(0.0f, 180f, 0.0f);
                        CameraController.Instance.FakeWebCam.transform.Rotate(-180f, 180f, 0.0f);
                    }
                    CameraController.Instance.fpv = false;
                    CameraController.Instance.fp = false;
                    CameraController.Instance.tpv = false;
                    freecam = !freecam;
                }
                if (GUI.Button(new Rect(35f, 90f, 100f, 20f), "Spectator"))
                {
                    if (!freecam)
                    {
                        if (PhotonNetwork.InRoom)
                        {
                            specui = !specui;
                        }
                    }
                    CameraController.Instance.fpv = false;
                    CameraController.Instance.fp = false;
                    CameraController.Instance.tpv = false;
                }
                if (GUI.Button(new Rect(140f, 90f, 45f, 20f), "Stop"))
                {
                    if (spectating)
                    {
                        followobject = null;
                        CameraController.Instance.CameraTablet.transform.position = Player.Instance.headCollider.transform.position + Player.Instance.headCollider.transform.forward;
                        spectating = false;
                    }
                }
                if (GUI.Button(new Rect(35f, 110f, 160f, 20f), "Load All Maps(PRIVS)"))
                {
                    if (!PhotonNetwork.CurrentRoom.IsVisible)
                    {
                        forest.SetActive(true);
                        cave.SetActive(true);
                        canyon.SetActive(true);
                        beach.SetActive(true);
                        beachthing.SetActive(true);
                        city.SetActive(true);
                        mountain.SetActive(true);
                        basement.SetActive(true);
                        clouds.SetActive(true);
                        cloudsbottom.SetActive(false);
                        citybuildings.SetActive(false);
                    }
                }
                if (specui)
                {
                    int i = 1;
                    foreach (VRRig player in rigcache.GetComponentsInChildren<VRRig>())
                    {
                        if (player.transform.parent.gameObject.active)
                        {
                            GUI.Label(new Rect(250, 20 + (i * 25), 160, 20), player.playerText.text);
                            if (GUI.Button(new Rect(360, 20 + (i * 25), 67, 20), "Spectate"))
                            {
                                followobject = player.gameObject;
                                spectating = true;
                                CameraController.Instance.fp = false;
                                CameraController.Instance.fpv = false;
                                CameraController.Instance.tpv = false;
                                if (CameraController.Instance.flipped)
                                {
                                    CameraController.Instance.flipped = false;
                                    CameraController.Instance.ThirdPersonCameraGO.transform.Rotate(0.0f, 180f, 0.0f);
                                    CameraController.Instance.TabletCameraGO.transform.Rotate(0.0f, 180f, 0.0f);
                                    CameraController.Instance.FakeWebCam.transform.Rotate(-180f, 180f, 0.0f);
                                }
                            }
                        }
                        i++;
                    }
                }

                controllerfreecam = GUI.Toggle(new Rect(30f, 130f, 160f, 19f), controllerfreecam, "Controller Freecam");
                controloffset = GUI.Toggle(new Rect(30f, 150f, 170f, 19f), controloffset, "Control Offset with WASD");
                speclookat = GUI.Toggle(new Rect(30f, 170f, 170f, 19f), speclookat, "Spectator Stare");
                GUI.Label(new Rect(35f, 188f, 160f, 30f), "         Spectator Offset:");
                GUI.Label(new Rect(35f, 200f, 160f, 30f), "     X            Y            Z");
                specoffset.x = GUI.HorizontalSlider(new Rect(35f, 215f, 50f, 20f), specoffset.x, -3, 3);
                specoffset.y = GUI.HorizontalSlider(new Rect(90f, 215f, 50f, 20f), specoffset.y, -3, 3);
                specoffset.z = GUI.HorizontalSlider(new Rect(145f, 215f, 50f, 20f), specoffset.z, -3, 3);

                GUI.Label(new Rect(35f, 232f, 160f, 30f), "          Freecam Speed");
                freecamspeed = GUI.HorizontalSlider(new Rect(35f, 250f, 160f, 5f), freecamspeed, 0.01f, 0.4f);
                GUI.Label(new Rect(35f, 258f, 160f, 20f), "0                0.5               1");
                GUI.Label(new Rect(35f, 275f, 160f, 30f), "          Freecam Sens");
                freecamsens = GUI.HorizontalSlider(new Rect(35f, 293f, 160f, 5f), freecamsens, 0.01f, 2f);
                GUI.Label(new Rect(35f, 301f, 160f, 20f), "0                0.5               1");

                if (!PhotonNetwork.InRoom)
                {
                    specui = false;
                    followobject = null;
                }
            }
            if (Keyboard.current.tabKey.isPressed)
            {
                if (!keyp)
                {
                    uiopen = !uiopen;
                }
                keyp = true;
            }
            else
            {
                keyp = false;
            }
        }
        void LateUpdate()
        {
            Spec();
            Freecam();
        }

        void Freecam()
        {
            if (freecam && !controllerfreecam)
            {
                //movement
                if (Keyboard.current.wKey.isPressed)
                {
                    CameraController.Instance.CameraTablet.transform.position -= CameraController.Instance.CameraTablet.transform.forward * +freecamspeed;
                }
                if (Keyboard.current.aKey.isPressed)
                {
                    CameraController.Instance.CameraTablet.transform.position += CameraController.Instance.CameraTablet.transform.right * +freecamspeed;
                }
                if (Keyboard.current.sKey.isPressed)
                {
                    CameraController.Instance.CameraTablet.transform.position += CameraController.Instance.CameraTablet.transform.forward * +freecamspeed;
                }
                if (Keyboard.current.dKey.isPressed)
                {
                    CameraController.Instance.CameraTablet.transform.position -= CameraController.Instance.CameraTablet.transform.right * +freecamspeed;
                }
                if (Keyboard.current.qKey.isPressed)
                {
                    CameraController.Instance.CameraTablet.transform.position -= CameraController.Instance.CameraTablet.transform.up * +freecamspeed;
                }
                if (Keyboard.current.eKey.isPressed)
                {
                    CameraController.Instance.CameraTablet.transform.position += CameraController.Instance.CameraTablet.transform.up * +freecamspeed;
                }
                // arrow key rotation
                if (Keyboard.current.leftArrowKey.isPressed)
                {
                    CameraController.Instance.CameraTablet.transform.eulerAngles += new Vector3(0f, -freecamsens, 0f);
                }
                if (Keyboard.current.rightArrowKey.isPressed)
                {
                    CameraController.Instance.CameraTablet.transform.eulerAngles += new Vector3(0f, freecamsens, 0f);
                }
                if (Keyboard.current.upArrowKey.isPressed)
                {
                    CameraController.Instance.CameraTablet.transform.eulerAngles += new Vector3(freecamsens, 0f, 0f);
                }
                if (Keyboard.current.downArrowKey.isPressed)
                {
                    CameraController.Instance.CameraTablet.transform.eulerAngles += new Vector3(-freecamsens, 0f, 0f);
                }
            }
            if (freecam && controllerfreecam)
            {
                float x = InputManager.instance.GPLeftStick.x;
                float y = InputManager.instance.GPLeftStick.y;
                rotX += InputManager.instance.GPRightStick.x * freecamsens;
                rotY += InputManager.instance.GPRightStick.y * freecamsens;
                Vector3 movementdir = new Vector3(-x, posY, -y);
                CameraController.Instance.CameraTablet.transform.Translate(movementdir * freecamspeed);
                rotY = Mathf.Clamp(rotY, -90f, 90f);
                CameraController.Instance.CameraTablet.transform.rotation = Quaternion.Euler(rotY, rotX, 0);
                if (Gamepad.current.rightShoulder.isPressed)
                {
                    posY = 3f * +freecamspeed;
                }
                else if (Gamepad.current.leftShoulder.isPressed)
                {
                    posY = -3f * +freecamspeed;
                }
                else
                {
                    posY = 0;
                }
            }
        }

        void Spec()
        {
            if (followobject != null)
            {
                Vector3 targetPosition = followobject.transform.TransformPoint(specoffset);
                CameraController.Instance.CameraTablet.transform.position = Vector3.SmoothDamp(CameraController.Instance.CameraTablet.transform.position, targetPosition, ref velocity, 0.2f);
                if (speclookat)
                {
                    var targetRotation = Quaternion.LookRotation(followobject.transform.position - CameraController.Instance.CameraTablet.transform.position);
                    CameraController.Instance.CameraTablet.transform.rotation = Quaternion.Lerp(CameraController.Instance.CameraTablet.transform.rotation, targetRotation, 0.2f);
                }
                else
                {
                    CameraController.Instance.CameraTablet.transform.rotation = Quaternion.Lerp(CameraController.Instance.CameraTablet.transform.rotation, followobject.transform.rotation, 0.2f);
                }
                if (controloffset)
                {
                    if (Keyboard.current.wKey.isPressed) // forward
                    {
                        if (specoffset.z >= 3.01)
                        {
                            specoffset.z = 3;
                        }
                        specoffset.z += 0.02f;
                    }
                    if (Keyboard.current.aKey.isPressed) // left
                    {
                        if (specoffset.x <= -3.01)
                        {
                            specoffset.x = -3;
                        }
                        specoffset.x -= 0.02f;
                    }
                    if (Keyboard.current.sKey.isPressed) // back
                    {
                        if (specoffset.z <= -3.01)
                        {
                            specoffset.z = -3;
                        }
                        specoffset.z -= 0.02f;
                    }
                    if (Keyboard.current.dKey.isPressed) // right
                    {
                        if (specoffset.x >= 3.01)
                        {
                            specoffset.x = 3;
                        }
                        specoffset.x += 0.02f;
                    }
                    if (Keyboard.current.qKey.isPressed) // up 
                    {
                        if (specoffset.y <= -3.01)
                        {
                            specoffset.y = -3;
                        }
                        specoffset.y -= 0.02f;
                    }
                    if (Keyboard.current.eKey.isPressed) // down
                    {
                        if (specoffset.y >= 3.01)
                        {
                            specoffset.y = 3;
                        }
                        specoffset.y += 0.02f;
                    }
                }
            }
            else
            {
                if (spectating)
                {
                    CameraController.Instance.CameraTablet.transform.position = Player.Instance.headCollider.transform.position + Player.Instance.headCollider.transform.forward;
                    spectating = false;
                }
            }
        }
    }
}
