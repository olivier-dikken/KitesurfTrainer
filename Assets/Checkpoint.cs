using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Path parentPath;
    private MeshRenderer _renderer;
    public ScoreManager scoreManager;

    [SerializeField] private Color discoveredColor;

    private void Start()
    {
        _renderer = GetComponent<MeshRenderer>();
    }


    public void SetDisplacement()
    {
        transform.position.Set(0, 0, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Increment Score
        scoreManager.score += 1;
        _renderer.material.color = discoveredColor;
    }
}