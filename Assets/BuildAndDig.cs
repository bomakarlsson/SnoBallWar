using UnityEngine;
using UnityEngine.InputSystem;

public class BuildAndDig : MonoBehaviour
{
    [SerializeField] TilePlacer tilePlacer;

    private Vector2 aimDirection = Vector2.zero;

    
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
               
        if (aimDirection == Vector2.zero)
        {
            aimDirection = Vector2.down;
        }
    }
}
