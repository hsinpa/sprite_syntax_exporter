using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="SpriteEditorSRP", menuName= "SRP/SpriteGeneralConfig")]
public class SpriteEditorSRP : ScriptableObject
{
    [System.Serializable]
    public struct EditorStruct {
        /// <summary>
        /// Lets give absolute path
        /// </summary>
        public string ExportPath;
    }

    [SerializeField]
    public EditorStruct editorStruct;
}
