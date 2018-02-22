using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///=================================================================
///   Namespace:      GameControll
///   Class:          StateManager
///   Description:    Handles the Input of Keyboard and Controller.
///   Date: 20-02-2018
///   Notes:
///   Revision History:   
///=================================================================

namespace GameControll
{

    public class StateManager : MonoBehaviour
    {
        #region Fields    
        [Header("Initialize")]
        public GameObject activeModel;
        [Header("Inputs")]
        public Vector3 moveDirection;
        public float moveAmount;
        public float horizontal;
        public float vertical;
        public bool rt, rb, lt, lb;
        [Header("Stats")]
        public float moveSpeed = 2;
        public float runSpeed = 3.5f;
        public float rotateSpeed = 5;
        public float toGround = 0.5f;
        [Header("States")]
        public bool run;
        public bool onGround;
        public bool lockon;
        public bool inAction;
        public bool canMove;


        [HideInInspector]
        public Animator anim;
        [HideInInspector]
        public Rigidbody rigidBody;
        [HideInInspector]
        public float delta;
        [HideInInspector]
        public LayerMask ignoreLayers;
        #endregion

        #region Methods
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
          

  

            DetectAction();

            if (inAction)
            {
                inAction = !anim.GetBool("canMove");
                    return;
            }
            canMove = anim.GetBool("canMove");

            rigidBody.drag = (moveAmount > 0|| onGround==false) ? 0 : 4;          

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

        public void DetectAction()
        {
            if (canMove == false)
                return;

            if (rb == false && rt == false && lt == false && lb == false)
                return;

            string targetAnim= null;

            if (rb)
                targetAnim = "oh_attack_1";
            if (rt)
                targetAnim = "oh_attack_1";
            if (lt)
                targetAnim = "oh_attack_1";
            if (lb)
                targetAnim = "oh_attack_1";

            if (string.IsNullOrEmpty(targetAnim))
                return;

            canMove = false;
            inAction = true;
            anim.Play(targetAnim);
            Debug.print("anim");
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
        #endregion
    }   
}
