public class Player_Combat : Entity_Combat
{
    public bool CounterAttackPerform()
    {
        bool hasCounterAttack = false;

        foreach (var hit in AttackHits())
        {
            if (!hit.GetComponent<Enemy>().canCounterAttack || hit == null)
                hasCounterAttack = false;


            if (!hit.TryGetComponent<ICounterable>(out var counterable))
                continue;

            counterable.HandleCounter();
            hasCounterAttack = true;
        }

        return hasCounterAttack;
    }
}
