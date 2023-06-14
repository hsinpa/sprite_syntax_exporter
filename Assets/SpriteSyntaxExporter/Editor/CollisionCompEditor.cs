using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using static Hsinpa.SSE.SpriteSyntaxStatic;
using System.Reflection.Emit;
using NUnit.Framework;

namespace Hsinpa.SSE
{
    [CustomEditor(typeof(CollisionComponent))]
    public class CollisionCompEditor : Editor
    {
        CollisionComponent m_spriteLayoutComponent;

        SerializedProperty m_line_collision_property;
        SerializedProperty m_rect_collision_property;
        SerializedProperty m_oval_collision_property;
        SerializedProperty m_sphere_collision_property;
        SerializedProperty m_collision_type;

        private bool lockInspectorFlag = false;
        private bool _mouseClickFlag = false;

        void OnEnable()
        {
            m_spriteLayoutComponent = (CollisionComponent)target;
            m_collision_type = serializedObject.FindProperty("type");

            m_line_collision_property = serializedObject.FindProperty("lineCollision");
            m_rect_collision_property = serializedObject.FindProperty("rectCollision");
            m_oval_collision_property = serializedObject.FindProperty("ovalCollision");
            m_sphere_collision_property = serializedObject.FindProperty("sphereCollision");

            SceneView.duringSceneGui += OnSceneGUI;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(m_collision_type);

            switch (m_spriteLayoutComponent.type) {
                case SpriteSyntaxStatic.CollisionType.Line:
                    EditorGUILayout.PropertyField(m_line_collision_property);
                    break;
                case SpriteSyntaxStatic.CollisionType.Rect:
                    EditorGUILayout.PropertyField(m_rect_collision_property);
                    break;
                case SpriteSyntaxStatic.CollisionType.Oval:
                    EditorGUILayout.PropertyField(m_oval_collision_property);
                    break;
                case SpriteSyntaxStatic.CollisionType.Sphere:
                    EditorGUILayout.PropertyField(m_sphere_collision_property);
                    break;
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void OnSceneGUI(SceneView sceneView) {
            SceneView.RepaintAll();

            Event guiEvent = Event.current;
            if (lockInspectorFlag) {
                HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
            }

            if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0)
                _mouseClickFlag = true;

            if (guiEvent.type == EventType.MouseUp && guiEvent.button == 0)
                _mouseClickFlag = false;

            switch (m_spriteLayoutComponent.type) {
                case SpriteSyntaxStatic.CollisionType.Polyline:
                    HandlePolyline();
                break;
            }
        }

        private void HandlePolyline() { 
            if (m_spriteLayoutComponent.polylineCollision.points == null || m_spriteLayoutComponent.polylineCollision.points.Count <= 0) {
                m_spriteLayoutComponent.polylineCollision.points = new List<Vector2>() { new Vector2(0, 0), new Vector2(0, 1) };
            }
            
            Vector3 world_position = m_spriteLayoutComponent.transform.position;

            m_spriteLayoutComponent.polylineEditorStruct.ctrl_point_index = -1;

            int lens = m_spriteLayoutComponent.polylineCollision.points.Count;

            var m = Matrix4x4.Rotate(m_spriteLayoutComponent.transform.rotation);

            Ray mouse_ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

            for (int i = 0; i < lens; i++) {
               Vector2 dynamic_position = m.MultiplyPoint3x4(m_spriteLayoutComponent.polylineCollision.points[i]) + world_position;
                float dist = Vector2.Distance(dynamic_position, mouse_ray.origin);

                if (dist <= 0.1f && _mouseClickFlag) {
                    //Debug.Log($"Mouse Index Index i {i}");

                    break;
                }

            }
        }

        private void OnDisable() {
            SceneView.duringSceneGui -= OnSceneGUI;
        }

    }
}
