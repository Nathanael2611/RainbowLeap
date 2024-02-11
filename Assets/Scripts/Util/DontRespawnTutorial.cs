using System;
using UnityEngine;

namespace Util
{
    public class DontRespawnTutorial : MonoBehaviour
    {
        private void Start()
        {
            GameObject.DontDestroyOnLoad(this.gameObject);
        }

        public static GameObject MarkTutorialFinished()
        {
            GameObject gameObject = new GameObject();
            gameObject.AddComponent<DontRespawnTutorial>();

            return gameObject;
        }
        
        public static bool ShouldSpawnTutorial()
        {
            GameObject[] objectsOfType = GameObject.FindObjectsOfType<GameObject>();
            foreach (GameObject o in objectsOfType)
            {
                if (o.GetComponent<DontRespawnTutorial>())
                    return false;
            }

            return true;
        }
        
    }
}