using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Util.Caches
{
    
    /**
     * Petite classe générique qui permet de créer des caches pour Resources.Load facilement, pour tout
     * type de trucs.
     */
    public class ResourceCache<T> where T : Object
    {
        
        private readonly Dictionary<String, T> _objects = new();

        /**
         * Si le path est contenu dans le dictionnaire, alors on retourne la valeur associée.
         * Si non, bah on la load, et on l'ajoute au cache.
         *
         * <param name="path">Chemin d'accès demandé</param>
         */
        public T Get(string path)
        {
            if(!_objects.ContainsKey(path))
                _objects.Add(path, Resources.Load<T>(path));
            return _objects.GetValueOrDefault(path);
        }

    }
}