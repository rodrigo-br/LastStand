using UnityEngine;
using UnityEngine.SceneManagement;

public class Aim : MonoBehaviour
{
    [SerializeField] private float velocity = 50;
    [SerializeField] private Collider2D confinementArea;
    [SerializeField] private float cursorSpeed = 20f;
    private Camera cam;
    private Vector3 mouseWorldPosition = Vector3.zero;
    private Vector3 gamepadStickInput = Vector3.zero;
    private Vector3 customVirtualMouse;
    private bool isMovingMouse = false;
    private bool lockMouse = false;

    private void Awake()
    {
        cam = Camera.main;
        customVirtualMouse = newVirtualPosition();
    }

    private void OnEnable()
    {
        GameManager.Instance.InputObserver.OnCursorMoveAction += CheckCursorPosition;
        GameManager.Instance.InputObserver.OnGamepadStickAction += CheckGamepadStickDirection;
        GameManager.OnResetLevel += ResetLevel;
    }

    private void ResetLevel(bool obj)
    {
        lockMouse = false;
    }

    private void OnDisable()
    {
        GameManager.Instance.InputObserver.OnCursorMoveAction -= CheckCursorPosition;
        GameManager.Instance.InputObserver.OnGamepadStickAction -= CheckGamepadStickDirection;
    }

    private void CheckGamepadStickDirection(Vector2 pos)
    {
        lockMouse = true;
        isMovingMouse = false;
        Vector3 gamestickMovement = (Vector3)(cursorSpeed * Time.deltaTime * pos);
        gamepadStickInput = gamestickMovement;
    }

    private void CheckCursorPosition(Vector2 pos)
    {
        if (lockMouse) { return; }
        isMovingMouse = true;
        if (pos == Vector2.zero)
        {
            pos = newVirtualPosition();
        }
        mouseWorldPosition = cam.ScreenToWorldPoint(new Vector3(pos.x, pos.y));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (isMovingMouse)
        {
            customVirtualMouse = Vector2.MoveTowards(customVirtualMouse, mouseWorldPosition, Time.deltaTime * cursorSpeed);
        }
        else
        {
            customVirtualMouse += gamepadStickInput;
            customVirtualMouse.x = Mathf.Clamp(customVirtualMouse.x, -1.5f, 5.5f);
            customVirtualMouse.y = Mathf.Clamp(customVirtualMouse.y, -1.5f, 5.5f);
        }

        Vector3 targetPosition = Vector2.Lerp(transform.position, customVirtualMouse, velocity * Time.deltaTime);

        if (confinementArea.OverlapPoint(targetPosition))
        {
            transform.position = targetPosition;
            return;
        }
        transform.position = GetClosestPointInBounds(targetPosition);
    }

    private Vector3 GetClosestPointInBounds(Vector3 targetPosition)
    {
        Vector3 closestPoint = confinementArea.ClosestPoint(targetPosition);
        return new Vector3(closestPoint.x, closestPoint.y, transform.position.z);
    }

    private Vector2 newVirtualPosition()
    {
        return new Vector2(Random.Range(200, 380), Random.Range(80, 250));
    }
}
