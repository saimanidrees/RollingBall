using System;
using System.Collections;
using System.Collections.Generic;
using GameData.MyScripts;
using UnityEngine;

public class BlocksScatter : MonoBehaviour
{
    Rigidbody rb;
   
    private void Start()
    {
   
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(PlayerPrefsHandler.Player))
        {
            rb.isKinematic = false;
            rb.AddForce(Vector3.forward * 10f, ForceMode.Impulse);
          
        }
    }
}
