using UnityEngine;
namespace _RollingBall.MyScripts
{
    public class ReviveTrigger : MonoBehaviour
    {
        private const string RespawnPointString = "RespawnPoint";
        private Transform _respawnPoint;
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag.Equals(PlayerPrefsHandler.BallTag))
            {
                if (!GamePlayManager.Instance.IsAlive())
                {
                    GamePlayManager.Instance.LevelFail(1f);
                    return;
                }
                if (!_respawnPoint) _respawnPoint = transform.Find(RespawnPointString);
                GamePlayManager.Instance.GetCameraViewController().ChangeCameraView(CameraViewController.CameraViews.DefaultView);
                var ball = other.GetComponent<BallMovement>();
                var rb = ball.GetComponent<Rigidbody>();
                rb.isKinematic = true;
                ball.transform.position = _respawnPoint.position;
                ball.transform.rotation = _respawnPoint.rotation;
                rb.isKinematic = false;
                GamePlayManager.Instance.SetXAxisValue(_respawnPoint.eulerAngles.y);
                GamePlayManager.Instance.GetCameraController().PauseTheAlignment(true);
                //ball.Push(_respawnPoint.forward * 4f);
                ball.AllowMovement(true);
            }
        }
    }
}