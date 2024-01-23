using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using Object = UnityEngine.Object;

namespace util
{
    public class ResourceCache<T> where T : Object
    {

        
        private Dictionary<String, T> sprites = new Dictionary<string, T>();

        public T Get(string path)
        {
            if(!sprites.ContainsKey(path))
                sprites.Add(path, Resources.Load<T>(path));
            return sprites.GetValueOrDefault(path);
        }

    }
}