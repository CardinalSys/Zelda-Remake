using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZaWarudo : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.name.Contains("Algo"))
        {
            other.gameObject.GetComponent<Algo>().isStopped = true;
        }
        else if (other.name.Contains("ElPulpo"))
        {
            other.gameObject.GetComponent<Octorok>().isStopped = true;
            Debug.Log("1");
        }
        else if (other.name.Contains("Rock"))
        {
            other.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name.Contains("Algo"))
        {
            other.gameObject.GetComponent<Algo>().isStopped = false;
        }
        else if (other.name.Contains("Pulpo"))
        {
            other.gameObject.GetComponent<Octorok>().isStopped = false;
        }
        else if (other.name.Contains("Rock"))
        {
            other.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        }
    }
}
