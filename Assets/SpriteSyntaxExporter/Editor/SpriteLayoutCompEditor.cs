using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Hsinpa.SSE {
    [CustomEditor(typeof(SpriteLayoutComponent))]
    public class SpriteLayoutCompEditor : Editor
    {
        SpriteLayoutComponent m_spriteLayoutComponent;
        SerializedProperty m_syntaxlayout_property;

        void OnEnable()
        {
            m_spriteLayoutComponent = (SpriteLayoutComponent)target;

            m_syntaxlayout_property = serializedObject.FindProperty("Properties");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(m_syntaxlayout_property);

            //Beauty json string
            HandlePropertiesField();

            serializedObject.ApplyModifiedProperties();
        }

        private async void HandlePropertiesField() {
            string beautified_json = await SpriteSyntaxUtility.FormatJson(m_syntaxlayout_property.stringValue);
            m_spriteLayoutComponent.Properties = beautified_json;
        }
    }
}