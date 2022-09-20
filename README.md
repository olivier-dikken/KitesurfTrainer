# KitesurfTrainer
VR kitesurf trainer "serious game"/education game

## Setup

##### Unity version
2021.3.5f1

##### How to setup VR 
Depending on your VR HMD device, follow the relevant steps: 
https://learn.unity.com/tutorial/0-1-set-up-unity-and-your-vr-device-1?uv=2021.3&courseId=60183276edbc2a2e6c4c7dae#628f989cedbc2a39c0405f4f

In Unity Editor, add packages:
- Open XR Plugin
- OpenVR XR Plugin (only needed for HMD compatible with steamVR)

In Unity Editor, go to Edit > Project Settings > XR Plug-in Management
Then make sure the correct plug-in providers are selected. 
For example for steamVR compatible devices select OpenVR Loader and OpenXR, then in the settings menus  XR Plug-in Management > OpenVR and  XR Plug-in Management > OpenXR, make sure the 'stereo rendering mode' is set to 'multi pass'

## How to setup a new scene

In Assets > KiteGame > Prefabs 
There is a [CameraRig] prefab that needs to be added to the scene for VR rendering.
