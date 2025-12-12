using UnityEngine;

public class Enums : MonoBehaviour
{
    
}

public enum CharacterSlot
{
    CharacterSlot_01,
    CharacterSlot_02,
    CharacterSlot_03,
    CharacterSlot_04,
    CharacterSlot_05,
    CharacterSlot_06,
    CharacterSlot_07,
    CharacterSlot_08,
    CharacterSlot_09,
    CharacterSlot_10,
    NO_SLOT
}

public enum CharacterGroup
{
    Team01,
    Team02,
}

public enum WeaponModelSlot 
{
    RightHand,
    LeftHandWeaponSlot,
    LeftHandShieldSlot,
    BackSlot
}

public enum WeaponModelType
{
    Weapon,
    Shield
}

public enum EquipmentType
{
    RightWeapon01, //0
    RightWeapon02, //1
    RightWeapon03, //2
    LeftWeapon01, //3
    LeftWeapon02, //4
    LeftWeapon03, //5
    Head, //6
    Body, //7
    Legs, //8
    Hands //9

}

public enum WeaponClass
{
    StraightSword,
    Spear,
    MediumShield,
    Fist,
    LightShield
}

public enum SpellClass
{
    Incantation,
    Sorcery
}

public enum EquipmentModelType
{
    FullHelmet, //hide face, hair, etc
    Hat, //hide hair
    Hood, //hide hair
    HelmetAcessorie,
    FaceCover,
    Torso,
    Back,
    RightShoulder,
    RightUpperArm,
    RightElbow,
    RightLowerArm,
    RightHand,
    LeftShoulder,
    LeftUpperArm,
    LeftElbow,
    LeftLowerArm,
    LeftHand,
    Hips,
    HipsAttachment,
    RightLeg,
    RightKnee,
    LeftLeg,
    LeftKnee
}

public enum HeadEquipmentType
{
    FullHelmet, //hide face, hair, etc
    Hat, //does not hide anything
    Hood, //hide hair
    FaceCover //hide beard
}

public enum AttackType
{
    LightAttack01,
    LightAttack02,
    HeavyAttack01,
    HeavyAttack02,
    ChargedAttack01,
    ChargedAttack02,
    RunningAttack01,
    RollingAttack01,
    BackstepAttack01
}

public enum DamageIntensity
{
    Ping, 
    Light,
    Medium,
    Heavy,
    Colossal
}

//used to determine item pickup types
public enum ItemPickUpType
{
    WorldSpawn,
    CharacterDrop
}
