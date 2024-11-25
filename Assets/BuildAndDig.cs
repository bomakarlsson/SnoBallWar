using UnityEngine;
using UnityEngine.InputSystem;

public class BuildAndDig : MonoBehaviour
{
    [SerializeField] TilePlacer tilePlacer;
    [SerializeField] float rayDistance = 10f; // Maximum distance for the raytrace

    Vector2 aimDirection = Vector2.zero;

    public void Build()
    {
        Debug.Log("Building");
        tilePlacer.placeTile(transform.position);
    }

    public void Dig()
    {
        Debug.Log("Digging");
        tilePlacer.removeTile(transform.position);
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        aimDirection = context.ReadValue<Vector2>().normalized;

        if (aimDirection != Vector2.zero)
        {
            Ray2D ray = new Ray2D(transform.position, aimDirection);

            Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.red);

            // 3 is Ground layer
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, rayDistance, LayerMask.GetMask("Ground"));

            if (hit.collider != null)
            {
                tilePlacer.placeTile(hit.point);
            }
        }
    }
}