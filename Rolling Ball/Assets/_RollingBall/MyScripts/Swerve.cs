using UnityEngine;

namespace _RollingBall.MyScripts
{
    public class Swerve : MonoBehaviour
    {
        private float lastFramePosX, lastFramePosY;
        private float moveFactorX, moveFactorY;
    
        public bool onScreenHold;
    
        public float MoveFactorX => moveFactorX;
        public float MoveFactorY => moveFactorY;

        public bool OnScreenHold
        {
            get => onScreenHold;
            set => onScreenHold = value;
        }


        private void Update()
        {
            
            if (Input.touches.Length > 0)
            {
                
                {
                    if (Input.GetTouch(0).phase == TouchPhase.Began)
                    {
                        lastFramePosX = Input.GetTouch(0).position.x;
                        onScreenHold = true;
                    }
                    else if (Input.GetTouch(0).phase == TouchPhase.Moved ||
                             Input.GetTouch(0).phase == TouchPhase.Stationary)
                    {
                        moveFactorX = Input.GetTouch(0).position.x - lastFramePosX;
                        lastFramePosX = Input.GetTouch(0).position.x;
                    }
                    else if (Input.GetTouch(0).phase == TouchPhase.Ended ||
                             Input.GetTouch(0).phase == TouchPhase.Canceled)
                    {
                        moveFactorX = 0f;
                        onScreenHold = false;
                    }
                }
                
                {
                    if (Input.GetTouch(0).phase == TouchPhase.Began)
                    {
                        lastFramePosY = Input.GetTouch(0).position.y;
                        onScreenHold = true;
                    }
                    else if (Input.GetTouch(0).phase == TouchPhase.Moved ||
                             Input.GetTouch(0).phase == TouchPhase.Stationary)
                    {
                        moveFactorY = Input.GetTouch(0).position.y - lastFramePosY;
                        lastFramePosY = Input.GetTouch(0).position.y;
                    }
                    else if (Input.GetTouch(0).phase == TouchPhase.Ended ||
                             Input.GetTouch(0).phase == TouchPhase.Canceled)
                    {
                        moveFactorY = 0f;
                        onScreenHold = false;
                    }
                    
                }
            }

        }
    }
}