using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using GameData.MyScripts;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerBallMerge : MonoBehaviour
{
    [SerializeField] private PlayerBall currentBall;
    [SerializeField] private List<PlayerBall> playerBalls = new List<PlayerBall>();
    [SerializeField] private LayerMask layersToDetect = 0;
    private bool _degradeFlag = false, _shieldFlag = false;
    [SerializeField] private Transform unmergingPoint;
    private List<Ball> mergedBalls = new List<Ball>();
    [SerializeField] private AnimationCurve animationCurve;
    private BallController ballController;
    private PlayerController playerController;
    private void Start()
    {
        ballController = GetComponent<BallController>();
        playerController = GetComponent<PlayerController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!IsInLayerMask(other.gameObject, layersToDetect))
            return;
        switch (other.gameObject.tag)
        {
            case PlayerPrefsHandler.Obstacle:
                if(_shieldFlag) return;
                if(_degradeFlag) return;
                _degradeFlag = true;
                HitObstacle();
                break;
            case PlayerPrefsHandler.LevelEndPoint:
                Debug.Log("LevelEndPoint triggered");
                other.gameObject.SetActive(false);
                //GamePlayManager.Instance.SetGameToPlay(3);
                if(ballController)
                    ballController.StartMovement(false);
                else
                    playerController.StartMovement(false);
                break;
            case PlayerPrefsHandler.LevelFailTrigger:
                other.gameObject.SetActive(false);
                SoundController.Instance.PlayBallWaterSplashSound();
                GamePlayManager.Instance.GameOver(1f);
                break;
            case PlayerPrefsHandler.RevivePointTrigger:
                GamePlayManager.Instance.currentLevel.SetRevivePoint(other.transform.GetChild(0));
                break;
            case PlayerPrefsHandler.EventTrigger:
                GamePlayManager.Instance.SendLevelMiddleProgressionEvent();
                break;
            case PlayerPrefsHandler.Lifter:
                other.GetComponent<BoxCollider>().enabled = false;
                other.GetComponent<Animator>().enabled = true;
                break;
            case PlayerPrefsHandler.Hole:
                if (ballController)
                {
                    ballController.startRun = false;
                    ballController.horizontalMovement = false;
                    ballController.speed = 0f;
                    ballController.actualSpeed = 0f;
                    Hole(other.transform.Find("Pos"));
                }
                else
                {
                    playerController.startRun = false;
                    playerController.horizontalMovement = false;
                    playerController.fwdSpeed = 0;
                    Hole(other.transform.Find("Pos"));
                }
                break;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (!IsInLayerMask(other.gameObject, layersToDetect))
            return;
        _degradeFlag = other.gameObject.tag switch
        {
            PlayerPrefsHandler.Obstacle => false,
            _ => _degradeFlag
        };
        switch (other.gameObject.tag)
        {
            case PlayerPrefsHandler.Obstacle:
                _degradeFlag = false;
                break;
        }
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
    private void HitBall(GameObject otherBall)
    {
        var ballScript = otherBall.GetComponent<Ball>();
        if(ballScript.IsBallMerged()) return;
        if (ballScript.GetNumber() != currentBall.ballNumber || GamePlayManager.Instance.IsLevelCompleted() || currentBall.ballNumber == 2048)
        {
            SoundController.Instance.PlayBallHitSound();
            return;
        }
        Upgrade();
        ballScript.SetMergingFlag(true);
        AddMergedBall(ballScript);
        otherBall.SetActive(false);
        otherBall.transform.parent = unmergingPoint;
        otherBall.transform.localPosition = Vector3.zero;
        otherBall.transform.localRotation = Quaternion.identity;
        //ballScript.DestroyBall();
    }
    private void HitObstacle()
    {
        Degrade();
    }
    private void Upgrade()
    {
        //transform.Find("MergeEffect").gameObject.SetActive(true);
        currentBall.ballNumber *= 2;
        SetPlayerBall();
        ScaleUp(currentBall.ballObject.transform);
        SoundController.Instance.PlayBallMergeSound();
    }
    private void Degrade()
    {
        if (currentBall.ballNumber == 2)
        {
            if(ballController)
                ballController.StartMovement(false);
            else
                playerController.StartMovement(false);
            GamePlayManager.Instance.GameOver(1f);
            return;
        }
        var ballToUnmerge = GetMergedBall(currentBall.ballNumber / 2);
        if (ballToUnmerge)
        {
            ballToUnmerge.transform.parent = null;
            ballToUnmerge.gameObject.SetActive(true);
            ballToUnmerge.GetComponent<Rigidbody>().AddForce(Vector3.back * 10f, ForceMode.Impulse);
            mergedBalls.Remove(ballToUnmerge);
        }
        //transform.Find("MergeEffect").gameObject.SetActive(true);
        currentBall.ballNumber /= 2;
        SetPlayerBall();
        SoundController.Instance.PlayBallUnMergeSound();
        //ScaleDown(currentBall.ballObject.transform);
    }
    private void SetPlayerBall()
    {
        SetBall();
        if (currentBall.ballObject == null)
        {
            Destroy(gameObject);
            return;
        }
    }
    private void SetBall()
    {
        currentBall.ballTrail.SetActive(false);
        currentBall.mergeEffect.SetActive(false);
        currentBall.ballObject.SetActive(false);
        foreach (var ball in playerBalls.Where(ball => ball.ballNumber == currentBall.ballNumber))
        {
            currentBall.ballObject = ball.ballObject;
            currentBall.mergeEffect = ball.mergeEffect;
            currentBall.ballTrail = ball.ballTrail;
        }
        currentBall.ballTrail.SetActive(true);
        currentBall.mergeEffect.SetActive(true);
        currentBall.ballObject.SetActive(true);
        if(ballController)
            ballController.SetBallToRotate(currentBall.ballObject.GetComponent<Animator>());
        /*else
            playerController.SetBallToRotate(currentBall.ballObject.GetComponent<Animator>());*/
        if(_shieldFlag)
            currentBall.ballObject.transform.Find("ShieldEffect").gameObject.SetActive(true);
    }
    public int GetPlayerNumber()
    {
        return currentBall.ballNumber;
    }
    private static bool IsInLayerMask(GameObject obj, LayerMask mask)
    {
        return ((mask.value & (1 << obj.layer)) > 0);
    }
    private void AddMergedBall(Ball ball)
    {
        if (mergedBalls.Count == 0)
            mergedBalls.Add(ball);
        else
        {
            if (mergedBalls.Any(mergedBall => mergedBall.GetNumber() == ball.GetNumber()))
            {
                return;
            }
            mergedBalls.Add(ball);
        }
    }
    private Ball GetMergedBall(int ballNumber)
    {
        return mergedBalls.Count == 0 ? null : mergedBalls.FirstOrDefault(mergedBall => mergedBall.GetNumber() == ballNumber);
    }
    private void ScaleUp(Transform ballToScale)
    {
        var scaleValue = PlayerPrefsHandler.GetBallSize(currentBall.ballNumber);
        var localScale = new Vector3(scaleValue, scaleValue, scaleValue);
        //var localScale = ballToScale.localScale;
        localScale = localScale - new Vector3(0.05f, 0.05f, 0.05f);
        ballToScale.localScale = localScale;
        var currentScale = localScale;
        var targetScale = currentScale + new Vector3(0.05f, 0.05f, 0.05f);
        //ballToScale.DOScale(targetScale, 0.5f);
        /*Debug.Log("BallName: " + ballToScale.name);
        Debug.Log("currentScale: " + currentScale);
        Debug.Log("targetScale: " + targetScale);*/
        StartCoroutine(DelayToScaleUp(ballToScale, targetScale));
    }
    private const float BounceFactor = 0.5f;
    private IEnumerator DelayToScaleUp(Transform ballToScale, Vector3 targetScale)
    {
        const float DURATION = 0.25f;
        var time = 0f;
        while (time <= DURATION)
        {
            time += Time.deltaTime;
            var linearT = time / DURATION;
            var factorT = animationCurve.Evaluate(linearT);
            var factor = Mathf.Lerp(0f, BounceFactor, factorT);
            //ballToScale.localScale = Vector3.Lerp(ballToScale.localScale, targetScale, time);
            ballToScale.localScale = Vector3.Lerp(ballToScale.localScale, targetScale, linearT) + new Vector3(factor, factor, factor);
            yield return null;
        }
    }
    private void ScaleDown(Transform ballToScale)
    {
        //Debug.Log("ballToScale: " + ballToScale.name);
        var scaleValue = PlayerPrefsHandler.GetBallSize(currentBall.ballNumber);
        var localScale = new Vector3(scaleValue, scaleValue, scaleValue);
        //localScale = localScale + new Vector3(0.2f, 0.2f, 0.2f);
        ballToScale.localScale = localScale;
        var targetScale = localScale - new Vector3(0.1f, 0.1f, 0.1f);
        StartCoroutine(DelayToScaleDown(ballToScale, targetScale));
    }
    private IEnumerator DelayToScaleDown(Transform ballToScale, Vector3 targetScale)
    {
        const float DURATION = 0.25f;
        var time = 0f;
        while (time <= 0.25f)
        {
            time += Time.deltaTime;
            var linearT = time / DURATION;
            var factorT = animationCurve.Evaluate(linearT);
            var factor = Mathf.Lerp(0f, BounceFactor, factorT);
            //ballToScale.localScale = Vector3.Lerp(ballToScale.localScale, targetScale, time);
            ballToScale.localScale = Vector3.Lerp(ballToScale.localScale, targetScale, linearT) + new Vector3(factor, factor, factor);
            yield return null;
        }
    }
    public void SpecificUpgrade(int number)
    {
        currentBall.ballNumber = number;
        SetPlayerBall();
        ScaleUp(currentBall.ballObject.transform);
        if(ballController)
            currentBall.ballObject.GetComponent<Animator>().enabled = false;
        SoundController.Instance.PlayParkingSound();
    }
    public void ActivateShield()
    {
        _shieldFlag = true;
        GetShieldEffect().SetActive(true);
    }
    private void Hole(Transform targetPos)
    {
        transform.DOMove(targetPos.position, 0.25f);
    }
    public void OpenScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
    public GameObject GetShieldEffect()
    {
        return currentBall.ballObject.transform.Find("ShieldEffect").gameObject;
    }
}
[Serializable]
public class PlayerBall
{
    public int ballNumber = 2;
    public GameObject ballObject;
    public GameObject mergeEffect;
    public GameObject ballTrail;
}