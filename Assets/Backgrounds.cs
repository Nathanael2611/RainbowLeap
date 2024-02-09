using Entity.Player;
using UnityEngine;

public class Backgrounds : MonoBehaviour
{

    public GameObject tileBase;

    private GameObject centerTile;
    
    void Start()
    {
        this.centerTile = Instantiate(this.tileBase);
        this.centerTile.transform.SetParent(this.transform);
        
    }

    // Update is called once per frame
    void Update()
    {
        Frog thePlayer = Frog.TheFrog();
        if(!thePlayer)
            return;
        Vector2 position = thePlayer.GetRigidbody().position;
        float tileX = Mathf.Floor((position.x + 25) / 50) * 50;
        float tileY = Mathf.Floor((position.y + 25) / 50) * 50;
        this.centerTile.transform.position = new Vector3(tileX, tileY, 200);
    }
}
