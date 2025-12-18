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
    public SpriteRenderer backgroundSprite; // نور محیط
    public AudioSource alarm;        // صدای آلارم
    public SpriteRenderer redLight;
    public SpriteRenderer blueLight;

    private Animator animator;      
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // تنظیم سرعت اولیه به سرعت عادی
        moveSpeed = normalSpeed;
        sr = GetComponent<SpriteRenderer>(); 
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // دریافت ورودی کاربر (افقی: A/D یا چپ/راست، عمودی: W/S یا بالا/پایین)
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveInput = new Vector2(moveX, moveY).normalized; // نرمالیزه کردن برای حرکت یکسان در تمام جهات


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
                // بررسی نگه داشتن Shift برای فعال کردن بوست
                if (Input.GetKey(KeyCode.LeftShift) | Input.GetKey(KeyCode.RightShift))
                    moveSpeed = boostSpeed; // سرعت بیشتر هنگام Shift
                else
                    moveSpeed = normalSpeed; // سرعت عادی در غیر این صورت
                if (backgroundSprite  != null) backgroundSprite.color = Color.white;
                if(sr != null) sr.color = Color.white;
                if (alarm != null && alarm.isPlaying) alarm.Stop();
                if (redLight != null) redLight.enabled = false;
                if (blueLight != null) blueLight.enabled = false;
                break;

            case BatmanState.Stealth:
                moveSpeed = normalSpeed * 0.5f;
                if (backgroundSprite  != null) backgroundSprite.color = new Color(0.3f, 0.3f, 0.3f);
                if(sr != null) sr.color = new Color(0.5f, 0.5f, 0.5f);
                if (alarm != null && alarm.isPlaying) alarm.Stop();
                if (redLight != null) redLight.enabled = false;
                if (blueLight != null) blueLight.enabled = false;
                break;

            case BatmanState.Alert:
                moveSpeed = normalSpeed * 1.2f;
                if (backgroundSprite  != null) backgroundSprite.color = Color.yellow;
                if(sr != null) sr.color = Color.yellow;
                if (alarm != null && !alarm.isPlaying) alarm.Play();
                float blink = Mathf.PingPong(Time.time * 2f, 1f); // بین 0 و 1
                if (redLight != null) redLight.color = new Color(1f, 0f, 0f, blink);
                if (blueLight != null) blueLight.color = new Color(0f, 0f, 1f, 1f - blink);
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
