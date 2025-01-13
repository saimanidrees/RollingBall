using System.Collections;
using UnityEngine;
using UnityEngine.Events;
public class Pickable : MonoBehaviour
{
    [SerializeField] private Vector3 targetPosition, targetRotation, targetScale;
    [SerializeField] private UnityEvent onPick;
    [Space]
    [SerializeField] private Vector3 lifterTargetEulerAngle;
    [SerializeField] private Vector3 lifterTargetScale;
    public Vector3 GetLifterTargetEulerAngle()
    {
        return lifterTargetEulerAngle;
    }
    public Vector3 GetLifterTargetScale()
    {
        return lifterTargetScale;
    }
    public void SetPickablePositionInLifter(bool instantPlacement = false)
    {
        if (instantPlacement)
        {
            transform.localPosition = targetPosition;
            transform.localRotation = Quaternion.Euler(targetRotation);
            transform.localScale = targetScale;
        }
        else
        {
            StartCoroutine(SetPickableObjectPosition());
        }
    }
    private IEnumerator SetPickableObjectPosition()
    {
        var time = 0f;
        const float duration = 1f;
        while (time < duration)
        {
            time += Time.deltaTime / duration;
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, time);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(targetRotation), time);
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, time);
            yield return null;
        }
        onPick.Invoke();
    }
}