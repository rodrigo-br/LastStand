using UnityEngine;
using UnityEngine.SceneManagement;

public class Aim : MonoBehaviour
{
    [SerializeField] private float velocity = 50;
    [SerializeField] private Collider2D confinementArea;
    Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        Vector3 mouseWorldPosition = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1));
        Vector3 targetPosition = Vector2.Lerp(transform.position, mouseWorldPosition, velocity * Time.deltaTime);

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
}
