using UnityEngine;

public class Enemy_Vfx : Entity_VFX
{
    [Header("Counter Attack windon")]
    [SerializeField] private GameObject attackAlert;

    public void EnableAttackAlert(bool enable) => attackAlert.SetActive(enable);
}
