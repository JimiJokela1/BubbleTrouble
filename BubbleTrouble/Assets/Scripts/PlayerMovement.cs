using System;
using UnityEngine;
using UnityEngine.VFX;

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

    public int Health = 100;

    public VisualEffect slideBubblesVFX;

    public static PlayerMovement Instance { get; internal set; }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Instance = this;
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 20), "Health: " + Health);
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
        _dodgeVector = _movement;
        slideBubblesVFX.SendEvent("OnSlide");
        _dodging = true;
        rb.linearVelocity = _dodgeVector.normalized * DodgeDistance;
    }

    void FixedUpdate()
    {
        // Dodge movement
        if (_dodging)
        {
            _dodgeTimer += Time.fixedDeltaTime;

            if (_dodgeTimer >= DodgeTime)
            {
                _dodging = false;
                _dodgeTimer = 0f;
                // rb.linearVelocity = Vector3.zero;
                slideBubblesVFX.SendEvent("OnSlideEnd");
            }
            return;
        }

        if (_movement != null)
        {
            rb.AddForce(_movement);
        }
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;

        if (Health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
