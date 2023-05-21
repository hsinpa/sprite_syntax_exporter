using System.Collections;
using System.Collections.Generic;
using Unity.Plastic.Newtonsoft.Json.Linq;
using UnityEngine;


namespace Hsinpa.SSE {
    public class SpriteLayoutComponent : MonoBehaviour
    {
        [TextArea(15, 20)]
        public string Properties;       
        
        public string MinimizeProperties => Properties.Replace(" " , "").Replace("\n", "");
    }
}