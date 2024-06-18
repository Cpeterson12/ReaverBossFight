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

  private WaitForSeconds waitTime = new(3f);
  private Coroutine waitCoroutine;

  
  
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

  private void DoDamage()
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

