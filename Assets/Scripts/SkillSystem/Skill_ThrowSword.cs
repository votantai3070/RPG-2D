using UnityEngine;

public class Skill_ThrowSword : Skill_Base
{
    private SkillObject_Sword currentSword;

    [Header("Regular Sword upgrade")]
    [SerializeField] private GameObject swordPrefab;
    [Range(0, 10)]
    [SerializeField] private float throwPower = 5;

    [Header("Pierce Sword upgrade")]
    [SerializeField] private GameObject pierceSwordPrefab;
    public int pierceAmount = 2;

    [Header("Spin Sword upgrade")]
    [SerializeField] private GameObject spinSwordPrefab;
    public int maxDistance = 5;
    public float attackPerSecond = 6;
    public float maxSpinDuration = 2;

    [Header("Trajectory prediction")]
    [SerializeField] private GameObject predictionDot;
    [SerializeField] private int numberOfDots = 20;
    [SerializeField] private float spaceBetweeDots = .05f;
    private float swordGravity;
    private Transform[] dots;
    private Vector2 confirmedDirection;

    protected override void Awake()
    {
        base.Awake();

        swordGravity = swordPrefab.GetComponent<Rigidbody2D>().gravityScale;
        dots = GenerateDots();
    }

    public override bool CanBeUsedSkill()
    {
        if (currentSword != null)
        {
            currentSword.GetSwordBackToPlayer();
            return false;
        }

        return base.CanBeUsedSkill();
    }

    public void ThrowSword()
    {
        GameObject newSword = Instantiate(GetSwordPrefab(), dots[1].position, Quaternion.identity);

        currentSword = newSword.GetComponent<SkillObject_Sword>();
        currentSword.SetupSword(this, GetThrowPower());
    }

    private GameObject GetSwordPrefab()
    {
        if (Unlocked(SkillUpgradeType.SwordThrow))
            return swordPrefab;

        if (Unlocked(SkillUpgradeType.SwordThrow_Pierce))
            return pierceSwordPrefab;

        if (Unlocked(SkillUpgradeType.SwordThrow_Spin))
            return spinSwordPrefab;

        Debug.Log("No valid sword upgrade selected!");
        return null;
    }

    private Vector2 GetThrowPower() => confirmedDirection * (throwPower * 10);

    public void PredictTrajection(Vector2 direction)
    {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].position = GetTrajectoryPoint(direction, i * spaceBetweeDots);
        }
    }

    private Vector2 GetTrajectoryPoint(Vector2 direction, float t)
    {
        float scaledThrowPower = throwPower * 10;

        // This gives us the inital velocity - the starting speed and direction of the throw
        Vector2 initialVelocity = direction * scaledThrowPower;

        // Gravity pulls the sword down over time. The long it's in a air, the more it drops.
        Vector2 gravityEffect = .5f * Physics2D.gravity * swordGravity * (t * t);

        // We calculate how far the sword will travel after time 't'
        // by combining the initial throw direction with the gravity pull
        Vector2 predictedPoint = (initialVelocity * t) + gravityEffect;

        Vector2 playerPos = player.transform.position;

        return playerPos + predictedPoint;

    }

    public void ConfirmTrajection(Vector2 direction) => confirmedDirection = direction;

    public void EnableDots(bool enable)
    {
        foreach (var dot in dots)
            dot.gameObject.SetActive(enable);
    }

    private Transform[] GenerateDots()
    {
        Transform[] dots = new Transform[numberOfDots];

        for (int i = 0; i < dots.Length; i++)
        {
            dots[i] = Instantiate(predictionDot, transform.position, Quaternion.identity).transform;
            dots[i].gameObject.SetActive(false);
        }

        return dots;
    }
}
