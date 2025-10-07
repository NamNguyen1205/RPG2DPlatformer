using UnityEngine;

public enum SkillUpgradeType
{
    None,

    //-------Dash Tree----------
    Dash,
    Dash_CloneOnStart, // create a clonce when dash starts
    Dash_CloneOnStartAndArrival, //create a clone when dash starts and ends
    Dash_ShardOnShart, // create a shard when dash stat
    Dash_ShardOnStartAndArrival, //Create a shard when dashs starts and ends

    //-------Shard Tree--------
    Shard, // the shard explodes when touched by an enemy or time goes up
    Shard_MoveToEnemy, // shard will move towards nearest enemy
    Shard_MultiCast, // shard ability can have up to N charges. you can cast them all in a raw
    Shard_Teleport, // you can swap places with the last shard you created
    Shard_TeleportHpRewind, // when you swap places with shard, your hp % is same as it was when you created shard

    //---------Sword Throw---------
    SwordThrow, // you can throw sword to damage enemies from range
    SwordThrow_Spin, // your sword will spin at one point and damage enemies. Like a chainsaw
    SwordThrow_Pierce, // Pierce sword will pierce N targets
    SwordThrow_Bounce // Bounce sword will bounce between enemies
}
