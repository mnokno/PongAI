using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
        /// Reference to text output for bounce count
        /// </summary>
        [SerializeField] private TextMeshProUGUI bounceCountText;


        /// <summary>
        /// Update is called once per frame
        /// </summary>
        public void Update()
        {
            bounceCountText.text = ball.paddleBounceCount.ToString();
        }
        
        /// <summary>
        /// Getter for ball object currently in play
        /// </summary>
        /// <returns>Game ball object for this match</returns>
        public Ball GetBall()
        {
            return ball;
        }

        /// <summary>
        /// Returns the poison of the opponents paddle
        /// </summary>
        /// <param name="you">Paddle asking for the opponents position</param>
        /// <returns>Position</returns>
        public float GetOpponentPos(PalletAgent you)
        {
            return (you == upperAgent ? lowerAgent : upperAgent).transform.localPosition.x;
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

