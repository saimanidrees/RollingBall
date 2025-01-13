using UnityEngine;
public class DisableObject : MonoBehaviour
{
    [SerializeField] private float delay = 2f;
    private void OnEnable()
    {
        Invoke(nameof(Disable), delay);
    }
    private void Disable()
    {
        gameObject.SetActive(false);
    }
}