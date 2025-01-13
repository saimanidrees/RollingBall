using System.Collections;
using System.Collections.Generic;
using GameData.MyScripts;
using UnityEngine;

public class DropGlassDown : MonoBehaviour
{
    public int value;
    public BoxCollider sheild;
    public GameObject glass;
    public GameObject particles;
    Animator glassAnim;
    public bool triggered;
    private void Start()
    {
        sheild = GetComponent<BoxCollider>();
        glassAnim = GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == PlayerPrefsHandler.Player)
        {
            if (GamePlayManager.Instance.currentPlayer.GetComponent<PlayerBallMerge>().GetPlayerNumber() >= value)
            {
                Debug.Log("Detected");
                sheild.isTrigger = true;
               // particles.SetActive(true);
                triggered = true;
                ApplyRotation();
            }
            else
            {
                sheild.isTrigger = false;
                RepelBall(collision.gameObject);
            }
        }
    }

    public void ApplyRotation ()
    {
        glassAnim.SetTrigger("trigger");
    }
    public void RepelBall(GameObject obj)
    {
        Rigidbody rb = obj.GetComponentInParent<Rigidbody>();
        rb.mass = 2;
        rb.velocity = Vector2.zero;
        rb.AddRelativeForce(Vector3.back * 20f, ForceMode.Impulse);
        obj.GetComponentInParent<BallController>().StartMovement(false);
        GamePlayManager.Instance.GameComplete(1f);
    }
}
