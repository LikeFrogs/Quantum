using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform objectOne;
    [SerializeField] private Transform objectTwo;

    private const float minDistanceToObject = 8.0f;

    private void LateUpdate()
    {
        transform.position = GetTargetCameraPosition();
    }

    /// <summary>
    /// Decides what calculation needs to be made to determine position of the camera
    /// </summary>
    private Vector3 GetTargetCameraPosition()
    {
        //one object is dead and the camera should view the the other one
        if (!objectOne.gameObject.activeSelf)
        {
            return objectTwo.position - transform.forward * minDistanceToObject;
        }
        if (!objectTwo.gameObject.activeSelf)
        {
            return objectOne.position - transform.forward * minDistanceToObject;
        }

        //both Objects are alive and optimal posion to fit them both on screen is calculated
        return GetOptimalCameraPosition();
    }


    /// <summary>
    /// Calculates the perfect position for the camera to be in so that object one and object two are just barely inside the field of view.
    /// The field of view is treated like a cone so on a rectangular camera view there will be some distance between the objects and the
    /// left and right sides of the camera viewport.
    /// </summary>
    /// <returns>The world space position of the optimal position for the camera</returns>
    private Vector3 GetOptimalCameraPosition()
    {
        // Get the first plane which is the plane
        // made up of the two objects and the forward vector of the camera
        Vector3 vectorBetweenObjects = objectTwo.position - objectOne.position;
        if (vectorBetweenObjects == Vector3.zero)
        {
            vectorBetweenObjects = Vector3.right;
        }
        Vector3 plane1Normal = Vector3.Cross(transform.forward, vectorBetweenObjects);
        // If the cross product returned the zero vector (becasue the objects line up with the forward of the camera)
        // then just make the plane have the up vector as its normal as any plane will do
        if (plane1Normal == Vector3.zero)
        {
            plane1Normal = Vector3.up;
        }
        // The w value of the first plane is just the normal vector times any point on that plane
        float plane1W = Vector3.Dot(plane1Normal, objectOne.position);

        // The next two planes are the planes formed by the normal of the first plane and
        // the vector on the first plane that points in the opposite direction of the camera forward
        // and is rotated by the cameras field of view towards the other object on the plane

        // 2nd Plane
        Vector3 fieldOfViewVectorOfObjectOne = Vector3.RotateTowards(transform.forward * -1, vectorBetweenObjects, Mathf.Deg2Rad * Camera.main.fieldOfView * 0.4f, 0.0f);
        Vector3 plane2Normal = Vector3.Cross(plane1Normal, fieldOfViewVectorOfObjectOne);
        float plane2W = Vector3.Dot(plane2Normal, objectOne.position);

        // Last plane
        Vector3 fieldOfViewVectorOfObjectTwo = Vector3.RotateTowards(transform.forward * -1, -1 * vectorBetweenObjects, Mathf.Deg2Rad * Camera.main.fieldOfView * 0.4f, 0.0f);
        Vector3 plane3Normal = Vector3.Cross(plane1Normal, fieldOfViewVectorOfObjectTwo);
        float plane3W = Vector3.Dot(plane3Normal, objectTwo.position);

        // REMARK: If the two objects line up with the camera forward with less than the field of view as the angle between them then
        // the intersection of the planes will be the position of the object farthest in the opposite direction of the camera forward.
        // This means that both objects will be in the field of view, but one could be inside the camera.
        // Technically speaking this is the desired result as that is the most optimal position for the camera to be in while still having
        // both objects in its field of view even if its clipping inside one of them.

        // Now just solve the system of equations of the form
        // plane1Normal . X = plane1W
        // plane2Normal . X = plane2W
        // plane3Normal . X = plane3W

        // Plug it all into a handy dandy CAS and this is what we get
        // Tons of math! Shield your eyes.
        // First lets just make some shorter variable names even if they aren't the most descriptive
        float a = plane1Normal.x;
        float b = plane1Normal.y;
        float c = plane1Normal.z;

        float d = plane2Normal.x;
        float e = plane2Normal.y;
        float f = plane2Normal.z;

        float g = plane3Normal.x;
        float h = plane3Normal.y;
        float i = plane3Normal.z;

        float q = plane1W;
        float r = plane2W;
        float s = plane3W;

        Vector3 optimalCameraPosition = Vector3.zero;

        optimalCameraPosition.x = ((f * h - e * i) * q - (c * h - b * i) * r + (c * e - b * f) * s) / ((c * e - b * f) * g - (c * d - a * f) * h + (b * d - a * e) * i);
        optimalCameraPosition.y = -1 * ((f * g - d * i) * q - (c * g - a * i) * r + (c * d - a * f) * s) / ((c * e - b * f) * g - (c * d - a * f) * h + (b * d - a * e) * i);
        optimalCameraPosition.z = ((e * g - d * h) * q - (b * g - a * h) * r + (b * d - a * e) * s) / ((c * e - b * f) * g - (c * d - a * f) * h + (b * d - a * e) * i);

        AddOffset(ref optimalCameraPosition);

        return optimalCameraPosition;
    }

    /// <summary>
    /// adds an offest to the camera's position if it's too close to an object
    /// </summary>
    /// <param name="position">calculated position of the camera</param>
    private void AddOffset(ref Vector3 position)
    {
        //find the distances between position and objects 
        float d1 = (position - objectOne.position).magnitude;
        float d2 = (position - objectTwo.position).magnitude;

        //move the camera backward localy if smallest distance is less than minimum distance
        position += transform.forward * -Mathf.Clamp(minDistanceToObject - (d1 < d2 ? d1 : d2), 0, minDistanceToObject);
    }
}
