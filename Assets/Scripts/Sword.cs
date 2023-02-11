using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody rb;
    public float force;

    public GameObject Link;

    private bool isReturning = false;


    private float time;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(-transform.up * force);
    }


    IEnumerator TimeToReturn()
    {
        yield return new WaitForSeconds(2f);
        isReturning = true;

    }

    private void Update()
    {
        float distance = Vector3.Distance(Link.transform.position, transform.position);

        if (rb.velocity.x < 0.1f && !isReturning && distance > 1)
        {
            StartCoroutine(TimeToReturn());

        }

        if (isReturning)
        {
            time += Time.deltaTime;
            Vector3 direction = new Vector3(Link.transform.position.x, Link.transform.position.y + 0.7f, Link.transform.position.z) - transform.position;
            rb.velocity =  direction * (1.2f + time);
        }


        if(distance < 1.2f && isReturning)
        {
            Link.GetComponent<Player>().Sword.SetActive(true);
            Destroy(gameObject);
        }
    }



    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
            isReturning = true;
        }
    }

}
