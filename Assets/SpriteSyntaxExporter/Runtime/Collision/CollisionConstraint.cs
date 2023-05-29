using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hsinpa.SSE
{
    public class CollisionConstraint : MonoBehaviour
    {
        [SerializeField]
        private SpriteSyntaxStatic.ConstraintStruct constraint;
    
        public SpriteSyntaxStatic.ConstraintStruct GetConstraintStruct() {
            //Convert to radian
            if (constraint.min_rotation != 0) constraint.min_rotation = constraint.min_rotation * Mathf.Deg2Rad;
            if (constraint.max_rotation != 0) constraint.max_rotation = constraint.max_rotation * Mathf.Deg2Rad;

            return constraint;
        }
    
    }
}