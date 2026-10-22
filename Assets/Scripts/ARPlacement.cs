
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
        if (PlacedObject != null || Input.touchCount == 0)
            return;

        Touch touch = Input.GetTouch(0);

        if (touch.phase != TouchPhase.Began)
            return;

        if (EventSystem.current != null &&
            EventSystem.current.IsPointerOverGameObject(
                touch.fingerId))
            return;

        if (raycastManager.Raycast(
            touch.position,
            hits,
            TrackableType.PlaneWithinPolygon))
        {
            Pose pose = hits[0].pose;

            PlacedObject = Instantiate(
                modelPrefab,
                pose.position,
                pose.rotation
            );
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
