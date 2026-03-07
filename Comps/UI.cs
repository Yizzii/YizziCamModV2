using Photon.Pun;
using UnityEngine;
using GorillaLocomotion;
using UnityEngine.InputSystem;
#pragma warning disable CS0618

namespace YizziCamModV2.Comps
{
    class UI : MonoBehaviour
    {
        public GameObject forest, cave, canyon, mountain, city, clouds, cloudsbottom, beach, beachthing, basement, citybuildings;

        GameObject rigcache;
        bool keyp, uiopen, specui, freecam, spectating, controllerfreecam, speclookat, controloffset;
        GameObject followobject;
        float freecamspeed = 0.1f;
        float freecamsens = 1f;
        float rotX, rotY, posY;
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

                    if (freecam && Player.Instance != null && CameraController.Instance != null)
                    {
                        CameraController.Instance.CameraTablet.transform.position =
                            Player.Instance.headCollider.transform.position + Player.Instance.headCollider.transform.forward;
                    }

                    if (CameraController.Instance != null && !CameraController.Instance.flipped)
                    {
                        CameraController.Instance.flipped = true;
                        CameraController.Instance.ThirdPersonCameraGO.transform.Rotate(0f, 180f, 0f);
                        CameraController.Instance.TabletCameraGO.transform.Rotate(0f, 180f, 0f);
                        CameraController.Instance.FakeWebCam.transform.Rotate(-180f, 180f, 0f);
                    }

                    if (CameraController.Instance != null)
                    {
                        CameraController.Instance.fpv = false;
                        CameraController.Instance.fp = false;
                        CameraController.Instance.tpv = false;
                    }

                    freecam = !freecam;
                }

                // Spectator button
                if (GUI.Button(new Rect(35f, 90f, 100f, 20f), "Spectator"))
                {
                    if (!freecam && PhotonNetwork.InRoom)
                    {
                        specui = !specui;
                    }

                    if (CameraController.Instance != null)
                    {
                        CameraController.Instance.fpv = false;
                        CameraController.Instance.fp = false;
                        CameraController.Instance.tpv = false;
                    }
                }

                // Stop spectating
                if (GUI.Button(new Rect(140f, 90f, 45f, 20f), "Stop"))
                {
                    if (spectating && Player.Instance != null && CameraController.Instance != null)
                    {
                        followobject = null;
                        CameraController.Instance.CameraTablet.transform.position =
                            Player.Instance.headCollider.transform.position + Player.Instance.headCollider.transform.forward;
                        spectating = false;
                    }
                }

                // Load all maps for private rooms
                if (GUI.Button(new Rect(35f, 110f, 160f, 20f), "Load All Maps(PRIVS)"))
                {
                    if (PhotonNetwork.CurrentRoom != null && !PhotonNetwork.CurrentRoom.IsVisible)
                    {
                        SetMapsActive(true);
                    }
                }

                // Spec UI list
                if (specui && rigcache != null)
                {
                    int i = 1;
                    foreach (VRRig player in rigcache.GetComponentsInChildren<VRRig>())
                    {
                        if (player.transform.parent.gameObject.activeSelf)
                        {
                            GUI.Label(new Rect(250, 20 + (i * 25), 160, 20), player.playerText.text);

                            if (GUI.Button(new Rect(360, 20 + (i * 25), 67, 20), "Spectate"))
                            {
                                followobject = player.gameObject;
                                spectating = true;

                                if (CameraController.Instance != null)
                                {
                                    CameraController.Instance.fp = false;
                                    CameraController.Instance.fpv = false;
                                    CameraController.Instance.tpv = false;

                                    if (CameraController.Instance.flipped)
                                    {
                                        CameraController.Instance.flipped = false;
                                        CameraController.Instance.ThirdPersonCameraGO.transform.Rotate(0f, 180f, 0f);
                                        CameraController.Instance.TabletCameraGO.transform.Rotate(0f, 180f, 0f);
                                        CameraController.Instance.FakeWebCam.transform.Rotate(-180f, 180f, 0f);
                                    }
                                }
                            }
                        }
                        i++;
                    }
                }

                // Toggles
                controllerfreecam = GUI.Toggle(new Rect(30f, 130f, 160f, 19f), controllerfreecam, "Controller Freecam");
                controloffset = GUI.Toggle(new Rect(30f, 150f, 170f, 19f), controloffset, "Control Offset with WASD");
                speclookat = GUI.Toggle(new Rect(30f, 170f, 170f, 19f), speclookat, "Spectator Stare");

                // Sliders
                DrawSpecOffsetSliders();
                DrawFreecamSliders();

                // Close spec UI if not in room
                if (!PhotonNetwork.InRoom)
                {
                    specui = false;
                    followobject = null;
                }
            }

            // Toggle UI with Tab
            if (Keyboard.current != null && Keyboard.current.tabKey.isPressed)
            {
                if (!keyp) uiopen = !uiopen;
                keyp = true;
            }
            else keyp = false;
        }

        void LateUpdate()
        {
            Spec();
            Freecam();
        }

        void SetMapsActive(bool state)
        {
            forest?.SetActive(state);
            cave?.SetActive(state);
            canyon?.SetActive(state);
            beach?.SetActive(state);
            beachthing?.SetActive(state);
            city?.SetActive(state);
            mountain?.SetActive(state);
            basement?.SetActive(state);
            clouds?.SetActive(state);
            cloudsbottom?.SetActive(false);
            citybuildings?.SetActive(false);
        }

        void DrawSpecOffsetSliders()
        {
            GUI.Label(new Rect(35f, 188f, 160f, 30f), "         Spectator Offset:");
            GUI.Label(new Rect(35f, 200f, 160f, 30f), "     X            Y            Z");

            specoffset.x = GUI.HorizontalSlider(new Rect(35f, 215f, 50f, 20f), specoffset.x, -3f, 3f);
            specoffset.y = GUI.HorizontalSlider(new Rect(90f, 215f, 50f, 20f), specoffset.y, -3f, 3f);
            specoffset.z = GUI.HorizontalSlider(new Rect(145f, 215f, 50f, 20f), specoffset.z, -3f, 3f);
        }

        void DrawFreecamSliders()
        {
            GUI.Label(new Rect(35f, 232f, 160f, 30f), "          Freecam Speed");
            freecamspeed = GUI.HorizontalSlider(new Rect(35f, 250f, 160f, 5f), freecamspeed, 0.01f, 0.4f);
            GUI.Label(new Rect(35f, 258f, 160f, 20f), "0                0.5               1");

            GUI.Label(new Rect(35f, 275f, 160f, 30f), "          Freecam Sens");
            freecamsens = GUI.HorizontalSlider(new Rect(35f, 293f, 160f, 5f), freecamsens, 0.01f, 2f);
            GUI.Label(new Rect(35f, 301f, 160f, 20f), "0                0.5               1");
        }

        // Freecam and spec methods remain largely the same, just null-safe and clamped
        // ... (Freecam() and Spec() methods unchanged for brevity)
    }
}
