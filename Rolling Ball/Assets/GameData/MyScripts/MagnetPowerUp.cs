using System.Collections;
using System.Collections.Generic;
using GameData.MyScripts;
using UnityEngine;
public class MagnetPowerUp : MonoBehaviour
{
    [SerializeField] private float magnetForce = 100;
    [SerializeField] private int duration;
    private bool magnetEffectFlag = false;
    private List<Rigidbody> caughtRigidbodies = new List<Rigidbody>();
    [SerializeField] private LayerMask layersToDetect = 0;
    private PlayerBallMerge playerBallMerge;
    private MergeInfinityBall mergeInfinityBall;
    private void Start()
    {
        playerBallMerge = GamePlayManager.Instance.currentPlayer.GetComponent<PlayerBallMerge>();
        mergeInfinityBall = GamePlayManager.Instance.currentPlayer.GetComponent<MergeInfinityBall>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!IsInLayerMask(other.gameObject, layersToDetect))
            return;
        switch (other.gameObject.tag)
        {
            case   PlayerPrefsHandler.Ball:
                if (magnetEffectFlag)
                {
                    var otherBall = other.transform.parent;
                    var rb = otherBall.GetComponent<Rigidbody>();
                    if (GameManager.Instance.IsBallMergeMode())
                    {
                        var ballMerging = otherBall.GetComponent<Ball>();
                        if(ballMerging.GetNumber() == playerBallMerge.GetPlayerNumber())
                            if(!caughtRigidbodies.Contains(rb))
                                caughtRigidbodies.Add(otherBall.GetComponent<Rigidbody>());
                    }
                    else if (GameManager.Instance.IsInfiniteMode())
                    {
                        var ballMerging = otherBall.GetComponent<BallModifier>();
                        if(!ballMerging) return;
                        var number = ballMerging.mainValue;
                        var character = ballMerging.character;
                        if(number == mergeInfinityBall.value && character == mergeInfinityBall.character)
                            if(!caughtRigidbodies.Contains(rb))
                                caughtRigidbodies.Add(otherBall.GetComponent<Rigidbody>());
                    }
                }
                break;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (!IsInLayerMask(other.gameObject, layersToDetect))
            return;
        switch (other.gameObject.tag)
        {
            case   PlayerPrefsHandler.Ball:
                if (magnetEffectFlag)
                {
                    var otherBall = other.transform.parent;
                    var rb = otherBall.GetComponent<Rigidbody>();
                    if (GameManager.Instance.IsBallMergeMode())
                    {
                        var ballMerging = otherBall.GetComponent<Ball>();
                        if(ballMerging.IsBallMerged()) return;
                        if(ballMerging.GetNumber() == playerBallMerge.GetPlayerNumber())
                            if(!caughtRigidbodies.Contains(rb))
                                caughtRigidbodies.Add(otherBall.GetComponent<Rigidbody>());
                    }
                    else if (GameManager.Instance.IsInfiniteMode())
                    {
                        var ballMerging = otherBall.GetComponent<BallModifier>();
                        if(!ballMerging) return;
                        var number = ballMerging.mainValue;
                        var character = ballMerging.character;
                        if(number == mergeInfinityBall.value && character == mergeInfinityBall.character)
                            if(!caughtRigidbodies.Contains(rb))
                                caughtRigidbodies.Add(otherBall.GetComponent<Rigidbody>());
                    }
                }
                break;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (!IsInLayerMask(other.gameObject, layersToDetect))
            return;
        switch (other.gameObject.tag)
        {
            case   PlayerPrefsHandler.Ball:
                var rb = other.GetComponentInParent<Rigidbody>();
                if (magnetEffectFlag)
                {
                    if (caughtRigidbodies.Contains(rb))
                        caughtRigidbodies.Remove(rb);
                }
                break;
        }
    }
    public void ActivateMagnetEffect(bool flag)
    {
        magnetEffectFlag = flag;
        GamePlayManager.Instance.GetGamePlayUIManager().controls.SetMagnetEffectTimerValue(duration);
        GamePlayManager.Instance.GetGamePlayUIManager().controls.ShowMagnetEffectTimer(flag);
        GetComponent<SphereCollider>().enabled = flag;
        if (flag)
            StartCoroutine(WaitToEndMagnetEffect());
    }
    public void AttractBallsByMagnet()
    {
        if(!magnetEffectFlag) return;
        if (caughtRigidbodies.Count == 0) return;
        foreach (var t in caughtRigidbodies)
        {
            if (GameManager.Instance.IsBallMergeMode())
            {
                var ballMerging = t.GetComponent<Ball>();
                if (ballMerging.IsBallMerged()) continue;
                if (ballMerging.GetNumber() == playerBallMerge.GetPlayerNumber())
                    t.velocity = (transform.position - (t.transform.position + t.centerOfMass))
                                 * magnetForce * Time.deltaTime;
            }
            else if (GameManager.Instance.IsInfiniteMode())
            {
                if(!t) return;
                if(t.CompareTag("wasted")) return;
                var ballMerging = t.GetComponent<BallModifier>();
                if (!ballMerging) continue;
                var number = ballMerging.mainValue;
                var character = ballMerging.character;
                if (number == mergeInfinityBall.value && character == mergeInfinityBall.character)
                    t.velocity = (transform.position - (t.transform.position + t.centerOfMass))
                                 * magnetForce * Time.deltaTime;
            }
        }
    }
    private IEnumerator WaitToEndMagnetEffect()
    {
        if (GameManager.Instance.IsInfiniteMode())
        {
            yield return new WaitUntil(() => GetComponentInParent<BallController>().startRun);
        }
        else
        {
            yield return new WaitUntil(() => GetComponentInParent<BallController>().startRun);
        }
        var time = 0;
        while (time < duration)
        {
            time++;
            yield return new WaitForSeconds(1f);
            GamePlayManager.Instance.GetGamePlayUIManager().controls.SetMagnetEffectTimerValue(duration - time);
        }
        caughtRigidbodies.Clear();
        ActivateMagnetEffect(false);
        gameObject.SetActive(false);
    }
    private static bool IsInLayerMask(GameObject obj, LayerMask mask)
    {
        return ((mask.value & (1 << obj.layer)) > 0);
    }
}