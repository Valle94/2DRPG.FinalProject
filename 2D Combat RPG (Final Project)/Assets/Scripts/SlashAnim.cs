using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashAnim : MonoBehaviour
{
    // This method is called in the sword slash animator. After the last
    // frame of the animation, it calls this function, which simply
    // destroys the instantiated animation.
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
