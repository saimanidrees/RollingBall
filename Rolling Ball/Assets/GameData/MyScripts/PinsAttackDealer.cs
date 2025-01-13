using System.Collections;
using System.Collections.Generic;
using GameData.MyScripts;
using UnityEngine;

public class PinsAttackDealer : MonoBehaviour
{
    public int value;
    public BoxCollider sheild;
    public List<GameObject> allPins;
    public GameObject particles;
    public bool scattered;
    private void Start()
    {
        sheild = GetComponent<BoxCollider>();
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == PlayerPrefsHandler.Player) {
            if (GamePlayManager.Instance.currentPlayer.GetComponent<PlayerBallMerge>().GetPlayerNumber() >= value)
            {
                Debug.Log("Detected");
                sheild.isTrigger = true;
                particles.SetActive(true);
                scattered = true;
                ApplyForceOnPins();
            }
            else
            {
                sheild.isTrigger = false;
               //RepelBall(collision.gameObject);
            }
        }
    }

    public void ApplyForceOnPins() {
        foreach (GameObject obj in allPins) {
            //obj.GetComponent<Rigidbody>().AddExplosionForce(20f, obj.transform.position+(Vector3.up*0.5f), 10f,0.1f, ForceMode.Impulse);
            //obj.GetComponent<BoxCollider>().isTrigger = true;
            obj.GetComponent<Rigidbody>().isKinematic = false;
        }
    }
    public void RepelBall(GameObject obj) {
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        rb.mass = 2;
        rb.velocity = Vector2.zero;
        rb.AddRelativeForce(Vector3.back * 20f, ForceMode.Impulse);
        obj.GetComponent<BallController>().StartMovement(false);
        GamePlayManager.Instance.GameComplete(1f);
    }
}
