using GameData.MyScripts;
using UnityEngine;
public class InfinityManager : MonoBehaviour
{
    #region Singleton
    public static InfinityManager instance;    
    private void Awake()
    {
        if (!instance) instance = this;
        else if (instance != this) Destroy(this.gameObject);
    }

    #endregion
    public GameObject mainBall;
    public GameObject patchPrefab;
    public PatchHandler currentPatch;
    public PatchHandler nextPatch;
    public PatchHandler previousPatch;
    public float zAxixLen = 55;
    public int valueOfBallOnTrigger = 2;
    public  char characterAfterTrigger = '\0';
    private GameObject newPatch;
    private int patchesCounter = 0;
    private int priorityLimit = 4;

    private int _count = 0;

    public void InstantiateNewPatch()
    {
        zAxixLen = nextPatch.patchMesh.transform.localScale.z;
        zAxixLen = (zAxixLen / 2) * 10;
        zAxixLen += nextPatch.transform.position.z;
        newPatch = Instantiate(patchPrefab);
        //Debug.Log("zAxisLen:"+zAxixLen);
        newPatch.transform.position = new Vector3(0, 0, zAxixLen);
        currentPatch = nextPatch;
        nextPatch = newPatch.GetComponent<PatchHandler>();
        nextPatch.SetPosition();

        if (_count >= 2)
        {
            _count = 0;
            /// get ball values dynamically
            // nextPatch.WallOn(valueOfBallOnTrigger, characterAfterTrigger);
            const int LIMIT = 2;
            var r = Random.Range(0, LIMIT);
            if (r == 2)
            {
                //nextPatch.plusMinus.SetActive(true);
            }
            else
            {
                nextPatch.WallOn(valueOfBallOnTrigger, characterAfterTrigger);
                nextPatch.wallOn = true;
            }
        }
        _count++;
    }
    public void GeneratePatch() 
    {
        zAxixLen = nextPatch.patchMesh.transform.localScale.z;
        zAxixLen = (zAxixLen / 2) * 10;
        zAxixLen += nextPatch.transform.position.z;
        if(!previousPatch)
            newPatch = Instantiate(patchPrefab);
        else
        {
            newPatch = previousPatch.gameObject;
            previousPatch.ResetPatch();
            //newPatch.SetActive(true);
        }
        //Debug.Log("zAxisLen:"+zAxixLen);
        newPatch.transform.position = new Vector3(0, 0, zAxixLen);
        previousPatch = currentPatch;
        currentPatch = nextPatch;
        nextPatch = newPatch.GetComponent<PatchHandler>();
        nextPatch.SetPosition();

        if (_count >= 2) {
            _count = 0;
            /// get ball values dynamically
            // nextPatch.WallOn(valueOfBallOnTrigger, characterAfterTrigger);
            nextPatch.WallOn(valueOfBallOnTrigger, characterAfterTrigger);
            nextPatch.wallOn = true;
        }
        _count++;
    }
    public void SetBallDetails(int value, char ch) 
    {
        valueOfBallOnTrigger = value;
        characterAfterTrigger = ch;
    }
    public void SetPatchesCounter()
    {
        patchesCounter++;
        if (patchesCounter % 2 == 0)
        {
            priorityLimit--;
            priorityLimit = Mathf.Clamp(priorityLimit, 2, 4);
            var r = Random.Range(0, 3);
            nextPatch.EnableVideoPoint(r == 0 ? "BallSkin" : "Magnet");
            GamePlayManager.Instance.SendInfiniteProgressionEvent(patchesCounter);
        }
    }
    public int GetPatchesCounter()
    {
        return patchesCounter;
    }
    public void SetPriorityLimit(int limit)
    {
        priorityLimit = limit;
    }
    public int GetPriorityLimit()
    {
        return priorityLimit;
    }
}