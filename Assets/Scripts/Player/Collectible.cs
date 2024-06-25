using UnityEngine;

public class Collectible : MonoBehaviour
{
    private bool canBeCollected = false;

    private void Start()
    {
        // Awalnya collectible tidak bisa diambil
        canBeCollected = false;
    }

    public void EnableCollection()
    {
        canBeCollected = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && canBeCollected)
        {
            GameData.Instance.CollectedItemCount++;
            // Tambahkan logika untuk event item dikumpulkan jika diperlukan
            Destroy(gameObject); // Hapus collectible dari scene
        }
    }
}
