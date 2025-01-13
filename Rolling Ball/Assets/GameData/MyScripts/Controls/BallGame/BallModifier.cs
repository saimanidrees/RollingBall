using System.Collections.Generic;
using GameData.MyScripts;
using UnityEngine;
public class BallModifier : MonoBehaviour
{
    [SerializeField] private bool selectBallNumberRandomly = true;
    public int mainValue;
    public char character ;
    public List<int> intNums;
    [SerializeField] private InfinitePlayerBall currentBall;
    [SerializeField] private List<InfinitePlayerBall> infinitePlayerBalls = new List<InfinitePlayerBall>();
    private readonly string[] _valuesArr = { "2", "4", "8", "16", "32", "64", "128", "256", "512", "1024", "2048" };
    //float[] ballScaleData = { 1, 1.2f, 1.4f, 1.6f, 1.8f, 2.0f, 2.2f, 2.4f, 2.6f, 2.8f, 3.0f};
    //public  Color[] colors = { Color.red,Color.gray,Color.green,Color.blue,Color.cyan,Color.magenta};
    //public Color[] colors;
    //public TextMeshPro valueTxt;
    //public GameObject ballMesh;
    //Material baseMat;
    private void Start()
    {
        if(selectBallNumberRandomly)
            SetStringValueRandom(mainValue, character);
    }
    // Start is called before the first frame update
    private void UpdateTextOnBall(string txt) 
    {
        currentBall.numberTxt.text = txt.ToString();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag(PlayerPrefsHandler.Ball)) return;
        var ball = collision.gameObject.GetComponent<BallModifier>();
        if (!ball) return;
        if (ball.mainValue != mainValue) return;
        var ch = collision.gameObject.GetComponent<BallModifier>().character;
        if (!ch.Equals(character)) return;
        if (mainValue == 2048) return;
        collision.gameObject.SetActive(false);
        SetStringValue(mainValue * 2, character);
    }
    private void SetStringValueRandom(int val,char alpha) {
        if (val >= 2048) {
            val = 2;
            if (character == '\0')
            {
                character = 'A';
            }
            else
            {
                character++;  
            }
        }
        if (intNums.Contains(val)) {
            var index = intNums.IndexOf(val);
            if (val == 2) { index++; }
            var value= Random.Range(index - 1, index + 2);
            //Debug.Log("Index :"+index+" value"+val);
            if (value >= 0 && value <= 11)
            {
                currentBall.ballObject.SetActive(false);
                currentBall.ballObject = infinitePlayerBalls[value].ballObject;
                currentBall.numberTxt = infinitePlayerBalls[value].numberTxt;
                currentBall.ballObject.SetActive(true);
                UpdateTextOnBall(_valuesArr[value]+character);
                mainValue = System.Convert.ToInt32(_valuesArr[value].ToString());
            }
            else {
                if (value < 0)
                    SetStringValue(2,character);
            }
        }
        if (character != '\0')
            currentBall.ballObject.transform.localScale = new Vector3(2f, 2f, 2f);
    }
    private void SetStringValue(int val, char alpha) {
        if (intNums.Contains(val))
        {
            var index = intNums.IndexOf(val);
            currentBall.ballObject.SetActive(false);
            currentBall.ballObject = infinitePlayerBalls[index].ballObject;
            currentBall.numberTxt = infinitePlayerBalls[index].numberTxt;
            currentBall.ballObject.SetActive(true);
            if (character != '\0')
                currentBall.ballObject.transform.localScale = new Vector3(2f, 2f, 2f);
            UpdateTextOnBall(_valuesArr[index] + character);
            mainValue = System.Convert.ToInt32(_valuesArr[index].ToString());
        }
    }
    public void SetSpecificStringValue(int val,char alpha)
    {
        mainValue = val;
        character = alpha;
        var index = intNums.IndexOf(val);
        currentBall.ballObject = infinitePlayerBalls[index].ballObject;
        currentBall.numberTxt = infinitePlayerBalls[index].numberTxt;
        if (character != '\0')
            currentBall.ballObject.transform.localScale = new Vector3(2f, 2f, 2f);
        currentBall.ballObject.SetActive(true);
        UpdateTextOnBall(val + alpha.ToString());
    }
    public GameObject GetBallText()
    {
        return currentBall.numberTxt.gameObject;
    }
}