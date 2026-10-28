
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARPlacement : MonoBehaviour
{
    public ARRaycastManager raycastManager;
    public GameObject modelPrefab;

    public GameObject PlacedObject { get; private set; }

    private static readonly List<ARRaycastHit> hits =
        new List<ARRaycastHit>();

    void Update()
    {
        if (PlacedObject != null ||
            raycastManager == null ||
            modelPrefab == null)
            return;

        Vector2 screenPosition;

#if UNITY_EDITOR

        // Computer mouse click
        if (!Input.GetMouseButtonDown(0))
            return;

        if (EventSystem.current != null &&
            EventSystem.current.IsPointerOverGameObject())
            return;

        screenPosition = Input.mousePosition;

#else

        // Mobile screen touch
        if (Input.touchCount == 0)
            return;

        Touch touch = Input.GetTouch(0);

        if (touch.phase != TouchPhase.Began)
            return;

        if (EventSystem.current != null &&
            EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            return;

        screenPosition = touch.position;

#endif

        if (raycastManager.Raycast(
            screenPosition,
            hits,
            TrackableType.PlaneWithinPolygon))
        {
            Pose pose = hits[0].pose;

            PlacedObject = Instantiate(
                modelPrefab,
                pose.position,
                pose.rotation
            );

            Debug.Log("Stack Model Placed Successfully!");
        }
        else
        {
            Debug.Log("No AR plane detected at click position");
        }
    }

    public void ResetPlacement()
    {
        if (PlacedObject != null)
        {
            Destroy(PlacedObject);
            PlacedObject = null;
        }
    }
}
