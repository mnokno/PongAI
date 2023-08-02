using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

namespace PongAI
{
    /// <summary>
    /// Simple agent implementation for pong paddle control. 
    /// Yes I know its spelled wrong but, I kinda like it now.
    /// </summary>
    public class PalletAgent : Agent
    {
        /// <summary>
        /// Stores the current movement direction of the paddle
        /// </summary>
        private MoveDirection moveDirection = MoveDirection.Still;
        /// <summary>
        /// Weather or note the paddle can be moved using arrow keys
        /// </summary>
        [SerializeField] private bool useArrowKeys = true;
        /// <summary>
        /// Weather or note the paddle can be moved using A and D keys
        /// </summary>
        [SerializeField] private bool useAD = true;
        /// <summary>
        /// Speed of the paddle movement
        /// </summary>
        [SerializeField] private float speed = 4f;
        /// <summary>
        /// Prevents the paddle from moving outside of the board
        /// </summary>
        [SerializeField] private float boardWidth = 6f;
        /// <summary>
        /// Stores reference to a match manager for this game, there may be more than one game running at the same time
        /// </summary>
        private MatchManager matchManager;
        /// <summary>
        /// Used to prevent multiple bounces in quick succession upon collision
        /// </summary>
        private bool isBouncing = false;
        /// <summary>
        /// True if its the upper agent, false otherwise
        /// </summary>
        private bool isUpper;


        /// <summary>
        /// Start is called before the first frame update
        /// </summary>
        public void Start()
        {
            matchManager = GetComponentInParent<MatchManager>();
            isUpper = transform.localPosition.z > 0;
        }

        /// <summary>
        /// Called when the paddle collides with the ball
        /// </summary>
        /// <param name="other">The ball collider</param>
        private void OnTriggerEnter(Collider other)
        {
            if (!isBouncing)
            {
                matchManager.GetBall().Bounce(moveDirection);
                AddReward(0.1f);
                isBouncing = true;
                // Delay the reset of 'isBouncing' to prevent multiple bounces in quick succession
                Invoke("ResetBouncing", 0.1f);
            }
        }

        /// <summary>
        /// Resets isBouncing
        /// </summary>
        private void ResetBouncing()
        {
            isBouncing = false;
        }

        /// <summary>
        /// Call to rest the position of the paddle, should be called at the end of each episode
        /// </summary>
        public void ResetPosition()
        {
            moveDirection = MoveDirection.Still;
            transform.localPosition = new Vector3(0f, transform.localPosition.y, transform.localPosition.z);
            StopAllCoroutines();
            isBouncing = false;
        }

        #region ML Agents Overrides and Related

        public override void OnActionReceived(ActionBuffers actions)
        {
            // 0 no move, 1 left, 2 right
            if (actions.DiscreteActions[0] == (int)MoveDirection.Still)
            {
                // No moment
                // Small reward for surviving when not moving
                AddReward(0.001f);
            }
            else if (actions.DiscreteActions[0] == (int)MoveDirection.Left)
            {
                // Move left
                Vector3 newPos = transform.localPosition - new Vector3(Time.deltaTime * speed, 0f, 0f);
                newPos.x = Mathf.Abs(newPos.x) > boardWidth / 2f ? boardWidth / 2f * Mathf.Sign(newPos.x) : newPos.x;
                transform.localPosition = newPos;
            }
            else if (actions.DiscreteActions[0] == (int)MoveDirection.Right)
            {
                // Move right
                Vector3 newPos = transform.localPosition + new Vector3(Time.deltaTime * speed, 0f, 0f);
                newPos.x = Mathf.Abs(newPos.x) > boardWidth / 2f ? boardWidth / 2f * Mathf.Sign(newPos.x) : newPos.x;
                transform.localPosition = newPos;
            }
        }

        public override void CollectObservations(VectorSensor sensor)
        {
            sensor.AddObservation(transform.localPosition.x);
            if (isUpper)
            {
                sensor.AddObservation(matchManager.GetBall().GetPostion());
                sensor.AddObservation(matchManager.GetBall().GetVelocity());
            }
            else
            {
                Vector2 pos = matchManager.GetBall().GetPostion();
                Vector2 vel = matchManager.GetBall().GetVelocity();

                pos.x = -pos.x;
                vel.x = -vel.x;

                sensor.AddObservation(pos);
                sensor.AddObservation(vel);
            }
        }
            

        /// <summary>
        /// For the testing, overrides action provided to the agent
        /// </summary>
        /// <param name="actionsOut">Buffer with the actions to be modified in place</param>
        public override void Heuristic(in ActionBuffers actionsOut)
        {
            // 0 no move, 1 left, 2 right
            ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
                       
            bool moveLeft = (Input.GetKey(KeyCode.A) && useAD) || (Input.GetKey(KeyCode.LeftArrow) && useArrowKeys);
            bool moveRight = (Input.GetKey(KeyCode.D) && useAD) || (Input.GetKey(KeyCode.RightArrow) && useArrowKeys);
            if (moveLeft && !moveRight)
            {
                moveDirection = MoveDirection.Left;
            }
            else if (moveRight && !moveLeft)
            {
                moveDirection = MoveDirection.Right;
            }
            else
            {
                moveDirection = MoveDirection.Still;
            }

            discreteActions[0] = (int)moveDirection;
        }

        #endregion
    }
}
    

