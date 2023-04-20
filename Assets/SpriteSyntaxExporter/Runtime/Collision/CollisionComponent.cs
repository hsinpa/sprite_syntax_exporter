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

        private void OnDrawGizmos() {
            switch (type) {
                case SpriteSyntaxStatic.CollisionType.Line:
                    DrawLineGizmos(lineCollision);
                    break;

                case SpriteSyntaxStatic.CollisionType.Rect:
                    DrawRectGizmos(rectCollision);
                    break;

                case SpriteSyntaxStatic.CollisionType.Oval:
                    DrawOvalGizmos(ovalCollision);
                    break;

                case SpriteSyntaxStatic.CollisionType.Sphere:
                    DrawSphereGizmos(sphereCollision);
                    break;
            }
        }

        private void DrawLineGizmos(SpriteSyntaxStatic.LineCollision lineCollision) {
            var m = Matrix4x4.Rotate(this.transform.rotation);

            Vector2 point_a = transform.position + m.MultiplyPoint3x4(new Vector3(lineCollision.point_a.x, lineCollision.point_a.y, 0));
            Vector2 point_b = transform.position + m.MultiplyPoint3x4( new Vector3(lineCollision.point_b.x, lineCollision.point_b.y, 0));

            Vector2 middle_vector = Vector2.Lerp(point_a, point_b, 0.5f);

            Gizmos.color = Color.green;
            Gizmos.DrawLine(point_a, point_b);

            Vector2 direction = (point_b - point_a).normalized;
            Vector2 perpendicular = Vector2.Perpendicular(direction);
            Vector2 perpendicular_point = middle_vector + (perpendicular * Vector2.Distance(point_a, point_b) * 0.5f);

            Gizmos.color = Color.red;
            Gizmos.DrawLine(middle_vector, perpendicular_point);
        }


        private void DrawRectGizmos(SpriteSyntaxStatic.RectCollision rectCollision) {
            var m = Matrix4x4.Rotate(this.transform.rotation);

            Vector3 point_top_left = new Vector3(rectCollision.x - (rectCollision.width * 0.5f), rectCollision.y + (rectCollision.height * 0.5f));
            Vector3 point_top_right = new Vector3(rectCollision.x + (rectCollision.width * 0.5f), rectCollision.y + (rectCollision.height * 0.5f));
            Vector3 point_bottom_left = new Vector3(rectCollision.x - (rectCollision.width * 0.5f), rectCollision.y - (rectCollision.height * 0.5f));
            Vector3 point_bottom_right = new Vector3(rectCollision.x + (rectCollision.width * 0.5f), rectCollision.y - (rectCollision.height * 0.5f));

            Vector2 point_top_left_r = transform.position + m.MultiplyPoint3x4(point_top_left);
            Vector2 point_top_right_r = transform.position + m.MultiplyPoint3x4(point_top_right);
            Vector2 point_bottom_left_r = transform.position + m.MultiplyPoint3x4(point_bottom_left);
            Vector2 point_bottom_right_r = transform.position + m.MultiplyPoint3x4(point_bottom_right);

            Gizmos.color = Color.green;
            Gizmos.DrawLine(point_top_left_r, point_top_right_r);
            Gizmos.DrawLine(point_top_right_r, point_bottom_right_r);
            Gizmos.DrawLine(point_bottom_right_r, point_bottom_left_r);
            Gizmos.DrawLine(point_bottom_left_r, point_top_left_r);
        }

        private void DrawOvalGizmos(SpriteSyntaxStatic.OvalCollision ovalCollision) {

        }

        private void DrawSphereGizmos(SpriteSyntaxStatic.SphereCollision sphereCollision) {

        }
    }
}