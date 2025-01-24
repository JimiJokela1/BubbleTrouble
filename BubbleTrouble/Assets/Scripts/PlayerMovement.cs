using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    Rigidbody rb;
    Vector3 _movement;
    private bool _dodging;
    private float _dodgeTimer = 0f;
    private Vector3 _dodgeVector = Vector3.zero;

    public float DodgeDistance;
    public float DodgeTime;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    void Update()
    {
        if (_dodging)
        {
            return;
        }

        // Dodge control
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DodgeRoll();
        }

        // Move
        _movement = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        _movement *= moveSpeed;

        // Rotate
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        Ray mousePosInWorldRay = Camera.main.ScreenPointToRay(mousePos);
        if (Physics.Raycast(mousePosInWorldRay, out RaycastHit hit))
        {
            Vector3 lookAt = hit.point;
            lookAt.y = 1f;
            transform.LookAt(lookAt);
        }
    }

    void DodgeRoll()
    {
        _dodgeVector = transform.forward;
        _dodging = true;
    }

    void FixedUpdate()
    {
        // Dodge movement
        if (_dodging)
        {
            rb.linearVelocity = _dodgeVector.normalized * DodgeDistance * _dodgeTimer / DodgeTime;
            _dodgeTimer += Time.fixedDeltaTime;

            if (_dodgeTimer >= DodgeTime)
            {
                _dodging = false;
                _dodgeTimer = 0f;
                rb.linearVelocity = Vector3.zero;
            }
            return;
        }

        if (_movement != null)
        {
            rb.AddForce(_movement);
        }
    }
}
