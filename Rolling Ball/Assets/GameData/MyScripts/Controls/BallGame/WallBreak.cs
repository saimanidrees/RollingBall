using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WallBreak : MonoBehaviour
{
    public float value;
    public char alphabet;
    public TextMeshPro textOnWall;
    public GameObject glassPieces;
    public GameObject GreenGradient;
    private void Start()
    {
        SetValues(value + alphabet.ToString());
    }
    public void SetValues(string txt) {
        textOnWall.text = txt.ToString();
    }
   /* private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag(PlayerPrefsHandler.Player)) {
            return;
        }
        // ball  charachter checked first 
        if (collision.gameObject.GetComponent<BallModifier>().character>alphabet) {

            // break the wall
        }
        if (collision.gameObject.GetComponent<BallModifier>().character == alphabet)
        {
            if (collision.gameObject.GetComponent<BallModifier>().mainValue >= value) {
                // break the wall
            }
        }
    }*/
    public void BreakWall()
    {
        glassPieces.SetActive(true);
        textOnWall.gameObject.SetActive(false);
        GreenGradient.SetActive(false);
        Rigidbody[] rbs = glassPieces.GetComponentsInChildren<Rigidbody>();
        var forceDirection = Vector3.up;
        foreach(Rigidbody rb in rbs)
        {
            switch (rb.name)
            {
                case  "Up":
                    forceDirection = Vector3.up;
                    break;
                case  "Down":
                    forceDirection = Vector3.down;
                    break;
                case  "Right":
                    forceDirection = Vector3.right;
                    break;
                case  "Left":
                    forceDirection = Vector3.left;
                    break;
                case "Forward":
                    forceDirection = Vector3.forward;
                    break;
                case  "RightUp":
                    forceDirection = new Vector3(1f, 1f, 0);
                    break;
                case  "LeftUp":
                    forceDirection = new Vector3(-1f, 1f, 0);
                    break;
            }
            rb.AddForce(forceDirection * 15f, ForceMode.Impulse);
        }
    }
    public void RepelFromWall()
    {
        GetComponent<BallController>().StartMovement(false);
        gameObject.GetComponent<Rigidbody>().AddForce(Vector3.back * 10f, ForceMode.Impulse);
    }
}