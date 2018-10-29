using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraResize : MonoBehaviour {

    [SerializeField] private Transform entityOne; //reference to the first entity to follow
    [SerializeField] private Transform entityTwo; //reference to the second entity to follow

    [SerializeField] [Range(0, 0.5f)] private float padding; //distance from the edge of viewport that causes camera movement when crossed
    [SerializeField] private float buffer; //helf the width of the zone in which the camera is at rest
    [SerializeField] private float minDistanceFromEntity; //minimum distance away from the entities

    [SerializeField] private float panSpeed; //rate at which the target position in cameras XY plane changes 
    [SerializeField] private float zoomSpeed; //rate at which the target distance changes to fit both entities

    private Vector2 entityOneViewportPos; //position of the first entity in the viewport
    private Vector2 entityTwoViewportPos; //position of the second entity in the viewport

    private Vector3 velocity;

    private Camera cam; //reference to the camera component

    // Use this for initialization
    void Start () {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        velocity = Vector3.zero;
        entityOneViewportPos = cam.WorldToViewportPoint(entityOne.position);
        entityTwoViewportPos = cam.WorldToViewportPoint(entityTwo.position);

        PanToFit((entityOneViewportPos + entityTwoViewportPos) / 2);
        ZoomToFit(entityOneViewportPos, entityTwoViewportPos);

        transform.position += velocity;

    }

    /// <summary>
    /// Pan to a location
    /// </summary>
    /// <param name="viewPortPosition">location to pan to</param>
    private void PanToFit(Vector2 centerPosition)
    {
        Vector2 offset = centerPosition - (Vector2.one / 2);
        //pan to center point
        if (offset.magnitude > buffer)
        {
            velocity += (transform.up * offset.y * panSpeed * Time.deltaTime)
                             + (transform.right * offset.x * panSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// zoom out to fit two locations on screen
    /// </summary>
    /// <param name="viewportPositionOne">location one</param>
    /// <param name="viewportPositionTwo">location two</param>
    private void ZoomToFit(Vector3 viewportPositionOne, Vector3 viewportPositionTwo)
    {
        //zoom out to fit entities
        if (ClosestDistance() + buffer < minDistanceFromEntity ||
            (viewportPositionOne.x < padding - buffer || viewportPositionOne.x > 1 - padding + buffer) ||
            (viewportPositionOne.y < padding - buffer || viewportPositionOne.y > 1 - padding + buffer) ||
            (viewportPositionTwo.x < padding - buffer || viewportPositionTwo.x > 1 - padding + buffer) ||
            (viewportPositionTwo.y < padding - buffer || viewportPositionTwo.y > 1 - padding + buffer))
        {
            velocity -= transform.forward * zoomSpeed * Time.deltaTime; //increase the target distance from entity center
        }
        //zoom in towards entities
        else if (ClosestDistance() - buffer > minDistanceFromEntity && 
            (viewportPositionOne.x > padding + buffer && viewportPositionOne.x < 1 - padding - buffer) &&
            (viewportPositionOne.y > padding + buffer && viewportPositionOne.y < 1 - padding - buffer) &&
            (viewportPositionTwo.x > padding + buffer && viewportPositionTwo.x < 1 - padding - buffer) &&
            (viewportPositionTwo.y > padding + buffer && viewportPositionTwo.y < 1 - padding - buffer))
        {
            velocity += transform.forward * zoomSpeed * Time.deltaTime; //decrease the target distance from entity center
        }
    }

    /// <summary>
    /// find the closest distance to an entity
    /// </summary>
    /// <returns></returns>
    private float ClosestDistance()
    {
        float distanceOne = (entityOne.position - transform.position).magnitude;
        float distanceTwo = (entityTwo.position - transform.position).magnitude;
        return distanceOne < distanceTwo ? distanceOne : distanceTwo;
    }
}
