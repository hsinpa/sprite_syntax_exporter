 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using System.IO;
using UnityEditor.U2D.Sprites;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System;
using static Hsinpa.SSE.SpriteSyntaxStatic;

namespace Hsinpa.SSE {
    public class SpriteSyntaxExporterEditor : EditorWindow
    {
        const string StorePath = "SpriteSyntax";

        [MenuItem("Assets/Actions/ExportSpriteSyntax")]
        public async static void ExportSpriteSyntax() {

            SpriteSyntaxUtility.PrepareDirectory(StorePath);

            string jsonFullPath = Path.Combine(Application.streamingAssetsPath, StorePath,  SpriteSyntaxStatic.FileJSONPath);
            string bsonFullPath = Path.Combine(Application.streamingAssetsPath, StorePath, SpriteSyntaxStatic.FileBSONPath);

            var currentSelections = Selection.objects;

            var factory = new SpriteDataProviderFactories();
            factory.Init();

            foreach (var g in currentSelections) {
                string path = AssetDatabase.GetAssetPath(g);

                var spriteObjArray = AssetDatabase.LoadAllAssetsAtPath(path);
                var sprites = spriteObjArray.Where(q => q is Sprite).Cast<Sprite>();
                Dictionary<string, Sprite> spriteDict = new Dictionary<string, Sprite>();

                //Form dict
                foreach (var sprite in sprites) spriteDict.Add(sprite.name, sprite);

                if (g.GetType() != typeof(Texture2D))
                    continue;
                
                Texture2D targetTexture = (Texture2D)g;
                
                var dataProvider = factory.GetSpriteEditorDataProviderFromObject(targetTexture);
                dataProvider.InitSpriteEditorDataProvider();

                SpriteSyntaxStatic.SpriteSyntaxStruct spriteSyntaxStruct = await ProcessSpriteData(spriteDict, dataProvider);
                spriteSyntaxStruct.name = targetTexture.name;

                string json = JsonUtility.ToJson(spriteSyntaxStruct);

                await SpriteSyntaxUtility.SaveJSONFileToPath(spriteSyntaxStruct.name, json, jsonFullPath, bsonFullPath);
            }

            AssetDatabase.Refresh();
        }

        private static async Task<SpriteSyntaxStatic.SpriteSyntaxStruct> ProcessSpriteData(Dictionary<string, Sprite> spriteDict, ISpriteEditorDataProvider dataProvider) {
            SpriteRect[] spriteRects = dataProvider.GetSpriteRects();
            int rectCount = spriteRects.Length;


            SpriteSyntaxStatic.SpriteStruct[] spriteArray = new SpriteSyntaxStatic.SpriteStruct[rectCount];

            for (int i = 0; i < rectCount; i++) {
                Sprite sprite = spriteDict[spriteRects[i].name];

                Vector2[] sprite_vertices = sprite.vertices;
                ushort[] sprite_triangles = sprite.triangles;
                int vertices_lens = sprite_vertices.Length;
                int triangles_lens = sprite_triangles.Length;

                spriteArray[i] = ProcessSpriteRect(spriteRects[i]);

                spriteArray[i].bound_height = sprite.bounds.size.y;
                spriteArray[i].bound_width = sprite.bounds.size.x;

                spriteArray[i].vertices = new float[vertices_lens * 2];
                spriteArray[i].triangles = new int[triangles_lens];

                await Task.Run(() => {
                    for (int v = 0; v < vertices_lens; v++) {
                        int step_v = v * 2;
                        spriteArray[i].vertices[step_v] = sprite_vertices[v].x;
                        spriteArray[i].vertices[step_v + 1] = sprite_vertices[v].y;
                    }

                    for (int t = 0; t < triangles_lens; t++) {
                        spriteArray[i].triangles[t] = sprite_triangles[t];
                    }
                });
            }

            return new SpriteSyntaxStatic.SpriteSyntaxStruct {
                sprites = spriteArray
            };
        }

        private static SpriteSyntaxStatic.SpriteStruct ProcessSpriteRect(SpriteRect spriteRect) {
            return new SpriteSyntaxStatic.SpriteStruct() {
                name = spriteRect.name,
                x = (int)spriteRect.rect.x,
                y = (int)spriteRect.rect.y,
                height = (int)spriteRect.rect.height,
                width = (int)spriteRect.rect.width,

                pivot_x = spriteRect.pivot.x,
                pivot_y = spriteRect.pivot.y,
            };
        }

    }
}
