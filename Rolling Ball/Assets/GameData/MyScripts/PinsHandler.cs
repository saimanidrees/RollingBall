using System.Collections;
using System.Collections.Generic;
using GameData.MyScripts;
using UnityEngine;
public class PinsHandler : MonoBehaviour
{
    public Transform[] positionsToLerp;
    private float[] lerpTime= { 0.25f,0.5f, 0.75f, 1.0f, 1.25f, 1.5f,1.75f,2.0f,2.25f,2.5f,2.75f,3.0f };
    List<int> valuesArr = new List<int>{ 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024, 2048 };
    private BallController ballController;
    private PlayerController playerController;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(PlayerPrefsHandler.Player)) {
            var ballNum = GamePlayManager.Instance.currentPlayer.GetComponent<PlayerBallMerge>().GetPlayerNumber();
            var index = valuesArr.IndexOf(ballNum);
            ballController = other.gameObject.GetComponentInParent<BallController>();
            playerController = other.gameObject.GetComponentInParent<PlayerController>();
            LerpToTransform(index, other.gameObject);
            if(ballController)
                ballController.horizontalMovement = false;
            else
                playerController.horizontalMovement = false;
            var xAxis = transform.Find("Pos").position.x;
            GamePlayManager.Instance.currentPlayer.transform.position = new Vector3(xAxis, GamePlayManager.Instance.currentPlayer.transform.position.y, 
                GamePlayManager.Instance.currentPlayer.transform.position.z);
            if(ballController)
                ballController.ResetCameraTarget();
            this.GetComponent<BoxCollider>().enabled = false;
        }
    }
    private Coroutine lerp;

    private void LerpToTransform(int index, GameObject ball) {
        if (lerp != null) {
            StopCoroutine(lerp);
        }
        lerp = StartCoroutine(LerpBall(index, ball));
    
    }
    public bool triggered = false;
    private IEnumerator LerpBall(int index, GameObject ball)
    {
        var delay = 0.5f;
        if (ballController)
        {
            ballController.speed *= 2f;
            delay = 0.5f;
        }
        else
        {
            playerController.fwdSpeed *= 3f;
            delay = 0.2f;
        }
        while (!triggered)
        {
            triggered = positionsToLerp[index].GetComponent<PinsAttackDealer>().scattered;
            if (ballController) 
                ballController.speed = 1000;
            yield return null;
        }
        yield return new WaitForSeconds(delay);
       if(ballController)
       {
           ballController.startRun = false;
            ball.GetComponent<Animator>().enabled = false;
        }
       else
       {
           playerController.startRun = false;
       }
        GamePlayManager.Instance.GameComplete(1f);
        yield return null;
        ball.GetComponentInParent<Rigidbody>().isKinematic = true;
    }
}