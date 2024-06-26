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
        Waiting
    }

    public Transform playerTransform;
    public float attackRange = 3f;
    public float moveSpeed = 3f;
    public float attackCooldown = 3f;

    public UnityEvent[] attackEvents;

    private EnemyState currentState;

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
        if (Vector3.Distance( playerTransform.position, transform.position) <= attackRange)
        {
            currentState = EnemyState.Attacking;
        }
        else
        {
            currentState = EnemyState.Approaching;
        }
        yield return null;
    }

    IEnumerator Approaching()
    {
        Debug.Log("Approaching player");
        Vector3 direction = (playerTransform.position - transform.position).normalized;
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

    IEnumerator Waiting()
    {
        Debug.Log("Waiting after attack");
        yield return new WaitForSeconds(attackCooldown);
        currentState = EnemyState.Searching;
    }
}