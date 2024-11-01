using UnityEngine;

public class PlayerBulletShooter : MonoBehaviour
{
    #region Bullet Related Parameters
    private GameObject bullet;
    private GameObject bullet1;
    private GameObject bullet2;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform spawnBulletRight;
    [SerializeField] private Transform spawnBulletLeft;
    [SerializeField] private Transform[] spawnBulletsShotGun;
    private float bulletSpeed = 15f;
    private float bulletLife = 0.5f;
    #endregion
    private Vector3 shootRight, shootLeft;
    private SpriteRenderer spriteRenderer;
    private Quaternion leftQuaternion = Quaternion.Euler(0f, 180f, 0f);
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void ShootPistol()
    {
        if (!spriteRenderer.flipX)
        {
            shootRight = transform.right;
            bullet = Instantiate(bulletPrefab, spawnBulletRight.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().velocity = shootRight * bulletSpeed;
            Destroy(bullet, bulletLife);
        }
        if (spriteRenderer.flipX)
        {
            shootLeft = transform.right * -1;
            bullet = Instantiate(bulletPrefab, spawnBulletLeft.position, leftQuaternion);
            bullet.GetComponent<Rigidbody2D>().velocity = shootLeft * bulletSpeed;
            Destroy(bullet, bulletLife);
        }
    }
    public void ShotGunShooter()
    {
        if (!spriteRenderer.flipX)
        {
            shootRight = transform.right;
            bullet = Instantiate(bulletPrefab, spawnBulletsShotGun[0].position, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().velocity = shootRight * bulletSpeed;
            bullet1 = Instantiate(bulletPrefab, spawnBulletsShotGun[1].position, Quaternion.identity);
            bullet1.GetComponent<Rigidbody2D>().velocity = shootRight * bulletSpeed;
            bullet2 = Instantiate(bulletPrefab, spawnBulletsShotGun[2].position, Quaternion.identity);
            bullet2.GetComponent<Rigidbody2D>().velocity = shootRight * bulletSpeed;
            Destroy(bullet, bulletLife);
            Destroy(bullet1, bulletLife);
            Destroy(bullet2, bulletLife);
        }
        if (spriteRenderer.flipX)
        {
            shootLeft = transform.right * -1;
            bullet = Instantiate(bulletPrefab, spawnBulletsShotGun[3].position, leftQuaternion);
            bullet.GetComponent<Rigidbody2D>().velocity = shootLeft * bulletSpeed;
            bullet1 = Instantiate(bulletPrefab, spawnBulletsShotGun[4].position, leftQuaternion);
            bullet1.GetComponent<Rigidbody2D>().velocity = shootLeft * bulletSpeed;
            bullet2 = Instantiate(bulletPrefab, spawnBulletsShotGun[5].position, leftQuaternion);
            bullet2.GetComponent<Rigidbody2D>().velocity = shootLeft * bulletSpeed;
            Destroy(bullet, bulletLife);
            Destroy(bullet1, bulletLife);
            Destroy(bullet2, bulletLife);
        }
    }
}
