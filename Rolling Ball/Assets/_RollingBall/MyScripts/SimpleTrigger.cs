using UnityEngine;
using UnityEngine.Events;
namespace _RollingBall.MyScripts
{
    public class SimpleTrigger : MonoBehaviour
    {
        [SerializeField] private string tagToDetect = PlayerPrefsHandler.BallTag;
        [SerializeField] private bool disableOnTriggering = true;
        [SerializeField] private UnityEvent onTriggers;
        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.tag.Equals(tagToDetect)) return;
            gameObject.SetActive(!disableOnTriggering);
            onTriggers?.Invoke();
        }
    }
}