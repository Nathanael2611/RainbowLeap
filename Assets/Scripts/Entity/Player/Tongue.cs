using System;
using physic;
using Unity.VisualScripting;
using UnityEngine;
using Util;

namespace entity
{
    /**
     * Component de la langue lancée par le·la joueur·se.
     */
    public class Tongue : MonoBehaviour
    {

        // Instance du·de la joueur·se auquel·à laquelle appartient la langue.
        private Player _player;

        /**
         * Tout un tas de définitions de components utilisés.
         */
        private LineRenderer _renderer;
        private Rigidbody2D _rigidbody;
        private CircleCollider2D _collider2D;
        private Attractor _attractor;

        /**
         * Préalablement définies depuis les variables du·de la joueur·se, on retrouve ces variables ici
         * aussi.
         */
        public float tongueMaxDistance, tongueStrength;

        // La langue a t-elle amorcé son retour vers le·la joueur·se?
        private bool _comeBack = false;
        // Temps auquel la langue a été lancée.
        private float _launchTime = 0;

        // Point absolu auquel la langue est attachée dans le monde.
        private Vector2 _lockPoint = Vector2.zero;
        // Le collectible que la langue accroche actuellement.
        private Grabbable _grabbable;

        /**
         * Définition des components.
         */
        private void Awake()
        {
            this._renderer = this.AddComponent<LineRenderer>();
            this._rigidbody = this.AddComponent<Rigidbody2D>();
            this._collider2D = this.AddComponent<CircleCollider2D>();
            this._attractor = this.AddComponent<Attractor>();
            RotationLock rotationLock = this.AddComponent<RotationLock>();
            rotationLock.x = true;
            rotationLock.y = true;
            rotationLock.z = true;
        }

        public void Comeback()
        {
            this._comeBack = true;
            this._lockPoint = Vector2.zero;
            if (this._grabbable)
            {
                this._grabbable.ResetScale();
            }
            this._grabbable = null;
        }

        /**
         * Initialisation de la langue, et envoi de cette dernière.
         * <param name="player">Le·la joueur·se qui lance la langue.</param>
         * <param name="aimDirection">La direction absolue dans laquelle lancer la langue.</param>
         * <param name="tongueStrength">La puissance de lancement de la langue.</param>
         * <param name="tongueMaxDistance">La distance que peut parcourir la langue avant d'être forcée au retour.</param>
         */
        public void Initialize(Player player, Vector2 aimDirection, float tongueStrength, float tongueMaxDistance)
        {
            this.tongueStrength = tongueStrength;
            this.tongueMaxDistance = tongueMaxDistance;
            this._renderer.positionCount = 2;
            this._renderer.startWidth = 0.22F;

            Attractor playerAttractor = player.GetComponent<Attractor>();
            this._attractor.DontAttract(playerAttractor);
            playerAttractor.DontAttract(this._attractor);
            this._attractor.autoPlanet = false;
            
            this._renderer.material = Resources.Load<Material>("Materials/TongueMaterial") as Material;

            this._player = player;

            this._renderer.sortingOrder = this._player.GetComponent<SpriteRenderer>().sortingOrder - 1;

            
            this.transform.position = this._player.transform.position + (new Vector3(aimDirection.x, aimDirection.y).normalized * 1);
            Physics2D.IgnoreCollision(this._collider2D, player.GetComponent<Collider2D>(), true);

            this._collider2D.radius = 0.2f;
            this._rigidbody.mass = 1;

            this._rigidbody.AddForce(aimDirection.normalized * tongueStrength);

            this._launchTime = Time.time;

        }

        /**
         * A chaque frame, on va mettre à jour des éléments, et dessiner la langue via le LineRenderer
         * On dessine une ligne rouge entre la position du joueur·se·s et celle de la langue afin de
         * les relier.
         *
         * On en profite aussi pour vérifier les distances, amorcer le retour si nécessaire, et quelques
         * autres trucs utiles.
         *
         * Si la langue est revenue au·à la joueur·se, alors on la détruit.
         */
        private void Update()
        {
            if (!this._renderer)
                return;
            this._attractor.planet = this._player.GetAttractor().planet;
            

            if (this._lockPoint != Vector2.zero)
            {
                this.transform.position = new Vector3(this._lockPoint.x, this._lockPoint.y);
            }
            Vector3[] positions = new Vector3[2];
            this._renderer.startColor =  Color.red;
            this._renderer.endColor =  Color.red;
            positions[0] = this._player.transform.position;
            positions[1] = this.transform.position;
            this._renderer.SetPositions(positions);

            if (this._grabbable)
            {
                this._grabbable.GrabUpdate(this._player);
            }

            var distance = Vector3.Distance(this.transform.position, this._player.transform.position);
            if (distance > this.tongueMaxDistance || Time.time - this._launchTime > 2 || this._lockPoint != Vector2.zero)
            {
                this._comeBack = true;
            }

            this._rigidbody.isKinematic = this._comeBack;
            
            if (this._comeBack)
            {
                this._rigidbody.velocity = Vector2.zero;
                if (this._lockPoint == Vector2.zero)
                {
                    this.transform.position = Vector3.Lerp(this.transform.position, this._player.transform.position,
                        3F * Time.deltaTime);
                }
                else
                {
                    this._player.GetRigidbody().velocity = (-(this._player.GetRigidbody().position - this._rigidbody.position).normalized * 6.5F);
                }

                if(distance < 1 && !_grabbable)
                    GameObject.Destroy(this.gameObject);
            }
        }
        
        /**
         * Appelé lorsqu'un joueur rentre en collision avec un collectible, on passe par la langue.
         *
         * En gros, on vérifie si le collectible est celui que la langue a attrapé, et si c'est le cas,
         * alors on demande au collectible de se faire récupérer par le·la joueur·se.
         */
        public void PlayerCollideWith(Grabbable grabbable)
        {
            if (this._grabbable == grabbable)
            {
                this._grabbable.PlayerGrab(this._player);
            }
        }

        /**
         * Lors d'un impact, on accroche la langue au point d'impact.
         *
         * Si le point touché est un collectible, alors on le définit comme tel.
         */
        private void OnCollisionEnter2D(Collision2D other)
        {
            ContactPoint2D contactPoint2D = other.contacts[0];
            this._lockPoint = contactPoint2D.point;
            GameObject hit = contactPoint2D.collider.gameObject;
            Grabbable grabbable = hit.GetComponent<Grabbable>();
            if (grabbable)
            {
                this._grabbable = grabbable;
                this._grabbable.TongueGrab(this, this._player);
            }
        }
    }
    
}