using UnityEngine;
using UnityEngine.InputSystem;

public class BuildAndDig : MonoBehaviour
{
    [SerializeField] TilePlacer tilePlacer;
    [SerializeField] float rayDistance = 7f; // Maximum distance for the raytrace
    [SerializeField] float squareRadius = 1f; // Radius of the square to fill

    Vector2 aimDirection = Vector2.zero;

    bool validAim = false;
    Vector2 hitPosition = Vector2.zero;

    public void Build(InputAction.CallbackContext context)
    {
        if (validAim && context.performed)
        {
            tilePlacer.FillCircleWithTiles(hitPosition, squareRadius);
            Debug.Log("Building");
        }
    }

    public void Dig(InputAction.CallbackContext context)
    {
        if (validAim && context.performed)
        {
            tilePlacer.FillSquareWithTiles(hitPosition, squareRadius, false);
            Debug.Log("Digging");
        }
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        aimDirection = context.ReadValue<Vector2>().normalized;

        if (aimDirection != Vector2.zero)
        {
            Ray2D ray = new Ray2D(transform.position, aimDirection);

            Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.red);

            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, rayDistance, LayerMask.GetMask("Ground"));

            if (hit.collider != null)
            {
                validAim = true;
                hitPosition = hit.point;
            }
        }
    }
}
