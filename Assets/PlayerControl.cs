using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private bool isMainBall = false;
    [SerializeField] private float minMouseY = 0f;
    [SerializeField] private float destroyY = 0f;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private GameObject OSCHandler;
    private OSCHandler osc;

    // Start is called before the first frame update
    void Start()
    {
        OSCHandler = GameObject.Find("@OSCHandler");
        osc = OSCHandler.GetComponent<OSCHandler>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        if (isMainBall)
        {
            rb.gravityScale = 0;
            osc.SendMessageToClient("PureData", "/unity/bgm", 1);
        }
        else
        {
            sr.color = Color.HSVToRGB(Random.Range(0f, 1f), 1f, 1f);
        }

    }

    void OnApplicationQuit()
    {
        if (isMainBall)
        {
            osc.SendMessageToClient("PureData", "/unity/bgm", 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isMainBall)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mousePos;
            if (transform.position.y < minMouseY)
            {
                transform.position = new Vector2(transform.position.x, minMouseY);
            }
            if (Input.GetMouseButtonDown(0))
            {
                // Get the number of balls
                GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");
                int numBalls = balls.Length;
                if (numBalls == 0)
                {
                    SpawnBall();
                }

            }
        }
        else
        {
            if (transform.position.y < destroyY)
            {
                osc.SendMessageToClient("PureData", "/unity/bucket", 50);
                Destroy(gameObject);
            }
        }
    }

    void SpawnBall()
    {
        GameObject ball = Instantiate(gameObject, transform.position, transform.rotation);
        ball.GetComponent<PlayerControl>().isMainBall = false;
        ball.GetComponent<Rigidbody2D>().gravityScale = 5;
        ball.tag = "Ball";
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Peg"))
        {
            float pegX = collision.gameObject.transform.position.x;
            float pegY = collision.gameObject.transform.position.y;
            // // Get the OSC Handler
            // GameObject oscHandler = GameObject.Find("@OSCHandler");
            // // Get the OSC script
            // OSCHandler osc = oscHandler.GetComponent<OSCHandler>();
            // Send the OSC message
            osc.SendMessageToClient("PureData", "/unity/peg", new List<float> { pegX, pegY });

            // Get the peg's sprite renderer
            SpriteRenderer pegSR = collision.gameObject.GetComponent<SpriteRenderer>();
            Color ballColor = sr.color;
            Color.RGBToHSV(ballColor, out float ballH, out float ballS, out float ballV);
            pegSR.color = Color.HSVToRGB(ballH, ballS, 0.85f * ballV);
        }
        if (collision.gameObject.CompareTag("Bucket"))
        {
            // GameObject oscHandler = GameObject.Find("@OSCHandler");
            // OSCHandler osc = oscHandler.GetComponent<OSCHandler>();
            int score = collision.gameObject.GetComponent<Buckets>().score;
            osc.SendMessageToClient("PureData", "/unity/bucket", score);
            Destroy(gameObject);
        }
    }
}
