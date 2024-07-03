using UnityEngine;
using System.Collections;

public class BoomerangWeapon : MonoBehaviour
{
    public float throwSpeed = 10f;
    public float returnSpeed = 15f;
    public float maxDistance = 10f;
    public AnimationCurve throwCurve;
    public AnimationCurve returnCurve;

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private bool isReturning = false;
    private float distanceTraveled = 0f;
    private Transform enemyTransform;
    private Transform originalParent;

    public void ThrowBoomerang(Vector3 targetPos, Transform enemy)
    {
        startPosition = transform.position;
        targetPosition = targetPos;
        enemyTransform = enemy;
        isReturning = false;
        distanceTraveled = 0f;
        
        // Store the original parent and unparent the boomerang
        originalParent = transform.parent;
        transform.SetParent(null);

        gameObject.SetActive(true);
        StartCoroutine(MoveBoomerang());
    }

    private IEnumerator MoveBoomerang()
    {
        yield return new WaitForSeconds(.55f);
       
        while (!isReturning)
        {
            float step = throwSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
            distanceTraveled += step;

            float heightOffset = throwCurve.Evaluate(distanceTraveled / maxDistance) * 2f;
            transform.position += Vector3.up * heightOffset;

            transform.Rotate(Vector3.forward, 720f * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) < 0.1f || distanceTraveled >= maxDistance)
            {
                isReturning = true;
            }

            yield return null;
        }

        while (isReturning)
        {
            float step = returnSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, enemyTransform.position, step);
            distanceTraveled -= step;

            float heightOffset = returnCurve.Evaluate(1f - (distanceTraveled / maxDistance)) * 2f;
            transform.position += Vector3.up * heightOffset;

            transform.Rotate(Vector3.forward, -720f * Time.deltaTime);

            if (Vector3.Distance(transform.position, enemyTransform.position) < 0.1f)
            {
                // Reparent the boomerang to its original parent
                transform.SetParent(originalParent);
                transform.localPosition = Vector3.zero; // Reset local position
                transform.localRotation = Quaternion.identity; // Reset local rotation

                gameObject.SetActive(false);
                break;
            }

            yield return null;
        }
    }
}