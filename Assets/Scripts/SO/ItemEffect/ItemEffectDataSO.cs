using UnityEngine;

public class ItemEffectDataSO : ScriptableObject
{
    [TextArea]
    public string effectionDiscription;
    protected Player player;

    public virtual bool CanBeUsed()
    {
        return true;
    }

    public virtual void ExecuteEffect()
    {

    }

    public virtual void Subcribe(Player player)
    {
        this.player = player;
    }

    public virtual void Unsubscribe()
    {

    }
}
