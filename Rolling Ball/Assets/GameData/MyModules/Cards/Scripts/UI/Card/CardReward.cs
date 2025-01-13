using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
public class CardReward : MonoBehaviour
{
    [SerializeField] private Card[] cards;
    [SerializeField] public GameObject nextButton;
    private List<string> rewardIndex;
    private void Start()
    {
        rewardIndex = new List<string>();
        for (var i = 0; i < cards.Length; i++)
        {
            var index = i;
            cards[i].GetComponent<Button>().onClick.AddListener(() =>
            {
                OnCardButtonClick(index);
            });
            SearchForReward(i);
        }
    }
    private void OnEnable()
    {
        rewardIndex = new List<string>();
        for (var i = 0; i < cards.Length; i++)
        {
            var index = i;
            cards[i].GetComponent<Button>().onClick.AddListener(() =>
            {
                OnCardButtonClick(index);
            });
            SearchForReward(i);
        }
    }

    //this variable is used for rewarded video Purposes
    private int clickedButtonIndex = -1;
    private void OnCardButtonClick(int i)
    {
        
        if(cards[i].IsClicked())
            return;
        
        clickedButtonIndex = i;
        if (cards[i].IsLocked())
        {
            Callbacks.ADType = "CardReward";
            AdsCaller.Instance.ShowRewardedAd();
        }
        else
        {
            //DisableItween();
            StartCoroutine(FlipTheCard(cards[i]));
        }
    }
    public void Reward()
    {
        StartCoroutine(FlipTheCard(cards[clickedButtonIndex]));
    }
    private IEnumerator FlipTheCard(Card t)
    {
        yield return null;
        t.SetIsClicked(true);
        //Lock other Buttons so User is unable to unlock those cards without watching video ad
        foreach (var t1 in cards)
        {
            if (!t1.GetComponent<Card>().IsClicked())
            {
                t1.SetIsLocked(true);
            }
        }
        t.StartShaking();
        yield return new WaitForSeconds(2);
        t.BlastEffect();
        t.StopShaking();
        t.SetReward();
        //Show Rewarded Ad image on other cards which are not clicked yet
        foreach (var t1 in cards)
        {
            if (!t1.GetComponent<Card>().IsClicked())
            {
                t1.GetComponent<Card>().ShowRewardedAdPopup();
            }
        }
        nextButton.SetActive(true);
    }
    private void DisableItween()
    {
        foreach (var t in cards)
        {
            var tweens = t.GetComponents<iTween>();
            DestroyItween(tweens);
            
            t.transform.rotation = Quaternion.identity;
        }
    }
    private void DestroyItween(iTween[] tweens)
    {
        foreach (var t in tweens)
        {
            Destroy(t);
        }
    }
    private void SearchForReward(int cardIndex)
    {
        var index = 0;
        var randomNo = Random.Range(0, 4);
        var type = "";
        var typeIndex = "";
        /*var vehicleIndex = Random.Range(0, GamePlayManager.Instance.garageInventory.GetTotalNoOfVehicleStores());
        switch (randomNo)
        {
            case 0:
                type = ShopType.Colors.ToString();
                index = RewardSearch(GamePlayManager.Instance.garageInventory.GetVehicleStore(vehicleIndex).GetShop()[0].locked);
                typeIndex = type + index;
                break;
            case 1:
                type = ShopType.Tires.ToString();
                index = RewardSearch(GamePlayManager.Instance.garageInventory.GetVehicleStore(vehicleIndex).GetShop()[2].locked);
                typeIndex = type + index;
                break;
            case 2:
                type = "Cash";
                index = Random.Range(0, PlayerPrefsHandler.CashRewards.Length);
                typeIndex = type + index;
                break;
        }*/
        //if this reward is already selected please select another reward
        if (rewardIndex.Contains(typeIndex))
        {
            SearchForReward(cardIndex);
            return;
        }
        rewardIndex.Add(typeIndex);
        //-1 means that the selected array has nothing to unlock in it
        //so randomly select another array
        if (index == -1)
        {
            SearchForReward(cardIndex);
            return;
        }
        /*if(type != "")
            cards[cardIndex].GetComponent<Card>().SetCardReward(vehicleIndex, index, type);*/
    }
    private int RewardSearch(List<bool> flagList)
    {
        var count = 0;
        //if all the element of array is true that means this
        //array is nothing which needs to be unlocked
        foreach (var t in flagList)
        {
            if (t)
                count++;
        }
        //returning -1 indicates that this array has nothing to be unlocked
        if (count == flagList.Count)
            return -1;
        var i = Random.Range(0, flagList.Count);
        //if random value is true in the array that means that item is already unlocked
        //so you need to unlock another locked item.
        if (flagList[i])
        {
            RewardSearch(flagList);
            return -1;
        }
        else
        {
            return i;
        }
    }
    private int RewardSearch(bool[] array)
    {
        var count = 0;
        //if all the element of array is true that means this
        //array is nothing which needs to be unlocked
        foreach (var t in array)
        {
            if (t)
                count++;
        }
        //returning -1 indicates that this array has nothing to be unlocked
        if (count == array.Length)
            return -1;
        var i = Random.Range(0, array.Length);
        //if random value is true in the array that means that item is already unlocked
        //so you need to unlock another locked item.
        if (array[i])
        {
            RewardSearch(array);
            return -1;
        }
        else
        {
            return i;
        }
    }
}