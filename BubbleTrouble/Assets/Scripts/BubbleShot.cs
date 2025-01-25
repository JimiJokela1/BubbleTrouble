using System.Collections;
using UnityEngine;

public class BubbleShot : MonoBehaviour
{
    public int CleanRadius = 10;
    public int Damage = 10;
    private Material bubbleMat;

    private void Start()
    {
        bubbleMat = GetComponent<MeshRenderer>().material;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponentInParent<Enemy>().TakeDamage(Damage);
            StartCoroutine(FadeOut());
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            StartCoroutine(FadeOut());
        }

        if (collision.gameObject.CompareTag("Floor"))
        {
            if (collision.gameObject.GetComponent<CleaningTest>() is CleaningTest cleaning)
            {
                cleaning?.Clean(transform.position, collision.GetContact(0).point, CleanRadius);
            }

            StartCoroutine(FadeOut());
        }
    }

    IEnumerator FadeOut()
    {
        GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        GetComponent<Collider>().enabled = false;
        float fade = 0;
        while (fade < 1)
        {
            fade += Time.deltaTime * 5;
            bubbleMat.SetFloat("_Dissolve", fade);
            yield return null;
        }

        Destroy(gameObject);
    }
}