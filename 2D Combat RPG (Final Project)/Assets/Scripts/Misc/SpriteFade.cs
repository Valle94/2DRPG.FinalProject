using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpriteFade : MonoBehaviour
{
    [SerializeField] float fadeTime = 0.4f;

    SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // This routine is almost identical to the routine used to fade
    // trees and the canopy, only in this case it is attached to an
    // an instantiated object, our laser, which needs to be destroyed
    // after the fade is finished. 
    public IEnumerator SlowFadeRoutine()
    {
        float elapsedTime = 0;
        float startValue = spriteRenderer.color.a;

        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startValue, 0f, elapsedTime / fadeTime);
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g,spriteRenderer.color.b, newAlpha);
            yield return null;
        }

        Destroy(gameObject);
    }
}
