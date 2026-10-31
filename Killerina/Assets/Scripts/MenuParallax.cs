using UnityEngine;
using UnityEngine.InputSystem;
public class MenuParallax : MonoBehaviour
{
    public float offsetMultiplier = 1f; // Ajusta este valor para controlar el efecto
    public float smoothTime = 0.3f; // Ajusta este valor para controlar la suavidad

    private Vector3 startPosition;
    private Vector3 velocity;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        // 2. Verificamos que el hardware del mouse esté conectado/activo
        if (Mouse.current == null) return;

        // 3. Reemplazo de Input.mousePosition por la lectura del nuevo Input System
        Vector3 mouseScreenPos = Mouse.current.position.ReadValue();

        // El resto de tu lógica matemática original se mantiene intacta
        Vector2 viewportMouse = Camera.main.ScreenToViewportPoint(mouseScreenPos);

        Vector2 offset = new Vector2(viewportMouse.x - 0.5f, viewportMouse.y - 0.5f);

        Vector3 targetPosition = startPosition + new Vector3(offset.x * offsetMultiplier, offset.y * offsetMultiplier, 0);

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
