using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class EnemyStateMachine : MonoBehaviour
{
    public enum EnemyState
    {
        Searching,
        Approaching,
        Attacking,
        Waiting,
        RangedAttack
    }

    public Transform playerTransform;
    public float attackRange = 5f;
    public float moveSpeed = 3f;
    public float attackCooldown = 3f;
    public float detectionRange = 20f;

    public UnityEvent[] attackEvents, rangedAttackEvents;

    private EnemyState currentState;
    public BoomerangWeapon snare;

    void Start()
    {
        if (playerTransform == null)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }
        currentState = EnemyState.Searching;
        StartCoroutine(StateMachine());
    }

    IEnumerator StateMachine()
    {
        while (true)
        {
            yield return StartCoroutine(currentState.ToString());
        }
    }

    IEnumerator Searching()
    {
        Debug.Log("Searching for player");
        float distanceToPlayer = Vector3.Distance(playerTransform.position, transform.position);
        
        if (distanceToPlayer <= attackRange)
        {
            currentState = EnemyState.Attacking;
        }
        else if (distanceToPlayer <= detectionRange)
        {
            currentState = EnemyState.Approaching;
        }
        else
        {
            currentState = EnemyState.RangedAttack;
        }
        yield return null;
    }


    IEnumerator Approaching()
    {
        Debug.Log("Approaching player");
        Vector3 playerPosition = playerTransform.position;
        Vector3 enemyPosition = transform.position;
        
        
        playerPosition.y = enemyPosition.y;
        
        Vector3 direction = (playerPosition - enemyPosition).normalized;
        transform.Translate(direction * moveSpeed * Time.deltaTime);
        currentState = EnemyState.Searching;
        yield return null;
    }

    IEnumerator Attacking()
    {
        Debug.Log("Attacking player");
        int randomAttack = Random.Range(0, attackEvents.Length);
        attackEvents[randomAttack].Invoke();
        currentState = EnemyState.Waiting;
        yield return null;
    }
    
    IEnumerator RangedAttack()
    {
        Debug.Log("Attacking player");
        int randomRangedAttack = Random.Range(0, rangedAttackEvents.Length);
        rangedAttackEvents[randomRangedAttack].Invoke();
        if (snare != null && playerTransform != null)
        {
            snare.ThrowBoomerang(playerTransform.position, this.transform);
        }
        currentState = EnemyState.Waiting;
        yield return null;
    }

    IEnumerator Waiting()
    {
        Debug.Log("Waiting after attack");
        yield return new WaitForSeconds(attackCooldown);
        currentState = EnemyState.Searching;
    }
}