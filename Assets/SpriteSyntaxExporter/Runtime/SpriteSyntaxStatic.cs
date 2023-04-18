using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hsinpa.SSE
{
    public class SpriteSyntaxStatic {
            
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
        }

    }
}