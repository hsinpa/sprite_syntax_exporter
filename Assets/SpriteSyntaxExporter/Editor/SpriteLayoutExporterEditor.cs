using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using static Hsinpa.SSE.SpriteSyntaxStatic;

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

            string jsonFullPath = Path.Combine(Application.streamingAssetsPath, StorePath, SpriteSyntaxStatic.FileJSONPath);
            string bsonFullPath = Path.Combine(Application.streamingAssetsPath, StorePath, SpriteSyntaxStatic.FileBSONPath);

            float frame_height = 2f * camera.orthographicSize;
            float frame_width = frame_height * camera.aspect;
            Debug.Log($"Camera width {frame_width}, height {frame_height}");

            SpriteSyntaxUtility.PrepareDirectory(StorePath);

            string scene_name = SceneManager.GetActiveScene().name;

            SpriteSyntaxStatic.SceneLayoutStruct sceneLayoutStruct = new SpriteSyntaxStatic.SceneLayoutStruct();
            SpriteLayoutComponent[] comps = GameObject.FindObjectsOfType<SpriteLayoutComponent>();

            int c_lens = comps.Length;
            SpriteSyntaxStatic.SpriteLayoutStruct[] layouts = new SpriteSyntaxStatic.SpriteLayoutStruct[c_lens];

            for (int i = 0; i < c_lens; i++) {
                layouts[i] = ProcessSpriteLayoutComponent(comps[i]);
            }

            sceneLayoutStruct.frame_height = frame_height;
            sceneLayoutStruct.frame_width = frame_width;
            sceneLayoutStruct.name = scene_name;
            sceneLayoutStruct.spriteLayoutStructs = layouts;

            await SpriteSyntaxUtility.SaveJSONFileToPath(scene_name, JsonUtility.ToJson(sceneLayoutStruct), jsonFullPath, bsonFullPath);

            AssetDatabase.Refresh();
        }

        private static SpriteSyntaxStatic.SpriteLayoutStruct ProcessSpriteLayoutComponent(SpriteLayoutComponent spriteLayoutComponent) {
            SpriteSyntaxStatic.SpriteLayoutStruct spriteLayoutStruct = new SpriteSyntaxStatic.SpriteLayoutStruct();
            SpriteRenderer sprite = spriteLayoutComponent.GetComponent<SpriteRenderer>();

            spriteLayoutStruct.sprite_name = sprite.sprite.name;
            spriteLayoutStruct.texture_name = sprite.sprite.texture.name;

            spriteLayoutStruct.x = spriteLayoutComponent.transform.position.x;
            spriteLayoutStruct.y = spriteLayoutComponent.transform.position.y;

            spriteLayoutStruct.scale_x = spriteLayoutComponent.transform.localScale.x;
            spriteLayoutStruct.scale_y = spriteLayoutComponent.transform.localScale.y;

            spriteLayoutStruct.rotation = spriteLayoutComponent.transform.eulerAngles.z * Mathf.Deg2Rad;

            return spriteLayoutStruct;
        } 
    }
}