using UnityEngine;
namespace _RollingBall.MyScripts
{
    public class BallInputProvider : MonoBehaviour, IInputProvider
    {
        private Swerve _swerveInputs;
        private const string HorizontalString = "Horizontal", VerticalString = "Vertical";
        public float GetHorizontalInput()
        {
            if (!_swerveInputs) _swerveInputs = GamePlayManager.Instance.uiManager.GetSwerveInputs();
            return _swerveInputs.MoveFactorX;
        }
        public float GetVerticalInput()
        {
            if (!_swerveInputs) _swerveInputs = GamePlayManager.Instance.uiManager.GetSwerveInputs();
            return _swerveInputs.MoveFactorY;
        }
    }
    public interface IInputProvider
    {
        float GetHorizontalInput();
        float GetVerticalInput();
    }
}