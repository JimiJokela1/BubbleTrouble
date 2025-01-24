using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W))
        {
            rb.AddForce(Vector3.forward * Time.fixedDeltaTime * moveSpeed);
        }

        if (Input.GetKey(KeyCode.S))
        {
            rb.AddForce( Vector3.back * Time.fixedDeltaTime * moveSpeed);
        }

        if (Input.GetKey(KeyCode.A))
        {
            rb.AddForce(Vector3.left * Time.fixedDeltaTime * moveSpeed);
        }

        if (Input.GetKey(KeyCode.D))
        {
            rb.AddForce(Vector3.right * Time.fixedDeltaTime * moveSpeed);
        }
    }
}
