using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{



    private Vector3 touchStartPos;

    private bool isTouching = false;


    void Update()
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
                    Debug.Log("Yukarý Kaydýrýldý");
                    isTouching = false;
                }
                else if ((touchStartPos.y - 100f) > touch.position.y && isTouching)
                {
                    Debug.Log("Aþaðýya Kaydýrýldý");
                    isTouching = false;
                }
                else if ((touchStartPos.x - 100f) > touch.position.x && isTouching)
                {
                    Debug.Log("Sola Kaydýrýldý");
                    isTouching = false;
                }
                else if ((touchStartPos.x + 100f) < touch.position.x && isTouching)
                {
                    Debug.Log("Saða Kaydýrýldý");
                    isTouching = false;
                }
            }
        }
    }





}
