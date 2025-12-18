using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BatmanLogoController : MonoBehaviour
{
    public Light2D batLight;
    public float rotationSpeed = 10f; // سرعت چرخش
    public float moveAmplitude = 2f;  // دامنه حرکت سینوسی
    public float moveFrequency = 1f;  // سرعت حرکت

    private Vector3 startPos;

    void Start()
    {
        if (batLight)
            batLight.enabled = false;

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
            // چرخش آرام
            batLight.transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);

            // حرکت آرام سینوسی
            float offset = Mathf.Sin(Time.time * moveFrequency) * moveAmplitude;
            batLight.transform.position = new Vector3(startPos.x + offset, startPos.y, startPos.z);
        }
        else
        {
            // برگشت به موقعیت اولیه
            batLight.transform.position = startPos;
        }
    }
}
