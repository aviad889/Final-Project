using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Animator anim;
    #region Bullet Shoot
    private GameObject bullet1;
    private GameObject bullet2;
    private GameObject bullet3;
    private float bulletSpeed = 10f;
    private float bulletLife = 1f;
    private Vector3 shootLeft = new(-1, 0, 0);
    private Quaternion quaternion = Quaternion.Euler(0, 180, 0);
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform spawnBullet1;
    [SerializeField] private Transform spawnBullet2;
    [SerializeField] private Transform spawnBullet3;
    private bool shoot;
    #endregion

    #region Enemy Health Parameters
    private float animDelay;
    private float animDuration = 0.5f;
    #endregion
    void Start()
    {
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        if (shoot)
        {
            bullet1 = Instantiate(bulletPrefab, spawnBullet1.position, quaternion);
            bullet2 = Instantiate(bulletPrefab, spawnBullet2.position, quaternion);
            bullet3 = Instantiate(bulletPrefab, spawnBullet3.position, quaternion);
            bullet1.GetComponent<Rigidbody2D>().velocity = shootLeft * bulletSpeed;
            bullet2.GetComponent<Rigidbody2D>().velocity = shootLeft * bulletSpeed;
            bullet3.GetComponent<Rigidbody2D>().velocity = shootLeft * bulletSpeed;
            Destroy(bullet1, bulletLife);
            Destroy(bullet2, bulletLife);
            Destroy(bullet3, bulletLife);
            shoot = false;
        }
        animDelay += Time.deltaTime;
        if (animDelay >= animDuration)
        {
            animDelay = 0f;
            anim.SetBool("isHit", false);
        }
    }
    public void IsShoot()
    {
        shoot = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerBullet"))
        {
            anim.SetBool("isHit", true);
        }        
    }
}
