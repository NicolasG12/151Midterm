using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private bool isMainBall = false;
    [SerializeField] private float minY = 0f;

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
            if (transform.position.y < minY)
            {
                transform.position = new Vector2(transform.position.x, minY);
            }
            if (Input.GetMouseButtonDown(0))
            {
                SpawnBall();
            }
        }
    }

    void SpawnBall()
    {
        GameObject ball = Instantiate(gameObject, transform.position, transform.rotation);
        ball.GetComponent<PlayerControl>().isMainBall = false;
        ball.GetComponent<Rigidbody2D>().gravityScale = 5;
    }
}
