using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using GameData.MyScripts;

public class EndingGlassRotate : MonoBehaviour
{
    public Transform[] positionsToLerp;
    public DropGlassDown[] glassesToDrop;
    private float[] lerpTime = { 0.25f, 0.5f, 0.75f, 1.0f, 1.25f, 1.5f, 1.75f, 2.0f, 2.25f, 2.5f, 2.75f, 3.0f };
    List<int> valuesArr = new List<int> { 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024, 2048 };

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == PlayerPrefsHandler.Player)
        {
            int ballNum = GamePlayManager.Instance.currentPlayer.GetComponent<PlayerBallMerge>().GetPlayerNumber();
            int index = valuesArr.IndexOf(ballNum);
            other.gameObject.GetComponentInParent<BallController>().startRun = true;
            GamePlayManager.Instance.playerCamera.GetComponent<CinemachineVirtualCamera>().LookAt = other.gameObject.transform;
            GamePlayManager.Instance.playerCamera.GetComponent<CinemachineVirtualCamera>().Follow = other.gameObject.transform;

            other.gameObject.transform.position = new Vector3(0, other.gameObject.transform.position.y, other.gameObject.transform.position.z);
            //other.GetComponentInParent<Rigidbody>().isKinematic = true;
            LerpToTranform(index, other.gameObject);
            other.gameObject.GetComponentInParent<BallController>().horizontalMovement = false;
            this.GetComponent<BoxCollider>().enabled = false;
        }
    }
    Coroutine lerp;
    public void LerpToTranform(int index, GameObject ball)
    {
        if (lerp != null)
        {
            StopCoroutine(lerp);
        }
        lerp = StartCoroutine(LerpingBall(index, ball));

    }
    public bool triggered = false;
    IEnumerator LerpingBall(int index, GameObject ball)
    {
        float t = 0;
        Vector3 initailPos = ball.transform.position;
        Vector3 targetpos = positionsToLerp[index].position + Vector3.up;
        float duration = lerpTime[index];
        ball.GetComponentInParent<BallController>().speed *= 2f;
        while (!triggered)
        {
            triggered = glassesToDrop[index].triggered;
            ball.GetComponentInParent<BallController>().speed = 800;
            yield return null;
        }
        yield return new WaitForSeconds(0.1f);
        ball.transform.position = targetpos;
        ball.GetComponentInParent<BallController>().startRun = false;
        ball.GetComponent<Animator>().enabled = false;
      
        yield return null;
     
    }
}
