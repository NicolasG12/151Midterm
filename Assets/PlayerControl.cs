using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private bool isMainBall = false;
    [SerializeField] private float minMouseY = 0f;
    [SerializeField] private float destroyY = 0f;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (isMainBall)
        {
            rb.gravityScale = 0;
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
                SpawnBall();
            }
        }
        else
        {
            if (transform.position.y < destroyY)
            {
                Destroy(gameObject);
            }
        }
    }

    void SpawnBall()
    {
        GameObject ball = Instantiate(gameObject, transform.position, transform.rotation);
        ball.GetComponent<PlayerControl>().isMainBall = false;
        ball.GetComponent<Rigidbody2D>().gravityScale = 5;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Peg"))
        {
            float pegX = collision.gameObject.transform.position.x;
            float pegY = collision.gameObject.transform.position.y;
            // Get the OSC Handler
            GameObject oscHandler = GameObject.Find("@OSCHandler");
            // Get the OSC script
            OSCHandler osc = oscHandler.GetComponent<OSCHandler>();
            // Send the OSC message
            osc.SendMessageToClient("PureData", "/unity/peg", new List<float> { pegX, pegY });
        }
    }
}
