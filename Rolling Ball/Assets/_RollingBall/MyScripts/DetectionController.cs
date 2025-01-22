using DG.Tweening;
using UnityEngine;
namespace _RollingBall.MyScripts
{
    public class DetectionController : MonoBehaviour
    {
        private IMovable _movable;
        private void OnCollisionEnter(Collision collision)
        {
            switch (collision.gameObject.tag)
            {
                case PlayerPrefsHandler.PendulumTag:
                    _movable ??= GetComponent<IMovable>();
                    _movable.Push(collision.transform.up * 500f);
                    SoundController.Instance.PlayRollingBallHitSound();
                    GamePlayManager.Instance.vibrationManager.TapPeekVibrate();
                    break;
                case PlayerPrefsHandler.PendulumSingleSideTag:
                    _movable ??= GetComponent<IMovable>();
                    _movable.MinimumDrag(true);
                    _movable.Push(collision.transform.up * 1000f);
                    SoundController.Instance.PlayRollingBallHitSound();
                    GamePlayManager.Instance.vibrationManager.TapPeekVibrate();
                    break;
                case PlayerPrefsHandler.MovingPlatformTag:
                    transform.SetParent(collision.transform);
                    GamePlayManager.Instance.vibrationManager.TapPeekVibrate();
                    break;
                case PlayerPrefsHandler.PropTag:
                    SoundController.Instance.PlayRollingBallHitSound();
                    GamePlayManager.Instance.vibrationManager.TapPeekVibrate();
                    break;
            }
        }
        private void OnCollisionStay(Collision collision)
        {
            if (collision.gameObject.tag.Equals(PlayerPrefsHandler.CirclePlatformTag))
            {
                transform.parent = collision.transform;
            }
        }
        private void OnCollisionExit(Collision collision)
        {
            switch (collision.gameObject.tag)
            {
                case PlayerPrefsHandler.CirclePlatformTag:
                    transform.parent = null;
                    break;
                case PlayerPrefsHandler.MovingPlatformTag:
                    transform.SetParent(null);
                    break;
                case PlayerPrefsHandler.PlatformTag:
                    _movable ??= GetComponent<IMovable>();
                    _movable.SetBallLastPos(transform.position);
                    break;
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            switch (other.gameObject.tag)
            {
                case PlayerPrefsHandler.ReverseViewTriggerTag:
                    GamePlayManager.Instance.GetCameraController().UnPauseTheAlignment();
                    break;
                case PlayerPrefsHandler.NonReverseViewTriggerTag:
                    GamePlayManager.Instance.GetCameraController().PauseTheAlignmentOnly();
                    break;
                case PlayerPrefsHandler.NonTopViewTriggerTag:
                    break;
                case PlayerPrefsHandler.TopViewTriggerTag:
                    break;
                case PlayerPrefsHandler.TopViewTrigger2Tag:
                    break;
                case PlayerPrefsHandler.CollectableTag:
                    other.tag = PlayerPrefsHandler.NoneTag;
                    other.transform.GetChild(0).gameObject.SetActive(false);
                    other.transform.GetChild(1).gameObject.SetActive(true);
                    SoundController.Instance.PlayRollingBallCoinSound();
                    GamePlayManager.Instance.vibrationManager.TapPeekVibrate();
                    break;
                case PlayerPrefsHandler.NormalDragTag:
                    _movable ??= GetComponent<IMovable>();
                    _movable.MinimumDrag(false);
                    break;
                case PlayerPrefsHandler.CameraViewTriggerTag:
                    break;
                case PlayerPrefsHandler.OppositeGravityTriggerTag:
                    _movable ??= GetComponent<IMovable>();
                    _movable.FlyUp(true);
                    GamePlayManager.Instance.GetCameraController().PauseTheAlignmentOnly();
                    break;
                case PlayerPrefsHandler.LevelCompleteTriggerTag:
                    _movable ??= GetComponent<IMovable>();
                    _movable.AllowMovement(false);
                    GamePlayManager.Instance.LevelComplete(3f);
                    break;
                case PlayerPrefsHandler.ActivateReCenteringTag:
                    break;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            switch (other.gameObject.tag)
            {
                case PlayerPrefsHandler.OppositeGravityTriggerTag:
                    _movable ??= GetComponent<IMovable>();
                    _movable.FlyUp(false);
                    GamePlayManager.Instance.GetCameraController().UnPauseTheAlignment();
                    break;
            }
        }
    }
}