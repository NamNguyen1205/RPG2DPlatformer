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
    SwordThrow_Bounce, // Bounce sword will bounce between enemies

    //--------------TIme Echo--------------
    TimeEcho, //create a clone of player, It can take damage from enemies
    TimeEcho_SingleAttack, // Time Echo can perform a single attack
    TimeEcho_MultiAttack, //TIme Echo can perform N attacks
    TimeEcho_ChanceToDuplicate, //TIme Echo has a chance to create another time echo when attacks

    TimeEcho_HealWisp, // when thime echo dies it create a wips that flies towards the player to heal it.
                       //heal is = to percantage of damage taken when died
    TImeEcho_CleanseWisp, //Wisp will now remove negative effects from player   
    TimeEcho_CooldownWisp,//Wisp will reduce cooldown of all skills by N second

    //----------Domain expansion-------------
    Domain_SlowingDown, // create an area in which you slow down enemies by 90 - 100%. you can freely move and fight
    Domain_EchoSpam, // you can no longer move, but you spam enemy with Time echo ability
    Domain_ShardSpam, // yhouu can no longer move, but you spam enemy with time shard ability
}
