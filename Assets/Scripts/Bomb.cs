using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    Rigidbody rb;

    public float force;

    public GameObject BombEffectPrefab;

    private float time;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {

    }

    IEnumerator TimeToExplode()
    {
        yield return new WaitForSeconds(5f);
        Instantiate(BombEffectPrefab, transform.position, transform.rotation);
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 3);
        foreach (var hitCollider in hitColliders)
        {
            if(hitCollider.CompareTag("Player"))
            {
                hitCollider.GetComponent<Player>().life--;
            }
            else if(hitCollider.name.Contains("Enemy"))
            {
                Destroy(hitCollider.gameObject);
            }
        }
        Destroy(gameObject);
    }

    public void ThrowBomb()
    {
        rb.constraints = RigidbodyConstraints.None;
        rb.AddForce((transform.forward * force) + (transform.up * force / 2));
        StartCoroutine(TimeToExplode());
    }
}
