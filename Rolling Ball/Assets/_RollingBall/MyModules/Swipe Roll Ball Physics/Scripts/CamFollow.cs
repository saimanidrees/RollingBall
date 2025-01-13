using UnityEngine;
public class CamFollow : MonoBehaviour
{
    [SerializeField]
    private Transform ball;
    [SerializeField] private Transform cam;
    [SerializeField]
    private float followCamOffsetY = 2, followCamOffsetZ = 10, followSpeedInverse = 0.5f,lookAtSpeed = 200f, lookAtOffsetY = 0;
    private Vector3 _targetPos, _camFollowPos;
    private Vector3 _velo = Vector3.up;
    private Rigidbody _rb;
    private void Start()
    {
        _rb = ball.GetComponent<Rigidbody>();
        var position = ball.position;
        _camFollowPos =new Vector3( position.x , position.y + followCamOffsetY,position.z - followCamOffsetZ);
        cam.position = _camFollowPos;       
    }
    private void FixedUpdate()
    {
        FindDisplacementDir();
    }
    private void FindDisplacementDir()
    {
        var velocity = -(_rb.velocity).normalized;
        _camFollowPos = velocity * followCamOffsetZ ;  
        _camFollowPos = ball.position + _camFollowPos;
        _camFollowPos = new Vector3(_camFollowPos.x , _camFollowPos.y + followCamOffsetY , _camFollowPos.z );
        //Debug.Log(_rb.velocity.magnitude);
        //if (_rb.velocity.z != 0)
        if(_rb.velocity.magnitude > 1f)
        {
            if (_rb.velocity.magnitude > 2f)
                followSpeedInverse = 0.5f;
            else
                followSpeedInverse = 1f;
            cam.position = Vector3.SmoothDamp(cam.position, _camFollowPos, ref _velo, followSpeedInverse, Mathf.Infinity);
        }
        else
        {
            /*var position = ball.position;
            _camFollowPos = new Vector3(position.x , position.y + followCamOffsetY, position.z - followCamOffsetZ);
            cam.position = _camFollowPos;*/
        }
        var position1 = ball.position;
        _targetPos = new Vector3(position1.x,position1.y + lookAtOffsetY ,position1.z);
         var targetRotation = Quaternion.LookRotation(_targetPos - cam.position);
         cam.rotation = Quaternion.Slerp(cam.rotation, targetRotation, lookAtSpeed * Time.deltaTime);
    }
    public void Reset()
    {
        var position = ball.position;
        _camFollowPos =new Vector3( position.x , position.y + followCamOffsetY,position.z - followCamOffsetZ);
        cam.position = _camFollowPos;
        _targetPos = new Vector3(position.x,position.y + lookAtOffsetY ,position.z);
        var targetRotation = Quaternion.LookRotation(_targetPos - cam.position);
        cam.rotation = targetRotation;
    }
}