using System;
using GameData.MyScripts;
using TMPro;
using UnityEngine;
public class PlusMinus : MonoBehaviour
{
    [SerializeField] private GameObject ballToPlusMinus;
    [SerializeField] private TextMeshPro plusText, minusText;
    private int _no = 2; 
    private char _character;
    private void OnEnable()
    {
        GetPlayerValues();
        MergeInfinityBall.OnInfiniteMerge += GetPlayerValues;
    }
    private void OnDisable()
    {
        MergeInfinityBall.OnInfiniteMerge += GetPlayerValues;
    }
    public void UpgradePlayer()
    {
        gameObject.SetActive(false);
        var ball = GamePlayManager.Instance.currentPlayer.GetComponent<MergeInfinityBall>();
        var ballNo = ball.value;
        var character = ball.character;
        ballToPlusMinus.GetComponent<BallModifier>().SetSpecificStringValue(ballNo, character);
        ball.PushInStack(ballToPlusMinus);
        ballToPlusMinus.gameObject.SetActive(true);
    }
    public void DegradePlayer()
    {
        gameObject.SetActive(false);
        var ball = GamePlayManager.Instance.currentPlayer.GetComponent<MergeInfinityBall>();
        ball.PopFromStack();
    }
    private void GetPlayerValues()
    {
        var ball = GamePlayManager.Instance.currentPlayer.GetComponent<MergeInfinityBall>();
        _no = ball.value;
        _character = ball.character;
        var no = _no;
        var character = _character;
        if (_no == 2048) {
            no = 2;
            if (_character == '\0')
            {
                character = 'A';
            }
            else
            {
                character++;  
            }
        }
        else
        {
            no = _no * 2;
        }
        plusText.text = no + character.ToString();
        if (_no == 2)
        {
            if (_character == '\0')
            {
                // do nothing
            }
            else
            {
                no = 2048;
                if (_character == 'A') {
                    character = '\0';
                }
                else
                    character--;
            }
        }
        else
        {
            if (_character == '\0')
            {
                character = '\0';
            }
            no = _no / 2;
        }
        minusText.text = no + character.ToString();
    }
}