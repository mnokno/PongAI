using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

namespace PongAI
{
    public class PalletAgent : Agent
    {
        /// <summary>
        /// Stores the current movement direction of the paddle
        /// </summary>
        private MoveDirection moveDirection = MoveDirection.Still;
        /// <summary>
        /// Weather or note the paddle can be moved using arrow keys
        /// </summary>
        public bool useArrowKeys = true;
        /// <summary>
        /// Weather or note the paddle can be moved using A and D keys
        /// </summary>
        public bool useAD = true;
        /// <summary>
        /// Speed of the paddle movement
        /// </summary>
        public float speed = 4f;
        /// <summary>
        /// Prevents the paddle from moving outside of the board
        /// </summary>
        public float boardWidth = 6f;

        public override void OnActionReceived(ActionBuffers actions)
        {
            // 0 no move, 1 left, 2 right
            if (actions.DiscreteActions[0] == (int)MoveDirection.Still)
            {
                // No moment
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
            base.CollectObservations(sensor);
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
    }
}
    

