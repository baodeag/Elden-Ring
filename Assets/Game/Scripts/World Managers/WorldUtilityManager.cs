using UnityEngine;

namespace baodeag
{
    public class WorldUtilityManager : MonoBehaviour
    {
        public static WorldUtilityManager Instance;

        [Header("Layers")]
        [SerializeField] LayerMask characterLayers;
        [SerializeField] LayerMask enviroLayers;
        [SerializeField] LayerMask slipperyEnviroLayers;

        [Header("UI Colors")]
        [SerializeField] Color poisonedColor;
        [SerializeField] Color burningColor = new Color(1f, 0.45f, 0.1f, 1f);

        [Header("Materials")]
        [SerializeField] Material frozenMaterial;

        [Header("Forces")]
        public float slopeSlideForce = -15;

        [Header("Detection")]
        public float hiddenTargetDetectionRadiusPenalty = 0.25f; //the modifier of distance an ai can detect their target if they are sneaking & hidden

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);
        }

        public LayerMask GetCharacterLayers()
        {
            return characterLayers;
        }

        public LayerMask GetEnviroLayers()
        {
            return enviroLayers;
        }

        public LayerMask GetSlipperyEnviroLayers()
        {
            return slipperyEnviroLayers;
        }

        public Color GetPoisonedColor()
        {
            return poisonedColor;
        }

        public Color GetBurningColor()
        {
            return burningColor;
        }

        public Material GetFrozenMaterial()
        {
            return frozenMaterial;
        }

        public bool CanIDamageThisTarget(CharacterGroup attackingCharacter, CharacterGroup targetCharacter)
        {
            if (attackingCharacter == CharacterGroup.Team01)
            {
                switch (targetCharacter)
                {
                    case CharacterGroup.Team01: return false;
                    case CharacterGroup.Team02: return true;
                    default:
                        break;
                }
            }
            else if (attackingCharacter == CharacterGroup.Team02)
            {
                switch (targetCharacter)
                {
                    case CharacterGroup.Team01: return true;
                    case CharacterGroup.Team02: return false;
                    default:
                        break;
                }
            }

            return false;
        }

        public float GetAngleOfTarget(Transform characterTransform, Vector3 targetsDirection)
        {
            targetsDirection.y = 0;
            float viewableAngle = Vector3.Angle(characterTransform.forward, targetsDirection);
            Vector3 cross = Vector3.Cross(characterTransform.forward, targetsDirection);

            if (cross.y < 0)
                viewableAngle = -viewableAngle;

            return viewableAngle;
        }

        public DamageIntensity GetDamageIntensityBasedOnPoiseDamage(float poiseDamage)
        {
            //throwing dagger, small items
            DamageIntensity damageIntensity = DamageIntensity.Ping;

            //dagger / light attacks
            if (poiseDamage >= 10)
                damageIntensity = DamageIntensity.Light;

            //standard weapons / medium attacks
            if (poiseDamage >= 30)
                damageIntensity = DamageIntensity.Medium;

            //great weapons / heavy attacks
            if (poiseDamage >= 70)
                damageIntensity = DamageIntensity.Heavy;

            //ultra weapons / colossal attacks
            if (poiseDamage >= 120)
                damageIntensity = DamageIntensity.Colossal;

            return damageIntensity;
        }

        public Vector3 GetRipostingPositionBasedOnWeaponClass(WeaponClass weaponClass)
        {
            Vector3 position = new Vector3(0.11f, 0, 0.7f);
            switch (weaponClass)
            {
                case WeaponClass.StraightSword: //change position here if you desire
                    break;
                case WeaponClass.Spear:
                    break;
                case WeaponClass.MediumShield:
                    break;
                case WeaponClass.Fist:
                    break;
                default:
                    break;
            }

            return position;
        }

        public Vector3 GetBackstabPositionBasedOnWeaponClass(WeaponClass weaponClass)
        {
            Vector3 position = new Vector3(0.12f, 0, 0.74f);
            switch (weaponClass)
            {
                case WeaponClass.StraightSword: //change position here if you desire
                    break;
                case WeaponClass.Spear:
                    break;
                case WeaponClass.MediumShield:
                    break;
                case WeaponClass.Fist:
                    break;
                default:
                    break;
            }

            return position;
        }
    }
}
