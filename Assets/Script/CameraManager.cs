using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    InputManager inputManager;


    public Transform targetTransform; //the object camera will follow
    public Transform cameraPivot;     //the object the camera uses to pivot (look up and down)
    public Transform cameraTransform; //the transform of the actual camera object in the scene
    public LayerMask collisionLayers; //the layers we want to collide with
    private float defaultPosition;
    private Vector3 cameraFollowVelocity = Vector3.zero;
    private Vector3 cameraVectorPosition;

    public float cameraCollisionOffset = 0.2f; //How much the camera will jump off of objects its colliding with
    public float minCollisionOffset = 0.2f;
    public float cameraCollisionRadius = 0.2f;
    public float cameraFollowSpeed = 0.2f;
    public float cameraLookSpeed = 2;
    public float cameraPivotSpeed = 2;

    public float lookAngle; //Camera look up and down
    public float pivotAngle; // Camera look left and right
    public float minPivotAngle = -35;
    public float maxPivotAngle = 35;
    public float maxLookAngle = 70;
    public float minLookAngle = -70;

    private void Awake()
    {   
        targetTransform = GameObject.Find("Player").transform;
        inputManager = FindObjectOfType<InputManager>();   
        cameraTransform = Camera.main.transform;
        defaultPosition = cameraTransform.localPosition.z;
    }

    public void HandleAllCameraMovement()
    {
        FollowTarget();
        RotateCamera();
        HandleCameraCollisions();
    }

    private void FollowTarget()
    {
        Vector3 targetPosition = Vector3.SmoothDamp(transform.position, targetTransform.position, ref cameraFollowVelocity, cameraFollowSpeed);
        transform.position = targetPosition;
    }

    private void RotateCamera()
    {
        Vector3 rotation;

        lookAngle = lookAngle + (inputManager.cameraInputX * cameraLookSpeed);
        pivotAngle = pivotAngle - (inputManager.cameraInputY * cameraPivotSpeed);
        
        lookAngle = Mathf.Clamp(lookAngle, minLookAngle, maxLookAngle);
        pivotAngle = Mathf.Clamp(pivotAngle, minPivotAngle, maxPivotAngle);

        rotation = Vector3.zero;
        rotation.y = lookAngle;
        Quaternion targetRotation = Quaternion.Euler(rotation);
        transform.rotation = targetRotation;

        rotation = Vector3.zero;
        rotation.x = pivotAngle;
        targetRotation = Quaternion.Euler(rotation);
        cameraPivot.localRotation = targetRotation;
    }

    private void HandleCameraCollisions()
    {
        float targetPostion = defaultPosition;
        RaycastHit hit;
        Vector3 direction = cameraTransform.position - cameraPivot.position;
        direction.Normalize();

        if (Physics.SphereCast(cameraPivot.transform.position, cameraCollisionRadius, direction, out hit, Mathf.Abs(targetPostion), collisionLayers))
        {
            float distance = Vector3.Distance(cameraPivot.position, hit.point);
            targetPostion =- (distance - cameraCollisionOffset);
        }

        if (Mathf.Abs(targetPostion) < minCollisionOffset)
        {
            targetPostion = targetPostion - minCollisionOffset;
        }

        cameraVectorPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPostion, 0.2f);
        cameraTransform.localPosition = cameraVectorPosition;
    }
}
