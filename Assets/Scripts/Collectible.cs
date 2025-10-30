using UnityEngine;

public class Collectible : MonoBehaviour
{
    public int value = 1;
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collectible triggered by: " + other.name);
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.AddScore(value);
            Destroy(gameObject);
        }
    }
}