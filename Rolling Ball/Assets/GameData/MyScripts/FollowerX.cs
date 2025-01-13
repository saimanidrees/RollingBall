using UnityEngine;
public class FollowerX : MonoBehaviour
{
    public Transform target;
    public float minLimit = -5f;  
    public float maxLimit = 5f;
    private Vector3 _startPos;
    private Vector3 _lastTargetPos;
    private void Start() {
        _startPos = transform.localPosition;  
        _lastTargetPos = target.localPosition;
    }
    private void Update() {
        var targetPos = target.localPosition;
        if(CheckLimits(targetPos)) {
            var offsetPos = GetOffsetPosition(targetPos);
            FollowWithLerp(offsetPos);
        } else {
            LerpBackToStart();
        }
        _lastTargetPos = targetPos;
    }
    private bool CheckLimits(Vector3 pos) {
        return pos.x < minLimit || pos.x > maxLimit;
    }
    private Vector3 GetOffsetPosition(Vector3 pos) {

        var offset = 1f + transform.localScale.x;
        if(pos.x < 0 && _lastTargetPos.x >= pos.x)  
            pos.x += offset;
        else if(pos.x > 0 && _lastTargetPos.x <= pos.x)
            pos.x -= offset;
        _lastTargetPos = pos;
        return pos;
    }
    private void FollowWithLerp(Vector3 targetPos) {

        transform.localPosition = Vector3.Lerp(
            transform.localPosition, 
            targetPos, 
            Time.deltaTime * 5f);
    }
    private void LerpBackToStart() {

        transform.localPosition = Vector3.Lerp(
            transform.localPosition,
            _startPos,
            Time.deltaTime * 5f);
    }
}