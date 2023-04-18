using UnityEditor;
using UnityEditor.U2D.Sprites;
using UnityEngine;

namespace Hsinpa.SSE
{
    public class SpritePostProcess : AssetPostprocessor
    {
        private void OnPreprocessTexture() {
            var factory = new SpriteDataProviderFactories();
            factory.Init();
            var dataProvider = factory.GetSpriteEditorDataProviderFromObject(assetImporter);
            dataProvider.InitSpriteEditorDataProvider();

            /* Use the data provider */

            // Apply the changes made to the data provider
            dataProvider.Apply();
        }
    }
}
