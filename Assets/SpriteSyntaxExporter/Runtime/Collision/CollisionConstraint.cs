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

            SpriteSyntaxStatic.ConstraintStruct copy_constraint = constraint;
            //Convert to radian
            if (copy_constraint.min_rotation != 0) copy_constraint.min_rotation = copy_constraint.min_rotation * Mathf.Deg2Rad;
            if (copy_constraint.max_rotation != 0) copy_constraint.max_rotation = copy_constraint.max_rotation * Mathf.Deg2Rad;

            copy_constraint.rest_point = -copy_constraint.rest_point * Mathf.Deg2Rad;

            return copy_constraint;
        }
    
    }
}