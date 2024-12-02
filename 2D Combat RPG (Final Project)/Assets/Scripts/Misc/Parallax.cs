using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] float parallaxOffset = -0.15f;

    Camera cam;
    Vector2 startPos;
    // This 'travel' acts as a property which changes constantly. It
    // is calculating the amount the camera has moved from the object's
    // initial position.
    Vector2 travel => (Vector2)cam.transform.position - startPos;

    void Awake() 
    {
        cam = Camera.main;
    }

    void Start() 
    {
        startPos = transform.position;
    }

    // Here we are adjusting the position of our canopy layer based on the 
    // initial position of the canopy, the Vector2 which says how much the camera
    // has moved, and an offset amount which determines the 'strength' of the parallax
    void FixedUpdate() 
    {
        transform.position = startPos + travel * parallaxOffset;
    }
}
