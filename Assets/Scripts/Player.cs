using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float speed = 10.0f;

    public GameObject Sword;
    public GameObject SwordPos;
    public GameObject SwordPrefab;

    public GameObject BombPos;
    public GameObject BombPrefab;
    public GameObject SpawnedBomb;


    public GameObject Shield;

    public Rigidbody rb;

    public Camera cam;

    public int life = 6;
    public int maxLife = 6;

    public GameObject shieldCol;

    public float rotSpeed;

    private Animator anim;

    public bool sword;

    public GameObject TextCanvas;

    public GameObject[] HeartImages;

    public bool zaWarudo = false;

    public bool canUseZaWarudo = true;

    public GameObject ZaWarudoZone;

    public Image ClockImage;

    private void Start()
    {
        cam = Camera.main;
        anim = GetComponent<Animator>();
    }

    private void CameraRotation()
    {
        RaycastHit hit;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out hit))
        {
            //transform.LookAt(new Vector3(hit.point.x, transform.position.y, hit.point.z));
            var rotation = Quaternion.LookRotation(new Vector3(hit.point.x, transform.position.y, hit.point.z) - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotSpeed);
        }
    }
    IEnumerator StopAtk()
    {
        yield return new WaitForSeconds(0.1f);
        anim.SetBool("Attack", false);
    }

    private void ThrowSword()
    {
        anim.SetBool("Attack", true);
        Sword.SetActive(false);
        GameObject throwedSword = Instantiate(SwordPrefab, SwordPos.transform.position, SwordPos.transform.rotation);
        throwedSword.GetComponent<Sword>().Link = gameObject;
        StartCoroutine(StopAtk());
    }

    IEnumerator ZaWarudoCoolDown()
    {
        yield return new WaitForSeconds(30f);
        canUseZaWarudo = true;
        ClockImage.color = Color.white;
    }
    IEnumerator StopZaWarudo()
    {
        yield return new WaitForSeconds(3f);
        zaWarudo = false;
        StartCoroutine(ZaWarudoCoolDown());
    }

    private void Update()
    {
        CameraRotation();

        if (Input.GetKey(KeyCode.W))
        {
            rb.velocity = transform.forward * speed;
            anim.SetBool("Walking", true);
        }
        else
        {
            anim.SetBool("Walking", false);
            rb.velocity = Vector3.zero;
        }


        if (Input.GetMouseButtonDown(0) && Sword.activeSelf && !shieldCol.activeSelf && sword)
        {
            ThrowSword();
        }
        if(Input.GetMouseButton(1))
        {
            anim.SetBool("Shield", true);
            shieldCol.SetActive(true);
            speed = 0f;
            
        }
        else if((Input.GetMouseButtonUp(1)))
        {
            anim.SetBool("Shield", false);
            shieldCol.SetActive(false);
            speed = 5f;
        }

        if(Input.GetKeyDown(KeyCode.Q))
        {
            SpawnedBomb = Instantiate(BombPrefab, BombPos.transform.position, BombPos.transform.rotation, transform);
            Sword.SetActive(false);
            Shield.SetActive(false);
            speed = 2.5f;

        }
        if(SpawnedBomb != null && Input.GetKeyUp(KeyCode.Q))
        {
            SpawnedBomb.transform.parent = null;
            SpawnedBomb.GetComponent<Bomb>().ThrowBomb();
            Sword.SetActive(true);
            Shield.SetActive(true);
            speed = 5f;
        }

        if(Input.GetMouseButtonDown(2) && ZaWarudoZone.transform.localScale.x <= 0.1f && canUseZaWarudo)
        {
            canUseZaWarudo = false;
            zaWarudo = true;
            ClockImage.color = Color.black;
        }

 
        cam.transform.position = new Vector3(transform.position.x, cam.transform.position.y, transform.position.z - 2);
    }

    private void FixedUpdate()
    {
        if (life < maxLife)
            HeartImages[life].SetActive(false);

        if(life <= 0)
        {
            SceneManager.LoadScene("SampleScene");
        }

        if (zaWarudo && ZaWarudoZone.transform.localScale.x <= 14)
        {
            ZaWarudoZone.transform.localScale = new Vector3(ZaWarudoZone.transform.localScale.x + Time.deltaTime * 5, ZaWarudoZone.transform.localScale.y + Time.deltaTime * 5, ZaWarudoZone.transform.localScale.z + Time.deltaTime * 5);
            ZaWarudoZone.transform.parent = null;
        }
        else if(zaWarudo)
        {
            StartCoroutine(StopZaWarudo());
        }
        else if(!zaWarudo && ZaWarudoZone.transform.localScale.x >= 0.1)
        {
            ZaWarudoZone.transform.localScale = new Vector3(ZaWarudoZone.transform.localScale.x - Time.deltaTime * 3, ZaWarudoZone.transform.localScale.y - Time.deltaTime * 3, ZaWarudoZone.transform.localScale.z - Time.deltaTime * 3);
        }
        else if(ZaWarudoZone.transform.localScale.x <= 0.1)
        {
            ZaWarudoZone.transform.parent = gameObject.transform;
        }
        ZaWarudoZone.transform.position = gameObject.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("Sword") && !sword)
        {
            sword = true;
            Destroy(other.gameObject);
            Destroy(TextCanvas);
            Sword.SetActive(true);
        }
    }
}
