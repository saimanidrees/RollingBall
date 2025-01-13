using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
namespace _RollingBall.MyScripts
{
    public class SewerageSystem : MonoBehaviour
    {
        [SerializeField] private Rigidbody[] balls;
        [SerializeField] private Transform[] pipePositions;
        [SerializeField] private float[] force;
        private int _index = 0;
        private void Start()
        {
            StartCoroutine(EjectBallWithDelay());
        }
        private IEnumerator EjectBallWithDelay()
        {
            yield return null;
            balls[_index].isKinematic = true;
            balls[_index].transform.localPosition = pipePositions[_index].localPosition;
            balls[_index].isKinematic = false;
            balls[_index].AddForce(pipePositions[_index].forward * force[_index], ForceMode.Impulse);
            _index++;
            if (_index >= balls.Length)
                _index = 0;
        }
        [Button]
        private void EjectBall()
        {
            StartCoroutine(EjectBallWithDelay());
        }
    }
}