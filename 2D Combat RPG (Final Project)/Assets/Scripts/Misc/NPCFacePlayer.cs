using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCFacePlayer : MonoBehaviour
{
    SpriteRenderer mySpriteRenderer;

    void Awake()
    {
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate() 
    {
        AdjustNPCFacingDirection();
    }

    void AdjustNPCFacingDirection()
    {
        Vector3 playerPos = PlayerController.Instance.transform.position;

        if (playerPos.x < transform.position.x)
        {
            mySpriteRenderer.flipX = true;
        }
        else
        {
            mySpriteRenderer.flipX = false;
        }
    }
}