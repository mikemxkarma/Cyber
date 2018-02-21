using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///=================================================================
///   Namespace:      GameControll
///   Class:          CameraManager
///   Description:    Handles the Input of Keyboard and Controller.
///   Date: 20-02-2018
///   Notes:
///   Revision History:   
///=================================================================
///
namespace GameControll
{

    public class CameraManager : MonoBehaviour
    {
        public float followSpeed = 9;
        public float mouseSpeed = 2;
        public float controllerSpeed = 2;
        [HideInInspector]
        public Transform target;
        [HideInInspector]
        public Transform pivot;

        public Transform camTransform;

        float turnSmoothing = 0.1f;
        public float minAngle = -35;
        public float maxAngle = 35;
        public float lookAngle;
        public float tiltAngle;
        public bool lockOnMode;
        float smoothX;
        float smoothY;
        float smoothXvelocity;
        float smoothYvelocity;

        public void Init(Transform t)
        {
            target = t;
            camTransform = Camera.main.transform;
            pivot = camTransform.parent;
        }

        public void Tick(float d)
        {
            float h = Input.GetAxis("Mouse X");
            float v = Input.GetAxis("Mouse Y");

            float c_h = Input.GetAxis("RightAxis X");
            float c_v = Input.GetAxis("RightAxis Y");

            float targetSpeed = mouseSpeed;

            if (c_h != 0 || c_v != 0)//mouse input over keyboard overlap
            {
                h = c_h;
                v = c_v;
                targetSpeed = controllerSpeed;
            }
            FollowTarget(d);
            HandleRotations(d, v, h, targetSpeed);
        }

        void FollowTarget(float d)
        {
            float speed = d * followSpeed;
            Vector3 targetPosition = Vector3.Lerp(transform.position, target.position, d);
            transform.position = targetPosition;
        }

        void HandleRotations(float d, float v, float h, float targetSpeed)
        {
            if (turnSmoothing > 0)
            {
                smoothX = Mathf.SmoothDamp(smoothX, h, ref smoothXvelocity, turnSmoothing);
                smoothY = Mathf.SmoothDamp(smoothY, v, ref smoothYvelocity, turnSmoothing);
            }
            else
            {
                smoothX = h;
                smoothY = v;
            }
            if (lockOnMode)
            {

            }

            lookAngle += smoothX * targetSpeed;
            transform.rotation = Quaternion.Euler(0, lookAngle, 0);//rotate camara Yaxis

            tiltAngle -= smoothY * targetSpeed;
            tiltAngle = Mathf.Clamp(tiltAngle, minAngle, maxAngle);
            pivot.localRotation = Quaternion.Euler(tiltAngle, 0, 0);
        }


        public static CameraManager singleton;
         void Awake()
        {
            singleton = this;
        }

    }
}