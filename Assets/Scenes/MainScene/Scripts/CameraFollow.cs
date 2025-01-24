using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scenes.MainScene.Scripts
{
    public class CameraFollow : MonoBehaviour
    {
        public Transform target; // The object for the camera to follow (assign in the Inspector)
        public Vector3 offset;   // Offset from the target object
        public float smoothSpeed = 0.1f; // Speed of smoothing for camera movement
    
        private void FixedUpdate()
        {
            // Desired position of the camera
            Vector3 desiredPosition = target.position + offset;

            // Maintain a fixed Z position for 2D (e.g., -10)
            desiredPosition.z = -10; // or any appropriate value for your game

            // Smooth the camera movement between its current and the desired position
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            // Apply the smoothed position to the camera
            transform.position = smoothedPosition;

            // Optional: Make the camera look at the target (not usually needed in 2D)
            // transform.LookAt(target); // Comment this out for 2D games
        }
    }
}
