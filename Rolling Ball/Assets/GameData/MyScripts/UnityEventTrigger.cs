using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class UnityEventTrigger : MonoBehaviour
{
    private Text _coinsText;
    public UnityEvent onEnableEvent = new UnityEvent();
    public UnityEvent onDisableEvent = new UnityEvent();
    [Space]
    public UnityEvent customEvent = new UnityEvent();
    [Space]
    public UnityEvent fadeEvent = new UnityEvent();
    private bool isCalled;
    private void OnEnable()
    {
        onEnableEvent?.Invoke();
    }
    private void OnDisable()
    {
        onDisableEvent?.Invoke();
    }
    public void InvokeCustomEvent()
    {
        customEvent?.Invoke();
    }
    public void FadeIn()
    {
            ImageFader.Instance.FadeIn();
    }
    public void FadeOut()
    {
        if (ImageFader.Instance.gameObject.activeSelf)
        {
            ImageFader.Instance.FadeOut();
        }
    }


    public void ShowFadeMenu()
    {
        ImageFader.Instance.FadeInOut(1f);
    }
    public void ShowFadeMenu(float delay)
    {
        ImageFader.Instance.FadeInOut(delay);
        //Invoke(nameof(EnablePlayer),delay);
        Invoke(nameof(InvokeEvent),delay);
    }
    public void InvokeEvent()
    {
        if (isCalled) return;
        isCalled = true;
        fadeEvent?.Invoke();
    }
}
