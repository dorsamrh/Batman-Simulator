using UnityEngine;

public class batmanInBatmobile : MonoBehaviour
{
    public float normalSpeed = 5f;
    public float boostSpeed = 8f;
    private float moveSpeed;

    private Rigidbody2D rb;
    private Vector2 moveInput;

    /// <summary>
    /// تابع Start زمانی که اسکریپت شروع به کار می‌کند اجرا می‌شود.
    /// </summary>
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // تنظیم سرعت اولیه به سرعت عادی
        moveSpeed = normalSpeed;
    }
    /// <summary>
    /// Update هر فریم اجرا می‌شود و ورودی کاربر و تغییر سرعت را بررسی می‌کند.
    /// </summary>
    void Update()
    {
        // دریافت ورودی کاربر (افقی: A/D یا چپ/راست، عمودی: W/S یا بالا/پایین)
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveInput = new Vector2(moveX, moveY).normalized; // نرمالیزه کردن برای حرکت یکسان در تمام جهات

        // بررسی نگه داشتن Shift برای فعال کردن بوست
        if (Input.GetKey(KeyCode.LeftShift) | Input.GetKey(KeyCode.RightShift))
            moveSpeed = boostSpeed; // سرعت بیشتر هنگام Shift
        else
            moveSpeed = normalSpeed; // سرعت عادی در غیر این صورت
    }

    void FixedUpdate()
    {
        // حرکت کاراکتر با Rigidbody2D
        // rb.MovePosition: موقعیت جدید کاراکتر را با توجه به ورودی و سرعت تعیین می‌کند
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
    }
}
