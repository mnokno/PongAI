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
        /// Height of the board
        /// </summary>
        public float boardHeight = 10f;

        /// <summary>
        /// Update is called once per frame
        /// </summary>
        public void Update()
        {
            bounceCountText.text = ball.paddleBounceCount.ToString();
            if (ball.paddleBounceCount > 50)
            {
                // Small negative punishment for not ending the game
                upperAgent.AddReward(-0.05f);
                lowerAgent.AddReward(-0.05f);
                
                upperAgent.EndEpisode();
                lowerAgent.EndEpisode();
                upperAgent.ResetPosition();
                lowerAgent.ResetPosition();

                ball.ResetBall();
            }
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
            float winRew = 1;
            float lossRew = -1;
            if (ball.transform.localPosition.z > 0)
            {
                upperAgent.AddReward(lossRew);
                lowerAgent.AddReward(winRew);
            }
            else
            {
                upperAgent.AddReward(winRew);
                lowerAgent.AddReward(lossRew);
            }

            upperAgent.EndEpisode();
            lowerAgent.EndEpisode();
            upperAgent.ResetPosition();
            lowerAgent.ResetPosition();

            ball.ResetBall();
        }
    }
}

