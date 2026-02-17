public class Player_AnimationEvent : Entity_AnimationEvent
{
    Player player;

    protected override void Awake()
    {
        base.Awake();
        player = GetComponentInParent<Player>();
    }
}
