using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEntrance : MonoBehaviour
{
    [SerializeField] string transitionName;

    // This transform is placed into a scene where we want the player to 
    // spawn into when coming from a different scene
    void Start()
    {
        // If our transition names match, place the player at the specified transform
        // We're also setting our camera to follow the created player instance
        if (transitionName == SceneManagement.Instance.SceneTransitionName)
        {
            PlayerController.Instance.transform.position = this.transform.position;
            CameraController.Instance.SetPlayerCameraFollow();

            // Call Fade to clear function when entering an area
            UIFade.Instance.FadeToClear();
        }
    }
}
