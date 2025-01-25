using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject ShotPrefab;
    public Transform ShotSpawnPoint;
    public float ShotSpeed = 10f;
    public float ShotChance = 5f;

    public AIType AI = AIType.Shooter;

    public float RandomDasherMovementInterval = 1f;
    public float RandomDasherMovementDistance = 3f;
    private Vector3 _randomDasherMovement;
    private float _randomDasherTimer = 0f;

    public enum AIType
    {
        Shooter,
        RandomDasher,
        FollowPlayer
    }

    void Update()
    {
        transform.LookAt(PlayerMovement.Instance.transform.position);

        if (AI == AIType.Shooter)
        {
            if (Random.Range(0, 100) < ShotChance)
            {
                ShootBubbleShot();
            }
        }
        else if (AI == AIType.RandomDasher)
        {
            // Move in random direction
            _randomDasherTimer += Time.deltaTime;
            if (_randomDasherTimer > RandomDasherMovementInterval)
            {
                _randomDasherTimer = 0;
                _randomDasherMovement = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
                transform.position += _randomDasherMovement * RandomDasherMovementDistance;
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
            transform.position += direction * Time.deltaTime;
            
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
    }

    public void TakeDamage(int damage)
    {
        // Destroy self
        Destroy(gameObject);
    }
}
