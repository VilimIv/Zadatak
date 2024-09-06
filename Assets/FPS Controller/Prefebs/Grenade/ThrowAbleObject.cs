using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowAbleObject : MonoBehaviour
{
    public float Speed = 10;
    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        rb.velocity = transform.forward * Speed;
    }
}