using UnityEngine;

public class Enemy2Controller : MonoBehaviour
{
    private Animator anim;

    #region Enemy Health Parameters
    private float animDelay;
    private float animDuration = 0.5f;
    #endregion

    #region Bullet Related Parameters
    private GameObject bullet;
    private float bulletSpeed = 10f;
    private float bulletLife = 1f;
    private Vector3 shootLeft = new(-1, 0, 0);
    private Quaternion quaternion = Quaternion.Euler(0, 180, 0);
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform spawnBullet;
    private bool shoot;
    #endregion
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        animDelay += Time.deltaTime;
        if (animDelay >= animDuration)
        {
            animDelay = 0f;
            anim.SetBool("isHit", false);
        }
        if (shoot)
        {
            bullet = Instantiate(bulletPrefab, spawnBullet.position, quaternion);
            bullet.GetComponent<Rigidbody2D>().velocity = shootLeft * bulletSpeed;
            Destroy(bullet, bulletLife);
            shoot = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerBullet"))
        {
            anim.SetBool("isHit", true);
        }
    }
    public void IsShootRifle()
    {
        shoot = true;
    }
}
