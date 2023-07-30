using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleMove : MonoBehaviour
{
    public float boardWidth = 6f;
    public float paddleWidth = 1f;
    public float speed = 2f;

    public bool useArrowKeys = true;
    public bool useAD = true;
    public MoveDirection moveDirection;

    // Update is called once per frame
    void Update()
    {
        moveDirection = MoveDirection.Still;
        if ((Input.GetKey(KeyCode.A) && useAD) || (Input.GetKey(KeyCode.LeftArrow) && useArrowKeys))
        {
            Vector3 newPos = transform.localPosition - new Vector3(Time.deltaTime * speed, 0f, 0f);
            transform.localPosition = newPos;
            moveDirection = MoveDirection.Left;
        }
        if ((Input.GetKey(KeyCode.D) && useAD) || (Input.GetKey(KeyCode.RightArrow) && useArrowKeys))
        {
            Vector3 newPos = transform.localPosition + new Vector3(Time.deltaTime * speed, 0f, 0f);
            transform.localPosition = newPos;
            moveDirection = MoveDirection.Right;
        }
    }
}
