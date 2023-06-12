using System.Collections;
using System.Collections.Generic;
using Unity.Plastic.Newtonsoft.Json.Linq;
using UnityEngine;


namespace Hsinpa.SSE {
    public class SpriteLayoutComponent : MonoBehaviour
    {
        [TextArea(15, 20)]
        public string Properties;       
        
        public string MinimizeProperties {
            get {
                if (Properties == null) return "";

                return Properties.Replace(" ", "").Replace("\n", "");
            } 
        }
    }
}