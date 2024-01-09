using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{

    public Player playerToFollow;
    
    void Start()
    {
        if (this.playerToFollow)
        {
            this.transform.SetParent(this.playerToFollow.transform);
        }
    }

}
