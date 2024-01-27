using UnityEngine;
using util;

namespace Util.Caches
{
    /**
     * Définitions des différents caches. Y'en a qu'un pour l'instant lol.
     */
    public abstract class Caches
    {
        
        // Le cache des Sprite.
        public static ResourceCache<Sprite> SpriteCache = new ResourceCache<Sprite>();

    }
}