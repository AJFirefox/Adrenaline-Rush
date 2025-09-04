using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class PlayerCollosion : MonoBehaviour
{

    public PlayerMovement movement;
    public GameManager gameManager;

     void OnCollisionEnter(Collision collisionInfo)
    {
        if (collisionInfo.collider.tag == "Obstacle")
        {
            movement.enabled = false;
            Object.FindFirstObjectByType<GameManager>().EndGame();
            
        }
    }

}
