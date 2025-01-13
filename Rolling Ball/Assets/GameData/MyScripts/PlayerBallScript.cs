using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameData.MyScripts;
using UnityEngine;
public class PlayerBallScript : MonoBehaviour
{
    [SerializeField] private PlayerBall currentBall;
    [SerializeField] private List<PlayerBall> playerBalls = new List<PlayerBall>();
    public LayerMask layerToDetect = 0;
    private bool _degradeFlag = false;
    [SerializeField] private Transform mergeEffect;
    private void Start()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!IsInLayerMask(other.gameObject, layerToDetect))
            return;
        switch (other.gameObject.tag)
        {
            case   PlayerPrefsHandler.Obstacle:
                if(_degradeFlag) return;
                _degradeFlag = true;
                HitObstacle();
                break;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (!IsInLayerMask(other.gameObject, layerToDetect))
            return;
        switch (other.gameObject.tag)
        {
            case   PlayerPrefsHandler.Obstacle:
                _degradeFlag = false;
                break;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!IsInLayerMask(collision.gameObject, layerToDetect))
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
        if (ballScript.GetNumber() == currentBall.ballNumber)
        {
            Upgrade();
            ballScript.DestroyBall();
        }
    }
    private void HitObstacle()
    {
        Degrade();
    }
    private void Upgrade()
    {
        transform.Find("MergeEffect").gameObject.SetActive(true);
        currentBall.ballNumber *= 2;
        SetPlayerBall();
    }
    private void Degrade()
    {
        transform.Find("MergeEffect").gameObject.SetActive(true);
        currentBall.ballNumber /= 2;
        SetPlayerBall();
    }
    private void SetPlayerBall()
    {
        currentBall.ballObject.SetActive(false);
        currentBall.ballObject = GetBallObject();
        if (currentBall.ballObject == null)
        {
            Destroy(gameObject);
            return;
        }
        currentBall.ballObject.SetActive(true);
    }
    private GameObject GetBallObject()
    {
        return (from ball in playerBalls where ball.ballNumber == currentBall.ballNumber select ball.ballObject).FirstOrDefault();
    }
    private static bool IsInLayerMask(GameObject obj, LayerMask mask)
    {
        return ((mask.value & (1 << obj.layer)) > 0);
    }
    [Serializable]
    private class PlayerBall
    {
        public int ballNumber = 2;
        public GameObject ballObject;
    }
}