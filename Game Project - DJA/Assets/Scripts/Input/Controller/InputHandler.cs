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

        bool runInput;
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
            runInput = Input.GetButton("RunInput");
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

            if (runInput)
                states.run = (states.moveAmount > 0);
            else
                states.run = false;
        }
        #endregion
    }
}