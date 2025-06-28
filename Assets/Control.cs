using UnityEngine;

public class Control : MonoBehaviour
{
    public Rigidbody2D rb;
    public int Tocdo = 4;
    public float TraiPhai;
    public float TrenDuoi;
    public float jumpForce = 8f; // Lực nhảy
    public int jumpCount = 0; // Số lần nhảy đã thực hiện
    public int maxJumps = 2; // Tối đa 2 lần nhảy
    public bool isFacingRight = true;
    public bool isGrounded = false;

    public Transform groundCheck; // Empty object để kiểm tra chân chạm đất
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer; // Layer của nền/đất

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Kiểm tra có đang đứng trên mặt đất không
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Nếu đang chạm đất thì reset số lần nhảy
        if (isGrounded)
        {
            jumpCount = 0;
        }

        // Di chuyển trái/phải
        TraiPhai = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(Tocdo * TraiPhai, rb.linearVelocity.y);

        // Nhảy (W hoặc mũi tên lên)
        if (Input.GetKeyDown(KeyCode.W) && jumpCount < maxJumps)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpCount++;
        }

        // Rơi nhanh khi nhấn S
        if (!isGrounded && Input.GetKey(KeyCode.S))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, -jumpForce);
        }

        // Lật nhân vật khi đổi hướng
        if (isFacingRight && TraiPhai == -1)
        {
            transform.localScale = new Vector3(-3, 3, 1);
            isFacingRight = false;
        }
        else if (!isFacingRight && TraiPhai == 1)
        {
            transform.localScale = new Vector3(3, 3, 1);
            isFacingRight = true;
        }
    }
}
