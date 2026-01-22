using TMPro;
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
    public bool isClimbing = false;
    public bool climbingUp = false;
    public bool climbingDown = false;
    public TextMeshProUGUI scoreText;
    public GameObject projectilePrefab;

    void Start()
    {
        initScale = transform.localScale;
        UpdateScoreUI();
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
            ScoreManager.Instance?.Reset();
            UpdateScoreUI();
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
            ScoreManager.Instance?.Add(1);
            UpdateScoreUI();
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("Finish"))
        {
            var activeScene = SceneManager.GetActiveScene();
            if (activeScene.name == "lv1")
            {
                SceneManager.LoadScene("lv2");
            }
            else if (activeScene.name == "lv2")
            { 
                 SceneManager.LoadScene("lv1");
            }
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

    void UpdateScoreUI()
    {
        if (scoreText == null)
        {
            return;
        }

        var currentScore = ScoreManager.Instance != null ? ScoreManager.Instance.Score : 0;
        scoreText.text = "Score: " + currentScore;
    }

}
