using ColossalFramework.UI;
using UnityEngine;

namespace FPSCamera
{
    public class FPSCamera : MonoBehaviour
    {

        public delegate void OnCameraModeChanged(bool state);

        public static OnCameraModeChanged onCameraModeChanged;

        public static void Initialize()
        {
            var controller = GameObject.FindObjectOfType<CameraController>();
            instance = controller.gameObject.AddComponent<FPSCamera>();
            instance.controller = controller;
        }

        public static FPSCamera instance;

        public float cameraMoveSpeed = 128.0f;
        public float MoveSpeedModifier = 0;
        static float defaultFOV = Camera.main.fieldOfView;
        private bool fpsModeEnabled = false;
        private CameraController controller;
        float rotationY = 0f;
        bool setFOV = false;

        private static void ShowHideUI(bool show)
        {
        }

        public static void SetMode(bool fpsMode)
        {
            instance.fpsModeEnabled = fpsMode;
            ShowHideUI(!instance.fpsModeEnabled);

            if (instance.fpsModeEnabled)
            {
                instance.controller.enabled = false;
                Cursor.visible = false;
                instance.rotationY = -instance.transform.localEulerAngles.x;
            }
            else
            {
                instance.controller.enabled = true;
                Cursor.visible = true;
                Camera.main.fieldOfView = defaultFOV;
            }

            if (onCameraModeChanged != null)
            {
                onCameraModeChanged(fpsMode);
            }
        }

        public static bool IsEnabled()
        {
            return instance.fpsModeEnabled;
        }

        void Update()
        {
            if (fpsModeEnabled)
            {
                if(Input.GetKeyDown(KeyCode.Tab))
                {
                    SetMode(false);
                    return;
                }

                if (Input.GetKey(KeyCode.W))
                {
                    gameObject.transform.position += gameObject.transform.forward * cameraMoveSpeed * Time.deltaTime;
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    gameObject.transform.position -= gameObject.transform.forward * cameraMoveSpeed * Time.deltaTime;
                }

                if (Input.GetKey(KeyCode.A))
                {
                    gameObject.transform.position -= gameObject.transform.right * cameraMoveSpeed * Time.deltaTime;
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    gameObject.transform.position += gameObject.transform.right * cameraMoveSpeed * Time.deltaTime;
                }
                if (Input.GetKey(KeyCode.Q))
                {
                    gameObject.transform.position += Vector3.up * cameraMoveSpeed * Time.deltaTime;
                }
                else if (Input.GetKey(KeyCode.Z))
                {
                    gameObject.transform.position -= Vector3.up * cameraMoveSpeed * Time.deltaTime;
                }

/*              if (Input.GetKey(KeyCode.LeftShift))
                {
                    cameraMoveSpeed = 1024.0f;
                }
                else
                {
                    cameraMoveSpeed = 128.0f;
                }*/
                if (Input.GetAxis("Mouse ScrollWheel")!=0){
                    MoveSpeedModifier = (Mathf.Clamp(MoveSpeedModifier + (Input.GetAxis("Mouse ScrollWheel")*25), -500, 100));
                    if (MoveSpeedModifier <= -1)
                    {
                        cameraMoveSpeed = (128 / (System.Math.Abs(MoveSpeedModifier)));
                    }
                    if (MoveSpeedModifier == 0)
                    {
                        cameraMoveSpeed = (128 * (MoveSpeedModifier + 1));
                    }
                    if (MoveSpeedModifier >= 1)
                    {
                        cameraMoveSpeed = (128 * MoveSpeedModifier);
                    }
                }
                if (Input.GetKey(KeyCode.Mouse1))
                {
                    setFOV = true;
                }
                else
                {
                    setFOV = false;
                }
                if (Input.GetKey(KeyCode.R))
                {
                    Camera.main.fieldOfView = defaultFOV;
                }
                if (setFOV == true)
                {
                    Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView - Input.GetAxis("Mouse Y"), 5, 170);
//                  Camera.main.fieldOfView -= Input.GetAxis("Mouse Y");
                    /*if (Camera.main.fieldOfView <= 5)
                    {
                        Camera.main.fieldOfView = 5;
                    }
                    else if (Camera.main.fieldOfView >= 170)
                    {
                        Camera.main.fieldOfView = 170;
                    }*/
                    
                }
                else
                {
                    float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X");
//                  rotationY += Input.GetAxis("Mouse Y");
                    rotationY = Mathf.Clamp(rotationY + Input.GetAxis("Mouse Y"), -90, 90);
                    transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
                    
                }
                /*if (rotationY >= 90)
                {
                    rotationY = 90;
                }
                else if(rotationY <= -90)
                {
                    rotationY = -90;
                }*/

            }
        }

    }

}
