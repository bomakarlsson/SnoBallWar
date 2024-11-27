using UnityEngine;
using UnityEngine.InputSystem;

public class BuildAndDig : MonoBehaviour
{
    [SerializeField] TilePlacer tilePlacer;
    [SerializeField] float rayDistance = 7f; // Maximum distance for the raytrace
    [SerializeField] float fillRadius = 1f; // Radius of the square to fill

    Vector2 aimDirection = Vector2.zero;

    bool validDig = false;
    bool validBuild = false;
    Vector2 hitPosition = Vector2.zero;

    public void Build(InputAction.CallbackContext context)
    {
        if (validBuild && context.performed)
        {
            tilePlacer.FillSquareWithTiles(hitPosition, fillRadius);
            Debug.Log("Building");
        }
    }

    public void Dig(InputAction.CallbackContext context)
    {
        if (validDig && context.performed)
        {
            tilePlacer.FillSquareWithTiles(hitPosition, fillRadius, false);
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

            RaycastHit2D groundHit = Physics2D.Raycast(ray.origin, ray.direction, rayDistance, LayerMask.GetMask("Ground"));

            if (groundHit.collider != null)
            {
                hitPosition = groundHit.point;
                validDig = true;

                //OBS checks for all colliders in player layer
                RaycastHit2D playerHit = Physics2D.CircleCast(groundHit.point, fillRadius, Vector2.zero, 0f, LayerMask.GetMask("Player"));
                if (playerHit.collider == null)
                {
                    validBuild = true;
                }
                else
                    validBuild = false;
            }
            else
                validDig = false;
        }
    }
}
 