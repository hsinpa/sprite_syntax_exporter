using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Hsinpa.SSE
{

    public class CollisionComponent : MonoBehaviour
    {
        public SpriteSyntaxStatic.CollisionType type;

        public SpriteSyntaxStatic.LineCollision lineCollision = new SpriteSyntaxStatic.LineCollision();
        public SpriteSyntaxStatic.RectCollision rectCollision = new SpriteSyntaxStatic.RectCollision();
        public SpriteSyntaxStatic.OvalCollision ovalCollision = new SpriteSyntaxStatic.OvalCollision();
        public SpriteSyntaxStatic.SphereCollision sphereCollision = new SpriteSyntaxStatic.SphereCollision();

        private void Start()
        {
            var c = GetCollisionStruct();
            Debug.Log(c.data);
        }

        public SpriteSyntaxStatic.CollisionStruct GetCollisionStruct() {
            SpriteSyntaxStatic.CollisionStruct collisionStruct = new SpriteSyntaxStatic.CollisionStruct();
            collisionStruct.collisionType = type;

            collisionStruct.rotation = gameObject.transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
            collisionStruct.data = CollisionTypeOpt<object, string>(type, (c) => JsonUtility.ToJson(c));

            return collisionStruct;
        }

        private K CollisionTypeOpt<T, K>(SpriteSyntaxStatic.CollisionType p_type, System.Func<T, K> callback) where T : class { 
            
            switch(p_type)
            {
                case SpriteSyntaxStatic.CollisionType.Line:
                    return callback(lineCollision as T);

                case SpriteSyntaxStatic.CollisionType.Rect:
                    return callback(rectCollision as T);

                case SpriteSyntaxStatic.CollisionType.Oval:
                    return callback(ovalCollision as T);

                default:
                    return callback(sphereCollision as T);
            }
        }


    }
}