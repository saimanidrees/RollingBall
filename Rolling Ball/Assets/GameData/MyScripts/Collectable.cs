using UnityEngine;
public class Collectable : MonoBehaviour
{
    [SerializeField] private int coinsReward = 5;
    [SerializeField] private GameObject currencyParticles;
    [SerializeField] private GameObject collectionEffect;
    [SerializeField] private float delayToDisable = 1f;
    public void Collect()
    {
        SoundController.Instance.PlayBuySound();
        CurrencyCounter.Instance.SetCoinsReward(coinsReward);
        currencyParticles.SetActive(false);
        collectionEffect.SetActive(true);
        Invoke(nameof(DisableCollectable), delayToDisable);
    }
    private void DisableCollectable()
    {
        gameObject.SetActive(false);
    }
}