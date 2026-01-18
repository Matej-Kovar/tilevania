using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public float moveSpeed = 7;
    public float jumpForce = 5f;
    Vector3 initScale;
    public Animator animator;
    public bool canJump = true;

    void Start()
    {
        initScale = transform.localScale;
    }

    void Update()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        if(moveInput != 0)
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }
        if (moveInput > 0)
            transform.localScale = initScale;
        else if (moveInput < 0)
            transform.localScale = new Vector3(-initScale.x, initScale.y, initScale.z);
        if (canJump && Input.GetButtonDown("Jump") && Mathf.Abs(rb.linearVelocity.y) < 0.001f)
        {
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            animator.SetBool("isJump", true);
canJump = false;

        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        animator.SetBool("isJump", false);
        canJump = true;
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
