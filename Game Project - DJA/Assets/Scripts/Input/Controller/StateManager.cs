using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA { 

public class StateManager : MonoBehaviour
    {
        [Header("Initialize")]
        public GameObject activeModel;

        [Header("Inputs")]
        public Vector3 moveDirection;    
        public float moveAmount;
        public float horizontal;
        public float vertical;

        [Header("Stats")]
        public float moveSpeed = 2;
        public float runSpeed = 3.5f;
        public float rotateSpeed = 5;
        public float toGround = 0.5f;
        [Header("States")]
        public bool run;
        public bool onGround;
        public bool lockon;
        [HideInInspector]
        public Animator anim;
        [HideInInspector]
        public Rigidbody rigidBody;
        


        [HideInInspector]   
        public float delta;
        [HideInInspector]
        public LayerMask ignoreLayers;

        public void Init()
        {
            SetupAnimator();
            rigidBody = GetComponent<Rigidbody>();
            rigidBody.angularDrag = 999;
            rigidBody.drag = 4;
            rigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

            gameObject.layer = 8;
            ignoreLayers = ~(1 << 9);

            anim.SetBool("onGround", true);
        }
        void SetupAnimator()
        {
            if (activeModel == null)
            {
                anim = GetComponentInChildren<Animator>();
                if (anim == null)
                {

                }
                else
                {
                    activeModel = anim.gameObject;
                }
            }
            if (anim == null)
                anim = activeModel.GetComponent<Animator>();
            anim.applyRootMotion = false;

        }

        public void FixedTick(float d)
        {
            delta = d;

            rigidBody.drag = (moveAmount > 0|| onGround==false) ? 0 : 4;

           
          //  if (moveAmount > 0)
          //  {
          //      rigidBody.drag = 0;
          //  }
          //  else
          //      rigidBody.drag = 4;

            float targetSpeed = moveSpeed;
            if (run)
                targetSpeed = runSpeed;

            if(onGround)
            rigidBody.velocity = moveDirection*(targetSpeed*moveAmount);

            if (run)
                lockon = false;

            if (!lockon)
            {
            Vector3 targetDirection = moveDirection;
            targetDirection.y = 0;
            if (targetDirection == Vector3.zero)
                targetDirection = transform.forward;
            Quaternion tRotation = Quaternion.LookRotation(targetDirection);
            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tRotation,delta*moveAmount*rotateSpeed);
            transform.rotation = targetRotation;

            }
            HandleMovementAnimations();
        }

        public void Tick(float d)
        {
            delta = d;
            onGround = OnGround();

            anim.SetBool("onGround", onGround);
        }

        void HandleMovementAnimations()
        {
            anim.SetBool("run", run);
            anim.SetFloat("vertical", moveAmount,0.4f,delta);
        }

        public bool OnGround()
        {
            bool r = false;
            Vector3 origin = transform.position + (Vector3.up * toGround);
            Vector3 direction = -Vector3.up;
            float distance = toGround + 0.3f;
            RaycastHit hit;
            //Debug.DrawRay(origin, direction * distance);
            if (Physics.Raycast(origin, direction, out hit, distance, ignoreLayers))
            {
                r = true;
                Vector3 targetPosition = hit.point;
                transform.position = targetPosition;
            }
            return r;
        }
    }
}
