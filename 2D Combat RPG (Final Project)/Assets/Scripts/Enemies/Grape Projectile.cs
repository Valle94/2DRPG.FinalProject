using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapeProjectile : MonoBehaviour
{
    [SerializeField] float duration = 1f;
    [SerializeField] AnimationCurve animCurve;
    [SerializeField] float heightY = 3f;
    [SerializeField] GameObject grapeProjectileShadow;
    [SerializeField] GameObject splatterPrefab;

    void Start()
    {
        // Instantiate the grape shadow just below the grape
        GameObject grapeShadow = 
        Instantiate(grapeProjectileShadow, transform.position + new Vector3(0, -0.3f, 0), Quaternion.identity);

        Vector3 playerPos = PlayerController.Instance.transform.position;
        Vector3 grapeShadowStartPosition = grapeShadow.transform.position;

        StartCoroutine(ProjectileCurveRoutine(transform.position, playerPos));
        StartCoroutine(MoveGrapeShadowRoutine(grapeShadow, grapeShadowStartPosition, playerPos));
    }

    // This coroutine handles the grape projectile flying through the air in an arc
    IEnumerator ProjectileCurveRoutine(Vector3 startPosition, Vector3 endPosition)
    {
        float timePassed = 0f;

        while (timePassed < duration)
        {   
            //This block of code is just defining how 'high' we want our grape
            // to go, using an animation curve as a reference.
            timePassed += Time.deltaTime;
            float linearT = timePassed / duration;
            float heightT = animCurve.Evaluate(linearT);
            float height = Mathf.Lerp(0f, heightY, heightT);

            // Here we are moving our grape along a Lerp path at a rate linearT while
            // adding a height component as calculated above. Because this height
            // component is in the Y direction of a vector2, it will give the 
            // appearance of flying through the air with some height. 
            transform.position = Vector2.Lerp(startPosition, endPosition, linearT) + new Vector2(0f, height);

            yield return null;
        }

        // Instantiate our splatter prefab
        Instantiate(splatterPrefab, transform.position, Quaternion.identity);
        // When the projectile 'hits the ground,' destroy it
        Destroy(gameObject);
    }

    IEnumerator MoveGrapeShadowRoutine(GameObject grapeShadow, Vector3 startPosition, Vector3 endPosition)
    {
        float timePassed = 0f;

        while (timePassed < duration)
        {
            timePassed += Time.deltaTime;
            float linearT = timePassed / duration;

            grapeShadow.transform.position = Vector2.Lerp(startPosition, endPosition, linearT);

            yield return null;
        }

        Destroy(grapeShadow);
    }
}
