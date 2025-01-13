using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameData.MyScripts;
using UnityEngine;
using TMPro;
public class MergeInfinityBall : MonoBehaviour
{
    public int value = 2;
    public char character = '\0';
    [SerializeField] private List<int> intNums;
    [SerializeField] private List<InfinitePlayerBall> infinitePlayerBalls = new List<InfinitePlayerBall>();
    private Stack<GameObject> _stackBalls;
    public GameObject stackPosition;
    [SerializeField] private InfinitePlayerBall currentInfinitePlayerBall;
    private bool _degrade;
    private WallBreak _currentWall;
    [SerializeField] private AnimationCurve animationCurve;
    private BallController _ballController;
    public List<Transform> stackedBalls = new List<Transform>();
    [SerializeField] private float stackedBallsDistance;
    private int _gapsCounter = 0, _gapFillerMatIndex = 0;
    [SerializeField] private List<Material> gapFillerMaterials;
    public delegate void InfiniteMerge();
    public static event InfiniteMerge OnInfiniteMerge;
    private void Start()
    {
        _ballController = GetComponent<BallController>();
        _stackBalls = new Stack<GameObject>();
        SetTextOn();
        stackedBalls.Add(transform.GetChild(0));
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(PlayerPrefsHandler.Ball))
        {
            var ball = collision.gameObject.GetComponent<BallModifier>();
            if (!ball) return;
            if (ball.mainValue != value) return;
            var ch = collision.gameObject.GetComponent<BallModifier>().character;
            if (ch.Equals(character)) {
                PushInStack(collision.gameObject);
            }
        }
        else if (collision.gameObject.CompareTag(PlayerPrefsHandler.Wall))
        {
            // check if both are null
            if (collision.gameObject.GetComponent<WallBreak>().alphabet == '\0' && character == '\0')
            {
                if (collision.gameObject.GetComponent<WallBreak>().value <= value)
                {
                    BreakWall(collision.gameObject);
                }
                else
                    RepelFromWall();
            }
            // check if both are not  null
            else if (collision.gameObject.GetComponent<WallBreak>().alphabet != '\0' && character != '\0')
            {
                if (collision.gameObject.GetComponent<WallBreak>().alphabet <= character)
                {
                    if (collision.gameObject.GetComponent<WallBreak>().alphabet < character) {
                        BreakWall(collision.gameObject);
                    }
                    else if (collision.gameObject.GetComponent<WallBreak>().value <= value)
                    {
                        BreakWall(collision.gameObject);
                    }
                    else 
                        RepelFromWall();
                }
                else
                    RepelFromWall();
            }
            //check if wall character is null only
            else if (collision.gameObject.GetComponent<WallBreak>().alphabet == '\0' && character != '\0')
            {
                BreakWall(collision.gameObject);
            }
            else
                RepelFromWall();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(PlayerPrefsHandler.Obstacle))
        {
            if(_degrade) return;
            _degrade = true;
            PopFromStack();
        }
        else if (other.gameObject.CompareTag(PlayerPrefsHandler.LevelFailTrigger))
        {
            Debug.Log("LevelFailTrigger");
            other.gameObject.SetActive(false);
            SoundController.Instance.PlayBallWaterSplashSound();
            GamePlayManager.Instance.GameOver(1f);
        }
        else if (other.gameObject.CompareTag(PlayerPrefsHandler.WallTriggerToUpgrade))
        {
            other.gameObject.SetActive(false);
            if(!AdsCaller.Instance.IsRewardedAdAvailable() && !AdsCaller.Instance.IsInterstitialAdAvailable()) return;
            var wall = other.GetComponentInParent<WallBreak>();
            if (wall.alphabet == '\0' && character == '\0')
            {
                if (wall.value <= value)
                {
                    //BreakWall(other.gameObject);
                }
                else
                    SetWallToUpgrade(wall);
            }
            // check if both are not  null
            else if (wall.alphabet != '\0' && character != '\0')
            {
                if (wall.alphabet <= character)
                {
                    if (wall.alphabet < character) {
                        //BreakWall(other.gameObject);
                    }
                    else if (wall.value <= value)
                    {
                        //BreakWall(other.gameObject);
                    }
                    else 
                        SetWallToUpgrade(wall);
                }
                else
                    SetWallToUpgrade(wall);
            }
            //check if wall character is null only
            else if (wall.alphabet == '\0' && character != '\0')
            {
                //BreakWall(other.gameObject);
            }
            else
                SetWallToUpgrade(wall);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(PlayerPrefsHandler.Obstacle))
        {
            if (_degrade) 
             _degrade = false;
        }
    }
    private void SetTextOn() 
    {
        //Debug.Log("SetTextOnValue" + value + character);
        currentInfinitePlayerBall.numberTxt.text = value + character.ToString();
        InfinityManager.instance.SetBallDetails(value, character);
    }
    private void SetBallObject()
    {
        var index = intNums.IndexOf(value);
        currentInfinitePlayerBall.ballObject.SetActive(false);
        currentInfinitePlayerBall.mergeEffect.SetActive(false);
        currentInfinitePlayerBall.ballTrail.SetActive(false);
        currentInfinitePlayerBall.ballObject = infinitePlayerBalls[index].ballObject;
        currentInfinitePlayerBall.mergeEffect = infinitePlayerBalls[index].mergeEffect;
        currentInfinitePlayerBall.ballTrail = infinitePlayerBalls[index].ballTrail;
        currentInfinitePlayerBall.numberTxt = infinitePlayerBalls[index].numberTxt;
        currentInfinitePlayerBall.ballObject.SetActive(true);
        currentInfinitePlayerBall.mergeEffect.SetActive(true);
        currentInfinitePlayerBall.ballTrail.SetActive(true);
        if (value != 2 || character != '\0')
        {
            currentInfinitePlayerBall.ballTrail.SetActive(false);
        }
        _ballController.SetBallToRotate(currentInfinitePlayerBall.ballObject.GetComponent<Animator>());
        OnInfiniteMerge?.Invoke();
    }
    public void PushInStack(GameObject ball) {
        _stackBalls.Push(ball);
        AddOtherBallInStack(ball.transform);
        value *= 2;
        if (value >= 4096) {
            value = 2;
            if (character == '\0')
            {
                character = 'A';
            }
            else
            {
                character++;
            }
        }
        SetBallObject();
        SetTextOn();
        SoundController.Instance.PlayBallMergeSound();
        ScaleUp(currentInfinitePlayerBall.ballObject.transform);
        _ballController.IncreaseActualSpeed(20);
    }

    public void PopFromStack() {
        Debug.Log("Pop");
        if (_stackBalls.Count == 0)
        {
            Debug.Log("Death");
            _ballController.StartMovement(false);
            GamePlayManager.Instance.GameOver(1);
            return;
        }
        var obj = _stackBalls.Pop();
        if (obj != null) { 
            obj.transform.parent = null;
            obj.tag = "wasted";
            RemoveBallFromStack(obj.transform);
                //iTween.Destroy(obj, 3f); 
        }
        value /= 2;
        if (value <= 1)
        {
            value = 2048;
            if (character == '\0')
            {
                // Do Nothing
            }
            else
            {
                if (character == 'A') {
                    character = '\0';
                }
                else
                    character--;
            }
        }
        SetBallObject();
        SetTextOn();
        SoundController.Instance.PlayBallUnMergeSound();
        //ScaleDown(currentInfinitePlayerBall.ballObject.transform);
        //GetComponent<BallController>().IncreaseActualSpeed(-20);
    }
    private void BreakWall(GameObject wall) {
        wall.GetComponent<BoxCollider>().isTrigger = true;
        wall.GetComponent<WallBreak>().BreakWall();
        SoundController.Instance.PlayGlassBreakSound();
    }
    private void RepelFromWall() {
        SoundController.Instance.PlayBallHitSound();
        GetComponent<Rigidbody>().mass = 2;
        GetComponent<Rigidbody>().AddRelativeForce(Vector3.back * 20f,ForceMode.Impulse);
        _ballController.StartMovement(false);
        GamePlayManager.Instance.GameOver(2f);
    }
    private void SetWallToUpgrade(WallBreak wall)
    {
        _currentWall = wall;
        GamePlayManager.Instance.GetGamePlayUIManager().SubMenu(PlayerPrefsHandler.BreakTheWallPopup);
        _ballController.StartMovement(false);
        _ballController.GetComponent<Rigidbody>().isKinematic = true;
    }
    public void WallUpgrade()
    {
        _currentWall.value = value;
        _currentWall.alphabet = character;
        _currentWall.SetValues(value + character.ToString());
        _ballController.GetComponent<Rigidbody>().isKinematic = false;
        _ballController.StartMovement(true);
        SoundController.Instance.PlayParkingSound();
    }
    private void ScaleUp(Transform ballToScale)
    {
        var scaleValue = PlayerPrefsHandler.GetBallSize(value);
        if (character != '\0') scaleValue = 2f;
        var localScale = new Vector3(scaleValue, scaleValue, scaleValue);
        //var localScale = ballToScale.localScale;
        localScale = localScale - new Vector3(0.1f, 0.1f, 0.1f);
        //Debug.Log("LocalScale: " + localScale);
        ballToScale.localScale = localScale;
        var currentScale = localScale;
        var targetScale = currentScale + new Vector3(0.1f, 0.1f, 0.1f);
        //Debug.Log("TargetScale: " + targetScale);
        StartCoroutine(DelayToScaleUp(ballToScale, targetScale));
        /*ballToScale.DOScale(targetScale, 0.25f)
            .SetEase(animationCurve);*/
        //ballToScale.localScale = localScale;
        /*ballToScale.DOScale(localScale, 0.25f)
            .SetEase(animationCurve);*/
    }
    private const float BounceFactor = 0.5f;
    private IEnumerator DelayToScaleUp(Transform ballToScale, Vector3 targetScale)
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
    private void ScaleDown(Transform ballToScale)
    {
        var scaleValue = PlayerPrefsHandler.GetBallSize(value);
        var localScale = new Vector3(scaleValue, scaleValue, scaleValue);
        //var localScale = ballToScale.localScale;
        localScale = localScale + new Vector3(0.1f, 0.1f, 0.1f);
        ballToScale.localScale = localScale;
        var currentScale = localScale;
        var targetScale = currentScale - new Vector3(0.1f, 0.1f, 0.1f);
        StartCoroutine(DelayToScaleDown(ballToScale, targetScale));
        /*ballToScale.DOScale(targetScale, 0.25f)
            .SetEase(animationCurve);*/
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
    public void SetStackedBallsPosition()
    {
        if (stackedBalls.Count > 1)
        {
            for (var i = 1; i < stackedBalls.Count; i++)
            {
                var firstBall = stackedBalls.ElementAt(i - 1);
                var secBall = stackedBalls.ElementAt(i);
                var desiredDistance = Vector3.Distance(secBall.position, firstBall.position);
                if (desiredDistance <= stackedBallsDistance)
                {
                    secBall.position = new Vector3(
                        Mathf.Lerp(secBall.position.x, firstBall.position.x, 10f * Time.deltaTime),
                        Mathf.Lerp(secBall.position.y, firstBall.position.y, 10f * Time.deltaTime),
                        Mathf.Lerp(secBall.position.z, firstBall.position.z + -0.5f,
                            10f * Time.deltaTime));
                    secBall.rotation = Quaternion.Lerp(secBall.rotation, Quaternion.identity, 10f * Time.deltaTime);
                }
                //Debug.Log(firstBall.name);
            }
        }
    }

    private void AddOtherBallInStack(Transform otherBall)
    {
        otherBall.transform.parent = null;
        otherBall.GetComponent<Rigidbody>().isKinematic = true;
        otherBall.GetComponentInChildren<SphereCollider>().isTrigger = true;
        otherBall.tag = gameObject.tag;
        otherBall.gameObject.layer = gameObject.layer;
        otherBall.GetComponent<BallModifier>().GetBallText().SetActive(false);
        stackedBalls.Insert(1, otherBall);
    }
    private void RemoveBallFromStack(Transform otherBall)
    {
        otherBall.GetComponent<BallModifier>().GetBallText().SetActive(true);
        stackedBalls.Remove(otherBall);
    }
    public Transform GetBallToFillTheGap()
    {
        //Debug.Log("stackedBalls.Count: " + stackedBalls.Count);
        //_gapsCounter = 0;
        return stackedBalls.Count == 1 ? null : currentInfinitePlayerBall.ballObject.transform;
    }
    
    public Transform RemoveBallToFillTheGap()
    {
        var maxBallNum = intNums.IndexOf(value);
        if (character != '\0')
        {
            Debug.Log("HasCharacterValue: " + stackedBalls.Count);
            //if(stackedBalls.Count >= 1)
                maxBallNum = stackedBalls.Count;
        }
        if (maxBallNum > 0)
            maxBallNum -= 1;
        //Debug.Log("gapFillerIndex: " + index);
        //var index = infinitePlayerBalls.IndexOf(currentInfinitePlayerBall);
        if (_gapsCounter > maxBallNum) return null;
        if (stackedBalls.Count == 1) return null;
        //var obj = infinitePlayerBalls[_gapsCounter].ballObject;
        //Debug.Log("index: " + (stackedBalls.Count - 1) + " Value: " + stackedBalls[stackedBalls.Count - 1].GetComponent<BallModifier>().mainValue);
        var obj = stackedBalls[stackedBalls.Count - 1].gameObject;
        var ballNo = obj.GetComponent<BallModifier>().mainValue;
        Debug.Log("BallNo: " + ballNo);
        var index = intNums.IndexOf(ballNo);
        _gapFillerMatIndex = index;
        Debug.Log("index: " + index);
        //index -= _gapsCounter;
        //if (_gapsCounter < maxBallNum)
        _gapsCounter++;
        //Debug.Log("gapFillerIndex: " + (_gapsCounter));
        //Debug.Log("_gapsCounter: " + _gapsCounter);
        //Debug.Log("gapFillerLimit: " + gapFillerLimit);
        //var obj = infinitePlayerBalls[index].ballObject;
        var tempBall = infinitePlayerBalls[index].ballObject;
        if (obj != null) { 
            /*obj.transform.parent = null;
            obj.tag = "wasted";
            Destroy(obj.GetComponent<Rigidbody>());
            RemoveBallFromStack(obj.transform);*/
            tempBall = Instantiate(infinitePlayerBalls[index].ballObject, infinitePlayerBalls[index].ballObject.transform);
            //Debug.Log(tempBall.GetComponent<BallModifier>().mainValue);
            tempBall.tag = "wasted";
            tempBall.layer = 25;
            tempBall.transform.localPosition = Vector3.zero;
            tempBall.transform.localRotation = Quaternion.identity;
            tempBall.transform.parent = null;
            tempBall.transform.GetComponent<Collider>().enabled = false;
            tempBall.SetActive(true);
            stackedBalls[stackedBalls.Count - 1].gameObject.SetActive(false);
            stackedBalls.RemoveAt(stackedBalls.Count - 1);
            //stackedBalls.RemoveAt(_gapsCounter);
            //Destroy(tempBall.GetComponent<Rigidbody>());
        }
        SoundController.Instance.PlayBallUnMergeSound();
        return tempBall.transform;
    }
    public void ResetTheGapCounter()
    {
        _gapsCounter = 0;
    }
    public Material GetGapFillerMaterial()
    {
        return gapFillerMaterials[_gapFillerMatIndex];
    }
}
[Serializable]
public class InfinitePlayerBall
{
    public GameObject ballObject;
    public TextMeshPro numberTxt;
    public GameObject mergeEffect;
    public GameObject ballTrail;
}