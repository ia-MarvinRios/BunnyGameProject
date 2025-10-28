using UnityEngine;

public class MenuParalax : MonoBehaviour
{
    [Header("Ajustes de movimiento")]
    public float OffsetMultiplier = 1f;
    public float smoothTime = 0.3f;

    private Vector3 startPosition;
    private Vector3 velocity;

    void Start()
    {
        startPosition = transform.position;
        Input.gyro.enabled = true; // Activa el giroscopio en móviles
    }

    void Update()
    {
        Vector2 offset;

#if UNITY_EDITOR || UNITY_STANDALONE
        // ??? En PC: usa el mouse
        offset = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        offset -= new Vector2(0.5f, 0.5f);
#else
        // ?? En móvil: usa el acelerómetro
        Vector3 tilt = Input.acceleration;
        offset = new Vector2(tilt.x, tilt.y);
#endif

        // Aplica el movimiento con suavizado
        Vector3 targetPosition = startPosition + (Vector3)(offset * OffsetMultiplier);
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}