using UnityEngine;

public class Object_Blacksmith : Object_NPC, IInteractable
{
    public void Interact()
    {
        Debug.Log("Interact Blacksmith");
    }

    protected override void Awake()
    {
        base.Awake();
        anim.SetBool("Blacksmith", true);
    }
}
