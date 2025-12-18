using UnityEngine;

public class batmanInBatmobile : MonoBehaviour
{
    public float normalSpeed = 5f;
    public float boostSpeed = 8f;
    private float moveSpeed;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private SpriteRenderer sr; // برای flip کردن تصویر
    public float minX = -7f;
    public float maxX = 7f;
    public float minY = -4f;
    public float maxY = 4f;
    public enum BatmanState { Normal, Stealth, Alert }
    public BatmanState currentState = BatmanState.Normal;
    public Light environmentLight; // نور محیط
    public AudioSource alarm;        // صدای آلارم
    public Light redLight;         // نور قرمز چشمک‌زن
    public Light blueLight;        // نور آبی چشمک‌زن
    private Animator animator;      
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // تنظیم سرعت اولیه به سرعت عادی
        moveSpeed = normalSpeed;
        sr = GetComponent<SpriteRenderer>(); 
        animator = GetComponent<Animator>();
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

        // تغییر جهت کاراکتر
        if (moveX > 0)
            sr.flipX = true;
        else if (moveX < 0)
            sr.flipX = false;  

        // تغییر حالت با کلیدها
        if (Input.GetKeyDown(KeyCode.N)) 
            currentState = BatmanState.Normal;
        else if (Input.GetKeyDown(KeyCode.C)) 
            currentState = BatmanState.Stealth;
        else if (Input.GetKeyDown(KeyCode.Space)) 
            currentState = BatmanState.Alert;
        
        // اعمال افکت‌ها و سرعت بر اساس حالت
        switch (currentState)
        {
            case BatmanState.Normal:
                moveSpeed = normalSpeed;
                if (environmentLight != null) environmentLight.intensity = 1f;
                if (alarm != null && alarm.isPlaying) alarm.Stop();
                if (redLight != null) redLight.enabled = false;
                if (blueLight != null) blueLight.enabled = false;
                break;

            case BatmanState.Stealth:
                moveSpeed = normalSpeed * 0.5f;
                if (environmentLight != null) environmentLight.intensity = 0.3f;
                if (alarm != null && alarm.isPlaying) alarm.Stop();
                if (redLight != null) redLight.enabled = false;
                if (blueLight != null) blueLight.enabled = false;
                break;

            case BatmanState.Alert:
                moveSpeed = normalSpeed * 1.2f;
                if (environmentLight != null) environmentLight.intensity = 1f;
                if (alarm != null && !alarm.isPlaying) alarm.Play();
                if (redLight != null) redLight.enabled = Mathf.FloorToInt(Time.time * 2) % 2 == 0;
                if (blueLight != null) blueLight.enabled = Mathf.FloorToInt(Time.time * 2) % 2 == 1;
                break;
        }
        // Animator: تغییر بین Idle و Moving
        if(animator != null) animator.SetFloat("Speed", moveInput.magnitude);
    }

    void FixedUpdate()
    {
        // محاسبه موقعیت جدید
        Vector2 newPos = rb.position + moveInput * moveSpeed * Time.fixedDeltaTime;

        // محدود کردن موقعیت به محدوده صحنه
        newPos.x = Mathf.Clamp(newPos.x, minX, maxX);
        newPos.y = Mathf.Clamp(newPos.y, minY, maxY);

        // حرکت بتمن
        rb.MovePosition(newPos);
    }
}
