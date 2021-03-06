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
        if (Input.touchCount > 0 && !CommandManager.GetInstance().IsReplay)
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
                    if (!CommandManager.GetInstance().IsReplay)
                        CommandManager.GetInstance().AddMove(new Move(MoveType.Swipe, Direction.UP));

                    BoardController.GetInstance().MoveToDirection(Direction.UP);
                    isTouching = false;
                }
                else if ((touchStartPos.y - 100f) > touch.position.y && isTouching)
                {
                    if (!CommandManager.GetInstance().IsReplay)
                        CommandManager.GetInstance().AddMove(new Move(MoveType.Swipe, Direction.DOWN));

                    BoardController.GetInstance().MoveToDirection(Direction.DOWN);
                    isTouching = false;
                }
                else if ((touchStartPos.x - 100f) > touch.position.x && isTouching)
                {
                    if (!CommandManager.GetInstance().IsReplay)
                        CommandManager.GetInstance().AddMove(new Move(MoveType.Swipe, Direction.LEFT));

                    BoardController.GetInstance().MoveToDirection(Direction.LEFT);
                    isTouching = false;
                }
                else if ((touchStartPos.x + 100f) < touch.position.x && isTouching)
                {
                    if (!CommandManager.GetInstance().IsReplay)
                        CommandManager.GetInstance().AddMove(new Move(MoveType.Swipe, Direction.RIGHT));

                    BoardController.GetInstance().MoveToDirection(Direction.RIGHT);
                    isTouching = false;
                }
            }
        }
    }





}
