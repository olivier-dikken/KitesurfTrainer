using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    public int score;

    public void Update()
    {
        scoreText.text = "Score: " + score;
    }
}