using System;
using System.Collections;
using System.Collections.Generic;
using GameData.MyScripts;
using UnityEngine;
using Random = UnityEngine.Random;

public class PatchHandler : MonoBehaviour
{
    public GameObject patchMesh;
    public GameObject ConesParent;
    public GameObject ballsParent;
    public GameObject wallBreak;
    public GameObject Rails;

    public Transform nextPatchPosition;
    public Transform start;
    public Transform end;

    public List<Transform> placmentPoints;
    
    // x-> is the scale of the patch y-> is the max index placement position allowed    
    public Vector2[] objectsRelatedToScale;
    public Vector2[] objectsSetLimit;
    public GameObject conePrefab;
    public GameObject ballPrefab;
    [SerializeField] private GameObject ballSkinPoint, magnetPoint, fillTheGapTrigger;
    [SerializeField] private Transform gapFiller;
    [SerializeField] private AnimationCurve animationCurve;
    [SerializeField] private int directionFactor = 1;
    [SerializeField] private float heightFactor = 0.5f;
    private int _gapFillerIndex = 0, _patchIndex = 0, _gapFillerLimit = 0;
    public bool wallOn;
   /* public int v
    public char alphabet;*/
   private int _uptoIndexes = 24;
    
    private void Start()
    {
        var index = Random.Range(0, 7);
        if (index > 5) index = 5;
        /*var randScale = Random.Range(5, 11);
        var indexForPatch = randScale % 5;
        if (randScale == 10) indexForPatch = 5;*/
        _patchIndex = index;
        ScalePatch(index);
        SetObjects(index);
        SetGapFillerLimit();
    }
    public void ResetPatch()
    {
        var index = Random.Range(0, 7);
        if (index > 5) index = 5;
        ScalePatch(index);
        SetObjects(index);
    }
    public void SetPosition() {
        var z = (patchMesh.transform.localScale.z / 2) * 10;
        var position = gameObject.transform.position;
        position = new Vector3(position.x, position.y, position.z + z);
        gameObject.transform.position = position;
        //Debug.Log("transform:" + gameObject.transform.position.z);
    }

    private void ScalePatch(int index) {
        Debug.Log("PatchIndex: " + index);
        var localScale = patchMesh.transform.localScale;
        localScale =new Vector3(localScale.x, localScale.y,objectsRelatedToScale[index].x);
        patchMesh.transform.localScale = localScale;
        _uptoIndexes = (int)objectsRelatedToScale[index].y;
        if (Math.Abs(objectsRelatedToScale[index].x - 9f) < 0.1f)
            fillTheGapTrigger.SetActive(false);
        else
            fillTheGapTrigger.SetActive(true);
    }

    private void SetObjects(int index) {
        var totalObjects = Random.Range((int)objectsSetLimit[index].x, (int)objectsSetLimit[index].y);
        var placementIndexesSet = new List<int>();
        var priorityLimit = InfinityManager.instance.GetPriorityLimit();
        for (var i = 0; i < totalObjects; i++) {
            var prefab = Random.Range(0, priorityLimit);
            var indexToSet = Random.Range(0, _uptoIndexes);
            if (!placementIndexesSet.Contains(indexToSet))
            {
                placementIndexesSet.Add(indexToSet);
                if (prefab == 0)//Cone prefab
                {
                   var cone= Instantiate(conePrefab, placmentPoints[indexToSet]);
                    cone.transform.parent = ConesParent.transform;
                }
                else
                { //Ball Prefab
                    var ball = Instantiate(ballPrefab, placmentPoints[indexToSet]);
                    ball.transform.parent = ballsParent.transform;
                    ball.GetComponent<BallModifier>().mainValue = InfinityManager.instance.valueOfBallOnTrigger;
                    ball.GetComponent<BallModifier>().character = InfinityManager.instance.characterAfterTrigger;
                }
            }
            else {
                i -= 1;
            }
        }
        /*for (var i = 0; i < totalObjects; i++) {
                var prefab = Random.Range(0, 2);
                var indexToSet = Random.Range(0, _uptoIndexes);
                if (!placementIndexesSet.Contains(indexToSet))
                {
                    placementIndexesSet.Add(indexToSet);
                    if (prefab == 0)//Cone prefab
                    {
                        var cone= Instantiate(conePrefab, placmentPoints[indexToSet]);
                        cone.transform.parent = ConesParent.transform;
                    }
                    else
                    { //Ball Prefab
                        var ball = Instantiate(ballPrefab, placmentPoints[indexToSet]);
                        ball.transform.parent = ballsParent.transform;
                        ball.GetComponent<BallModifier>().mainValue = InfinityManager.instance.valueOfBallOnTrigger;
                        ball.GetComponent<BallModifier>().character = InfinityManager.instance.characterAfterTrigger;
                    }
                }
                else {
                    i -= 1;
                }
                }*/
    }
    public void WallOn(float valueOnWall, char alphabet) {
        wallBreak.GetComponent<WallBreak>().value = valueOnWall;
        wallBreak.GetComponent<WallBreak>().alphabet = alphabet;
        wallBreak.SetActive(true);
    }
    public void EnableVideoPoint(string point)
    {
        if (point == "Magnet")
        {
            magnetPoint.SetActive(true);
            ballSkinPoint.SetActive(false);
        }
        else
        {
            magnetPoint.SetActive(false);
            ballSkinPoint.SetActive(true);
        }
    }
    public void FillTheGap()
    {
        if(_gapFillerLimit == 0) return;
        //Debug.Log("Fill The Gap");
        var player = GamePlayManager.Instance.currentPlayer.GetComponent<MergeInfinityBall>();
        var ball = player.GetBallToFillTheGap();
        if(!ball) return;
        StartCoroutine(DelayToFillTheGap());
    }
    private IEnumerator DelayToFillTheGap()
    {
        //Debug.Log("gapFillerLimit: " + _gapFillerLimit);
        var player = GamePlayManager.Instance.currentPlayer.GetComponent<MergeInfinityBall>();
        var ball = player.RemoveBallToFillTheGap();
        if (!ball)
        {
            /*ball = player.GetBallToFillTheGap();
            if (!ball)
            {
                Debug.LogError("No ball to fill");
                GamePlayManager.Instance.currentPlayer.GetComponent<MergeInfinityBall>().ResetTheGapCounter();
                yield break;
            }*/
            Debug.LogError("No ball to fill");
            GamePlayManager.Instance.currentPlayer.GetComponent<MergeInfinityBall>().ResetTheGapCounter();
            yield break;
        }
        //Debug.Log("BallName: " + ball.gameObject.name);
        var time = 0f;
        const float DURATION = 1f;
        while (time < DURATION / 2f)
        {
            /*time += Time.deltaTime / DURATION;
            ball.transform.position = Vector3.Lerp(ball.transform.position, gapFiller.transform.position, time);*/
            time += Time.deltaTime;
            var linearT = time / DURATION;
                var heightT = animationCurve.Evaluate(linearT);
                var height = Mathf.Lerp(0f, heightFactor, heightT);
                ball.transform.position = Vector3.Lerp(ball.transform.position, gapFiller.GetChild(_gapFillerIndex).position, linearT) + new Vector3(0, height * directionFactor, 0f);
            yield return null;
        }
        ball.gameObject.SetActive(false);
        var filler = gapFiller.GetChild(_gapFillerIndex).transform;
        filler.GetChild(0).GetComponent<MeshRenderer>().material = player.GetGapFillerMaterial();
        filler.gameObject.SetActive(true);
        _gapFillerIndex++;
        if (_gapFillerIndex > _gapFillerLimit)
        {
            _gapFillerIndex = 0;
            _gapFillerLimit = 0;
            player.ResetTheGapCounter();
            yield break;
        }
        FillTheGap();
    }
    private void SetGapFillerLimit()
    {
        _gapFillerLimit = _patchIndex switch
        {
            0 => 5,
            1 => 4,
            2 => 3,
            3 => 2,
            4 => 1,
            5 => 0,
            _ => _gapFillerLimit
        };
    }
    
    [Serializable]
    public class Points
    {
        public Transform point;
        public bool isObjectPlaced = false;
    }
}