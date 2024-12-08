using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : Singleton<CameraController>
{
    CinemachineVirtualCamera cinemachineVirtualCamera;

    void Start()
    {
        SetPlayerCameraFollow();
    }

    // This method is grabbing our cinemachine virtual camera component
    // and changing the 'follow' property to follow the player's transform. 
    public void SetPlayerCameraFollow()
    {
        cinemachineVirtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        cinemachineVirtualCamera.Follow = PlayerController.Instance.transform;
    }
}
