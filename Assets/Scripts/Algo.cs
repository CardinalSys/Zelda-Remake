using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Algo : MonoBehaviour
{
    public GameObject Player;

    private NavMeshAgent agent;

    private bool canMove = true;

    float distance;

    public bool isStopped;
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isStopped)
        {
            distance = Vector3.Distance(transform.position, Player.transform.position);

            if (canMove)
            {
                if (distance < 5)
                {
                    agent.SetDestination(Player.transform.position);
                }
                else if (!agent.hasPath)
                {
                    agent.SetDestination(new Vector3(transform.position.x + Random.Range(-5, 5), transform.position.y, transform.position.z + Random.Range(-5, 5)));
                }
            }
        }


    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(2f);
        canMove = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && canMove)
        {
            canMove = false;
            collision.gameObject.GetComponent<Player>().life--;  
            StartCoroutine(Delay());
        }

    }
}
