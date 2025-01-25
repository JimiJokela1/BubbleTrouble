using UnityEngine;

public class BubbleShot : MonoBehaviour
{
    public int CleanRadius = 10;
    public int Damage = 10;
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponentInParent<Enemy>().TakeDamage(Damage);
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Floor") && collision.gameObject.GetComponent<CleaningTest>() is CleaningTest cleaning)
        {
            cleaning?.Clean(transform.position, collision.GetContact(0).point, CleanRadius);
            Destroy(gameObject);
        }
    }
}
