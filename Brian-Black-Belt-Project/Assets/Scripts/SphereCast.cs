using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SphereCast : MonoBehaviour
{
    [Header("Detection")]
    [SerializeField] private float castDistance = 3f;
    [SerializeField] private float castRadius = 0.5f;
    [SerializeField] private LayerMask pickupLayer;
    [SerializeField] private Transform castOrigin; // usually your camera

    [Header("UI Prompt")]
    [SerializeField] private CanvasGroup interactCircleGroup;
    [SerializeField] private float fadeSpeed = 8f;

    [Header("Player")]
    [SerializeField] private Player player;

    private ItemWorld currentTarget;
    private float targetAlpha = 0f;

    private void Update()
    {
        DetectItem();

        if (interactCircleGroup != null)
        {
            interactCircleGroup.alpha = Mathf.Lerp(
                interactCircleGroup.alpha,
                targetAlpha,
                Time.deltaTime * fadeSpeed
            );
        }

        if (currentTarget != null && Input.GetKeyDown(KeyCode.E))
        {
            PickUp(currentTarget);
        }
    }

    private void DetectItem()
    {
        Ray ray = new Ray(castOrigin.position, castOrigin.forward);

        if (Physics.SphereCast(ray, castRadius, out RaycastHit hit, castDistance, pickupLayer))
        {
            ItemWorld itemWorld = hit.collider.GetComponentInParent<ItemWorld>();

            if (itemWorld != null)
            {
                currentTarget = itemWorld;

                // Closer = more opaque. hit.distance is 0 at point-blank, castDistance at max range.
                float proximity = 1f - Mathf.Clamp01(hit.distance / castDistance);
                targetAlpha = proximity;
                return;
            }
        }

        currentTarget = null;
        targetAlpha = 0f;
    }

    private void PickUp(ItemWorld itemWorld)
    {
        Item item = itemWorld.GetItem();

        bool added = player.GetInventory().AddItem(item);

        if (added)
        {
            itemWorld.DestroySelf();
            currentTarget = null;
            targetAlpha = 0f;
        }
        // if inventory is full, added == false, item stays in world untouched
    }
}