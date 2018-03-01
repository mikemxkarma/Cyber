using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameControll
{
    public class AnimatorHook : MonoBehaviour
    {
         Animator anim;
        StateManager states;

        public void Init(StateManager st)
        {
            states  = st;
            anim = st.anim;
        }
           void OnAnimatorMove()
        {
         if(states ==null)
                return;
            if (states.canMove)
                return;

            states.rigidBody.drag = 0;
            float multiplier = 1;

            Vector3 delta = anim.deltaPosition;
            delta.y = 0;
            Vector3 v = (delta*multiplier)/ states.delta;
            states.rigidBody.velocity = v;
        }
    }
}