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
                    break;
                case PlayerPrefsHandler.PendulumSingleSideTag:
                    _movable ??= GetComponent<IMovable>();
                    _movable.MinimumDrag(true);
                    _movable.Push(collision.transform.up * 1000f);
                    SoundController.Instance.PlayRollingBallHitSound();
                    break;
                case PlayerPrefsHandler.MovingPlatformTag:
                    transform.SetParent(collision.transform);
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
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            switch (other.gameObject.tag)
            {
                case PlayerPrefsHandler.ReverseViewTriggerTag:
                    GamePlayManager.Instance.SetReverseViewCamera(9);
                    break;
                case PlayerPrefsHandler.NonReverseViewTriggerTag:
                    GamePlayManager.Instance.SetReverseViewCamera(11);
                    break;
                case PlayerPrefsHandler.NonTopViewTriggerTag:
                    GamePlayManager.Instance.SetTopViewCamera(9);
                    break;
                case PlayerPrefsHandler.TopViewTriggerTag:
                    GamePlayManager.Instance.SetTopViewCamera(11);
                    break;
                case PlayerPrefsHandler.TopViewTrigger2Tag:
                    GamePlayManager.Instance.SetTopViewCamera2(11);
                    break;
                case PlayerPrefsHandler.CollectableTag:
                    other.tag = PlayerPrefsHandler.NoneTag;
                    other.transform.GetChild(0).gameObject.SetActive(false);
                    other.transform.GetChild(1).gameObject.SetActive(true);
                    SoundController.Instance.PlayRollingBallCoinSound();
                    break;
                case PlayerPrefsHandler.NormalDragTag:
                    _movable ??= GetComponent<IMovable>();
                    _movable.MinimumDrag(false);
                    break;
                case PlayerPrefsHandler.CameraViewTriggerTag:
                    GamePlayManager.Instance.GetCameraViewController().ChangeCameraView(other.GetComponent<CameraViewTrigger>().cameraViewToChange);
                    break;
                case PlayerPrefsHandler.OppositeGravityTriggerTag:
                    _movable ??= GetComponent<IMovable>();
                    _movable.FlyUp(true);
                    GamePlayManager.Instance.GetCameraViewController().ActivateReCentering(false);
                    break;
                case PlayerPrefsHandler.LevelCompleteTriggerTag:
                    _movable ??= GetComponent<IMovable>();
                    _movable.AllowMovement(false);
                    GamePlayManager.Instance.LevelComplete(3f);
                    break;
                case PlayerPrefsHandler.ActivateReCenteringTag:
                    GamePlayManager.Instance.GetCameraViewController().ActivateReCentering(true);
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
                    GamePlayManager.Instance.GetCameraViewController().ActivateReCentering(true);
                    break;
            }
        }
    }
}