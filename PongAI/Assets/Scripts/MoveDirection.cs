using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PongAI
{
    /// <summary>
    /// Describes direction of movement for the paddle
    /// </summary>
    public enum MoveDirection
    {
        Still = 0, // Not moving
        Left = 1,  // Moving left
        Right = 2  // Moving right
    }
}
