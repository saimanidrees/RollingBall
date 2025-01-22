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
    public void Refill()
    {
        for (var i = 0; i < balls.Length; i++)
        {
            balls[i].SetActive(true);
            crossImages[i].SetActive(false);
        }
        balls[0].transform.parent.gameObject.SetActive(true);
    }
}