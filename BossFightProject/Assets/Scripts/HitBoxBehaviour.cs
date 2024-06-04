using System;
using UnityEngine;
using UnityEngine.Events;

public class HitBoxBehaviour : MonoBehaviour
{
  public FloatData damage;
  public Vector3 knockback = new Vector3(0, 5, 5);
  private HurtBoxBehaviour hurtBox;
  public UnityEvent damageEvent;
  public LayerMask layerMask;

  private void OnTriggerEnter(Collider other)
  {
    Debug.Log("frick");
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
    }
  }
}
