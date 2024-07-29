using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class HitBoxBehaviour : MonoBehaviour
{
  public FloatData damage;
  public Vector3 knockback = new Vector3(0, 5, 5);
  private HurtBoxBehaviour hurtBox;
  public UnityEvent damageEvent;
  public LayerMask layerMask;
  public Collider2D hitboxCollider;
  public FloatData activationDelay;
  public FloatData activeTime;

  private WaitForSeconds waitTime = new(3f);
  private Coroutine waitCoroutine;

  public void Start()
  {
    if (hitboxCollider != null)
    {
      hitboxCollider.enabled = false;
    }
  }
  
  public void ActivateHitbox()
  {
    if (hitboxCollider != null)
    {
      StartCoroutine(HitboxActivationRoutine());
    }
    
  }
  
  private IEnumerator HitboxActivationRoutine()
  {
    yield return new WaitForSeconds(activationDelay.data);

    hitboxCollider.enabled = true;
    
    yield return new WaitForSeconds(activeTime.data);

    hitboxCollider.enabled = false;
  }


  private void OnTriggerEnter2D(Collider2D other)
  {
    hurtBox = other.GetComponent<HurtBoxBehaviour>(); 
    if (layerMask == (layerMask | (1 << other.transform.gameObject.layer)))
    {
      if (hurtBox != null)
      {
        damageEvent.Invoke();
        DoDamage();
      }
    }
  }

  public void DoDamage()
  {
    if (hurtBox != null) 
    {
      hurtBox.health.data -= damage.data;
      waitCoroutine ??= StartCoroutine(WaitAfterDamage());
    }
  }

  private IEnumerator WaitAfterDamage()
  {
    yield return waitTime;
    waitCoroutine = null;
  }
  
}

