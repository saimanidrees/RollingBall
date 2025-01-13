using _RollingBall.MyScripts;
using UnityEngine;
public class TouchInputProvider : MonoBehaviour, IInputProvider
{
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    private bool isDragging;

    public bool IsDragging => isDragging;  // Property to check if dragging

    public float GetHorizontalInput()
    {
        return endTouchPosition.x - startTouchPosition.x;
    }

    public float GetVerticalInput()
    {
        return endTouchPosition.y - startTouchPosition.y;
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    startTouchPosition = touch.position;
                    isDragging = true;
                    break;

                case TouchPhase.Moved:
                    endTouchPosition = touch.position;
                    break;

                case TouchPhase.Ended:
                    isDragging = false;
                    endTouchPosition = Vector2.zero; // Reset to avoid carrying over
                    break;
            }
        }
        else if (!isDragging)
        {
            endTouchPosition = Vector2.zero; // Reset when not dragging
        }
    }
}