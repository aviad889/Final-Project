public abstract class EnemyBaseState
{
    protected EnemyStateMachine _ctx;
    protected EnemyFactoryState _factory;

    public EnemyBaseState(EnemyStateMachine currentContext, EnemyFactoryState enemyStateFactory)
    {
        _ctx = currentContext;
        _factory = enemyStateFactory;
    }

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
    public abstract void CheckSwitchStates();
    protected void SwitchState(EnemyBaseState newState)
    {
        ExitState();
        newState.EnterState();
        _ctx.CurrentState = newState;
    }
}
