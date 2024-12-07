using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaExit : MonoBehaviour
{
    [SerializeField] string sceneToLoad;
    [SerializeField] string sceneTransitionName;

    float waitToLoadTime = 1f;

    // This collider is placed at the exit of a scene. When the player
    // triggers the collider, we want to load the corresponding scene and
    // set our transition name to the name of the exit we walked through.
    void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.GetComponent<PlayerController>())
        {
            SceneManagement.Instance.SetTransitionName(sceneTransitionName);

            StartCoroutine(LoadSceneRoutine());
            // Call Fade to black function when leaving an area
            UIFade.Instance.FadeToBlack();
        }
    }

    IEnumerator LoadSceneRoutine()
    {
        yield return new WaitForSeconds(waitToLoadTime);
        SceneManager.LoadScene(sceneToLoad);
    }
}
