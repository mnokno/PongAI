using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleBounce : MonoBehaviour
{
    private bool isBouncing = false;
    [SerializeField] private GameObject ballGo;
    private Rigidbody rb;
    private Ball ball;
    private PaddleMove paddleMove;

    private void Start()
    {
        rb = ballGo.GetComponent<Rigidbody>();
        ball = ballGo.GetComponent<Ball>();
        paddleMove = GetComponent<PaddleMove>();
    }

    private void ResetBouncing()
    {
        isBouncing = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isBouncing)
        { 
            Vector3 newVel = new Vector3(rb.velocity.x, 0f, -rb.velocity.z) * ball.speedIncreaseOnBounce;
            float mag = Mathf.Min(newVel.magnitude, ball.maxSpeed);
            float moveMod = paddleMove.moveDirection == MoveDirection.Still ? 0 : (paddleMove.moveDirection == MoveDirection.Left ? -0.5f : 0.5f);
            newVel = newVel.normalized + new Vector3(moveMod, 0f, 0f);
            newVel = newVel.normalized * mag;
            rb.velocity = newVel;
            isBouncing = true;
            // Delay the reset of 'isBouncing' to prevent multiple bounces in quick succession
            Invoke("ResetBouncing", 0.1f);
        }
    }
}
