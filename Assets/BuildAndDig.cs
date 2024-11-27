using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildAndDig : MonoBehaviour
{
    [SerializeField] TilePlacer tilePlacer;
    [SerializeField] float aimDistance = 7f;
    [SerializeField] float fillRadius = 1f;
    [SerializeField] float digFillMultiplier = 1f;
    [SerializeField] float buildTime, digTime = 0.5f;
    [SerializeField] bool squareBuild, squareDig = false;


    Vector2 aimDirection = Vector2.zero;

    bool validBuild, validDig = false;
    Vector2 hitPosition = Vector2.zero;

    private Coroutine buildCoroutine, digCoroutine;

    public void Build(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (buildCoroutine == null)
            {
                buildCoroutine = StartCoroutine(Building());
            }
        }
        else if (context.canceled)
        {
            if (buildCoroutine != null)
            {
                StopCoroutine(buildCoroutine);
                buildCoroutine = null;
            }
        }
    }
    
    public void Dig(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (digCoroutine == null)
            {
                digCoroutine = StartCoroutine(Digging());
            }
        }
        else if (context.canceled)
        {
            if (digCoroutine != null)
            {
                StopCoroutine(digCoroutine);
                digCoroutine = null;
            }
        }
    }

    private IEnumerator Building()
    {
        while (true)
        {
            if (validBuild)
            {
                if (squareBuild)
                    tilePlacer.FillSquareWithTiles(hitPosition, fillRadius);
                else
                    tilePlacer.FillCircleWithTiles(hitPosition, fillRadius);
                Debug.Log("Building");
            }
            yield return new WaitForSeconds(buildTime); // Continue in the next frame
        }
    }

    private IEnumerator Digging()
    {
        while (true)
        {
            if (validDig)
            {
                if (squareDig)
                    tilePlacer.FillSquareWithTiles(hitPosition, fillRadius * digFillMultiplier, false);
                else
                    tilePlacer.FillCircleWithTiles(hitPosition, fillRadius * digFillMultiplier, false);
                Debug.Log("Digging");
            }
            yield return new WaitForSeconds(digTime); // Continue in the next frame
        }
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        aimDirection = context.ReadValue<Vector2>().normalized;

        if (aimDirection != Vector2.zero)
        {
            Ray2D ray = new Ray2D(transform.position, aimDirection);
            Debug.DrawRay(ray.origin, ray.direction * aimDistance, Color.red);

            RaycastHit2D groundHit = Physics2D.Raycast(ray.origin, ray.direction, aimDistance, LayerMask.GetMask("Ground"));

            if (groundHit.collider != null)
            {
                hitPosition = groundHit.point;
                validDig = true;

                // OBS checks for all colliders in player layer
                // (should maybe be square cast)
                RaycastHit2D playerHit = Physics2D.CircleCast(groundHit.point, fillRadius, Vector2.zero, 0f, LayerMask.GetMask("Player"));
                if (playerHit.collider == null)
                    validBuild = true;
                else
                    validBuild = false;
            }
            else
            {
                validDig = false;
                validBuild = false;
            }
        }
    }
}