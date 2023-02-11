using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody rb;
    public float force;

    public GameObject Owner;

    private bool flag = false;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * force);
    }

    private void Update()
    {
        if (rb.velocity.x < 0.1f && !flag)
        {
            flag = true;
            StartCoroutine(TimeToDestroy());
        }
    }

    IEnumerator TimeToDestroy()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().life--;
            Destroy(gameObject);
        }
        else if(collision.gameObject.CompareTag("Shield"))
        {
            Destroy(gameObject);
        }
    }
}
