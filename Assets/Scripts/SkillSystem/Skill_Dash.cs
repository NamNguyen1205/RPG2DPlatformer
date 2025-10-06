using UnityEngine;
using UnityEngine.Rendering;

public class Skill_Dash : Skill_Base
{

    public void OnStartEffect()
    {
        if (Unlocked(SkillUpgradeType.Dash_CloneOnStart) || Unlocked(SkillUpgradeType.Dash_CloneOnStartAndArrival))
            CreateClone();
        

        if (Unlocked(SkillUpgradeType.Dash_ShardOnShart) || Unlocked(SkillUpgradeType.Dash_ShardOnStartAndArrival))
            CreateShard();
    }

    public void OnEndEffect()
    {
        if (Unlocked(SkillUpgradeType.Dash_CloneOnStartAndArrival))
            CreateClone();

        if (Unlocked(SkillUpgradeType.Dash_ShardOnStartAndArrival))
            CreateShard();
    }

    private void CreateShard()
    {
        Debug.Log("create time shard!");

        //skill manaeer shard create shard
    }

    private void CreateClone()
    {
        Debug.Log("Create Time echo!");
    } 
}
