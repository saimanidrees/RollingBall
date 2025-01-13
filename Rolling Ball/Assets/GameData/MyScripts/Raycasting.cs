using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycasting : MonoBehaviour
{
    [SerializeField] private LayerMask layersToDetect;
    private void FixedUpdate()
    {
        if(Input.GetMouseButtonDown(0)) {

            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log(1 << LayerMask.NameToLayer("Enemy"));
            var hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity,1 << LayerMask.NameToLayer("Enemy"));

            if(hit) {
                // Raycast hit something
                Debug.Log(hit.collider.name);
            }

        }
    }
}
