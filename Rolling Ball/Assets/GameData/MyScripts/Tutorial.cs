using System;
using System.Collections.Generic;
using GameData.MyScripts;
using UnityEngine;
using UnityEngine.UI;
public class Tutorial : MonoBehaviour
{
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private Controls controls;
    [SerializeField] private List<TutorialParams> tutorialsParams;
    private Transform hand, tutorialDescription;
    private TutorialType currentTutorial;
    private void Start()
    {
        hand = tutorialPanel.transform.Find("Hand");
        tutorialDescription = tutorialPanel.transform.Find("TutorialDescription");
    }
    public void ShowTutorial(string tutorialName)
    {
        if(PlayerPrefsHandler.GetBool(tutorialName)) return;
        currentTutorial = GetTutorialType(tutorialName);
        /*switch (currentTutorial)
        {
            case TutorialType.None:
                Debug.Log("No Tutorial to Show");
                return;
            case TutorialType.HoldToDrive:
                SetTutorialData(tutorialsParams[0]);
                controls.EnableSteeringMask(true);
                break;
            case TutorialType.SteerToTurn:
                SetTutorialData(tutorialsParams[1]);
                controls.EnableSteeringMask(true);
                break;
            case TutorialType.TapToGear:
                SetTutorialData(tutorialsParams[2]);
                controls.EnableGearMask(true);
                break;
            default:
                Debug.Log("No Tutorial to Show");
                return;
        }*/
    }
    private TutorialType GetTutorialType(string tutorialType)
    {
        return tutorialType switch
        {
            "HoldToDrive" => TutorialType.HoldToDrive,
            "SteerToTurn" => TutorialType.SteerToTurn,
            "TapToGear" => TutorialType.TapToGear,
            _ => TutorialType.None
        };
    }
    private void SetTutorialData(TutorialParams tutorialData)
    {
        Debug.Log("SetTutorialData: " + tutorialData.handAnimationName);
        if(!hand)
            hand = tutorialPanel.transform.Find("Hand");
        if(!tutorialDescription)
            tutorialDescription = tutorialPanel.transform.Find("TutorialDescription");
        var handRect = hand.GetComponent<RectTransform>();
        handRect.anchorMin = tutorialData.anchorMix;
        handRect.anchorMax = tutorialData.anchorMax;
        handRect.anchoredPosition = tutorialData.handPos;
        var descriptionRect = tutorialDescription.GetComponent<RectTransform>();
        descriptionRect.anchorMin = tutorialData.anchorMix;
        descriptionRect.anchorMax = tutorialData.anchorMax;
        descriptionRect.anchoredPosition = tutorialData.descriptionPos;
        tutorialDescription.Find("DescriptionText").GetComponent<Text>().text = tutorialData.descriptionString;
        tutorialPanel.SetActive(true);
        hand.GetComponent<Animator>().Play(tutorialData.handAnimationName);
    }
    public void CloseTutorial()
    {
        if(currentTutorial == TutorialType.None) return;
        PlayerPrefsHandler.SetBool(currentTutorial.ToString(), true);
        currentTutorial = TutorialType.None;
        tutorialPanel.SetActive(false);
    }
    [Serializable]
    public class TutorialParams
    {
        public Vector3 handPos, descriptionPos;
        public string handAnimationName, descriptionString;
        public Vector2 anchorMix, anchorMax;
    }
}
public enum TutorialType
{
    None,
    HoldToDrive,
    SteerToTurn,
    TapToGear
}