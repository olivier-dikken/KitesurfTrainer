using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRHandler : MonoBehaviour
{
    List<UnityEngine.XR.InputDevice> rightHandedControllers;
    List<UnityEngine.XR.InputDevice> leftHandedControllers;

    // Start is called before the first frame update
    void Start()
    {
        print("***** Attempt to access XR controllers *****");

        print("Listing all XR devices and roles:");

        var inputDevices = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevices(inputDevices);

        foreach (var device in inputDevices)
        {
            Debug.Log(string.Format("Device found with name '{0}' and role '{1}'", device.name, device.role.ToString()));
        }

        print("Getting left handed controller:");

        leftHandedControllers = new List<UnityEngine.XR.InputDevice>();
        var desiredCharacteristics_leftController = UnityEngine.XR.InputDeviceCharacteristics.HeldInHand | UnityEngine.XR.InputDeviceCharacteristics.Left | UnityEngine.XR.InputDeviceCharacteristics.Controller;
        UnityEngine.XR.InputDevices.GetDevicesWithCharacteristics(desiredCharacteristics_leftController, leftHandedControllers);

        foreach (var device in leftHandedControllers)
        {
            Debug.Log(string.Format("Device name '{0}' has characteristics '{1}'", device.name, device.characteristics.ToString()));
        }

        print("Getting right handed controller:");

        rightHandedControllers = new List<UnityEngine.XR.InputDevice>();
        var desiredCharacteristics_rightController = UnityEngine.XR.InputDeviceCharacteristics.HeldInHand | UnityEngine.XR.InputDeviceCharacteristics.Right | UnityEngine.XR.InputDeviceCharacteristics.Controller;
        UnityEngine.XR.InputDevices.GetDevicesWithCharacteristics(desiredCharacteristics_rightController, rightHandedControllers);

        foreach (var device in rightHandedControllers)
        {
            Debug.Log(string.Format("Device name '{0}' has characteristics '{1}'", device.name, device.characteristics.ToString()));
        }

    }

    // Update is called once per frame
    void Update()
    {
        bool triggerValueRight;
        foreach (var device in rightHandedControllers)
        {
            if (device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out triggerValueRight) && triggerValueRight)
            {
                Debug.Log("RightController Trigger button is pressed.");
            }
        }

        bool triggerValueLeft;
        foreach (var device in rightHandedControllers)
        {
            if (device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out triggerValueLeft) && triggerValueLeft)
            {
                Debug.Log("LeftController Trigger button is pressed.");
            }
        }

    }
}
