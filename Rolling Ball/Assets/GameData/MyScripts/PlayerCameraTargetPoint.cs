using System;
using UnityEngine;
using UnityEngine.Animations;
using DG.Tweening;

    public class PlayerCameraTargetPoint : MonoBehaviour
    {
        private void Awake()
        {
            Application.targetFrameRate = 120;
            transform.SetParent(null,false);
            transform.eulerAngles = Vector3.zero;
        }

        private void LateUpdate()
        {
            if (!lerping)
            {

                    LerpingXValue = Mathf.MoveTowards(LerpingXValue, 0, 16 * CameraLerpSpeed * Time.fixedDeltaTime);
                    transform.position = new Vector3(LerpingXValue, transform.position.y, transform.position.z);
                //if (LerpingXValue >=0.)
                //{
                //}
                //else
                //{
                //    LerpingXValue = 0;
                //    transform.position = new Vector3(LerpingXValue, transform.position.y, transform.position.z);
                //}
               
            }
        }
        public float CameraLerpSpeed=5;
        float LerpingXValue=0;
        internal  bool lerping=false;
        public void FollowtoXaxis( bool RightSide)
        {
           // print("Move to  X");
            lerping = true;
            if (RightSide)
            {
                LerpingXValue = Mathf.Lerp(LerpingXValue, 3.94f, CameraLerpSpeed * Time.fixedDeltaTime * 1);
            }
            else
            {
                LerpingXValue = Mathf.Lerp(LerpingXValue, -3.94f, CameraLerpSpeed * Time.fixedDeltaTime * 1);

            }
                transform.position = new Vector3(LerpingXValue, transform.position.y, transform.position.z);
        }
        internal void BackToNormal()
        {
            lerping = false;
        }
        internal void SetDefaultFollowAxis()
        {
            LerpingXValue = 0;
        }
        internal void ReachOnEnd()
        {
           // transform.DORotate(new Vector3(0, -35.75f, 0), 1);
            transform.DORotate(new Vector3(0, -14.3f, 0), 1);
        }

       
    }