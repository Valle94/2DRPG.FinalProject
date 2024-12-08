using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{

    [SerializeField] float roamChangeDirFloat = 2f;
    [SerializeField] float attackRange = 0f;
    [SerializeField] MonoBehaviour enemyType;
    [SerializeField] float attackCooldown = 2f;
    [SerializeField] bool stopMovingWhileAttacking = false;

    bool canAttack = true;
    
    // Create an enumerator which stores the 
    // possible states our enemy can be in.
    enum State 
    {
        Roaming,
        Attacking
    }

    State state;
    EnemyPathfinding enemyPathfinding;
    Vector2 roamPosition;
    float timeRoaming = 0f;

    //Initialize EnemyPathfinding script and starting state in Awake
    void Awake() 
    {
        enemyPathfinding = GetComponent<EnemyPathfinding>();
        state = State.Roaming;
    }


    void Start() 
    {
        roamPosition = GetRoamingPosition();
    }

    void Update()
    {
        MovementStateControl();
    }

    // This method contains our state control switch
    void MovementStateControl()
    {
        // The switch is basically a bunch of if-then statements
        // where we pass in our state, and do an action depending
        // on that state. 
        switch (state)
        {
            default:
            case State.Roaming:
                Roaming();
            break;

            case State.Attacking:
                Attacking();
            break;
        }
    }

    // This roaming method is executed every frame while State.Roaming.
    void Roaming()
    {
        // Increment time roaming
        timeRoaming += Time.deltaTime;
        // Move to randomly generated roamPosition
        enemyPathfinding.MoveTo(roamPosition);

        // If the player is close enough to the enemy, as dictated 
        // by the enemy's attack range, set State.Attacking
        if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) < attackRange)
        {
            state = State.Attacking;
        }

        // After some time, get a new position to roam to
        if (timeRoaming > roamChangeDirFloat)
        {
            roamPosition = GetRoamingPosition();
        }
    }

    // This roaming method is executed every frame while State.Attacking.
    void Attacking()
    {
        // If the player moves out of range of the enemy, set State.Roaming
        if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) > attackRange)
        {
            state = State.Roaming;
        }

        // This if statement is useful to melee range enemies
        // that don't have a built in attack range
        if (attackRange != 0 && canAttack)
        {
            canAttack = false;
            // Call the attack method from each specific enemy type.
            (enemyType as IEnemy).Attack();

            // This bit of logic just stops an enemy from moving if
            // we don't want them to be able to attack while moving
            if (stopMovingWhileAttacking)
            {
                enemyPathfinding.StopMoving();
            }
            else
            {
                enemyPathfinding.MoveTo(roamPosition);
            }

            StartCoroutine(AttackCooldownRoutine());
        }
    }

    IEnumerator AttackCooldownRoutine()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    // This method returns a random normalized vector2
    Vector2 GetRoamingPosition()
    {
        timeRoaming = 0f;
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }
}
