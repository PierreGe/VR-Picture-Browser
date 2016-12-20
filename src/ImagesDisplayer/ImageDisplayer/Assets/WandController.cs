using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandController : MonoBehaviour {

    // Controller

    private Valve.VR.EVRButtonId gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;
    private Valve.VR.EVRButtonId upButton = Valve.VR.EVRButtonId.k_EButton_DPad_Up;
    private Valve.VR.EVRButtonId downButton = Valve.VR.EVRButtonId.k_EButton_DPad_Down;
    private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;

    private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }
    private SteamVR_TrackedObject trackedObj;


    // Use this for initialization
    void Start () {

        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (controller == null)
        {
            Debug.Log("Controller not initialized");
            return;
        }
        if (controller.GetPressDown(triggerButton))
        {
            GameObject selected = Main.scriptInstance.currentSelected;
            if (selected)
                Main.scriptInstance.currentSelected.transform.Rotate(new Vector3(0, 90, 0));
        }
        if (controller.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
        {
            print("Touchpad");
            Vector2 touchpad = (controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0));

            if (touchpad.y > 0.7f)
            {
                print("Moving Up");
                GameObject.Find("CameraParent").transform.Translate(new Vector3(0, (float)0.3, 0));
            }

            else if (touchpad.y < -0.7f)
            {
                print("Moving Down");
                GameObject.Find("CameraParent").transform.Translate(new Vector3(0, (float)-0.3, 0));
            }
        }
    }
}
