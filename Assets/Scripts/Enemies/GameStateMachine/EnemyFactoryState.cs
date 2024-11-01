public class EnemyFactoryState
{
    EnemyStateMachine _context;

    public EnemyFactoryState(EnemyStateMachine currentContext)
    {
        _context = currentContext;
    }

    public EnemyBaseState Health()
    {
        return new EnemyHealthState(_context, this);
    }
}
