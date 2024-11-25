using UnityEngine;
using UnityEngine.InputSystem;

public class BuildAndDig : MonoBehaviour
{
    [SerializeField] TilePlacer tilePlacer;
    [SerializeField] float rayDistance = 7f; // Maximum distance for the raytrace
    [SerializeField] float radius = 2f; // Radius of the CircleCast

    Vector2 aimDirection = Vector2.zero;

    bool validAim = false;
    Vector2 hitPosition = Vector2.zero;

    public void Build()
    {
        if (validAim)
            FillSquareWithTiles(hitPosition, radius);
    }

    public void Dig()
    {
        if (validAim)
            FillSquareWithTiles(hitPosition, radius, false);
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        aimDirection = context.ReadValue<Vector2>().normalized;

        if (aimDirection != Vector2.zero)
        {
            // Define the origin and direction
            Vector2 origin = transform.position;

            // Perform the CircleCast
            RaycastHit2D[] hits = Physics2D.CircleCastAll(origin, 
                                                          radius, 
                                                          aimDirection, 
                                                          rayDistance);

            foreach (RaycastHit2D hit in hits)
            {
                // Skip hits on the specific layer
                if (hit.collider != null && hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
                    continue;

                validAim = true;
                hitPosition = hit.point;
            }

            // DEBUGGING
            Debug.DrawRay(origin, aimDirection * rayDistance, Color.red);

            // Radius ray 1
            Vector2 radiusStart = origin + aimDirection * rayDistance;
            Debug.DrawRay(radiusStart, aimDirection * radius, Color.green);
            // Radius ray 2
            Vector2 perpendicular = Vector2.Perpendicular(aimDirection).normalized;
            Vector2 perpendicularStart = origin + aimDirection * rayDistance;
            Debug.DrawRay(perpendicularStart, perpendicular * radius, Color.green);
        }    
    }

    private void FillSquareWithTiles(Vector2 center, float radius, bool fill = true)
    {
        for (int x = -Mathf.FloorToInt(radius); x <= Mathf.FloorToInt(radius); x++)
        {
            for (int y = -Mathf.FloorToInt(radius); y <= Mathf.FloorToInt(radius); y++)
            {
                Vector2 position = new Vector2(center.x + x, center.y + y);
                if (fill)
                    tilePlacer.placeTile(position);
                else
                    tilePlacer.removeTile(position);
            }
        }
    }
}