using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{



    private Vector3 touchStartPos;

    private bool isTouching = false;


    private void Update()
    {
        TouchControl();
    }


    private void TouchControl()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                touchStartPos = touch.position;
                isTouching = true;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                if ((touchStartPos.y + 100f) < touch.position.y && isTouching)
                {
                    BoardController.GetInstance().MoveToDirection(Direction.UP);
                    CommandManager.GetInstance().AddMove(Direction.UP);
                    isTouching = false;
                }
                else if ((touchStartPos.y - 100f) > touch.position.y && isTouching)
                {
                    BoardController.GetInstance().MoveToDirection(Direction.DOWN);
                    CommandManager.GetInstance().AddMove(Direction.DOWN);
                    isTouching = false;
                }
                else if ((touchStartPos.x - 100f) > touch.position.x && isTouching)
                {
                    BoardController.GetInstance().MoveToDirection(Direction.LEFT);
                    CommandManager.GetInstance().AddMove(Direction.LEFT);
                    isTouching = false;
                }
                else if ((touchStartPos.x + 100f) < touch.position.x && isTouching)
                {
                    BoardController.GetInstance().MoveToDirection(Direction.RIGHT);
                    CommandManager.GetInstance().AddMove(Direction.RIGHT);
                    isTouching = false;
                }
            }
        }
    }





}
