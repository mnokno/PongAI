using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PongAI
{
    /// <summary>
    /// Simple implementation of a ball
    /// </summary>
    public class Ball : MonoBehaviour
    {
        /// <summary>
        /// Initial speed of the ball
        /// </summary>
        public float speed = 3f;
        /// <summary>
        /// Maximum speed of for the ball
        /// </summary>
        public float maxSpeed = 7f;
        /// <summary>
        /// How much the ball incenses in speed after bouncing, 1.05 = 5% increase on each bounce
        /// </summary>
        public float speedIncreaseOnBounce = 1.05f;

        /// <summary>
        /// Width of the board, used to efficiently detect collisions with side walls 
        /// </summary>
        public float width = 6f;
        /// <summary>
        /// Height of the board used to detect ball that pass the paddle (collisions with walls perpendicular to side wall) 
        /// </summary>
        public float height = 10f;
        /// <summary>
        /// Used to align the ball bounces visually with the edges (used to make the ball bounce from its edge, not its center) 
        /// </summary>
        public float ballRadius = 0.25f;
        /// <summary>
        /// Used to prevent multiple bounces in quick succession upon collision
        /// </summary>
        private bool isBouncing = false;
        /// <summary>
        /// Stores reference to the ball Rigidbody
        /// </summary>
        private Rigidbody rb;
        /// <summary>
        /// Stores reference to a match manager for this game, there may be more than one game running at the same time
        /// </summary>
        private MatchManager matchManager;
        /// <summary>
        /// Stores the number of paddle bounces for this ball
        /// </summary>
        public int paddleBounceCount = 0;
        /// <summary>
        /// Used to ensure that the ball does not drift up over time
        /// </summary>
        private float ballY;


        /// <summary>
        /// Called before the first frame update
        /// </summary>
        public void Start()
        {
            rb = GetComponent<Rigidbody>();
            matchManager = GetComponentInParent<MatchManager>();
            ballY = transform.localPosition.y;
            ResetBall();
        }

        /// <summary>
        ///  Update is called once per frame
        /// </summary>
        void Update()
        {
            if (!isBouncing)
            {
                if (Mathf.Abs(transform.localPosition.x) > width * 0.5f - ballRadius)
                {
                    Vector3 newVel = new Vector3(-rb.velocity.x, 0f, rb.velocity.z) * speedIncreaseOnBounce;
                    newVel = newVel.magnitude > maxSpeed ? newVel.normalized * maxSpeed : newVel;
                    rb.velocity = newVel;
                }

                if (Mathf.Abs(transform.localPosition.z) > height * 0.5f - ballRadius)
                {
                    matchManager.BallPassedPaddle();
                }

                isBouncing = true;
                // Delay the reset of 'isBouncing' to prevent multiple bounces in quick succession
                Invoke("ResetBouncing", 0.2f);
            }
        }

        /// <summary>
        /// Should be called to start and/or reset the ball
        /// </summary>
        public void ResetBall()
        {
            transform.localPosition = new Vector3(0f, ballY, 0f);
            Vector3 startVel = new Vector3(
                (Random.Range(0f, 1f) > 0.5f ? speed : -speed) * Random.Range(0.5f, 4f),
                0f,
                (Random.Range(0f, 1f) > 0.5f ? speed : -speed) * Random.Range(0.5f, 2f));
            rb.velocity = startVel.normalized * speed;
            StopAllCoroutines();
            isBouncing = false;
            paddleBounceCount = 0;
        }

        /// <summary>
        /// Resets isBouncing
        /// </summary>
        private void ResetBouncing()
        {
            isBouncing = false;
        }

        /// <summary>
        /// Returns the velocity of the ball on the XZ plane
        /// </summary>
        /// <returns>Velocity of the ball on the XZ plane</returns>
        public Vector2 GetVelocity()
        {
            return new Vector2(rb.velocity.x, rb.velocity.z);
        }

        /// <summary>
        /// Returns the local position of the ball on the XZ plane
        /// </summary>
        /// <returns>Local position of the ball on the XZ plane</returns>
        public Vector2 GetPostion()
        {
            return new Vector2(transform.localPosition.x, transform.localPosition.z);
        }    

        /// <summary>
        /// Makes the ball bounce bouncing with a non-edge object (in this case a paddle)
        /// Prevision of multiple bounces in quick succession upon collision should be handled by the other object, NOT THE BALL! 
        /// </summary>
        /// <param name="moveDirectionOfOtherOject">The movement direction of the object the ball collided with</param>
        public void Bounce(MoveDirection moveDirectionOfOtherOject = MoveDirection.Still)
        {
            paddleBounceCount++;
            Vector3 newVel = new Vector3(rb.velocity.x, 0f, -rb.velocity.z) * speedIncreaseOnBounce;
            float mag = Mathf.Min(newVel.magnitude, maxSpeed);
            float moveMod = moveDirectionOfOtherOject == MoveDirection.Still ? 0 : (moveDirectionOfOtherOject == MoveDirection.Left ? -0.5f : 0.5f);
            newVel = newVel + new Vector3(moveMod, 0f, 0f);
            newVel = newVel.normalized * mag;
            rb.velocity = newVel;
        }
    }
}
