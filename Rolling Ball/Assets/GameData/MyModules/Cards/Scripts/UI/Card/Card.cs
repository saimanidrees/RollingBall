using UnityEngine;
using UnityEngine.UI;
public class Card : MonoBehaviour
{
    [SerializeField] private int vehicleIndex;
    [SerializeField] private int index;
    [SerializeField] private string rewardType;
    [SerializeField] private Image rewardIcon , questionIcon , cardImage , rewardedAdBg;
    [SerializeField] private Sprite cardOpen;
    [SerializeField] private GameObject explosion;
    [SerializeField] private Sprite cashIcon;
    private bool isLocked;
    private bool isClicked;

    private void Start()
    {
        isLocked = false;
    }
    public void SetCardReward(int categoryIndex, int i  , string type)
    {
        vehicleIndex = categoryIndex;
        rewardType = type;
        index = i;
        Debug.Log("rewardType:"+rewardType +":::::index:"+index);
       /*switch (rewardType)
        {
            case "Colors":
                var colorSprites = GamePlayManager.Instance.garageInventory.GetVehicleStore(categoryIndex).GetShop()[0].sprite;
                rewardIcon.sprite = colorSprites[index];
                break;
            case "Tires":
                var tiresSprites = GamePlayManager.Instance.garageInventory.GetVehicleStore(categoryIndex).GetShop()[2].sprite;
                rewardIcon.sprite = tiresSprites[index];
                break;
            case "Cash":
                //Sprite[] shoeSprites = GameplayManager.instance.dogShop.GetShop()[ShopIndex.shoeShop].sprite;
                rewardIcon.sprite = cashIcon;
                break;
        }*/
    }
    public void SetReward()
    {
        rewardedAdBg.gameObject.SetActive(false);
        rewardIcon.gameObject.SetActive(true);
        questionIcon.gameObject.SetActive(false);
        cardImage.sprite = cardOpen;
        AssignReward();
    }
    private void AssignReward()
    {
        /*switch (rewardType)
        {
            case "Colors":
                PlayerPrefsHandler.UnlockVehicleColor(vehicleIndex, index);
                //PlayerPrefsHandler.SetCurrentVehicleColorNo(vehicleIndex, index);
                break;
            case "Tires":
                PlayerPrefsHandler.UnlockVehicleTire(vehicleIndex, index);
                break;
            case "Cash":
                //Debug.Log("CashReward: " + PlayerPrefsHandler.CashRewards[index]);
                CurrencyCounter.Instance.SetCashReward(PlayerPrefsHandler.CashRewards[index]);
                break;
        }*/
    }
    public void StartShaking()
    {
        //GetComponent<ITweenMagic>().PlayForwardRotation();
        GetComponent<Animator>().enabled = true;
    }
    public void StopShaking()
    {
        GetComponent<Animator>().enabled = false;
        transform.localEulerAngles = Vector3.zero;
    }
    public void BlastEffect()
    {
        Destroy(GetComponent<iTween>());
        transform.rotation = Quaternion.identity;
        explosion.SetActive(true);
    }
    public void SetIsClicked(bool value)
    {
        isClicked = value;
    }
    public bool IsClicked()
    {
        return isClicked;
    }
    public void ShowRewardedAdPopup()
    {
        rewardedAdBg.gameObject.SetActive(true);
    }
    public void SetIsLocked(bool value)
    {
        isLocked = value;
    }
    public bool IsLocked()
    {
        return isLocked;
    } 
}