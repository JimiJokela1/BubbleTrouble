using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    public Vector3 ResetPos;

    public static PlayerMovement Instance { get; internal set; }

    public bool dead = false;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        
        FindObjectsByType<PlayerMovement>(sortMode: FindObjectsSortMode.None, findObjectsInactive: FindObjectsInactive.Include).ToList().ForEach((player) =>
        {
            if (player != this)
            {
                SceneManager.activeSceneChanged -= player.OnSceneLoaded;
                Destroy(player.gameObject);
            }
        });

        rb = GetComponent<Rigidbody>();
        Instance = this;
        SceneManager.activeSceneChanged += OnSceneLoaded;
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 20), "Health: " + Health);
    }

    void OnSceneLoaded(Scene scene, Scene scene2)
    {
        transform.position = ResetPos;
    }

    void Update()
    {
        if (dead)
        {
            _movement = Vector3.zero;
            return;
        }

        if (_dodging)
        {
            Ray ray = new Ray(transform.position, Vector3.down);
            if (Physics.Raycast(ray, out RaycastHit hitFloor, 100f))
            {
                if (hitFloor.collider.tag == "Floor")
                {
                    if (hitFloor.collider.GetComponent<CleaningTest>() is CleaningTest cleaning)
                    {
                        cleaning?.Clean(transform.position, hitFloor.point, 10);
                    }
                }
            }
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
        GetComponentsInChildren<MeshRenderer>().ToList().ForEach((mesh) => mesh.enabled = false);
        FindFirstObjectByType<GameOverUI>().ShowGameOver();
        dead = true;
    }
}
