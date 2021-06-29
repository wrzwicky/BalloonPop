using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Manage the game score on screen.
/// </summary>
public class ScoreKeeper : MonoBehaviour
{
    public Text scoreText;
    private int _score;

    public int Score
    {
        get { return _score; }
        set
        {
            _score = value;
            scoreText.text = "Score: " + _score;
        }
    }

    // Use this for initialization
    void Start()
    {
        scoreText.text = "Score: " + _score;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
