using System.Collections.Generic;
using UnityEngine;

namespace physic
{
    
    /**
     * Le component qui permet de faire un objet répondant aux lois de la gravité dans le jeu.
     */
    [RequireComponent (typeof (Rigidbody2D))]
    public class Attractor : MonoBehaviour
    {
    
        // Liste des attracteurs présents dans le jeu.
        private static readonly List<Attractor> Attractors = new List<Attractor>();
        
        // Constante gravitationnelle utilisée.
        private static float G = 6.64f;

        // Le rigidbody, nécessaire au bon fonctionnement
        private Rigidbody2D _rigidBody;
    
        // La vélocité de départ.
        public Vector2 startVelocity;
        /**
         * La planète, si autoPlanet est true, alors elle sera toujours l'objet exerçant sur nous la plus
         * grosse force d'attraction.
         * Le sprite sera toujours tourné les pieds en direction de la planète.
         */
        public Attractor planet = null;
        /**
         * Si mit à false, permet de définir manuellement la planète, qui ne changera pas quel que soit l'attraction
         * des autres objets.
         */
        public bool autoPlanet = true;
        public bool onlyAttractWhenPlanet = false; 

        // Ces objets ne vont pas être affectés par la gravité de ce component.
        private List<Attractor> _dontAttract = new();

        /**
         * Définition du RigidBody dès le début.
         */
        private void Awake()
        {
            this._rigidBody = this.GetComponent<Rigidbody2D>();
        }

        /**
         * Au début, on s'assure de désactiver la gravité d'Unity.
         * On ajoute la vélocité initiale si existante.
         */
        public virtual void Start()
        {
            this._rigidBody.gravityScale = 0;
            if (!this._rigidBody.isKinematic) 
                this._rigidBody.velocity = startVelocity;
        
        }

        /**
         * Permet de savoir si cet élément en attire un autre.
         * <param name="attractor">L'élément qui est peut être attiré par celui-ci.</param>
         */
        public bool DoAttract(Attractor attractor)
        {
            return !this._dontAttract.Contains(attractor);
        }
        
        /**
         * Désactive l'attraction gravitationnelle de cet élément sur l'autre.
         * <param name="other">L'élément qu'on ne veut plus attirer.</param>
         */
        public void DontAttract(Attractor other)
        {
            this._dontAttract.Add(other);
        }

        /**
         * Lorsqu'activé, on l'ajoute aux attracteurs.
         */
        private void OnEnable()
        {
            Attractors.Add(this);
        }

        /**
         * Lorsque désactivé, on le retire.
         */
        private void OnDisable()
        {
            Attractors.Remove(this);
        }
        
        /**
         * Permet d'obtenir la planète.
         */
        public Attractor GetPlanet()
        {
            return this.planet;
        }
    
        /**
         * Lors des updates physiques, on va s'orienter en direction de la planète qui nous attire
         * Attirer tous les attracteurs vers nous en utilisant la formule de Newton
         * Et définir, si nécessaire, notre planète à l'objet qui exerce sur nous la plus grosse attraction
         */
        void FixedUpdate()
        {
            // Si la planète n'est pas nulle, alors on s'oriente en sa direction
            if (this.planet)
            {
                Vector3 targetDir = (this.transform.position - this.planet.transform.position).normalized;
                Quaternion toAttain = Quaternion.FromToRotation(this.transform.up, targetDir) * this.transform.rotation;
                this.transform.rotation = Quaternion.Lerp(this.transform.rotation, toAttain, 0.1F);
            }
            
            float mostAttraction = 0F;
            foreach (Attractor attractor in Attractors)
            {
                // Si on peut attirer "attractor", alors on l'attire vers nous.
                if (attractor != this && (!this.onlyAttractWhenPlanet || attractor.planet == this))
                    this.Attract(attractor);
                // Les lignes ci desous permette de définir la planète à l'objet qui exerce sur nous la
                // plus grosse attraction
                if (attractor.DoAttract(this) && this.autoPlanet)
                {
                    float attraction = attractor.GetAttractionForce(this);
                    if (attraction > mostAttraction)
                    {
                        this.planet = attractor;
                        mostAttraction = attraction;
                    }
                }
            }
        }

        /**
         * Méthode qui permet d'attirer un attracteur vers celui-ci.
         * <param name="other">L'attracteur a attirer.</param>
         */
        private void Attract(Attractor other)
        {
            if (!this.DoAttract(other))
                return;
            Vector3 direction = (this._rigidBody.position - other._rigidBody.position);
            float distance = direction.magnitude;
        
            if(distance == 0)
                return;
        
            float forceMagnitude = G * (this._rigidBody.mass * other._rigidBody.mass) / Mathf.Pow(distance, 2);
            Vector2 force = direction.normalized * forceMagnitude;

            if(other.OnAttractedBy(this, ref force)) 
                other._rigidBody.AddForce(force);
        }

        /**
         * Utilise la formule de newton pour calculer l'attraction gravitiationnelle entre cet objet, et l'autre.
         * <param name="other">L'autre objet en question</param>
         */
        public float GetAttractionForce(Attractor other)
        {
            Vector3 direction = (this._rigidBody.position - other._rigidBody.position);
            float distance = direction.magnitude;
            if(distance == 0)
                return 1;
            return G * (this._rigidBody.mass * other._rigidBody.mass) / Mathf.Pow(distance, 2);
        }

        /**
         * Permet d'obtenir le Rigidbody.
         */
        public Rigidbody2D GetRigidBody()
        {
            return this._rigidBody;
        }

        public virtual bool OnAttractedBy(Attractor attractor, ref Vector2 force)
        {
            return true;
        }
        
    }
}
