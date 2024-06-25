using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    public float scrollSpeed = 0.5f;
    private float tileSizeX; // Ukuran horizontal dari sprite
    private Vector3 startPosition;

    void Start()
    {
        // Menyimpan posisi awal background
        startPosition = transform.position;

        // Mendapatkan ukuran horizontal dari sprite
        tileSizeX = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        // Menghitung offset berdasarkan waktu dan kecepatan scroll
        float newPosition = Mathf.Repeat(Time.time * scrollSpeed, tileSizeX);

        // Mengubah posisi background dengan offset yang dihitung
        transform.position = startPosition + Vector3.left * newPosition;
    }
}
