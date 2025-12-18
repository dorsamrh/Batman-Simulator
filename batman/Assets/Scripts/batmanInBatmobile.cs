using UnityEngine;
using UnityEngine.Rendering.Universal;

public class batmanInBatmobile : MonoBehaviour
{
    public float normalSpeed = 5f;
    public float boostSpeed = 13f;
    private float moveSpeed;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private SpriteRenderer sr;
    public float minX = -7f;
    public float maxX = 7f;
    public float minY = -4f;
    public float maxY = 4f;

    public enum BatmanState { Normal, Stealth, Alert }
    public BatmanState currentState = BatmanState.Normal;

    public Light2D globalLight;    // نور اصلی صحنه (Global Light 2D)
    public AudioSource alarm;
    public Light2D redLight;
    public Light2D blueLight;

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
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveInput = new Vector2(moveX, moveY).normalized;

        // Flip
        if (moveX > 0) sr.flipX = true;
        else if (moveX < 0) sr.flipX = false;

        // تغییر حالت
        if (Input.GetKeyDown(KeyCode.N)) currentState = BatmanState.Normal;
        if (Input.GetKeyDown(KeyCode.C)) currentState = BatmanState.Stealth;
        if (Input.GetKeyDown(KeyCode.Space)) currentState = BatmanState.Alert;

        switch (currentState)
        {
            case BatmanState.Normal:
                moveSpeed = Input.GetKey(KeyCode.LeftShift | KeyCode.RightShift) ? boostSpeed : normalSpeed;
                if (globalLight) globalLight.intensity = 1f;
                if (alarm && alarm.isPlaying) alarm.Stop();
                if (redLight) redLight.intensity = 0f;
                if (blueLight) blueLight.intensity = 0f;
                break;

            case BatmanState.Stealth:
                moveSpeed = normalSpeed * 0.5f;
                if (globalLight) globalLight.intensity = 0.3f;
                if (alarm && alarm.isPlaying) alarm.Stop();
                if (redLight) redLight.intensity = 0f;
                if (blueLight) blueLight.intensity = 0f;
                break;

            case BatmanState.Alert:
                moveSpeed = normalSpeed * 1.2f;
                if (globalLight) globalLight.intensity = 1.2f;
                if (alarm && !alarm.isPlaying) alarm.Play();

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
        Vector2 newPos = rb.position + moveInput * moveSpeed * Time.fixedDeltaTime;
        newPos.x = Mathf.Clamp(newPos.x, minX, maxX);
        newPos.y = Mathf.Clamp(newPos.y, minY, maxY);
        rb.MovePosition(newPos);
    }
}
