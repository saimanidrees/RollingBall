using Sirenix.OdinInspector;
using UnityEngine;
namespace _RollingBall.MyScripts
{
    public class CameraViewTrigger : MonoBehaviour
    {
        [EnumToggleButtons] public CameraViewController.CameraViews cameraViewToChange;
    }
}