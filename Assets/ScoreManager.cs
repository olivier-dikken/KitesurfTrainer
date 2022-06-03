using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public int score;
   
    // make singleton
    public static ScoreManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void Update()
    {
        scoreText.text = "Score: " + score;
    }
}