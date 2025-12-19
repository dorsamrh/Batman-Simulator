using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BatmanLogoController : MonoBehaviour
{
    public Light2D batLight;             // نور Bat-Signal
    public float moveAmplitude = 1f;     // دامنه حرکت
    public float moveFrequency = 1f;     // سرعت حرکت

    private Vector3 startPos;

    void Start()
    {

        // خاموش کردن نور در ابتدا
        batLight.enabled = false;

        // ذخیره موقعیت اولیه
        startPos = batLight.transform.position;
    }

    void Update()
    {
        if (batLight == null) return;

        // روشن/خاموش با کلید B
        if (Input.GetKeyDown(KeyCode.B))
            batLight.enabled = !batLight.enabled;

        if (batLight.enabled)
        {
            // حرکت آرام سینوسی X و Y
            // برای اینکه نشان بتمن در اسمان حرکت کند
            float offsetX = Mathf.Sin(Time.time * moveFrequency) * moveAmplitude;
            float offsetY = Mathf.Cos(Time.time * moveFrequency) * moveAmplitude;
            batLight.transform.position = new Vector3(startPos.x + offsetX, startPos.y + offsetY, startPos.z);
        }
        else
        {
            // برگشت به موقعیت اولیه
            batLight.transform.position = startPos;
        }
    }
}
