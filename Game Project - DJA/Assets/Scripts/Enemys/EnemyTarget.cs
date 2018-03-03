using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameControll
{
    public class EnemyTarget : MonoBehaviour
    {
        public int index;
        public List<Transform> targets = new List<Transform>();
        public List<HumanBodyBones> humanoidBones = new List<HumanBodyBones>();
        Animator anim;

        void Start()
        {
            anim = GetComponent<Animator>();
            if (anim.isHuman == false)
                return;
            for (int i = 0; i < humanoidBones.Count; i++)
            {
                targets.Add(anim.GetBoneTransform(humanoidBones[i]));
            }

        }


        public Transform GetTarget()
        {
            int targetIndex = index;

            if (index < targets.Count - 1)
            {
                index++;
            }
            else
            {
                index = 0;
                targetIndex = 0;
            }

                return targets[targetIndex];//return target from above33
        }
          
    }
}

