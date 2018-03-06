using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    namespace GameControll
{

    public class ActionManager : MonoBehaviour
    {
     



        public ActionInput GetAction(StateManager st)
        {
    
            if (st.rb)
                return ActionInput.rb;
            if (st.rt)
                return ActionInput.rt;
            if (st.lb)
                return ActionInput.lb;
            if (st.lt)
                return ActionInput.lt;

            return ActionInput.rb;
        }
    }

    public enum ActionInput
    {
        rb,lb,rt,lt
    }

    [System.Serializable]
    public class Action
    {
        public ActionInput input;
        public string targetAnimation;
    }
}
