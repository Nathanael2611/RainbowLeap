using System;
using System.Collections.Generic;
using UnityEngine;

namespace input
{
    
    /**
     * Ce component va manager et dispenser les one button inputs.
     */
    public class PressManager : MonoBehaviour
    {
        
        /**
         * Instance supposée unique du PressManager, qui doit être un objet unique.
         */
        public static PressManager Instance;

        // La touche du clavier à utiliser pour le one button.
        public KeyCode keyToUse = KeyCode.Space;
        // L'interval de temps dans lequel un double clique est accepté.
        public float doubleClickTime = 0.3F;

        // Liste de tous les listeners enregistrés.
        private List<IInputListener> _listeners = new();
        // Variables utiles pour la logique du code.
        private float _lastDown = -9, _lastUp = 0;
        private bool _clickPending = false, _wasHolding = false;

        /**
         * Définition de l'instance.
         */
        private void Awake()
        {
            Instance = this;
        }

        /**
         * Dans cette méthode se trouve toute la logique derrière les différents inputs
         * possibles avec un bouton.
         *
         * Elle fonctionne principalement avec un principe de temps et de comparaison.
         */
        private void Update()
        {
            // A chaque update, on définit l'instance supposée unique, juste pour être sûr·e·s.
            Instance = this;

            // Si la touche est pressée, et qu'elle a déjà été pressée il y a moins de X secondes, alors on dispense un double clique.
            // Sinon, on annonce qu'on a besoin d'un clique.
            // On définit la dernière fois que la touche a été pressée à l'instant T.
            if (Input.GetKeyDown(this.keyToUse))
            {
                if (Time.unscaledTime - this._lastDown < this.doubleClickTime)
                    this.DoubleClick();
                else
                    this._clickPending = true;
                this._lastDown = Time.unscaledTime;
            }

            // Si la touche est relevée, alors on prévient notre programme en définissant _lastUp a l'instant T.
            if (Input.GetKeyUp(this.keyToUse))
                this._lastUp = Time.unscaledTime;

            // Si lastUp est supérieur à lastDown, que le temps depuis la dernière fois que la touche a été
            // pressée est supérieur au temps de double clique:
            if (this._lastUp > this._lastDown && (Time.unscaledTime - this._lastDown) > this.doubleClickTime)
            {
                // Si on attendait un clique, on le trigger
                if (this._clickPending)
                {
                    this.SimpleClick();
                }
                // Sinon, si jamais on était entrain de hold à la frame précédente, on arrête le hold.
                if (this._wasHolding)
                {
                    this.HoldStop();
                }
            }

            // Si un hold est en cours
            if (this.IsHolding())
            {
                // Nous n'attendons plus de click, c'est un hold maintenant.
                this._clickPending = false;
                // Si nous n'étions pas entrain de hold au tour précédent, alors on prévient
                // les listeners qu'un hold a lieu.
                if (!this._wasHolding)
                    this.HoldStart();
            }
        }

        /**
         * Définit le wasHolding à true, pour la frame suivante.
         * Prévient tous les listeners qu'un hold a débuté.
         */
        private void HoldStart()
        {
            this._wasHolding = true;
            foreach (IInputListener listener in this._listeners)
            {
                listener.HoldStart();
            }
        }

        /**
         * Met fin à l'attente d'un clique.
         * Dispense la fin d'un hold à tous les listeners.
         */
        private void HoldStop()
        {
            this._wasHolding = false;
            foreach (IInputListener listener in this._listeners)
            {
                listener.HoldEnd();
            }
        }

        /**
         * Met fin à l'attente d'un clique.
         * Dispense le double-clique à tous les listeners.
         */
        public void DoubleClick()
        {
            this._clickPending = false;
            foreach (IInputListener listener in this._listeners)
            {
                listener.DoubleClick();
            }
        }

        /**
         * Met fin à l'attente d'un clique.
         * Dispense le clique simple à tous les listeners.
         */
        public void SimpleClick()
        {
            this._clickPending = false;
            foreach (IInputListener listener in this._listeners)
            {
                listener.SimpleClick();
            }
        }

        /**
         * Retourne le temps depuis lequel on est resté·e·s appuyé·e·s sur le bouton.
         */
        public float GetHoldTime()
        {
            return this._lastUp > this._lastDown
                ? 0
                : Math.Max(0, Time.unscaledTime - this._lastDown - this.doubleClickTime);
        }

        /**
         * Retourne simplement true si jamais la touche est hold.
         */
        public bool IsHolding()
        {
            return GetHoldTime() > 0;
        }

        /**
         * Permet d'enregistrer un listener, qui écoutera les entrées one button.
         */
        public void RegisterListener(IInputListener listener)
        {
            this._listeners.Add(listener);
        }

        /**
         * Permet de désenregistrer un listener.
         */
        public void UnregisterListener(IInputListener listener)
        {
            this._listeners.Remove(listener);
        }
    }
}