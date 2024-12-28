using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.InputSystem;

[RequireComponent(typeof(ARRaycastManager))]
public class PlaceOnIndicator : MonoBehaviour
{
    [SerializeField]
    private GameObject placementIndicator;
    [SerializeField]
    private GameObject placedPrefab;

    private GameObject spawnedObject;

    [SerializeField]
    private InputAction touchInput;

    private ARRaycastManager arRaycastManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private void Awake()
    {
        arRaycastManager = GetComponent<ARRaycastManager>();

        // InputActionÇÃÉCÉxÉìÉgìoò^
        if (touchInput != null)
        {
            touchInput.performed += context => PlaceObject();
        }
        else
        {
            Debug.LogError("Touch Input is not assigned!");
        }

        if (placementIndicator != null)
        {
            placementIndicator.SetActive(false);
        }
        else
        {
            Debug.LogError("Placement Indicator is not assigned!");
        }
    }

    private void OnEnable()
    {
        if (touchInput != null)
        {
            touchInput.Enable();
        }
    }

    private void OnDisable()
    {
        if (touchInput != null)
        {
            touchInput.Disable();
        }
    }

    private void Update()
    {
        if (arRaycastManager.Raycast(new Vector2(Screen.width / 2, Screen.height / 2), hits, TrackableType.PlaneWithinPolygon))
        {
            var hitPose = hits[0].pose;

            // IndicatorÇÃà íuÇ∆âÒì]ÇçXêV
            placementIndicator.transform.SetPositionAndRotation(hitPose.position, hitPose.rotation);

            if (!placementIndicator.activeInHierarchy)
            {
                placementIndicator.SetActive(true);
            }
        }
        else
        {
            if (placementIndicator.activeInHierarchy)
            {
                placementIndicator.SetActive(false);
            }
        }
    }

    private void PlaceObject()
    {
        if (placementIndicator == null || !placementIndicator.activeInHierarchy)
        {
            Debug.LogWarning("Placement Indicator is inactive or not assigned.");
            return;
        }

        if (spawnedObject == null)
        {
            spawnedObject = Instantiate(placedPrefab, placementIndicator.transform.position, placementIndicator.transform.rotation);
        }
        else
        {
            spawnedObject.transform.SetPositionAndRotation(placementIndicator.transform.position, placementIndicator.transform.rotation);
        }
    }
}
