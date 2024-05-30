using System;
using UnityEngine;

public class HitBoxBehaviour : MonoBehaviour
{
  public FloatData damage;
  public Vector3 knockback = new Vector3(0, 5, 5);

  public LayerMask layerMask;

  private void OnTriggerEnter(Collider other)
  {
    HurtBoxBehaviour h = other.GetComponent<HurtBoxBehaviour>();
    Debug.Log("frick");

    if (layerMask == (layerMask | (1 << other.transform.gameObject.layer)))
    {
      if (h != null)
      {
        h.health.data -= damage.data;
      }
    }
  }
}
