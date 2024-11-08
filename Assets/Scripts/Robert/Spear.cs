using System.Collections;
using UnityEngine;


/// <summary>
/// 
/// </summary>
public class Spear : MonoBehaviour,IGrabbable
{
    public GameObject player;
    public GameObject enemy;
    public Transform spearSpawnPt;
    public GameObject spearPrefab;
    public Transform waterSurface;
    public float attackRange;
    public float attackCoolDown;
    public float spearSpeed = 100;
    private float duration = 200;
    private float speed = .02f;
    private bool canAttack = true;
    private bool isBeingGrabbed = false;
    private Rigidbody rb;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody>();
        transform.SetParent(null);
    }

    private void Update()
    {
        if(transform.position.y <= waterSurface.position.y)
        {
            Destroy(gameObject);
        }

        if (Vector3.Distance(transform.position,player.transform.position) <= attackRange && canAttack)
        {
            StartCoroutine(StartCoolDown());
            GameObject spear = Instantiate(spearPrefab, spearSpawnPt.position, spearSpawnPt.rotation);
            spear.GetComponent<Rigidbody>().velocity = spearSpawnPt.forward * spearSpeed;
            spear.GetComponent<Buoyancy>().waterSurface = waterSurface;
        }

        enemy.transform.LookAt(player.transform.position);
    }

    private void FixedUpdate()
    {
        if (isBeingGrabbed)
        {
            Vector3 dir = player.transform.position - transform.position;

            rb.constraints = 0;
            rb.AddForce(dir * 400f * Time.deltaTime);
        }
    }

    public void OnGetGrabbed()
    {
        isBeingGrabbed = true;
    }

    public void StopGrab()
    {
        isBeingGrabbed = false;
    }

    private IEnumerator StartCoolDown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCoolDown);
        canAttack = true;
    }
}
    