using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///=================================================================
///   Namespace:      GameControll
///   Class:          InputHandler
///   Description:    Handles the Input of Keyboard and Controller.
///   Date: 20-02-2018
///   Notes:
///   Revision History:   
///=================================================================

namespace GameControll
{
    public class InputHandler : MonoBehaviour
    {
        #region Fields
        float vertical;
        float horizontal;
        float delta;
        StateManager states;
        CameraManager cameraManager;

        bool b_input;
        bool a_input;
        bool x_input;
        bool y_input;

        bool rb_input;
        bool lb_input;
        bool rt_input;
        bool lt_input;
        float rt_axis;
        float lt_axis;


        #endregion

        #region Constructors
        void Start()
        {
            states = GetComponent<StateManager>();
            states.Init();
            cameraManager = CameraManager.singleton;
            cameraManager.Init(this.transform);
        } 
        
        void FixedUpdate()
       {
            delta = Time.fixedDeltaTime;
            GetInput();
            UpdateStates();
            states.FixedTick(delta);
            cameraManager.Tick(delta);
        }
       
        void Update()
        {
            delta = Time.deltaTime;
            states.Tick(delta);        
        }
        #endregion

        #region Methods
        void GetInput()
        {
            vertical = Input.GetAxis("Vertical");
            horizontal = Input.GetAxis("Horizontal");
            b_input = Input.GetButton("B");
            a_input = Input.GetButton("A");
            y_input = Input.GetButtonUp("Y");
            x_input = Input.GetButton("X");
            rt_input = Input.GetButton("RT");
            rt_axis = Input.GetAxis("RT");

            if (rt_axis != 0)
                rt_input = true;

            lt_input = Input.GetButton("LT");
            lt_axis = Input.GetAxis("LT");

            if (lt_axis != 0)
                lt_input = true;

            rb_input= Input.GetButton("RB");
            lb_input = Input.GetButton("LB");

        }

        void UpdateStates()
        {
            states.horizontal = horizontal;
            states.vertical = vertical;
            //move on base camara angle
            Vector3 v = vertical * cameraManager.transform.forward;//orientation of were camara is looking
            Vector3 h = horizontal * cameraManager.transform.right;//were we want the camara to be
            states.moveDirection = (v + h).normalized;
            float m = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
            states.moveAmount = Mathf.Clamp01(m);//tel if it as movement

            if (b_input)
            {
                states.run = (states.moveAmount > 0);
            }
            else
            {
                states.run = false;
            }
            states.rt = rt_input;
            states.lt = lt_input;
            states.rb = rb_input;
            states.lb = lb_input;

            if (y_input)
            {
                states.isTwoHanded = !states.isTwoHanded;
                states.HandlerTwoHanded();
            }
        }
        #endregion
    }
}