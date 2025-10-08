using Unity.Mathematics;
using UnityEngine;

public class Skill_SwordThrow : Skill_Base
{
    private SkillObject_Sword currentSword;

    [Header("Regular sword Upgrade")]
    [Range(0,10)]
    [SerializeField] private float throwPower = 6;
    [SerializeField] private GameObject swordPrefab;

    [Header("Peirce Sword Upgrade")]
    [SerializeField] private GameObject pierceSwordPrefab;
    public int amountToPierce = 2;


    [Header("Spin sword upgrade")]
    [SerializeField] private GameObject spinSwordPrefab;
    public int maxDistance = 5;
    public float attackPersecond = 6;
    public float maxSpinDuration = 3;

    
    [Header("Trajectory prediction")]
    [SerializeField] private GameObject perdictionDot;
    [SerializeField] private int numberOfDots = 20;
    [SerializeField] private float spaceBetweenDots = 0.05f;
    private float swordGravity = 3.5f;
    private Transform[] dots;
    private Vector2 confirmedDirection;

    protected override void Awake()
    {
        base.Awake();
        swordGravity = swordPrefab.GetComponent<Rigidbody2D>().gravityScale;
        dots = GenerateDots();
    }

    public override bool CanUseSkill()
    {


        if (currentSword != null)
        {
            currentSword.GetSwordBackToPlayer();
            return false;
        }

        return base.CanUseSkill();
    }

    public void ThrowSword()
    {
        GameObject swordPrefab = GetSwordPrefab();
        GameObject newSword = Instantiate(swordPrefab, dots[1].position, quaternion.identity);

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

        Debug.Log("no valid sword upgrade selected");
        return null;
    }
    private Vector2 GetThrowPower() => confirmedDirection * (throwPower * 10);

    public void PredictTrajectory(Vector2 direction)
    {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].position = GetTrajectoryPoint(direction, i * spaceBetweenDots);
        }
    }

    private Vector2 GetTrajectoryPoint(Vector2 direction, float t)
    {
        float scaledThrowPower = throwPower * 10;

        Vector2 initialVelocity = direction * scaledThrowPower;

        Vector2 gravityEffect = 0.5f * Physics2D.gravity * swordGravity * (t * t);

        Vector2 predictedPoint = (initialVelocity * t) + gravityEffect;

        Vector2 playerPosition = transform.root.position;

        return playerPosition + predictedPoint;
    }

    public void ComfirmTrajectory(Vector2 direction) => confirmedDirection = direction;

    public void EnableDots(bool enable)
    {
        foreach (Transform t in dots)
            t.gameObject.SetActive(enable);
    }

    private Transform[] GenerateDots()
    {
        Transform[] newDots = new Transform[numberOfDots];

        for (int i = 0; i < numberOfDots; i++)
        {
            newDots[i] = Instantiate(perdictionDot, transform.position, quaternion.identity, transform).transform;
            newDots[i].gameObject.SetActive(false);
        }

        return newDots;
    }
}
