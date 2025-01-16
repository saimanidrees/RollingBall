using UnityEngine;
public class BallLivesUI : MonoBehaviour
{
    [SerializeField] private GameObject[] balls, crossImages;
    public void Die(int index)
    {
        if(index >= balls.Length) return;
        balls[index].SetActive(false);
        crossImages[index].SetActive(true);
    }
}