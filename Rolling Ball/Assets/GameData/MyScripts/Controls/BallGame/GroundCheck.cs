using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public LayerMask groundLayer;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer.Equals(3))
        {
            Debug.Log("Ground Check");
        }
        else {

            Debug.Log("Ground nai milla"+ other.gameObject.name+"  "+ groundLayer.value);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer.Equals(3))
        {
            gameObject.transform.SetParent(null);
            Debug.Log("Fail Level Here");
        }
    }
}
