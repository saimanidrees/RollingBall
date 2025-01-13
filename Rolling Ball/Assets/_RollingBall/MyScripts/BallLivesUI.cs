using UnityEngine;
public class BallLivesUI : MonoBehaviour
{
    [SerializeField] private GameObject[] balls, crossImages;
    [SerializeField] private GameObject cam;
    public void Die(int index)
    {
        //cam.SetActive(false);
        if(index >= balls.Length) return;
        balls[index].SetActive(false);
        crossImages[index].SetActive(true);
        //cam.SetActive(true);
        Debug.Log("Die");
    }
}