using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hsinpa.SSE
{
    public class SpriteSyntaxStatic {
        public const string FileJSONPath = "{0}.json";
        public const string FileBSONPath = "{0}.bson";

        [System.Serializable]
        public struct SpriteSyntaxStruct {
            public string name;
            public SpriteStruct[] sprites;
        }

        [System.Serializable]
        public struct SpriteStruct
        {
            public string name;
            public int x;
            public int y;
            public int width;
            public int height;

            public float pivot_x;
            public float pivot_y;

            public float bound_width;
            public float bound_height;

            public float[] vertices;
            public int[] triangles;
        }


        [System.Serializable]
        public struct SceneLayoutStruct
        {
            public string name;
            public float frame_height;
            public float frame_width;

            public SpriteLayoutStruct[] spriteLayoutStructs;
        }

        [System.Serializable]
        public struct SpriteLayoutStruct
        {
            public string texture_name;
            public string sprite_name;

            public float x;
            public float y;
            public float scale_x;
            public float scale_y;
            public float rotation;

            public bool is_valid => !string.IsNullOrEmpty(texture_name) && !string.IsNullOrEmpty(sprite_name);
        }
    }
}