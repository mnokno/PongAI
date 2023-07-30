using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float speed = 10f;
    public float width = 6;
    public float height = 10;
    public float ballRadius = 0.25f;
    public float speedIncreaseOnBounce = 1.1f;
    private bool isBouncing = false;
    private Rigidbody rb;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Set the initial velocity of the ball
        rb.velocity = new Vector3(speed, 0f, speed);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isBouncing)
        {
            if (Mathf.Abs(transform.localPosition.x) > width * 0.5f - ballRadius)
            {
                rb.velocity = new Vector3(-rb.velocity.x, 0f, rb.velocity.z) * speedIncreaseOnBounce;
            }

            if (Mathf.Abs(transform.position.z) > height * 0.5f - ballRadius)
            {
                rb.velocity = new Vector3(speed, 0f, speed);
                transform.localPosition = new Vector3(0f, transform.localPosition.y, 0f);
            }

            isBouncing = true;
            // Delay the reset of 'isBouncing' to prevent multiple bounces in quick succession
            Invoke("ResetBouncing", 0.1f);
        }
    }

    private void ResetBouncing()
    {
        isBouncing = false;
    }
}
