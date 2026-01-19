using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public float moveSpeed = 7;
    public float jumpForce = 5f;
    Vector3 initScale;
    public Animator animator;
    public bool canJump = true;
    public bool isClimbing = false;
    public bool climbingUp = false;
    public bool climbingDown = false;
    public GameObject projectilePrefab;
    int score = 0;

    void Start()
    {
        initScale = transform.localScale;
    }

    void Update()
    {
        float moveInput = Input.GetAxis("Horizontal");
        float climbInput = Input.GetAxis("Vertical");
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
        if (Input.GetButtonDown("Jump"))
        {
            if (canJump && Mathf.Abs(rb.linearVelocity.y) < 0.001f)
            {
                rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
                animator.SetBool("isJump", true);
                canJump = false;
            }
        }

        if (isClimbing)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, climbInput * moveSpeed);
        }

        if(Input.GetMouseButtonDown(0))
        {
            var rotationZ = transform.localScale.x > 0 ? 0 : 180;
            Instantiate(projectilePrefab, transform.position, Quaternion.Euler(0, 0, rotationZ + 90));
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        animator.SetBool("isJump", false);
        canJump = true;
        if (collision.gameObject.CompareTag("Enemy"))
        {
            gameObject.SetActive(false);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ladder"))
        {
            isClimbing = true;
            rb.gravityScale = 0;
        }
        if(other.gameObject.CompareTag("Coin"))
        {
            score += 1;
            Destroy(other.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ladder"))
        {
            isClimbing = false;
            rb.gravityScale = 2;
        }
    }

}
