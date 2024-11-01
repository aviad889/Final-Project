using UnityEngine;
using UnityEngine.UI;

public class EnemyStateMachine : MonoBehaviour
{
    EnemyBaseState _currentState;
    EnemyFactoryState _states;

    [SerializeField] private Image _healthBar;
    [SerializeField] private GameObject _deathEffectPrefab;
    private Vector3 _deathEffectPos;
    private GameObject _deathEffect;
    private float _currHealth;
    private float _fraction;
    private float _damage = 1f;
    private float _maxHealth;// = 3f;

    public Image HealthBar {  get { return _healthBar; } set { _healthBar = value; } }
    public GameObject DeathEffectPrefab { get { return _deathEffectPrefab; } set { _deathEffectPrefab = value; } }
    public Vector3 DeathEffectPos { get { return _deathEffectPos; } set { _deathEffectPos = value; } }
    public GameObject DeathEffect { get { return _deathEffect; } set { _deathEffect = value; } }
    public float CurrHealth {  get { return _currHealth; } set { _currHealth = value; } }
    public float Fraction { get { return _fraction; } set { _fraction = value; } }
    public float MaxHealth { get { return _maxHealth; } set { _maxHealth = value; } }
    public EnemyBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }

    void Awake()
    {
        _states = new EnemyFactoryState(this);
        if (gameObject.CompareTag("EnemyController"))
            _maxHealth = 3f;
        if (gameObject.CompareTag("Enemy"))
            _maxHealth = 9f;
        _currHealth = _maxHealth;
        _currentState = _states.Health();
        _currentState.EnterState();
    }
    void Update()
    {
        _deathEffectPos = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);
        CurrentState.UpdateState();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerBullet"))
        {
            _currHealth -= _damage;
        }
    }
}
