using UnityEngine;
using UnityEngine.Rendering.Universal;

public class batmanInBatmobile : MonoBehaviour
{
    //سرعت ها
    public float normalSpeed = 5f;
    public float boostSpeed = 13f;
    private float moveSpeed;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private SpriteRenderer sr;
    //محدوده صفحه و حرکت بتمن
    public float minX = -7f;
    public float maxX = 7f;
    public float minY = -4f;
    public float maxY = 4f;
    // حالت های بتمن در بازی
    public enum BatmanState { Normal, Stealth, Alert }
    public BatmanState currentState = BatmanState.Normal;

    public Light2D globalLight;    // نور اصلی صحنه (Global Light 2D)
    public AudioSource alarm; //صدای الارم
    public Light2D redLight; //نور چشمک زن قرمز
    public Light2D blueLight; //نور چشمک زن ابی

    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        moveSpeed = normalSpeed;

        // خاموش کردن نورها در شروع
        if (redLight) redLight.intensity = 0f;
        if (blueLight) blueLight.intensity = 0f;
    }

    void Update()
    {
        //گرفتن حرکت بتمن از کاربر
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveInput = new Vector2(moveX, moveY).normalized;

        // تغییر جهت رفتن بتمن به چپ و راست با flip
        if (moveX > 0) sr.flipX = true;
        else if (moveX < 0) sr.flipX = false;

        // تغییر حالت های بتمن در بازی
        if (Input.GetKeyDown(KeyCode.N)) currentState = BatmanState.Normal;
        if (Input.GetKeyDown(KeyCode.C)) currentState = BatmanState.Stealth;
        if (Input.GetKeyDown(KeyCode.Space)) currentState = BatmanState.Alert;

        switch (currentState)
        {
            // وقتی حالت نرمال است با نگه داشتن شیفت سرعت زیاد میشود
            case BatmanState.Normal:
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                    moveSpeed = boostSpeed;
                else
                    moveSpeed = normalSpeed;      
                // نور کلی روشن است و نرمال است و الارم و نور های چشمک زن خاموش است          
                if (globalLight) globalLight.color = Color.white;
                if (alarm && alarm.isPlaying) alarm.Stop();
                if (redLight) redLight.intensity = 0f;
                if (blueLight) blueLight.intensity = 0f;
                break;

            case BatmanState.Stealth:
                // در حالت هشدار سرعت حرکت بتمن کم میشود و نور محیط کم و تیره تر میشود
                moveSpeed = normalSpeed * 0.5f;
                if (globalLight) globalLight.color = new Color(0.3f, 0.3f, 0.3f);
                // الارم و نور های چشمک زن در این حالت هم خاموش هستند
                if (alarm && alarm.isPlaying) alarm.Stop();
                if (redLight) redLight.intensity = 0f;
                if (blueLight) blueLight.intensity = 0f;
                break;

            case BatmanState.Alert:
                // سرعت بتمن در این حالت کمی تغییر میکند و رنگ کلی تم زرد میگیرد
                moveSpeed = normalSpeed * 1.2f;
                if (globalLight) globalLight.color = new Color(1f, 0.9f, 0.4f); 
                if (alarm && !alarm.isPlaying) alarm.Play();
                // الارم روشن میشود و چراغ های چشمک زن پشت بت موبیل فعال میشوند
                float blink = Mathf.PingPong(Time.time * 3f, 1f);
                if (redLight) redLight.intensity = blink * 1.5f;    
                if (blueLight) blueLight.intensity = (1f - blink) * 1.5f;
                break;
        }

        if (animator)
            animator.SetFloat("Speed", moveInput.magnitude);
    }

    void FixedUpdate()
    {
        //چک کردن در محدوده بودن حرکت بتمن
        Vector2 newPos = rb.position + moveInput * moveSpeed * Time.fixedDeltaTime;
        newPos.x = Mathf.Clamp(newPos.x, minX, maxX);
        newPos.y = Mathf.Clamp(newPos.y, minY, maxY);
        rb.MovePosition(newPos);
    }
}
