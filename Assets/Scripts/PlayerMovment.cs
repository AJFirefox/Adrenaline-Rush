using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // This is a reference to the Rigodbody component called "rb"
    public Rigidbody rb;

    public float forwardForce = 2000f;
    public float sidewaysForce = 500f;
    public float backwardForce = -200f;


    // We marked this as "Fixed"Update because 
    // we are using it to mess with physics
    void FixedUpdate()
    {
        if (Input.GetKey("w"))
        {
            rb.AddForce(0, 0, forwardForce * Time.deltaTime);
        } 

        if ( Input.GetKey("d") )
        {
            rb.AddForce(sidewaysForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
        }
        
        if ( Input.GetKey("a") )
        {
            rb.AddForce(-sidewaysForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
        }

        if (Input.GetKey("s"))
        {
            rb.AddForce(0, 0, backwardForce * Time.deltaTime);
        }
    }
}
  