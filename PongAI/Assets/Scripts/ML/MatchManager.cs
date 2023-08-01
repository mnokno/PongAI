using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PongAI
{
    /// <summary>
    /// Manages a pong game
    /// </summary>
    public class MatchManager : MonoBehaviour
    {
        /// <summary>
        /// References to the agent thats on the top of the screen, positive local z coordinate
        /// </summary>
        [SerializeField] private PalletAgent upperAgent;
        /// <summary>
        /// References to the agent thats at the bottom of the screen, negative local z coordinate
        /// </summary>
        [SerializeField] private PalletAgent lowerAgent;
        /// <summary>
        /// Reference to the ball object
        /// </summary>
        [SerializeField] private Ball ball;

        /// <summary>
        /// Getter for ball object currently in play
        /// </summary>
        /// <returns>Game ball object for this match</returns>
        public Ball GetBall()
        {
            return ball;
        }

        /// <summary>
        /// Should be called when a ball has past a paddle
        /// </summary>
        public void BallPassedPaddle()
        {
            float reward = ball.transform.localPosition.z > 0 ? 1f : -1f;
            upperAgent.AddReward(reward);
            lowerAgent.AddReward(-reward);
            upperAgent.EndEpisode();
            lowerAgent.EndEpisode();
            upperAgent.ResetPosition();
            lowerAgent.ResetPosition();

            ball.ResetBall();
        }
    }
}

