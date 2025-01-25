using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject ShotPrefab;
    public Transform ShotSpawnPoint;
    public float ShotSpeed = 10f;
    public float ShotChance = 5f;

    void Update()
    {
        if (Random.Range(0, 100) < ShotChance)
        {
            ShootBubbleShot();
        }

        transform.LookAt(PlayerMovement.Instance.transform.position);

        // Move towards player
        Vector3 direction = (PlayerMovement.Instance.transform.position - transform.position).normalized;
        direction.y = 0;
        transform.position += direction * Time.deltaTime;
    }

    void ShootBubbleShot()
    {
        GameObject bubbleShot = Instantiate(ShotPrefab, ShotSpawnPoint.position, ShotSpawnPoint.rotation);
        Rigidbody rb = bubbleShot.GetComponent<Rigidbody>();
        // Get random vector inside a 10 degree cone
        Vector3 direction = (PlayerMovement.Instance.transform.position - transform.position).normalized;
        direction = Quaternion.Euler(Random.Range(-10, 10), Random.Range(-10, 10), 0) * direction;

        rb.linearVelocity = direction * ShotSpeed;
    }

    public void TakeDamage(int damage)
    {
        // Destroy self
        Destroy(gameObject);
    }
}
