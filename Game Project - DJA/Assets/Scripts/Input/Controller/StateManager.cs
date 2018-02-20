using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA { 

public class StateManager : MonoBehaviour
    {
        public float horizontal;
        public float vertical;
        [HideInInspector]
        public Animator anim;
        [HideInInspector]
        public Rigidbody rigidBody;
        public GameObject activeModel;
        [HideInInspector]
        public float delta;

        public void Init()
        {
            SetupAnimator();
            rigidBody = GetComponent<Rigidbody>();
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

        public void Tick(float d)
        {
            delta = d;
        }
    }
}
