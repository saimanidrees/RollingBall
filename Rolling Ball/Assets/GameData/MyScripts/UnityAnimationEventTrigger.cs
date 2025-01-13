using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class UnityAnimationEventTrigger : MonoBehaviour
{
    [SerializeField] private UnityEvent[] animationEvents;
    public void InvokeAnimationEvent(int eventIndex)
    {
        animationEvents[eventIndex].Invoke();
    }
    public void CalculatePercentage()
    {
        StartCoroutine(DelayForCalculation());
    }
    private IEnumerator DelayForCalculation()
    {
        var animator = GetComponent<Animator>();
        var percentageText = transform.Find("PercentageText").GetComponent<Text>();
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.99f)
        {
            percentageText.text = (int)(animator.GetCurrentAnimatorStateInfo(0).normalizedTime * 100) + "%";
            yield return null;
        }
        percentageText.text = "100%";
    }
}