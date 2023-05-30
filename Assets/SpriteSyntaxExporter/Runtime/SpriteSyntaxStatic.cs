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
            public int width;
            public int height;
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

            public float screen_height;
            public float screen_width;

            public SpriteLayoutStruct[] spriteLayoutStructs;
        }

        [System.Serializable]
        public struct SpriteLayoutStruct
        {
            public int id;
            public string texture_name;
            public string sprite_name;

            public float x;
            public float y;
            public float scale_x;
            public float scale_y;
            public float rotation;

            public int flip_x;
            public int flip_y;

            public int tag;
            public string properties;
            public CollisionStruct collisionStruct;
            public ConstraintStruct constraintStruct;

            public bool is_valid => !string.IsNullOrEmpty(texture_name) && !string.IsNullOrEmpty(sprite_name);
        }

        #region Collisoion
        public enum CollisionType { Line, Rect, Oval, Sphere }

        [System.Serializable]
        public struct CollisionStruct {
            public CollisionType collisionType;
            public string data;
        }

        [System.Serializable]
        public struct LineCollision {
            public Vector2 point_a;
            public Vector2 point_b;
        }

        [System.Serializable]
        public struct RectCollision
        {
            public float x;
            public float y;

            public float height;
            public float width;
        }

        [System.Serializable]
        public struct OvalCollision
        {
            public SphereCollision sphere_a;
            public SphereCollision sphere_b;
        }

        [System.Serializable]
        public struct SphereCollision
        {
            public float x;
            public float y;
            public float radius;
        }

        [System.Serializable]
        public struct ConstraintStruct
        {
            public float max_rotation;
            public float min_rotation;
            public float rest_point;

            public float max_x;
            public float min_x;
            public float rest_x;

            public float max_y;
            public float min_y;
            public float rest_y;
        }
        #endregion
    }
}