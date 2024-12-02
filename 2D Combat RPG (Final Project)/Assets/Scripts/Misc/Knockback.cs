using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    // This way of declaring a variable lets us set it as public
    // which allows us to get it in other classes, but keep the 
    // ability to set change it to private
    public bool gettingKnockedBack { get; private set;}

    [SerializeField] float knockBackTime = 0.2f;

    Rigidbody2D rb;

    void Awake() 
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // This method will handle the transformations on our RigidBody2D when
    // struck by a damage source with a specific knockback amount.
    public void GetKnockedBack(Transform damageSource, float knockBackThrust)
    {
        gettingKnockedBack = true;
        Vector2 difference = (transform.position - damageSource.position).normalized * knockBackThrust * rb.mass;
        rb.AddForce(difference, ForceMode2D.Impulse);
        StartCoroutine(KnockRoutine());
    }

    // This coroutine used above determines how long the enemy will be 
    // 'knocked back' for. Because we're using a constant force application
    // instead of a physics application, we need to stop applying the force
    // eventually. This coroutine does that, and resets the bool to false. 
    IEnumerator KnockRoutine()
    {
        yield return new WaitForSeconds(knockBackTime);
        rb.velocity = Vector2.zero;
        gettingKnockedBack = false;
    }
}
