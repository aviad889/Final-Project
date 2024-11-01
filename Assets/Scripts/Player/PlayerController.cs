using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    #region Player's Movement Parameters
    private float horizontalInput, verticalInput;
    private Vector3 movementDirection;
    private float speed = 6f;
    private float rollSpeed = 7.5f;
    private float boostedSpeed = 4f;
    private bool isBoosted = false;
    private bool isRolling;
    private bool isDuck;
    #endregion

    #region Jump Related Parameters
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    private float jumpSpeed = 12f;
    private float jumpSpeedBoosted = 10f;
    #endregion

    #region Player's Physics Parameters
    private Animator anim;
    private Rigidbody2D playerRb;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCol;
    private CircleCollider2D circleCol;
    #endregion

    #region Weapons Related Parameters
    private int weaponIndex = 0;
    private int spawnIndex = 0;
    private GameObject[] weapons = new GameObject[2];
    [SerializeField] private GameObject[] weaponsPrefabs = new GameObject[2];
    private Quaternion leftSideQuaternion = Quaternion.Euler(0f, 180f, 0f);
    [SerializeField] private Transform[] spawnWeapons = new Transform[4];
    #endregion

    #region Player's Health Parameters
    [SerializeField] private Image healthBar;
    private GameObject maxHealthTrigger;
    [SerializeField] private GameObject maxHealthTriggerPrefab;
    private float currHealth;
    private float fraction;
    private float damage = 1f;
    private float maxHealth = 10f;
    private bool isMaxCurrentHealth;
    private bool takeDamage;
    private float animHitDelay;
    private float animHitDuration = 0.5f;
    public UnityEvent respawnPlayerEvent;
    public UnityEvent shootPistol;
    public UnityEvent shootShotgun;
    #endregion

    void Start()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCol = GetComponent<BoxCollider2D>();
        circleCol = GetComponent<CircleCollider2D>();
        playerRb = GetComponent<Rigidbody2D>();
        weapons[weaponIndex] = Instantiate(weaponsPrefabs[weaponIndex], spawnWeapons[spawnIndex].position, Quaternion.identity);
        weapons[weaponIndex].transform.parent = spawnWeapons[spawnIndex].transform;
        respawnPlayerEvent.AddListener(GameObject.FindGameObjectWithTag("PlayerSpawner").GetComponent<PlayerSpawner>().RespawnPlayer);
        shootPistol.AddListener(gameObject.GetComponent<PlayerBulletShooter>().ShootPistol);
        shootShotgun.AddListener(gameObject.GetComponent<PlayerBulletShooter>().ShotGunShooter);
        maxHealthTrigger = Instantiate(maxHealthTriggerPrefab, transform.position, Quaternion.identity);
        currHealth = maxHealth;
    }
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        movementDirection = new(horizontalInput, 0, 0);
        WalkAndLookLeftAndRight();
        JumpAndLand();
        if (weapons[weaponIndex] != null)
            ChangeWeaponSpawnPosition();
        if (weapons[0] != null) 
            RollPistolAnimation();
        if (!isRolling && !isDuck && Input.GetKeyDown(KeyCode.Space))
        {
            if (weaponIndex == 0) shootPistol.Invoke();
            if (weaponIndex == 1) shootShotgun.Invoke();
        }
        HideWeaponWhileRolling();
        ShowWeaponWhileNotRollingOrDuck();
        DisableHealthTrigger();
        HitAnimationDelay();
        Duck();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("EnemyController"))
        {
            anim.SetBool("isHit", true);
            takeDamage = true;
            UpdateHealth();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyBullet"))
        {
            takeDamage = true;
            anim.SetBool("isHit", true);
            UpdateHealth();
        }
        if (collision.gameObject.CompareTag("MaxHealth"))
        {
            isMaxCurrentHealth = true;
            takeDamage = false;
            UpdateHealth();
        }
        if (collision.gameObject.CompareTag("Booster"))
            isBoosted = true;

        if (collision.gameObject.CompareTag("SwitchWeapon"))
        {
            weaponIndex = 1;
            spawnIndex = 2;
            DestroyPreviousWeapon();
        }
        if (collision.gameObject.CompareTag("Water"))
        {
            maxHealthTrigger.SetActive(true);
            respawnPlayerEvent.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Booster"))
            isBoosted = false;
    }
    private void WalkAndLookLeftAndRight()
    {
        if (horizontalInput != 0 && !isDuck)
        {
            transform.Translate(speed * Time.deltaTime * movementDirection);
            anim.SetBool("isRun", true);
        }
        else
        {
            anim.SetBool("isRun", false);
        }
        if (horizontalInput > 0)
        {
            spriteRenderer.flipX = false;

        }
        if (horizontalInput < 0)
        {
            spriteRenderer.flipX = true;
        }
    }
    private bool IsGrounded()
    {
        return Physics2D.OverlapCapsule(groundCheck.position, new Vector2(1.3f, 0.4f), CapsuleDirection2D.Horizontal, 0, groundLayer);
    }
    private void JumpAndLand()
    {
        if (verticalInput > 0 && IsGrounded())
        {
            playerRb.velocity = new Vector2(playerRb.velocity.x, jumpSpeed);
        }
        if (!IsGrounded() || verticalInput > 0)
        {
            ChangeColliderWhileRollOrJump();
            anim.SetBool("isRoll", true);
        }
        if (IsGrounded())
        {
            ChangeColliderToDefault();
            anim.SetBool("isRoll", false);
        }
        if (verticalInput > 0 && isBoosted)
            playerRb.velocity = new Vector2(boostedSpeed, jumpSpeedBoosted);
    }
    private void ChangeWeaponSpawnPosition()
    {
        if (spawnWeapons[spawnIndex].childCount == 1 && spriteRenderer.flipX)
        {
            Destroy(weapons[weaponIndex]);
            weapons[weaponIndex] = Instantiate(weaponsPrefabs[weaponIndex], spawnWeapons[spawnIndex + 1].position, leftSideQuaternion);
            weapons[weaponIndex].transform.parent = spawnWeapons[spawnIndex + 1];
        }
        if (spawnWeapons[spawnIndex + 1].childCount == 1 && !spriteRenderer.flipX)
        {
            Destroy(weapons[weaponIndex]);
            weapons[weaponIndex] = Instantiate(weaponsPrefabs[weaponIndex], spawnWeapons[spawnIndex].position, Quaternion.identity);
            weapons[weaponIndex].transform.parent = spawnWeapons[spawnIndex];
        }
    }
    private void HideWeaponWhileRolling()
    {
        if (verticalInput < 0 && (horizontalInput > 0.5f || horizontalInput < -0.5f) && IsGrounded())
        {
            Destroy(weapons[weaponIndex]);
            anim.SetBool("isRoll", true);
            ChangeColliderWhileRollOrJump();
            transform.Translate(rollSpeed * Time.deltaTime * movementDirection);
        }
        if (verticalInput < 0) isRolling = true;
        else isRolling = false;
    }
    private void ShowWeaponWhileNotRollingOrDuck()
    {
        if (verticalInput == 0)
        {
            anim.SetBool("isRoll", false);
            ChangeColliderToDefault();
        }
        if (verticalInput >= 0 && weapons[weaponIndex] == null)
        {
            if (!spriteRenderer.flipX)
            {
                weapons[weaponIndex] = Instantiate(weaponsPrefabs[weaponIndex], spawnWeapons[spawnIndex].position, Quaternion.identity);
                weapons[weaponIndex].transform.parent = spawnWeapons[spawnIndex];
            }
            if (spriteRenderer.flipX)
            {
                weapons[weaponIndex] = Instantiate(weaponsPrefabs[weaponIndex], spawnWeapons[spawnIndex+1].position, leftSideQuaternion);
                weapons[weaponIndex].transform.parent = spawnWeapons[spawnIndex+1];
            }
        }
    }
    private void ChangeColliderWhileRollOrJump()
    {
        boxCol.enabled = false;
        circleCol.enabled = true;
    }
    private void ChangeColliderToDefault()
    {
        boxCol.enabled = true;
        circleCol.enabled = false;
    }
    private void RollPistolAnimation()
    {
        if (Input.GetKey(KeyCode.V))
        {
            weapons[0].GetComponent<Animator>().SetBool("isRollPistol", true);
        }
        if (Input.GetKeyUp(KeyCode.V))
        {
            weapons[0].GetComponent<Animator>().SetBool("isRollPistol", false);
        }
    }
    private void DestroyPreviousWeapon()
    {
        if (weapons[0] != null)
        {
            Destroy(weapons[0]);
        }
    }
    private void Duck()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            ChangeColliderWhileRollOrJump();
            Destroy(weapons[weaponIndex]);
            isDuck = true;
            anim.SetBool("isDuck", true);
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            ChangeColliderToDefault();
            isDuck = false;
            anim.SetBool("isDuck", false);
        }
    }
    private void UpdateHealth()
    {
        if (isMaxCurrentHealth)
        {
            currHealth = maxHealth;
            healthBar.fillAmount = currHealth;
        }
        if (takeDamage)
            currHealth -= damage;
        if (currHealth <= 0)
        {
            maxHealthTrigger.SetActive(true);
            currHealth = maxHealth;
            respawnPlayerEvent.Invoke();
        }
        fraction = currHealth / maxHealth;
        healthBar.fillAmount = fraction;
    }
    private void DisableHealthTrigger()
    {
        if (transform.position.x >= -6f)
        {
            maxHealthTrigger.SetActive(false);
            isMaxCurrentHealth = false;
        }
    }
    private void HitAnimationDelay()
    {
        animHitDelay += Time.deltaTime;
        if (animHitDelay >= animHitDuration)
        {
            animHitDelay = 0f;
            anim.SetBool("isHit", false);
        }
    }
}
