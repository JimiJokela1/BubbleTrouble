using UnityEngine;

public class EnemyBodyCollide : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        //GetComponentInParent<Enemy>().OnBodyCollide(collision);
    }
}
