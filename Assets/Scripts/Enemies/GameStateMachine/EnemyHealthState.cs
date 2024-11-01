using UnityEngine;

public class EnemyHealthState : EnemyBaseState
{
    public EnemyHealthState(EnemyStateMachine currentContext, EnemyFactoryState enemyFactoryState)
    : base(currentContext, enemyFactoryState) { }
    public override void EnterState() 
    {
        UpdateHealth();
    }
    public override void UpdateState()
    {
        CheckSwitchStates();
    }
    public override void ExitState() { }
    public override void CheckSwitchStates()
    {
        SwitchState(_factory.Health());
    }
    
    void UpdateHealth()
    {
        if (_ctx.CurrHealth <= 0)
        {
            Object.Destroy(_ctx.gameObject, 0.25f);
            _ctx.DeathEffect = Object.Instantiate(_ctx.DeathEffectPrefab, _ctx.DeathEffectPos, Quaternion.identity);
            Object.Destroy(_ctx.DeathEffect, 0.25f);
        }
        _ctx.Fraction = _ctx.CurrHealth / _ctx.MaxHealth;
        _ctx.HealthBar.fillAmount = _ctx.Fraction;
    }
}
