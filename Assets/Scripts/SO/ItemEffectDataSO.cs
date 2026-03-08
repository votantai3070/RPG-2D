using UnityEngine;

public class ItemEffectDataSO : ScriptableObject
{
    [TextArea]
    public string effectionDiscription;

    public virtual bool CanBeUsed()
    {
        return true;
    }

    public virtual void ExecuteEffect()
    {

    }
}
