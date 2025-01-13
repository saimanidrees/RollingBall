using UnityEngine;
using DG.Tweening;
using System.Collections;
public class CameraControl : MonoBehaviour
{
    public Camera camera;
    public Transform target;
    public Vector3 offset, ToChangeOffSet;
    public Vector3 viewPointOffset, ToChangeVP,EndPointOffSet;
    public float speed;
    public bool LerpNow, GameEnded;
    private float LerpTime;
    private Vector3 OffsetBackUp, VeiwPointBackUP;
    public bool ActivateSmoothRotation;
    private float angle;
    private float FieldOfViewBackup;
    private Quaternion q;
    private Vector3 v3TargetOffset;
    private void Start()
    {
        LerpNow = false;
        LerpTime = 0;
        OffsetBackUp = offset;
        VeiwPointBackUP = viewPointOffset;
        GameEnded = false;
        FieldOfViewBackup = camera.fieldOfView;
    }
    private void LateUpdate()
    {
        var Y = target.transform.localEulerAngles.y;

        if (Y < 0)
            Y *= -1;

        //Debug.Log(Y);



        if ((Y >= 0 && Y <= 10) || (Y >= 80 && Y <= 100) || (Y >= 170 && Y <= 190) || (Y >= 260 && Y <= 280) || (Y >= 350 && Y <= 370))
        {
            gameObject.transform.parent = null; //Debug.Log("Unchild");
        }
        else
        {
            gameObject.transform.parent = target.transform;
         //   Debug.Log("Child");
        }


        if (camera && target)
        {
            Vector3 v3TargetOffset = target.position;
            if (GameEnded)
            {
                v3TargetOffset += (offset.z * target.transform.forward+EndPointOffSet);
                v3TargetOffset += (offset.y * target.transform.up + EndPointOffSet);
                v3TargetOffset += (offset.x * target.transform.right + EndPointOffSet);
                camera.transform.parent = null;
            }
            else
            {
                v3TargetOffset += (offset.z * target.transform.forward);
                v3TargetOffset += (offset.y * target.transform.up);
                v3TargetOffset += (offset.x * target.transform.right);
            }



            Vector3 v3viewPointOffset = target.position;
            v3viewPointOffset += (viewPointOffset.y * target.transform.up);


            if (ActivateSmoothRotation)
            {
                Quaternion rotation = Quaternion.LookRotation(v3viewPointOffset - camera.transform.position);
                Quaternion smoothRotation = Quaternion.Slerp(camera.transform.rotation, rotation, Time.deltaTime * speed);
                camera.transform.rotation = smoothRotation;
            }

            camera.transform.position = Vector3.Lerp(camera.transform.position, v3TargetOffset, Time.deltaTime * speed);

            if (!ActivateSmoothRotation)
                camera.transform.LookAt(v3viewPointOffset);
        }
        if (LerpNow)
        {
            if (LerpTime <= 1)
            {
                offset = Vector3.Lerp(OffsetBackUp, ToChangeOffSet, LerpTime);
                viewPointOffset = Vector3.Lerp(VeiwPointBackUP, ToChangeVP, LerpTime);
                LerpTime += 0.01f;
            }
        }
    }
    internal void SetEndPointOffSet(Vector3 Value,float changeSpeed)
    {
        EndPointOffSet = Value;
        speed = changeSpeed;
    }
    internal void ChangeTargetAndLookPt(Transform Target,Vector3 BlockOffSet)
    {
        target = Target;
        offset = BlockOffSet;
        EndPointOffSet = Vector3.zero;
    }
    bool AllowtoSwitch;
    internal void AddOffSetInZAxis()
    {
        if (!AllowtoSwitch)
        {
            AllowtoSwitch = true;
           // offset -= new Vector3(0, 0, 4);
            DOTween.Kill(camera);
            camera.DOFieldOfView(71,.51f).OnComplete(()=> AllowtoSwitch=false);
        }
    }

    internal void BackToNormal()
    {
      //  offset = OffsetBackUp;
            DOTween.Kill(camera);
        camera.DOFieldOfView(FieldOfViewBackup, .51f).OnComplete(() => AllowtoSwitch = false);
    }
}


