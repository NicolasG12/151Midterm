using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Buckets : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private TextMeshProUGUI totalScore;
    [SerializeField] public int score = 0;

    // Start is called before the first frame update
    void Start()
    {
        text.text = score.ToString();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D col)
    {
        totalScore.GetComponent<Score>().score += score;
    }
}
