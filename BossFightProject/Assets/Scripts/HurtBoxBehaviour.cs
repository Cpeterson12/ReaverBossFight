using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtBoxBehaviour : MonoBehaviour
{
  public FloatData health;
  public Rigidbody2D rb;

  public void Awake()
  {
    health.data = 200;
  }
}
