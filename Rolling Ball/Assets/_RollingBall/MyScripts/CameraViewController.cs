using Cinemachine;
using UnityEngine;
namespace _RollingBall.MyScripts
{
    public class CameraViewController : MonoBehaviour
    {
        public enum CameraViews
        {
            DefaultView,
            WithoutReverse,
            TopView0,
            TopView1,
            TopView2,
            TopView3,
            TopView4,
            TopView5,
            TopView6
        }
        [SerializeField] private CinemachineFreeLook freeLookCamera;
        [SerializeField] private CinemachineVirtualCameraBase cameraWithoutReverseView, topViewCamera0, topViewCamera1, topViewCamera2, ballSelectionView;
        [SerializeField] private CinemachineVirtualCameraBase levelCompleteView, topViewCamera3, topViewCamera4, topViewCamera5, topViewCamera6;
        private bool _cameraReCentering = true;
        public void ChangeCameraView(CameraViews cameraView)
        {
            switch (cameraView)
            {
                case CameraViews.DefaultView:
                    cameraWithoutReverseView.Priority = 9;
                    topViewCamera0.Priority = 9;
                    topViewCamera1.Priority = 9;
                    topViewCamera2.Priority = 9;
                    topViewCamera3.Priority = 9;
                    topViewCamera4.Priority = 9;
                    topViewCamera5.Priority = 9;
                    topViewCamera6.Priority = 9;
                    GamePlayManager.Instance.GetCameraController().UnPauseTheAlignment();
                    break;
                case CameraViews.WithoutReverse:
                    cameraWithoutReverseView.Priority = 11;
                    GamePlayManager.Instance.GetCameraController().PauseTheAlignmentOnly();
                    break;
                case CameraViews.TopView0:
                    topViewCamera0.Priority = 11;
                    topViewCamera1.Priority = 9;
                    topViewCamera2.Priority = 9;
                    topViewCamera3.Priority = 9;
                    topViewCamera4.Priority = 9;
                    topViewCamera5.Priority = 9;
                    topViewCamera6.Priority = 9;
                    break;
                case CameraViews.TopView1:
                    topViewCamera0.Priority = 9;
                    topViewCamera1.Priority = 11;
                    topViewCamera2.Priority = 9;
                    topViewCamera3.Priority = 9;
                    topViewCamera4.Priority = 9;
                    topViewCamera5.Priority = 9;
                    topViewCamera6.Priority = 9;
                    break;
                case CameraViews.TopView2:
                    topViewCamera0.Priority = 9;
                    topViewCamera1.Priority = 9;
                    topViewCamera2.Priority = 11;
                    topViewCamera3.Priority = 9;
                    topViewCamera4.Priority = 9;
                    topViewCamera5.Priority = 9;
                    topViewCamera6.Priority = 9;
                    break;
                case CameraViews.TopView3:
                    topViewCamera0.Priority = 9;
                    topViewCamera1.Priority = 9;
                    topViewCamera2.Priority = 9;
                    topViewCamera3.Priority = 11;
                    topViewCamera4.Priority = 9;
                    topViewCamera5.Priority = 9;
                    topViewCamera6.Priority = 9;
                    break;
                case CameraViews.TopView4:
                    topViewCamera0.Priority = 9;
                    topViewCamera1.Priority = 9;
                    topViewCamera2.Priority = 9;
                    topViewCamera3.Priority = 9;
                    topViewCamera4.Priority = 11;
                    topViewCamera5.Priority = 9;
                    topViewCamera6.Priority = 9;
                    break;
                case CameraViews.TopView5:
                    topViewCamera0.Priority = 9;
                    topViewCamera1.Priority = 9;
                    topViewCamera2.Priority = 9;
                    topViewCamera3.Priority = 9;
                    topViewCamera4.Priority = 9;
                    topViewCamera5.Priority = 11;
                    topViewCamera6.Priority = 9;
                    break;
                case CameraViews.TopView6:
                    topViewCamera0.Priority = 9;
                    topViewCamera1.Priority = 9;
                    topViewCamera2.Priority = 9;
                    topViewCamera3.Priority = 9;
                    topViewCamera4.Priority = 9;
                    topViewCamera5.Priority = 9;
                    topViewCamera6.Priority = 11;
                    break;
                default:
                    cameraWithoutReverseView.Priority = 9;
                    topViewCamera0.Priority = 9;
                    topViewCamera1.Priority = 9;
                    topViewCamera2.Priority = 9;
                    topViewCamera3.Priority = 9;
                    topViewCamera4.Priority = 9;
                    topViewCamera5.Priority = 9;
                    topViewCamera6.Priority = 9;
                    break;
            }
        }
        public void SetReverseViewCamera(int priorityValue)
        {
            cameraWithoutReverseView.Priority = priorityValue;
        }
        public void SetTopViewCamera(int priorityValue)
        {
            topViewCamera0.Priority = priorityValue;
        }
        public void SetTopViewCamera2(int priorityValue)
        {
            topViewCamera1.Priority = priorityValue;
        }
        public void SetReCentering(bool flag)
        {
            if (!_cameraReCentering)
            {
                freeLookCamera.m_RecenterToTargetHeading.m_enabled = _cameraReCentering;
                return;
            }
            freeLookCamera.m_RecenterToTargetHeading.m_enabled = flag;
        }
        public void ActivateReCentering(bool flag)
        {
            _cameraReCentering = flag;
        }
        public void SetXAxisValue(float newValue)
        {
            freeLookCamera.m_XAxis.Value = newValue;
        }
        public void SetBallSelectionView(int priorityValue)
        {
            ballSelectionView.Priority = priorityValue;
        }
        public void SetLevelCompleteView()
        {
            levelCompleteView.Priority = 12;
        }
    }
}