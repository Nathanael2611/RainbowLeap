using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class debugtest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D overlapCircle = Physics2D.OverlapCircle(this.transform.position, 0.5f);
        Debug.Log(overlapCircle == null);
    }
}
