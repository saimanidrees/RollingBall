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
                var ball = other.GetComponent<BallMovement>();
                _respawnPoint = GamePlayManager.Instance.currentLevel.GetClosestTransform(ball.GetBallLastPos());
                if (!GamePlayManager.Instance.IsAlive())
                {
                    GamePlayManager.Instance.SetPositionRotationForRevive(_respawnPoint.position, _respawnPoint.rotation);
                    GamePlayManager.Instance.LevelFail(1f);
                    return;
                }
                var rb = ball.GetComponent<Rigidbody>();
                rb.isKinematic = true;
                ball.transform.position = _respawnPoint.position;
                ball.transform.rotation = _respawnPoint.rotation;
                rb.isKinematic = false;
                GamePlayManager.Instance.GetCameraController().PauseTheAlignment(true);
                //ball.Push(_respawnPoint.forward * 4f);
                ball.AllowMovement(true);
            }
        }
    }
}