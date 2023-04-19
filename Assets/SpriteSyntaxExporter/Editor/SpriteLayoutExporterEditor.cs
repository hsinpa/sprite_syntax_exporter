using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Hsinpa.SSE
{
    public class SpriteLayoutExporterEditor : EditorWindow
    {
        const string StorePath = "SpriteLayout";

        [MenuItem("Assets/Actions/ExportSpriteLayout")]
        public async static void ExportSpriteLayout()
        {
            Camera camera = Camera.main;

            if (camera == null) {
                Debug.LogError("No camera are found");
                return;
            }

            float frame_height = 2f * camera.orthographicSize;
            float frame_width = frame_height * camera.aspect;
            Debug.Log($"Camera width {frame_width}, height {frame_height}");

            SpriteSyntaxUtility.PrepareDirectory(StorePath);

            string scene_name = SceneManager.GetActiveScene().name;

            SpriteSyntaxStatic.SceneLayoutStruct sceneLayoutStruct = new SpriteSyntaxStatic.SceneLayoutStruct();
            SpriteLayoutComponent[] comps = GameObject.FindObjectsOfType<SpriteLayoutComponent>();
            int c_lens = comps.Length;

            for (int i = 0; i < c_lens; i++) {
                ProcessSpriteLayoutComponent(comps[i]);
            }

            sceneLayoutStruct.frame_height = frame_height;
            sceneLayoutStruct.frame_width = frame_width;
        }

        private async static Task ProcessSpriteLayoutComponent(SpriteLayoutComponent spriteLayoutComponent) {
            SpriteSyntaxStatic.SpriteLayoutStruct spriteLayoutStruct = new SpriteSyntaxStatic.SpriteLayoutStruct();
            SpriteRenderer sprite = spriteLayoutComponent.GetComponent<SpriteRenderer>();

            Debug.Log("ProcessSpriteLayoutComponent " + spriteLayoutComponent.name);
        } 
    }
}