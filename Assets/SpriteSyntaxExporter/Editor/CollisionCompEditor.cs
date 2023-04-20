using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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

        void OnEnable()
        {
            m_spriteLayoutComponent = (CollisionComponent)target;
            m_collision_type = serializedObject.FindProperty("type");

            m_line_collision_property = serializedObject.FindProperty("lineCollision");
            m_rect_collision_property = serializedObject.FindProperty("rectCollision");
            m_oval_collision_property = serializedObject.FindProperty("ovalCollision");
            m_sphere_collision_property = serializedObject.FindProperty("sphereCollision");
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
    }
}
