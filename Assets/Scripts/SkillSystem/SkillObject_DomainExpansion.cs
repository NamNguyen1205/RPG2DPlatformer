using UnityEngine;

public class SkillObject_DomainExpansion : SkillObject_Base
{
    private Skill_DomainExpansion domainManager;

    private float expandSpeed = 2;
    private float slowDownPercent = 0.9f;
    private float duration;
    private Vector3 targetScale;
    private bool isShrinking;

    public void SetUpDomain(Skill_DomainExpansion domainManager)
    {
        this.domainManager = domainManager;

        duration = domainManager.GetDomainDuration();
        slowDownPercent = domainManager.GetSlowPercentage();
        expandSpeed = domainManager.expandSpeed;
        float maxSize = domainManager.maxDomainSize;
        
        targetScale = Vector3.one * maxSize;
        Invoke(nameof(ShrinkDomain), duration);
    }

    private void Update()
    {
        HandleScaling();
    }

    private void HandleScaling()
    {
        float sizeDifference = Mathf.Abs(transform.localScale.x - targetScale.x);
        bool shouldChangeScale = sizeDifference > 0.1f;

        if (shouldChangeScale)
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, expandSpeed * Time.deltaTime);
        if (isShrinking && sizeDifference < 0.1f)
            TerminateDomain();
        
    }

    private void TerminateDomain()
    {
        domainManager.ClearTargets();
        Destroy(gameObject);
    }

    private void ShrinkDomain()
    {
        targetScale = Vector3.zero;
        isShrinking = true;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();

        if (enemy == null)
            return;

        domainManager.AddTarget(enemy);
        enemy.SlowDownEntityBy(duration, slowDownPercent, true);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();

        if (enemy == null)
            return;

        enemy.StopSlowDown();
    }
}
