using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class EnemyStateMachine : MonoBehaviour
{
    public enum EnemyState
    {
        Searching,
        Approaching,
        Attacking,
        Waiting,
        RangedAttack,
        FazeTrans
    }

    public Transform playerTransform;
    public float attackRange = 6f;
    public float moveSpeed = 5f;
    public float attackCooldown = 1f;
    public float detectionRange = 22f;
    public FloatData health;
    private float speedIndicator = 1f;

    public bool isFirstFaze = true;

    public UnityEvent[] attackEvents, rangedAttackEvents;
    public UnityEvent transitionEvent;

    private EnemyState currentState;
    public BoomerangWeapon snare;
    public FacePlayer facePlayer;

    public float jumpHeight = 5f;
    public float jumpDuration = 1f;
    public float slamDuration = 0.3f;
    public AnimationCurve jumpCurve;
    public AnimationCurve slamCurve;

    private Vector3 jumpTargetPosition;

    void Start()
    {
        if (playerTransform == null)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }
        facePlayer = GetComponent<FacePlayer>();
        currentState = EnemyState.Searching;
        StartCoroutine(StateMachine());
        
    }

    public void ChangeWaitTime(FloatData obj)
    {
        attackCooldown = obj.data;
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
        if (isFirstFaze)
        {
            speedIndicator = 0.7f;

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
        }
        else
        {
            speedIndicator = .5f;
            
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
        }

        yield return null;
    }


    IEnumerator Approaching()
    {
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
        facePlayer.EnableTracking(false);
        int randomAttack = Random.Range(0, attackEvents.Length);
        attackEvents[randomAttack].Invoke();
        currentState = EnemyState.Waiting;
        yield return null;
    }
    
    IEnumerator RangedAttack()
    {
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
        yield return new WaitForSeconds((attackCooldown) * speedIndicator) ;
        facePlayer.EnableTracking(true);
        currentState = EnemyState.Searching;
        
        if (health.data <= 100 && isFirstFaze)
        { 
            isFirstFaze = false;
            currentState = EnemyState.FazeTrans;
        }
    }
    
    public void TriggerJumpAttack()
    {
        StartCoroutine(JumpAttackCoroutine());
    }

    private IEnumerator JumpAttackCoroutine()
    {
        Vector3 startPosition = transform.position;
        float groundLevel = startPosition.y;

        yield return new WaitForSeconds(.30f);

        // Find player position at the start
        Vector3 playerPosition = playerTransform != null ? playerTransform.position : startPosition;
        Vector3 targetPosition = new Vector3(playerPosition.x, groundLevel, startPosition.z);

        Vector3 highestPoint = startPosition + Vector3.up * jumpHeight;

        // Jump up
        float elapsedTime = 0f;
        while (elapsedTime < jumpDuration)
        {
            float t = elapsedTime / jumpDuration;
            float heightProgress = jumpCurve.Evaluate(t);
            float xProgress = Mathf.SmoothStep(0, 1, t); // Smooth horizontal movement
        
            float currentX = Mathf.Lerp(startPosition.x, targetPosition.x, xProgress);
            float currentY = Mathf.Lerp(startPosition.y, highestPoint.y, heightProgress);
        
            transform.position = new Vector3(currentX, currentY, startPosition.z);
        
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Short pause at the top
        yield return new WaitForSeconds(0.2f);

        // Slam down
        elapsedTime = 0f;
        while (elapsedTime < slamDuration)
        {
            float t = elapsedTime / slamDuration;
            float slamProgress = slamCurve.Evaluate(t);
        
            float currentY = Mathf.Lerp(highestPoint.y, groundLevel, slamProgress);
        
            transform.position = new Vector3(targetPosition.x, currentY, startPosition.z);
        
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure final position is exactly the target
        transform.position = targetPosition;
    }

    IEnumerator FazeTrans()
    {
        facePlayer.EnableTracking(false);
       
        transitionEvent.Invoke();
        
        yield return new WaitForSeconds(3f);
       
        currentState = EnemyState.Waiting;
        
        yield return null;
    }
}