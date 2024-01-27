using UnityEngine;

namespace Util
{
    
    /**
     * Ce component va nous permettre de nous assurer que les rotations d'un objet restent lock à 0 sur certains axes
     * que nous pouvons donc définir.
     */
    public class RotationLock : MonoBehaviour
    {
        
        // Les axes à lock, ou non.
        public bool x, y, z;

        /**
         * A chaque frame, on s'assure que les axes qui le nécessitent sont à 0.
         */
        void Update()
        {
            Vector3 eulers = this.transform.rotation.eulerAngles;

            if (this.x)
                eulers.x = 0;
            if (this.y)
                eulers.y = 0;
            if (this.z)
                eulers.z = 0;

            this.transform.rotation = Quaternion.Euler(eulers);
        }
    }
}