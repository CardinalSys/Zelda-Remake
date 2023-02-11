using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Octorok : MonoBehaviour
{
    public float distance;

    private GameObject Player;

    public NavMeshAgent agent;

    public GameObject RockPrefab;

    public GameObject SpawnedRock;

    private bool flag = false;

    public bool isStopped = false;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
    }

    IEnumerator Rock()
    {
        flag = true;
        yield return new WaitForSeconds(2.5f);
        if(!isStopped)
        {
            SpawnedRock = Instantiate(RockPrefab, transform.position + transform.forward, transform.rotation);
            SpawnedRock.GetComponent<Rock>().Owner = gameObject;
        }

        flag = false;
    }


    void Update()
    {
        if(!isStopped)
        {
            distance = Vector3.Distance(transform.position, Player.transform.position);

            if (distance <= 5)
            {
                //Shoot
                agent.isStopped = true;
                transform.LookAt(Player.transform);
                if (!flag)
                    StartCoroutine(Rock());

            }
            else if (distance > 5 && distance < 10)
            {
                //Move to player
                agent.isStopped = false;
                agent.SetDestination(Player.transform.position);
            }
            else if (distance >= 10 && !agent.hasPath)
            {
                agent.SetDestination(new Vector3(transform.position.x + Random.Range(-5, 5), transform.position.y, transform.position.z + Random.Range(-5, 5)));
            }
        }

    }

}
