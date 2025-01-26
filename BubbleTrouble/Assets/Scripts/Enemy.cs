using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject ShotPrefab;
    public Transform ShotSpawnPoint;
    public float ShotSpeed = 10f;
    public float ShotChance = 5f;
    public float ShotCooldown = 1.5f;
    private float _shotTimer = 0f;

    public AIType AI = AIType.Shooter;

    public float RandomDasherMovementInterval = 1f;
    public float RandomDasherMovementDistance = 3f;
    private Vector3 _randomDasherMovement;
    private float _randomDasherTimer = 0f;

    public float FollowPlayerStartSpeed = 1f;
    private float _followPlayerSpeed = 0f;
    public float FollowPlayerAcceleration = 1f;
    public int TouchDamage = 10;
    
    public float EmergeHeight = 1f;

    private Rigidbody rb;

    public AudioClip shotSound;
    public AudioClip dieSound;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        _followPlayerSpeed = FollowPlayerStartSpeed;
    }

    public enum AIType
    {
        Shooter,
        RandomDasher,
        FollowPlayer
    }

    private void Stun()
    {
        _followPlayerSpeed = FollowPlayerStartSpeed;
    }

    void FixedUpdate()
    {
        transform.LookAt(PlayerMovement.Instance.transform.position);

        if (AI == AIType.Shooter)
        {
            _shotTimer += Time.fixedDeltaTime;
            if (_shotTimer > ShotCooldown)
            {
                _shotTimer = 0;
                if (Random.Range(0, 100) < ShotChance)
                {
                    ShootBubbleShot();
                }
            }
        }
        else if (AI == AIType.RandomDasher)
        {
            // Move in random direction
            _randomDasherTimer += Time.fixedDeltaTime;
            if (_randomDasherTimer > RandomDasherMovementInterval)
            {
                _randomDasherTimer = 0;
                _randomDasherMovement = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
                rb.MovePosition(rb.position + _randomDasherMovement * RandomDasherMovementDistance);
            }

            if (Random.Range(0, 100) < ShotChance)
            {
                ShootBubbleShot();
            }
        }
        else if (AI == AIType.FollowPlayer)
        {
            // Move towards player
            Vector3 direction = (PlayerMovement.Instance.transform.position - transform.position).normalized;
            direction.y = 0;
            direction *= _followPlayerSpeed;
            _followPlayerSpeed += FollowPlayerAcceleration * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + direction * Time.fixedDeltaTime);
            
            if (Random.Range(0, 100) < ShotChance)
            {
                ShootBubbleShot();
            }
        }
    }

    void ShootBubbleShot()
    {
        GameObject bubbleShot = Instantiate(ShotPrefab, ShotSpawnPoint.position, ShotSpawnPoint.rotation);
        Rigidbody rb = bubbleShot.GetComponent<Rigidbody>();
        // Get random vector inside a 10 degree cone
        Vector3 direction = (PlayerMovement.Instance.transform.position - transform.position).normalized;
        direction = Quaternion.Euler(0, Random.Range(-10, 10), 0) * direction;

        rb.linearVelocity = direction * ShotSpeed;

        AudioSource.PlayClipAtPoint(shotSound, transform.position);
    }

    public void TakeDamage(int damage)
    {
        AudioSource.PlayClipAtPoint(dieSound, transform.position);
        // Destroy self
        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerMovement.Instance.TakeDamage(TouchDamage);
            Stun();
        }
    }

    public void StartEmerge()
    {
        StartCoroutine(Emerge());
    }

    IEnumerator Emerge()
    {
        rb.isKinematic = true;

        while (transform.position.y < EmergeHeight)
        {
            transform.position += Vector3.up * 2 * Time.deltaTime;
            yield return null;
        }

        rb.isKinematic = false;
    }
}
