using UnityEngine;

public class EnemyShot : MonoBehaviour
{
    public int Damage = 10;
    public int Radius = 10;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            PlayerMovement.Instance.TakeDamage(Damage);
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Floor") && collision.gameObject.GetComponent<CleaningTest>() is CleaningTest cleaning)
        {
            cleaning?.Dirtify(transform.position, collision.GetContact(0).point, Radius);
            Destroy(gameObject);
        }
    }
}
