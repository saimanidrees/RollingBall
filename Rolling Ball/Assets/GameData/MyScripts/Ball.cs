using UnityEngine;
public class Ball : MonoBehaviour
{
    [SerializeField] private int number = 2;
    private bool isMerged = false;
    public int GetNumber()
    {
        return number;
    }
    public void DestroyBall()
    {
        gameObject.SetActive(false);
    }
    public bool IsBallMerged()
    {
        return isMerged;
    }
    public void SetMergingFlag(bool flag)
    {
        isMerged = flag;
    }
    public void IncreaseNumber()
    {
        number *= 2;
    }
}