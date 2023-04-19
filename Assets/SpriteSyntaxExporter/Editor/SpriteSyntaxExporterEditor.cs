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

                if (g.GetType() != typeof(Texture2D))
                    continue;
                
                Texture2D targetTexture = (Texture2D)g;

                var dataProvider = factory.GetSpriteEditorDataProviderFromObject(targetTexture);
                dataProvider.InitSpriteEditorDataProvider();

                SpriteSyntaxStatic.SpriteSyntaxStruct spriteSyntaxStruct = ProcessSpriteData(dataProvider);
                spriteSyntaxStruct.name = targetTexture.name;

                string json = JsonUtility.ToJson(spriteSyntaxStruct);
                string jsonPathFilter = string.Format(jsonFullPath, spriteSyntaxStruct.name);
                string bsonPathFilter = string.Format(bsonFullPath, spriteSyntaxStruct.name);

                await Task.WhenAll(
                    //Pure Raw JSON Text
                    File.WriteAllTextAsync(jsonPathFilter, json),

                    //Binary
                    File.WriteAllTextAsync(bsonPathFilter, SpriteSyntaxUtility.ToBinary(Encoding.UTF8.GetBytes(json)))
                );
            }

            AssetDatabase.Refresh();
        }

        private static SpriteSyntaxStatic.SpriteSyntaxStruct ProcessSpriteData(ISpriteEditorDataProvider dataProvider) {
            SpriteRect[] spriteRects = dataProvider.GetSpriteRects();
            
            int rectCount = spriteRects.Length;
            SpriteSyntaxStatic.SpriteStruct[] spriteArray = new SpriteSyntaxStatic.SpriteStruct[rectCount];

            for (int i = 0; i < rectCount; i++) {
                spriteArray[i] = ProcessSpriteRect(spriteRects[i]);
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
