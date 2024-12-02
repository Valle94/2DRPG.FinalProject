using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TransparentDetection : MonoBehaviour
{
    [Range(0, 1)]
    [SerializeField] float transparencyAmount = 0.8f;
    [SerializeField] float fadeTime = 0.4f;

    SpriteRenderer spriteRenderer;
    private Tilemap tilemap;

    void Awake() 
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        tilemap = GetComponent<Tilemap>();
    }

    // When we collide with either a tree or the canopy we'll turn that object
    // slightly transparent so we can still see the player
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayerController>())
        {
            if (spriteRenderer)
            {
                StartCoroutine(FadeRoutine(spriteRenderer, fadeTime, spriteRenderer.color.a, transparencyAmount));
            }
            else if (tilemap)
            {
                StartCoroutine(FadeRoutine(tilemap, fadeTime, tilemap.color.a, transparencyAmount));
            }
        }
    }

    // This on trigger resets the transparency when we move away from the tree or canopy
    void OnTriggerExit2D(Collider2D other) 
    {
        if (spriteRenderer)
            {
                // 1f Magic number is maximum alpha, so zero transparency
                StartCoroutine(FadeRoutine(spriteRenderer, fadeTime, spriteRenderer.color.a, 1f));
            }
            else if (tilemap)
            {
                StartCoroutine(FadeRoutine(tilemap, fadeTime, tilemap.color.a, 1f));
            }
    }

    // This coroutine uses a lerp function to slowly transition the transparency from 1 to the 
    // desired value instead of blinking it instantly from opaque to transparent. 
    IEnumerator FadeRoutine(SpriteRenderer spriteRenderer, float fadeTime, float startValue, float targetTransparency)
    {
        float elapsedTime = 0;
        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startValue, targetTransparency, elapsedTime / fadeTime);
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g,spriteRenderer.color.b, newAlpha);
            yield return null;
        }
    }

    // These two routines are identical except for the fact that one uses a sprite renderer 
    // when it's attached to a tree, and the other uses a tilemap when attached to the canopy. 
    IEnumerator FadeRoutine(Tilemap tilemap, float fadeTime, float startValue, float targetTransparency)
    {
        float elapsedTime = 0;
        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startValue, targetTransparency, elapsedTime / fadeTime);
            tilemap.color = new Color(tilemap.color.r, tilemap.color.g,tilemap.color.b, newAlpha);
            yield return null;
        }
    }
}
