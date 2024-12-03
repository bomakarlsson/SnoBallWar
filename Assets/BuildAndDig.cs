using System;
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
    [SerializeField] bool drawIndicators = true;

    Vector2 aimDirection = Vector2.zero;

    bool isInBuildMode = false;
    bool validBuild, validDig = false;
    Vector2 hitPosition = Vector2.zero;

    Coroutine buildCoroutine, digCoroutine;

    DrawLineTo aimDrawer;
    SpriteRenderer BuildIndicator, DigIndicator;

    void Awake()
    {
        if (tilePlacer == null)
            tilePlacer = FindObjectOfType<TilePlacer>();

        aimDrawer = GetComponent<DrawLineTo>();
        aimDrawer.enabled = false;

        BuildIndicator = transform.GetChild(1).GetComponent<SpriteRenderer>();
        BuildIndicator.transform.localScale = Vector3.one * fillRadius;
        BuildIndicator.enabled = false;

        DigIndicator = transform.GetChild(0).GetComponent<SpriteRenderer>();
        DigIndicator.transform.localScale = Vector3.one * fillRadius * digFillMultiplier;
        DigIndicator.enabled = false;
    }

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
        yield return new WaitForSeconds(buildTime);

        Vector2 firstBuildPosition = hitPosition;
        bool isBuildingConsecutive = false;
        Vector2 buildDirection = Vector2.zero;
        int i = 0;
       
        while (true)
        {
            if (!isBuildingConsecutive)
            {
                if (validBuild)
                {
                    if (squareBuild)
                        tilePlacer.FillSquareWithTiles(hitPosition, fillRadius);
                    else
                        tilePlacer.FillCircleWithTiles(hitPosition, fillRadius);
                    Debug.Log("Building");
                }

                Vector2 difference = hitPosition - firstBuildPosition;

                if (difference.magnitude > fillRadius / 2)
                {
                    isBuildingConsecutive = true;
                    buildDirection = hitPosition - firstBuildPosition;
                    buildDirection.Normalize();
                    i = 1;
                }
            }
            else
            { 
                i++;
                Vector2 newPlacePosition = firstBuildPosition + buildDirection * fillRadius / 2 * i;
                if (Vector2.Distance(newPlacePosition, transform.position) <= aimDistance && 
                    GetPlayerHit(newPlacePosition).collider == null)
                {
                    if (squareBuild)
                        tilePlacer.FillSquareWithTiles(newPlacePosition, fillRadius);
                    else
                        tilePlacer.FillCircleWithTiles(newPlacePosition, fillRadius);
                    Debug.Log("Building");
                }
            }
            
            yield return new WaitForSeconds(buildTime);
        }
    }

    private IEnumerator Digging()
    {
        yield return new WaitForSeconds(digTime);

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
            yield return new WaitForSeconds(digTime);
        }
    }

    public void OnBuildMode(InputAction.CallbackContext context)
    {
        isInBuildMode = context.performed;

        if (!isInBuildMode)
        {
            aimDrawer.enabled = false;
            DigIndicator.enabled = false;
            BuildIndicator.enabled = false;
        }
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        if (!isInBuildMode)
        {
            return;
        }

        if (context.control.device.name == "Mouse")
        {
            Vector3 mousePosition = context.ReadValue<Vector2>();
            Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, 
                                                                                    mousePosition.y, 
                                                                                    Camera.main.transform.position.z * -1));
            aimDirection = (worldMousePosition - transform.position).normalized;
        }
        else
            aimDirection = context.ReadValue<Vector2>().normalized;
        
        if (aimDirection == Vector2.zero)
        {
            aimDrawer.enabled = false;
            DigIndicator.enabled = false;
            BuildIndicator.enabled = false;
            return;
        }

        Ray2D ray = new Ray2D(transform.position, aimDirection);
        
        if (drawIndicators)
        {
            // Debug.DrawRay(ray.origin, ray.direction * aimDistance, Color.red);
            aimDrawer.target = (Vector2)transform.position + aimDirection * aimDistance;
            aimDrawer.enabled = true;
        }

        RaycastHit2D groundHit = Physics2D.Raycast(ray.origin, ray.direction, aimDistance, LayerMask.GetMask("Ground"));
        if (groundHit.collider == null)
        {
            validDig = false;
            validBuild = false;
            DigIndicator.enabled = false;
            BuildIndicator.enabled = false;
            return;
        }

        hitPosition = groundHit.point;
        validDig = true;

        if (drawIndicators)
        {
            DigIndicator.transform.position = groundHit.point;
            DigIndicator.enabled = true;
        }

        if (GetPlayerHit(hitPosition).collider == null)
        {
            validBuild = true;
            if (drawIndicators)
            {
                BuildIndicator.transform.position = groundHit.point;
                BuildIndicator.enabled = true;
            }
        }
        else
        {
            validBuild = false;
            BuildIndicator.enabled = false;
        }
    }

    // OBS checks for all colliders in player layer
    // (should maybe be square cast)
    RaycastHit2D GetPlayerHit(Vector2 fillPosition)
    {
        return Physics2D.CircleCast(fillPosition, fillRadius, Vector2.zero, 0f, LayerMask.GetMask("Player"));
    }
}