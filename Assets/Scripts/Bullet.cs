using UnityEngine;
using UnityEngine.SceneManagement;

public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private float speed;
    [SerializeField] private float shrinkSpeed;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] LayerMask layerMask;
    private bool debugShoot;
    private Vector2 targetPosition;

    private void Update()
    {
        if (debugShoot)
        {
            trailRenderer.emitting = true;
            this.transform.localScale = Vector2.MoveTowards(this.transform.localScale, Vector2.zero, shrinkSpeed * Time.deltaTime);
            this.transform.position = Vector2.MoveTowards(this.transform.position, targetPosition, speed * Time.deltaTime);
            if (Mathf.Abs(Vector2.Distance(this.transform.position, targetPosition)) < 0.2f)
            {
                Collider2D collider = Physics2D.OverlapPoint(transform.position, layerMask);
                if (collider != null)
                {
                    collider.gameObject.SetActive(false);
                }
                this.enabled = false;
            }
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            targetPosition = new Vector2(target.transform.position.x, target.transform.position.y);
            debugShoot = true;
        }
    }
}
