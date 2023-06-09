using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Hsinpa.SSE
{

    public class CollisionComponent : MonoBehaviour
    {
        public SpriteSyntaxStatic.CollisionType type;

        public SpriteSyntaxStatic.LineCollision lineCollision = new SpriteSyntaxStatic.LineCollision();
        public SpriteSyntaxStatic.RectCollision rectCollision = new SpriteSyntaxStatic.RectCollision();
        public SpriteSyntaxStatic.OvalCollision ovalCollision = new SpriteSyntaxStatic.OvalCollision();
        public SpriteSyntaxStatic.SphereCollision sphereCollision = new SpriteSyntaxStatic.SphereCollision();
        public SpriteSyntaxStatic.PolyLineCollision polylineCollision = new SpriteSyntaxStatic.PolyLineCollision();
        public SpriteSyntaxStatic.PolyLineEditorStruct polylineEditorStruct= new SpriteSyntaxStatic.PolyLineEditorStruct();

        private void Start()
        {
            var c = GetCollisionStruct();

        }

        public SpriteSyntaxStatic.CollisionStruct GetCollisionStruct() {
            SpriteSyntaxStatic.CollisionStruct collisionStruct = new SpriteSyntaxStatic.CollisionStruct();
            collisionStruct.collisionType = type;
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

                case SpriteSyntaxStatic.CollisionType.Polyline:
                    return callback(polylineCollision as T);

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

                case SpriteSyntaxStatic.CollisionType.Polyline:
                    DrawPolylineGizmos(polylineCollision);
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
            Gizmos.color = Color.red;
            var t_position = this.transform.position;
            var m = Matrix4x4.Rotate(this.transform.rotation);

            //First Sphere
            Vector3 sphere_a = new Vector3(ovalCollision.sphere_a.x, ovalCollision.sphere_a.y, 0);
            Vector3 sphere_a_pos = m.MultiplyPoint3x4(sphere_a) + t_position;
            Gizmos.DrawWireSphere(sphere_a_pos, ovalCollision.sphere_a.radius);

            //Second Sphere
            Vector3 sphere_b = new Vector3(ovalCollision.sphere_b.x, ovalCollision.sphere_b.y, 0);
            Vector3 sphere_b_pos = m.MultiplyPoint3x4(sphere_b) + t_position;
            Gizmos.DrawWireSphere(sphere_b_pos, ovalCollision.sphere_b.radius);


            Vector3 direction = (sphere_a_pos - sphere_b_pos).normalized;
            direction.Set(direction.y, -direction.x, 0);

            //Top Line
            Vector3 line_point_a = m.MultiplyPoint3x4(sphere_a + (direction * ovalCollision.sphere_a.radius)) + t_position;
            Vector3 line_point_b = m.MultiplyPoint3x4(sphere_a + (-direction * ovalCollision.sphere_a.radius)) + t_position;
            Vector3 line_point_c = m.MultiplyPoint3x4(sphere_b + (direction * ovalCollision.sphere_b.radius)) + t_position;
            Vector3 line_point_d = m.MultiplyPoint3x4(sphere_b + (-direction * ovalCollision.sphere_b.radius)) + t_position;

            Gizmos.color = Color.green;
            Gizmos.DrawLine(line_point_a, line_point_b);
            Gizmos.DrawLine(line_point_a, line_point_c);
            Gizmos.DrawLine(line_point_c, line_point_d);
            Gizmos.DrawLine(line_point_b, line_point_d);
        }

        private void DrawSphereGizmos(SpriteSyntaxStatic.SphereCollision sphereCollision) {
            Gizmos.color = Color.red;
            var m = Matrix4x4.Rotate(this.transform.rotation);

            Vector3 sphere_a = new Vector3(sphereCollision.x, sphereCollision.y, 0);
            Vector3 sphere_a_pos = m.MultiplyPoint3x4(sphere_a) + this.transform.position;
            Gizmos.DrawWireSphere(sphere_a_pos, sphereCollision.radius);
        }

        private void DrawPolylineGizmos(SpriteSyntaxStatic.PolyLineCollision polylineCollision) {
            if (polylineCollision.points == null) return;


            int lens = polylineCollision.points.Count;
            Vector3 world_position = this.transform.position;
            Vector3 dynamic_position = new Vector3();
            Vector3 dynamic_2_position = new Vector3();
            var m = Matrix4x4.Rotate(this.transform.rotation);

            Vector3 mouse_position = Event.current.mousePosition;
            Camera camera = Camera.main;

            for (int i = 0; i < lens; i++) {
                dynamic_position.Set(polylineCollision.points[i].x, polylineCollision.points[i].y, 0);

                dynamic_position = m.MultiplyPoint3x4(dynamic_position) + world_position;

                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(dynamic_position, 0.1f);
            }
        }
    }
}