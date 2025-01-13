using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using GameData.MyScripts;
using UnityEngine;
public class BallMerging : MonoBehaviour
{
    private Ball ball;
    [SerializeField] private PlayerBall currentBall;
    [SerializeField] private List<PlayerBall> playerBalls = new List<PlayerBall>();
    [SerializeField] private LayerMask layersToDetect = 0;
    private void Start()
    {
        ball = GetComponent<Ball>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!IsInLayerMask(collision.gameObject, layersToDetect))
            return;
        switch (collision.gameObject.tag)
        {
            case   PlayerPrefsHandler.Ball:
                    HitBall(collision.gameObject);
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsInLayerMask(other.gameObject, layersToDetect))
            return;
        switch (other.gameObject.tag)
        {
            case   PlayerPrefsHandler.LevelFailTrigger:
                gameObject.SetActive(false);
                break;
        }
    }
    private void ShowPlayer()
    {
        GamePlayManager.Instance.playerCamera.SetActive(true);
        GamePlayManager.Instance.EnablePlayer(true);
        GamePlayManager.Instance.GetGamePlayUIManager().controls.EnableControls();
    }
    private void HitBall(GameObject otherBall)
    {
        var ballScript = otherBall.GetComponent<Ball>();
        if(ballScript.IsBallMerged()) return;
        if (ballScript.GetNumber() != ball.GetNumber()) return;
        Upgrade();
        ballScript.SetMergingFlag(true);
        otherBall.SetActive(false);
    }
    private void Upgrade()
    {
        ball.IncreaseNumber();
        currentBall.ballNumber *= 2;
        SetPlayerBall();
        ScaleUp(transform);
    }
    public void SetBall(int number) {
        currentBall.ballNumber = number;
        SetPlayerBall();
        ScaleUp(transform);
    }
    private void SetPlayerBall()
    {
        currentBall.ballObject.SetActive(false);
        SetBall();
        if (currentBall.ballObject == null)
        {
            Destroy(gameObject);
            return;
        }
        currentBall.ballObject.SetActive(true);
    }
    private void SetBall()
    {
        currentBall.ballObject.SetActive(false);
        foreach (var ball in playerBalls.Where(ball => ball.ballNumber == currentBall.ballNumber))
        {
            currentBall.ballObject = ball.ballObject;
            currentBall.ballTrail = ball.ballTrail;
        }
        currentBall.ballObject.SetActive(true);
    }
    private void ScaleUp(Transform ballToScale)
    {
        var localScale = ballToScale.localScale;
        localScale = localScale - new Vector3(0.1f, 0.1f, 0.1f);
        ballToScale.localScale = localScale;
        var currentScale = localScale;
        var targetScale = currentScale + new Vector3(0.1f, 0.1f, 0.1f);
        ballToScale.DOScale(targetScale, 1f)
            .SetEase(Ease.OutBack);
    }
    private static bool IsInLayerMask(GameObject obj, LayerMask mask)
    {
        return ((mask.value & (1 << obj.layer)) > 0);
    }
}