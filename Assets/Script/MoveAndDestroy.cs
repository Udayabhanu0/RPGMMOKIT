using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAndDestroy : MonoBehaviour
{
      public float speed = 10f;
    public Rigidbody rb;

    void Start()
    {
        
    }

    void Update()
    {
        rb.velocity = transform.forward * speed;
    }

    void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
