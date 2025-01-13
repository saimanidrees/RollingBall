using System.Collections;
using System.Collections.Generic;
using GameData.MyScripts;
using UnityEngine;

public class DestructableCubePiece : MonoBehaviour
{
    public int value;
    public BoxCollider sheild;
    public List<GameObject> allBoxes;
    public GameObject particles;
    public bool scattered;
    private bool soundFlag = false;
    [SerializeField] private GameObject nextObject;
    private void Start()
    {
        scattered = false;
        sheild = GetComponent<BoxCollider>();
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == PlayerPrefsHandler.Player)
        {
            if (GamePlayManager.Instance.currentPlayer.GetComponent<PlayerBallMerge>().GetPlayerNumber() >= value)
            {
               
                sheild.isTrigger = true;
                scattered = true;
                if(nextObject)
                    nextObject.SetActive(true);
                ApplyForceOnBoxes();
            }
            else
            {
                sheild.isTrigger = false;
                //RepelBall(collision.gameObject);
            }
        }
    }

    public void ApplyForceOnBoxes()
    {
        foreach (GameObject obj in allBoxes)
        {
            obj.GetComponent<Rigidbody>().isKinematic = false;
           // obj.GetComponent<Rigidbody>().AddExplosionForce(20f, obj.transform.position , 10f, 0.1f, ForceMode.Impulse);
         //   obj.GetComponent<BoxCollider>().isTrigger = true;
        }
        if (soundFlag) return;
        soundFlag = true;
        SoundController.Instance.PlayWallBreakingSound();

    }
    public void RepelBall(GameObject obj)
    {
        Rigidbody rb = obj.GetComponentInParent<Rigidbody>();
        rb.mass = 2;
        rb.velocity = Vector2.zero;
        rb.AddRelativeForce(Vector3.back * 20f, ForceMode.Impulse);
        obj.GetComponent<BallController>().StartMovement(false);
        GamePlayManager.Instance.GameComplete(1f);
    }
}
