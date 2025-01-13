using CnControls;
using UnityEngine;
namespace _RollingBall.MyScripts
{
    public class BallInputProvider : MonoBehaviour, IInputProvider
    {
        private const string HorizontalString = "Horizontal", VerticalString = "Vertical";
        public float GetHorizontalInput()
        {
            return CnInputManager.GetAxis(HorizontalString);
        }
        public float GetVerticalInput()
        {
            return CnInputManager.GetAxis(VerticalString);
        }
    }
    public interface IInputProvider
    {
        float GetHorizontalInput();
        float GetVerticalInput();
    }
}