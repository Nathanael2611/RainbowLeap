using System;
using entity;
using Entity.Grabbables;
using input;
using physic;
using Scrtwpns.Mixbox;
using UnityEngine;
using util;
using Util;
using Color = UnityEngine.Color;

namespace Entity.Player
{
    /**
     * Component principal du·de la joueur·se.
     */
    [RequireComponent( typeof(Collider2D) )]
    [RequireComponent( typeof(Rigidbody2D) )]
    [RequireComponent( typeof(PlayerAttractor) )]
    [RequireComponent( typeof(PlayerSpriteManager))]
    public class Frog : MonoBehaviour, IInputListener
    {
    
        // Instance unique du·de la joueur·se.
        public static Frog TheFrog;
        
        /**
         * De nombreuuuuuses instances de pleins de components contenus dans le GameObject du·de la joueur·se.
         */
        private Rigidbody2D     _rigidBody;
        private PlayerSpriteManager _spriteManager;
        private Collider2D      _collider2D;
        private PlayerAttractor       _attractor;
        private SpriteRenderer  _spriteRenderer;
        
        private float           _aimValue, _aimAngle, _turnAngle, _aimStop, _aimFactor = 1;
        private Vector2         _aimDirection;
        private bool            _onGround = false;

        public bool reverseJumpAndDirection = false;
        // Le nombre d'actions qu'il reste au·à la joueur·se
        public int actions = 10, score = 0, maxActions = 30;

        private float _turnValue = 1, _lastJump;

        /**
         * jumpStrength, permet depuis l'éditeur de changer la puissance du saut de la grenouille.
         * tongueStrength, premet de changer la force de départ de la langue.
         * tongueMaxDistance, la distance maximum à laquelle peut se trouver la langue avant d'entamer le chemin retour. 
         */
        public float jumpStrength = 1000, tongueStrength = 1200, tongueMaxDistance;

        // L'instance de la langue actuelle. (Il ne peut y en avoir qu'une à la fois)
        private Tongue _tongue = null;

        private Vector2 _lastJumpDirection = Vector2.zero;

        private float _setObjectiveTime = 0;
        private Color _baseColor = Color.white;
        public Color objectiveColor = Color.white;
        private float _objectiveDensity = 1;

        /**
         * Au tout début, initialise les différents components nécessaires au bon fonctionnement du code.
         */
        private void Awake()
        {
            Frog.TheFrog = this;
            this._rigidBody = this.GetComponent<Rigidbody2D>();
            this._rigidBody.gravityScale = 0;
            this._spriteRenderer = this.GetComponent<SpriteRenderer>();
            this._collider2D = this.GetComponent<Collider2D>();
            this._attractor = this.GetComponent<PlayerAttractor>();
            this._attractor.frog = this;
            this._spriteManager = this.GetComponent<PlayerSpriteManager>();
        }

        public Tongue GetTongue()
        {
            return this._tongue;
        }

        /**
         * Définition de quelques valeurs au lancement de l'objet.
         */
        private void Start()
        {
            PressManager.Instance.RegisterListener(this);
            this.objectiveColor = this.GetColor();
            this._baseColor = this.GetColor();
        }

        /**
         * Lors de la mise à jour physique, vérifie si le·la joueur·se touche un sol.
         * Et définit la variable onGround.
         */
        private void FixedUpdate()
        {
            Vector2 pos = this._rigidBody.position;
            
            if (this._attractor.planet)
            {
                RaycastHit2D[] hits = Physics2D.RaycastAll(pos, -(pos - this._attractor.planet.GetRigidBody().position).normalized, _collider2D.bounds.extents.y + 0.1f);
                int i = 0;
                foreach (var hit2D in hits)
                {
                    if (hit2D.rigidbody == this._rigidBody) continue;
                    i++;
                }
                this._onGround = i > 0;
            }
        }

        /**
         * Retourne la similitude entre la couleur du·de la joueur·se et la planète.
         */
        public float GetSimilitude()
        {
            if (this._attractor.planet && this._attractor.planet.isActiveAndEnabled)
            {
                Planet planet = (Planet) this._attractor.planet;
                return (float)Mathf.Max(0, Mathf.Min(100f, 120f - (float) CEDES.CalculateDeltaE(planet.GetPlanetColor(), this._spriteRenderer.color)));
            }

            return 0F;
        }

        /**
         * Va Va mettre à jour les valeurs nécessaires à chaque frame.
         */
        private void Update()
        {
            float colorChangeProgress = Math.Max(0, Math.Min(1, (Time.time - this._setObjectiveTime) * 1 / 2));
            this._spriteRenderer.color = Mixbox.Lerp(this._baseColor, this.objectiveColor, colorChangeProgress * this._objectiveDensity);
            if (PressManager.Instance.IsHolding())
            {
                this._aimValue += Time.unscaledDeltaTime ;
            }
            
            //if (Time.unscaledTime - this._lastJump > 1)
                //this._turnValue += Time.unscaledDeltaTime;

            if (this.IsAiming())
            {
                this._aimAngle = (Mathf.Sin(this._aimValue) * (70 * this._aimFactor));
            }
            else
            {
                this._turnAngle = (this.GetNonAimingAngle());
            }
        }

        // ReSharper disable Unity.PerformanceAnalysis
        /**
         * Va sauter ou lancer la langue du·de la joueur·se dans une direction indiquée par l'angle.
         * <param name="angle">L'angle dans lequel sauter/lancer sa langue</param>
         * <param name="forceTongue">Si oui ou non on veut lancer la langue. true = langue, false = saut</param>
         */
        private void TryGrab(float angle, bool forceTongue)
        {
            this._aimDirection = Helpers.Rotate(new Vector2(0, 1), -Mathf.Deg2Rad * angle);

            if (this._onGround && !forceTongue)
            {
                this._rigidBody.AddRelativeForce(this._aimDirection * this.jumpStrength);
            }
            this._lastJumpDirection = this._aimDirection;
            if(forceTongue || !this._onGround)
            {
                this.RequestTongue();
            }
            this._lastJump = Time.unscaledTime;
        }

        /**
         * Va simplement créer une nouvelle langue et la lancer dans la direction visée par le·la joueur·se.
         */
        private void RequestTongue()
        {
            if (!this._tongue)
            {
                GameObject tongue = new GameObject("Tongue");
                tongue.AddComponent<Tongue>();
                Tongue component = tongue.GetComponent<Tongue>();
                this._tongue = component;
                component.Initialize(this, this.transform.TransformDirection(this._lastJumpDirection), this.tongueStrength, this.tongueMaxDistance);
            }
        }

        /**
         * Récupère l'angle de tir de la langue.
         *      Si le·la joueur·se vise, alors on retourne l'angle de visée.
         *      Si iel ne vise pas, alors on retourne l'angle de saut.
         */
        public float GetAimAngle()
        {
            return this.IsAiming() ? (this._aimAngle) : (this._turnAngle);
        }

        /**
         * Récupère le component Attractor du·de la joueur·se.
         */
        public Attractor GetAttractor()
        {
            return this._attractor;
        }

        /**
         * Récupère la direction visée.
         */
        public Vector2 GetAimDirection() => this._aimDirection;
        
        /**
         * Récupère l'angle de saut dans lequel le·la joueur·se va sauter lors d'un saut simple.
         */
        public float GetNonAimingAngle()
        {
            //float cos = Mathf.Cos(this._turnValue * 3F);
            //return (int) (cos < 0 ? Mathf.Floor(cos) : Mathf.Ceil(cos)) * 75;
            return (int) (this._turnValue < 0 ? Mathf.Floor(this._turnValue) : Mathf.Ceil(this._turnValue)) * 75;
        }
        
        /**
         * Renvoie true si le joueur hold la touche.
         */
        public bool IsAiming()
        {
            return PressManager.Instance.IsHolding();
        }

        /**
         * Récupère simplement la couleur du SpriteRenderer.
         */
        public Color GetColor()
        {
            return this._spriteRenderer.color;
        }

        /**
         * Définit la couleur objectif, celle entre laquelle on va interpoler.
         * <param name="color">La couleur vers laquelle on va tendre.</param>
         * <param name="density">La densité de la couleur vers laquelle on va teindre</param>
         * TODO: Ajouter un pourcentage d'interpolation pour gérer les futures boules de tailles différentes.
         */
        public void setObjectiveColor(Color color, float density)
        {
            this._setObjectiveTime = Time.time;
            this._objectiveDensity = density;
            this._baseColor = this.GetColor();
            this.objectiveColor = color;
        }
        
        
        /**
         * Gère le double clique.
         */
        public void DoubleClick()
        {
            if (!this.reverseJumpAndDirection)
            {
                this.ChangeDirectionClick();
            }
            else
            {
                this.JumpClick();
            }
        }

        public void JumpClick()
        {
            // Ici, ForceTongue est à false, pour lui demander de ne pas lancer la langue.
            // Ce qui revient à faire un saut.
            if (this._tongue)
            {
                this._tongue.Comeback();
            }
            else
            {
                this.TryGrab(this._turnAngle, false);
            }
        }
        
        /**
         * Lors d'un clique simple.
         */
        public void SimpleClick()
        {
            if (this.reverseJumpAndDirection)
            {
                this.ChangeDirectionClick();
            }
            else
            {
                this.JumpClick();
            }
        }

        public void ChangeDirectionClick()
        {
            if (this._turnValue > 0)
            {
                this._turnValue = -1;
            }
            else
            {
                this._turnValue = 1;
            }
        }

        /**
         * Permet de commencer à viser quand le·la joueur·se commence le hold.
         */
        public void HoldStart()
        {
            this._aimValue = 0;
            this._aimFactor = this._turnAngle > 0 ? 1 : -1;
            if (this._tongue)
            {
                this._tongue.Comeback();
            }
        }

        /**
         * Permet de stopper le viseur lorsque le·la joueur·se lâche la touche.
         */
        public void HoldEnd()
        {
            this._aimStop = Time.unscaledTime;
            this.TryGrab(this._aimAngle, true);
        }

        /**
         * Renvoie true si le·la joueur·se est en collision avec le sol.
         */
        public bool OnGround()
        {
            return this._onGround;
        }

        /**
         * Récupère le RigidBody2D
         */
        public Rigidbody2D GetRigidbody()
        {
            return this._rigidBody;
        }
        
        /**
         * Permet de récolter le collectible lors de la collision avec celui-ci.
         */
        public void OnCollisionEnter2D(Collision2D other)
        {
            if (this._tongue)
            {
                foreach (ContactPoint2D point in other.contacts)
                {
                    Grabbable grabbable = point.collider.gameObject.GetComponent<Grabbable>();
                    if (grabbable)
                    {
                        this._tongue.PlayerCollideWith(grabbable);
                    }
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (this._tongue)
            {
                Grabbable grabbable = other.gameObject.GetComponent<Grabbable>();
                if (grabbable)
                {
                    this._tongue.PlayerCollideWith(grabbable);
                }
            }
        }

        public Collider2D GetCollider()
        {
            return this._collider2D;
        }


        public void IncrementActions(int i)
        {
            this.actions = Math.Min(this.maxActions, Math.Max(this.actions + i, 0));
        }
    }
    
}
