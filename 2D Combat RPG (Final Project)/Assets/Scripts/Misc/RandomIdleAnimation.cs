using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomIdleAnimation : MonoBehaviour
{
    Animator myAnimator;

    void Awake()
    {
        myAnimator = GetComponent<Animator>();
    }

    void Start()
    {
        // This animator state info is basically populating a variable with info
        // about a particular component, in this case and animator, which can 
        // then be referenced or altered. 
        AnimatorStateInfo state = myAnimator.GetCurrentAnimatorStateInfo(0);
        // Here we're going to play our animation starting at a random time variable from 
        // 0 to 1 second, which will give an effect of randomly flickering torches
        myAnimator.Play(state.fullPathHash, -1, Random.Range(0f, 1f));
    }
}
