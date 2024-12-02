using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flash : MonoBehaviour
{
    [SerializeField] Material whiteFlashMat;
    [SerializeField] float restoreDefaultMatTime = .2f;

    Material defaultMat;
    SpriteRenderer spriteRenderer;

    void Awake() 
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultMat = spriteRenderer.material;
    }

    // Getter method for using the restore material time other places
    public float GetRestoreMatTime()
    {
        return restoreDefaultMatTime;
    }

    // Public coroutine for changing the sprite material from the default
    // to the white 'damage flash' for some time, then changing it back
    public IEnumerator FlashRoutine()
    {
        spriteRenderer.material = whiteFlashMat;
        yield return new WaitForSeconds(restoreDefaultMatTime);
        spriteRenderer.material = defaultMat;
    }
}
