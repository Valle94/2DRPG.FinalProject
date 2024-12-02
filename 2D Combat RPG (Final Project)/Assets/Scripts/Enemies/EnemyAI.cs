using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{

    [SerializeField] float roamChangeDirFloat = 2f;
    
    // Create an enumerator which stores the 
    // possible states our enemy can be in.
    enum State 
    {
        Roaming
    }

    State state;
    EnemyPathfinding enemyPathfinding;

    //Initialize EnemyPathfinding script and starting state in Awake
    void Awake() 
    {
        enemyPathfinding = GetComponent<EnemyPathfinding>();
        state = State.Roaming;
    }


    void Start() 
    {
        StartCoroutine(RoamingRoutine()); //Begin roaming coroutine
    }

    // This coroutine gets a random vector2 position using GetRoamingPosition()
    // and passes that into the MoveTo method from our pathfinding 
    // script, which moves the enemy. It then waits a number of seconds 
    // before repeating.
    IEnumerator RoamingRoutine()
    {
        // The coroutine will only execute while
        // enemy is in the Roaming state.
        while (state == State.Roaming)
        {
            Vector2 roamPosition = GetRoamingPosition();
            enemyPathfinding.MoveTo(roamPosition);
            yield return new WaitForSeconds(roamChangeDirFloat);
        }
    }

    // This method returns a random normalized vector2
    Vector2 GetRoamingPosition()
    {
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }
}
