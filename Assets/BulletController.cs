using UnityEngine;

public class BulletController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.Rotate(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.down * 10f * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
