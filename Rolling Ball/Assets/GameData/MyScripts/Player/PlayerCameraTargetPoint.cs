using System;
using UnityEngine;

namespace HCStore.Player
{
    public class PlayerCameraTargetPoint : MonoBehaviour
    {
        [SerializeField] private Vector3 followAxis = new Vector3(0, 1, 1);
        private PlayerController _playerController;
        private Vector3 _offset;

        private void Awake()
        {
            _playerController = GetComponentInParent<PlayerController>();
            _offset = transform.position - _playerController.transform.position;
            transform.SetParent(null,true);
        }
        private void Update()
        {
            transform.position = Vector3.Scale(_playerController.transform.position + _offset,followAxis);
        }
    }
}