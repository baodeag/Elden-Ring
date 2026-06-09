# Báo Cáo Chi Tiết Script Chính - Assets/Game/Scripts

Ngày tạo: 2026-06-09.
Tổng số script phân tích: **255**.
Tổng số hàm/method ghi nhận: **3088**.

## Cách đọc

- Báo cáo chỉ lấy file `.cs` nằm trong `Assets/Game/Scripts`.
- `Cha` là class nội bộ dự án mà script kế thừa; `Con` là script kế thừa trực tiếp từ script đó.
- `Liên kết script` là các class/script nội bộ xuất hiện trong code của script.
- `Liên kết trong hàm` là các class/script nội bộ xuất hiện trong thân hàm đó.
- Phần giải thích hàm được viết ngắn, dễ hiểu dựa trên tên hàm, vị trí module, kiểu Unity callback/RPC và các class được hàm dùng.

## Tổng quan module

| Module | Số script | Số hàm | Ý nghĩa |
|---|---:|---:|---|
| Assets/Game/Scripts/Animator | 2 | 2 | Animator callbacks. |
| Assets/Game/Scripts/Character | 93 | 1335 | Nền nhân vật, Player, AI, boss, combat, network, stats, locomotion và UI player. |
| Assets/Game/Scripts/Colliders | 6 | 30 | Hitbox/collider gây sát thương, parry/block và projectile collision. |
| Assets/Game/Scripts/Editor | 22 | 527 | Công cụ Unity Editor nội bộ để build/fix/setup asset, scene, boss, merchant. |
| Assets/Game/Scripts/Effects | 16 | 53 | Hiệu ứng instant/timed/static như damage, buff, buildup, poison, frost, burn. |
| Assets/Game/Scripts/Function | 31 | 175 | Object tương tác/trigger trong scene như pickup, elevator, fog wall, site of grace, dialogue. |
| Assets/Game/Scripts/Game Saving | 8 | 24 | Module gameplay chính. |
| Assets/Game/Scripts/Items | 27 | 63 | Dữ liệu và hành vi item: weapon, spell, armor, flask, material, quick slot. |
| Assets/Game/Scripts/Menu Scene | 4 | 223 | Title/menu scene, save slots, settings và preview. |
| Assets/Game/Scripts/Scenes | 3 | 12 | Load scene/additive scene/bootstrap world location. |
| Assets/Game/Scripts/Settings | 1 | 31 | Game settings. |
| Assets/Game/Scripts/Shop | 3 | 22 | Merchant/shop inventory và mua bán. |
| Assets/Game/Scripts/UI | 13 | 69 | UI component độc lập như bar, slot, button animation/sound. |
| Assets/Game/Scripts/Utility | 3 | 2 | Helper nhỏ dùng chung. |
| Assets/Game/Scripts/Weapon Actions | 7 | 25 | Action được weapon/combat gọi để đánh, aim, bắn projectile hoặc cast spell. |
| Assets/Game/Scripts/World Managers | 16 | 495 | Manager cấp world: save, scene, object, AI, sound, item database, boss catalog, progression. |

## Quan hệ kế thừa chính

- `AIBossCharacterManager` -> `AIDurkCharacterManager`
- `AIBossCharacterManager` -> `AIKnightBossCharacterManager`
- `AIBossCharacterManager` -> `AIMonster30CharacterManager`
- `AIBossCharacterManager` -> `AIMonster33CharacterManager`
- `AIBossCharacterManager` -> `AITormentedSoulBossCharacterManager`
- `AIBossCharacterNetworkManager` -> `AIKnightBossNetworkManager`
- `AIBossCharacterNetworkManager` -> `AITormentedSoulBossNetworkManager`
- `AICharacterCombatManager` -> `AIDurkCombatManager`
- `AICharacterCombatManager` -> `AIKnightCombatManager`
- `AICharacterCombatManager` -> `AIMonster30CombatManager`
- `AICharacterCombatManager` -> `AIMonster33CombatManager`
- `AICharacterCombatManager` -> `AITormentedSoulCombatManager`
- `AICharacterCombatManager` -> `AIUndeadCombatManager`
- `AICharacterManager` -> `AIBossCharacterManager`
- `AICharacterNetworkManager` -> `AIBossCharacterNetworkManager`
- `AICharacterNetworkManager` -> `AIMonster33BossCharacterNetworkManager`
- `AIState` -> `AttackState`
- `AIState` -> `BossSleepState`
- `AIState` -> `CombatStanceState`
- `AIState` -> `IdleState`
- `AIState` -> `InvestigateSoundState`
- `AIState` -> `PursueTargetState`
- `ArmorItem` -> `BodyEquipmentItem`
- `ArmorItem` -> `HandEquipmentItem`
- `ArmorItem` -> `HeadEquipmentItem`
- `ArmorItem` -> `LegEquipmentItem`
- `AshOfWar` -> `ParryAshOfWar`
- `CallElevatorInteractable` -> `CallElevatorLeverInteractable`
- `CharacterAnimatorManager` -> `AICharacterAnimatorManager`
- `CharacterAnimatorManager` -> `PlayerAnimatorManager`
- `CharacterCombatManager` -> `AICharacterCombatManager`
- `CharacterCombatManager` -> `PlayerCombatManager`
- `CharacterEffectsManager` -> `PlayerEffectsManager`
- `CharacterEquipmentManager` -> `PlayerEquipmentManager`
- `CharacterInventoryManager` -> `AICharacterInventoryManager`
- `CharacterInventoryManager` -> `PlayerInventoryManager`
- `CharacterLocomotionManager` -> `AICharacterLocomotionManager`
- `CharacterLocomotionManager` -> `PlayerLocomotionManager`
- `CharacterManager` -> `AICharacterManager`
- `CharacterManager` -> `PlayerManager`
- `CharacterNetworkManager` -> `AICharacterNetworkManager`
- `CharacterNetworkManager` -> `PlayerNetworkManager`
- `CharacterSoundFXManager` -> `AICharacterSoundFXManager`
- `CharacterSoundFXManager` -> `AIDurkSoundFXManager`
- `CharacterSoundFXManager` -> `AIMonster30SoundFXManager`
- `CharacterSoundFXManager` -> `PlayerSoundFXManager`
- `CharacterStatsManager` -> `PlayerStatsManager`
- `DamageCollider` -> `DurkClubDamageCollider`
- `DamageCollider` -> `DurkStompCollider`
- `DamageCollider` -> `ManualDamageCollider`
- `DamageCollider` -> `MeleeWeaponDamageCollider`
- `DamageCollider` -> `RangedProjectileDamageCollider`
- `DamageCollider` -> `SpellProjectileDamageCollider`
- `EquipmentItem` -> `ArmorItem`
- `EquipmentItem` -> `WeaponItem`
- `InstantCharacterEffect` -> `BloodLossEffect`
- `InstantCharacterEffect` -> `TakeBlockedDamageEffect`
- `InstantCharacterEffect` -> `TakeBuildUpEffect`
- `InstantCharacterEffect` -> `TakeDamageEffect`
- `InstantCharacterEffect` -> `TakeStaminaDamageEffect`
- `Interactable` -> `AnvilInteractable`
- `Interactable` -> `CallElevatorInteractable`
- `Interactable` -> `DialogueInteractable`
- `Interactable` -> `ElevatorInteractable`
- `Interactable` -> `FogWallInteractable`
- `Interactable` -> `PickUpItemInteractable`
- `Interactable` -> `PickUpRunesInteractable`
- `Interactable` -> `ShopInteractable`
- `Interactable` -> `SiteOfGraceInteractable`
- `Interactable` -> `WorldMapTransitionInteractable`
- `Item` -> `AshOfWar`
- `Item` -> `EquipmentItem`
- `Item` -> `QuickSlotItem`
- `Item` -> `RangedProjectileItem`
- `Item` -> `SpellItem`
- `Item` -> `UpgradeMaterial`
- `ManualDamageCollider` -> `Monster33FireDamageCollider`
- `PlayerUIMenu` -> `PlayerUICharacterMenuManager`
- `PlayerUIMenu` -> `PlayerUIEquipmentManager`
- `PlayerUIMenu` -> `PlayerUILevelUpManager`
- `PlayerUIMenu` -> `PlayerUISettingsManager`
- `PlayerUIMenu` -> `PlayerUIShopManager`
- `PlayerUIMenu` -> `PlayerUISiteOfGraceManager`
- `PlayerUIMenu` -> `PlayerUITeleportLocationManager`
- `PlayerUIMenu` -> `PlayerUIWeaponUpgradeManager`
- `QuickSlotItem` -> `BuffCharmItem`
- `QuickSlotItem` -> `FlaskItem`
- `SpellItem` -> `FireBallSpell`
- `SpellItem` -> `TestSpell`
- `SpellManager` -> `FireBallManager`
- `SpellProjectileDamageCollider` -> `FireBallDamageCollider`
- `StaticCharacterEffect` -> `TwoHandingEffect`
- `TakeDamageEffect` -> `TakeCriticalDamageEffect`
- `TimedCharacterEffect` -> `BuildUpEffect`
- `TimedCharacterEffect` -> `BurningEffect`
- `TimedCharacterEffect` -> `FrostBiteEffect`
- `TimedCharacterEffect` -> `ModifyStaminaRegenerationForATimeEffect`
- `TimedCharacterEffect` -> `PlayerStatBuffTimedEffect`
- `TimedCharacterEffect` -> `PoisonedEffect`
- `UI_StatBar` -> `UI_Boss_HP_Bar`
- `UI_StatBar` -> `UI_BuildUpBar`
- `UI_StatBar` -> `UI_Character_HP_Bar`
- `WeaponItem` -> `CasterWeaponItem`
- `WeaponItem` -> `MeleeWeaponItem`
- `WeaponItem` -> `RangedWeaponItem`
- `WeaponItemAction` -> `AimAction`
- `WeaponItemAction` -> `CastIncantationAction`
- `WeaponItemAction` -> `FireProjectileAction`
- `WeaponItemAction` -> `HeavyAttackWeaponItemAction`
- `WeaponItemAction` -> `LightAttackWeaponItemAction`
- `WeaponItemAction` -> `OffHandMeleeAction`

## Chi tiết từng script

### Assets/Game/Scripts/Animator

#### ResetJumping

- **Đường dẫn:** `Assets/Game/Scripts/Animator/ResetJumping.cs`
- **Loại:** Animator state
- **Vai trò dễ hiểu:** Callback trong Animator để reset hoặc toggle parameter animation.
- **Kế thừa/cha:** StateMachineBehaviour
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** CharacterManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)` | 11 | Thực hiện logic on state enter trong script ResetJumping. Liên kết trực tiếp: CharacterManager. | CharacterManager |

#### ToggleAttackType

- **Đường dẫn:** `Assets/Game/Scripts/Animator/ToggleAttackType.cs`
- **Loại:** Animator state
- **Vai trò dễ hiểu:** Callback trong Animator để reset hoặc toggle parameter animation.
- **Kế thừa/cha:** StateMachineBehaviour
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** AttackType, CharacterManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)` | 11 | Thực hiện logic on state enter trong script ToggleAttackType. Liên kết trực tiếp: CharacterManager. | CharacterManager |

### Assets/Game/Scripts/Character

#### CharacterAnimatorManager

- **Đường dẫn:** `Assets/Game/Scripts/Character/CharacterAnimatorManager.cs`
- **Loại:** MonoBehaviour
- **Vai trò dễ hiểu:** Base module dùng chung cho Character, được Player và AI kế thừa hoặc tham chiếu.
- **Kế thừa/cha:** MonoBehaviour
- **Script con:** AICharacterAnimatorManager, PlayerAnimatorManager
- **Field public/serialized chính:** public bool applyRootMotion, public string lastDamageAnimationPlayed, public List<string> forward_Ping_Damage, public List<string> backward_Ping_Damage, public List<string> left_Ping_Damage, public List<string> right_Ping_Damage, public List<string> forward_Medium_Damage, public List<string> backward_Medium_Damage, public List<string> left_Medium_Damage, public List<string> right_Medium_Damage
- **Liên kết script:** AttackType, CharacterManager, WeaponItem

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `protected virtual void Awake()` | 52 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. Liên kết trực tiếp: CharacterManager. | CharacterManager |
| `protected virtual void Start()` | 60 | Chạy sau Awake để hoàn tất thiết lập ban đầu, nhất là khi cần các object khác đã sẵn sàng. | - |
| `public string GetRandomAnimationFromList(List<string> animationList)` | 89 | Lấy dữ liệu random animation from list cho hệ thống khác sử dụng. | - |
| `public void UpdateAnimatorMovementParameters(float horizontalMovement, float verticalMovement, bool isSprinting)` | 112 | Cập nhật animator movement parameters theo trạng thái mới. | - |
| `public void SetAnimatorMovementParameters(float horizontalMovement, float verticalMovement)` | 170 | Thiết lập giá trị hoặc trạng thái animator movement parameters. | - |
| `public virtual void PlayTargetActionAnimation( string targetAnimation, bool isPerformingAction, bool applyRootMotion = true, bool canRotate = false, bool canMove = false, bool canRun = true, bool canRoll = false)` | 176 | Phát target action animation, thường là animation, sound hoặc VFX. | - |
| `public virtual void PlayTargetActionAnimationInstantly( string targetAnimation, bool isPerformingAction, bool applyRootMotion = true, bool canRotate = false, bool canMove = false, bool canRun = true, bool canRoll = false)` | 204 | Phát target action animation instantly, thường là animation, sound hoặc VFX. | - |
| `public virtual void PlayTargetAttackActionAnimation( WeaponItem weapon, AttackType attackType, string targetAnimation, bool isPerformingAction, bool applyRootMotion = true, bool canRotate = false, bool canMove = false, bool canRoll = false)` | 232 | Phát target attack action animation, thường là animation, sound hoặc VFX. | - |
| `public void UpdateAnimatorController(AnimatorOverrideController weaponController)` | 260 | Cập nhật animator controller theo trạng thái mới. | - |

#### CharacterCombatManager

- **Đường dẫn:** `Assets/Game/Scripts/Character/CharacterCombatManager.cs`
- **Loại:** NetworkBehaviour
- **Vai trò dễ hiểu:** Base module dùng chung cho Character, được Player và AI kế thừa hoặc tham chiếu.
- **Kế thừa/cha:** NetworkBehaviour
- **Script con:** AICharacterCombatManager, PlayerCombatManager
- **Field public/serialized chính:** public string lastAttackAnimationPerformed, public float previousPoiseDamageTaken, public CharacterManager currentTarget, public AttackType currentAttackType, public Transform lockOnTransform, public bool canPerformRollingAttack, public bool canPerformBackstepAttack, public bool canBlock, public bool canBeBackstabbed, public int pendingCriticalDamage, public List<CharacterManager> charactersTargetingMe, public List<StealthObject> stealthObjectsCurrentlyStandingIn
- **Liên kết script:** AttackType, CharacterManager, StealthObject, WorldUtilityManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `protected virtual void Awake()` | 46 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. Liên kết trực tiếp: CharacterManager. | CharacterManager |
| `public virtual void SetTarget(CharacterManager newTarget)` | 51 | Thiết lập giá trị hoặc trạng thái target. | - |
| `public virtual void AttemptCriticalAttack()` | 73 | Cố gắng kích hoạt critical attack nếu trạng thái hiện tại cho phép. Liên kết trực tiếp: CharacterManager, WorldUtilityManager. | CharacterManager, WorldUtilityManager |
| `public virtual void AttemptRiposte(RaycastHit hit)` | 135 | Cố gắng kích hoạt riposte nếu trạng thái hiện tại cho phép. | - |
| `public virtual void AttemptBackstab(RaycastHit hit)` | 140 | Cố gắng kích hoạt backstab nếu trạng thái hiện tại cho phép. | - |
| `public virtual void ApplyCriticalDamage()` | 145 | Áp dụng critical damage lên character/object mục tiêu. | - |
| `public IEnumerator ForceMoveEnemyCharacterToRipostePosition(CharacterManager enemyCharacter, Vector3 ripostePosition)` | 154 | Thực hiện logic force move enemy character to riposte position trong script CharacterCombatManager. | - |
| `new GameObject("Riposte Transform")` | 164 | Thực hiện logic game object trong script CharacterCombatManager. | - |
| `public IEnumerator ForceMoveEnemyCharacterToBackstabPosition(CharacterManager enemyCharacter, Vector3 backstabPosition)` | 177 | Thực hiện logic force move enemy character to backstab position trong script CharacterCombatManager. | - |
| `new GameObject("Backstab Transform")` | 187 | Thực hiện logic game object trong script CharacterCombatManager. | - |
| `public void CheckForHiddenStatus()` | 200 | Thực hiện logic check for hidden status trong script CharacterCombatManager. | - |
| `public void EnableIsInvulnerable()` | 218 | Bật is invulnerable. | - |
| `public void DisableIsInvulnerable()` | 224 | Tắt is invulnerable. | - |
| `public void EnableIsParrying()` | 230 | Bật is parrying. | - |
| `public void DisableIsParrying()` | 236 | Tắt is parrying. | - |
| `public void EnableIsRipostable()` | 242 | Bật is ripostable. | - |
| `public void EnableCanDoRollingAttack()` | 248 | Bật can do rolling attack. | - |
| `public void DisableCanDoRollingAttack()` | 253 | Tắt can do rolling attack. | - |
| `public void EnableCanDoBackstepAttack()` | 258 | Bật can do backstep attack. | - |
| `public void DisableCanDoBackstepAttack()` | 263 | Tắt can do backstep attack. | - |
| `public virtual void EnableCanDoCombo()` | 268 | Bật can do combo. | - |
| `public virtual void DisableCanDoCombo()` | 273 | Tắt can do combo. | - |
| `public virtual void CloseAllDamageColliders()` | 278 | Đóng UI/trạng thái/luồng all damage colliders. | - |
| `public void DestroyAllCurrentActionFX()` | 284 | Thực hiện logic destroy all current action fx trong script CharacterCombatManager. | - |
| `public void AddStealthObject(StealthObject stealthObject)` | 290 | Thêm stealth object vào danh sách, trạng thái hoặc dữ liệu. | - |
| `public void RemoveStealthObject(StealthObject stealthObject)` | 306 | Loại bỏ stealth object khỏi danh sách, trạng thái hoặc dữ liệu. | - |

#### CharacterEffectsManager

- **Đường dẫn:** `Assets/Game/Scripts/Character/CharacterEffectsManager.cs`
- **Loại:** MonoBehaviour
- **Vai trò dễ hiểu:** Base module dùng chung cho Character, được Player và AI kế thừa hoặc tham chiếu.
- **Kế thừa/cha:** MonoBehaviour
- **Script con:** PlayerEffectsManager
- **Field public/serialized chính:** public GameObject activeQuickSlotItemFX, public GameObject activeSpellWarmUpFX, public GameObject activeDrawnProjectileFX, public GameObject poisonedVFX, public GameObject burningVFX, public GameObject frostBiteVFX, public List<StaticCharacterEffect> staticEffects, [SerializeField] protected float effectTickTimer, [SerializeField] protected float defaultEffectTickTime, public List<TimedCharacterEffect> timedEffects
- **Liên kết script:** BuildUp, CharacterManager, InstantCharacterEffect, StaticCharacterEffect, TimedCharacterEffect, WorldCharacterEffectsManager, WorldUtilityManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `protected virtual void Awake()` | 49 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. Liên kết trực tiếp: CharacterManager. | CharacterManager |
| `protected virtual void Update()` | 54 | Cập nhật logic mỗi frame như input, timer, trạng thái chiến đấu hoặc UI. | - |
| `public virtual void ProcessInstantEffect(InstantCharacterEffect effect)` | 65 | Thực hiện logic process instant effect trong script CharacterEffectsManager. | - |
| `public void PlayBloodSplatterVFX(Vector3 contactPoint)` | 70 | Phát blood splatter vfx, thường là animation, sound hoặc VFX. Liên kết trực tiếp: WorldCharacterEffectsManager. | WorldCharacterEffectsManager |
| `public void PlayCriticalBloodSplatterVFX(Vector3 contactPoint)` | 82 | Phát critical blood splatter vfx, thường là animation, sound hoặc VFX. Liên kết trực tiếp: WorldCharacterEffectsManager. | WorldCharacterEffectsManager |
| `public virtual void AddBuildUps(BuildUp buildUpType, float amount)` | 94 | Thêm build ups vào danh sách, trạng thái hoặc dữ liệu. Liên kết trực tiếp: BuildUp. | BuildUp |
| `public void AddStaticEffect(StaticCharacterEffect effect)` | 119 | Thêm static effect vào danh sách, trạng thái hoặc dữ liệu. | - |
| `public void RemoveStaticEffect(int effectID)` | 132 | Loại bỏ static effect khỏi danh sách, trạng thái hoặc dữ liệu. Liên kết trực tiếp: StaticCharacterEffect. | StaticCharacterEffect |
| `public void ProcessTimedEffects()` | 158 | Thực hiện logic process timed effects trong script CharacterEffectsManager. | - |
| `public void AddTimedEffect(TimedCharacterEffect effect)` | 170 | Thêm timed effect vào danh sách, trạng thái hoặc dữ liệu. | - |
| `public void RemoveTimedEffect(int effectID)` | 198 | Loại bỏ timed effect khỏi danh sách, trạng thái hoặc dữ liệu. Liên kết trực tiếp: TimedCharacterEffect. | TimedCharacterEffect |
| `public TimedCharacterEffect CheckForTimedEffect(int effectID)` | 225 | Thực hiện logic check for timed effect trong script CharacterEffectsManager. Liên kết trực tiếp: TimedCharacterEffect. | TimedCharacterEffect |
| `public void ProcessEffectDamage(int effectDamage)` | 241 | Thực hiện logic process effect damage trong script CharacterEffectsManager. | - |
| `public void PlayFrozenFX()` | 263 | Phát frozen fx, thường là animation, sound hoặc VFX. Liên kết trực tiếp: WorldUtilityManager. | WorldUtilityManager |
| `private IEnumerator ActivateFrozenVFXCoroutine(Material frozenMaterial)` | 274 | Thực hiện logic activate frozen vfxcoroutine trong script CharacterEffectsManager. | - |

#### CharacterEquipmentManager

- **Đường dẫn:** `Assets/Game/Scripts/Character/CharacterEquipmentManager.cs`
- **Loại:** MonoBehaviour
- **Vai trò dễ hiểu:** Base module dùng chung cho Character, được Player và AI kế thừa hoặc tham chiếu.
- **Kế thừa/cha:** MonoBehaviour
- **Script con:** PlayerEquipmentManager
- **Field public/serialized chính:** -
- **Liên kết script:** -

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `protected virtual void Awake()` | 7 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. | - |
| `protected virtual void Start()` | 12 | Chạy sau Awake để hoàn tất thiết lập ban đầu, nhất là khi cần các object khác đã sẵn sàng. | - |

#### CharacterFootStepSFXMaker

- **Đường dẫn:** `Assets/Game/Scripts/Character/CharacterFootStepSFXMaker.cs`
- **Loại:** MonoBehaviour
- **Vai trò dễ hiểu:** Base module dùng chung cho Character, được Player và AI kế thừa hoặc tham chiếu.
- **Kế thừa/cha:** MonoBehaviour
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** CharacterManager, WorldUtilityManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `private void Awake()` | 16 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. Liên kết trực tiếp: CharacterManager. | CharacterManager |
| `private void FixedUpdate()` | 22 | Cập nhật theo bước vật lý, thường xử lý movement, trigger hoặc physics. | - |
| `private void CheckForFootSteps()` | 27 | Thực hiện logic check for foot steps trong script CharacterFootStepSFXMaker. Liên kết trực tiếp: WorldUtilityManager. | WorldUtilityManager |
| `private void PlayFootStepSoundFX()` | 58 | Phát foot step sound fx, thường là animation, sound hoặc VFX. | - |

#### CharacterInventoryManager

- **Đường dẫn:** `Assets/Game/Scripts/Character/CharacterInventoryManager.cs`
- **Loại:** MonoBehaviour
- **Vai trò dễ hiểu:** Base module dùng chung cho Character, được Player và AI kế thừa hoặc tham chiếu.
- **Kế thừa/cha:** MonoBehaviour
- **Script con:** AICharacterInventoryManager, PlayerInventoryManager
- **Field public/serialized chính:** -
- **Liên kết script:** -

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `protected virtual void Awake()` | 7 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. | - |

#### CharacterLocomotionManager

- **Đường dẫn:** `Assets/Game/Scripts/Character/CharacterLocomotionManager.cs`
- **Loại:** MonoBehaviour
- **Vai trò dễ hiểu:** Base module dùng chung cho Character, được Player và AI kế thừa hoặc tham chiếu.
- **Kế thừa/cha:** MonoBehaviour
- **Script con:** AICharacterLocomotionManager, PlayerLocomotionManager
- **Field public/serialized chính:** [SerializeField] protected float gravityForce, [SerializeField] protected Vector3 yVelocity, [SerializeField] protected float groundedYVelocity, [SerializeField] protected float fallStartYVelocity, [SerializeField] protected float inAirTimer, public bool isRolling, public bool canRotate, public bool canMove, public bool canRun, public bool canRoll, public bool isGrounded, public bool isRidingLift
- **Liên kết script:** CharacterManager, PlayerUIManager, WorldUtilityManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `protected virtual void Awake()` | 43 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. Liên kết trực tiếp: CharacterManager. | CharacterManager |
| `protected virtual void Update()` | 48 | Cập nhật logic mỗi frame như input, timer, trạng thái chiến đấu hoặc UI. Liên kết trực tiếp: PlayerUIManager. | PlayerUIManager |
| `protected void OnControllerColliderHit(ControllerColliderHit hit)` | 92 | Thực hiện logic on controller collider hit trong script CharacterLocomotionManager. | - |
| `protected void HandleGroundCheck()` | 99 | Xử lý luồng ground check. | - |
| `protected void OnDrawGizmosSelected()` | 126 | Thực hiện logic on draw gizmos selected trong script CharacterLocomotionManager. | - |
| `public void EnableCanRotate()` | 131 | Bật can rotate. | - |
| `public void DisableCanRotate()` | 136 | Tắt can rotate. | - |
| `private void HandleSlopeSlideCheck()` | 143 | Xử lý luồng slope slide check. Liên kết trực tiếp: WorldUtilityManager. | WorldUtilityManager |
| `private void SetSlopeSlideVelocity(LayerMask layers)` | 161 | Thiết lập giá trị hoặc trạng thái slope slide velocity. | - |
| `new Vector3(transform.position.x, transform.position.y + slopeSlideStartPositionYOffset, transform.position.z)` | 163 | Thực hiện logic vector3 trong script CharacterLocomotionManager. | - |
| `new Vector3(0, slopeSlideSpeed, 0), hitinfo.normal)` | 170 | Thực hiện logic vector3 trong script CharacterLocomotionManager. | - |
| `new Vector3(0, slopeSlideSpeed, 0), hitinfo.normal)` | 174 | Thực hiện logic vector3 trong script CharacterLocomotionManager. | - |
| `private void SetGroundedVelocity()` | 196 | Thiết lập giá trị hoặc trạng thái grounded velocity. Liên kết trực tiếp: WorldUtilityManager. | WorldUtilityManager |
| `protected virtual void SlideOffCharacter()` | 267 | Thực hiện logic slide off character trong script CharacterLocomotionManager. | - |
| `protected virtual IEnumerator SlideOffCharacterCoroutine()` | 275 | Thực hiện logic slide off character coroutine trong script CharacterLocomotionManager. Liên kết trực tiếp: WorldUtilityManager. | WorldUtilityManager |
| `new Vector3(0, yVelocity.y, 0), hitInfo.normal)` | 284 | Thực hiện logic vector3 trong script CharacterLocomotionManager. | - |
| `protected virtual void OnIsGrounded()` | 303 | Thực hiện logic on is grounded trong script CharacterLocomotionManager. | - |
| `protected virtual void OnIsNotGrounded()` | 308 | Thực hiện logic on is not grounded trong script CharacterLocomotionManager. | - |

#### CharacterManager

- **Đường dẫn:** `Assets/Game/Scripts/Character/CharacterManager.cs`
- **Loại:** NetworkBehaviour
- **Vai trò dễ hiểu:** Base module dùng chung cho Character, được Player và AI kế thừa hoặc tham chiếu.
- **Kế thừa/cha:** NetworkBehaviour
- **Script con:** AICharacterManager, PlayerManager
- **Field public/serialized chính:** public NetworkVariable<bool> isDead, public CharacterController characterController, public Animator animator, public CharacterNetworkManager characterNetworkManager, public CharacterEffectsManager characterEffectsManager, public CharacterAnimatorManager characterAnimatorManager, public CharacterCombatManager characterCombatManager, public CharacterSoundFXManager characterSoundFXManager, public CharacterLocomotionManager characterLocomotionManager, public CharacterUIManager characterUIManager, public CharacterStatsManager characterStatsManager, public CharacterGroup characterGroup +1
- **Liên kết script:** CharacterAnimatorManager, CharacterCombatManager, CharacterEffectsManager, CharacterGroup, CharacterLocomotionManager, CharacterNetworkManager, CharacterSoundFXManager, CharacterStatsManager, CharacterUIManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `protected virtual void Awake()` | 34 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. Liên kết trực tiếp: CharacterAnimatorManager, CharacterCombatManager, CharacterEffectsManager, CharacterLocomotionManager, CharacterNetworkManager +3. | CharacterAnimatorManager, CharacterCombatManager, CharacterEffectsManager, CharacterLocomotionManager, CharacterNetworkManager, CharacterSoundFXManager, CharacterStatsManager, CharacterUIManager |
| `protected virtual void Start()` | 48 | Chạy sau Awake để hoàn tất thiết lập ban đầu, nhất là khi cần các object khác đã sẵn sàng. | - |
| `protected virtual void Update()` | 53 | Cập nhật logic mỗi frame như input, timer, trạng thái chiến đấu hoặc UI. | - |
| `new Vector3 (characterNetworkManager.networkPosition.Value.x, transform.position.y, characterNetworkManager.networkPosition.Value.z)` | 82 | Thực hiện logic vector3 trong script CharacterManager. | - |
| `protected virtual void FixedUpdate()` | 90 | Cập nhật theo bước vật lý, thường xử lý movement, trigger hoặc physics. | - |
| `protected virtual void LateUpdate()` | 95 | Cập nhật cuối frame, thường dùng cho camera, animation hoặc đồng bộ trạng thái sau movement. | - |
| `protected virtual void OnEnable()` | 104 | Đăng ký event/callback hoặc bật trạng thái khi component được enable. | - |
| `protected virtual void OnDisable()` | 109 | Gỡ event/callback hoặc dọn trạng thái khi component bị disable. | - |
| `public override void OnNetworkSpawn()` | 114 | Thiết lập dữ liệu và callback khi NetworkObject được spawn trong Netcode. | - |
| `public override void OnNetworkDespawn()` | 127 | Dọn dữ liệu/callback khi NetworkObject bị despawn khỏi Netcode. | - |
| `public virtual IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false)` | 136 | Thực hiện logic process death event trong script CharacterManager. | - |
| `new WaitForSeconds(5)` | 151 | Thực hiện logic wait for seconds trong script CharacterManager. | - |
| `public virtual void ReviveCharacter()` | 154 | Thực hiện logic revive character trong script CharacterManager. | - |
| `protected virtual void IgnoreMyOwnColliders()` | 159 | Thực hiện logic ignore my own colliders trong script CharacterManager. | - |

#### CharacterNetworkManager

- **Đường dẫn:** `Assets/Game/Scripts/Character/CharacterNetworkManager.cs`
- **Loại:** NetworkBehaviour
- **Vai trò dễ hiểu:** Base module dùng chung cho Character, được Player và AI kế thừa hoặc tham chiếu.
- **Kế thừa/cha:** NetworkBehaviour
- **Script con:** AICharacterNetworkManager, PlayerNetworkManager
- **Field public/serialized chính:** public NetworkVariable<bool> isActive, public NetworkVariable<Vector3> networkPosition, public NetworkVariable<Quaternion> networkRotation, public Vector3 networkPositionVelocity, public float networkPositionSmoothTime, public float networkRotationSmoothTime, public NetworkVariable<bool> isMoving, public NetworkVariable<float> horizontalMovement, public NetworkVariable<float> verticalMovement, public NetworkVariable<float> moveAmount, public NetworkVariable<ulong> currentTargetNetworkObjectID, public NetworkVariable<bool> isBlocking +38
- **Liên kết script:** CharacterManager, TakeCriticalDamageEffect, TakeDamageEffect, WeaponItem, WorldCharacterEffectsManager, WorldItemDatabase, WorldUtilityManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `protected virtual void Awake()` | 221 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. Liên kết trực tiếp: CharacterManager. | CharacterManager |
| `public virtual void OnHPChanged(int oldValue, int newValue)` | 226 | Thực hiện logic on hpchanged trong script CharacterNetworkManager. | - |
| `public virtual void OnIsDeadChanged(bool oldStatus, bool newStatus)` | 245 | Thực hiện logic on is dead changed trong script CharacterNetworkManager. | - |
| `public virtual void OnLockOnTargetIDChange(ulong oldID, ulong newID)` | 253 | Thực hiện logic on lock on target idchange trong script CharacterNetworkManager. Liên kết trực tiếp: CharacterManager. | CharacterManager |
| `public void OnIsLockedOnChanged(bool old, bool isLockedOn)` | 262 | Thực hiện logic on is locked on changed trong script CharacterNetworkManager. | - |
| `public void OnIsChargingAttackChanged(bool oldStatus, bool newStatus)` | 270 | Thực hiện logic on is charging attack changed trong script CharacterNetworkManager. | - |
| `public void OnIsMovingChanged(bool oldStatus, bool newStatus)` | 275 | Thực hiện logic on is moving changed trong script CharacterNetworkManager. | - |
| `public virtual void OnIsActiveChanged(bool oldStatus, bool newStatus)` | 280 | Thực hiện logic on is active changed trong script CharacterNetworkManager. | - |
| `public virtual void OnIsBlockingChanged(bool oldStatus, bool newStatus)` | 285 | Thực hiện logic on is blocking changed trong script CharacterNetworkManager. | - |
| `public virtual void OnIsPoisonedChanged(bool oldStatus, bool newStatus)` | 290 | Thực hiện logic on is poisoned changed trong script CharacterNetworkManager. Liên kết trực tiếp: WorldCharacterEffectsManager. | WorldCharacterEffectsManager |
| `public virtual void OnIsBurningChanged(bool oldStatus, bool newStatus)` | 312 | Thực hiện logic on is burning changed trong script CharacterNetworkManager. Liên kết trực tiếp: WorldCharacterEffectsManager. | WorldCharacterEffectsManager |
| `public virtual void OnIsBleedingChanged(bool oldStatus, bool newStatus)` | 334 | Thực hiện logic on is bleeding changed trong script CharacterNetworkManager. Liên kết trực tiếp: WorldCharacterEffectsManager. | WorldCharacterEffectsManager |
| `public virtual void OnIsFrostBittenChanged(bool oldStatus, bool newStatus)` | 345 | Thực hiện logic on is frost bitten changed trong script CharacterNetworkManager. Liên kết trực tiếp: WorldCharacterEffectsManager. | WorldCharacterEffectsManager |
| `public virtual void OnIsFrozenChanged(bool oldStatus, bool newStatus)` | 368 | Thực hiện logic on is frozen changed trong script CharacterNetworkManager. | - |
| `public virtual void AddCharacterToListOfCharactersTargetingMeServerRpc(ulong characterTargetingMeID)` | 383 | Gửi yêu cầu lên server trong Netcode để server xử lý add character to list of characters targeting me. | - |
| `protected virtual void AddCharacterToListOfCharactersTargetingMeClientRpc(ulong characterTargetingMeID)` | 390 | Server gọi xuống client để đồng bộ hoặc phát hiệu ứng cho add character to list of characters targeting me. Liên kết trực tiếp: CharacterManager. | CharacterManager |
| `public virtual void RemoveCharacterFromListOfCharactersTargetingMeServerRpc(ulong characterTargetingMeID)` | 407 | Gửi yêu cầu lên server trong Netcode để server xử lý remove character from list of characters targeting me. | - |
| `protected virtual void RemoveCharacterFromListOfCharactersTargetingMeClientRpc(ulong characterTargetingMeID)` | 414 | Server gọi xuống client để đồng bộ hoặc phát hiệu ứng cho remove character from list of characters targeting me. Liên kết trực tiếp: CharacterManager. | CharacterManager |
| `public virtual void ClearTargetServerRpc()` | 431 | Gửi yêu cầu lên server trong Netcode để server xử lý clear target. | - |
| `public virtual void ClearTargetClientRpc()` | 438 | Server gọi xuống client để đồng bộ hoặc phát hiệu ứng cho clear target. | - |
| `public void DestroyAllCurrentActionFXServerRpc()` | 446 | Gửi yêu cầu lên server trong Netcode để server xử lý destroy all current action fx. | - |
| `public virtual void DestroyAllCurrentActionFXClientRpc()` | 455 | Server gọi xuống client để đồng bộ hoặc phát hiệu ứng cho destroy all current action fx. | - |
| `public void NotifyTheServerOfActionAnimationServerRpc(ulong clientID, string animationID, bool applyRootMotion)` | 469 | Gửi yêu cầu lên server trong Netcode để server xử lý notify the server of action animation. | - |
| `public void PlayActionAnimationForAllClientsClientRpc(ulong clientID, string animationID, bool applyRootMotion)` | 481 | Server gọi xuống client để đồng bộ hoặc phát hiệu ứng cho play action animation for all clients. | - |
| `private void PerformActionAnimationFromServer(string animationID, bool applyRootMotion)` | 490 | Thực hiện logic perform action animation from server trong script CharacterNetworkManager. | - |
| `public void NotifyTheServerOfInstantActionAnimationServerRpc(ulong clientID, string animationID, bool applyRootMotion)` | 497 | Gửi yêu cầu lên server trong Netcode để server xử lý notify the server of instant action animation. | - |
| `public void PlayInstantActionAnimationForAllClientsClientRpc(ulong clientID, string animationID, bool applyRootMotion)` | 509 | Server gọi xuống client để đồng bộ hoặc phát hiệu ứng cho play instant action animation for all clients. | - |
| `private void PerformInstantActionAnimationFromServer(string animationID, bool applyRootMotion)` | 518 | Thực hiện logic perform instant action animation from server trong script CharacterNetworkManager. | - |
| `public void NotifyTheServerOfAttackActionAnimationServerRpc(ulong clientID, string animationID, bool applyRootMotion)` | 527 | Gửi yêu cầu lên server trong Netcode để server xử lý notify the server of attack action animation. | - |
| `public void PlayAttackActionAnimationForAllClientsClientRpc(ulong clientID, string animationID, bool applyRootMotion)` | 538 | Server gọi xuống client để đồng bộ hoặc phát hiệu ứng cho play attack action animation for all clients. | - |
| `private void PerformAttackActionAnimationFromServer(string animationID, bool applyRootMotion)` | 547 | Thực hiện logic perform attack action animation from server trong script CharacterNetworkManager. | - |
| `public void NotifyTheServerOfCharacterDamageServerRpc( ulong damagedCharacterID, ulong characterCausingDamageID, float physicalDamage, float magicDamage, float fireDamage, float lightningDamage, float holyDamage, float poiseDamage, float angleHitFrom, float contactPointX, float contactPointY, float contactPointZ)` | 555 | Gửi yêu cầu lên server trong Netcode để server xử lý notify the server of character damage. | - |
| `public void NotifyTheServerOfCharacterDamageClientRpc( ulong damagedCharacterID, ulong characterCausingDamageID, float physicalDamage, float magicDamage, float fireDamage, float lightningDamage, float holyDamage, float poiseDamage, float angleHitFrom, float contactPointX, float contactPointY, float contactPointZ)` | 577 | Server gọi xuống client để đồng bộ hoặc phát hiệu ứng cho notify the server of character damage. | - |
| `public void ProcessCharacterDamageFromServer( ulong damagedCharacterID, ulong characterCausingDamageID, float physicalDamage, float magicDamage, float fireDamage, float lightningDamage, float holyDamage, float poiseDamage, float angleHitFrom, float contactPointX, float contactPointY, float contactPointZ)` | 595 | Thực hiện logic process character damage from server trong script CharacterNetworkManager. Liên kết trực tiếp: CharacterManager, TakeDamageEffect, WorldCharacterEffectsManager. | CharacterManager, TakeDamageEffect, WorldCharacterEffectsManager |
| `new Vector3(contactPointX, contactPointY, contactPointZ)` | 621 | Thực hiện logic vector3 trong script CharacterNetworkManager. | - |
| `public void NotifyTheServerOfRiposteServerRpc( ulong damagedCharacterID, ulong characterCausingDamageID, string criticalDamageAnimation, int weaponID, float physicalDamage, float magicDamage, float fireDamage, float lightningDamage, float holyDamage, float poiseDamage)` | 629 | Gửi yêu cầu lên server trong Netcode để server xử lý notify the server of riposte. | - |
| `public void NotifyTheServerOfRiposteClientRpc( ulong damagedCharacterID, ulong characterCausingDamageID, string criticalDamageAnimation, int weaponID, float physicalDamage, float magicDamage, float fireDamage, float lightningDamage, float holyDamage, float poiseDamage)` | 658 | Server gọi xuống client để đồng bộ hoặc phát hiệu ứng cho notify the server of riposte. | - |
| `public void ProcessRiposteFromServer( ulong damagedCharacterID, ulong characterCausingDamageID, string criticalDamageAnimation, int weaponID, float physicalDamage, float magicDamage, float fireDamage, float lightningDamage, float holyDamage, float poiseDamage)` | 683 | Thực hiện logic process riposte from server trong script CharacterNetworkManager. Liên kết trực tiếp: CharacterManager, TakeCriticalDamageEffect, WeaponItem, WorldCharacterEffectsManager, WorldItemDatabase +1. | CharacterManager, TakeCriticalDamageEffect, WeaponItem, WorldCharacterEffectsManager, WorldItemDatabase, WorldUtilityManager |
| `public void NotifyTheServerOfBackstabServerRpc( ulong damagedCharacterID, ulong characterCausingDamageID, string criticalDamageAnimation, int weaponID, float physicalDamage, float magicDamage, float fireDamage, float lightningDamage, float holyDamage, float poiseDamage)` | 723 | Gửi yêu cầu lên server trong Netcode để server xử lý notify the server of backstab. | - |
| `public void NotifyTheServerOfBackstabClientRpc( ulong damagedCharacterID, ulong characterCausingDamageID, string criticalDamageAnimation, int weaponID, float physicalDamage, float magicDamage, float fireDamage, float lightningDamage, float holyDamage, float poiseDamage)` | 752 | Server gọi xuống client để đồng bộ hoặc phát hiệu ứng cho notify the server of backstab. | - |
| `public void ProcessBackstabFromServer( ulong damagedCharacterID, ulong characterCausingDamageID, string criticalDamageAnimation, int weaponID, float physicalDamage, float magicDamage, float fireDamage, float lightningDamage, float holyDamage, float poiseDamage)` | 777 | Thực hiện logic process backstab from server trong script CharacterNetworkManager. Liên kết trực tiếp: CharacterManager, TakeCriticalDamageEffect, WeaponItem, WorldCharacterEffectsManager, WorldItemDatabase +1. | CharacterManager, TakeCriticalDamageEffect, WeaponItem, WorldCharacterEffectsManager, WorldItemDatabase, WorldUtilityManager |
| `public void NotifyServerOfParryServerRpc(ulong parriedClientID)` | 815 | Gửi yêu cầu lên server trong Netcode để server xử lý notify server of parry. | - |
| `protected void NotifyServerOfParryClientRpc(ulong parriedClientID)` | 824 | Server gọi xuống client để đồng bộ hoặc phát hiệu ứng cho notify server of parry. | - |
| `protected void ProcessParryFromServer(ulong parriedClient)` | 829 | Thực hiện logic process parry from server trong script CharacterNetworkManager. Liên kết trực tiếp: CharacterManager. | CharacterManager |

#### CharacterSoundFXManager

- **Đường dẫn:** `Assets/Game/Scripts/Character/CharacterSoundFXManager.cs`
- **Loại:** MonoBehaviour
- **Vai trò dễ hiểu:** Base module dùng chung cho Character, được Player và AI kế thừa hoặc tham chiếu.
- **Kế thừa/cha:** MonoBehaviour
- **Script con:** AICharacterSoundFXManager, AIDurkSoundFXManager, AIMonster30SoundFXManager, PlayerSoundFXManager
- **Field public/serialized chính:** public AudioSource audioSource, [SerializeField] protected AudioClip[] damageGrunts, [SerializeField] protected AudioClip[] attackGrunts, [SerializeField] protected AudioClip[] footSteps
- **Liên kết script:** GameSettingsManager, WorldSoundFXManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `protected virtual void Awake()` | 18 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. | - |
| `protected virtual void Start()` | 24 | Chạy sau Awake để hoàn tất thiết lập ban đầu, nhất là khi cần các object khác đã sẵn sàng. | - |
| `public void PlaySoundFX(AudioClip soundFX, float volume = 1, bool randomizePitch = true, float pitchRandom = 0.1f)` | 29 | Phát sound fx, thường là animation, sound hoặc VFX. | - |
| `public void PlayRollSoundFX()` | 41 | Phát roll sound fx, thường là animation, sound hoặc VFX. Liên kết trực tiếp: WorldSoundFXManager. | WorldSoundFXManager |
| `public virtual void PlayDamageGruntSoundFX()` | 46 | Phát damage grunt sound fx, thường là animation, sound hoặc VFX. Liên kết trực tiếp: WorldSoundFXManager. | WorldSoundFXManager |
| `public virtual void PlayAttackGruntSoundFX()` | 52 | Phát attack grunt sound fx, thường là animation, sound hoặc VFX. Liên kết trực tiếp: WorldSoundFXManager. | WorldSoundFXManager |
| `public virtual void PlayFootStepSoundFX()` | 58 | Phát foot step sound fx, thường là animation, sound hoặc VFX. Liên kết trực tiếp: WorldSoundFXManager. | WorldSoundFXManager |
| `public virtual void PlayStanceBreakSoundFX()` | 64 | Phát stance break sound fx, thường là animation, sound hoặc VFX. Liên kết trực tiếp: WorldSoundFXManager. | WorldSoundFXManager |
| `public virtual void PlayCriticalStrikeSoundFX()` | 69 | Phát critical strike sound fx, thường là animation, sound hoặc VFX. Liên kết trực tiếp: WorldSoundFXManager. | WorldSoundFXManager |
| `public virtual void PlayBlockSoundFX()` | 74 | Phát block sound fx, thường là animation, sound hoặc VFX. | - |
| `public void RefreshAudioSettings()` | 79 | Làm mới dữ liệu/hiển thị audio settings. Liên kết trực tiếp: GameSettingsManager. | GameSettingsManager |

#### CharacterStatsManager

- **Đường dẫn:** `Assets/Game/Scripts/Character/CharacterStatsManager.cs`
- **Loại:** MonoBehaviour
- **Vai trò dễ hiểu:** Base module dùng chung cho Character, được Player và AI kế thừa hoặc tham chiếu.
- **Kế thừa/cha:** MonoBehaviour
- **Script con:** PlayerStatsManager
- **Field public/serialized chính:** public int runesDroppedOnDeath, public float blockingPhysicalAbsorption, public float blockingFireAbsorption, public float blockingLightningAbsorption, public float blockingMagicAbsorption, public float blockingHolyAbsorption, public float blockingStability, public float armorPhysicalDamageAbsorption, public float armorMagicDamageAbsorption, public float armorFireDamageAbsorption, public float armorLightningDamageAbsorption, public float armorHolyDamageAbsorption +9
- **Liên kết script:** BuildUp, BuildUpEffect, CharacterManager, PlayerUIManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `protected virtual void Awake()` | 46 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. Liên kết trực tiếp: CharacterManager. | CharacterManager |
| `protected virtual void Start()` | 51 | Chạy sau Awake để hoàn tất thiết lập ban đầu, nhất là khi cần các object khác đã sẵn sàng. | - |
| `protected virtual void Update()` | 56 | Cập nhật logic mỗi frame như input, timer, trạng thái chiến đấu hoặc UI. | - |
| `public int CalculateHealthBasedOnVitalityLevel(int vitality)` | 61 | Tính toán health based on vitality level từ chỉ số hoặc dữ liệu hiện có. | - |
| `public int CalculateStaminaBasedOnEnduranceLevel(int endurance)` | 70 | Tính toán stamina based on endurance level từ chỉ số hoặc dữ liệu hiện có. | - |
| `public int CalculateFocusPointsBasedOnMindLevel(int mind)` | 79 | Tính toán focus points based on mind level từ chỉ số hoặc dữ liệu hiện có. | - |
| `public int CalculateCharacterLevelBasedOnAttributes(bool calculatedProjectedLevel = false)` | 86 | Tính toán character level based on attributes từ chỉ số hoặc dữ liệu hiện có. Liên kết trực tiếp: PlayerUIManager. | PlayerUIManager |
| `public int CalculateBuildUpCapacityBasedOnVitalityLevel(int vitality)` | 124 | Tính toán build up capacity based on vitality level từ chỉ số hoặc dữ liệu hiện có. | - |
| `public virtual void RegenerateStamina()` | 133 | Thực hiện logic regenerate stamina trong script CharacterStatsManager. | - |
| `protected virtual void HandlePoiseResetTimer()` | 164 | Xử lý luồng poise reset timer. | - |
| `public virtual void DegradeBuildUps(BuildUp buildUp, int amount, BuildUpEffect effect)` | 176 | Thực hiện logic degrade build ups trong script CharacterStatsManager. Liên kết trực tiếp: BuildUp. | BuildUp |

#### CharacterUIManager

- **Đường dẫn:** `Assets/Game/Scripts/Character/CharacterUIManager.cs`
- **Loại:** MonoBehaviour
- **Vai trò dễ hiểu:** Base module dùng chung cho Character, được Player và AI kế thừa hoặc tham chiếu.
- **Kế thừa/cha:** MonoBehaviour
- **Script con:** -
- **Field public/serialized chính:** public bool hasFloatingHPBar, public UI_Character_HP_Bar characterHPBar
- **Liên kết script:** UI_Character_HP_Bar

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public void OnHPChanged(int oldValue, int newValue)` | 11 | Thực hiện logic on hpchanged trong script CharacterUIManager. | - |
| `public void ResetCharacterHPBar()` | 20 | Đưa character hpbar về trạng thái mặc định. | - |

### Assets/Game/Scripts/Character/AI Character

#### AIActivationBeacon

- **Đường dẫn:** `Assets/Game/Scripts/Character/AI Character/AIActivationBeacon.cs`
- **Loại:** MonoBehaviour
- **Vai trò dễ hiểu:** Module enemy AI: quản lý hành vi, spawn, animation, locomotion, combat, inventory, sound hoặc network.
- **Kế thừa/cha:** MonoBehaviour
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** AICharacterManager, BeaconDetector, PlayerManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public void SetOwnerOfBeacon(AICharacterManager aiCharacter)` | 9 | Thiết lập giá trị hoặc trạng thái owner of beacon. | - |
| `public void ReactivateAICharacter(PlayerManager player)` | 14 | Thực hiện logic reactivate aicharacter trong script AIActivationBeacon. | - |
| `private void OnTriggerEnter(Collider other)` | 22 | Xử lý khi collider khác đi vào trigger của object này. Liên kết trực tiếp: BeaconDetector. | BeaconDetector |

#### AICharacterAnimatorManager

- **Đường dẫn:** `Assets/Game/Scripts/Character/AI Character/AICharacterAnimatorManager.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Module enemy AI: quản lý hành vi, spawn, animation, locomotion, combat, inventory, sound hoặc network.
- **Kế thừa/cha:** CharacterAnimatorManager
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** AICharacterManager, CharacterAnimatorManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `protected override void Awake()` | 9 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. Liên kết trực tiếp: AICharacterManager. | AICharacterManager |
| `private void OnAnimatorMove()` | 15 | Nhận root motion từ Animator để áp dụng movement/rotation theo animation. | - |
| `private Vector3 GetAnimationOrNavMeshDelta()` | 49 | Lấy dữ liệu animation or nav mesh delta cho hệ thống khác sử dụng. | - |

#### AICharacterCombatManager

- **Đường dẫn:** `Assets/Game/Scripts/Character/AI Character/AICharacterCombatManager.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Module enemy AI: quản lý hành vi, spawn, animation, locomotion, combat, inventory, sound hoặc network.
- **Kế thừa/cha:** CharacterCombatManager
- **Script con:** AIDurkCombatManager, AIKnightCombatManager, AIMonster30CombatManager, AIMonster33CombatManager, AITormentedSoulCombatManager, AIUndeadCombatManager
- **Field public/serialized chính:** [SerializeField] protected int baseDamage, [SerializeField] protected int basePoiseDamage, public float actionRecoveryTimer, public bool enablePivot, public bool canPerformCombo, public bool hasHitTargetDuringCombo, public float distanceFromTarget, public float viewableAngle, public Vector3 targetsDirection, public float minimumFOV, public float maximumFOV, public float attackRotationSpeed +3
- **Liên kết script:** AICharacterManager, CharacterCombatManager, CharacterGroup, CharacterManager, DamageIntensity, IdleStateMode, LockOnTransform, PlayerManager, WorldUtilityManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `protected override void Awake()` | 56 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. Liên kết trực tiếp: AICharacterManager, LockOnTransform. | AICharacterManager, LockOnTransform |
| `public virtual void ApplyProgressionDifficultyScaling(float damageMultiplier)` | 65 | Áp dụng progression difficulty scaling lên character/object mục tiêu. | - |
| `private void Update()` | 76 | Cập nhật logic mỗi frame như input, timer, trạng thái chiến đấu hoặc UI. | - |
| `public void AddPlayerToPlayersWithinRange(PlayerManager player)` | 81 | Thêm player to players within range vào danh sách, trạng thái hoặc dữ liệu. | - |
| `public void RemovePlayerFromPlayersWithinRange(PlayerManager player)` | 95 | Loại bỏ player from players within range khỏi danh sách, trạng thái hoặc dữ liệu. | - |
| `public void AwardRunesOnDeath(PlayerManager player)` | 109 | Thực hiện logic award runes on death trong script AICharacterCombatManager. Liên kết trực tiếp: CharacterGroup. | CharacterGroup |
| `private void HandleStanceBreak()` | 125 | Xử lý luồng stance break. Liên kết trực tiếp: DamageIntensity, WorldUtilityManager. | DamageIntensity, WorldUtilityManager |
| `public void DamageStance(int stanceDamage)` | 176 | Gây hoặc xử lý sát thương cho stance. | - |
| `public virtual void AlertCharacterToSound(Vector3 positionOfSound)` | 184 | Thực hiện logic alert character to sound trong script AICharacterCombatManager. Liên kết trực tiếp: IdleStateMode. | IdleStateMode |
| `public virtual void FindATargetViaLineOfSight(AICharacterManager aiCharacter)` | 211 | Tìm atarget via line of sight trong scene/danh sách dữ liệu. Liên kết trực tiếp: CharacterManager, WorldUtilityManager. | CharacterManager, WorldUtilityManager |
| `public virtual void PivotTowardsTarget(AICharacterManager aiCharacter)` | 272 | Thực hiện logic pivot towards target trong script AICharacterCombatManager. | - |
| `public virtual void PivotTowardsPosition(AICharacterManager aiCharacter, Vector3 position)` | 291 | Thực hiện logic pivot towards position trong script AICharacterCombatManager. Liên kết trực tiếp: WorldUtilityManager. | WorldUtilityManager |
| `public void RotateTowardsAgent(AICharacterManager aiCharacter)` | 313 | Thực hiện logic rotate towards agent trong script AICharacterCombatManager. | - |
| `public void RotateTowardsTargetWhilstAttacking(AICharacterManager aiCharacter)` | 321 | Thực hiện logic rotate towards target whilst attacking trong script AICharacterCombatManager. | - |
| `public void HandleActionRecovery(AICharacterManager aiCharacter)` | 344 | Xử lý luồng action recovery. | - |
| `public override void EnableCanDoCombo()` | 356 | Bật can do combo. | - |
| `public override void DisableCanDoCombo()` | 361 | Tắt can do combo. | - |
| `public virtual void PerformEvasion()` | 369 | Thực hiện logic perform evasion trong script AICharacterCombatManager. | - |
| `public virtual bool TryStartSpecialSkill()` | 385 | Thử thực hiện start special skill, thường có kiểm tra điều kiện trước khi chạy. | - |

#### AICharacterInventoryManager

- **Đường dẫn:** `Assets/Game/Scripts/Character/AI Character/AICharacterInventoryManager.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Module enemy AI: quản lý hành vi, spawn, animation, locomotion, combat, inventory, sound hoặc network.
- **Kế thừa/cha:** CharacterInventoryManager
- **Script con:** -
- **Field public/serialized chính:** public int dropItemChance
- **Liên kết script:** AIBossCharacterManager, AICharacterManager, CharacterInventoryManager, Item, PickUpItemInteractable, WorldItemDatabase

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `protected override void Awake()` | 13 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. Liên kết trực tiếp: AICharacterManager. | AICharacterManager |
| `public void DropItem()` | 20 | Thực hiện logic drop item trong script AICharacterInventoryManager. Liên kết trực tiếp: AIBossCharacterManager, Item, PickUpItemInteractable, WorldItemDatabase. | AIBossCharacterManager, Item, PickUpItemInteractable, WorldItemDatabase |

#### AICharacterLocomotionManager

- **Đường dẫn:** `Assets/Game/Scripts/Character/AI Character/AICharacterLocomotionManager.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Module enemy AI: quản lý hành vi, spawn, animation, locomotion, combat, inventory, sound hoặc network.
- **Kế thừa/cha:** CharacterLocomotionManager
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** AICharacterManager, CharacterLocomotionManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `protected override void Awake()` | 11 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. Liên kết trực tiếp: AICharacterManager. | AICharacterManager |
| `public void RotateTowardsAgent(AICharacterManager aiCharacter)` | 18 | Thực hiện logic rotate towards agent trong script AICharacterLocomotionManager. | - |
| `protected override void Update()` | 26 | Cập nhật logic mỗi frame như input, timer, trạng thái chiến đấu hoặc UI. | - |
| `void UpdateOwnerAnimatorMovementParameters()` | 41 | Cập nhật owner animator movement parameters theo trạng thái mới. | - |

#### AICharacterManager

- **Đường dẫn:** `Assets/Game/Scripts/Character/AI Character/AICharacterManager.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Module enemy AI: quản lý hành vi, spawn, animation, locomotion, combat, inventory, sound hoặc network.
- **Kế thừa/cha:** CharacterManager
- **Script con:** AIBossCharacterManager
- **Field public/serialized chính:** public string characterName, public AICharacterCombatManager aiCharacterCombatManager, public AICharacterNetworkManager aiCharacterNetworkManager, public AICharacterLocomotionManager aiCharacterLocomotionManager, public AICharacterInventoryManager aiCharacterInventoryManager, public AICharacterSoundFXManager aiCharacterSoundFXManager, public NavMeshAgent navMeshAgent, public AIState currentState, public bool hasManuallySwitchedState, public IdleState idle, public PursueTargetState pursueTarget, public CombatStanceState combatStance +2
- **Liên kết script:** AIActivationBeacon, AIBossCharacterManager, AICharacterCombatManager, AICharacterInventoryManager, AICharacterLocomotionManager, AICharacterNetworkManager, AICharacterSoundFXManager, AIState, AttackState, CharacterManager, CombatStanceState, DamageCollider, GameProgressionManager, IdleState, InvestigateSoundState, PlayerManager +3

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `protected override void Awake()` | 45 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. Liên kết trực tiếp: AICharacterCombatManager, AICharacterInventoryManager, AICharacterLocomotionManager, AICharacterNetworkManager, AICharacterSoundFXManager +1. | AICharacterCombatManager, AICharacterInventoryManager, AICharacterLocomotionManager, AICharacterNetworkManager, AICharacterSoundFXManager, DamageCollider |
| `protected override void Start()` | 59 | Chạy sau Awake để hoàn tất thiết lập ban đầu, nhất là khi cần các object khác đã sẵn sàng. | - |
| `public override void OnNetworkSpawn()` | 66 | Thiết lập dữ liệu và callback khi NetworkObject được spawn trong Netcode. Liên kết trực tiếp: WorldAIManager. | WorldAIManager |
| `public override void OnNetworkDespawn()` | 107 | Dọn dữ liệu/callback khi NetworkObject bị despawn khỏi Netcode. Liên kết trực tiếp: WorldAIManager. | WorldAIManager |
| `protected override void OnEnable()` | 123 | Đăng ký event/callback hoặc bật trạng thái khi component được enable. | - |
| `protected override void OnDisable()` | 131 | Gỡ event/callback hoặc dọn trạng thái khi component bị disable. | - |
| `public override void OnDestroy()` | 139 | Dọn đăng ký/event/tài nguyên khi object bị hủy. Liên kết trực tiếp: WorldAIManager. | WorldAIManager |
| `protected override void Update()` | 150 | Cập nhật logic mỗi frame như input, timer, trạng thái chiến đấu hoặc UI. | - |
| `public void ApplyDeadState(bool snapToFinalPose = false)` | 187 | Áp dụng dead state lên character/object mục tiêu. Liên kết trực tiếp: DamageCollider. | DamageCollider |
| `private void QueueLateJoinDeadPose()` | 236 | Thực hiện logic queue late join dead pose trong script AICharacterManager. | - |
| `private IEnumerator ApplyLateJoinDeadPoseRoutine()` | 244 | Áp dụng late join dead pose routine lên character/object mục tiêu. | - |
| `public void BeginCoopDeathDespawn(float delay = 1.5f)` | 255 | Thực hiện logic begin coop death despawn trong script AICharacterManager. Liên kết trực tiếp: AIBossCharacterManager. | AIBossCharacterManager |
| `private IEnumerator CoopDeathDespawnRoutine(float delay)` | 269 | Thực hiện logic coop death despawn routine trong script AICharacterManager. Liên kết trực tiếp: WorldAIManager. | WorldAIManager |
| `new WaitForSeconds(delay)` | 271 | Thực hiện logic wait for seconds trong script AICharacterManager. | - |
| `public void RegisterLastPlayerWhoDealtDamage(PlayerManager player)` | 282 | Thực hiện logic register last player who dealt damage trong script AICharacterManager. | - |
| `public ulong GetLastPlayerWhoDealtDamageClientId()` | 290 | Lấy dữ liệu last player who dealt damage client id cho hệ thống khác sử dụng. | - |
| `public void ClearLastPlayerWhoDealtDamage()` | 295 | Thực hiện logic clear last player who dealt damage trong script AICharacterManager. | - |
| `private void ProcessStateMachine()` | 300 | Thực hiện logic process state machine trong script AICharacterManager. Liên kết trực tiếp: AIState, WorldUtilityManager. | AIState, WorldUtilityManager |
| `public virtual void ActivateCharacter(PlayerManager player)` | 343 | Thực hiện logic activate character trong script AICharacterManager. | - |
| `public virtual void DeactivateCharacter(PlayerManager player)` | 365 | Thực hiện logic deactivate character trong script AICharacterManager. | - |
| `public void CreateActivationBeacon()` | 394 | Tạo object/dữ liệu activation beacon. Liên kết trực tiếp: AIActivationBeacon, WorldAIManager. | AIActivationBeacon, WorldAIManager |
| `protected virtual void ApplyProgressionDifficultyScaling()` | 411 | Áp dụng progression difficulty scaling lên character/object mục tiêu. Liên kết trực tiếp: AIBossCharacterManager, GameProgressionManager. | AIBossCharacterManager, GameProgressionManager |
| `public bool ShouldUseNavMeshTranslationForInPlaceAnimations()` | 430 | Thực hiện logic should use nav mesh translation for in place animations trong script AICharacterManager. | - |

#### AICharacterNetworkManager

- **Đường dẫn:** `Assets/Game/Scripts/Character/AI Character/AICharacterNetworkManager.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Module enemy AI: quản lý hành vi, spawn, animation, locomotion, combat, inventory, sound hoặc network.
- **Kế thừa/cha:** CharacterNetworkManager
- **Script con:** AIBossCharacterNetworkManager, AIMonster33BossCharacterNetworkManager
- **Field public/serialized chính:** public NetworkVariable<bool> isAwake, public NetworkVariable<FixedString64Bytes> sleepingAnimation, public NetworkVariable<FixedString64Bytes> wakingAnimation
- **Liên kết script:** AIBossCharacterManager, AICharacterManager, CharacterNetworkManager, PlayerUIManager, WorldGameSessionManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `protected override void Awake()` | 25 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. Liên kết trực tiếp: AICharacterManager. | AICharacterManager |
| `public override void OnIsDeadChanged(bool oldStatus, bool newStatus)` | 31 | Thực hiện logic on is dead changed trong script AICharacterNetworkManager. Liên kết trực tiếp: AIBossCharacterManager, PlayerUIManager, WorldGameSessionManager. | AIBossCharacterManager, PlayerUIManager, WorldGameSessionManager |
| `public override void OnLockOnTargetIDChange(ulong oldID, ulong newID)` | 52 | Thực hiện logic on lock on target idchange trong script AICharacterNetworkManager. | - |

#### AICharacterSoundFXManager

- **Đường dẫn:** `Assets/Game/Scripts/Character/AI Character/AICharacterSoundFXManager.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Module enemy AI: quản lý hành vi, spawn, animation, locomotion, combat, inventory, sound hoặc network.
- **Kế thừa/cha:** CharacterSoundFXManager
- **Script con:** -
- **Field public/serialized chính:** public CharacterDialogueID characterDialogueID, public GameObject interactableDialogueCollider, public CharacterDialogue currentDialogue, public GameObject interactableDialogueObject, public bool dialogueIsPlaying
- **Liên kết script:** AICharacterManager, CharacterDialogue, CharacterDialogueID, CharacterSoundFXManager, PlayerUIManager, WorldAIManager, WorldSaveGameManager, WorldSoundFXManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `protected override void Awake()` | 20 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. Liên kết trực tiếp: AICharacterManager. | AICharacterManager |
| `protected override void Start()` | 27 | Chạy sau Awake để hoàn tất thiết lập ban đầu, nhất là khi cần các object khác đã sẵn sàng. Liên kết trực tiếp: CharacterDialogueID, WorldAIManager, WorldSaveGameManager. | CharacterDialogueID, WorldAIManager, WorldSaveGameManager |
| `public override void PlayBlockSoundFX()` | 42 | Phát block sound fx, thường là animation, sound hoặc VFX. Liên kết trực tiếp: WorldSoundFXManager. | WorldSoundFXManager |
| `public void PlayCurrentDialogueEvent()` | 51 | Phát current dialogue event, thường là animation, sound hoặc VFX. Liên kết trực tiếp: PlayerUIManager. | PlayerUIManager |
| `public void CancelCurrentDialogueEvent()` | 67 | Kiểm tra có được phép cel current dialogue event hay không. Liên kết trực tiếp: PlayerUIManager. | PlayerUIManager |
| `public void OnCurrentDialogueEnded()` | 77 | Thực hiện logic on current dialogue ended trong script AICharacterSoundFXManager. Liên kết trực tiếp: WorldSaveGameManager. | WorldSaveGameManager |

#### AICharacterSpawner

- **Đường dẫn:** `Assets/Game/Scripts/Character/AI Character/AICharacterSpawner.cs`
- **Loại:** MonoBehaviour
- **Vai trò dễ hiểu:** Module enemy AI: quản lý hành vi, spawn, animation, locomotion, combat, inventory, sound hoặc network.
- **Kế thừa/cha:** MonoBehaviour
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** AIBossCharacterManager, AICharacterManager, WorldAIManager, WorldBossCatalog

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `private void Awake()` | 34 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. | - |
| `private void Start()` | 39 | Chạy sau Awake để hoàn tất thiết lập ban đầu, nhất là khi cần các object khác đã sẵn sàng. Liên kết trực tiếp: WorldAIManager. | WorldAIManager |
| `private void OnValidate()` | 47 | Tự kiểm tra/cập nhật giá trị trong Unity Editor khi inspector thay đổi. | - |
| `private void HideSpawnerVisuals()` | 56 | Thực hiện logic hide spawner visuals trong script AICharacterSpawner. | - |
| `private void AutoAssignBossPrefabForCurrentScene()` | 73 | Thực hiện logic auto assign boss prefab for current scene trong script AICharacterSpawner. Liên kết trực tiếp: WorldBossCatalog. | WorldBossCatalog |
| `private bool ShouldResolveBossPrefabFromScene()` | 94 | Thực hiện logic should resolve boss prefab from scene trong script AICharacterSpawner. Liên kết trực tiếp: AIBossCharacterManager. | AIBossCharacterManager |
| `public void AttemptToSpawnCharacter()` | 108 | Cố gắng kích hoạt to spawn character nếu trạng thái hiện tại cho phép. Liên kết trực tiếp: AIBossCharacterManager, AICharacterManager, WorldAIManager. | AIBossCharacterManager, AICharacterManager, WorldAIManager |
| `public void ResetCharacter()` | 145 | Đưa character về trạng thái mặc định. Liên kết trực tiếp: AIBossCharacterManager. | AIBossCharacterManager |

#### AIForceAnimatorBool

- **Đường dẫn:** `Assets/Game/Scripts/Character/AI Character/AIForceAnimatorBool.cs`
- **Loại:** MonoBehaviour
- **Vai trò dễ hiểu:** Module enemy AI: quản lý hành vi, spawn, animation, locomotion, combat, inventory, sound hoặc network.
- **Kế thừa/cha:** MonoBehaviour
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** -

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `void Awake()` | 12 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. | - |
| `void OnEnable()` | 18 | Đăng ký event/callback hoặc bật trạng thái khi component được enable. | - |
| `void Start()` | 23 | Chạy sau Awake để hoàn tất thiết lập ban đầu, nhất là khi cần các object khác đã sẵn sàng. | - |
| `void ApplyValue()` | 28 | Áp dụng value lên character/object mục tiêu. | - |

#### AIPatrolPath

- **Đường dẫn:** `Assets/Game/Scripts/Character/AI Character/AIPatrolPath.cs`
- **Loại:** MonoBehaviour
- **Vai trò dễ hiểu:** Module enemy AI: quản lý hành vi, spawn, animation, locomotion, combat, inventory, sound hoặc network.
- **Kế thừa/cha:** MonoBehaviour
- **Script con:** -
- **Field public/serialized chính:** public int patrolPathID, public List<Vector3> patrolPoints
- **Liên kết script:** WorldAIManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `private void Awake()` | 13 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. | - |
| `private void OnEnable()` | 18 | Đăng ký event/callback hoặc bật trạng thái khi component được enable. | - |
| `private void OnDisable()` | 23 | Gỡ event/callback hoặc dọn trạng thái khi component bị disable. | - |
| `private void CachePatrolPoints()` | 32 | Thực hiện logic cache patrol points trong script AIPatrolPath. | - |
| `private void TryRegisterWithWorldAIManager()` | 42 | Thử thực hiện register with world aimanager, thường có kiểm tra điều kiện trước khi chạy. Liên kết trực tiếp: WorldAIManager. | WorldAIManager |
| `private IEnumerator RegisterWhenWorldAIManagerIsReady()` | 54 | Thực hiện logic register when world aimanager is ready trong script AIPatrolPath. Liên kết trực tiếp: WorldAIManager. | WorldAIManager |

### Assets/Game/Scripts/Character/AI Character/Actions

#### AICharacterAttackAction

- **Đường dẫn:** `Assets/Game/Scripts/Character/AI Character/Actions/AICharacterAttackAction.cs`
- **Loại:** ScriptableObject
- **Vai trò dễ hiểu:** Module enemy AI: quản lý hành vi, spawn, animation, locomotion, combat, inventory, sound hoặc network.
- **Kế thừa/cha:** ScriptableObject
- **Script con:** -
- **Field public/serialized chính:** [SerializeField] private string attackAnimation, public AICharacterAttackAction comboAction, public int attackWeight, public float actionRecoveryTime, public float minimumAttackAngle, public float maximumAttackAngle, public float minimumAttackDistance, public float maximumAttackDistance
- **Liên kết script:** AICharacterManager, AttackType

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public void AttemptToPerformAction(AICharacterManager aiCharacter)` | 24 | Cố gắng kích hoạt to perform action nếu trạng thái hiện tại cho phép. | - |

### Assets/Game/Scripts/Character/AI Character/Boss Character

#### AIBossCharacterManager

- **Đường dẫn:** `Assets/Game/Scripts/Character/AI Character/Boss Character/AIBossCharacterManager.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Module boss AI: quản lý boss, combat, network, phase, skill, collider hoặc âm thanh riêng của boss.
- **Kế thừa/cha:** AICharacterManager
- **Script con:** AIDurkCharacterManager, AIKnightBossCharacterManager, AIMonster30CharacterManager, AIMonster33CharacterManager, AITormentedSoulBossCharacterManager
- **Field public/serialized chính:** public int bossID, public NetworkVariable<bool> bossFightIsActive, public NetworkVariable<bool> hasBeenAwakened, public NetworkVariable<bool> hasBeenDefeated, public float minimumHealthPercentageToShift, public BossSleepState sleepState
- **Liên kết script:** AICharacterManager, BossSleepState, CombatStanceState, FogWallInteractable, GameProgressionManager, PlayerManager, PlayerUIManager, UI_Boss_HP_Bar, WorldAIManager, WorldGameSessionManager, WorldObjectManager, WorldSaveGameManager, WorldSoundFXManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `protected override void Awake()` | 45 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. | - |
| `private void OnValidate()` | 51 | Tự kiểm tra/cập nhật giá trị trong Unity Editor khi inspector thay đổi. | - |
| `private void AutoAssignBossIDFromWorldScene()` | 56 | Thực hiện logic auto assign boss idfrom world scene trong script AIBossCharacterManager. | - |
| `public override void OnNetworkSpawn()` | 66 | Thiết lập dữ liệu và callback khi NetworkObject được spawn trong Netcode. Liên kết trực tiếp: WorldSaveGameManager. | WorldSaveGameManager |
| `public override void OnNetworkDespawn()` | 107 | Dọn dữ liệu/callback khi NetworkObject bị despawn khỏi Netcode. | - |
| `private IEnumerator GetFogWallsFromWorldObjectManager()` | 114 | Lấy dữ liệu fog walls from world object manager cho hệ thống khác sử dụng. Liên kết trực tiếp: WorldObjectManager. | WorldObjectManager |
| `new WaitForEndOfFrame()` | 117 | Thực hiện logic wait for end of frame trong script AIBossCharacterManager. | - |
| `private void RefreshFogWallsFromWorldObjectManager()` | 122 | Làm mới dữ liệu/hiển thị fog walls from world object manager. Liên kết trực tiếp: FogWallInteractable, WorldObjectManager. | FogWallInteractable, WorldObjectManager |
| `private IEnumerator InitializeBossWorldState()` | 136 | Thực hiện logic initialize boss world state trong script AIBossCharacterManager. | - |
| `return StartCoroutine(GetFogWallsFromWorldObjectManager())` | 138 | Thực hiện logic start coroutine trong script AIBossCharacterManager. | - |
| `private IEnumerator AutoWakeWhenReady()` | 142 | Thực hiện logic auto wake when ready trong script AIBossCharacterManager. | - |
| `private void ApplyBossWorldState()` | 155 | Áp dụng boss world state lên character/object mục tiêu. | - |
| `public override IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false)` | 176 | Thực hiện logic process death event trong script AIBossCharacterManager. Liên kết trực tiếp: GameProgressionManager, PlayerManager, PlayerUIManager, WorldGameSessionManager, WorldSaveGameManager. | GameProgressionManager, PlayerManager, PlayerUIManager, WorldGameSessionManager, WorldSaveGameManager |
| `private void RequestProcessBossDeathServerRpc(bool manuallySelectDeathAnimation)` | 357 | Gửi yêu cầu lên server trong Netcode để server xử lý request process boss death. | - |
| `private void BeginBossDefeatCleanup(float delay = 1.5f)` | 366 | Thực hiện logic begin boss defeat cleanup trong script AIBossCharacterManager. | - |
| `private IEnumerator BossDefeatCleanupCoroutine(float delay)` | 377 | Thực hiện logic boss defeat cleanup coroutine trong script AIBossCharacterManager. Liên kết trực tiếp: WorldAIManager. | WorldAIManager |
| `new WaitForSeconds(delay)` | 379 | Thực hiện logic wait for seconds trong script AIBossCharacterManager. | - |
| `public void WakeBoss()` | 401 | Thực hiện logic wake boss trong script AIBossCharacterManager. Liên kết trực tiếp: WorldSaveGameManager. | WorldSaveGameManager |
| `public void RequestWakeBossServerRpc()` | 438 | Gửi yêu cầu lên server trong Netcode để server xử lý request wake boss. | - |
| `private void BroadcastVictoryAchievedClientRpc(bool canContinueProgression, float delay)` | 445 | Server gọi xuống client để đồng bộ hoặc phát hiệu ứng cho broadcast victory achieved. Liên kết trực tiếp: PlayerUIManager, WorldGameSessionManager. | PlayerUIManager, WorldGameSessionManager |
| `private void OnBossFightIsActiveChanged(bool oldStatus, bool newStatus)` | 457 | Thực hiện logic on boss fight is active changed trong script AIBossCharacterManager. Liên kết trực tiếp: PlayerUIManager, UI_Boss_HP_Bar, WorldSoundFXManager. | PlayerUIManager, UI_Boss_HP_Bar, WorldSoundFXManager |
| `public void PhaseShift()` | 477 | Thực hiện logic phase shift trong script AIBossCharacterManager. | - |
| `public override void ActivateCharacter(PlayerManager player)` | 484 | Thực hiện logic activate character trong script AIBossCharacterManager. | - |

#### AIBossCharacterNetworkManager

- **Đường dẫn:** `Assets/Game/Scripts/Character/AI Character/Boss Character/AIBossCharacterNetworkManager.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Module boss AI: quản lý boss, combat, network, phase, skill, collider hoặc âm thanh riêng của boss.
- **Kế thừa/cha:** AICharacterNetworkManager
- **Script con:** AIKnightBossNetworkManager, AITormentedSoulBossNetworkManager
- **Field public/serialized chính:** -
- **Liên kết script:** AIBossCharacterManager, AICharacterNetworkManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `protected override void Awake()` | 9 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. Liên kết trực tiếp: AIBossCharacterManager. | AIBossCharacterManager |
| `public override void OnHPChanged(int oldValue, int newValue)` | 16 | Thực hiện logic on hpchanged trong script AIBossCharacterNetworkManager. | - |
| `public override void OnIsActiveChanged(bool oldStatus, bool newStatus)` | 46 | Thực hiện logic on is active changed trong script AIBossCharacterNetworkManager. | - |

#### AIDurkCharacterManager

- **Đường dẫn:** `Assets/Game/Scripts/Character/AI Character/Boss Character/AIDurkCharacterManager.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Module boss AI: quản lý boss, combat, network, phase, skill, collider hoặc âm thanh riêng của boss.
- **Kế thừa/cha:** AIBossCharacterManager
- **Script con:** -
- **Field public/serialized chính:** public AIDurkSoundFXManager durkSoundFXManager, public AIDurkCombatManager durkCombatManager
- **Liên kết script:** AIBossCharacterManager, AIDurkCombatManager, AIDurkSoundFXManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `protected override void Awake()` | 11 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. Liên kết trực tiếp: AIDurkCombatManager, AIDurkSoundFXManager. | AIDurkCombatManager, AIDurkSoundFXManager |

#### AIDurkCombatManager

- **Đường dẫn:** `Assets/Game/Scripts/Character/AI Character/Boss Character/AIDurkCombatManager.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Module boss AI: quản lý boss, combat, network, phase, skill, collider hoặc âm thanh riêng của boss.
- **Kế thừa/cha:** AICharacterCombatManager
- **Script con:** -
- **Field public/serialized chính:** public float stompAttackAOERadius, public float stompDamage, public GameObject durkImpactVFX
- **Liên kết script:** AICharacterCombatManager, AICharacterManager, AIDurkCharacterManager, DurkClubDamageCollider, DurkStompCollider, WorldSoundFXManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `protected override void Awake()` | 25 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. Liên kết trực tiếp: AIDurkCharacterManager. | AIDurkCharacterManager |
| `public void SetAttack01Damage()` | 32 | Thiết lập giá trị hoặc trạng thái attack01 damage. | - |
| `public void SetAttack02Damage()` | 39 | Thiết lập giá trị hoặc trạng thái attack02 damage. | - |
| `public void SetAttack03Damage()` | 46 | Thiết lập giá trị hoặc trạng thái attack03 damage. | - |
| `public void OpenClubDamageCollider()` | 53 | Mở UI/trạng thái/luồng club damage collider. Liên kết trực tiếp: WorldSoundFXManager. | WorldSoundFXManager |
| `public void CloseClubDamageCollider()` | 59 | Đóng UI/trạng thái/luồng club damage collider. | - |
| `public void ActivateDurkStomp()` | 64 | Thực hiện logic activate durk stomp trong script AIDurkCombatManager. | - |
| `public override void PivotTowardsTarget(AICharacterManager aiCharacter)` | 69 | Thực hiện logic pivot towards target trong script AIDurkCombatManager. | - |

#### AIDurkSoundFXManager

- **Đường dẫn:** `Assets/Game/Scripts/Character/AI Character/Boss Character/AIDurkSoundFXManager.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Module boss AI: quản lý boss, combat, network, phase, skill, collider hoặc âm thanh riêng của boss.
- **Kế thừa/cha:** CharacterSoundFXManager
- **Script con:** -
- **Field public/serialized chính:** public AudioClip[] clubWhooshes, public AudioClip[] clubImpacts, public AudioClip[] stompImpacts
- **Liên kết script:** CharacterSoundFXManager, WorldSoundFXManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public virtual void PlayClubImpactSoundFX()` | 17 | Phát club impact sound fx, thường là animation, sound hoặc VFX. Liên kết trực tiếp: WorldSoundFXManager. | WorldSoundFXManager |
| `public virtual void PlayStompImpactSoundFX()` | 23 | Phát stomp impact sound fx, thường là animation, sound hoặc VFX. Liên kết trực tiếp: WorldSoundFXManager. | WorldSoundFXManager |

#### AIKnightBossCharacterManager

- **Đường dẫn:** `Assets/Game/Scripts/Character/AI Character/Boss Character/AIKnightBossCharacterManager.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Module boss AI: quản lý boss, combat, network, phase, skill, collider hoặc âm thanh riêng của boss.
- **Kế thừa/cha:** AIBossCharacterManager
- **Script con:** -
- **Field public/serialized chính:** public AIKnightCombatManager knightCombatManager, public TwinMoonSkill twinMoonSkill
- **Liên kết script:** AIBossCharacterManager, AIKnightCombatManager, TwinMoonSkill

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `protected override void Awake()` | 10 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. Liên kết trực tiếp: AIKnightCombatManager, TwinMoonSkill. | AIKnightCombatManager, TwinMoonSkill |
| `public override void OnNetworkSpawn()` | 18 | Thiết lập dữ liệu và callback khi NetworkObject được spawn trong Netcode. | - |
| `public void ForceEndCurrentAction()` | 29 | Thực hiện logic force end current action trong script AIKnightBossCharacterManager. | - |

#### AIKnightBossNetworkManager

- **Đường dẫn:** `Assets/Game/Scripts/Character/AI Character/Boss Character/AIKnightBossNetworkManager.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Module boss AI: quản lý boss, combat, network, phase, skill, collider hoặc âm thanh riêng của boss.
- **Kế thừa/cha:** AIBossCharacterNetworkManager
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** AIBossCharacterNetworkManager, AIKnightBossCharacterManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `protected override void Awake()` | 9 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. Liên kết trực tiếp: AIKnightBossCharacterManager. | AIKnightBossCharacterManager |
| `public override void OnHPChanged(int oldValue, int newValue)` | 15 | Thực hiện logic on hpchanged trong script AIKnightBossNetworkManager. | - |
| `public override void OnIsActiveChanged(bool oldStatus, bool newStatus)` | 28 | Thực hiện logic on is active changed trong script AIKnightBossNetworkManager. | - |

#### AIMonster30CharacterManager

- **Đường dẫn:** `Assets/Game/Scripts/Character/AI Character/Boss Character/AIMonster30CharacterManager.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Module boss AI: quản lý boss, combat, network, phase, skill, collider hoặc âm thanh riêng của boss.
- **Kế thừa/cha:** AIBossCharacterManager
- **Script con:** -
- **Field public/serialized chính:** public AIMonster30CombatManager monster30CombatManager
- **Liên kết script:** AIBossCharacterManager, AIMonster30CombatManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `protected override void Awake()` | 9 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. Liên kết trực tiếp: AIMonster30CombatManager. | AIMonster30CombatManager |
| `public override void OnNetworkSpawn()` | 16 | Thiết lập dữ liệu và callback khi NetworkObject được spawn trong Netcode. | - |
| `public void ForceEndCurrentAction()` | 27 | Thực hiện logic force end current action trong script AIMonster30CharacterManager. | - |

#### AIMonster30CombatManager

- **Đường dẫn:** `Assets/Game/Scripts/Character/AI Character/Boss Character/AIMonster30CombatManager.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Module boss AI: quản lý boss, combat, network, phase, skill, collider hoặc âm thanh riêng của boss.
- **Kế thừa/cha:** AICharacterCombatManager
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** AICharacterCombatManager, AICharacterManager, ManualDamageCollider

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `protected override void Awake()` | 16 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. | - |
| `void OnValidate()` | 22 | Tự kiểm tra/cập nhật giá trị trong Unity Editor khi inspector thay đổi. | - |
| `public void SetAttack01Damage()` | 29 | Thiết lập giá trị hoặc trạng thái attack01 damage. | - |
| `public void SetAttack02Damage()` | 37 | Thiết lập giá trị hoặc trạng thái attack02 damage. | - |
| `public void SetAttack03Damage()` | 45 | Thiết lập giá trị hoặc trạng thái attack03 damage. | - |
| `public void OpenRightHandDamageCollider()` | 53 | Mở UI/trạng thái/luồng right hand damage collider. | - |
| `public void CloseRightHandDamageCollider()` | 59 | Đóng UI/trạng thái/luồng right hand damage collider. | - |
| `public void OpenLeftHandDamageCollider()` | 64 | Mở UI/trạng thái/luồng left hand damage collider. | - |
| `public void CloseLeftHandDamageCollider()` | 70 | Đóng UI/trạng thái/luồng left hand damage collider. | - |
| `public override void CloseAllDamageColliders()` | 75 | Đóng UI/trạng thái/luồng all damage colliders. | - |
| `public override void PivotTowardsTarget(AICharacterManager aiCharacter)` | 83 | Thực hiện logic pivot towards target trong script AIMonster30CombatManager. | - |
| `private void ApplyDamage(ManualDamageCollider collider, float modifier)` | 102 | Áp dụng damage lên character/object mục tiêu. | - |
| `private void ResolveDamageColliders()` | 111 | Thực hiện logic resolve damage colliders trong script AIMonster30CombatManager. | - |
| `private ManualDamageCollider FindColliderByObjectName(string objectName)` | 122 | Tìm collider by object name trong scene/danh sách dữ liệu. Liên kết trực tiếp: ManualDamageCollider. | ManualDamageCollider |

#### AIMonster30SoundFXManager

- **Đường dẫn:** `Assets/Game/Scripts/Character/AI Character/Boss Character/AIMonster30SoundFXManager.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Module boss AI: quản lý boss, combat, network, phase, skill, collider hoặc âm thanh riêng của boss.
- **Kế thừa/cha:** CharacterSoundFXManager
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** CharacterSoundFXManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| - | - | Script này chủ yếu là dữ liệu/enum/struct, không có method rõ ràng. | - |

#### AIMonster33BossCharacterNetworkManager

- **Đường dẫn:** `Assets/Game/Scripts/Character/AI Character/Boss Character/AIMonster33BossCharacterNetworkManager.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Module boss AI: quản lý boss, combat, network, phase, skill, collider hoặc âm thanh riêng của boss.
- **Kế thừa/cha:** AICharacterNetworkManager
- **Script con:** -
- **Field public/serialized chính:** public NetworkVariable<bool> isPowerUpPhaseActive
- **Liên kết script:** AICharacterNetworkManager, AIMonster33CharacterManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `protected override void Awake()` | 15 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. Liên kết trực tiếp: AIMonster33CharacterManager. | AIMonster33CharacterManager |
| `public override void OnHPChanged(int oldValue, int newValue)` | 22 | Thực hiện logic on hpchanged trong script AIMonster33BossCharacterNetworkManager. | - |
| `public override void OnIsActiveChanged(bool oldStatus, bool newStatus)` | 40 | Thực hiện logic on is active changed trong script AIMonster33BossCharacterNetworkManager. | - |
| `public void ActivatePowerUpPhaseFXClientRpc()` | 58 | Server gọi xuống client để đồng bộ hoặc phát hiệu ứng cho activate power up phase fx. Liên kết trực tiếp: AIMonster33CharacterManager. | AIMonster33CharacterManager |

#### AIMonster33CharacterManager

- **Đường dẫn:** `Assets/Game/Scripts/Character/AI Character/Boss Character/AIMonster33CharacterManager.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Module boss AI: quản lý boss, combat, network, phase, skill, collider hoặc âm thanh riêng của boss.
- **Kế thừa/cha:** AIBossCharacterManager
- **Script con:** -
- **Field public/serialized chính:** public AIMonster33CombatManager monster33CombatManager, public Monster33Phase2FireController phase2FireController, public bool hasActivatedPowerUpPhase
- **Liên kết script:** AIBossCharacterManager, AIMonster33BossCharacterNetworkManager, AIMonster33CombatManager, Monster33Phase2FireController

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `protected override void Awake()` | 16 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. Liên kết trực tiếp: AIMonster33CombatManager, Monster33Phase2FireController. | AIMonster33CombatManager, Monster33Phase2FireController |
| `public override void OnNetworkSpawn()` | 24 | Thiết lập dữ liệu và callback khi NetworkObject được spawn trong Netcode. | - |
| `public void ForceEndCurrentAction()` | 35 | Thực hiện logic force end current action trong script AIMonster33CharacterManager. | - |
| `public bool TryActivatePowerUpPhase()` | 74 | Thử thực hiện activate power up phase, thường có kiểm tra điều kiện trước khi chạy. Liên kết trực tiếp: AIMonster33BossCharacterNetworkManager. | AIMonster33BossCharacterNetworkManager |
| `private void RecoverFromPowerUpActionAfterAnimation()` | 95 | Thực hiện logic recover from power up action after animation trong script AIMonster33CharacterManager. | - |
| `private IEnumerator RecoverFromPowerUpAction()` | 103 | Thực hiện logic recover from power up action trong script AIMonster33CharacterManager. | - |
| `new WaitForSeconds(powerUpActionRecoveryDelay)` | 105 | Thực hiện logic wait for seconds trong script AIMonster33CharacterManager. | - |

#### AIMonster33CombatManager

- **Đường dẫn:** `Assets/Game/Scripts/Character/AI Character/Boss Character/AIMonster33CombatManager.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Module boss AI: quản lý boss, combat, network, phase, skill, collider hoặc âm thanh riêng của boss.
- **Kế thừa/cha:** AICharacterCombatManager
- **Script con:** -
- **Field public/serialized chính:** public bool IsPoweredUp
- **Liên kết script:** AICharacterCombatManager, AICharacterManager, ManualDamageCollider, Monster33FireDamageCollider

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `protected override void Awake()` | 21 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. | - |
| `void OnValidate()` | 27 | Tự kiểm tra/cập nhật giá trị trong Unity Editor khi inspector thay đổi. | - |
| `public void SetAttack01Damage()` | 34 | Thiết lập giá trị hoặc trạng thái attack01 damage. | - |
| `public void SetAttack02Damage()` | 42 | Thiết lập giá trị hoặc trạng thái attack02 damage. | - |
| `public void SetAttack03Damage()` | 50 | Thiết lập giá trị hoặc trạng thái attack03 damage. | - |
| `public void OpenRightHandDamageCollider()` | 58 | Mở UI/trạng thái/luồng right hand damage collider. | - |
| `public void CloseRightHandDamageCollider()` | 64 | Đóng UI/trạng thái/luồng right hand damage collider. | - |
| `public void OpenLeftHandDamageCollider()` | 69 | Mở UI/trạng thái/luồng left hand damage collider. | - |
| `public void CloseLeftHandDamageCollider()` | 75 | Đóng UI/trạng thái/luồng left hand damage collider. | - |
| `public override void CloseAllDamageColliders()` | 80 | Đóng UI/trạng thái/luồng all damage colliders. | - |
| `public override void PivotTowardsTarget(AICharacterManager aiCharacter)` | 88 | Thực hiện logic pivot towards target trong script AIMonster33CombatManager. | - |
| `public void ApplyPowerUpBuff()` | 107 | Áp dụng power up buff lên character/object mục tiêu. | - |
| `private void ApplyDamage(ManualDamageCollider collider, float modifier)` | 114 | Áp dụng damage lên character/object mục tiêu. | - |
| `private void SetFirePhaseOnCollider(ManualDamageCollider collider)` | 125 | Thiết lập giá trị hoặc trạng thái fire phase on collider. Liên kết trực tiếp: Monster33FireDamageCollider. | Monster33FireDamageCollider |
| `private void ImbueFireDamage(ManualDamageCollider collider)` | 131 | Thực hiện logic imbue fire damage trong script AIMonster33CombatManager. | - |
| `private void ResolveDamageColliders()` | 140 | Thực hiện logic resolve damage colliders trong script AIMonster33CombatManager. | - |
| `private ManualDamageCollider FindColliderByObjectName(string objectName)` | 151 | Tìm collider by object name trong scene/danh sách dữ liệu. Liên kết trực tiếp: ManualDamageCollider. | ManualDamageCollider |

#### AITormentedSoulBossCharacterManager

- **Đường dẫn:** `Assets/Game/Scripts/Character/AI Character/Boss Character/AITormentedSoulBossCharacterManager.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Module boss AI: quản lý boss, combat, network, phase, skill, collider hoặc âm thanh riêng của boss.
- **Kế thừa/cha:** AIBossCharacterManager
- **Script con:** -
- **Field public/serialized chính:** public AITormentedSoulCombatManager tormentedSoulCombatManager, public DeathMoonSlash deathMoonSlash
- **Liên kết script:** AIBossCharacterManager, AITormentedSoulCombatManager, DeathMoonSlash

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `protected override void Awake()` | 10 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. Liên kết trực tiếp: AITormentedSoulCombatManager, DeathMoonSlash. | AITormentedSoulCombatManager, DeathMoonSlash |
| `public override void OnNetworkSpawn()` | 18 | Thiết lập dữ liệu và callback khi NetworkObject được spawn trong Netcode. | - |
| `public void ForceEndCurrentAction()` | 29 | Thực hiện logic force end current action trong script AITormentedSoulBossCharacterManager. | - |

#### AITormentedSoulBossNetworkManager

- **Đường dẫn:** `Assets/Game/Scripts/Character/AI Character/Boss Character/AITormentedSoulBossNetworkManager.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Module boss AI: quản lý boss, combat, network, phase, skill, collider hoặc âm thanh riêng của boss.
- **Kế thừa/cha:** AIBossCharacterNetworkManager
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** AIBossCharacterNetworkManager, AITormentedSoulBossCharacterManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `protected override void Awake()` | 9 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. Liên kết trực tiếp: AITormentedSoulBossCharacterManager. | AITormentedSoulBossCharacterManager |
| `public override void OnHPChanged(int oldValue, int newValue)` | 15 | Thực hiện logic on hpchanged trong script AITormentedSoulBossNetworkManager. | - |
| `public override void OnIsActiveChanged(bool oldStatus, bool newStatus)` | 28 | Thực hiện logic on is active changed trong script AITormentedSoulBossNetworkManager. | - |

#### AITormentedSoulCombatManager

- **Đường dẫn:** `Assets/Game/Scripts/Character/AI Character/Boss Character/AITormentedSoulCombatManager.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Module boss AI: quản lý boss, combat, network, phase, skill, collider hoặc âm thanh riêng của boss.
- **Kế thừa/cha:** AICharacterCombatManager
- **Script con:** -
- **Field public/serialized chính:** public ManualDamageCollider ScytheDamageCollider, public bool IsPoweredUp
- **Liên kết script:** AICharacterCombatManager, DeathCycloneSkill, DeathMoonSlash, ManualDamageCollider

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `protected override void Awake()` | 22 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. Liên kết trực tiếp: DeathCycloneSkill, DeathMoonSlash. | DeathCycloneSkill, DeathMoonSlash |
| `public void SetAttack01Damage()` | 29 | Thiết lập giá trị hoặc trạng thái attack01 damage. | - |
| `public void SetAttack02Damage()` | 36 | Thiết lập giá trị hoặc trạng thái attack02 damage. | - |
| `public void OpenScytheDamageCollider()` | 43 | Mở UI/trạng thái/luồng scythe damage collider. | - |
| `public void CloseScytheDamageCollider()` | 49 | Đóng UI/trạng thái/luồng scythe damage collider. | - |
| `public void OpenDamageCollider()` | 54 | Mở UI/trạng thái/luồng damage collider. | - |
| `public void CloseDamageCollider()` | 61 | Đóng UI/trạng thái/luồng damage collider. | - |
| `public void DrainStaminaBasedOnAttack()` | 66 | Thực hiện logic drain stamina based on attack trong script AITormentedSoulCombatManager. | - |
| `public void ActivatePowerUp(float damageMultiplierOverride = -1f)` | 70 | Thực hiện logic activate power up trong script AITormentedSoulCombatManager. | - |
| `public override bool TryStartSpecialSkill()` | 78 | Thử thực hiện start special skill, thường có kiểm tra điều kiện trước khi chạy. | - |
| `float GetCurrentDamageMultiplier()` | 104 | Lấy dữ liệu current damage multiplier cho hệ thống khác sử dụng. | - |
| `public override void CloseAllDamageColliders()` | 109 | Đóng UI/trạng thái/luồng all damage colliders. | - |

#### DeathCycloneSkill

- **Đường dẫn:** `Assets/Game/Scripts/Character/AI Character/Boss Character/DeathCycloneSkill.cs`
- **Loại:** NetworkBehaviour
- **Vai trò dễ hiểu:** Module boss AI: quản lý boss, combat, network, phase, skill, collider hoặc âm thanh riêng của boss.
- **Kế thừa/cha:** NetworkBehaviour
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** AICharacterManager, AITormentedSoulCombatManager, CharacterManager, PlayerCamera, WorldUtilityManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public bool TryActivateSkill()` | 61 | Thử thực hiện activate skill, thường có kiểm tra điều kiện trước khi chạy. Liên kết trực tiếp: CharacterManager. | CharacterManager |
| `void Awake()` | 102 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. Liên kết trực tiếp: AICharacterManager, AITormentedSoulCombatManager. | AICharacterManager, AITormentedSoulCombatManager |
| `void Update()` | 112 | Cập nhật logic mỗi frame như input, timer, trạng thái chiến đấu hoặc UI. | - |
| `void UpdateProximityTimer()` | 117 | Cập nhật proximity timer theo trạng thái mới. Liên kết trực tiếp: CharacterManager. | CharacterManager |
| `IEnumerator PerformDeathCyclone(Transform target)` | 141 | Thực hiện logic perform death cyclone trong script DeathCycloneSkill. | - |
| `return StartCoroutine(FaceTargetBeforeSpin(target))` | 147 | Thực hiện logic start coroutine trong script DeathCycloneSkill. | - |
| `IEnumerator FaceTargetBeforeSpin(Transform target)` | 180 | Thực hiện logic face target before spin trong script DeathCycloneSkill. | - |
| `void ApplyCycloneTick()` | 204 | Áp dụng cyclone tick lên character/object mục tiêu. Liên kết trực tiếp: CharacterManager, WorldUtilityManager. | CharacterManager, WorldUtilityManager |
| `void ApplyDamageToTarget(CharacterManager damageTarget, Collider hitCollider)` | 232 | Áp dụng damage to target lên character/object mục tiêu. | - |
| `void PullTargetTowardsBoss(CharacterManager damageTarget)` | 258 | Thực hiện logic pull target towards boss trong script DeathCycloneSkill. | - |
| `void ApplyPullToOwnerClientRpc(ulong targetNetworkObjectId, Vector3 bossPosition, float force)` | 289 | Server gọi xuống client để đồng bộ hoặc phát hiệu ứng cho apply pull to owner. Liên kết trực tiếp: CharacterManager. | CharacterManager |
| `void StopNavigation()` | 322 | Thực hiện logic stop navigation trong script DeathCycloneSkill. | - |
| `void HaltMomentum()` | 332 | Thực hiện logic halt momentum trong script DeathCycloneSkill. | - |
| `void SetSkillState(bool skillIsActive)` | 341 | Thiết lập giá trị hoặc trạng thái skill state. | - |
| `void ResetAfterSkill()` | 356 | Đưa after skill về trạng thái mặc định. | - |
| `void SpawnCycloneVFX()` | 364 | Spawn object/dữ liệu cyclone vfx. | - |
| `void DestroyCycloneVFX()` | 374 | Thực hiện logic destroy cyclone vfx trong script DeathCycloneSkill. | - |
| `void PlayCycloneAudio()` | 383 | Phát cyclone audio, thường là animation, sound hoặc VFX. | - |
| `void StopCycloneAudio()` | 393 | Thực hiện logic stop cyclone audio trong script DeathCycloneSkill. | - |
| `void TryPlayCameraShake(Vector3 impactPoint)` | 405 | Thử thực hiện play camera shake, thường có kiểm tra điều kiện trước khi chạy. Liên kết trực tiếp: PlayerCamera. | PlayerCamera |
| `IEnumerator DoCameraShake(Transform cameraPivot)` | 421 | Thực hiện logic do camera shake trong script DeathCycloneSkill. | - |
| `void OnDrawGizmosSelected()` | 440 | Thực hiện logic on draw gizmos selected trong script DeathCycloneSkill. | - |
| `new Color(0.55f, 0.1f, 0.75f, 0.35f)` | 445 | Thực hiện logic color trong script DeathCycloneSkill. | - |
| `new Color(0.75f, 0.3f, 1f, 0.85f)` | 447 | Thực hiện logic color trong script DeathCycloneSkill. | - |

#### DeathCycloneVFX

- **Đường dẫn:** `Assets/Game/Scripts/Character/AI Character/Boss Character/DeathCycloneVFX.cs`
- **Loại:** MonoBehaviour
- **Vai trò dễ hiểu:** Module boss AI: quản lý boss, combat, network, phase, skill, collider hoặc âm thanh riêng của boss.
- **Kế thừa/cha:** MonoBehaviour
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** -

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `new Color(0.52f, 0.15f, 0.82f, 0.85f)` | 7 | Thực hiện logic color trong script DeathCycloneVFX. | - |
| `new Color(0.22f, 0.38f, 0.86f, 0.75f)` | 8 | Thực hiện logic color trong script DeathCycloneVFX. | - |
| `void Awake()` | 23 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. | - |
| `void Update()` | 29 | Cập nhật logic mỗi frame như input, timer, trạng thái chiến đấu hoặc UI. | - |
| `void OnDestroy()` | 37 | Dọn đăng ký/event/tài nguyên khi object bị hủy. | - |
| `void CreateVFX()` | 43 | Tạo object/dữ liệu vfx. | - |
| `ParticleSystem CreateSwirl( string objectName, Color color, float radius, float emissionRate, float startSize, float startLifetime, float orbitalVelocity, float radialVelocity, float verticalHeight)` | 56 | Tạo object/dữ liệu swirl. | - |
| `new GameObject(objectName)` | 67 | Thực hiện logic game object trong script DeathCycloneVFX. | - |
| `new AnimationCurve( new Keyframe(0f, 0.2f), new Keyframe(0.35f, 1f), new Keyframe(1f, 0.15f)))` | 120 | Thực hiện logic animation curve trong script DeathCycloneVFX. | - |
| `ParticleSystem CreateDust(string objectName, Color color)` | 136 | Tạo object/dữ liệu dust. | - |
| `new GameObject(objectName)` | 138 | Thực hiện logic game object trong script DeathCycloneVFX. | - |
| `new Color(color.r, color.g, color.b, 0.4f)` | 161 | Thực hiện logic color trong script DeathCycloneVFX. | - |
| `Material GetParticleMaterial()` | 188 | Lấy dữ liệu particle material cho hệ thống khác sử dụng. | - |
| `new Material(shader)` | 201 | Thực hiện logic material trong script DeathCycloneVFX. | - |
| `Gradient BuildGradient(Color color, float alpha)` | 221 | Thực hiện logic build gradient trong script DeathCycloneVFX. | - |
| `new Gradient()` | 223 | Thực hiện logic gradient trong script DeathCycloneVFX. | - |

#### Monster30WeaponConstraintBootstrap

- **Đường dẫn:** `Assets/Game/Scripts/Character/AI Character/Boss Character/Monster30WeaponConstraintBootstrap.cs`
- **Loại:** MonoBehaviour
- **Vai trò dễ hiểu:** Module boss AI: quản lý boss, combat, network, phase, skill, collider hoặc âm thanh riêng của boss.
- **Kế thừa/cha:** MonoBehaviour
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** -

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `void Awake()` | 6 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. | - |
| `static void ApplyCurrentPoseAsConstraintOffset(ParentConstraint constraint)` | 21 | Áp dụng current pose as constraint offset lên character/object mục tiêu. | - |

#### Monster33FireDamageCollider

- **Đường dẫn:** `Assets/Game/Scripts/Character/AI Character/Boss Character/Monster33FireDamageCollider.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Module boss AI: quản lý boss, combat, network, phase, skill, collider hoặc âm thanh riêng của boss.
- **Kế thừa/cha:** ManualDamageCollider
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** CharacterManager, ManualDamageCollider

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public void SetFirePhaseActive(bool active)` | 10 | Thiết lập giá trị hoặc trạng thái fire phase active. | - |
| `public void ConfigureFireHit(GameObject burningHitVFXPrefab, int fireBuildUpAmount)` | 15 | Thực hiện logic configure fire hit trong script Monster33FireDamageCollider. | - |
| `protected override void DamageTarget(CharacterManager damageTarget)` | 20 | Gây hoặc xử lý sát thương cho target. | - |
| `private void SpawnBurningHitVFX()` | 34 | Spawn object/dữ liệu burning hit vfx. | - |

#### Monster33Phase2FireController

- **Đường dẫn:** `Assets/Game/Scripts/Character/AI Character/Boss Character/Monster33Phase2FireController.cs`
- **Loại:** MonoBehaviour
- **Vai trò dễ hiểu:** Module boss AI: quản lý boss, combat, network, phase, skill, collider hoặc âm thanh riêng của boss.
- **Kế thừa/cha:** MonoBehaviour
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** -

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `new Vector3(0f, 0.9f, 0f)` | 16 | Thực hiện logic vector3 trong script Monster33Phase2FireController. | - |
| `public void Configure( GameObject bodyFireVFXPrefab, GameObject weaponTrailPrefab, Material weaponFireMaterial, Transform visualRoot, Transform rightWeaponRoot, Transform leftWeaponRoot)` | 25 | Thực hiện logic configure trong script Monster33Phase2FireController. | - |
| `public void ActivateAfterPowerUpAnimation()` | 41 | Thực hiện logic activate after power up animation trong script Monster33Phase2FireController. | - |
| `public void ActivateNow()` | 52 | Thực hiện logic activate now trong script Monster33Phase2FireController. | - |
| `private IEnumerator ActivateAfterDelay()` | 63 | Thực hiện logic activate after delay trong script Monster33Phase2FireController. | - |
| `new WaitForSeconds(activationDelay)` | 65 | Thực hiện logic wait for seconds trong script Monster33Phase2FireController. | - |
| `private void SpawnBodyFireVFX()` | 69 | Spawn object/dữ liệu body fire vfx. | - |
| `private void SpawnWeaponTrails()` | 81 | Spawn object/dữ liệu weapon trails. | - |
| `private GameObject SpawnWeaponTrail(Transform weaponRoot, GameObject existingTrail)` | 87 | Spawn object/dữ liệu weapon trail. | - |
| `private void ApplyFullBodyFireMaterial()` | 99 | Áp dụng full body fire material lên character/object mục tiêu. | - |
| `private Material GetFireMaterial()` | 117 | Lấy dữ liệu fire material cho hệ thống khác sử dụng. | - |
| `new Material(shader)` | 132 | Thực hiện logic material trong script Monster33Phase2FireController. | - |
| `new Color(3.5f, 0.95f, 0.08f, 1f))` | 139 | Thực hiện logic color trong script Monster33Phase2FireController. | - |

#### Monster33WeaponConstraintBootstrap

- **Đường dẫn:** `Assets/Game/Scripts/Character/AI Character/Boss Character/Monster33WeaponConstraintBootstrap.cs`
- **Loại:** MonoBehaviour
- **Vai trò dễ hiểu:** Module boss AI: quản lý boss, combat, network, phase, skill, collider hoặc âm thanh riêng của boss.
- **Kế thừa/cha:** MonoBehaviour
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** -

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `void Awake()` | 6 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. | - |
| `static void ApplyCurrentPoseAsConstraintOffset(ParentConstraint constraint)` | 21 | Áp dụng current pose as constraint offset lên character/object mục tiêu. | - |

### Assets/Game/Scripts/Character/AI Character/Knight

#### AIKnightCombatManager

- **Đường dẫn:** `Assets/Game/Scripts/Character/AI Character/Knight/AIKnightCombatManager.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Kỹ năng/hành vi riêng của Knight boss, gồm projectile, shockwave, slash và VFX.
- **Kế thừa/cha:** AICharacterCombatManager
- **Script con:** -
- **Field public/serialized chính:** public ManualDamageCollider SwordDamageCollider, public bool IsPoweredUp, public int PoweredUpFrostBuildUpAmount
- **Liên kết script:** AICharacterCombatManager, ManualDamageCollider, TwinMoonSkill

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `protected override void Awake()` | 25 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. Liên kết trực tiếp: TwinMoonSkill. | TwinMoonSkill |
| `public void SetAttack01Damage()` | 31 | Thiết lập giá trị hoặc trạng thái attack01 damage. | - |
| `public void SetAttack02Damage()` | 38 | Thiết lập giá trị hoặc trạng thái attack02 damage. | - |
| `public void OpenSwordDamageCollider()` | 45 | Mở UI/trạng thái/luồng sword damage collider. | - |
| `public void CloseSwordDamageCollider()` | 51 | Đóng UI/trạng thái/luồng sword damage collider. | - |
| `public void DrainStaminaBasedOnAttack()` | 56 | Thực hiện logic drain stamina based on attack trong script AIKnightCombatManager. | - |
| `public void OpenDamageCollider()` | 60 | Mở UI/trạng thái/luồng damage collider. | - |
| `public void CloseDamageCollider()` | 66 | Đóng UI/trạng thái/luồng damage collider. | - |
| `public void ApplyPowerUpBuff(float damageMultiplierOverride = -1f)` | 71 | Áp dụng power up buff lên character/object mục tiêu. | - |
| `public override bool TryStartSpecialSkill()` | 79 | Thử thực hiện start special skill, thường có kiểm tra điều kiện trước khi chạy. | - |
| `float GetCurrentDamageMultiplier()` | 84 | Lấy dữ liệu current damage multiplier cho hệ thống khác sử dụng. | - |
| `public override void CloseAllDamageColliders()` | 89 | Đóng UI/trạng thái/luồng all damage colliders. | - |

#### DeathMoonSlash

- **Đường dẫn:** `Assets/Game/Scripts/Character/AI Character/Knight/DeathMoonSlash.cs`
- **Loại:** NetworkBehaviour
- **Vai trò dễ hiểu:** Kỹ năng/hành vi riêng của Knight boss, gồm projectile, shockwave, slash và VFX.
- **Kế thừa/cha:** NetworkBehaviour
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** AICharacterManager, AITormentedSoulBossCharacterManager, AITormentedSoulCombatManager, CharacterManager, MoonSlashProjectile, PlayerCamera, TwinMoonVFXFactory

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `new Color(0.48f, 0.15f, 0.78f, 1f)` | 57 | Thực hiện logic color trong script DeathMoonSlash. | - |
| `new Color(0.2f, 0.36f, 0.78f, 1f)` | 58 | Thực hiện logic color trong script DeathMoonSlash. | - |
| `new Vector3(0f, 0.95f, 0f)` | 59 | Thực hiện logic vector3 trong script DeathMoonSlash. | - |
| `public bool TryActivateSkill()` | 76 | Thử thực hiện activate skill, thường có kiểm tra điều kiện trước khi chạy. Liên kết trực tiếp: CharacterManager. | CharacterManager |
| `void Awake()` | 114 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. Liên kết trực tiếp: AICharacterManager, AITormentedSoulBossCharacterManager, AITormentedSoulCombatManager. | AICharacterManager, AITormentedSoulBossCharacterManager, AITormentedSoulCombatManager |
| `void OnEnable()` | 123 | Đăng ký event/callback hoặc bật trạng thái khi component được enable. | - |
| `void OnDisable()` | 130 | Gỡ event/callback hoặc dọn trạng thái khi component bị disable. | - |
| `void Update()` | 136 | Cập nhật logic mỗi frame như input, timer, trạng thái chiến đấu hoặc UI. | - |
| `public override void OnNetworkSpawn()` | 142 | Thiết lập dữ liệu và callback khi NetworkObject được spawn trong Netcode. | - |
| `IEnumerator PerformDeathMoonSlash()` | 150 | Thực hiện logic perform death moon slash trong script DeathMoonSlash. | - |
| `new WaitForSeconds(slashReleaseDelay)` | 174 | Thực hiện logic wait for seconds trong script DeathMoonSlash. | - |
| `new WaitForSeconds(comboDelay)` | 188 | Thực hiện logic wait for seconds trong script DeathMoonSlash. | - |
| `new WaitForSeconds(0.25f)` | 191 | Thực hiện logic wait for seconds trong script DeathMoonSlash. | - |
| `void StopNavigation()` | 196 | Thực hiện logic stop navigation trong script DeathMoonSlash. | - |
| `void HaltCharacterMomentum()` | 206 | Thực hiện logic halt character momentum trong script DeathMoonSlash. | - |
| `public void FireAttackSlash()` | 218 | Thực hiện logic fire attack slash trong script DeathMoonSlash. | - |
| `void SpawnMoonSlashProjectile(Vector3 direction)` | 245 | Spawn object/dữ liệu moon slash projectile. | - |
| `void SpawnMoonSlashProjectile(Vector3 direction, float projectileDamage, float projectilePoise)` | 250 | Spawn object/dữ liệu moon slash projectile. Liên kết trực tiếp: MoonSlashProjectile. | MoonSlashProjectile |
| `void SetSkillState(bool skillIsActive)` | 274 | Thiết lập giá trị hoặc trạng thái skill state. | - |
| `void ResetAfterSkill()` | 289 | Đưa after skill về trạng thái mặc định. | - |
| `int GetSlashCount()` | 301 | Lấy dữ liệu slash count cho hệ thống khác sử dụng. | - |
| `1, IsInPhaseTwo() ? phaseTwoNumberOfSlashes : numberOfSlashes)` | 303 | Kiểm tra điều kiện/trạng thái in phase two. | - |
| `float GetSpreadAngle()` | 306 | Lấy dữ liệu spread angle cho hệ thống khác sử dụng. | - |
| `0f, IsInPhaseTwo() ? phaseTwoSpreadAngle : spreadAngle)` | 308 | Kiểm tra điều kiện/trạng thái in phase two. | - |
| `bool IsInPhaseTwo()` | 311 | Kiểm tra điều kiện/trạng thái in phase two. | - |
| `void EvaluatePowerUpState()` | 325 | Thực hiện logic evaluate power up state trong script DeathMoonSlash. | - |
| `public void EvaluatePowerUpStateFromBossNetwork()` | 341 | Thực hiện logic evaluate power up state from boss network trong script DeathMoonSlash. | - |
| `void ActivatePowerUp()` | 346 | Thực hiện logic activate power up trong script DeathMoonSlash. | - |
| `Vector3 GetLockedForward()` | 356 | Lấy dữ liệu locked forward cho hệ thống khác sử dụng. | - |
| `Vector3 GetSpreadDirection(Vector3 baseDirection, int shotIndex, int shotCount, float totalSpread)` | 370 | Lấy dữ liệu spread direction cho hệ thống khác sử dụng. | - |
| `void FaceTowards(Vector3 direction, float rotationSpeed)` | 380 | Thực hiện logic face towards trong script DeathMoonSlash. | - |
| `Vector3 GetProjectileOrigin()` | 389 | Lấy dữ liệu projectile origin cho hệ thống khác sử dụng. | - |
| `void TryAutoAssignProjectileSpawnPoint()` | 400 | Thử thực hiện auto assign projectile spawn point, thường có kiểm tra điều kiện trước khi chạy. | - |
| `void PlayAnimation(string animationName, string triggerName)` | 422 | Phát animation, thường là animation, sound hoặc VFX. | - |
| `void SpawnChargeVFX(float chargeDuration)` | 443 | Spawn object/dữ liệu charge vfx. Liên kết trực tiếp: TwinMoonVFXFactory. | TwinMoonVFXFactory |
| `void DestroyChargeVFX()` | 459 | Thực hiện logic destroy charge vfx trong script DeathMoonSlash. | - |
| `void RefreshAuraState()` | 468 | Làm mới dữ liệu/hiển thị aura state. | - |
| `GameObject CreateAuraVFX()` | 483 | Tạo object/dữ liệu aura vfx. | - |
| `new GameObject("DeathMoonSlash_Aura")` | 493 | Thực hiện logic game object trong script DeathMoonSlash. | - |
| `void CreateAuraParticles( Transform parent, string objectName, Color color, float radius, float rateOverTime, float startSize, float lifetime, float orbitalVelocity, float radialVelocity)` | 511 | Tạo object/dữ liệu aura particles. | - |
| `new GameObject(objectName)` | 522 | Thực hiện logic game object trong script DeathMoonSlash. | - |
| `new Color(color.r, color.g, color.b, 0.65f)` | 544 | Thực hiện logic color trong script DeathMoonSlash. | - |
| `new AnimationCurve( new Keyframe(0f, 0.2f), new Keyframe(0.35f, 1f), new Keyframe(1f, 0f)))` | 570 | Thực hiện logic animation curve trong script DeathMoonSlash. | - |
| `Material GetRuntimeAuraMaterial()` | 578 | Lấy dữ liệu runtime aura material cho hệ thống khác sử dụng. | - |
| `new Material(shader)` | 588 | Thực hiện logic material trong script DeathMoonSlash. | - |
| `Gradient BuildGradient(Color color, float alpha)` | 608 | Thực hiện logic build gradient trong script DeathMoonSlash. | - |
| `new Gradient()` | 610 | Thực hiện logic gradient trong script DeathMoonSlash. | - |
| `void DestroyAuraVFX()` | 628 | Thực hiện logic destroy aura vfx trong script DeathMoonSlash. | - |
| `void BroadcastSlashFeedbackClientRpc(Vector3 origin, Vector3 direction)` | 638 | Server gọi xuống client để đồng bộ hoặc phát hiệu ứng cho broadcast slash feedback. | - |
| `void SpawnSlashCastVFX(Vector3 origin, Vector3 direction)` | 644 | Spawn object/dữ liệu slash cast vfx. Liên kết trực tiếp: TwinMoonVFXFactory. | TwinMoonVFXFactory |
| `void TryPlayCameraShake(Vector3 impactPoint)` | 655 | Thử thực hiện play camera shake, thường có kiểm tra điều kiện trước khi chạy. Liên kết trực tiếp: PlayerCamera. | PlayerCamera |
| `IEnumerator DoCameraShake(Transform cameraPivot)` | 671 | Thực hiện logic do camera shake trong script DeathMoonSlash. | - |

#### MoonSlashProjectile

- **Đường dẫn:** `Assets/Game/Scripts/Character/AI Character/Knight/MoonSlashProjectile.cs`
- **Loại:** NetworkBehaviour
- **Vai trò dễ hiểu:** Kỹ năng/hành vi riêng của Knight boss, gồm projectile, shockwave, slash và VFX.
- **Kế thừa/cha:** NetworkBehaviour
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** CharacterManager, TwinMoonVFXFactory, WorldUtilityManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `new Color(0.48f, 0.15f, 0.78f, 1f)` | 22 | Thực hiện logic color trong script MoonSlashProjectile. | - |
| `new Color(0.2f, 0.36f, 0.78f, 1f)` | 23 | Thực hiện logic color trong script MoonSlashProjectile. | - |
| `void Awake()` | 35 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. | - |
| `void OnEnable()` | 44 | Đăng ký event/callback hoặc bật trạng thái khi component được enable. | - |
| `void Update()` | 51 | Cập nhật logic mỗi frame như input, timer, trạng thái chiến đấu hoặc UI. | - |
| `void FixedUpdate()` | 62 | Cập nhật theo bước vật lý, thường xử lý movement, trigger hoặc physics. | - |
| `public void Initialize( CharacterManager owner, Vector3 direction, float damageAmount, float projectileSpeed, float lifeTime, float poiseDamageAmount, LayerMask validTargetLayers, GameObject impactVFX, Color auraColor, Color coreColor)` | 71 | Thực hiện logic initialize trong script MoonSlashProjectile. | - |
| `void OnTriggerEnter(Collider other)` | 107 | Xử lý khi collider khác đi vào trigger của object này. Liên kết trực tiếp: CharacterManager, WorldUtilityManager. | CharacterManager, WorldUtilityManager |
| `void ApplyDamage(CharacterManager damageTarget, Collider hitCollider)` | 148 | Áp dụng damage lên character/object mục tiêu. | - |
| `void SpawnTrailVFX()` | 174 | Spawn object/dữ liệu trail vfx. | - |
| `GameObject CreateRuntimeTrail()` | 190 | Tạo object/dữ liệu runtime trail. | - |
| `new GameObject("MoonSlashProjectile_Trail")` | 192 | Thực hiện logic game object trong script MoonSlashProjectile. | - |
| `new AnimationCurve( new Keyframe(0f, 0.2f), new Keyframe(0.35f, 1f), new Keyframe(1f, 0f))` | 208 | Thực hiện logic animation curve trong script MoonSlashProjectile. | - |
| `Material CreateRuntimeTrailMaterial()` | 216 | Tạo object/dữ liệu runtime trail material. | - |
| `new Material(shader)` | 226 | Thực hiện logic material trong script MoonSlashProjectile. | - |
| `Gradient BuildTrailGradient()` | 246 | Thực hiện logic build trail gradient trong script MoonSlashProjectile. | - |
| `new Gradient()` | 248 | Thực hiện logic gradient trong script MoonSlashProjectile. | - |
| `void SpawnImpactVFX(Vector3 impactPoint)` | 265 | Spawn object/dữ liệu impact vfx. Liên kết trực tiếp: TwinMoonVFXFactory. | TwinMoonVFXFactory |
| `void DestroyProjectile()` | 276 | Thực hiện logic destroy projectile trong script MoonSlashProjectile. | - |

#### TwinMoonShockwaveHitbox

- **Đường dẫn:** `Assets/Game/Scripts/Character/AI Character/Knight/TwinMoonShockwaveHitbox.cs`
- **Loại:** MonoBehaviour
- **Vai trò dễ hiểu:** Kỹ năng/hành vi riêng của Knight boss, gồm projectile, shockwave, slash và VFX.
- **Kế thừa/cha:** MonoBehaviour
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** AICharacterManager, CharacterManager, TwinMoonSkill, WorldUtilityManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `void Awake()` | 27 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. | - |
| `public void Initialize( TwinMoonSkill skill, AICharacterManager source, float targetRadius, float lifetime, float waveDamage, float wavePoiseDamage, float waveKnockbackForce, float hitShellThickness, float hitVerticalTolerance)` | 38 | Thực hiện logic initialize trong script TwinMoonShockwaveHitbox. | - |
| `void Update()` | 67 | Cập nhật logic mỗi frame như input, timer, trạng thái chiến đấu hoặc UI. | - |
| `void OnTriggerEnter(Collider other)` | 78 | Xử lý khi collider khác đi vào trigger của object này. | - |
| `void OnTriggerStay(Collider other)` | 83 | Thực hiện logic on trigger stay trong script TwinMoonShockwaveHitbox. | - |
| `void UpdateColliderRadius()` | 88 | Cập nhật collider radius theo trạng thái mới. | - |
| `void TryDamageTarget(Collider other)` | 96 | Thử thực hiện damage target, thường có kiểm tra điều kiện trước khi chạy. Liên kết trực tiếp: CharacterManager, WorldUtilityManager. | CharacterManager, WorldUtilityManager |

#### TwinMoonShockwaveRing

- **Đường dẫn:** `Assets/Game/Scripts/Character/AI Character/Knight/TwinMoonShockwaveRing.cs`
- **Loại:** MonoBehaviour
- **Vai trò dễ hiểu:** Kỹ năng/hành vi riêng của Knight boss, gồm projectile, shockwave, slash và VFX.
- **Kế thừa/cha:** MonoBehaviour
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** -

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `new Color(0.35f, 0.95f, 1f, 0.95f)` | 15 | Thực hiện logic color trong script TwinMoonShockwaveRing. | - |
| `new Color(0.35f, 0.95f, 1f, 0f)` | 16 | Thực hiện logic color trong script TwinMoonShockwaveRing. | - |
| `void Awake()` | 21 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. | - |
| `public void Initialize(float radius, float lifetime, Color color)` | 31 | Thực hiện logic initialize trong script TwinMoonShockwaveRing. | - |
| `new Color(color.r, color.g, color.b, 0f)` | 36 | Thực hiện logic color trong script TwinMoonShockwaveRing. | - |
| `void Update()` | 42 | Cập nhật logic mỗi frame như input, timer, trạng thái chiến đấu hoặc UI. | - |
| `void DrawRing(float radius)` | 51 | Thực hiện logic draw ring trong script TwinMoonShockwaveRing. | - |
| `new Vector3(Mathf.Cos(angle) * radius, heightOffset, Mathf.Sin(angle) * radius)` | 56 | Thực hiện logic vector3 trong script TwinMoonShockwaveRing. | - |
| `void ApplyVisuals(float normalizedTime)` | 61 | Áp dụng visuals lên character/object mục tiêu. | - |
| `new Gradient()` | 65 | Thực hiện logic gradient trong script TwinMoonShockwaveRing. | - |

#### TwinMoonSkill

- **Đường dẫn:** `Assets/Game/Scripts/Character/AI Character/Knight/TwinMoonSkill.cs`
- **Loại:** NetworkBehaviour
- **Vai trò dễ hiểu:** Kỹ năng/hành vi riêng của Knight boss, gồm projectile, shockwave, slash và VFX.
- **Kế thừa/cha:** NetworkBehaviour
- **Script con:** -
- **Field public/serialized chính:** public bool IsPoweredUp
- **Liên kết script:** AICharacterManager, AIKnightCombatManager, CharacterManager, PlayerCamera, PlayerManager, TwinMoonShockwaveHitbox, TwinMoonVFXFactory, WorldUtilityManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `new Vector3(0f, 1.25f, 0f)` | 33 | Thực hiện logic vector3 trong script TwinMoonSkill. | - |
| `new Color(0.15f, 0.9f, 1f, 1f)` | 57 | Thực hiện logic color trong script TwinMoonSkill. | - |
| `new Color(0.2f, 2.8f, 4f, 1f)` | 58 | Thực hiện logic color trong script TwinMoonSkill. | - |
| `void Awake()` | 99 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. Liên kết trực tiếp: AICharacterManager, AIKnightCombatManager. | AICharacterManager, AIKnightCombatManager |
| `void OnEnable()` | 108 | Đăng ký event/callback hoặc bật trạng thái khi component được enable. | - |
| `void OnDisable()` | 113 | Gỡ event/callback hoặc dọn trạng thái khi component bị disable. | - |
| `public override void OnNetworkSpawn()` | 118 | Thiết lập dữ liệu và callback khi NetworkObject được spawn trong Netcode. | - |
| `public override void OnNetworkDespawn()` | 125 | Dọn dữ liệu/callback khi NetworkObject bị despawn khỏi Netcode. | - |
| `public bool TryActivateSkill()` | 131 | Thử thực hiện activate skill, thường có kiểm tra điều kiện trước khi chạy. Liên kết trực tiếp: CharacterManager. | CharacterManager |
| `void TrySubscribeToHealth()` | 166 | Thử thực hiện subscribe to health, thường có kiểm tra điều kiện trước khi chạy. | - |
| `void UnsubscribeFromHealth()` | 175 | Thực hiện logic unsubscribe from health trong script TwinMoonSkill. | - |
| `void OnHealthChanged(int oldValue, int newValue)` | 184 | Thực hiện logic on health changed trong script TwinMoonSkill. | - |
| `void EvaluatePowerUpState()` | 189 | Thực hiện logic evaluate power up state trong script TwinMoonSkill. | - |
| `public void EvaluatePowerUpStateFromBossNetwork()` | 205 | Thực hiện logic evaluate power up state from boss network trong script TwinMoonSkill. | - |
| `void ActivatePowerUp()` | 210 | Thực hiện logic activate power up trong script TwinMoonSkill. | - |
| `IEnumerator PerformTwinMoonSkill()` | 220 | Thực hiện logic perform twin moon skill trong script TwinMoonSkill. | - |
| `return StartCoroutine(PhaseJump())` | 237 | Thực hiện logic start coroutine trong script TwinMoonSkill. | - |
| `return StartCoroutine(PhaseHover())` | 238 | Thực hiện logic start coroutine trong script TwinMoonSkill. | - |
| `return StartCoroutine(PhaseSlam())` | 239 | Thực hiện logic start coroutine trong script TwinMoonSkill. | - |
| `return StartCoroutine(PhaseTwinShockwaves())` | 240 | Thực hiện logic start coroutine trong script TwinMoonSkill. | - |
| `new WaitForSeconds(postImpactRecovery)` | 242 | Thực hiện logic wait for seconds trong script TwinMoonSkill. | - |
| `IEnumerator PhaseJump()` | 250 | Thực hiện logic phase jump trong script TwinMoonSkill. | - |
| `IEnumerator PhaseHover()` | 273 | Thực hiện logic phase hover trong script TwinMoonSkill. | - |
| `IEnumerator PhaseSlam()` | 293 | Thực hiện logic phase slam trong script TwinMoonSkill. | - |
| `IEnumerator PhaseTwinShockwaves()` | 309 | Thực hiện logic phase twin shockwaves trong script TwinMoonSkill. | - |
| `return StartCoroutine(ExpandShockwave(1, firstWaveRadius, firstWaveDamage, firstWavePoiseDamage, firstWaveKnockbackForce, 0f))` | 311 | Thực hiện logic start coroutine trong script TwinMoonSkill. | - |
| `new WaitForSeconds(secondWaveDelay)` | 312 | Thực hiện logic wait for seconds trong script TwinMoonSkill. | - |
| `return StartCoroutine(ExpandShockwave(2, secondWaveRadius, secondWaveDamage, secondWavePoiseDamage, secondWaveKnockbackForce, secondWaveDelay))` | 313 | Thực hiện logic start coroutine trong script TwinMoonSkill. | - |
| `IEnumerator ExpandShockwave(int waveIndex, float maxRadius, float damage, float poiseDamage, float knockbackForce, float visualDelay)` | 316 | Thực hiện logic expand shockwave trong script TwinMoonSkill. | - |
| `new Vector3(scale, 1f, scale)` | 337 | Thực hiện logic vector3 trong script TwinMoonSkill. | - |
| `void SpawnShockwaveHitbox(float radius, float damage, float poiseDamage, float knockbackForce)` | 346 | Spawn object/dữ liệu shockwave hitbox. Liên kết trực tiếp: TwinMoonShockwaveHitbox. | TwinMoonShockwaveHitbox |
| `new GameObject("TwinMoonShockwaveHitbox")` | 348 | Thực hiện logic game object trong script TwinMoonSkill. | - |
| `public void ApplyShockwaveHit(CharacterManager target, float damage, float poiseDamage, float knockbackForce)` | 365 | Áp dụng shockwave hit lên character/object mục tiêu. | - |
| `void ApplyShockwaveFrostBuildUp(CharacterManager target)` | 372 | Áp dụng shockwave frost build up lên character/object mục tiêu. Liên kết trực tiếp: PlayerManager. | PlayerManager |
| `void ApplyDamageToTarget(CharacterManager target, float damage, float poiseDamage)` | 386 | Áp dụng damage to target lên character/object mục tiêu. | - |
| `void ApplyKnockback(CharacterManager target, float knockbackForce)` | 412 | Áp dụng knockback lên character/object mục tiêu. | - |
| `void SetSkillState(bool skillIsActive)` | 439 | Thiết lập giá trị hoặc trạng thái skill state. | - |
| `void ResetAfterSkill()` | 455 | Đưa after skill về trạng thái mặc định. | - |
| `void PlayAnimationIfAssigned(string animationName)` | 466 | Phát animation if assigned, thường là animation, sound hoặc VFX. | - |
| `void MoveCharacter(Vector3 worldDelta)` | 481 | Thực hiện logic move character trong script TwinMoonSkill. | - |
| `bool IsGroundedForImpact()` | 498 | Kiểm tra điều kiện/trạng thái grounded for impact. Liên kết trực tiếp: WorldUtilityManager. | WorldUtilityManager |
| `Vector3 GetSkillForward()` | 507 | Lấy dữ liệu skill forward cho hệ thống khác sử dụng. | - |
| `void FaceTowards(Vector3 direction)` | 521 | Thực hiện logic face towards trong script TwinMoonSkill. | - |
| `void SpawnChargeVFX()` | 530 | Spawn object/dữ liệu charge vfx. Liên kết trực tiếp: TwinMoonVFXFactory. | TwinMoonVFXFactory |
| `void DestroyChargeVFX()` | 546 | Thực hiện logic destroy charge vfx trong script TwinMoonSkill. | - |
| `void SpawnImpactVFX(Vector3 impactPoint)` | 555 | Spawn object/dữ liệu impact vfx. Liên kết trực tiếp: TwinMoonVFXFactory. | TwinMoonVFXFactory |
| `GameObject SpawnShockwaveVFX(float radius)` | 566 | Spawn object/dữ liệu shockwave vfx. Liên kết trực tiếp: TwinMoonVFXFactory. | TwinMoonVFXFactory |
| `return Instantiate(shockwaveVFXPrefab, transform.position + Vector3.up * 0.05f, Quaternion.identity)` | 571 | Thực hiện logic instantiate trong script TwinMoonSkill. | - |
| `void BroadcastImpactFeedbackClientRpc(Vector3 impactPoint)` | 575 | Server gọi xuống client để đồng bộ hoặc phát hiệu ứng cho broadcast impact feedback. | - |
| `IEnumerator PlayShockwaveVisualsOnly()` | 584 | Phát shockwave visuals only, thường là animation, sound hoặc VFX. | - |
| `return StartCoroutine(PlayShockwaveVisual(firstWaveRadius))` | 586 | Thực hiện logic start coroutine trong script TwinMoonSkill. | - |
| `new WaitForSeconds(secondWaveDelay)` | 587 | Thực hiện logic wait for seconds trong script TwinMoonSkill. | - |
| `return StartCoroutine(PlayShockwaveVisual(secondWaveRadius))` | 588 | Thực hiện logic start coroutine trong script TwinMoonSkill. | - |
| `IEnumerator PlayShockwaveVisual(float radius)` | 591 | Phát shockwave visual, thường là animation, sound hoặc VFX. | - |
| `new Vector3(scale, 1f, scale)` | 605 | Thực hiện logic vector3 trong script TwinMoonSkill. | - |
| `void TryPlayCameraShake(Vector3 impactPoint)` | 615 | Thử thực hiện play camera shake, thường có kiểm tra điều kiện trước khi chạy. Liên kết trực tiếp: PlayerCamera. | PlayerCamera |
| `IEnumerator DoCameraShake(Transform cameraPivot)` | 631 | Thực hiện logic do camera shake trong script TwinMoonSkill. | - |
| `void TryPlaySlowMotion(Vector3 impactPoint)` | 652 | Thử thực hiện play slow motion, thường có kiểm tra điều kiện trước khi chạy. Liên kết trực tiếp: PlayerCamera. | PlayerCamera |
| `IEnumerator DoSlowMotion()` | 665 | Thực hiện logic do slow motion trong script TwinMoonSkill. | - |
| `new WaitForSecondsRealtime(slowMotionDuration)` | 674 | Thực hiện logic wait for seconds realtime trong script TwinMoonSkill. | - |
| `void CacheSwordVisualReferences()` | 681 | Thực hiện logic cache sword visual references trong script TwinMoonSkill. | - |
| `void ApplySwordPowerUpVisuals()` | 697 | Áp dụng sword power up visuals lên character/object mục tiêu. | - |
| `new Color(powerUpColor.r, powerUpColor.g, powerUpColor.b, 0f)` | 745 | Thực hiện logic color trong script TwinMoonSkill. | - |
| `float GetCurrentSkillDamageMultiplier()` | 764 | Lấy dữ liệu current skill damage multiplier cho hệ thống khác sử dụng. | - |
| `void OnDrawGizmosSelected()` | 769 | Thực hiện logic on draw gizmos selected trong script TwinMoonSkill. | - |
| `new Color(0.2f, 0.8f, 1f, 0.15f)` | 774 | Thực hiện logic color trong script TwinMoonSkill. | - |
| `new Color(0.45f, 0.55f, 1f, 0.15f)` | 777 | Thực hiện logic color trong script TwinMoonSkill. | - |
| `new Color(0.2f, 1f, 0.6f, 0.15f)` | 780 | Thực hiện logic color trong script TwinMoonSkill. | - |

#### TwinMoonVFXFactory

- **Đường dẫn:** `Assets/Game/Scripts/Character/AI Character/Knight/TwinMoonVFXFactory.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Kỹ năng/hành vi riêng của Knight boss, gồm projectile, shockwave, slash và VFX.
- **Kế thừa/cha:** -
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** TwinMoonShockwaveRing, Utility_DestroyAfterTime

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public static GameObject CreateChargeVFX(Transform parent, Vector3 localOffset, Color color, float duration)` | 9 | Tạo object/dữ liệu charge vfx. | - |
| `new GameObject("TwinMoon_Charge_VFX")` | 11 | Thực hiện logic game object trong script TwinMoonVFXFactory. | - |
| `public static GameObject CreateImpactVFX(Vector3 position, Color color)` | 31 | Tạo object/dữ liệu impact vfx. | - |
| `new GameObject("TwinMoon_Impact_VFX")` | 33 | Thực hiện logic game object trong script TwinMoonVFXFactory. | - |
| `public static GameObject CreateShockwaveVFX(Vector3 position, float radius, float duration, Color color)` | 44 | Tạo object/dữ liệu shockwave vfx. | - |
| `new GameObject("TwinMoon_Shockwave_VFX")` | 46 | Thực hiện logic game object trong script TwinMoonVFXFactory. | - |
| `static void CreateChargeSwirl(Transform parent, Color color)` | 56 | Tạo object/dữ liệu charge swirl. | - |
| `new Vector3(0f, 0.1f, 0f))` | 58 | Thực hiện logic vector3 trong script TwinMoonVFXFactory. | - |
| `new Color(color.r, color.g, color.b, 0.8f)` | 65 | Thực hiện logic color trong script TwinMoonVFXFactory. | - |
| `static void CreateChargeCore(Transform parent, Color color)` | 94 | Tạo object/dữ liệu charge core. | - |
| `new Color(color.r, color.g, color.b, 0.45f)` | 103 | Thực hiện logic color trong script TwinMoonVFXFactory. | - |
| `static void CreateChargeSparks(Transform parent, Color color)` | 123 | Tạo object/dữ liệu charge sparks. | - |
| `static void CreateImpactFlash(Transform parent, Color color)` | 157 | Tạo object/dữ liệu impact flash. | - |
| `new Vector3(0f, 0.05f, 0f))` | 159 | Thực hiện logic vector3 trong script TwinMoonVFXFactory. | - |
| `static void CreateImpactBurst(Transform parent, Color color)` | 183 | Tạo object/dữ liệu impact burst. | - |
| `new Vector3(0f, 0.05f, 0f))` | 185 | Thực hiện logic vector3 trong script TwinMoonVFXFactory. | - |
| `new Color(color.r, color.g, color.b, 0.85f)` | 192 | Thực hiện logic color trong script TwinMoonVFXFactory. | - |
| `static void CreateImpactDustRing(Transform parent, Color color)` | 214 | Tạo object/dữ liệu impact dust ring. | - |
| `new Vector3(0f, 0.02f, 0f))` | 216 | Thực hiện logic vector3 trong script TwinMoonVFXFactory. | - |
| `new Color(0.85f, 0.95f, 1f, 0.42f)` | 223 | Thực hiện logic color trong script TwinMoonVFXFactory. | - |
| `new Color(color.r, color.g, color.b, 0.5f), 0.5f)` | 241 | Thực hiện logic color trong script TwinMoonVFXFactory. | - |
| `static void CreateShockwaveRing(Transform parent, float radius, float duration, Color color)` | 246 | Tạo object/dữ liệu shockwave ring. Liên kết trực tiếp: TwinMoonShockwaveRing. | TwinMoonShockwaveRing |
| `new GameObject("Shockwave_Ring")` | 248 | Thực hiện logic game object trong script TwinMoonVFXFactory. | - |
| `static void CreateShockwaveDust(Transform parent, float radius, Color color, float duration)` | 265 | Tạo object/dữ liệu shockwave dust. | - |
| `new Vector3(0f, 0.02f, 0f))` | 267 | Thực hiện logic vector3 trong script TwinMoonVFXFactory. | - |
| `new Color(color.r, color.g, color.b, 0.35f)` | 274 | Thực hiện logic color trong script TwinMoonVFXFactory. | - |
| `static ParticleSystem CreateParticleSystem(string name, Transform parent, Vector3 localPosition)` | 296 | Tạo object/dữ liệu particle system. | - |
| `new GameObject(name)` | 298 | Thực hiện logic game object trong script TwinMoonVFXFactory. | - |
| `static Material GetRuntimeAdditiveMaterial()` | 321 | Lấy dữ liệu runtime additive material cho hệ thống khác sử dụng. | - |
| `new Material(shader)` | 334 | Thực hiện logic material trong script TwinMoonVFXFactory. | - |
| `static Gradient BuildGradient(Color color, float alpha)` | 354 | Thực hiện logic build gradient trong script TwinMoonVFXFactory. | - |
| `new Gradient()` | 356 | Thực hiện logic gradient trong script TwinMoonVFXFactory. | - |
| `static ParticleSystem.MinMaxCurve BuildCurve(float start, float mid, float end)` | 374 | Thực hiện logic build curve trong script TwinMoonVFXFactory. | - |
| `new AnimationCurve( new Keyframe(0f, start), new Keyframe(0.35f, mid), new Keyframe(1f, end)))` | 376 | Thực hiện logic animation curve trong script TwinMoonVFXFactory. | - |
| `static void AddAutoDestroy(GameObject target, float lifetime)` | 382 | Thêm auto destroy vào danh sách, trạng thái hoặc dữ liệu. Liên kết trực tiếp: Utility_DestroyAfterTime. | Utility_DestroyAfterTime |

### Assets/Game/Scripts/Character/AI Character/States

#### AIState

- **Đường dẫn:** `Assets/Game/Scripts/Character/AI Character/States/AIState.cs`
- **Loại:** ScriptableObject
- **Vai trò dễ hiểu:** State trong state machine AI, quyết định AI đang idle, truy đuổi, tấn công, ngủ boss hoặc vào combat stance.
- **Kế thừa/cha:** ScriptableObject
- **Script con:** AttackState, BossSleepState, CombatStanceState, IdleState, InvestigateSoundState, PursueTargetState
- **Field public/serialized chính:** -
- **Liên kết script:** AICharacterManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public virtual AIState Tick(AICharacterManager aiCharacter)` | 8 | Thực hiện logic tick trong script AIState. | - |
| `public virtual AIState SwitchState(AICharacterManager aiCharacter, AIState newState)` | 13 | Thực hiện logic switch state trong script AIState. | - |
| `public virtual AIState ManuallySwitchState(AICharacterManager aiCharacter, AIState newState)` | 20 | Thực hiện logic manually switch state trong script AIState. | - |
| `protected virtual void ResetStateFlags(AICharacterManager aiCharacter)` | 27 | Đưa state flags về trạng thái mặc định. | - |
| `public bool IsDestinationReachable(AICharacterManager aiCharacter, Vector3 destination)` | 32 | Kiểm tra điều kiện/trạng thái destination reachable. | - |
| `new NavMeshPath()` | 36 | Thực hiện logic nav mesh path trong script AIState. | - |

#### AttackState

- **Đường dẫn:** `Assets/Game/Scripts/Character/AI Character/States/AttackState.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** State trong state machine AI, quyết định AI đang idle, truy đuổi, tấn công, ngủ boss hoặc vào combat stance.
- **Kế thừa/cha:** AIState
- **Script con:** -
- **Field public/serialized chính:** public AICharacterAttackAction currentAttack, public bool willPerformCombo, [SerializeField] protected bool pivotAfterAttack
- **Liên kết script:** AICharacterAttackAction, AICharacterManager, AIState

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public override AIState Tick(AICharacterManager aiCharacter)` | 20 | Thực hiện logic tick trong script AttackState. | - |
| `return SwitchState(aiCharacter, aiCharacter.idle)` | 23 | Thực hiện logic switch state trong script AttackState. | - |
| `return SwitchState(aiCharacter, aiCharacter.idle)` | 26 | Thực hiện logic switch state trong script AttackState. | - |
| `return SwitchState(aiCharacter, aiCharacter.combatStance)` | 51 | Thực hiện logic switch state trong script AttackState. | - |
| `protected void PerformAttack(AICharacterManager aiCharacter)` | 54 | Thực hiện logic perform attack trong script AttackState. | - |
| `protected override void ResetStateFlags(AICharacterManager aiCharacter)` | 61 | Đưa state flags về trạng thái mặc định. | - |
| `protected virtual void PerformCombo(AICharacterManager aiCharacter)` | 70 | Thực hiện logic perform combo trong script AttackState. | - |

#### BossSleepState

- **Đường dẫn:** `Assets/Game/Scripts/Character/AI Character/States/BossSleepState.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** State trong state machine AI, quyết định AI đang idle, truy đuổi, tấn công, ngủ boss hoặc vào combat stance.
- **Kế thừa/cha:** AIState
- **Script con:** -
- **Field public/serialized chính:** public bool hasBeenAwakened
- **Liên kết script:** AICharacterManager, AIState

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public override AIState Tick(AICharacterManager aiCharacter)` | 14 | Thực hiện logic tick trong script BossSleepState. | - |
| `return HasNotBeenAwakened(aiCharacter)` | 21 | Thực hiện logic has not been awakened trong script BossSleepState. | - |
| `return HasBeenAwakened(aiCharacter)` | 25 | Thực hiện logic has been awakened trong script BossSleepState. | - |
| `private AIState HasBeenAwakened(AICharacterManager aiCharacter)` | 29 | Thực hiện logic has been awakened trong script BossSleepState. | - |
| `return SwitchState(aiCharacter, aiCharacter.pursueTarget)` | 34 | Thực hiện logic switch state trong script BossSleepState. | - |
| `private AIState HasNotBeenAwakened(AICharacterManager aiCharacter)` | 40 | Thực hiện logic has not been awakened trong script BossSleepState. | - |
| `return SwitchState(aiCharacter, aiCharacter.pursueTarget)` | 59 | Thực hiện logic switch state trong script BossSleepState. | - |

#### CombatStanceState

- **Đường dẫn:** `Assets/Game/Scripts/Character/AI Character/States/CombatStanceState.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** State trong state machine AI, quyết định AI đang idle, truy đuổi, tấn công, ngủ boss hoặc vào combat stance.
- **Kế thừa/cha:** AIState
- **Script con:** -
- **Field public/serialized chính:** public List<AICharacterAttackAction> aiCharacterAttacks, [SerializeField] protected List<AICharacterAttackAction> potentialAttacks, [SerializeField] private AICharacterAttackAction chosenAttack, [SerializeField] private AICharacterAttackAction previousAttack, [SerializeField] protected bool canPerformCombo, [SerializeField] protected int percentageOfTimeWillPerformCombo, [SerializeField] public bool onlyPerformComboIfInitialAttackHits, [SerializeField] public float maximumEngagementDistance
- **Liên kết script:** AICharacterAttackAction, AICharacterManager, AIState, WorldUtilityManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public override AIState Tick(AICharacterManager aiCharacter)` | 45 | Thực hiện logic tick trong script CombatStanceState. | - |
| `return SwitchState(aiCharacter, aiCharacter.idle)` | 68 | Thực hiện logic switch state trong script CombatStanceState. | - |
| `return SwitchState(aiCharacter, aiCharacter.attack)` | 114 | Thực hiện logic switch state trong script CombatStanceState. | - |
| `return SwitchState(aiCharacter, aiCharacter.pursueTarget)` | 118 | Thực hiện logic switch state trong script CombatStanceState. | - |
| `new NavMeshPath()` | 120 | Thực hiện logic nav mesh path trong script CombatStanceState. | - |
| `protected virtual void GetNewAttack(AICharacterManager aiCharacter)` | 127 | Lấy dữ liệu new attack cho hệ thống khác sử dụng. Liên kết trực tiếp: AICharacterAttackAction. | AICharacterAttackAction |
| `protected virtual bool RollForOutcomeChance(int outcomeChance)` | 178 | Thực hiện logic roll for outcome chance trong script CombatStanceState. | - |
| `protected virtual void SetCirclePath(AICharacterManager aiCharacter)` | 190 | Thiết lập giá trị hoặc trạng thái circle path. Liên kết trực tiếp: WorldUtilityManager. | WorldUtilityManager |
| `protected override void ResetStateFlags(AICharacterManager aiCharacter)` | 223 | Đưa state flags về trạng thái mặc định. | - |

#### IdleState

- **Đường dẫn:** `Assets/Game/Scripts/Character/AI Character/States/IdleState.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** State trong state machine AI, quyết định AI đang idle, truy đuổi, tấn công, ngủ boss hoặc vào combat stance.
- **Kế thừa/cha:** AIState
- **Script con:** -
- **Field public/serialized chính:** public IdleStateMode idleStateMode, public AIPatrolPath aiPatrolPath, public bool willInvestigateSound
- **Liên kết script:** AICharacterManager, AIPatrolPath, AIState, IdleStateMode

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public override AIState Tick(AICharacterManager aiCharacter)` | 30 | Thực hiện logic tick trong script IdleState. Liên kết trực tiếp: IdleStateMode. | IdleStateMode |
| `return Idle(aiCharacter)` | 38 | Thực hiện logic idle trong script IdleState. | - |
| `return Patrol(aiCharacter)` | 40 | Thực hiện logic patrol trong script IdleState. | - |
| `return SleepUntilDisturbed(aiCharacter)` | 42 | Thực hiện logic sleep until disturbed trong script IdleState. | - |
| `protected virtual AIState Idle(AICharacterManager aiCharacter)` | 50 | Thực hiện logic idle trong script IdleState. | - |
| `return SwitchState(aiCharacter, aiCharacter.pursueTarget)` | 54 | Thực hiện logic switch state trong script IdleState. | - |
| `protected virtual AIState Patrol(AICharacterManager aiCharacter)` | 62 | Thực hiện logic patrol trong script IdleState. | - |
| `return SwitchState(aiCharacter, aiCharacter.pursueTarget)` | 78 | Thực hiện logic switch state trong script IdleState. | - |
| `new NavMeshPath()` | 157 | Thực hiện logic nav mesh path trong script IdleState. | - |
| `protected virtual AIState SleepUntilDisturbed(AICharacterManager aiCharacter)` | 164 | Thực hiện logic sleep until disturbed trong script IdleState. | - |
| `return SwitchState(aiCharacter, aiCharacter.pursueTarget)` | 183 | Thực hiện logic switch state trong script IdleState. | - |
| `protected override void ResetStateFlags(AICharacterManager aiCharacter)` | 189 | Đưa state flags về trạng thái mặc định. | - |

#### InvestigateSoundState

- **Đường dẫn:** `Assets/Game/Scripts/Character/AI Character/States/InvestigateSoundState.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** State trong state machine AI, quyết định AI đang idle, truy đuổi, tấn công, ngủ boss hoặc vào combat stance.
- **Kế thừa/cha:** AIState
- **Script con:** -
- **Field public/serialized chính:** public Vector3 positionOfSound
- **Liên kết script:** AICharacterManager, AIState

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public override AIState Tick(AICharacterManager aiCharacter)` | 20 | Thực hiện logic tick trong script InvestigateSoundState. | - |
| `return SwitchState(aiCharacter, aiCharacter.pursueTarget)` | 28 | Thực hiện logic switch state trong script InvestigateSoundState. | - |
| `new NavMeshPath()` | 42 | Thực hiện logic nav mesh path trong script InvestigateSoundState. | - |
| `new NavMeshPath()` | 49 | Thực hiện logic nav mesh path trong script InvestigateSoundState. | - |
| `return SwitchState(aiCharacter, aiCharacter.idle)` | 70 | Thực hiện logic switch state trong script InvestigateSoundState. | - |
| `protected override void ResetStateFlags(AICharacterManager aiCharacter)` | 77 | Đưa state flags về trạng thái mặc định. | - |

#### PursueTargetState

- **Đường dẫn:** `Assets/Game/Scripts/Character/AI Character/States/PursueTargetState.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** State trong state machine AI, quyết định AI đang idle, truy đuổi, tấn công, ngủ boss hoặc vào combat stance.
- **Kế thừa/cha:** AIState
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** AICharacterManager, AIState

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public override AIState Tick(AICharacterManager aiCharacter)` | 9 | Thực hiện logic tick trong script PursueTargetState. | - |
| `return SwitchState(aiCharacter, aiCharacter.idle)` | 20 | Thực hiện logic switch state trong script PursueTargetState. | - |
| `return SwitchState(aiCharacter, aiCharacter.combatStance)` | 40 | Thực hiện logic switch state trong script PursueTargetState. | - |
| `new NavMeshPath()` | 44 | Thực hiện logic nav mesh path trong script PursueTargetState. | - |

### Assets/Game/Scripts/Character/AI Character/Undead Character

#### AIUndeadCombatManager

- **Đường dẫn:** `Assets/Game/Scripts/Character/AI Character/Undead Character/AIUndeadCombatManager.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Module enemy AI: quản lý hành vi, spawn, animation, locomotion, combat, inventory, sound hoặc network.
- **Kế thừa/cha:** AICharacterCombatManager
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** AICharacterCombatManager, ManualDamageCollider

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public void SetAttack01Damage()` | 15 | Thiết lập giá trị hoặc trạng thái attack01 damage. | - |
| `public void SetAttack02Damage()` | 24 | Thiết lập giá trị hoặc trạng thái attack02 damage. | - |
| `public void OpenRightHandDamageCollider()` | 33 | Mở UI/trạng thái/luồng right hand damage collider. | - |
| `public void CloseRightHandDamageCollider()` | 39 | Đóng UI/trạng thái/luồng right hand damage collider. | - |
| `public void OpenLeftHandDamageCollider()` | 44 | Mở UI/trạng thái/luồng left hand damage collider. | - |
| `public void CloseLeftHandDamageCollider()` | 50 | Đóng UI/trạng thái/luồng left hand damage collider. | - |
| `public override void CloseAllDamageColliders()` | 55 | Đóng UI/trạng thái/luồng all damage colliders. | - |

### Assets/Game/Scripts/Character/Player

#### PlayerAimCameraFollowTransform

- **Đường dẫn:** `Assets/Game/Scripts/Character/Player/PlayerAimCameraFollowTransform.cs`
- **Loại:** MonoBehaviour
- **Vai trò dễ hiểu:** Module dành cho người chơi: input, camera, di chuyển, combat, inventory, stats, network hoặc hiệu ứng.
- **Kế thừa/cha:** MonoBehaviour
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** -

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| - | - | Script này chủ yếu là dữ liệu/enum/struct, không có method rõ ràng. | - |

#### PlayerAnimatorManager

- **Đường dẫn:** `Assets/Game/Scripts/Character/Player/PlayerAnimatorManager.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Module dành cho người chơi: input, camera, di chuyển, combat, inventory, stats, network hoặc hiệu ứng.
- **Kế thừa/cha:** CharacterAnimatorManager
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** CharacterAnimatorManager, PlayerManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `protected override void Awake()` | 9 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. Liên kết trực tiếp: PlayerManager. | PlayerManager |
| `private void OnAnimatorMove()` | 14 | Nhận root motion từ Animator để áp dụng movement/rotation theo animation. | - |

#### PlayerBodyManager

- **Đường dẫn:** `Assets/Game/Scripts/Character/Player/PlayerBodyManager.cs`
- **Loại:** MonoBehaviour
- **Vai trò dễ hiểu:** Module dành cho người chơi: input, camera, di chuyển, combat, inventory, stats, network hoặc hiệu ứng.
- **Kế thừa/cha:** MonoBehaviour
- **Script con:** -
- **Field public/serialized chính:** [SerializeField] public GameObject hair, [SerializeField] private GameObject[] hairObjects, [SerializeField] public GameObject facialHair, [SerializeField] public GameObject maleObject, [SerializeField] public GameObject maleHead, [SerializeField] public GameObject[] maleBody, [SerializeField] public GameObject[] maleArms, [SerializeField] public GameObject[] maleLegs, [SerializeField] public GameObject maleEyebrows, [SerializeField] public GameObject maleFacialHair, [SerializeField] public GameObject femaleObject, [SerializeField] public GameObject femaleHead +4
- **Liên kết script:** PlayerManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `private void Awake()` | 31 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. Liên kết trực tiếp: PlayerManager. | PlayerManager |
| `public void EnableHead()` | 37 | Bật head. | - |
| `public void DisableHead()` | 46 | Tắt head. | - |
| `public void EnableHair()` | 56 | Bật hair. | - |
| `public void DisableHair()` | 61 | Tắt hair. | - |
| `public void EnableFacialHair()` | 67 | Bật facial hair. | - |
| `public void DisableFacialHair()` | 72 | Tắt facial hair. | - |
| `public void EnableBody()` | 78 | Bật body. | - |
| `public void DisableBody()` | 91 | Tắt body. | - |
| `public void EnableLowerBody()` | 104 | Bật lower body. | - |
| `public void DisableLowerBody()` | 116 | Tắt lower body. | - |
| `public void EnableArms()` | 129 | Bật arms. | - |
| `public void DisableArms()` | 141 | Tắt arms. | - |
| `public void ToggleBodyType(bool isMale)` | 153 | Thực hiện logic toggle body type trong script PlayerBodyManager. | - |
| `public void ToggleHairType(int hairType)` | 169 | Thực hiện logic toggle hair type trong script PlayerBodyManager. | - |
| `public void SetHairColor()` | 181 | Thiết lập giá trị hoặc trạng thái hair color. | - |
| `new Color32(red, green, blue, 255)` | 189 | Thực hiện logic color32 trong script PlayerBodyManager. | - |

#### PlayerCamera

- **Đường dẫn:** `Assets/Game/Scripts/Character/Player/PlayerCamera.cs`
- **Loại:** MonoBehaviour
- **Vai trò dễ hiểu:** Module dành cho người chơi: input, camera, di chuyển, combat, inventory, stats, network hoặc hiệu ứng.
- **Kế thừa/cha:** MonoBehaviour
- **Script con:** -
- **Field public/serialized chính:** public static PlayerCamera instance, public PlayerManager player, public Camera cameraObject, public Transform cameraPivotTransform, public float cameraPivotYPositionOffSet, [SerializeField] private float cameraSmoothSpeed, public CharacterManager nearestLockOnTarget, public CharacterManager leftLockOnTarget, public CharacterManager rightLockOnTarget, public Vector3 aimDirection
- **Liên kết script:** CharacterManager, GameSettingsManager, PlayerInputManager, PlayerManager, WorldUtilityManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `private void Awake()` | 54 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. | - |
| `private void Start()` | 66 | Chạy sau Awake để hoàn tất thiết lập ban đầu, nhất là khi cần các object khác đã sẵn sàng. Liên kết trực tiếp: GameSettingsManager. | GameSettingsManager |
| `public void HandleAllCameraActions()` | 75 | Xử lý luồng all camera actions. | - |
| `private void HandleFollowTarget()` | 85 | Xử lý luồng follow target. | - |
| `private void HandleRotations()` | 100 | Xử lý luồng rotations. | - |
| `private void HandleAimRotations()` | 112 | Xử lý luồng aim rotations. Liên kết trực tiếp: PlayerInputManager. | PlayerInputManager |
| `new Vector3(upAndDownLookAngle, leftAndRightLookAngle, 0)` | 134 | Thực hiện logic vector3 trong script PlayerCamera. | - |
| `private void HandleStandardRotations()` | 137 | Xử lý luồng standard rotations. Liên kết trực tiếp: PlayerInputManager. | PlayerInputManager |
| `private void HandleCollisions()` | 184 | Xử lý luồng collisions. | - |
| `public void HandleLocatingLockOnTarget()` | 222 | Xử lý luồng locating lock on target. Liên kết trực tiếp: CharacterManager, WorldUtilityManager. | CharacterManager, WorldUtilityManager |
| `public void SetLockCameraHeight()` | 308 | Thiết lập giá trị hoặc trạng thái lock camera height. | - |
| `public void ClearLockOnTargets()` | 317 | Thực hiện logic clear lock on targets trong script PlayerCamera. | - |
| `public IEnumerator WaitThenFindNewTarget()` | 325 | Thực hiện logic wait then find new target trong script PlayerCamera. | - |
| `private IEnumerator SetCameraHeight()` | 344 | Thiết lập giá trị hoặc trạng thái camera height. | - |
| `new Vector3 (cameraPivotTransform.transform.localPosition.x, lockedCameraHeight)` | 350 | Thực hiện logic vector3 trong script PlayerCamera. | - |
| `new Vector3 (cameraPivotTransform.transform.localPosition.x, unlockedCameraHeight)` | 351 | Thực hiện logic vector3 trong script PlayerCamera. | - |
| `public void SetCameraSensitivityMultiplier(float value)` | 399 | Thiết lập giá trị hoặc trạng thái camera sensitivity multiplier. | - |

#### PlayerCombatManager

- **Đường dẫn:** `Assets/Game/Scripts/Character/Player/PlayerCombatManager.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Module dành cho người chơi: input, camera, di chuyển, combat, inventory, stats, network hoặc hiệu ứng.
- **Kế thừa/cha:** CharacterCombatManager
- **Script con:** -
- **Field public/serialized chính:** public WeaponItem currentWeaponBeingUsed, public ProjectileSlot currentProjectileBeingUsed, public bool canComboWithMainHandWeapon, public bool canComboWithOffHandWeapon, public bool isUsingItem
- **Liên kết script:** AttackType, CharacterCombatManager, CharacterManager, MeleeWeaponDamageCollider, MeleeWeaponItem, PickUpRunesInteractable, PlayerCamera, PlayerManager, PlayerUIManager, ProjectileSlot, RangedProjectileDamageCollider, RangedProjectileItem, TakeCriticalDamageEffect, WeaponItem, WeaponItemAction, WorldCharacterEffectsManager +2

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `protected override void Awake()` | 24 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. Liên kết trực tiếp: PlayerManager. | PlayerManager |
| `private void Start()` | 30 | Chạy sau Awake để hoàn tất thiết lập ban đầu, nhất là khi cần các object khác đã sẵn sàng. | - |
| `public override void OnDestroy()` | 35 | Dọn đăng ký/event/tài nguyên khi object bị hủy. | - |
| `private void OnSceneChanged(Scene arg0, Scene arg1)` | 42 | Thực hiện logic on scene changed trong script PlayerCombatManager. Liên kết trực tiếp: WorldSaveGameManager. | WorldSaveGameManager |
| `new Vector3( WorldSaveGameManager.instance.currentCharacterData.deadSpotPositionX, WorldSaveGameManager.instance.currentCharacterData.deadSpotPositionY, WorldSaveGameManager.instance.currentCharacterData.deadSpotPositionZ)` | 48 | Thực hiện logic vector3 trong script PlayerCombatManager. | - |
| `public void CreateDeadSpot(Vector3 position, int runesCount, bool removePlayerRunes = true)` | 57 | Tạo object/dữ liệu dead spot. Liên kết trực tiếp: PickUpRunesInteractable, WorldCharacterEffectsManager, WorldSaveGameManager. | PickUpRunesInteractable, WorldCharacterEffectsManager, WorldSaveGameManager |
| `public void PerformWeaponBasedAction(WeaponItemAction weaponAction, WeaponItem weaponPerformingAction)` | 87 | Thực hiện logic perform weapon based action trong script PlayerCombatManager. | - |
| `public override void CloseAllDamageColliders()` | 95 | Đóng UI/trạng thái/luồng all damage colliders. | - |
| `public override void AttemptRiposte(RaycastHit hit)` | 104 | Cố gắng kích hoạt riposte nếu trạng thái hiện tại cho phép. Liên kết trực tiếp: CharacterManager, MeleeWeaponDamageCollider, MeleeWeaponItem, TakeCriticalDamageEffect, WorldCharacterEffectsManager. | CharacterManager, MeleeWeaponDamageCollider, MeleeWeaponItem, TakeCriticalDamageEffect, WorldCharacterEffectsManager |
| `public override void AttemptBackstab(RaycastHit hit)` | 174 | Cố gắng kích hoạt backstab nếu trạng thái hiện tại cho phép. Liên kết trực tiếp: CharacterManager, MeleeWeaponDamageCollider, MeleeWeaponItem, TakeCriticalDamageEffect, WorldCharacterEffectsManager. | CharacterManager, MeleeWeaponDamageCollider, MeleeWeaponItem, TakeCriticalDamageEffect, WorldCharacterEffectsManager |
| `public virtual void DrainStaminaBasedOnAttack()` | 244 | Thực hiện logic drain stamina based on attack trong script PlayerCombatManager. Liên kết trực tiếp: AttackType. | AttackType |
| `public override void SetTarget(CharacterManager newTarget)` | 315 | Thiết lập giá trị hoặc trạng thái target. Liên kết trực tiếp: PlayerCamera. | PlayerCamera |
| `public override void EnableCanDoCombo()` | 326 | Bật can do combo. | - |
| `public override void DisableCanDoCombo()` | 338 | Tắt can do combo. | - |
| `public void ReleaseArrow()` | 345 | Thực hiện logic release arrow trong script PlayerCombatManager. Liên kết trực tiếp: PlayerCamera, PlayerUIManager, ProjectileSlot, RangedProjectileDamageCollider, RangedProjectileItem +1. | PlayerCamera, PlayerUIManager, ProjectileSlot, RangedProjectileDamageCollider, RangedProjectileItem, WorldSoundFXManager |
| `new Ray(player.playerCombatManager.lockOnTransform.position, PlayerCamera.instance.aimDirection)` | 435 | Thực hiện logic ray trong script PlayerCombatManager. | - |
| `public void InstantiateSpellWarmUpFX()` | 483 | Thực hiện logic instantiate spell warm up fx trong script PlayerCombatManager. | - |
| `public void SuccessfullyCastSpell()` | 491 | Thực hiện logic successfully cast spell trong script PlayerCombatManager. | - |
| `public void SuccessfullyChargeSpell()` | 499 | Thực hiện logic successfully charge spell trong script PlayerCombatManager. | - |
| `public void SuccessfullyCastSpellFullCharge()` | 507 | Thực hiện logic successfully cast spell full charge trong script PlayerCombatManager. | - |
| `public void SuccesfullyUseQuickSlotItem()` | 516 | Thực hiện logic succesfully use quick slot item trong script PlayerCombatManager. | - |
| `public WeaponItem SelectWeaponToPerformAshOfWar()` | 525 | Thực hiện logic select weapon to perform ash of war trong script PlayerCombatManager. Liên kết trực tiếp: WeaponItem. | WeaponItem |

#### PlayerEffectsManager

- **Đường dẫn:** `Assets/Game/Scripts/Character/Player/PlayerEffectsManager.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Module dành cho người chơi: input, camera, di chuyển, combat, inventory, stats, network hoặc hiệu ứng.
- **Kế thừa/cha:** CharacterEffectsManager
- **Script con:** -
- **Field public/serialized chính:** public const int DefaultFireBuildUpFromHit, public const int DefaultFrostBuildUpFromHit
- **Liên kết script:** BuffCharmItem, BuildUp, CharacterEffectsManager, PlayerStatBuffTimedEffect, PlayerUIManager, SerializableActiveBuff, TakeBuildUpEffect, WorldCharacterEffectsManager, WorldItemDatabase

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `protected override void Update()` | 29 | Cập nhật logic mỗi frame như input, timer, trạng thái chiến đấu hoặc UI. | - |
| `private void ProcessDebugBuildUps()` | 42 | Thực hiện logic process debug build ups trong script PlayerEffectsManager. Liên kết trực tiếp: WorldCharacterEffectsManager. | WorldCharacterEffectsManager |
| `private void ApplyDebugBuildUp(TakeBuildUpEffect buildUpTemplate, int buildUpAmount)` | 69 | Áp dụng debug build up lên character/object mục tiêu. Liên kết trực tiếp: TakeBuildUpEffect. | TakeBuildUpEffect |
| `private void HandleFireBuildUpDegradation()` | 79 | Xử lý luồng fire build up degradation. | - |
| `private void HandleBurningDamage()` | 95 | Xử lý luồng burning damage. | - |
| `public void ApplyFireBuildUpFromHit(int buildUpAmount)` | 131 | Áp dụng fire build up from hit lên character/object mục tiêu. | - |
| `public void ApplyFrostBuildUpFromHit(int buildUpAmount)` | 136 | Áp dụng frost build up from hit lên character/object mục tiêu. Liên kết trực tiếp: WorldCharacterEffectsManager. | WorldCharacterEffectsManager |
| `public void ApplyFireBuildUp(int buildUpAmount, bool useHitCooldown = false)` | 144 | Áp dụng fire build up lên character/object mục tiêu. Liên kết trực tiếp: BuildUp. | BuildUp |
| `private void TryActivateBurningFromFireBuildUp()` | 159 | Thử thực hiện activate burning from fire build up, thường có kiểm tra điều kiện trước khi chạy. | - |
| `private void RefreshFireBuildUpBar()` | 173 | Làm mới dữ liệu/hiển thị fire build up bar. Liên kết trực tiếp: PlayerUIManager. | PlayerUIManager |
| `public void SaveActiveBuffs(List<SerializableActiveBuff> activeBuffs)` | 185 | Lưu dữ liệu liên quan tới active buffs. Liên kết trực tiếp: PlayerStatBuffTimedEffect, SerializableActiveBuff. | PlayerStatBuffTimedEffect, SerializableActiveBuff |
| `public void LoadActiveBuffs(List<SerializableActiveBuff> activeBuffs)` | 208 | Nạp dữ liệu hoặc scene liên quan tới active buffs. Liên kết trực tiếp: BuffCharmItem, PlayerStatBuffTimedEffect, PlayerUIManager, SerializableActiveBuff, WorldItemDatabase. | BuffCharmItem, PlayerStatBuffTimedEffect, PlayerUIManager, SerializableActiveBuff, WorldItemDatabase |

#### PlayerEquipmentManager

- **Đường dẫn:** `Assets/Game/Scripts/Character/Player/PlayerEquipmentManager.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Module dành cho người chơi: input, camera, di chuyển, combat, inventory, stats, network hoặc hiệu ứng.
- **Kế thừa/cha:** CharacterEquipmentManager
- **Script con:** -
- **Field public/serialized chính:** public WeaponModelInstantiationSlot rightHandWeaponSlot, public WeaponModelInstantiationSlot leftHandWeaponSlot, public WeaponModelInstantiationSlot leftHandShieldSlot, public WeaponModelInstantiationSlot backSlot, public GameObject rightHandWeaponModel, public GameObject leftHandWeaponModel, public WeaponManager rightWeaponManager, public WeaponManager leftWeaponManager, public GameObject hatsObject, public GameObject[] hats, public GameObject hoodsObject, public GameObject[] hoods +64
- **Liên kết script:** BodyEquipmentItem, CharacterEquipmentManager, HandEquipmentItem, HeadEquipmentItem, HeadEquipmentType, LegEquipmentItem, MeleeWeaponDamageCollider, PlayerManager, PlayerUIManager, QuickSlotItem, RangedProjectileItem, WeaponItem, WeaponManager, WeaponModelInstantiationSlot, WeaponModelSlot, WeaponModelType +2

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `protected override void Awake()` | 100 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. Liên kết trực tiếp: PlayerManager. | PlayerManager |
| `protected override void Start()` | 108 | Chạy sau Awake để hoàn tất thiết lập ban đầu, nhất là khi cần các object khác đã sẵn sàng. | - |
| `public void EquipArmor()` | 114 | Trang bị armor và cập nhật model/chỉ số liên quan. | - |
| `public void ForceClassLegPreview(string equipmentName)` | 130 | Thực hiện logic force class leg preview trong script PlayerEquipmentManager. | - |
| `public void ActivateEquipmentModelByName(string modelName)` | 138 | Thực hiện logic activate equipment model by name trong script PlayerEquipmentManager. | - |
| `public void SwitchQuickSlotItem()` | 157 | Thực hiện logic switch quick slot item trong script PlayerEquipmentManager. | - |
| `public void RefreshCurrentQuickSlotSelection()` | 166 | Làm mới dữ liệu/hiển thị current quick slot selection. | - |
| `private int GetNextAvailableQuickSlotIndex(int currentIndex)` | 184 | Lấy dữ liệu next available quick slot index cho hệ thống khác sử dụng. Liên kết trực tiếp: QuickSlotItem. | QuickSlotItem |
| `private void SetCurrentQuickSlotIndex(int slotIndex)` | 208 | Thiết lập giá trị hoặc trạng thái current quick slot index. | - |
| `private void InitializeArmorModels()` | 224 | Thực hiện logic initialize armor models trong script PlayerEquipmentManager. | - |
| `public void LoadHeadEquipment(HeadEquipmentItem equipment)` | 567 | Nạp dữ liệu hoặc scene liên quan tới head equipment. Liên kết trực tiếp: HeadEquipmentType. | HeadEquipmentType |
| `private void UnloadHeadEquipmentModels()` | 612 | Thực hiện logic unload head equipment models trong script PlayerEquipmentManager. | - |
| `public void LoadBodyEquipment(BodyEquipmentItem equipment)` | 648 | Nạp dữ liệu hoặc scene liên quan tới body equipment. | - |
| `private void UnloadBodyEquipmentModels()` | 678 | Thực hiện logic unload body equipment models trong script PlayerEquipmentManager. | - |
| `public void LoadLegEquipment(LegEquipmentItem equipment)` | 741 | Nạp dữ liệu hoặc scene liên quan tới leg equipment. | - |
| `private bool HasVisibleLegEquipment(bool isMale)` | 782 | Thực hiện logic has visible leg equipment trong script PlayerEquipmentManager. | - |
| `private void TryLoadClassLegFallback(string equipmentName, bool isMale)` | 809 | Thử thực hiện load class leg fallback, thường có kiểm tra điều kiện trước khi chạy. | - |
| `private void SetLegModelsActiveByName(bool isMale, string hipsCodeName, string rightLegCodeName, string leftLegCodeName)` | 830 | Thiết lập giá trị hoặc trạng thái leg models active by name. | - |
| `femaleHips, BuildGenderedModelName(hipsCodeName, isMale))` | 832 | Thực hiện logic build gendered model name trong script PlayerEquipmentManager. | - |
| `femaleRightLegs, BuildGenderedModelName(rightLegCodeName, isMale))` | 833 | Thực hiện logic build gendered model name trong script PlayerEquipmentManager. | - |
| `femaleLeftLegs, BuildGenderedModelName(leftLegCodeName, isMale))` | 834 | Thực hiện logic build gendered model name trong script PlayerEquipmentManager. | - |
| `private void SetActiveModelByName(GameObject[] models, string modelName)` | 837 | Thiết lập giá trị hoặc trạng thái active model by name. | - |
| `private string BuildGenderedModelName(string baseName, bool isMale)` | 860 | Thực hiện logic build gendered model name trong script PlayerEquipmentManager. | - |
| `private void SetModelAndParentsActive(Transform modelTransform)` | 874 | Thiết lập giá trị hoặc trạng thái model and parents active. | - |
| `private void EnableRenderersRecursively(Transform root)` | 889 | Bật renderers recursively. | - |
| `private void UnloadLegEquipmentModels()` | 899 | Thực hiện logic unload leg equipment models trong script PlayerEquipmentManager. | - |
| `public void LoadHandEquipment(HandEquipmentItem equipment)` | 944 | Nạp dữ liệu hoặc scene liên quan tới hand equipment. | - |
| `private void UnloadHandEquipmentModels()` | 974 | Thực hiện logic unload hand equipment models trong script PlayerEquipmentManager. | - |
| `public void LoadMainProjectileEquipment(RangedProjectileItem equipment)` | 1020 | Nạp dữ liệu hoặc scene liên quan tới main projectile equipment. | - |
| `public void LoadSecondaryProjectileEquipment(RangedProjectileItem equipment)` | 1037 | Nạp dữ liệu hoặc scene liên quan tới secondary projectile equipment. | - |
| `public void LoadQuickSlotEquipment(QuickSlotItem equipment)` | 1055 | Nạp dữ liệu hoặc scene liên quan tới quick slot equipment. Liên kết trực tiếp: PlayerUIManager. | PlayerUIManager |
| `public void RefreshWeaponDamage()` | 1081 | Làm mới dữ liệu/hiển thị weapon damage. | - |
| `private void InitializeWeaponSlot()` | 1091 | Thực hiện logic initialize weapon slot trong script PlayerEquipmentManager. Liên kết trực tiếp: WeaponModelInstantiationSlot, WeaponModelSlot. | WeaponModelInstantiationSlot, WeaponModelSlot |
| `public void EquipWeapons()` | 1117 | Trang bị weapons và cập nhật model/chỉ số liên quan. | - |
| `public void SwitchRightWeapon()` | 1124 | Thực hiện logic switch right weapon trong script PlayerEquipmentManager. Liên kết trực tiếp: WeaponItem, WorldItemDatabase. | WeaponItem, WorldItemDatabase |
| `public void LoadRightWeapon()` | 1197 | Nạp dữ liệu hoặc scene liên quan tới right weapon. Liên kết trực tiếp: WeaponManager, WorldItemDatabase. | WeaponManager, WorldItemDatabase |
| `public void SwitchLeftWeapon()` | 1235 | Thực hiện logic switch left weapon trong script PlayerEquipmentManager. Liên kết trực tiếp: WeaponItem, WorldItemDatabase. | WeaponItem, WorldItemDatabase |
| `public void LoadLeftWeapon()` | 1308 | Nạp dữ liệu hoặc scene liên quan tới left weapon. Liên kết trực tiếp: WeaponManager, WeaponModelType, WorldItemDatabase. | WeaponManager, WeaponModelType, WorldItemDatabase |
| `public void UnTwoHandWeapon()` | 1359 | Thực hiện logic un two hand weapon trong script PlayerEquipmentManager. Liên kết trực tiếp: WeaponModelType. | WeaponModelType |
| `public void TwoHandRightWeapon()` | 1383 | Thực hiện logic two hand right weapon trong script PlayerEquipmentManager. Liên kết trực tiếp: WorldItemDatabase. | WorldItemDatabase |
| `public void TwoHandLeftWeapon()` | 1409 | Thực hiện logic two hand left weapon trong script PlayerEquipmentManager. Liên kết trực tiếp: WorldItemDatabase. | WorldItemDatabase |
| `public void OpenDamageCollider()` | 1436 | Mở UI/trạng thái/luồng damage collider. | - |
| `public void CloseDamageCollider()` | 1454 | Đóng UI/trạng thái/luồng damage collider. | - |
| `public void OpenMainHandDamageCollider()` | 1472 | Mở UI/trạng thái/luồng main hand damage collider. | - |
| `public void CloseMainHandDamageCollider()` | 1477 | Đóng UI/trạng thái/luồng main hand damage collider. | - |
| `public void OpenOffHandDamageCollider()` | 1482 | Mở UI/trạng thái/luồng off hand damage collider. | - |
| `public void CloseOffHandDamageCollider()` | 1487 | Đóng UI/trạng thái/luồng off hand damage collider. | - |
| `private void OpenWeaponDamageCollider(ref WeaponManager weaponManager, GameObject weaponModel, WeaponItem weaponItem)` | 1492 | Mở UI/trạng thái/luồng weapon damage collider. Liên kết trực tiếp: WorldSoundFXManager. | WorldSoundFXManager |
| `private void CloseWeaponDamageCollider(ref WeaponManager weaponManager, GameObject weaponModel)` | 1507 | Đóng UI/trạng thái/luồng weapon damage collider. | - |
| `private bool TryResolveWeaponManager(ref WeaponManager weaponManager, GameObject weaponModel)` | 1518 | Thử thực hiện resolve weapon manager, thường có kiểm tra điều kiện trước khi chạy. Liên kết trực tiếp: MeleeWeaponDamageCollider, WeaponManager. | MeleeWeaponDamageCollider, WeaponManager |
| `public void UnHideWeapons()` | 1536 | Thực hiện logic un hide weapons trong script PlayerEquipmentManager. | - |

#### PlayerInputManager

- **Đường dẫn:** `Assets/Game/Scripts/Character/Player/PlayerInputManager.cs`
- **Loại:** MonoBehaviour
- **Vai trò dễ hiểu:** Module dành cho người chơi: input, camera, di chuyển, combat, inventory, stats, network hoặc hiệu ứng.
- **Kế thừa/cha:** MonoBehaviour
- **Script con:** -
- **Field public/serialized chính:** public static PlayerInputManager instance, public PlayerManager player, public float cameraHorizontal_Input, public float cameraVertical_Input, public float horizontal_Input, public float vertical_Input, public float moveAmount, [SerializeField] private bool input_Que_Is_Active
- **Liên kết script:** PlayerCamera, PlayerManager, PlayerUIManager, WeaponItem, WorldSaveGameManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `private void Awake()` | 73 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. | - |
| `private void Start()` | 86 | Chạy sau Awake để hoàn tất thiết lập ban đầu, nhất là khi cần các object khác đã sẵn sàng. | - |
| `private void OnSceneChange(Scene oldScene, Scene newScene)` | 102 | Thực hiện logic on scene change trong script PlayerInputManager. Liên kết trực tiếp: WorldSaveGameManager. | WorldSaveGameManager |
| `private void OnEnable()` | 127 | Đăng ký event/callback hoặc bật trạng thái khi component được enable. Liên kết trực tiếp: PlayerCamera. | PlayerCamera |
| `new PlayerControls()` | 131 | Phát er controls, thường là animation, sound hoặc VFX. | - |
| `private void OnDestroy()` | 193 | Dọn đăng ký/event/tài nguyên khi object bị hủy. | - |
| `private void OnApplicationFocus(bool focus)` | 200 | Thực hiện logic on application focus trong script PlayerInputManager. | - |
| `private void Update()` | 215 | Cập nhật logic mỗi frame như input, timer, trạng thái chiến đấu hoặc UI. | - |
| `private void HandleAllInputs()` | 220 | Xử lý luồng all inputs. | - |
| `private bool IsGameplayInputLocked()` | 255 | Kiểm tra điều kiện/trạng thái gameplay input locked. Liên kết trực tiếp: PlayerUIManager. | PlayerUIManager |
| `public void SuppressGameplayInputs(bool suppress)` | 260 | Thực hiện logic suppress gameplay inputs trong script PlayerInputManager. Liên kết trực tiếp: PlayerCamera. | PlayerCamera |
| `private void EnableInputMaps()` | 279 | Bật input maps. Liên kết trực tiếp: PlayerCamera. | PlayerCamera |
| `private void ClearGameplayInputsWhileMenuOpen()` | 296 | Thực hiện logic clear gameplay inputs while menu open trong script PlayerInputManager. | - |
| `private void HandleUseItemInput()` | 351 | Xử lý luồng use item input. Liên kết trực tiếp: PlayerUIManager. | PlayerUIManager |
| `private void HandleTwoHandInput()` | 372 | Xử lý luồng two hand input. | - |
| `private void HandleLockOnInput()` | 457 | Xử lý luồng lock on input. Liên kết trực tiếp: PlayerCamera. | PlayerCamera |
| `private void HandleLockOnSwitchTargetInput()` | 544 | Xử lý luồng lock on switch target input. Liên kết trực tiếp: PlayerCamera. | PlayerCamera |
| `private void HandlePlayerMovementInput()` | 578 | Xử lý luồng player movement input. | - |
| `private void HandleCameraMovementInput()` | 641 | Xử lý luồng camera movement input. | - |
| `private void HandleDodgeInput()` | 648 | Xử lý luồng dodge input. Liên kết trực tiếp: PlayerUIManager. | PlayerUIManager |
| `private void HandleSprintInput()` | 662 | Xử lý luồng sprint input. | - |
| `private void HandleJumpInput()` | 674 | Xử lý luồng jump input. Liên kết trực tiếp: PlayerUIManager. | PlayerUIManager |
| `private void HandleSneakInput()` | 687 | Xử lý luồng sneak input. | - |
| `private void HandleRBInput()` | 701 | Xử lý luồng rbinput. | - |
| `private void HandleHoldRBInput()` | 718 | Xử lý luồng hold rbinput. | - |
| `private void HandleLBInput()` | 732 | Xử lý luồng lbinput. | - |
| `private void HandleHoldLBInput()` | 754 | Xử lý luồng hold lbinput. | - |
| `private void HandleRTInput()` | 766 | Xử lý luồng rtinput. | - |
| `private void HandleChargeRTInput()` | 778 | Xử lý luồng charge rtinput. | - |
| `private void HandleLTInput()` | 789 | Xử lý luồng ltinput. Liên kết trực tiếp: WeaponItem. | WeaponItem |
| `private void HandleSwitchRightWeaponInput()` | 801 | Xử lý luồng switch right weapon input. Liên kết trực tiếp: PlayerUIManager. | PlayerUIManager |
| `private void HandleSwitchLeftWeaponInput()` | 820 | Xử lý luồng switch left weapon input. Liên kết trực tiếp: PlayerUIManager. | PlayerUIManager |
| `private void HandleSwitchQuickSlotItemInput()` | 839 | Xử lý luồng switch quick slot item input. Liên kết trực tiếp: PlayerUIManager. | PlayerUIManager |
| `private void HandleInteractionInput()` | 858 | Xử lý luồng interaction input. | - |
| `private void QueInput(ref bool quedInput)` | 868 | Thực hiện logic que input trong script PlayerInputManager. | - |
| `private void ProcessQuedInput()` | 883 | Thực hiện logic process qued input trong script PlayerInputManager. | - |
| `private void HandleQuedInput()` | 895 | Xử lý luồng qued input. | - |
| `private void HandleOpenCharacterMenuInput()` | 916 | Xử lý luồng open character menu input. Liên kết trực tiếp: PlayerUIManager. | PlayerUIManager |
| `private void HandleCloseUIInputs()` | 934 | Xử lý luồng close uiinputs. Liên kết trực tiếp: PlayerUIManager. | PlayerUIManager |

#### PlayerInteractionManager

- **Đường dẫn:** `Assets/Game/Scripts/Character/Player/PlayerInteractionManager.cs`
- **Loại:** MonoBehaviour
- **Vai trò dễ hiểu:** Module dành cho người chơi: input, camera, di chuyển, combat, inventory, stats, network hoặc hiệu ứng.
- **Kế thừa/cha:** MonoBehaviour
- **Script con:** -
- **Field public/serialized chính:** public List<Interactable> currentInteractableActions
- **Liên kết script:** Interactable, PlayerManager, PlayerUIManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `private void Awake()` | 15 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. Liên kết trực tiếp: PlayerManager. | PlayerManager |
| `private void Start()` | 20 | Chạy sau Awake để hoàn tất thiết lập ban đầu, nhất là khi cần các object khác đã sẵn sàng. Liên kết trực tiếp: Interactable. | Interactable |
| `private void FixedUpdate()` | 25 | Cập nhật theo bước vật lý, thường xử lý movement, trigger hoặc physics. Liên kết trực tiếp: PlayerUIManager. | PlayerUIManager |
| `private void CheckForInteractable()` | 47 | Thực hiện logic check for interactable trong script PlayerInteractionManager. Liên kết trực tiếp: PlayerUIManager. | PlayerUIManager |
| `private void RefreshInteractableList()` | 64 | Làm mới dữ liệu/hiển thị interactable list. | - |
| `public void AddInteractionToList(Interactable interactableObject)` | 73 | Thêm interaction to list vào danh sách, trạng thái hoặc dữ liệu. | - |
| `public void RemoveInteractionFromList(Interactable interactableObject)` | 81 | Loại bỏ interaction from list khỏi danh sách, trạng thái hoặc dữ liệu. | - |
| `public void Interact()` | 89 | Thực hiện hành động tương tác khi player chọn object này. Liên kết trực tiếp: PlayerUIManager. | PlayerUIManager |
| `public void ClearInteractionList()` | 118 | Thực hiện logic clear interaction list trong script PlayerInteractionManager. Liên kết trực tiếp: PlayerUIManager. | PlayerUIManager |
| `private bool IsValidInteractable(Interactable interactableObject)` | 124 | Kiểm tra điều kiện/trạng thái valid interactable. | - |

#### PlayerInventoryManager

- **Đường dẫn:** `Assets/Game/Scripts/Character/Player/PlayerInventoryManager.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Module dành cho người chơi: input, camera, di chuyển, combat, inventory, stats, network hoặc hiệu ứng.
- **Kế thừa/cha:** CharacterInventoryManager
- **Script con:** -
- **Field public/serialized chính:** public WeaponItem currentRightHandWeapon, public WeaponItem currentLeftHandWeapon, public WeaponItem currentTwoHandWeapon, public WeaponItem[] weaponsInRightHandSlots, public int rightHandWeaponIndex, public WeaponItem[] weaponsInLeftHandSlots, public int leftHandWeaponIndex, public SpellItem currentSpell, public QuickSlotItem[] quickSlotItemsInQuickSlots, public int quickSlotItemIndex, public QuickSlotItem currentQuickSlotItem, public HeadEquipmentItem headEquipment +6
- **Liên kết script:** BodyEquipmentItem, CharacterInventoryManager, HandEquipmentItem, HeadEquipmentItem, Item, LegEquipmentItem, QuickSlotItem, RangedProjectileItem, SpellItem, WeaponItem

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `protected override void Awake()` | 37 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. Liên kết trực tiếp: Item. | Item |
| `public void AddItemToInventory(Item item)` | 45 | Thêm item to inventory vào danh sách, trạng thái hoặc dữ liệu. Liên kết trực tiếp: Item. | Item |
| `public void RemoveItemFromInventory(Item item)` | 56 | Loại bỏ item from inventory khỏi danh sách, trạng thái hoặc dữ liệu. | - |
| `public bool TryRemoveItemFromInventory(Item item)` | 96 | Thử thực hiện remove item from inventory, thường có kiểm tra điều kiện trước khi chạy. | - |
| `public bool RemoveFirstItemByID(int itemID)` | 112 | Loại bỏ first item by id khỏi danh sách, trạng thái hoặc dữ liệu. | - |
| `public int GetInventoryCountByItemID(int itemID)` | 132 | Lấy dữ liệu inventory count by item id cho hệ thống khác sử dụng. Liên kết trực tiếp: QuickSlotItem, RangedProjectileItem. | QuickSlotItem, RangedProjectileItem |

#### PlayerLocomotionManager

- **Đường dẫn:** `Assets/Game/Scripts/Character/Player/PlayerLocomotionManager.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Module dành cho người chơi: input, camera, di chuyển, combat, inventory, stats, network hoặc hiệu ứng.
- **Kế thừa/cha:** CharacterLocomotionManager
- **Script con:** -
- **Field public/serialized chính:** public float verticalMovement, public float horizontalMovement, public float moveAmount
- **Liên kết script:** CharacterLocomotionManager, PlayerCamera, PlayerInputManager, PlayerManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `protected override void Awake()` | 39 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. Liên kết trực tiếp: PlayerManager. | PlayerManager |
| `protected override void Update()` | 46 | Cập nhật logic mỗi frame như input, timer, trạng thái chiến đấu hoặc UI. | - |
| `public void HandleAllMovement()` | 76 | Xử lý luồng all movement. | - |
| `private void GetMovementValues()` | 86 | Lấy dữ liệu movement values cho hệ thống khác sử dụng. Liên kết trực tiếp: PlayerInputManager. | PlayerInputManager |
| `private void HandleGroundedMovement()` | 94 | Xử lý luồng grounded movement. Liên kết trực tiếp: PlayerCamera. | PlayerCamera |
| `private void HandleJumpingMovement()` | 137 | Xử lý luồng jumping movement. | - |
| `private void HandleFreeFallMovement()` | 145 | Xử lý luồng free fall movement. Liên kết trực tiếp: PlayerCamera, PlayerInputManager. | PlayerCamera, PlayerInputManager |
| `private void HandleRotation()` | 159 | Xử lý luồng rotation. | - |
| `private void HandleAimRotations()` | 177 | Xử lý luồng aim rotations. Liên kết trực tiếp: PlayerCamera. | PlayerCamera |
| `private void HandleStandardRotation()` | 189 | Xử lý luồng standard rotation. Liên kết trực tiếp: PlayerCamera. | PlayerCamera |
| `public void HandleSprinting()` | 241 | Xử lý luồng sprinting. | - |
| `public void AttemptToPerformDodge()` | 272 | Cố gắng kích hoạt to perform dodge nếu trạng thái hiện tại cho phép. Liên kết trực tiếp: PlayerCamera, PlayerInputManager. | PlayerCamera, PlayerInputManager |
| `public void AttemptToPerformJump()` | 309 | Cố gắng kích hoạt to perform jump nếu trạng thái hiện tại cho phép. Liên kết trực tiếp: PlayerCamera, PlayerInputManager. | PlayerCamera, PlayerInputManager |
| `public void ApplyJumpingVelocity()` | 362 | Áp dụng jumping velocity lên character/object mục tiêu. | - |
| `private void MoveAtRegularSpeed()` | 369 | Thực hiện logic move at regular speed trong script PlayerLocomotionManager. Liên kết trực tiếp: PlayerInputManager. | PlayerInputManager |
| `private void MoveAtSprintingSpeed()` | 392 | Thực hiện logic move at sprinting speed trong script PlayerLocomotionManager. | - |
| `private void MoveAtSneakingSpeed()` | 400 | Thực hiện logic move at sneaking speed trong script PlayerLocomotionManager. Liên kết trực tiếp: PlayerInputManager. | PlayerInputManager |

#### PlayerManager

- **Đường dẫn:** `Assets/Game/Scripts/Character/Player/PlayerManager.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Module dành cho người chơi: input, camera, di chuyển, combat, inventory, stats, network hoặc hiệu ứng.
- **Kế thừa/cha:** CharacterManager
- **Script con:** -
- **Field public/serialized chính:** public PlayerAnimatorManager playerAnimatorManager, public PlayerLocomotionManager playerLocomotionManager, public PlayerNetworkManager playerNetworkManager, public PlayerStatsManager playerStatsManager, public PlayerInventoryManager playerInventoryManager, public PlayerEquipmentManager playerEquipmentManager, public PlayerCombatManager playerCombatManager, public PlayerInteractionManager playerInteractionManager, public PlayerEffectsManager playerEffectsManager, public PlayerBodyManager playerBodyManager, public PlayerShopManager playerShopManager, public WorldLocationSceneSet areaCurrentlyIn
- **Liên kết script:** BodyEquipmentItem, BuffCharmItem, BuildRuntimeLogger, CharacterManager, CharacterSaveData, EquipmentItem, GameProgressionManager, HandEquipmentItem, HeadEquipmentItem, LegEquipmentItem, PlayerAnimatorManager, PlayerBodyManager, PlayerCamera, PlayerCombatManager, PlayerEffectsManager, PlayerEquipmentManager +22

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `protected override void Awake()` | 30 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. Liên kết trực tiếp: PlayerAnimatorManager, PlayerBodyManager, PlayerCombatManager, PlayerEffectsManager, PlayerEquipmentManager +6. | PlayerAnimatorManager, PlayerBodyManager, PlayerCombatManager, PlayerEffectsManager, PlayerEquipmentManager, PlayerInteractionManager, PlayerInventoryManager, PlayerLocomotionManager, PlayerNetworkManager, PlayerShopManager +1 |
| `protected override void Update()` | 50 | Cập nhật logic mỗi frame như input, timer, trạng thái chiến đấu hoặc UI. | - |
| `protected override void LateUpdate()` | 65 | Cập nhật cuối frame, thường dùng cho camera, animation hoặc đồng bộ trạng thái sau movement. Liên kết trực tiếp: PlayerCamera. | PlayerCamera |
| `protected override void OnEnable()` | 75 | Đăng ký event/callback hoặc bật trạng thái khi component được enable. | - |
| `protected override void OnDisable()` | 80 | Gỡ event/callback hoặc dọn trạng thái khi component bị disable. | - |
| `public override void OnNetworkSpawn()` | 85 | Thiết lập dữ liệu và callback khi NetworkObject được spawn trong Netcode. Liên kết trực tiếp: BuildRuntimeLogger, PlayerCamera, PlayerInputManager, PlayerUIManager, WorldSaveGameManager. | BuildRuntimeLogger, PlayerCamera, PlayerInputManager, PlayerUIManager, WorldSaveGameManager |
| `public override void OnNetworkDespawn()` | 203 | Dọn dữ liệu/callback khi NetworkObject bị despawn khỏi Netcode. Liên kết trực tiếp: PlayerUIManager. | PlayerUIManager |
| `private void OnClientConnectedCallback(ulong clientID)` | 295 | Thực hiện logic on client connected callback trong script PlayerManager. Liên kết trực tiếp: BuildRuntimeLogger, PlayerUIManager, WorldAIManager, WorldGameSessionManager. | BuildRuntimeLogger, PlayerUIManager, WorldAIManager, WorldGameSessionManager |
| `private IEnumerator EmergeAtMostRecentSiteOfGrace()` | 323 | Thực hiện logic emerge at most recent site of grace trong script PlayerManager. Liên kết trực tiếp: PlayerUIManager, WorldGameSessionManager, WorldObjectManager, WorldSaveGameManager. | PlayerUIManager, WorldGameSessionManager, WorldObjectManager, WorldSaveGameManager |
| `new WaitForSeconds(1.5f)` | 325 | Thực hiện logic wait for seconds trong script PlayerManager. | - |
| `public override IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false)` | 358 | Thực hiện logic process death event trong script PlayerManager. Liên kết trực tiếp: GameProgressionManager, PlayerUIManager, WorldGameSessionManager. | GameProgressionManager, PlayerUIManager, WorldGameSessionManager |
| `return StartCoroutine(base.ProcessDeathEvent(manuallySelectDeathAnimation))` | 389 | Thực hiện logic start coroutine trong script PlayerManager. | - |
| `public override void ReviveCharacter()` | 392 | Thực hiện logic revive character trong script PlayerManager. | - |
| `public void SaveGameDataToCurrentCharacterData(ref CharacterSaveData currentCharacterData)` | 406 | Lưu dữ liệu liên quan tới game data to current character data. Liên kết trực tiếp: BodyEquipmentItem, HandEquipmentItem, HeadEquipmentItem, LegEquipmentItem, QuickSlotItem +7. | BodyEquipmentItem, HandEquipmentItem, HeadEquipmentItem, LegEquipmentItem, QuickSlotItem, RangedProjectileItem, SerializableActiveBuff, SerializableQuickSlotItem, SerializableRangedProjectile, SerializableWeapon +2 |
| `public void LoadGameDataFromCurrentCharacterData(ref CharacterSaveData currentCharacterData)` | 516 | Nạp dữ liệu hoặc scene liên quan tới game data from current character data. Liên kết trực tiếp: BodyEquipmentItem, BuildRuntimeLogger, EquipmentItem, HandEquipmentItem, HeadEquipmentItem +7. | BodyEquipmentItem, BuildRuntimeLogger, EquipmentItem, HandEquipmentItem, HeadEquipmentItem, LegEquipmentItem, PlayerUIManager, QuickSlotItem, RangedProjectileItem, SpellItem +2 |
| `new Vector3( currentCharacterData.xPosition, currentCharacterData.yPosition, currentCharacterData.zPosition)` | 524 | Thực hiện logic vector3 trong script PlayerManager. | - |
| `public void LoadOtherPlayerCharacterWhenJoiningServer()` | 719 | Nạp dữ liệu hoặc scene liên quan tới other player character when joining server. | - |
| `private void EnsureDefaultBuffCharmsAvailable(bool fillEmptyQuickSlots)` | 764 | Thực hiện logic ensure default buff charms available trong script PlayerManager. Liên kết trực tiếp: BuffCharmItem, WorldItemDatabase. | BuffCharmItem, WorldItemDatabase |
| `private bool PlayerAlreadyHasQuickSlotItem(int itemID)` | 799 | Phát er already has quick slot item, thường là animation, sound hoặc VFX. Liên kết trực tiếp: QuickSlotItem. | QuickSlotItem |

#### PlayerNetworkManager

- **Đường dẫn:** `Assets/Game/Scripts/Character/Player/PlayerNetworkManager.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Module dành cho người chơi: input, camera, di chuyển, combat, inventory, stats, network hoặc hiệu ứng.
- **Kế thừa/cha:** CharacterNetworkManager
- **Script con:** -
- **Field public/serialized chính:** public NetworkVariable<FixedString64Bytes> characterName, public NetworkVariable<int> lastSiteOfGraceUsed, public NetworkVariable<int> remainingHealthFlasks, public NetworkVariable<int> remainingFocusPointsFlasks, public NetworkVariable<bool> isChugging, public NetworkVariable<bool> isUsingRightHand, public NetworkVariable<bool> isUsingLeftHand, public NetworkVariable<int> hairStyleID, public NetworkVariable<float> hairColorRed, public NetworkVariable<float> hairColorGreen, public NetworkVariable<float> hairColorBlue, public NetworkVariable<int> currentWeaponBeingUsed +20
- **Liên kết script:** BodyEquipmentItem, BuildUp, CharacterNetworkManager, EquipmentType, HandEquipmentItem, HeadEquipmentItem, Item, LegEquipmentItem, PlayerCamera, PlayerManager, PlayerUIManager, QuickSlotItem, RangedProjectileDamageCollider, RangedProjectileItem, SessionEndGameActionType, SpellItem +12

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `protected override void Awake()` | 163 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. Liên kết trực tiếp: PlayerManager. | PlayerManager |
| `public override void OnIsDeadChanged(bool oldStatus, bool newStatus)` | 169 | Thực hiện logic on is dead changed trong script PlayerNetworkManager. Liên kết trực tiếp: PlayerUIManager, WorldAIManager. | PlayerUIManager, WorldAIManager |
| `public void ReportDeathForLoseConditionServerRpc(int mapIndex)` | 187 | Gửi yêu cầu lên server trong Netcode để server xử lý report death for lose condition. Liên kết trực tiếp: WorldGameSessionManager. | WorldGameSessionManager |
| `private void BroadcastSessionLoseClientRpc(ulong failedPlayerClientId, int deathCount, int mapIndex)` | 199 | Server gọi xuống client để đồng bộ hoặc phát hiệu ứng cho broadcast session lose. Liên kết trực tiếp: WorldGameSessionManager. | WorldGameSessionManager |
| `public void RequestSynchronizedEndGameActionServerRpc(int actionID)` | 208 | Gửi yêu cầu lên server trong Netcode để server xử lý request synchronized end game action. Liên kết trực tiếp: SessionEndGameActionType, WorldGameSessionManager. | SessionEndGameActionType, WorldGameSessionManager |
| `private void ExecuteSynchronizedEndGameActionClientRpc(int actionID, bool shouldShowLoadingScreen)` | 223 | Server gọi xuống client để đồng bộ hoặc phát hiệu ứng cho execute synchronized end game action. Liên kết trực tiếp: PlayerUIManager, SessionEndGameActionType, WorldGameSessionManager. | PlayerUIManager, SessionEndGameActionType, WorldGameSessionManager |
| `public override void OnIsBleedingChanged(bool oldStatus, bool newStatus)` | 234 | Thực hiện logic on is bleeding changed trong script PlayerNetworkManager. Liên kết trực tiếp: BuildUp, PlayerUIManager, WorldCharacterEffectsManager. | BuildUp, PlayerUIManager, WorldCharacterEffectsManager |
| `public override void OnIsPoisonedChanged(bool oldStatus, bool newStatus)` | 251 | Thực hiện logic on is poisoned changed trong script PlayerNetworkManager. Liên kết trực tiếp: BuildUp, PlayerUIManager, WorldCharacterEffectsManager. | BuildUp, PlayerUIManager, WorldCharacterEffectsManager |
| `public override void OnIsBurningChanged(bool oldStatus, bool newStatus)` | 284 | Thực hiện logic on is burning changed trong script PlayerNetworkManager. Liên kết trực tiếp: BuildUp, PlayerUIManager. | BuildUp, PlayerUIManager |
| `public override void OnIsFrostBittenChanged(bool oldStatus, bool newStatus)` | 297 | Thực hiện logic on is frost bitten changed trong script PlayerNetworkManager. Liên kết trực tiếp: BuildUp, PlayerUIManager, WorldCharacterEffectsManager. | BuildUp, PlayerUIManager, WorldCharacterEffectsManager |
| `public override void OnIsFrozenChanged(bool oldStatus, bool newStatus)` | 323 | Thực hiện logic on is frozen changed trong script PlayerNetworkManager. | - |
| `private void RefreshHealthBarStatusColor()` | 333 | Làm mới dữ liệu/hiển thị health bar status color. Liên kết trực tiếp: PlayerUIManager, WorldUtilityManager. | PlayerUIManager, WorldUtilityManager |
| `public void OnIsSneakingChanged(bool oldStatus, bool newStatus)` | 352 | Thực hiện logic on is sneaking changed trong script PlayerNetworkManager. | - |
| `public void SetCharacterActionHand(bool rightHandedAction)` | 357 | Thiết lập giá trị hoặc trạng thái character action hand. | - |
| `public void SetNewMaxHealthValue(int oldVitality, int newVitality)` | 371 | Thiết lập giá trị hoặc trạng thái new max health value. Liên kết trực tiếp: PlayerUIManager. | PlayerUIManager |
| `public void SetNewMaxStaminaValue(int oldEndurance, int newEndurance)` | 378 | Thiết lập giá trị hoặc trạng thái new max stamina value. Liên kết trực tiếp: PlayerUIManager. | PlayerUIManager |
| `public void SetNewMaxFocusPointsValue(int oldMind, int newMind)` | 385 | Thiết lập giá trị hoặc trạng thái new max focus points value. Liên kết trực tiếp: PlayerUIManager. | PlayerUIManager |
| `public void SetNewMaxBuildUpCapacityValue(int oldVitality, int newVitality)` | 392 | Thiết lập giá trị hoặc trạng thái new max build up capacity value. Liên kết trực tiếp: PlayerUIManager. | PlayerUIManager |
| `public void OnHairStyleIDChanged(int oldValue, int newValue)` | 398 | Thực hiện logic on hair style idchanged trong script PlayerNetworkManager. | - |
| `public void OnHairColorRedChanged(float oldValue, float newValue)` | 403 | Thực hiện logic on hair color red changed trong script PlayerNetworkManager. | - |
| `public void OnHairColorGreenChanged(float oldValue, float newValue)` | 408 | Thực hiện logic on hair color green changed trong script PlayerNetworkManager. | - |
| `public void OnHairColorBlueChanged(float oldValue, float newValue)` | 413 | Thực hiện logic on hair color blue changed trong script PlayerNetworkManager. | - |
| `public void OnCurrentRightHandWeaponIDChange(int oldID, int newID)` | 418 | Thực hiện logic on current right hand weapon idchange trong script PlayerNetworkManager. Liên kết trực tiếp: PlayerUIManager, WeaponClass, WeaponItem, WorldItemDatabase. | PlayerUIManager, WeaponClass, WeaponItem, WorldItemDatabase |
| `public void OnCurrentLeftHandWeaponIDChange(int oldID, int newID)` | 443 | Thực hiện logic on current left hand weapon idchange trong script PlayerNetworkManager. Liên kết trực tiếp: PlayerUIManager, WeaponClass, WeaponItem, WorldItemDatabase. | PlayerUIManager, WeaponClass, WeaponItem, WorldItemDatabase |
| `public void OnCurrentWeaponBeingUsedIDChange(int oldID, int newID)` | 468 | Thực hiện logic on current weapon being used idchange trong script PlayerNetworkManager. Liên kết trực tiếp: Item, WeaponItem, WorldItemDatabase. | Item, WeaponItem, WorldItemDatabase |
| `private WeaponItem GetEquippedWeaponByID(int weaponID)` | 493 | Lấy dữ liệu equipped weapon by id cho hệ thống khác sử dụng. | - |
| `public void OnCurrentSpellIDChange(int oldID, int newID)` | 510 | Thực hiện logic on current spell idchange trong script PlayerNetworkManager. Liên kết trực tiếp: PlayerUIManager, SpellItem, WorldItemDatabase. | PlayerUIManager, SpellItem, WorldItemDatabase |
| `public void OnCurrentQuickSlotItemIDChange(int oldID, int newID)` | 526 | Thực hiện logic on current quick slot item idchange trong script PlayerNetworkManager. Liên kết trực tiếp: PlayerUIManager, QuickSlotItem, WorldItemDatabase. | PlayerUIManager, QuickSlotItem, WorldItemDatabase |
| `public void OnMainProjectileIDChange(int oldID, int newID)` | 572 | Thực hiện logic on main projectile idchange trong script PlayerNetworkManager. Liên kết trực tiếp: PlayerUIManager, RangedProjectileItem, WorldItemDatabase. | PlayerUIManager, RangedProjectileItem, WorldItemDatabase |
| `public void OnSecondaryProjectileIDChange(int oldID, int newID)` | 586 | Thực hiện logic on secondary projectile idchange trong script PlayerNetworkManager. Liên kết trực tiếp: PlayerUIManager, RangedProjectileItem, WorldItemDatabase. | PlayerUIManager, RangedProjectileItem, WorldItemDatabase |
| `public void OnMaxFocusPointsChanged(int oldFP, int newFP)` | 600 | Thực hiện logic on max focus points changed trong script PlayerNetworkManager. Liên kết trực tiếp: PlayerUIManager. | PlayerUIManager |
| `public void OnFocusPointsChanged(int oldFP, int newFP)` | 606 | Thực hiện logic on focus points changed trong script PlayerNetworkManager. Liên kết trực tiếp: PlayerUIManager. | PlayerUIManager |
| `public void OnIsHoldingArrowChanged(bool oldStatus, bool newStatus)` | 612 | Thực hiện logic on is holding arrow changed trong script PlayerNetworkManager. | - |
| `public void OnIsAimingChanged(bool oldStatus, bool newStatus)` | 617 | Thực hiện logic on is aiming changed trong script PlayerNetworkManager. Liên kết trực tiếp: PlayerCamera, PlayerUIManager. | PlayerCamera, PlayerUIManager |
| `new Vector3(0, 0, 0)` | 621 | Thực hiện logic vector3 trong script PlayerNetworkManager. | - |
| `new Vector3(0, PlayerCamera.instance.cameraPivotYPositionOffSet, 0)` | 624 | Thực hiện logic vector3 trong script PlayerNetworkManager. | - |
| `new Vector3(0, 0, 0)` | 629 | Thực hiện logic vector3 trong script PlayerNetworkManager. | - |
| `new Vector3(0, 0, 0)` | 630 | Thực hiện logic vector3 trong script PlayerNetworkManager. | - |
| `public void OnIsChargingRightSpellChanged(bool oldStatus, bool newStatus)` | 638 | Thực hiện logic on is charging right spell changed trong script PlayerNetworkManager. | - |
| `public void OnIsChargingLeftSpellChanged(bool oldStatus, bool newStatus)` | 643 | Thực hiện logic on is charging left spell changed trong script PlayerNetworkManager. | - |
| `public override void OnIsBlockingChanged(bool oldStatus, bool newStatus)` | 648 | Thực hiện logic on is blocking changed trong script PlayerNetworkManager. | - |
| `public void OnIsTwoHandingWeaponChanged(bool oldStatus, bool newStatus)` | 663 | Thực hiện logic on is two handing weapon changed trong script PlayerNetworkManager. Liên kết trực tiếp: StaticCharacterEffect, WorldCharacterEffectsManager. | StaticCharacterEffect, WorldCharacterEffectsManager |
| `public void OnIsTwoHandingRightWeaponChanged(bool oldStatus, bool newStatus)` | 690 | Thực hiện logic on is two handing right weapon changed trong script PlayerNetworkManager. | - |
| `public void OnIsTwoHandingLeftWeaponChanged(bool oldStatus, bool newStatus)` | 705 | Thực hiện logic on is two handing left weapon changed trong script PlayerNetworkManager. | - |
| `public void OnIsChuggingChanged(bool oldStatus, bool newStatus)` | 720 | Thực hiện logic on is chugging changed trong script PlayerNetworkManager. | - |
| `public void OnHeadEquipmentChanged(int oldValue, int newValue)` | 725 | Thực hiện logic on head equipment changed trong script PlayerNetworkManager. Liên kết trực tiếp: HeadEquipmentItem, WorldItemDatabase. | HeadEquipmentItem, WorldItemDatabase |
| `public void OnBodyEquipmentChanged(int oldValue, int newValue)` | 743 | Thực hiện logic on body equipment changed trong script PlayerNetworkManager. Liên kết trực tiếp: BodyEquipmentItem, WorldItemDatabase. | BodyEquipmentItem, WorldItemDatabase |
| `public void OnLegEquipmentChanged(int oldValue, int newValue)` | 761 | Thực hiện logic on leg equipment changed trong script PlayerNetworkManager. Liên kết trực tiếp: LegEquipmentItem, WorldItemDatabase. | LegEquipmentItem, WorldItemDatabase |
| `public void OnHandEquipmentChanged(int oldValue, int newValue)` | 779 | Thực hiện logic on hand equipment changed trong script PlayerNetworkManager. Liên kết trực tiếp: HandEquipmentItem, WorldItemDatabase. | HandEquipmentItem, WorldItemDatabase |
| `public void OnIsMaleChanged(bool oldStatus, bool newStatus)` | 797 | Thực hiện logic on is male changed trong script PlayerNetworkManager. | - |
| `public void NotifyTheServerOfWeaponActionServerRpc(ulong clientID, int actionID, int weaponID)` | 803 | Gửi yêu cầu lên server trong Netcode để server xử lý notify the server of weapon action. | - |
| `private void NotifyTheServerOfWeaponActionClientRpc(ulong clientID, int actionID, int weaponID)` | 812 | Server gọi xuống client để đồng bộ hoặc phát hiệu ứng cho notify the server of weapon action. | - |
| `private void PerformWeaponBasedAction(int actionID, int weaponID)` | 820 | Thực hiện logic perform weapon based action trong script PlayerNetworkManager. Liên kết trực tiếp: WeaponItem, WeaponItemAction, WorldActionManager, WorldItemDatabase. | WeaponItem, WeaponItemAction, WorldActionManager, WorldItemDatabase |
| `public override void DestroyAllCurrentActionFXClientRpc()` | 839 | Server gọi xuống client để đồng bộ hoặc phát hiệu ứng cho destroy all current action fx. | - |
| `public void NotifyServerOfDrawnProjectileServerRpc(int projectileID)` | 875 | Gửi yêu cầu lên server trong Netcode để server xử lý notify server of drawn projectile. | - |
| `private void NotifyServerOfDrawnProjectileClientRpc(int projectileID)` | 884 | Server gọi xuống client để đồng bộ hoặc phát hiệu ứng cho notify server of drawn projectile. Liên kết trực tiếp: WorldItemDatabase, WorldSoundFXManager. | WorldItemDatabase, WorldSoundFXManager |
| `public void NotifyServerOfReleasedProjectileServerRpc(ulong playerClientID, int projectileID, float xPosition, float yPosition, float zPosition, float yCharacterRotation)` | 911 | Gửi yêu cầu lên server trong Netcode để server xử lý notify server of released projectile. | - |
| `public void NotifyServerOfReleasedProjectileClientRpc(ulong playerClientID, int projectileID, float xPosition, float yPosition, float zPosition, float yCharacterRotation)` | 920 | Server gọi xuống client để đồng bộ hoặc phát hiệu ứng cho notify server of released projectile. | - |
| `private void PerformReleasedProjectileFromRpc(int projectileID, float xPosition, float yPosition, float zPosition, float yCharacterRotation)` | 926 | Thực hiện logic perform released projectile from rpc trong script PlayerNetworkManager. Liên kết trực tiếp: RangedProjectileDamageCollider, RangedProjectileItem, WorldItemDatabase. | RangedProjectileDamageCollider, RangedProjectileItem, WorldItemDatabase |
| `new Vector3(xPosition, yPosition, zPosition))` | 958 | Thực hiện logic vector3 trong script PlayerNetworkManager. | - |
| `public void HideWeaponsServerRPC()` | 996 | Thực hiện logic hide weapons server rpc trong script PlayerNetworkManager. | - |
| `private void HideWeaponsClientRPC()` | 1003 | Thực hiện logic hide weapons client rpc trong script PlayerNetworkManager. | - |
| `public void NotifyServerOfQuickSlotItemActionServerRpc(ulong clientID, int quickSlotItemID)` | 1013 | Gửi yêu cầu lên server trong Netcode để server xử lý notify server of quick slot item action. | - |
| `private void NotifyServerOfQuickSlotItemActionClientRpc(ulong clientID, int quickSlotItemID)` | 1019 | Server gọi xuống client để đồng bộ hoặc phát hiệu ứng cho notify server of quick slot item action. Liên kết trực tiếp: QuickSlotItem, WorldItemDatabase. | QuickSlotItem, WorldItemDatabase |
| `public void SyncWeaponUpgradeServerRpc(int equipmentSlot, int upgradedItemID, int newUpgradeLevel)` | 1029 | Gửi yêu cầu lên server trong Netcode để server xử lý sync weapon upgrade. Liên kết trực tiếp: EquipmentType. | EquipmentType |
| `private void ApplyWeaponUpgradeState(EquipmentType equipmentSlot, int upgradedItemID, int newUpgradeLevel)` | 1034 | Áp dụng weapon upgrade state lên character/object mục tiêu. Liên kết trực tiếp: EquipmentType, UpgradeLevel. | EquipmentType, UpgradeLevel |
| `private void UpdateWeaponUpgradeLevel(WeaponItem weapon, int upgradedItemID, UpgradeLevel upgradedLevel)` | 1071 | Cập nhật weapon upgrade level theo trạng thái mới. | - |

#### PlayerShopManager

- **Đường dẫn:** `Assets/Game/Scripts/Character/Player/PlayerShopManager.cs`
- **Loại:** MonoBehaviour
- **Vai trò dễ hiểu:** Module dành cho người chơi: input, camera, di chuyển, combat, inventory, stats, network hoặc hiệu ứng.
- **Kế thừa/cha:** MonoBehaviour
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** CharacterSlot, Item, PlayerManager, PlayerUIManager, ShopInventory, ShopStockEntry, WorldItemDatabase, WorldSaveGameManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `private void Awake()` | 11 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. Liên kết trực tiếp: PlayerManager. | PlayerManager |
| `public bool TryBuyItem(ShopStockEntry entry, ShopInventory shopInventory = null)` | 16 | Thử thực hiện buy item, thường có kiểm tra điều kiện trước khi chạy. Liên kết trực tiếp: Item. | Item |
| `public bool TrySellItem(Item item, ShopInventory shopInventory = null)` | 48 | Thử thực hiện sell item, thường có kiểm tra điều kiện trước khi chạy. | - |
| `public List<Item> GetSellableInventoryItems()` | 71 | Lấy dữ liệu sellable inventory items cho hệ thống khác sử dụng. Liên kết trực tiếp: Item. | Item |
| `public int GetOwnedAmount(Item item)` | 91 | Lấy dữ liệu owned amount cho hệ thống khác sử dụng. | - |
| `public int GetCurrentRunes()` | 99 | Lấy dữ liệu current runes cho hệ thống khác sử dụng. | - |
| `private void SyncBuyItemServerRpc(int itemID, int price)` | 108 | Gửi yêu cầu lên server trong Netcode để server xử lý sync buy item. Liên kết trực tiếp: Item, WorldItemDatabase. | Item, WorldItemDatabase |
| `private void SyncSellItemServerRpc(int itemID, int sellPrice)` | 121 | Gửi yêu cầu lên server trong Netcode để server xử lý sync sell item. | - |
| `private Item CreatePurchasedItem(Item shopItem)` | 130 | Tạo object/dữ liệu purchased item. Liên kết trực tiếp: Item, WorldItemDatabase. | Item, WorldItemDatabase |
| `return Instantiate(shopItem)` | 144 | Thực hiện logic instantiate trong script PlayerShopManager. | - |
| `private void RefreshOwnedPlayerUI()` | 147 | Làm mới dữ liệu/hiển thị owned player ui. Liên kết trực tiếp: PlayerUIManager. | PlayerUIManager |
| `private void TryAutoSave()` | 159 | Thử thực hiện auto save, thường có kiểm tra điều kiện trước khi chạy. Liên kết trực tiếp: CharacterSlot, WorldSaveGameManager. | CharacterSlot, WorldSaveGameManager |

#### PlayerSoundFXManager

- **Đường dẫn:** `Assets/Game/Scripts/Character/Player/PlayerSoundFXManager.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Module dành cho người chơi: input, camera, di chuyển, combat, inventory, stats, network hoặc hiệu ứng.
- **Kế thừa/cha:** CharacterSoundFXManager
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** CharacterSoundFXManager, PlayerManager, WorldSoundFXManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `protected override void Awake()` | 9 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. Liên kết trực tiếp: PlayerManager. | PlayerManager |
| `public override void PlayBlockSoundFX()` | 16 | Phát block sound fx, thường là animation, sound hoặc VFX. Liên kết trực tiếp: WorldSoundFXManager. | WorldSoundFXManager |
| `public override void PlayFootStepSoundFX()` | 21 | Phát foot step sound fx, thường là animation, sound hoặc VFX. Liên kết trực tiếp: WorldSoundFXManager. | WorldSoundFXManager |

#### PlayerStatsManager

- **Đường dẫn:** `Assets/Game/Scripts/Character/Player/PlayerStatsManager.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Module dành cho người chơi: input, camera, di chuyển, combat, inventory, stats, network hoặc hiệu ứng.
- **Kế thừa/cha:** CharacterStatsManager
- **Script con:** -
- **Field public/serialized chính:** public int runes, public int maxHealthBuff, public int maxStaminaBuff, public int maxFocusPointsBuff, public float outgoingDamageBonusPercentage
- **Liên kết script:** CharacterStatsManager, PlayerManager, PlayerUIManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `protected override void Awake()` | 19 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. Liên kết trực tiếp: PlayerManager. | PlayerManager |
| `protected override void Start()` | 26 | Chạy sau Awake để hoàn tất thiết lập ban đầu, nhất là khi cần các object khác đã sẵn sàng. | - |
| `public int CalculateModifiedMaxHealth()` | 37 | Tính toán modified max health từ chỉ số hoặc dữ liệu hiện có. | - |
| `1, CalculateHealthBasedOnVitalityLevel(player.playerNetworkManager.vigor.Value) + maxHealthBuff)` | 39 | Tính toán health based on vitality level từ chỉ số hoặc dữ liệu hiện có. | - |
| `public int CalculateModifiedMaxStamina()` | 42 | Tính toán modified max stamina từ chỉ số hoặc dữ liệu hiện có. | - |
| `1, CalculateStaminaBasedOnEnduranceLevel(player.playerNetworkManager.endurance.Value) + maxStaminaBuff)` | 44 | Tính toán stamina based on endurance level từ chỉ số hoặc dữ liệu hiện có. | - |
| `public int CalculateModifiedMaxFocusPoints()` | 47 | Tính toán modified max focus points từ chỉ số hoặc dữ liệu hiện có. | - |
| `0, CalculateFocusPointsBasedOnMindLevel(player.playerNetworkManager.mind.Value) + maxFocusPointsBuff)` | 49 | Tính toán focus points based on mind level từ chỉ số hoặc dữ liệu hiện có. | - |
| `public float GetOutgoingDamageMultiplier()` | 52 | Lấy dữ liệu outgoing damage multiplier cho hệ thống khác sử dụng. | - |
| `public void RefreshDerivedStats()` | 57 | Làm mới dữ liệu/hiển thị derived stats. Liên kết trực tiếp: PlayerUIManager. | PlayerUIManager |
| `public void CalculateTotalArmorAbsorption()` | 89 | Tính toán total armor absorption từ chỉ số hoặc dữ liệu hiện có. | - |
| `public void AddRunes(int runesToAdd)` | 186 | Thêm runes vào danh sách, trạng thái hoặc dữ liệu. Liên kết trực tiếp: PlayerUIManager. | PlayerUIManager |

### Assets/Game/Scripts/Character/Player/PlayerUI

#### PlayerUICharacterMenuManager

- **Đường dẫn:** `Assets/Game/Scripts/Character/Player/PlayerUI/PlayerUICharacterMenuManager.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Điều khiển một phần UI của player như HUD, menu, popup, shop, level up, site of grace hoặc equipment.
- **Kế thừa/cha:** PlayerUIMenu
- **Script con:** -
- **Field public/serialized chính:** [SerializeField] private RectTransform serializedJoinWorldControlsRoot, [SerializeField] private TextMeshProUGUI serializedWorldAddressLabel, [SerializeField] private TMP_InputField serializedJoinWorldAddressInputField, [SerializeField] private Button serializedCheckCodeButton, [SerializeField] private Button serializedJoinWorldButton, [SerializeField] private TextMeshProUGUI serializedJoinStatusLabel
- **Liên kết script:** PlayerUIManager, PlayerUIMenu, WorldGameSessionManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `private void OnEnable()` | 40 | Đăng ký event/callback hoặc bật trạng thái khi component được enable. Liên kết trực tiếp: WorldGameSessionManager. | WorldGameSessionManager |
| `private void OnDisable()` | 46 | Gỡ event/callback hoặc dọn trạng thái khi component bị disable. Liên kết trực tiếp: WorldGameSessionManager. | WorldGameSessionManager |
| `public override void OpenMenu()` | 52 | Mở UI/trạng thái/luồng menu. | - |
| `private void OnCurrentConnectionAddressChanged()` | 61 | Thực hiện logic on current connection address changed trong script PlayerUICharacterMenuManager. | - |
| `private void EnsureMenuButtons()` | 67 | Thực hiện logic ensure menu buttons trong script PlayerUICharacterMenuManager. | - |
| `private void OpenShopMenu()` | 103 | Mở UI/trạng thái/luồng shop menu. Liên kết trực tiếp: PlayerUIManager. | PlayerUIManager |
| `private void OpenSettingsMenu()` | 112 | Mở UI/trạng thái/luồng settings menu. Liên kết trực tiếp: PlayerUIManager. | PlayerUIManager |
| `private void EnsureJoinWorldUI()` | 120 | Thực hiện logic ensure join world ui trong script PlayerUICharacterMenuManager. | - |
| `private bool UseSerializedJoinWorldUI()` | 154 | Thực hiện logic use serialized join world ui trong script PlayerUICharacterMenuManager. | - |
| `private void TryFindSerializedJoinWorldUIByName()` | 202 | Thử thực hiện find serialized join world uiby name, thường có kiểm tra điều kiện trước khi chạy. | - |
| `private RectTransform CreateControlsRoot(RectTransform menuRoot)` | 233 | Tạo object/dữ liệu controls root. | - |
| `new GameObject("Join World Controls", typeof(RectTransform), typeof(Image))` | 235 | Thực hiện logic game object trong script PlayerUICharacterMenuManager. | - |
| `new Vector2(1f, 1f)` | 241 | Thực hiện logic vector2 trong script PlayerUICharacterMenuManager. | - |
| `new Vector2(1f, 1f)` | 242 | Thực hiện logic vector2 trong script PlayerUICharacterMenuManager. | - |
| `new Vector2(1f, 1f)` | 243 | Thực hiện logic vector2 trong script PlayerUICharacterMenuManager. | - |
| `new Vector2(-48f, -48f)` | 244 | Thực hiện logic vector2 trong script PlayerUICharacterMenuManager. | - |
| `new Vector2(430f, 410f)` | 245 | Thực hiện logic vector2 trong script PlayerUICharacterMenuManager. | - |
| `new Color(0f, 0f, 0f, 0.72f)` | 247 | Thực hiện logic color trong script PlayerUICharacterMenuManager. | - |
| `private TextMeshProUGUI CreateInfoLabel(RectTransform parent, TextMeshProUGUI textTemplate)` | 252 | Tạo object/dữ liệu info label. | - |
| `new GameObject("World Address Label", typeof(RectTransform), typeof(TextMeshProUGUI))` | 254 | Thực hiện logic game object trong script PlayerUICharacterMenuManager. | - |
| `new Vector2(0f, 1f)` | 259 | Thực hiện logic vector2 trong script PlayerUICharacterMenuManager. | - |
| `new Vector2(1f, 1f)` | 260 | Thực hiện logic vector2 trong script PlayerUICharacterMenuManager. | - |
| `new Vector2(0.5f, 1f)` | 261 | Thực hiện logic vector2 trong script PlayerUICharacterMenuManager. | - |
| `new Vector2(0f, -18f)` | 262 | Thực hiện logic vector2 trong script PlayerUICharacterMenuManager. | - |
| `new Vector2(-32f, 86f)` | 263 | Thực hiện logic vector2 trong script PlayerUICharacterMenuManager. | - |
| `private TextMeshProUGUI CreateStatusLabel(RectTransform parent, TextMeshProUGUI textTemplate)` | 276 | Tạo object/dữ liệu status label. | - |
| `new GameObject("Join Status Label", typeof(RectTransform), typeof(TextMeshProUGUI))` | 278 | Thực hiện logic game object trong script PlayerUICharacterMenuManager. | - |
| `new Vector2(0f, 1f)` | 283 | Thực hiện logic vector2 trong script PlayerUICharacterMenuManager. | - |
| `new Vector2(1f, 1f)` | 284 | Thực hiện logic vector2 trong script PlayerUICharacterMenuManager. | - |
| `new Vector2(0.5f, 1f)` | 285 | Thực hiện logic vector2 trong script PlayerUICharacterMenuManager. | - |
| `new Vector2(0f, -352f)` | 286 | Thực hiện logic vector2 trong script PlayerUICharacterMenuManager. | - |
| `new Vector2(-32f, 42f)` | 287 | Thực hiện logic vector2 trong script PlayerUICharacterMenuManager. | - |
| `private TMP_InputField CreateAddressInputField(RectTransform parent, TextMeshProUGUI textTemplate)` | 300 | Tạo object/dữ liệu address input field. | - |
| `new GameObject("Join World Address Input", typeof(RectTransform), typeof(Image), typeof(TMP_InputField))` | 302 | Thực hiện logic game object trong script PlayerUICharacterMenuManager. | - |
| `new Vector2(0f, 1f)` | 308 | Thực hiện logic vector2 trong script PlayerUICharacterMenuManager. | - |
| `new Vector2(1f, 1f)` | 309 | Thực hiện logic vector2 trong script PlayerUICharacterMenuManager. | - |
| `new Vector2(0.5f, 1f)` | 310 | Thực hiện logic vector2 trong script PlayerUICharacterMenuManager. | - |
| `new Vector2(0f, -128f)` | 311 | Thực hiện logic vector2 trong script PlayerUICharacterMenuManager. | - |
| `new Vector2(-32f, 54f)` | 312 | Thực hiện logic vector2 trong script PlayerUICharacterMenuManager. | - |
| `new Color(1f, 1f, 1f, 0.12f)` | 314 | Thực hiện logic color trong script PlayerUICharacterMenuManager. | - |
| `new GameObject("Text Area", typeof(RectTransform), typeof(RectMask2D))` | 316 | Thực hiện logic game object trong script PlayerUICharacterMenuManager. | - |
| `new Vector2(16f, 8f)` | 321 | Thực hiện logic vector2 trong script PlayerUICharacterMenuManager. | - |
| `new Vector2(-16f, -8f)` | 322 | Thực hiện logic vector2 trong script PlayerUICharacterMenuManager. | - |
| `new GameObject("Placeholder", typeof(RectTransform), typeof(TextMeshProUGUI))` | 324 | Thực hiện logic game object trong script PlayerUICharacterMenuManager. | - |
| `new Color(1f, 1f, 1f, 0.35f)` | 334 | Thực hiện logic color trong script PlayerUICharacterMenuManager. | - |
| `new GameObject("Text", typeof(RectTransform), typeof(TextMeshProUGUI))` | 338 | Thực hiện logic game object trong script PlayerUICharacterMenuManager. | - |
| `private Button CreateCheckCodeButton(RectTransform parent, TextMeshProUGUI textTemplate)` | 366 | Tạo object/dữ liệu check code button. | - |
| `return CreateActionButton( parent, textTemplate, "Check Relay Code Button", "CHECK CODE", -204f, CheckRelayCodeFromCharacterMenu)` | 368 | Tạo object/dữ liệu action button. | - |
| `private Button CreateJoinWorldButton(RectTransform parent, TextMeshProUGUI textTemplate)` | 377 | Tạo object/dữ liệu join world button. | - |
| `private Button CreateActionButton(RectTransform parent, TextMeshProUGUI textTemplate, string objectName, string label, float anchoredY, UnityEngine.Events.UnityAction callback)` | 391 | Tạo object/dữ liệu action button. | - |
| `new GameObject(objectName, typeof(RectTransform), typeof(Image), typeof(Button))` | 393 | Thực hiện logic game object trong script PlayerUICharacterMenuManager. | - |
| `new Vector2(0f, 1f)` | 399 | Thực hiện logic vector2 trong script PlayerUICharacterMenuManager. | - |
| `new Vector2(1f, 1f)` | 400 | Thực hiện logic vector2 trong script PlayerUICharacterMenuManager. | - |
| `new Vector2(0.5f, 1f)` | 401 | Thực hiện logic vector2 trong script PlayerUICharacterMenuManager. | - |
| `new Vector2(0f, anchoredY)` | 402 | Thực hiện logic vector2 trong script PlayerUICharacterMenuManager. | - |
| `new Vector2(-32f, 62f)` | 403 | Thực hiện logic vector2 trong script PlayerUICharacterMenuManager. | - |
| `new Color(1f, 1f, 1f, 0.14f)` | 405 | Thực hiện logic color trong script PlayerUICharacterMenuManager. | - |
| `new Color(1f, 1f, 1f, 0.14f)` | 413 | Thực hiện logic color trong script PlayerUICharacterMenuManager. | - |
| `new Color(1f, 1f, 1f, 0.24f)` | 414 | Thực hiện logic color trong script PlayerUICharacterMenuManager. | - |
| `new Color(1f, 1f, 1f, 0.34f)` | 415 | Thực hiện logic color trong script PlayerUICharacterMenuManager. | - |
| `new Color(1f, 1f, 1f, 0.05f)` | 417 | Thực hiện logic color trong script PlayerUICharacterMenuManager. | - |
| `new GameObject("Label", typeof(RectTransform), typeof(TextMeshProUGUI))` | 420 | Thực hiện logic game object trong script PlayerUICharacterMenuManager. | - |
| `private void RefreshJoinWorldUI()` | 440 | Làm mới dữ liệu/hiển thị join world ui. Liên kết trực tiếp: WorldGameSessionManager. | WorldGameSessionManager |
| `private async void CheckRelayCodeFromCharacterMenu()` | 488 | Thực hiện logic check relay code from character menu trong script PlayerUICharacterMenuManager. Liên kết trực tiếp: WorldGameSessionManager. | WorldGameSessionManager |
| `new Color(1f, 0.78f, 0.25f, 1f))` | 506 | Thực hiện logic color trong script PlayerUICharacterMenuManager. | - |
| `new Color(1f, 0.38f, 0.28f, 1f))` | 521 | Thực hiện logic color trong script PlayerUICharacterMenuManager. | - |
| `new Color(1f, 0.38f, 0.28f, 1f))` | 539 | Thực hiện logic color trong script PlayerUICharacterMenuManager. | - |
| `new Color(0.45f, 1f, 0.55f, 1f))` | 545 | Thực hiện logic color trong script PlayerUICharacterMenuManager. | - |
| `private async void JoinWorldFromCharacterMenu()` | 549 | Thực hiện logic join world from character menu trong script PlayerUICharacterMenuManager. Liên kết trực tiếp: PlayerUIManager, WorldGameSessionManager. | PlayerUIManager, WorldGameSessionManager |
| `new Color(1f, 0.78f, 0.25f, 1f))` | 558 | Thực hiện logic color trong script PlayerUICharacterMenuManager. | - |
| `new Color(1f, 0.78f, 0.25f, 1f))` | 565 | Thực hiện logic color trong script PlayerUICharacterMenuManager. | - |
| `new Color(1f, 0.78f, 0.25f, 1f))` | 572 | Thực hiện logic color trong script PlayerUICharacterMenuManager. | - |
| `new Color(1f, 0.38f, 0.28f, 1f))` | 588 | Thực hiện logic color trong script PlayerUICharacterMenuManager. | - |
| `private void SetJoinWorldButtonInteractable(bool isInteractable)` | 595 | Thiết lập giá trị hoặc trạng thái join world button interactable. | - |
| `private void RefreshCheckCodeButtonState()` | 601 | Làm mới dữ liệu/hiển thị check code button state. Liên kết trực tiếp: WorldGameSessionManager. | WorldGameSessionManager |
| `private void RefreshJoinControlsState()` | 612 | Làm mới dữ liệu/hiển thị join controls state. | - |
| `private void RefreshJoinButtonState()` | 618 | Làm mới dữ liệu/hiển thị join button state. Liên kết trực tiếp: WorldGameSessionManager. | WorldGameSessionManager |
| `private string GetJoinAddressInput()` | 649 | Lấy dữ liệu join address input cho hệ thống khác sử dụng. | - |
| `private void ClearOwnHostCodeFromJoinInput(string currentConnectionAddress)` | 656 | Thực hiện logic clear own host code from join input trong script PlayerUICharacterMenuManager. | - |
| `private bool IsRelayCodeInput(string addressInput)` | 668 | Kiểm tra điều kiện/trạng thái relay code input. | - |
| `private bool IsAddressInput(string addressInput)` | 684 | Kiểm tra điều kiện/trạng thái address input. | - |
| `private bool IsCurrentRelayCodeVerified(string addressInput)` | 693 | Kiểm tra điều kiện/trạng thái current relay code verified. Liên kết trực tiếp: WorldGameSessionManager. | WorldGameSessionManager |
| `private bool IsOwnRelayCodeInput(string addressInput)` | 702 | Kiểm tra điều kiện/trạng thái own relay code input. Liên kết trực tiếp: WorldGameSessionManager. | WorldGameSessionManager |
| `private void ShowJoinStatus(string statusText, Color statusColor)` | 710 | Thực hiện logic show join status trong script PlayerUICharacterMenuManager. | - |
| `private void ClearJoinStatus()` | 719 | Thực hiện logic clear join status trong script PlayerUICharacterMenuManager. | - |
| `private void UpdateJoinInputPlaceholder()` | 728 | Cập nhật join input placeholder theo trạng thái mới. Liên kết trực tiếp: WorldGameSessionManager. | WorldGameSessionManager |
| `private void ForceButtonUsable(Button button)` | 753 | Thực hiện logic force button usable trong script PlayerUICharacterMenuManager. | - |
| `private void CopyTextStyle(TextMeshProUGUI source, TextMeshProUGUI destination)` | 780 | Thực hiện logic copy text style trong script PlayerUICharacterMenuManager. | - |

#### PlayerUIEquipmentManager

- **Đường dẫn:** `Assets/Game/Scripts/Character/Player/PlayerUI/PlayerUIEquipmentManager.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Điều khiển một phần UI của player như HUD, menu, popup, shop, level up, site of grace hoặc equipment.
- **Kế thừa/cha:** PlayerUIMenu
- **Script con:** -
- **Field public/serialized chính:** public EquipmentType currentSelectedEquipmentSlot
- **Liên kết script:** BodyEquipmentItem, EquipmentType, HandEquipmentItem, HeadEquipmentItem, Item, LegEquipmentItem, PlayerManager, PlayerUIMenu, QuickSlotItem, RangedProjectileItem, UI_EquipmentInventorySlot, WeaponItem, WorldItemDatabase

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `private void Awake()` | 64 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. | - |
| `public override void OpenMenu()` | 87 | Mở UI/trạng thái/luồng menu. | - |
| `public void RefreshMenu()` | 98 | Làm mới dữ liệu/hiển thị menu. | - |
| `private void ToggleEquipmentButtons(bool isEnabled)` | 104 | Thực hiện logic toggle equipment buttons trong script PlayerUIEquipmentManager. | - |
| `private void ClearCurrentSelection()` | 127 | Thực hiện logic clear current selection trong script PlayerUIEquipmentManager. | - |
| `private void SelectButton(Button button)` | 135 | Thực hiện logic select button trong script PlayerUIEquipmentManager. | - |
| `public void SelectLastSelectedEquipmentSlot()` | 151 | Thực hiện logic select last selected equipment slot trong script PlayerUIEquipmentManager. Liên kết trực tiếp: EquipmentType. | EquipmentType |
| `private void RefreshEquipmentSlotIcons()` | 219 | Làm mới dữ liệu/hiển thị equipment slot icons. Liên kết trực tiếp: BodyEquipmentItem, HandEquipmentItem, HeadEquipmentItem, LegEquipmentItem, PlayerManager +3. | BodyEquipmentItem, HandEquipmentItem, HeadEquipmentItem, LegEquipmentItem, PlayerManager, QuickSlotItem, RangedProjectileItem, WeaponItem |
| `private void ClearEquipmentInventory()` | 446 | Thực hiện logic clear equipment inventory trong script PlayerUIEquipmentManager. | - |
| `public void LoadEquipmentInventory()` | 454 | Nạp dữ liệu hoặc scene liên quan tới equipment inventory. Liên kết trực tiếp: EquipmentType. | EquipmentType |
| `private void LoadWeaponInventory()` | 513 | Nạp dữ liệu hoặc scene liên quan tới weapon inventory. Liên kết trực tiếp: PlayerManager, UI_EquipmentInventorySlot, WeaponItem. | PlayerManager, UI_EquipmentInventorySlot, WeaponItem |
| `private void LoadHeadEquipmentInventory()` | 554 | Nạp dữ liệu hoặc scene liên quan tới head equipment inventory. Liên kết trực tiếp: HeadEquipmentItem, PlayerManager, UI_EquipmentInventorySlot. | HeadEquipmentItem, PlayerManager, UI_EquipmentInventorySlot |
| `private void LoadBodyEquipmentInventory()` | 595 | Nạp dữ liệu hoặc scene liên quan tới body equipment inventory. Liên kết trực tiếp: BodyEquipmentItem, PlayerManager, UI_EquipmentInventorySlot. | BodyEquipmentItem, PlayerManager, UI_EquipmentInventorySlot |
| `private void LoadLegEquipmentInventory()` | 636 | Nạp dữ liệu hoặc scene liên quan tới leg equipment inventory. Liên kết trực tiếp: LegEquipmentItem, PlayerManager, UI_EquipmentInventorySlot. | LegEquipmentItem, PlayerManager, UI_EquipmentInventorySlot |
| `private void LoadHandEquipmentInventory()` | 677 | Nạp dữ liệu hoặc scene liên quan tới hand equipment inventory. Liên kết trực tiếp: HandEquipmentItem, PlayerManager, UI_EquipmentInventorySlot. | HandEquipmentItem, PlayerManager, UI_EquipmentInventorySlot |
| `private void LoadProjectileInventory()` | 718 | Nạp dữ liệu hoặc scene liên quan tới projectile inventory. Liên kết trực tiếp: PlayerManager, RangedProjectileItem, UI_EquipmentInventorySlot. | PlayerManager, RangedProjectileItem, UI_EquipmentInventorySlot |
| `private void LoadQuickSlotInventory()` | 759 | Nạp dữ liệu hoặc scene liên quan tới quick slot inventory. Liên kết trực tiếp: PlayerManager, QuickSlotItem, UI_EquipmentInventorySlot. | PlayerManager, QuickSlotItem, UI_EquipmentInventorySlot |
| `public void SelectEquipmentSlot(int equipmentSlot)` | 800 | Thực hiện logic select equipment slot trong script PlayerUIEquipmentManager. Liên kết trực tiếp: EquipmentType. | EquipmentType |
| `public void UnEquipSelectedItem()` | 805 | Thực hiện logic un equip selected item trong script PlayerUIEquipmentManager. Liên kết trực tiếp: EquipmentType, Item, PlayerManager, WorldItemDatabase. | EquipmentType, Item, PlayerManager, WorldItemDatabase |

#### PlayerUIEquipmentManagerInputManager

- **Đường dẫn:** `Assets/Game/Scripts/Character/Player/PlayerUI/PlayerUIEquipmentManagerInputManager.cs`
- **Loại:** MonoBehaviour
- **Vai trò dễ hiểu:** Điều khiển một phần UI của player như HUD, menu, popup, shop, level up, site of grace hoặc equipment.
- **Kế thừa/cha:** MonoBehaviour
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** PlayerUIEquipmentManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `private void Awake()` | 14 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. Liên kết trực tiếp: PlayerUIEquipmentManager. | PlayerUIEquipmentManager |
| `private void OnEnable()` | 19 | Đăng ký event/callback hoặc bật trạng thái khi component được enable. | - |
| `new PlayerControls()` | 23 | Phát er controls, thường là animation, sound hoặc VFX. | - |
| `private void OnDisable()` | 31 | Gỡ event/callback hoặc dọn trạng thái khi component bị disable. | - |
| `private void Update()` | 36 | Cập nhật logic mỗi frame như input, timer, trạng thái chiến đấu hoặc UI. | - |
| `private void HandlePlayerUIEquipmentManagerInputs()` | 41 | Xử lý luồng player uiequipment manager inputs. | - |

#### PlayerUIHudManager

- **Đường dẫn:** `Assets/Game/Scripts/Character/Player/PlayerUI/PlayerUIHudManager.cs`
- **Loại:** MonoBehaviour
- **Vai trò dễ hiểu:** Điều khiển một phần UI của player như HUD, menu, popup, shop, level up, site of grace hoặc equipment.
- **Kế thừa/cha:** MonoBehaviour
- **Script con:** -
- **Field public/serialized chính:** public UI_StatBar healthBar, public Transform bossHealthBarParent, public GameObject bossHealthBarObject, public UI_Boss_HP_Bar currentBossHealthBar, public GameObject crossHair
- **Liên kết script:** BuffCharmItem, Item, PlayerEffectsManager, PlayerManager, PlayerStatBuffTimedEffect, PlayerUIManager, QuickSlotItem, RangedProjectileItem, SpellItem, UI_Boss_HP_Bar, UI_BuildUpBar, UI_StatBar, WeaponItem, WorldItemDatabase

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `private void Awake()` | 68 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. | - |
| `private void OnEnable()` | 73 | Đăng ký event/callback hoặc bật trạng thái khi component được enable. | - |
| `private void Update()` | 78 | Cập nhật logic mỗi frame như input, timer, trạng thái chiến đấu hoặc UI. | - |
| `public void ToggleHUD(bool status)` | 83 | Thực hiện logic toggle hud trong script PlayerUIHudManager. | - |
| `public void ToggleHUDWithOutPopUps(bool status)` | 105 | Thực hiện logic toggle hudwith out pop ups trong script PlayerUIHudManager. | - |
| `private bool IsGameplayHUDCanvasGroup(int index)` | 118 | Kiểm tra điều kiện/trạng thái gameplay hudcanvas group. | - |
| `private void ApplyGameplayHUDVisibility(bool isVisible)` | 123 | Áp dụng gameplay hudvisibility lên character/object mục tiêu. | - |
| `private void SetCanvasGroupVisible(CanvasGroup canvas, bool isVisible)` | 131 | Thiết lập giá trị hoặc trạng thái canvas group visible. | - |
| `public void RefreshHUD()` | 141 | Làm mới dữ liệu/hiển thị hud. | - |
| `public void SetRunesCount(int runesToAdd)` | 151 | Thiết lập giá trị hoặc trạng thái runes count. | - |
| `private IEnumerator WaitThenUpdateRuneCount()` | 163 | Thực hiện logic wait then update rune count trong script PlayerUIHudManager. Liên kết trực tiếp: PlayerUIManager. | PlayerUIManager |
| `public void RefreshRuneCountImmediate()` | 202 | Làm mới dữ liệu/hiển thị rune count immediate. Liên kết trực tiếp: PlayerUIManager. | PlayerUIManager |
| `public void SetNewPoisonBuildUpAmount(float oldValue, float amount)` | 215 | Thiết lập giá trị hoặc trạng thái new poison build up amount. | - |
| `public void SetNewFireBuildUpAmount(float oldValue, float amount)` | 220 | Thiết lập giá trị hoặc trạng thái new fire build up amount. | - |
| `public void SetNewBleedBuildUpAmount(float oldValue, float amount)` | 225 | Thiết lập giá trị hoặc trạng thái new bleed build up amount. | - |
| `public void SetNewFrostBuildUpAmount(float oldValue, float amount)` | 230 | Thiết lập giá trị hoặc trạng thái new frost build up amount. | - |
| `public void SetMaxBuildUpValue(int buildUpCapacity)` | 235 | Thiết lập giá trị hoặc trạng thái max build up value. | - |
| `public void SetNewHealthValue(int oldValue, int newValue)` | 243 | Thiết lập giá trị hoặc trạng thái new health value. | - |
| `public void SetMaxHealthValue(int maxHealth)` | 248 | Thiết lập giá trị hoặc trạng thái max health value. | - |
| `public void SetNewStaminaValue(float oldValue, float newValue)` | 253 | Thiết lập giá trị hoặc trạng thái new stamina value. | - |
| `public void SetMaxStaminaValue(int maxStamina)` | 258 | Thiết lập giá trị hoặc trạng thái max stamina value. | - |
| `public void SetNewFocusPointValue(int oldValue, int newValue)` | 263 | Thiết lập giá trị hoặc trạng thái new focus point value. | - |
| `public void SetMaxFocusPointValue(int maxFocusPoints)` | 268 | Thiết lập giá trị hoặc trạng thái max focus point value. | - |
| `public void SetRightWeaponQuickSlotIcon(int weaponID)` | 273 | Thiết lập giá trị hoặc trạng thái right weapon quick slot icon. Liên kết trực tiếp: Item, WeaponItem. | Item, WeaponItem |
| `public void SetLeftWeaponQuickSlotIcon(int weaponID)` | 297 | Thiết lập giá trị hoặc trạng thái left weapon quick slot icon. Liên kết trực tiếp: Item, WeaponItem. | Item, WeaponItem |
| `private WeaponItem ResolveEquippedWeaponForHUD(int weaponID, bool isRightHand)` | 319 | Thực hiện logic resolve equipped weapon for hud trong script PlayerUIHudManager. Liên kết trực tiếp: PlayerManager, PlayerUIManager, WeaponItem, WorldItemDatabase. | PlayerManager, PlayerUIManager, WeaponItem, WorldItemDatabase |
| `public void SetSpellItemQuickSlotIcon(int spellID)` | 355 | Thiết lập giá trị hoặc trạng thái spell item quick slot icon. Liên kết trực tiếp: Item, SpellItem, WorldItemDatabase. | Item, SpellItem, WorldItemDatabase |
| `public void SetQuickSlotItemQuickSlotIcon(QuickSlotItem quickSlotItem)` | 379 | Thiết lập giá trị hoặc trạng thái quick slot item quick slot icon. Liên kết trực tiếp: Item, PlayerUIManager. | Item, PlayerUIManager |
| `public void ToggleProjectileQuickSlotsVisibility(bool status)` | 421 | Thực hiện logic toggle projectile quick slots visibility trong script PlayerUIHudManager. | - |
| `public void SetMainProjectileQuickSlotIcon(RangedProjectileItem projectileItem)` | 426 | Thiết lập giá trị hoặc trạng thái main projectile quick slot icon. Liên kết trực tiếp: Item. | Item |
| `public void SetSecondaryProjectileQuickSlotIcon(RangedProjectileItem projectileItem)` | 452 | Thiết lập giá trị hoặc trạng thái secondary projectile quick slot icon. Liên kết trực tiếp: Item. | Item |
| `public void ShowActiveBuff(BuffCharmItem buffItem)` | 478 | Thực hiện logic show active buff trong script PlayerUIHudManager. | - |
| `public void HideActiveBuff(int sourceItemID)` | 502 | Thực hiện logic hide active buff trong script PlayerUIHudManager. Liên kết trực tiếp: BuffCharmItem, WorldItemDatabase. | BuffCharmItem, WorldItemDatabase |
| `public void ClearActiveBuffs()` | 536 | Thực hiện logic clear active buffs trong script PlayerUIHudManager. | - |
| `private Image GetBuffIcon(BuffCharmItem buffItem)` | 553 | Lấy dữ liệu buff icon cho hệ thống khác sử dụng. | - |
| `private TextMeshProUGUI GetBuffTimerText(BuffCharmItem buffItem)` | 575 | Lấy dữ liệu buff timer text cho hệ thống khác sử dụng. | - |
| `private bool HasVisibleBuffIcon()` | 597 | Thực hiện logic has visible buff icon trong script PlayerUIHudManager. | - |
| `return IsBuffIconVisible(guardianBuffIcon) \|\| IsBuffIconVisible(windBuffIcon) \|\| IsBuffIconVisible(sageBuffIcon) \|\| IsBuffIconVisible(warBuffIcon)` | 599 | Kiểm tra điều kiện/trạng thái buff icon visible. | - |
| `private bool IsBuffIconVisible(Image buffIcon)` | 605 | Kiểm tra điều kiện/trạng thái buff icon visible. | - |
| `private void RemoveExistingBuffMapping(Image buffIcon)` | 610 | Loại bỏ existing buff mapping khỏi danh sách, trạng thái hoặc dữ liệu. | - |
| `private void RemoveExistingBuffTimerMapping(TextMeshProUGUI timerText)` | 630 | Loại bỏ existing buff timer mapping khỏi danh sách, trạng thái hoặc dữ liệu. | - |
| `private void RefreshActiveBuffTimerTexts()` | 650 | Làm mới dữ liệu/hiển thị active buff timer texts. Liên kết trực tiếp: PlayerEffectsManager, PlayerStatBuffTimedEffect, PlayerUIManager. | PlayerEffectsManager, PlayerStatBuffTimedEffect, PlayerUIManager |
| `private void SetBuffTimerTextVisible(TextMeshProUGUI timerText, bool isVisible)` | 697 | Thiết lập giá trị hoặc trạng thái buff timer text visible. | - |
| `private void ClearBuffIcon(Image buffIcon)` | 708 | Thực hiện logic clear buff icon trong script PlayerUIHudManager. | - |
| `private void ClearBuffTimerText(TextMeshProUGUI timerText)` | 717 | Thực hiện logic clear buff timer text trong script PlayerUIHudManager. | - |

#### PlayerUILevelUpManager

- **Đường dẫn:** `Assets/Game/Scripts/Character/Player/PlayerUI/PlayerUILevelUpManager.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Điều khiển một phần UI của player như HUD, menu, popup, shop, level up, site of grace hoặc equipment.
- **Kế thừa/cha:** PlayerUIMenu
- **Script con:** -
- **Field public/serialized chính:** public CharacterAttribute currentSelectedAttribute, public Slider vigorSlider, public Slider mindSlider, public Slider enduranceSlider, public Slider strengthSlider, public Slider dexteritySlider, public Slider intelligenceSlider, public Slider faithSlider
- **Liên kết script:** CharacterAttribute, PlayerManager, PlayerUIManager, PlayerUIMenu, WorldSaveGameManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `private void Awake()` | 53 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. | - |
| `public override void OpenMenu()` | 58 | Mở UI/trạng thái/luồng menu. | - |
| `private void SetCurrentStats()` | 66 | Thiết lập giá trị hoặc trạng thái current stats. Liên kết trực tiếp: PlayerUIManager. | PlayerUIManager |
| `public void UpdateSliderBasedOnCurrentlySelectedAttributes()` | 111 | Cập nhật slider based on currently selected attributes theo trạng thái mới. Liên kết trực tiếp: CharacterAttribute, PlayerManager, PlayerUIManager. | CharacterAttribute, PlayerManager, PlayerUIManager |
| `public void ConfirmLevels()` | 165 | Thực hiện logic confirm levels trong script PlayerUILevelUpManager. Liên kết trực tiếp: PlayerManager, PlayerUIManager, WorldSaveGameManager. | PlayerManager, PlayerUIManager, WorldSaveGameManager |
| `private void SetAllLevelsCost()` | 188 | Thiết lập giá trị hoặc trạng thái all levels cost. | - |
| `private void CalculateLevelCost(int currentLevel, int projectedLevel)` | 199 | Tính toán level cost từ chỉ số hoặc dữ liệu hiện có. Liên kết trực tiếp: PlayerUIManager. | PlayerUIManager |
| `private void ChangeTextColorsDependingOnCosts()` | 231 | Thực hiện logic change text colors depending on costs trong script PlayerUILevelUpManager. Liên kết trực tiếp: PlayerManager, PlayerUIManager. | PlayerManager, PlayerUIManager |
| `private void ChangeTextFieldToSpecificColorBaseOnStat(PlayerManager player, TextMeshProUGUI textField, int stat, int projectedStat)` | 281 | Thực hiện logic change text field to specific color base on stat trong script PlayerUILevelUpManager. | - |

#### PlayerUILoadingScreenManager

- **Đường dẫn:** `Assets/Game/Scripts/Character/Player/PlayerUI/PlayerUILoadingScreenManager.cs`
- **Loại:** MonoBehaviour
- **Vai trò dễ hiểu:** Điều khiển một phần UI của player như HUD, menu, popup, shop, level up, site of grace hoặc equipment.
- **Kế thừa/cha:** MonoBehaviour
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** BuildRuntimeLogger, PlayerUIManager, WorldAIManager, WorldSceneManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `private void Awake()` | 23 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. Liên kết trực tiếp: BuildRuntimeLogger. | BuildRuntimeLogger |
| `private void Start()` | 29 | Chạy sau Awake để hoàn tất thiết lập ban đầu, nhất là khi cần các object khác đã sẵn sàng. Liên kết trực tiếp: BuildRuntimeLogger. | BuildRuntimeLogger |
| `private void OnSceneChanged(Scene arg0, Scene arg1)` | 36 | Thực hiện logic on scene changed trong script PlayerUILoadingScreenManager. Liên kết trực tiếp: BuildRuntimeLogger. | BuildRuntimeLogger |
| `private void Update()` | 44 | Cập nhật logic mỗi frame như input, timer, trạng thái chiến đấu hoặc UI. Liên kết trực tiếp: BuildRuntimeLogger, WorldSceneManager. | BuildRuntimeLogger, WorldSceneManager |
| `public void ActivateLoadingScreen()` | 62 | Thực hiện logic activate loading screen trong script PlayerUILoadingScreenManager. Liên kết trực tiếp: BuildRuntimeLogger. | BuildRuntimeLogger |
| `public void SetProgress(float progress, string label = DefaultProgressLabel)` | 80 | Thiết lập giá trị hoặc trạng thái progress. Liên kết trực tiếp: BuildRuntimeLogger. | BuildRuntimeLogger |
| `public void DeactivateLoadingScreen(float delay = 1)` | 96 | Thực hiện logic deactivate loading screen trong script PlayerUILoadingScreenManager. Liên kết trực tiếp: BuildRuntimeLogger. | BuildRuntimeLogger |
| `private IEnumerator FadeLoadingScreen(float duration, float delay)` | 153 | Thực hiện logic fade loading screen trong script PlayerUILoadingScreenManager. Liên kết trực tiếp: BuildRuntimeLogger, WorldAIManager. | BuildRuntimeLogger, WorldAIManager |
| `private void ToggleGameplayHUDForLoading(bool isVisible)` | 201 | Thực hiện logic toggle gameplay hudfor loading trong script PlayerUILoadingScreenManager. Liên kết trực tiếp: PlayerUIManager. | PlayerUIManager |
| `private void HideLoadingProgressUIIfNeeded()` | 212 | Thực hiện logic hide loading progress uiif needed trong script PlayerUILoadingScreenManager. | - |
| `public bool LoadingScreenIsActive()` | 233 | Nạp dữ liệu hoặc scene liên quan tới ing screen is active. | - |
| `public void ForceHideLoadingScreen()` | 238 | Thực hiện logic force hide loading screen trong script PlayerUILoadingScreenManager. Liên kết trực tiếp: BuildRuntimeLogger. | BuildRuntimeLogger |

#### PlayerUIManager

- **Đường dẫn:** `Assets/Game/Scripts/Character/Player/PlayerUI/PlayerUIManager.cs`
- **Loại:** MonoBehaviour
- **Vai trò dễ hiểu:** Điều khiển một phần UI của player như HUD, menu, popup, shop, level up, site of grace hoặc equipment.
- **Kế thừa/cha:** MonoBehaviour
- **Script con:** -
- **Field public/serialized chính:** public static PlayerUIManager instance, public PlayerManager localPlayer, public PlayerUIHudManager playerUIHudManager, public PlayerUIPopUpManager playerUIPopUpManager, public PlayerUICharacterMenuManager playerUICharacterMenuManager, public PlayerUIEquipmentManager playerUIEquipmentManager, public PlayerUISiteOfGraceManager playerUISiteOfGraceManager, public PlayerUITeleportLocationManager playerUITeleportLocationManager, public PlayerUILoadingScreenManager playerUILoadingScreenManager, public PlayerUILevelUpManager playerUILevelUpManager, public PlayerUIWeaponUpgradeManager playerUIWeaponUpgradeManager, public PlayerUIShopManager playerUIShopManager +3
- **Liên kết script:** GameSettingsManager, PlayerInputManager, PlayerManager, PlayerUICharacterMenuManager, PlayerUIEquipmentManager, PlayerUIHudManager, PlayerUILevelUpManager, PlayerUILoadingScreenManager, PlayerUIMenu, PlayerUIPopUpManager, PlayerUISettingsManager, PlayerUIShopManager, PlayerUISiteOfGraceManager, PlayerUITeleportLocationManager, PlayerUIWeaponUpgradeManager, WorldSoundFXManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `private void Awake()` | 34 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. Liên kết trực tiếp: PlayerUICharacterMenuManager, PlayerUIEquipmentManager, PlayerUIHudManager, PlayerUILevelUpManager, PlayerUILoadingScreenManager +6. | PlayerUICharacterMenuManager, PlayerUIEquipmentManager, PlayerUIHudManager, PlayerUILevelUpManager, PlayerUILoadingScreenManager, PlayerUIPopUpManager, PlayerUISettingsManager, PlayerUIShopManager, PlayerUISiteOfGraceManager, PlayerUITeleportLocationManager +1 |
| `private void Start()` | 63 | Chạy sau Awake để hoàn tất thiết lập ban đầu, nhất là khi cần các object khác đã sẵn sàng. | - |
| `private void Update()` | 69 | Cập nhật logic mỗi frame như input, timer, trạng thái chiến đấu hoặc UI. | - |
| `public void CloseAllMenuWindows()` | 81 | Đóng UI/trạng thái/luồng all menu windows. | - |
| `public void OpenMenuAsRoot(PlayerUIMenu menu)` | 99 | Mở UI/trạng thái/luồng menu as root. | - |
| `public void TransitionToMenu(PlayerUIMenu fromMenu, PlayerUIMenu toMenu)` | 110 | Thực hiện logic transition to menu trong script PlayerUIManager. | - |
| `public bool CloseCurrentMenuStep()` | 128 | Đóng UI/trạng thái/luồng current menu step. Liên kết trực tiếp: PlayerUIMenu. | PlayerUIMenu |
| `public void RefreshMenuWindowState()` | 146 | Làm mới dữ liệu/hiển thị menu window state. Liên kết trực tiếp: PlayerInputManager. | PlayerInputManager |
| `private bool IsMenuOpen(PlayerUIMenu menu)` | 162 | Kiểm tra điều kiện/trạng thái menu open. | - |
| `private void CloseAllMenuWindowsImmediate()` | 167 | Đóng UI/trạng thái/luồng all menu windows immediate. | - |
| `private PlayerUIMenu GetTopOpenMenu()` | 194 | Lấy dữ liệu top open menu cho hệ thống khác sử dụng. | - |
| `private PlayerUIMenu GetTrackedPreviousMenu()` | 223 | Lấy dữ liệu tracked previous menu cho hệ thống khác sử dụng. | - |
| `private void EnsureMenuTracked(PlayerUIMenu menu)` | 234 | Thực hiện logic ensure menu tracked trong script PlayerUIManager. | - |
| `private void RemoveTrackedMenu(PlayerUIMenu menu)` | 243 | Loại bỏ tracked menu khỏi danh sách, trạng thái hoặc dữ liệu. | - |
| `public void PlayUnableToContinueSFX()` | 256 | Phát unable to continue sfx, thường là animation, sound hoặc VFX. Liên kết trực tiếp: WorldSoundFXManager. | WorldSoundFXManager |
| `public void PlayConfirmSFX()` | 264 | Phát confirm sfx, thường là animation, sound hoặc VFX. Liên kết trực tiếp: WorldSoundFXManager. | WorldSoundFXManager |
| `public void PlayHoverSFX()` | 272 | Phát hover sfx, thường là animation, sound hoặc VFX. Liên kết trực tiếp: WorldSoundFXManager. | WorldSoundFXManager |
| `public void ApplyAudioSettings()` | 280 | Áp dụng audio settings lên character/object mục tiêu. Liên kết trực tiếp: GameSettingsManager. | GameSettingsManager |

#### PlayerUIMenu

- **Đường dẫn:** `Assets/Game/Scripts/Character/Player/PlayerUI/PlayerUIMenu.cs`
- **Loại:** MonoBehaviour
- **Vai trò dễ hiểu:** Điều khiển một phần UI của player như HUD, menu, popup, shop, level up, site of grace hoặc equipment.
- **Kế thừa/cha:** MonoBehaviour
- **Script con:** PlayerUICharacterMenuManager, PlayerUIEquipmentManager, PlayerUILevelUpManager, PlayerUISettingsManager, PlayerUIShopManager, PlayerUISiteOfGraceManager, PlayerUITeleportLocationManager, PlayerUIWeaponUpgradeManager
- **Field public/serialized chính:** [SerializeField] protected GameObject menu
- **Liên kết script:** PlayerUIManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public bool IsMenuOpen()` | 11 | Kiểm tra điều kiện/trạng thái menu open. | - |
| `public virtual void OpenMenu()` | 17 | Mở UI/trạng thái/luồng menu. Liên kết trực tiếp: PlayerUIManager. | PlayerUIManager |
| `public virtual void OpenMenuAfterFixedFrame()` | 32 | Mở UI/trạng thái/luồng menu after fixed frame. | - |
| `protected virtual IEnumerator WaitThenOpenMenu()` | 45 | Thực hiện logic wait then open menu trong script PlayerUIMenu. | - |
| `new WaitForFixedUpdate()` | 47 | Thực hiện logic wait for fixed update trong script PlayerUIMenu. | - |
| `public virtual void CloseMenu()` | 52 | Đóng UI/trạng thái/luồng menu. Liên kết trực tiếp: PlayerUIManager. | PlayerUIManager |
| `public virtual void CloseMenuAfterFixedFrame()` | 68 | Đóng UI/trạng thái/luồng menu after fixed frame. | - |
| `protected virtual IEnumerator WaitThenCloseMenu()` | 81 | Thực hiện logic wait then close menu trong script PlayerUIMenu. | - |
| `new WaitForFixedUpdate()` | 83 | Thực hiện logic wait for fixed update trong script PlayerUIMenu. | - |
| `protected virtual void EnsureMenuReference()` | 88 | Thực hiện logic ensure menu reference trong script PlayerUIMenu. | - |

#### PlayerUIPopUpManager

- **Đường dẫn:** `Assets/Game/Scripts/Character/Player/PlayerUI/PlayerUIPopUpManager.cs`
- **Loại:** MonoBehaviour
- **Vai trò dễ hiểu:** Điều khiển một phần UI của player như HUD, menu, popup, shop, level up, site of grace hoặc equipment.
- **Kế thừa/cha:** MonoBehaviour
- **Script con:** -
- **Field public/serialized chính:** [SerializeField] private GameObject statusEffectPopUpPrefab
- **Liên kết script:** AICharacterManager, BuildUp, CharacterDialogue, CharacterSaveData, EndGameActionType, GameProgressionManager, Item, PlayerInputManager, PlayerUIManager, SerializableDictionary, SessionEndGameActionType, UI_StatusEffectWarning, WorldGameSessionManager, WorldSaveGameManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `new Color(0.96f, 0.84f, 0.42f, 1f)` | 83 | Thực hiện logic color trong script PlayerUIPopUpManager. | - |
| `public bool IsEndGameOverlayOpen()` | 94 | Kiểm tra điều kiện/trạng thái end game overlay open. | - |
| `public bool IsLeaderboardOverlayOpen()` | 99 | Kiểm tra điều kiện/trạng thái leaderboard overlay open. | - |
| `public bool TryHandleCloseLeaderboardInput()` | 104 | Thử thực hiện handle close leaderboard input, thường có kiểm tra điều kiện trước khi chạy. | - |
| `public void CloseAllPopUpWindows()` | 116 | Đóng UI/trạng thái/luồng all pop up windows. Liên kết trực tiếp: PlayerUIManager. | PlayerUIManager |
| `public void SendPlayerMessagePopUp(string messageText)` | 163 | Thực hiện logic send player message pop up trong script PlayerUIPopUpManager. Liên kết trực tiếp: PlayerUIManager. | PlayerUIManager |
| `public void SendItemPopUp(Item item, int amount)` | 170 | Thực hiện logic send item pop up trong script PlayerUIPopUpManager. Liên kết trực tiếp: PlayerUIManager. | PlayerUIManager |
| `public void SendYouDiedPopUp()` | 187 | Thực hiện logic send you died pop up trong script PlayerUIPopUpManager. | - |
| `public void SendBossDefeatedPopUp(string bossDefeatedMessage) => PlayBossStylePopUp(bossDefeatedMessage)` | 192 | Thực hiện logic send boss defeated pop up trong script PlayerUIPopUpManager. | - |
| `public void SendMapUnlockedPopUp(string mapUnlockedMessage) => PlayBossStylePopUp(mapUnlockedMessage)` | 195 | Thực hiện logic send map unlocked pop up trong script PlayerUIPopUpManager. | - |
| `public void SendVictoryPopUp(string victoryMessage) => PlayYouDiedStylePopUp(victoryMessage)` | 198 | Thực hiện logic send victory pop up trong script PlayerUIPopUpManager. | - |
| `public void SendLosePopUp(string loseMessage) => PlayYouDiedStylePopUp(loseMessage)` | 201 | Thực hiện logic send lose pop up trong script PlayerUIPopUpManager. | - |
| `public void ShowLoseEndGameOverlay()` | 204 | Thực hiện logic show lose end game overlay trong script PlayerUIPopUpManager. Liên kết trực tiếp: EndGameActionType. | EndGameActionType |
| `public void ShowVictoryEndGameOverlay(bool canContinueProgression)` | 217 | Thực hiện logic show victory end game overlay trong script PlayerUIPopUpManager. Liên kết trực tiếp: EndGameActionType. | EndGameActionType |
| `public void ShowVictoryEndGameOverlayDelayed(bool canContinueProgression, float delay)` | 232 | Thực hiện logic show victory end game overlay delayed trong script PlayerUIPopUpManager. | - |
| `public void SendMapUnlockedPopUpDelayed(string mapUnlockedMessage, float delay)` | 249 | Thực hiện logic send map unlocked pop up delayed trong script PlayerUIPopUpManager. | - |
| `public void SendVictoryPopUpDelayed(string victoryMessage, float delay)` | 254 | Thực hiện logic send victory pop up delayed trong script PlayerUIPopUpManager. | - |
| `private IEnumerator SendMapUnlockedPopUpDelayedCoroutine(string message, float delay)` | 259 | Thực hiện logic send map unlocked pop up delayed coroutine trong script PlayerUIPopUpManager. | - |
| `new WaitForSeconds(delay)` | 261 | Thực hiện logic wait for seconds trong script PlayerUIPopUpManager. | - |
| `private IEnumerator SendVictoryPopUpDelayedCoroutine(string message, float delay)` | 265 | Thực hiện logic send victory pop up delayed coroutine trong script PlayerUIPopUpManager. | - |
| `new WaitForSeconds(delay)` | 267 | Thực hiện logic wait for seconds trong script PlayerUIPopUpManager. | - |
| `private IEnumerator ShowVictoryEndGameOverlayDelayedCoroutine(bool canContinueProgression, float delay)` | 271 | Thực hiện logic show victory end game overlay delayed coroutine trong script PlayerUIPopUpManager. | - |
| `new WaitForSeconds(delay)` | 273 | Thực hiện logic wait for seconds trong script PlayerUIPopUpManager. | - |
| `private void PlayBossStylePopUp(string message)` | 280 | Phát boss style pop up, thường là animation, sound hoặc VFX. | - |
| `private IEnumerator PlayBossStylePopUpCoroutine()` | 297 | Phát boss style pop up coroutine, thường là animation, sound hoặc VFX. | - |
| `new WaitForSeconds(visibleDelay)` | 329 | Thực hiện logic wait for seconds trong script PlayerUIPopUpManager. | - |
| `public void SendGraceRestoredPopUp(string graceRestoredMessage)` | 344 | Thực hiện logic send grace restored pop up trong script PlayerUIPopUpManager. | - |
| `private void PlayYouDiedStylePopUp(string message)` | 349 | Phát you died style pop up, thường là animation, sound hoặc VFX. | - |
| `private IEnumerator PlayYouDiedStylePopUpCoroutine()` | 366 | Phát you died style pop up coroutine, thường là animation, sound hoặc VFX. | - |
| `return StartCoroutine(FadeInPopUpOverTime(youDiedPopUpCanvasGroup, 1.5f))` | 369 | Thực hiện logic start coroutine trong script PlayerUIPopUpManager. | - |
| `return StartCoroutine(WaitThenFadeOutPopUpOverTime(youDiedPopUpCanvasGroup, 2, 2.5f))` | 370 | Thực hiện logic start coroutine trong script PlayerUIPopUpManager. | - |
| `private void PlayGraceRestoredStylePopUp(string message)` | 374 | Phát grace restored style pop up, thường là animation, sound hoặc VFX. | - |
| `private IEnumerator PlayGraceRestoredStylePopUpCoroutine()` | 391 | Phát grace restored style pop up coroutine, thường là animation, sound hoặc VFX. | - |
| `return StartCoroutine(FadeInPopUpOverTime(graceRestoredPopUpCanvasGroup, 1.5f))` | 394 | Thực hiện logic start coroutine trong script PlayerUIPopUpManager. | - |
| `return StartCoroutine(WaitThenFadeOutPopUpOverTime(graceRestoredPopUpCanvasGroup, 2, 2.5f))` | 395 | Thực hiện logic start coroutine trong script PlayerUIPopUpManager. | - |
| `public void SendStatusEffectPopUp(BuildUp status)` | 399 | Thực hiện logic send status effect pop up trong script PlayerUIPopUpManager. Liên kết trực tiếp: UI_StatusEffectWarning. | UI_StatusEffectWarning |
| `public void SendBuffPopUp(Item item)` | 408 | Thực hiện logic send buff pop up trong script PlayerUIPopUpManager. | - |
| `public void SendDialoguePopUp(CharacterDialogue dialogue, AICharacterManager aiCharacter)` | 425 | Thực hiện logic send dialogue pop up trong script PlayerUIPopUpManager. Liên kết trực tiếp: PlayerUIManager. | PlayerUIManager |
| `public void SendNextDialoguePopUpInIndex(CharacterDialogue dialogue, AICharacterManager aiCharacter)` | 440 | Thực hiện logic send next dialogue pop up in index trong script PlayerUIPopUpManager. Liên kết trực tiếp: PlayerUIManager. | PlayerUIManager |
| `public void SetDialoguePopUpSubtitles(string dialogueText)` | 458 | Thiết lập giá trị hoặc trạng thái dialogue pop up subtitles. | - |
| `public void EndDialoguePopUp()` | 464 | Thực hiện logic end dialogue pop up trong script PlayerUIPopUpManager. Liên kết trực tiếp: PlayerUIManager. | PlayerUIManager |
| `public void CancelDialoguePopUp(AICharacterManager aiCharacter)` | 470 | Kiểm tra có được phép cel dialogue pop up hay không. Liên kết trực tiếp: PlayerUIManager. | PlayerUIManager |
| `private IEnumerator StretchPopUpTextOverTime(TextMeshProUGUI text, float duration, float stretchAmount)` | 484 | Thực hiện logic stretch pop up text over time trong script PlayerUIPopUpManager. | - |
| `private IEnumerator FadeInPopUpOverTime(CanvasGroup canvas, float duration)` | 501 | Thực hiện logic fade in pop up over time trong script PlayerUIPopUpManager. | - |
| `private IEnumerator WaitThenFadeOutPopUpOverTime(CanvasGroup canvas, float duration, float delay)` | 520 | Thực hiện logic wait then fade out pop up over time trong script PlayerUIPopUpManager. | - |
| `private IEnumerator FadeOutThenDestroy(CanvasGroup canvas, float duration, GameObject objectToDestroy)` | 544 | Thực hiện logic fade out then destroy trong script PlayerUIPopUpManager. | - |
| `private IEnumerator FadeOutExistingPopUp(CanvasGroup canvas, float duration, GameObject popUpObject)` | 568 | Thực hiện logic fade out existing pop up trong script PlayerUIPopUpManager. | - |
| `private void ForceShowEndGameOverlay( string title, string subtitle, string primaryButtonLabel, EndGameActionType primaryAction, string secondaryButtonLabel, EndGameActionType secondaryAction)` | 592 | Thực hiện logic force show end game overlay trong script PlayerUIPopUpManager. Liên kết trực tiếp: PlayerInputManager, PlayerUIManager. | PlayerInputManager, PlayerUIManager |
| `private void HideEndGameOverlay()` | 692 | Thực hiện logic hide end game overlay trong script PlayerUIPopUpManager. Liên kết trực tiếp: EndGameActionType, PlayerInputManager, PlayerUIManager. | EndGameActionType, PlayerInputManager, PlayerUIManager |
| `public void HandlePrimaryEndGameButtonPressed()` | 720 | Xử lý luồng primary end game button pressed. | - |
| `public void HandleSecondaryEndGameButtonPressed()` | 725 | Xử lý luồng secondary end game button pressed. | - |
| `public void HandleLeaderboardEndGameButtonPressed()` | 730 | Xử lý luồng leaderboard end game button pressed. | - |
| `public void HandleLeaderboardCloseButtonPressed()` | 735 | Xử lý luồng leaderboard close button pressed. | - |
| `public void DismissEndGameOverlayForTransition(bool showLoadingScreen)` | 740 | Thực hiện logic dismiss end game overlay for transition trong script PlayerUIPopUpManager. Liên kết trực tiếp: PlayerUIManager. | PlayerUIManager |
| `private string BuildEndGameSubtitle(string hostSubtitle)` | 753 | Thực hiện logic build end game subtitle trong script PlayerUIPopUpManager. | - |
| `private void ApplyEndGameActionAvailability()` | 761 | Áp dụng end game action availability lên character/object mục tiêu. | - |
| `private bool CanLocalPlayerControlEndGameActions()` | 775 | Kiểm tra có được phép local player control end game actions hay không. Liên kết trực tiếp: PlayerUIManager, WorldGameSessionManager. | PlayerUIManager, WorldGameSessionManager |
| `private void ExecuteEndGameAction(EndGameActionType action)` | 789 | Thực hiện logic execute end game action trong script PlayerUIPopUpManager. Liên kết trực tiếp: EndGameActionType, PlayerUIManager, SessionEndGameActionType, WorldGameSessionManager. | EndGameActionType, PlayerUIManager, SessionEndGameActionType, WorldGameSessionManager |
| `private void CacheEndGameSummary(string resultLabel, bool canContinueProgression)` | 832 | Thực hiện logic cache end game summary trong script PlayerUIPopUpManager. | - |
| `private void ShowLeaderboardOverlay()` | 841 | Thực hiện logic show leaderboard overlay trong script PlayerUIPopUpManager. Liên kết trực tiếp: PlayerUIManager. | PlayerUIManager |
| `private void HideLeaderboardOverlay(bool restoreEndGameSelection)` | 874 | Thực hiện logic hide leaderboard overlay trong script PlayerUIPopUpManager. | - |
| `private void RefreshLeaderboardOverlayContent()` | 894 | Làm mới dữ liệu/hiển thị leaderboard overlay content. Liên kết trực tiếp: WorldGameSessionManager. | WorldGameSessionManager |
| `private string BuildLeaderboardSummary(int localDeathCount, int maxDeaths)` | 905 | Thực hiện logic build leaderboard summary trong script PlayerUIPopUpManager. Liên kết trực tiếp: CharacterSaveData, GameProgressionManager, WorldSaveGameManager. | CharacterSaveData, GameProgressionManager, WorldSaveGameManager |
| `private int ResolveRunMapIndex()` | 935 | Thực hiện logic resolve run map index trong script PlayerUIPopUpManager. Liên kết trực tiếp: GameProgressionManager. | GameProgressionManager |
| `private int GetLocalPlayerDeathCountForCurrentMap()` | 949 | Lấy dữ liệu local player death count for current map cho hệ thống khác sử dụng. Liên kết trực tiếp: PlayerUIManager, WorldGameSessionManager. | PlayerUIManager, WorldGameSessionManager |
| `private string GetLeaderboardPlayerName(CharacterSaveData currentCharacterData)` | 961 | Lấy dữ liệu leaderboard player name cho hệ thống khác sử dụng. | - |
| `private string BuildProgressionSummary(int runMapIndex)` | 969 | Thực hiện logic build progression summary trong script PlayerUIPopUpManager. Liên kết trực tiếp: GameProgressionManager. | GameProgressionManager |
| `private string BuildRunRank(string resultLabel, int localDeathCount)` | 991 | Thực hiện logic build run rank trong script PlayerUIPopUpManager. | - |
| `private int CountCompletedEntries(SerializableDictionary<int, bool> source)` | 1008 | Thực hiện logic count completed entries trong script PlayerUIPopUpManager. | - |
| `private void SetEndGameOverlayVisible(bool isVisible)` | 1024 | Thiết lập giá trị hoặc trạng thái end game overlay visible. | - |

#### PlayerUISelectButtonOnEnable

- **Đường dẫn:** `Assets/Game/Scripts/Character/Player/PlayerUI/PlayerUISelectButtonOnEnable.cs`
- **Loại:** MonoBehaviour
- **Vai trò dễ hiểu:** Điều khiển một phần UI của player như HUD, menu, popup, shop, level up, site of grace hoặc equipment.
- **Kế thừa/cha:** MonoBehaviour
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** -

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `private void Awake()` | 10 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. | - |
| `private void OnEnable()` | 15 | Đăng ký event/callback hoặc bật trạng thái khi component được enable. | - |

#### PlayerUISettingsManager

- **Đường dẫn:** `Assets/Game/Scripts/Character/Player/PlayerUI/PlayerUISettingsManager.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Điều khiển một phần UI của player như HUD, menu, popup, shop, level up, site of grace hoặc equipment.
- **Kế thừa/cha:** PlayerUIMenu
- **Script con:** -
- **Field public/serialized chính:** [SerializeField] private RectTransform contentRoot
- **Liên kết script:** GameSettingsManager, GameSettingsMenuViewUtility, PlayerUIManager, PlayerUIMenu

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public override void OpenMenu()` | 33 | Mở UI/trạng thái/luồng menu. | - |
| `private void OnEnable()` | 40 | Đăng ký event/callback hoặc bật trạng thái khi component được enable. Liên kết trực tiếp: GameSettingsManager. | GameSettingsManager |
| `private void OnDisable()` | 45 | Gỡ event/callback hoặc dọn trạng thái khi component bị disable. Liên kết trực tiếp: GameSettingsManager. | GameSettingsManager |
| `public void OpenFromCharacterMenu()` | 50 | Mở UI/trạng thái/luồng from character menu. Liên kết trực tiếp: PlayerUIManager. | PlayerUIManager |
| `public void Refresh()` | 60 | Làm mới dữ liệu/hiển thị . Liên kết trực tiếp: GameSettingsManager. | GameSettingsManager |
| `private void CacheReferences()` | 103 | Thực hiện logic cache references trong script PlayerUISettingsManager. | - |
| `private void EnsureInitialized()` | 129 | Thực hiện logic ensure initialized trong script PlayerUISettingsManager. | - |
| `private void BindListeners()` | 138 | Thực hiện logic bind listeners trong script PlayerUISettingsManager. Liên kết trực tiếp: GameSettingsManager, PlayerUIManager. | GameSettingsManager, PlayerUIManager |
| `private void BindSlider(Slider slider, float minValue, float maxValue, UnityEngine.Events.UnityAction<float> callback)` | 208 | Thực hiện logic bind slider trong script PlayerUISettingsManager. | - |
| `private void BindButton(Button button, UnityEngine.Events.UnityAction callback)` | 220 | Thực hiện logic bind button trong script PlayerUISettingsManager. | - |
| `private Slider FindSlider(string relativePath)` | 229 | Tìm slider trong scene/danh sách dữ liệu. Liên kết trực tiếp: GameSettingsMenuViewUtility. | GameSettingsMenuViewUtility |
| `private Button FindButton(string relativePath)` | 234 | Tìm button trong scene/danh sách dữ liệu. Liên kết trực tiếp: GameSettingsMenuViewUtility. | GameSettingsMenuViewUtility |
| `private TextMeshProUGUI FindText(string relativePath)` | 239 | Tìm text trong scene/danh sách dữ liệu. Liên kết trực tiếp: GameSettingsMenuViewUtility. | GameSettingsMenuViewUtility |
| `private void HandleSettingsChanged()` | 244 | Xử lý luồng settings changed. | - |

#### PlayerUIShopManager

- **Đường dẫn:** `Assets/Game/Scripts/Character/Player/PlayerUI/PlayerUIShopManager.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Điều khiển một phần UI của player như HUD, menu, popup, shop, level up, site of grace hoặc equipment.
- **Kế thừa/cha:** PlayerUIMenu
- **Script con:** -
- **Field public/serialized chính:** [SerializeField] private RectTransform listContentRoot, [SerializeField] private RectTransform stockContentRoot, [SerializeField] private RectTransform priceContentRoot, [SerializeField] private ScrollRect listScrollRect, [SerializeField] private TextMeshProUGUI titleText, [SerializeField] private TextMeshProUGUI runeText, [SerializeField] private TextMeshProUGUI itemDescriptionText, [SerializeField] private TextMeshProUGUI itemMetaText, [SerializeField] private Button modeButton, [SerializeField] private Button actionButton, [SerializeField] private Button closeButton, [SerializeField] private Button entryButtonTemplate +3
- **Liên kết script:** Item, PlayerUIManager, PlayerUIMenu, ShopInventory, ShopStockEntry, ShopViewMode

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `new Color(0.08f, 0.08f, 0.08f, 1f)` | 16 | Thực hiện logic color trong script PlayerUIShopManager. | - |
| `new Color(0.28f, 0.18f, 0.08f, 1f)` | 17 | Thực hiện logic color trong script PlayerUIShopManager. | - |
| `private void Awake()` | 45 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. | - |
| `public void OpenGlobalShop(string shopTitle)` | 50 | Mở UI/trạng thái/luồng global shop. Liên kết trực tiếp: ShopInventory. | ShopInventory |
| `new GameObject("Runtime Global Shop")` | 54 | Thực hiện logic game object trong script PlayerUIShopManager. | - |
| `public void OpenShop(ShopInventory shopInventory)` | 63 | Mở UI/trạng thái/luồng shop. Liên kết trực tiếp: ShopViewMode. | ShopViewMode |
| `public override void CloseMenu()` | 74 | Đóng UI/trạng thái/luồng menu. | - |
| `public void RefreshCurrentView()` | 81 | Làm mới dữ liệu/hiển thị current view. Liên kết trực tiếp: PlayerUIManager, ShopViewMode. | PlayerUIManager, ShopViewMode |
| `private void PopulateEntries()` | 105 | Thực hiện logic populate entries trong script PlayerUIShopManager. Liên kết trực tiếp: Item, PlayerUIManager, ShopViewMode. | Item, PlayerUIManager, ShopViewMode |
| `private List<Item> GetBuyItems()` | 135 | Lấy dữ liệu buy items cho hệ thống khác sử dụng. Liên kết trực tiếp: Item, ShopStockEntry. | Item, ShopStockEntry |
| `private Button CreateEntryButton(Item item)` | 151 | Tạo object/dữ liệu entry button. Liên kết trực tiếp: Item. | Item |
| `new Vector2(0f, 72f)` | 159 | Thực hiện logic vector2 trong script PlayerUIShopManager. | - |
| `private void BindEntryButton(Transform buttonTransform, Transform stockTransform, Transform priceTransform, Item item)` | 185 | Thực hiện logic bind entry button trong script PlayerUIShopManager. Liên kết trực tiếp: Item, PlayerUIManager, ShopViewMode. | Item, PlayerUIManager, ShopViewMode |
| `private int GetBuyPrice(Item item)` | 220 | Lấy dữ liệu buy price cho hệ thống khác sử dụng. | - |
| `private void RefreshSelectionDetails()` | 228 | Làm mới dữ liệu/hiển thị selection details. Liên kết trực tiếp: Item, PlayerUIManager, ShopViewMode. | Item, PlayerUIManager, ShopViewMode |
| `private void ToggleViewMode()` | 263 | Thực hiện logic toggle view mode trong script PlayerUIShopManager. Liên kết trực tiếp: ShopViewMode. | ShopViewMode |
| `private void PerformCurrentTransaction()` | 270 | Thực hiện logic perform current transaction trong script PlayerUIShopManager. Liên kết trực tiếp: PlayerUIManager, ShopStockEntry, ShopViewMode. | PlayerUIManager, ShopStockEntry, ShopViewMode |
| `new ShopStockEntry()` | 279 | Thực hiện logic shop stock entry trong script PlayerUIShopManager. | - |
| `private void ConfigureStaticUI()` | 300 | Thực hiện logic configure static ui trong script PlayerUIShopManager. | - |
| `private void ResolveRuntimeReferences()` | 347 | Thực hiện logic resolve runtime references trong script PlayerUIShopManager. Liên kết trực tiếp: Item. | Item |
| `private void ConfigureEntryTemplate()` | 366 | Thực hiện logic configure entry template trong script PlayerUIShopManager. Liên kết trực tiếp: Item. | Item |
| `new Color(0f, 0f, 0f, 0f)` | 376 | Thực hiện logic color trong script PlayerUIShopManager. | - |
| `new Vector2(0f, 0f)` | 390 | Thực hiện logic vector2 trong script PlayerUIShopManager. | - |
| `new Vector2(1f, 1f)` | 391 | Thực hiện logic vector2 trong script PlayerUIShopManager. | - |
| `new Vector2(-8f, -8f)` | 393 | Thực hiện logic vector2 trong script PlayerUIShopManager. | - |
| `new Vector2(0.5f, 0.5f)` | 394 | Thực hiện logic vector2 trong script PlayerUIShopManager. | - |
| `private void ConfigureEntryText(TextMeshProUGUI text, float fontSize, TextAlignmentOptions alignment)` | 417 | Thực hiện logic configure entry text trong script PlayerUIShopManager. | - |
| `private static TextMeshProUGUI FindText(Transform root, string childName)` | 429 | Tìm text trong scene/danh sách dữ liệu. | - |
| `private static RectTransform FindRect(Transform root, string childName)` | 445 | Tìm rect trong scene/danh sách dữ liệu. | - |
| `private static Image FindImage(Transform root, string childName)` | 451 | Tìm image trong scene/danh sách dữ liệu. | - |
| `private static TextMeshProUGUI FindEntryText(Transform root, string childName)` | 457 | Tìm entry text trong scene/danh sách dữ liệu. | - |
| `return FindText(root, childName)` | 459 | Tìm text trong scene/danh sách dữ liệu. | - |
| `private static Image FindEntryImage(Transform root, string childName)` | 462 | Tìm entry image trong scene/danh sách dữ liệu. | - |
| `return FindImage(root, childName)` | 464 | Tìm image trong scene/danh sách dữ liệu. | - |
| `private static Transform FindChild(Transform root, string childName)` | 467 | Tìm child trong scene/danh sách dữ liệu. | - |
| `private void UpdateEntrySelectionVisuals()` | 483 | Cập nhật entry selection visuals theo trạng thái mới. Liên kết trực tiếp: Item. | Item |
| `private void ResetScrollPosition()` | 505 | Đưa scroll position về trạng thái mặc định. | - |
| `private void SetButtonLabel(Button button, string label)` | 515 | Thiết lập giá trị hoặc trạng thái button label. | - |
| `private void ClearEntryButtons()` | 526 | Thực hiện logic clear entry buttons trong script PlayerUIShopManager. | - |
| `private void ConfigureColumnContentRoot(RectTransform contentRoot, int leftPadding, int rightPadding)` | 546 | Thực hiện logic configure column content root trong script PlayerUIShopManager. | - |
| `new RectOffset(leftPadding, rightPadding, 12, 12)` | 557 | Thực hiện logic rect offset trong script PlayerUIShopManager. | - |
| `private GameObject CreatePassiveEntry(Image template, RectTransform parent, string objectName)` | 573 | Tạo object/dữ liệu passive entry. | - |
| `new Vector2(0f, 72f)` | 586 | Thực hiện logic vector2 trong script PlayerUIShopManager. | - |
| `private void SyncAuxiliaryColumnScrolls(Vector2 _)` | 591 | Thực hiện logic sync auxiliary column scrolls trong script PlayerUIShopManager. | - |
| `private void SyncContentPosition(RectTransform contentRoot)` | 600 | Thực hiện logic sync content position trong script PlayerUIShopManager. | - |

#### PlayerUISiteOfGraceManager

- **Đường dẫn:** `Assets/Game/Scripts/Character/Player/PlayerUI/PlayerUISiteOfGraceManager.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Điều khiển một phần UI của player như HUD, menu, popup, shop, level up, site of grace hoặc equipment.
- **Kế thừa/cha:** PlayerUIMenu
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** PlayerUIManager, PlayerUIMenu

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public void OpenTeleportLocationMenu()` | 7 | Mở UI/trạng thái/luồng teleport location menu. Liên kết trực tiếp: PlayerUIManager. | PlayerUIManager |
| `public void OpenLevelUpMenu()` | 12 | Mở UI/trạng thái/luồng level up menu. Liên kết trực tiếp: PlayerUIManager. | PlayerUIManager |

#### PlayerUITeleportLocationManager

- **Đường dẫn:** `Assets/Game/Scripts/Character/Player/PlayerUI/PlayerUITeleportLocationManager.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Điều khiển một phần UI của player như HUD, menu, popup, shop, level up, site of grace hoặc equipment.
- **Kế thừa/cha:** PlayerUIMenu
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** PlayerUIMenu, WorldObjectManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public override void OpenMenu()` | 11 | Mở UI/trạng thái/luồng menu. | - |
| `private void CheckForUnlockedTeleports()` | 18 | Thực hiện logic check for unlocked teleports trong script PlayerUITeleportLocationManager. Liên kết trực tiếp: WorldObjectManager. | WorldObjectManager |
| `public void TeleportToSiteOfGrace(int siteID)` | 47 | Thực hiện logic teleport to site of grace trong script PlayerUITeleportLocationManager. Liên kết trực tiếp: WorldObjectManager. | WorldObjectManager |

#### PlayerUIToggleHud

- **Đường dẫn:** `Assets/Game/Scripts/Character/Player/PlayerUI/PlayerUIToggleHud.cs`
- **Loại:** MonoBehaviour
- **Vai trò dễ hiểu:** Điều khiển một phần UI của player như HUD, menu, popup, shop, level up, site of grace hoặc equipment.
- **Kế thừa/cha:** MonoBehaviour
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** PlayerUIManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `private void OnEnable()` | 7 | Đăng ký event/callback hoặc bật trạng thái khi component được enable. Liên kết trực tiếp: PlayerUIManager. | PlayerUIManager |
| `private void OnDisable()` | 13 | Gỡ event/callback hoặc dọn trạng thái khi component bị disable. Liên kết trực tiếp: PlayerUIManager. | PlayerUIManager |

#### PlayerUIWeaponUpgradeManager

- **Đường dẫn:** `Assets/Game/Scripts/Character/Player/PlayerUI/PlayerUIWeaponUpgradeManager.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Điều khiển một phần UI của player như HUD, menu, popup, shop, level up, site of grace hoặc equipment.
- **Kế thừa/cha:** PlayerUIMenu
- **Script con:** -
- **Field public/serialized chính:** public EquipmentType currentSelectedEquipmentSlot, [SerializeField] private WeaponItem currentSelectedWeapon
- **Liên kết script:** CharacterSlot, EquipmentType, PlayerManager, PlayerUIManager, PlayerUIMenu, UpgradeLevel, UpgradeMaterial, UpgradeStone, WeaponItem, WorldItemDatabase, WorldSaveGameManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `private void Awake()` | 44 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. | - |
| `private void Update()` | 55 | Cập nhật logic mỗi frame như input, timer, trạng thái chiến đấu hoặc UI. | - |
| `public override void OpenMenu()` | 64 | Mở UI/trạng thái/luồng menu. | - |
| `private void AlignMenuTitle()` | 77 | Thực hiện logic align menu title trong script PlayerUIWeaponUpgradeManager. | - |
| `new Vector2(0f, 1f)` | 98 | Thực hiện logic vector2 trong script PlayerUIWeaponUpgradeManager. | - |
| `new Vector2(1f, 1f)` | 99 | Thực hiện logic vector2 trong script PlayerUIWeaponUpgradeManager. | - |
| `new Vector2(0.5f, 0.5f)` | 100 | Thực hiện logic vector2 trong script PlayerUIWeaponUpgradeManager. | - |
| `new Vector2(0f, -64f)` | 101 | Thực hiện logic vector2 trong script PlayerUIWeaponUpgradeManager. | - |
| `new Vector2(0f, 50f)` | 102 | Thực hiện logic vector2 trong script PlayerUIWeaponUpgradeManager. | - |
| `private void ToggleEquipmentButtons(bool isEnabled)` | 115 | Thực hiện logic toggle equipment buttons trong script PlayerUIWeaponUpgradeManager. | - |
| `private void RefreshEquipmentSlotIcons()` | 126 | Làm mới dữ liệu/hiển thị equipment slot icons. Liên kết trực tiếp: PlayerManager, WeaponItem. | PlayerManager, WeaponItem |
| `public void AttemptToUpgradeWeapon()` | 204 | Cố gắng kích hoạt to upgrade weapon nếu trạng thái hiện tại cho phép. Liên kết trực tiếp: PlayerUIManager, UpgradeLevel, WorldItemDatabase. | PlayerUIManager, UpgradeLevel, WorldItemDatabase |
| `public void UpgradeWeapon()` | 232 | Thực hiện logic upgrade weapon trong script PlayerUIWeaponUpgradeManager. Liên kết trực tiếp: CharacterSlot, PlayerManager, PlayerUIManager, UpgradeLevel, WorldSaveGameManager. | CharacterSlot, PlayerManager, PlayerUIManager, UpgradeLevel, WorldSaveGameManager |
| `public void SelectEquipmentSlot(int equipmentSlot)` | 268 | Thực hiện logic select equipment slot trong script PlayerUIWeaponUpgradeManager. Liên kết trực tiếp: EquipmentType, PlayerUIManager, UpgradeLevel. | EquipmentType, PlayerUIManager, UpgradeLevel |
| `public void SelectLastSelectedEquipmentSlot()` | 345 | Thực hiện logic select last selected equipment slot trong script PlayerUIWeaponUpgradeManager. Liên kết trực tiếp: EquipmentType. | EquipmentType |
| `private bool PlayerHasUpgradeCost()` | 380 | Phát er has upgrade cost, thường là animation, sound hoặc VFX. Liên kết trực tiếp: PlayerUIManager, UpgradeMaterial, WorldItemDatabase. | PlayerUIManager, UpgradeMaterial, WorldItemDatabase |
| `private UpgradeMaterial DetermineUpgradeCostOfWeapon(WeaponItem weapon)` | 428 | Thực hiện logic determine upgrade cost of weapon trong script PlayerUIWeaponUpgradeManager. Liên kết trực tiếp: UpgradeLevel, UpgradeMaterial, UpgradeStone, WorldItemDatabase. | UpgradeLevel, UpgradeMaterial, UpgradeStone, WorldItemDatabase |

### Assets/Game/Scripts/Colliders

#### DamageCollider

- **Đường dẫn:** `Assets/Game/Scripts/Colliders/DamageCollider.cs`
- **Loại:** MonoBehaviour
- **Vai trò dễ hiểu:** Collider/hitbox gây sát thương hoặc phát hiện va chạm để áp dụng damage/effect.
- **Kế thừa/cha:** MonoBehaviour
- **Script con:** DurkClubDamageCollider, DurkStompCollider, ManualDamageCollider, MeleeWeaponDamageCollider, RangedProjectileDamageCollider, SpellProjectileDamageCollider
- **Field public/serialized chính:** public Collider damageCollider, public float physicalDamage, public float magicDamage, public float fireDamage, public float lightningDamage, public float holyDamage, public float poiseDamage
- **Liên kết script:** CharacterManager, TakeBlockedDamageEffect, TakeDamageEffect, WorldCharacterEffectsManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `protected virtual void Awake()` | 32 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. | - |
| `protected virtual void OnTriggerEnter(Collider other)` | 38 | Xử lý khi collider khác đi vào trigger của object này. Liên kết trực tiếp: CharacterManager. | CharacterManager |
| `protected virtual void CheckForBlock(CharacterManager damageTarget)` | 56 | Thực hiện logic check for block trong script DamageCollider. Liên kết trực tiếp: TakeBlockedDamageEffect, WorldCharacterEffectsManager. | TakeBlockedDamageEffect, WorldCharacterEffectsManager |
| `protected virtual void CheckForParry(CharacterManager damageTarget)` | 82 | Thực hiện logic check for parry trong script DamageCollider. | - |
| `protected virtual void GetBlockingDotValues(CharacterManager damageTarget)` | 87 | Lấy dữ liệu blocking dot values cho hệ thống khác sử dụng. | - |
| `protected virtual void DamageTarget(CharacterManager damageTarget)` | 93 | Gây hoặc xử lý sát thương cho target. Liên kết trực tiếp: TakeDamageEffect, WorldCharacterEffectsManager. | TakeDamageEffect, WorldCharacterEffectsManager |
| `public virtual void EnableDamageCollider()` | 114 | Bật damage collider. | - |
| `public virtual void DisableDamageCollider()` | 126 | Tắt damage collider. | - |

#### DurkClubDamageCollider

- **Đường dẫn:** `Assets/Game/Scripts/Colliders/DurkClubDamageCollider.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Collider/hitbox gây sát thương hoặc phát hiện va chạm để áp dụng damage/effect.
- **Kế thừa/cha:** DamageCollider
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** AIBossCharacterManager, CharacterManager, DamageCollider, TakeDamageEffect, WorldCharacterEffectsManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `protected override void Awake()` | 9 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. Liên kết trực tiếp: AIBossCharacterManager. | AIBossCharacterManager |
| `protected override void DamageTarget(CharacterManager damageTarget)` | 17 | Gây hoặc xử lý sát thương cho target. Liên kết trực tiếp: TakeDamageEffect, WorldCharacterEffectsManager. | TakeDamageEffect, WorldCharacterEffectsManager |

#### DurkStompCollider

- **Đường dẫn:** `Assets/Game/Scripts/Colliders/DurkStompCollider.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Collider/hitbox gây sát thương hoặc phát hiện va chạm để áp dụng damage/effect.
- **Kế thừa/cha:** DamageCollider
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** AIDurkCharacterManager, CharacterManager, DamageCollider, TakeDamageEffect, WorldCharacterEffectsManager, WorldUtilityManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `protected override void Awake()` | 10 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. Liên kết trực tiếp: AIDurkCharacterManager. | AIDurkCharacterManager |
| `public void StompAttack()` | 19 | Thực hiện logic stomp attack trong script DurkStompCollider. Liên kết trực tiếp: CharacterManager, TakeDamageEffect, WorldCharacterEffectsManager, WorldUtilityManager. | CharacterManager, TakeDamageEffect, WorldCharacterEffectsManager, WorldUtilityManager |

#### ManualDamageCollider

- **Đường dẫn:** `Assets/Game/Scripts/Colliders/ManualDamageCollider.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Collider/hitbox gây sát thương hoặc phát hiện va chạm để áp dụng damage/effect.
- **Kế thừa/cha:** DamageCollider
- **Script con:** Monster33FireDamageCollider
- **Field public/serialized chính:** -
- **Liên kết script:** AICharacterManager, AIKnightCombatManager, AIMonster33CombatManager, CharacterManager, DamageCollider, PlayerEffectsManager, PlayerManager, TakeDamageEffect, WorldCharacterEffectsManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `protected override void Awake()` | 9 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. Liên kết trực tiếp: AICharacterManager. | AICharacterManager |
| `protected override void GetBlockingDotValues(CharacterManager damageTarget)` | 17 | Lấy dữ liệu blocking dot values cho hệ thống khác sử dụng. | - |
| `protected override void DamageTarget(CharacterManager damageTarget)` | 23 | Gây hoặc xử lý sát thương cho target. Liên kết trực tiếp: TakeDamageEffect, WorldCharacterEffectsManager. | TakeDamageEffect, WorldCharacterEffectsManager |
| `private void ApplyMonster33PowerUpFireBuildUp(CharacterManager damageTarget)` | 63 | Áp dụng monster33 power up fire build up lên character/object mục tiêu. Liên kết trực tiếp: AIMonster33CombatManager, PlayerEffectsManager, PlayerManager. | AIMonster33CombatManager, PlayerEffectsManager, PlayerManager |
| `private void ApplyKnightPowerUpFrostBuildUp(CharacterManager damageTarget)` | 81 | Áp dụng knight power up frost build up lên character/object mục tiêu. Liên kết trực tiếp: AIKnightCombatManager, PlayerManager. | AIKnightCombatManager, PlayerManager |
| `protected override void CheckForParry(CharacterManager damageTarget)` | 99 | Thực hiện logic check for parry trong script ManualDamageCollider. | - |

#### MeleeWeaponDamageCollider

- **Đường dẫn:** `Assets/Game/Scripts/Colliders/MeleeWeaponDamageCollider.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Collider/hitbox gây sát thương hoặc phát hiện va chạm để áp dụng damage/effect.
- **Kế thừa/cha:** DamageCollider
- **Script con:** -
- **Field public/serialized chính:** public CharacterManager characterCausingDamage, public float light_Attack_01_Modifier, public float light_Attack_02_Modifier, public float light_Jump_Attack_01_Modifier, public float heavy_Attack_01_Modifier, public float heavy_Attack_02_Modifier, public float heavy_Jump_Attack_01_Modifier, public float charge_Attack_01_Modifier, public float charge_Attack_02_Modifier, public float running_Attack_01_Modifier, public float rolling_Attack_01_Modifier, public float backstep_Attack_01_Modifier +6
- **Liên kết script:** AttackType, CharacterManager, DamageCollider, TakeDamageEffect, WorldCharacterEffectsManager, WorldUtilityManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `protected override void Awake()` | 29 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. | - |
| `protected override void OnTriggerEnter(Collider other)` | 40 | Xử lý khi collider khác đi vào trigger của object này. Liên kết trực tiếp: CharacterManager, WorldUtilityManager. | CharacterManager, WorldUtilityManager |
| `protected override void CheckForParry(CharacterManager damageTarget)` | 68 | Thực hiện logic check for parry trong script MeleeWeaponDamageCollider. | - |
| `protected override void GetBlockingDotValues(CharacterManager damageTarget)` | 87 | Lấy dữ liệu blocking dot values cho hệ thống khác sử dụng. | - |
| `protected override void DamageTarget(CharacterManager damageTarget)` | 93 | Gây hoặc xử lý sát thương cho target. Liên kết trực tiếp: AttackType, TakeDamageEffect, WorldCharacterEffectsManager. | AttackType, TakeDamageEffect, WorldCharacterEffectsManager |
| `private void ApplyAttackDamageModifiers(float modifier, TakeDamageEffect damage)` | 188 | Áp dụng attack damage modifiers lên character/object mục tiêu. | - |

#### RangedProjectileDamageCollider

- **Đường dẫn:** `Assets/Game/Scripts/Colliders/RangedProjectileDamageCollider.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Collider/hitbox gây sát thương hoặc phát hiện va chạm để áp dụng damage/effect.
- **Kế thừa/cha:** DamageCollider
- **Script con:** -
- **Field public/serialized chính:** public CharacterManager characterShootingProjectile, public Rigidbody rigidbody
- **Liên kết script:** CharacterManager, DamageCollider, TakeBlockedDamageEffect, WorldCharacterEffectsManager, WorldSoundFXManager, WorldUtilityManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `protected override void Awake()` | 15 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. | - |
| `private void FixedUpdate()` | 23 | Cập nhật theo bước vật lý, thường xử lý movement, trigger hoặc physics. | - |
| `private void OnCollisionEnter(Collision collision)` | 31 | Xử lý va chạm vật lý khi object chạm object khác. Liên kết trực tiếp: CharacterManager, WorldSoundFXManager, WorldUtilityManager. | CharacterManager, WorldSoundFXManager, WorldUtilityManager |
| `protected override void CheckForBlock(CharacterManager damageTarget)` | 57 | Thực hiện logic check for block trong script RangedProjectileDamageCollider. Liên kết trực tiếp: TakeBlockedDamageEffect, WorldCharacterEffectsManager. | TakeBlockedDamageEffect, WorldCharacterEffectsManager |
| `private void CreatePenetrationIntoObject(Collision hit)` | 85 | Tạo object/dữ liệu penetration into object. | - |
| `new GameObject()` | 95 | Thực hiện logic game object trong script RangedProjectileDamageCollider. | - |

### Assets/Game/Scripts/Editor

#### DurkMaterialFixer

- **Đường dẫn:** `Assets/Game/Scripts/Editor/DurkMaterialFixer.cs`
- **Loại:** Editor tool
- **Vai trò dễ hiểu:** Công cụ chạy trong Unity Editor để dựng, sửa, kiểm tra hoặc tự động cấu hình scene/prefab/asset.
- **Kế thừa/cha:** -
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** GameAssetPaths

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `static DurkMaterialFixer()` | 13 | Thực hiện logic durk material fixer trong script DurkMaterialFixer. | - |
| `public static void ApplyDurkBossMaterials()` | 19 | Áp dụng durk boss materials lên character/object mục tiêu. | - |
| `private static bool IsClubRenderer(Renderer renderer)` | 87 | Kiểm tra điều kiện/trạng thái club renderer. | - |
| `private static void TryAutoFixOnce()` | 112 | Thử thực hiện auto fix once, thường có kiểm tra điều kiện trước khi chạy. | - |

#### GeneratedWorldCleanupUtility

- **Đường dẫn:** `Assets/Game/Scripts/Editor/GeneratedWorldCleanupUtility.cs`
- **Loại:** Editor tool
- **Vai trò dễ hiểu:** Công cụ chạy trong Unity Editor để dựng, sửa, kiểm tra hoặc tự động cấu hình scene/prefab/asset.
- **Kế thừa/cha:** -
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** GameAssetPaths, RandomMapGenerator

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public static void ListWorld02Roots()` | 16 | Thực hiện logic list world02 roots trong script GeneratedWorldCleanupUtility. | - |
| `public static void CleanupWorld02CloneRoots()` | 24 | Thực hiện logic cleanup world02 clone roots trong script GeneratedWorldCleanupUtility. | - |
| `public static void CleanupCurrentGeneratedWorld()` | 31 | Thực hiện logic cleanup current generated world trong script GeneratedWorldCleanupUtility. | - |
| `private static void CleanupOpenScene(Scene scene)` | 36 | Thực hiện logic cleanup open scene trong script GeneratedWorldCleanupUtility. | - |
| `old NavMeshSurface(s).")` | 56 | Thực hiện logic nav mesh surface trong script GeneratedWorldCleanupUtility. | - |
| `private static int RemoveOldNavMeshSurfaces(Scene scene)` | 59 | Loại bỏ old nav mesh surfaces khỏi danh sách, trạng thái hoặc dữ liệu. | - |
| `private static bool IsGeneratedNavMeshSurface(NavMeshSurface surface)` | 78 | Kiểm tra điều kiện/trạng thái generated nav mesh surface. | - |
| `private static bool ShouldKeepRoot(GameObject root)` | 95 | Thực hiện logic should keep root trong script GeneratedWorldCleanupUtility. Liên kết trực tiếp: RandomMapGenerator. | RandomMapGenerator |

#### KnightBossArenaSetupUtility

- **Đường dẫn:** `Assets/Game/Scripts/Editor/KnightBossArenaSetupUtility.cs`
- **Loại:** Editor tool
- **Vai trò dễ hiểu:** Công cụ chạy trong Unity Editor để dựng, sửa, kiểm tra hoặc tự động cấu hình scene/prefab/asset.
- **Kế thừa/cha:** -
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** GameAssetPaths, Interactable

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public static void SetupKnightBossArena()` | 17 | Thiết lập giá trị hoặc trạng thái up knight boss arena. | - |
| `new GameObject("Knight Boss Arena Setup")` | 33 | Thực hiện logic game object trong script KnightBossArenaSetupUtility. | - |
| `new Vector3(14f, 4f, 8f)` | 76 | Thực hiện logic vector3 trong script KnightBossArenaSetupUtility. | - |
| `new Vector3(0f, 1.5f, 0f)` | 77 | Thực hiện logic vector3 trong script KnightBossArenaSetupUtility. | - |

#### MerchantSetupChecklistGenerator

- **Đường dẫn:** `Assets/Game/Scripts/Editor/MerchantSetupChecklistGenerator.cs`
- **Loại:** Editor tool
- **Vai trò dễ hiểu:** Công cụ chạy trong Unity Editor để dựng, sửa, kiểm tra hoặc tự động cấu hình scene/prefab/asset.
- **Kế thừa/cha:** -
- **Script con:** -
- **Field public/serialized chính:** public string assetPath, public string hierarchyPath, public string shopName, public string merchantID, public bool autoScaleShopTierFromProgression, public int shopTierOffset, public bool useGlobalPurchasableItems
- **Liên kết script:** GameAssetPaths, MerchantRecord, ShopInventory

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public static void GenerateChecklistMenuItem()` | 27 | Thực hiện logic generate checklist menu item trong script MerchantSetupChecklistGenerator. | - |
| `public static void GenerateChecklist()` | 32 | Thực hiện logic generate checklist trong script MerchantSetupChecklistGenerator. Liên kết trực tiếp: MerchantRecord. | MerchantRecord |
| `ChecklistPath, BuildChecklistContent(merchantRecords))` | 40 | Thực hiện logic build checklist content trong script MerchantSetupChecklistGenerator. | - |
| `private static void GatherPrefabMerchants(List<MerchantRecord> merchantRecords)` | 51 | Thực hiện logic gather prefab merchants trong script MerchantSetupChecklistGenerator. Liên kết trực tiếp: ShopInventory. | ShopInventory |
| `prefabPath, GetHierarchyPath(shop.transform), shop))` | 72 | Lấy dữ liệu hierarchy path cho hệ thống khác sử dụng. | - |
| `private static void GatherSceneMerchants(List<MerchantRecord> merchantRecords)` | 77 | Thực hiện logic gather scene merchants trong script MerchantSetupChecklistGenerator. Liên kết trực tiếp: ShopInventory. | ShopInventory |
| `scenePath, GetHierarchyPath(shop.transform), shop))` | 102 | Lấy dữ liệu hierarchy path cho hệ thống khác sử dụng. | - |
| `private static MerchantRecord CreateRecord(string assetPath, string hierarchyPath, ShopInventory shop)` | 116 | Tạo object/dữ liệu record. Liên kết trực tiếp: MerchantRecord. | MerchantRecord |
| `new SerializedObject(shop)` | 118 | Thực hiện logic serialized object trong script MerchantSetupChecklistGenerator. | - |
| `private static string GetHierarchyPath(Transform currentTransform)` | 132 | Lấy dữ liệu hierarchy path cho hệ thống khác sử dụng. | - |
| `private static string BuildChecklistContent(List<MerchantRecord> merchantRecords)` | 148 | Thực hiện logic build checklist content trong script MerchantSetupChecklistGenerator. Liên kết trực tiếp: MerchantRecord, ShopInventory. | MerchantRecord, ShopInventory |
| `new StringBuilder()` | 150 | Thực hiện logic string builder trong script MerchantSetupChecklistGenerator. | - |

#### Monster30BossPrefabBuilder

- **Đường dẫn:** `Assets/Game/Scripts/Editor/Monster30BossPrefabBuilder.cs`
- **Loại:** Editor tool
- **Vai trò dễ hiểu:** Công cụ chạy trong Unity Editor để dựng, sửa, kiểm tra hoặc tự động cấu hình scene/prefab/asset.
- **Kế thừa/cha:** -
- **Script con:** -
- **Field public/serialized chính:** public readonly HumanBodyBones Bone, public readonly float Radius, public readonly float Height, public readonly int Direction, public readonly Vector3 Center, public readonly string[] FallbackNames
- **Liên kết script:** AIBossCharacterManager, AIBossCharacterNetworkManager, AICharacterAttackAction, AICharacterCombatManager, AICharacterManager, AICharacterNetworkManager, AICharacterSoundFXManager, AIMonster30CharacterManager, AIMonster30CombatManager, AttackDamageProfile, AttackState, AttackType, BoneCapsuleDefinition, CharacterGroup, CharacterSoundFXManager, CharacterStatsManager +9

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public static void CreateMonster30Boss()` | 52 | Tạo object/dữ liệu monster30 boss. Liên kết trực tiếp: AIBossCharacterManager. | AIBossCharacterManager |
| `public static void MakeMonster30BossPrefabEditable()` | 171 | Thực hiện logic make monster30 boss prefab editable trong script Monster30BossPrefabBuilder. | - |
| `private static void ReplaceCharacterManager(GameObject prefabRoot, AIBossCharacterManager bossReferenceManager)` | 195 | Thực hiện logic replace character manager trong script Monster30BossPrefabBuilder. Liên kết trực tiếp: AICharacterManager, AIMonster30CharacterManager, AttackState, CharacterGroup, CombatStanceState +3. | AICharacterManager, AIMonster30CharacterManager, AttackState, CharacterGroup, CombatStanceState, IdleState, InvestigateSoundState, PursueTargetState |
| `new SerializedObject(bossManager)` | 214 | Thực hiện logic serialized object trong script Monster30BossPrefabBuilder. | - |
| `private static void ReplaceCombatManager(GameObject prefabRoot)` | 236 | Thực hiện logic replace combat manager trong script Monster30BossPrefabBuilder. Liên kết trực tiếp: AICharacterCombatManager, AIMonster30CombatManager. | AICharacterCombatManager, AIMonster30CombatManager |
| `private static void ReplaceNetworkManager(GameObject prefabRoot)` | 250 | Thực hiện logic replace network manager trong script Monster30BossPrefabBuilder. Liên kết trực tiếp: AIBossCharacterNetworkManager, AICharacterNetworkManager. | AIBossCharacterNetworkManager, AICharacterNetworkManager |
| `private static void ReplaceSoundFXManager(GameObject prefabRoot, GameObject bossReference)` | 264 | Thực hiện logic replace sound fxmanager trong script Monster30BossPrefabBuilder. Liên kết trực tiếp: AICharacterSoundFXManager, CharacterSoundFXManager. | AICharacterSoundFXManager, CharacterSoundFXManager |
| `new SerializedObject(referenceSoundFX)` | 280 | Thực hiện logic serialized object trong script Monster30BossPrefabBuilder. | - |
| `new SerializedObject(monster30SoundFX)` | 281 | Thực hiện logic serialized object trong script Monster30BossPrefabBuilder. | - |
| `private static void PrepareBossStats(GameObject prefabRoot)` | 292 | Thực hiện logic prepare boss stats trong script Monster30BossPrefabBuilder. Liên kết trực tiếp: AICharacterNetworkManager, AIMonster30CombatManager, CharacterStatsManager. | AICharacterNetworkManager, AIMonster30CombatManager, CharacterStatsManager |
| `new SerializedObject(networkManager)` | 297 | Thực hiện logic serialized object trong script Monster30BossPrefabBuilder. | - |
| `new SerializedObject(combatManager)` | 317 | Thực hiện logic serialized object trong script Monster30BossPrefabBuilder. | - |
| `new SerializedObject(statsManager)` | 331 | Thực hiện logic serialized object trong script Monster30BossPrefabBuilder. | - |
| `private static void RemoveOldVisualChildren(Transform root)` | 339 | Loại bỏ old visual children khỏi danh sách, trạng thái hoặc dữ liệu. | - |
| `private static void ConfigureVisualHierarchy(Transform visualRoot)` | 364 | Thực hiện logic configure visual hierarchy trong script Monster30BossPrefabBuilder. | - |
| `private static void RebuildGameplayHooks(GameObject prefabRoot, Transform visualRoot, Animator sourceAnimator)` | 372 | Thực hiện logic rebuild gameplay hooks trong script Monster30BossPrefabBuilder. | - |
| `private static GameObject CreateEmbeddedMonster30Visual(Scene targetScene)` | 381 | Tạo object/dữ liệu embedded monster30 visual. | - |
| `private static void UnpackVisualHierarchyForEditing(GameObject visualRoot)` | 399 | Thực hiện logic unpack visual hierarchy for editing trong script Monster30BossPrefabBuilder. | - |
| `private static void AssignMonster30Animator(Animator rootAnimator)` | 423 | Thực hiện logic assign monster30 animator trong script Monster30BossPrefabBuilder. | - |
| `private static AnimatorController CreateOrUpdateMonster30AnimatorController()` | 435 | Tạo object/dữ liệu or update monster30 animator controller. | - |
| `private static AnimationClip CreateOrUpdateRebasedAnimationClip(string sourceClipPath, string rebasedClipPath)` | 519 | Tạo object/dữ liệu or update rebased animation clip. | - |
| `private static AnimationClip LoadPrimaryAnimationClip(string assetPath)` | 549 | Nạp dữ liệu hoặc scene liên quan tới primary animation clip. | - |
| `private static AnimationClip CreateOrUpdateMappedAttackClip( string sourceAssetPath, string targetClipPath, string setDamageFunctionName, float openNormalizedTime, float closeNormalizedTime, bool enablesCombo)` | 556 | Tạo object/dữ liệu or update mapped attack clip. | - |
| `targetClip, BuildAttackEvents(sourceClip.length, setDamageFunctionName, openNormalizedTime, closeNormalizedTime, enablesCombo))` | 643 | Thực hiện logic build attack events trong script Monster30BossPrefabBuilder. | - |
| `string> BuildHumanoidBonePathMapWithFallback( string primaryAssetPath, string rootPrefix, params string[] candidateAssetPaths)` | 650 | Thực hiện logic build humanoid bone path map with fallback trong script Monster30BossPrefabBuilder. | - |
| `string> BuildHumanoidBonePathMap(string assetPath, string rootPrefix)` | 668 | Thực hiện logic build humanoid bone path map trong script Monster30BossPrefabBuilder. | - |
| `private static GameObject InstantiateHumanoidAsset(GameObject asset)` | 723 | Thực hiện logic instantiate humanoid asset trong script Monster30BossPrefabBuilder. | - |
| `private static string GetBonePathRootPrefix(string assetPath, string requestedRootPrefix)` | 745 | Lấy dữ liệu bone path root prefix cho hệ thống khác sử dụng. | - |
| `private static bool TryMapHumanoidBindingPath( string sourcePath, Dictionary<HumanBodyBones, string> sourceBonePaths, Dictionary<HumanBodyBones, string> targetBonePaths, out string mappedPath)` | 760 | Thử thực hiện map humanoid binding path, thường có kiểm tra điều kiện trước khi chạy. | - |
| `private static bool TryMapBindingPath( string sourcePath, Dictionary<HumanBodyBones, string> sourceBonePaths, Dictionary<HumanBodyBones, string> targetBonePaths, out string mappedPath)` | 791 | Thử thực hiện map binding path, thường có kiểm tra điều kiện trước khi chạy. | - |
| `return TryMapMonster30BindingPath(sourcePath, out mappedPath)` | 802 | Thử thực hiện map monster30 binding path, thường có kiểm tra điều kiện trước khi chạy. | - |
| `private static bool TryMapMonster30BindingPath(string sourcePath, out string mappedPath)` | 805 | Thử thực hiện map monster30 binding path, thường có kiểm tra điều kiện trước khi chạy. | - |
| `in GetMonster30SourceToTargetPathMap().OrderByDescending(entry => entry.Key.Length))` | 807 | Lấy dữ liệu monster30 source to target path map cho hệ thống khác sử dụng. | - |
| `string> GetMonster30SourceToTargetPathMap()` | 829 | Lấy dữ liệu monster30 source to target path map cho hệ thống khác sử dụng. | - |
| `private static void ClearClipCurves(AnimationClip clip)` | 877 | Thực hiện logic clear clip curves trong script Monster30BossPrefabBuilder. | - |
| `private static void ConfigureLoopingForOneShotClip(AnimationClip clip)` | 892 | Thực hiện logic configure looping for one shot clip trong script Monster30BossPrefabBuilder. | - |
| `new SerializedObject(clip)` | 899 | Thực hiện logic serialized object trong script Monster30BossPrefabBuilder. | - |
| `private static void CopyObjectArrayProperty(SerializedProperty source, SerializedProperty target)` | 914 | Thực hiện logic copy object array property trong script Monster30BossPrefabBuilder. | - |
| `private static AnimationEvent[] BuildAttackEvents( float clipLength, string setDamageFunctionName, float openNormalizedTime, float closeNormalizedTime, bool enablesCombo)` | 928 | Thực hiện logic build attack events trong script Monster30BossPrefabBuilder. | - |
| `private static AnimationEvent CreateAnimationEvent(float time, string functionName)` | 960 | Tạo object/dữ liệu animation event. | - |
| `private static void ConfigureLoopingForLocomotionClip(AnimationClip clip, string clipPath)` | 969 | Thực hiện logic configure looping for locomotion clip trong script Monster30BossPrefabBuilder. | - |
| `new SerializedObject(clip)` | 984 | Thực hiện logic serialized object trong script Monster30BossPrefabBuilder. | - |
| `private static void RebaseAnimationClipPaths(AnimationClip clip, string prefix)` | 1000 | Thực hiện logic rebase animation clip paths trong script Monster30BossPrefabBuilder. | - |
| `private static Monster30AIAssetSet CreateOrUpdateMonster30AIAssets()` | 1023 | Tạo object/dữ liệu or update monster30 aiassets. Liên kết trực tiếp: AICharacterAttackAction, AttackState, AttackType, CombatStanceState, Monster30AIAssetSet. | AICharacterAttackAction, AttackState, AttackType, CombatStanceState, Monster30AIAssetSet |
| `new InvalidOperationException("Monster30 boss builder could not create the Monster30 AI assets.")` | 1034 | Thực hiện logic invalid operation exception trong script Monster30BossPrefabBuilder. | - |
| `new Monster30AIAssetSet(attackState, combatStanceState)` | 1085 | Thực hiện logic monster30 aiasset set trong script Monster30BossPrefabBuilder. | - |
| `private static void ConfigureAttackAction( AICharacterAttackAction attackAction, string assetName, string animationName, bool isParryable, AttackType attackType, AICharacterAttackAction comboAction, int attackWeight, float minimumDistance, float maximumDistance)` | 1107 | Thực hiện logic configure attack action trong script Monster30BossPrefabBuilder. Liên kết trực tiếp: AttackType. | AttackType |
| `new SerializedObject(attackAction)` | 1120 | Thực hiện logic serialized object trong script Monster30BossPrefabBuilder. | - |
| `private static void ConfigureCombatStance( CombatStanceState combatStanceState, AICharacterAttackAction attack01, AICharacterAttackAction attack02, AICharacterAttackAction attack03)` | 1135 | Thực hiện logic configure combat stance trong script Monster30BossPrefabBuilder. | - |
| `new SerializedObject(combatStanceState)` | 1143 | Thực hiện logic serialized object trong script Monster30BossPrefabBuilder. | - |
| `private static void SetObjectArrayProperty(SerializedProperty arrayProperty, params UnityEngine.Object[] objects)` | 1158 | Thiết lập giá trị hoặc trạng thái object array property. | - |
| `private static void ApplyStateMotion(AnimatorController controller, string stateName, Motion motion)` | 1172 | Áp dụng state motion lên character/object mục tiêu. | - |
| `private static void EnsureComboState(AnimatorController controller, AnimationClip comboClip)` | 1184 | Thực hiện logic ensure combo state trong script Monster30BossPrefabBuilder. | - |
| `new Vector3(870f, -190f, 0f))` | 1194 | Thực hiện logic vector3 trong script Monster30BossPrefabBuilder. | - |
| `private static AnimatorState FindState(AnimatorController controller, string stateName)` | 1224 | Tìm state trong scene/danh sách dữ liệu. | - |
| `private static AnimatorStateMachine FindStateMachine(AnimatorController controller, string stateMachineName)` | 1238 | Tìm state machine trong scene/danh sách dữ liệu. | - |
| `private static AnimatorState FindStateRecursive(AnimatorStateMachine stateMachine, string stateName)` | 1252 | Tìm state recursive trong scene/danh sách dữ liệu. | - |
| `private static AnimatorStateMachine FindStateMachineRecursive(AnimatorStateMachine stateMachine, string stateMachineName)` | 1274 | Tìm state machine recursive trong scene/danh sách dữ liệu. | - |
| `private static AnimatorState FindStateInStateMachine(AnimatorStateMachine stateMachine, string stateName)` | 1293 | Tìm state in state machine trong scene/danh sách dữ liệu. | - |
| `private static void RemoveStateIfExists(AnimatorStateMachine stateMachine, string stateName)` | 1311 | Loại bỏ state if exists khỏi danh sách, trạng thái hoặc dữ liệu. | - |
| `private static void DeleteLegacyAnimatorController()` | 1329 | Thực hiện logic delete legacy animator controller trong script Monster30BossPrefabBuilder. | - |
| `private static void EnsureFolderExistsForAsset(string assetPath)` | 1340 | Thực hiện logic ensure folder exists for asset trong script Monster30BossPrefabBuilder. | - |
| `private static void EnsureLockOnTarget(Animator animator)` | 1363 | Thực hiện logic ensure lock on target trong script Monster30BossPrefabBuilder. Liên kết trực tiếp: LockOnTransform. | LockOnTransform |
| `private static void EnsureMainHurtbox(Animator animator)` | 1381 | Thực hiện logic ensure main hurtbox trong script Monster30BossPrefabBuilder. | - |
| `new Vector3(0f, 0.55f, 0f)` | 1395 | Thực hiện logic vector3 trong script Monster30BossPrefabBuilder. | - |
| `private static void EnsureWeaponDamageColliders(GameObject prefabRoot, Animator animator)` | 1398 | Thực hiện logic ensure weapon damage colliders trong script Monster30BossPrefabBuilder. Liên kết trực tiếp: AIMonster30CombatManager. | AIMonster30CombatManager |
| `new SerializedObject(combatManager)` | 1418 | Thực hiện logic serialized object trong script Monster30BossPrefabBuilder. | - |
| `private static void EnsureWeaponConstraints(Animator animator)` | 1425 | Thực hiện logic ensure weapon constraints trong script Monster30BossPrefabBuilder. | - |
| `private static void ConfigureWeaponConstraint(Transform weaponRoot, Transform hand)` | 1442 | Thực hiện logic configure weapon constraint trong script Monster30BossPrefabBuilder. | - |
| `private static void EnsureWeaponConstraintBootstrap(GameObject visualRoot)` | 1477 | Thực hiện logic ensure weapon constraint bootstrap trong script Monster30BossPrefabBuilder. Liên kết trực tiếp: Monster30WeaponConstraintBootstrap. | Monster30WeaponConstraintBootstrap |
| `private static void EnsureBodyColliders(Animator animator)` | 1500 | Thực hiện logic ensure body colliders trong script Monster30BossPrefabBuilder. Liên kết trực tiếp: BoneCapsuleDefinition. | BoneCapsuleDefinition |
| `private static ManualDamageCollider EnsureManualDamageCollider(Transform handBone, string hitboxName)` | 1538 | Thực hiện logic ensure manual damage collider trong script Monster30BossPrefabBuilder. Liên kết trực tiếp: ManualDamageCollider. | ManualDamageCollider |
| `private static ManualDamageCollider EnsureWeaponDamageCollider(Transform weaponBone, string hitboxName)` | 1558 | Thực hiện logic ensure weapon damage collider trong script Monster30BossPrefabBuilder. Liên kết trực tiếp: ManualDamageCollider. | ManualDamageCollider |
| `new Vector3(0f, 0f, -0.42f)` | 1571 | Thực hiện logic vector3 trong script Monster30BossPrefabBuilder. | - |
| `private static Transform FindOrCreateDirectChild(Transform parent, string childName, int layer)` | 1583 | Tìm or create direct child trong scene/danh sách dữ liệu. | - |
| `new GameObject(childName)` | 1594 | Thực hiện logic game object trong script Monster30BossPrefabBuilder. | - |
| `private static UnityEngine.Object GetSerializedReference(UnityEngine.Object target, string propertyName)` | 1603 | Lấy dữ liệu serialized reference cho hệ thống khác sử dụng. | - |
| `new SerializedObject(target)` | 1605 | Thực hiện logic serialized object trong script Monster30BossPrefabBuilder. | - |
| `private static Avatar LoadMonsterAvatar()` | 1609 | Nạp dữ liệu hoặc scene liên quan tới monster avatar. | - |
| `private static Transform FindBone(Animator animator, HumanBodyBones humanoidBone, params string[] fallbackNames)` | 1627 | Tìm bone trong scene/danh sách dữ liệu. | - |
| `private static Transform FindTransformByName(Transform root, string transformName)` | 1652 | Tìm transform by name trong scene/danh sách dữ liệu. | - |
| `private static void SetIntPropertyIfPresent(SerializedObject serializedObject, string propertyPath, int value)` | 1665 | Thiết lập giá trị hoặc trạng thái int property if present. | - |
| `private static void SetBoolPropertyIfPresent(SerializedObject serializedObject, string propertyPath, bool value)` | 1674 | Thiết lập giá trị hoặc trạng thái bool property if present. | - |
| `public Monster30AIAssetSet(AttackState attackState, CombatStanceState combatStanceState)` | 1715 | Thực hiện logic monster30 aiasset set trong script Monster30BossPrefabBuilder. Liên kết trực tiếp: AttackState, CombatStanceState. | AttackState, CombatStanceState |
| `public BoneCapsuleDefinition(HumanBodyBones bone, float radius, float height, int direction, Vector3 center, params string[] fallbackNames)` | 1734 | Thực hiện logic bone capsule definition trong script Monster30BossPrefabBuilder. | - |

#### Monster33BossPrefabBuilder

- **Đường dẫn:** `Assets/Game/Scripts/Editor/Monster33BossPrefabBuilder.cs`
- **Loại:** Editor tool
- **Vai trò dễ hiểu:** Công cụ chạy trong Unity Editor để dựng, sửa, kiểm tra hoặc tự động cấu hình scene/prefab/asset.
- **Kế thừa/cha:** -
- **Script con:** -
- **Field public/serialized chính:** public readonly HumanBodyBones Bone, public readonly float Radius, public readonly float Height, public readonly int Direction, public readonly Vector3 Center, public readonly string[] FallbackNames
- **Liên kết script:** AIBossCharacterManager, AICharacterAttackAction, AICharacterCombatManager, AICharacterManager, AICharacterNetworkManager, AICharacterSoundFXManager, AIMonster33BossCharacterNetworkManager, AIMonster33CharacterManager, AIMonster33CombatManager, AttackDamageProfile, AttackState, AttackType, BoneCapsuleDefinition, CharacterGroup, CharacterSoundFXManager, CharacterStatsManager +11

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public static void CreateMonster33Boss()` | 60 | Tạo object/dữ liệu monster33 boss. Liên kết trực tiếp: AIBossCharacterManager. | AIBossCharacterManager |
| `public static void MakeMonster33BossPrefabEditable()` | 181 | Thực hiện logic make monster33 boss prefab editable trong script Monster33BossPrefabBuilder. | - |
| `private static void ReplaceCharacterManager(GameObject prefabRoot, AIBossCharacterManager bossReferenceManager)` | 205 | Thực hiện logic replace character manager trong script Monster33BossPrefabBuilder. Liên kết trực tiếp: AICharacterManager, AIMonster33CharacterManager, AttackState, CharacterGroup, CombatStanceState +3. | AICharacterManager, AIMonster33CharacterManager, AttackState, CharacterGroup, CombatStanceState, IdleState, InvestigateSoundState, PursueTargetState |
| `new SerializedObject(bossManager)` | 224 | Thực hiện logic serialized object trong script Monster33BossPrefabBuilder. | - |
| `private static void ReplaceCombatManager(GameObject prefabRoot)` | 246 | Thực hiện logic replace combat manager trong script Monster33BossPrefabBuilder. Liên kết trực tiếp: AICharacterCombatManager, AIMonster33CombatManager. | AICharacterCombatManager, AIMonster33CombatManager |
| `private static void ReplaceNetworkManager(GameObject prefabRoot)` | 260 | Thực hiện logic replace network manager trong script Monster33BossPrefabBuilder. Liên kết trực tiếp: AICharacterNetworkManager, AIMonster33BossCharacterNetworkManager. | AICharacterNetworkManager, AIMonster33BossCharacterNetworkManager |
| `private static void ReplaceSoundFXManager(GameObject prefabRoot, GameObject bossReference)` | 274 | Thực hiện logic replace sound fxmanager trong script Monster33BossPrefabBuilder. Liên kết trực tiếp: AICharacterSoundFXManager, CharacterSoundFXManager. | AICharacterSoundFXManager, CharacterSoundFXManager |
| `new SerializedObject(referenceSoundFX)` | 290 | Thực hiện logic serialized object trong script Monster33BossPrefabBuilder. | - |
| `new SerializedObject(Monster33SoundFX)` | 291 | Thực hiện logic serialized object trong script Monster33BossPrefabBuilder. | - |
| `private static void PrepareBossStats(GameObject prefabRoot)` | 302 | Thực hiện logic prepare boss stats trong script Monster33BossPrefabBuilder. Liên kết trực tiếp: AICharacterNetworkManager, AIMonster33CombatManager, CharacterStatsManager. | AICharacterNetworkManager, AIMonster33CombatManager, CharacterStatsManager |
| `new SerializedObject(networkManager)` | 307 | Thực hiện logic serialized object trong script Monster33BossPrefabBuilder. | - |
| `new SerializedObject(combatManager)` | 327 | Thực hiện logic serialized object trong script Monster33BossPrefabBuilder. | - |
| `new SerializedObject(statsManager)` | 341 | Thực hiện logic serialized object trong script Monster33BossPrefabBuilder. | - |
| `private static void RemoveOldVisualChildren(Transform root)` | 349 | Loại bỏ old visual children khỏi danh sách, trạng thái hoặc dữ liệu. | - |
| `private static void ConfigureVisualHierarchy(Transform visualRoot)` | 374 | Thực hiện logic configure visual hierarchy trong script Monster33BossPrefabBuilder. | - |
| `private static void EnsureMonster33Materials(GameObject visualRoot)` | 382 | Thực hiện logic ensure monster33 materials trong script Monster33BossPrefabBuilder. | - |
| `private static void RebuildGameplayHooks(GameObject prefabRoot, Transform visualRoot, Animator sourceAnimator)` | 427 | Thực hiện logic rebuild gameplay hooks trong script Monster33BossPrefabBuilder. | - |
| `private static void EnsurePhase2FireHooks(GameObject prefabRoot, Transform visualRoot, Animator sourceAnimator)` | 436 | Thực hiện logic ensure phase2 fire hooks trong script Monster33BossPrefabBuilder. Liên kết trực tiếp: Monster33Phase2FireController. | Monster33Phase2FireController |
| `private static GameObject CreateEmbeddedMonster33Visual(Scene targetScene)` | 460 | Tạo object/dữ liệu embedded monster33 visual. | - |
| `private static void UnpackVisualHierarchyForEditing(GameObject visualRoot)` | 478 | Thực hiện logic unpack visual hierarchy for editing trong script Monster33BossPrefabBuilder. | - |
| `private static void AssignMonster33Animator(Animator rootAnimator)` | 502 | Thực hiện logic assign monster33 animator trong script Monster33BossPrefabBuilder. | - |
| `private static AnimatorController CreateOrUpdateMonster33AnimatorController()` | 514 | Tạo object/dữ liệu or update monster33 animator controller. | - |
| `private static AnimationClip CreateOrUpdateRebasedAnimationClip(string sourceClipPath, string rebasedClipPath)` | 607 | Tạo object/dữ liệu or update rebased animation clip. | - |
| `private static AnimationClip LoadPrimaryAnimationClip(string assetPath)` | 637 | Nạp dữ liệu hoặc scene liên quan tới primary animation clip. | - |
| `private static AnimationClip CreateOrUpdateMappedAttackClip( string sourceAssetPath, string targetClipPath, string setDamageFunctionName, float openNormalizedTime, float closeNormalizedTime, bool enablesCombo)` | 644 | Tạo object/dữ liệu or update mapped attack clip. | - |
| `string> BuildHumanoidBonePathMapWithFallback( string primaryAssetPath, string rootPrefix, params string[] candidateAssetPaths)` | 742 | Thực hiện logic build humanoid bone path map with fallback trong script Monster33BossPrefabBuilder. | - |
| `string> BuildHumanoidBonePathMap(string assetPath, string rootPrefix)` | 760 | Thực hiện logic build humanoid bone path map trong script Monster33BossPrefabBuilder. | - |
| `private static GameObject InstantiateHumanoidAsset(GameObject asset)` | 815 | Thực hiện logic instantiate humanoid asset trong script Monster33BossPrefabBuilder. | - |
| `private static string GetBonePathRootPrefix(string assetPath, string requestedRootPrefix)` | 837 | Lấy dữ liệu bone path root prefix cho hệ thống khác sử dụng. | - |
| `private static bool TryMapHumanoidBindingPath( string sourcePath, Dictionary<HumanBodyBones, string> sourceBonePaths, Dictionary<HumanBodyBones, string> targetBonePaths, out string mappedPath)` | 852 | Thử thực hiện map humanoid binding path, thường có kiểm tra điều kiện trước khi chạy. | - |
| `private static bool TryMapBindingPath( string sourcePath, Dictionary<HumanBodyBones, string> sourceBonePaths, Dictionary<HumanBodyBones, string> targetBonePaths, out string mappedPath)` | 883 | Thử thực hiện map binding path, thường có kiểm tra điều kiện trước khi chạy. | - |
| `return TryMapMonster33BindingPath(sourcePath, out mappedPath)` | 894 | Thử thực hiện map monster33 binding path, thường có kiểm tra điều kiện trước khi chạy. | - |
| `private static bool TryMapMonster33BindingPath(string sourcePath, out string mappedPath)` | 897 | Thử thực hiện map monster33 binding path, thường có kiểm tra điều kiện trước khi chạy. | - |
| `in GetMonster33SourceToTargetPathMap().OrderByDescending(entry => entry.Key.Length))` | 899 | Lấy dữ liệu monster33 source to target path map cho hệ thống khác sử dụng. | - |
| `string> GetMonster33SourceToTargetPathMap()` | 921 | Lấy dữ liệu monster33 source to target path map cho hệ thống khác sử dụng. | - |
| `private static void ClearClipCurves(AnimationClip clip)` | 969 | Thực hiện logic clear clip curves trong script Monster33BossPrefabBuilder. | - |
| `private static void ConfigureLoopingForOneShotClip(AnimationClip clip)` | 984 | Thực hiện logic configure looping for one shot clip trong script Monster33BossPrefabBuilder. | - |
| `new SerializedObject(clip)` | 991 | Thực hiện logic serialized object trong script Monster33BossPrefabBuilder. | - |
| `private static void CopyObjectArrayProperty(SerializedProperty source, SerializedProperty target)` | 1006 | Thực hiện logic copy object array property trong script Monster33BossPrefabBuilder. | - |
| `private static AnimationEvent[] BuildAttackEvents( float clipLength, string setDamageFunctionName, float openNormalizedTime, float closeNormalizedTime, bool enablesCombo)` | 1020 | Thực hiện logic build attack events trong script Monster33BossPrefabBuilder. | - |
| `private static AnimationEvent CreateAnimationEvent(float time, string functionName)` | 1052 | Tạo object/dữ liệu animation event. | - |
| `private static void ConfigureLoopingForLocomotionClip(AnimationClip clip, string clipPath)` | 1061 | Thực hiện logic configure looping for locomotion clip trong script Monster33BossPrefabBuilder. | - |
| `new SerializedObject(clip)` | 1076 | Thực hiện logic serialized object trong script Monster33BossPrefabBuilder. | - |
| `private static void RebaseAnimationClipPaths(AnimationClip clip, string prefix)` | 1092 | Thực hiện logic rebase animation clip paths trong script Monster33BossPrefabBuilder. | - |
| `private static Monster33AIAssetSet CreateOrUpdateMonster33AIAssets()` | 1115 | Tạo object/dữ liệu or update monster33 aiassets. Liên kết trực tiếp: AICharacterAttackAction, AttackState, AttackType, CombatStanceState, Monster33AIAssetSet. | AICharacterAttackAction, AttackState, AttackType, CombatStanceState, Monster33AIAssetSet |
| `new InvalidOperationException("Monster33 boss builder could not create the Monster33 AI assets.")` | 1127 | Thực hiện logic invalid operation exception trong script Monster33BossPrefabBuilder. | - |
| `new Monster33AIAssetSet(attackState, combatStanceState, phase02CombatStanceState)` | 1180 | Thực hiện logic monster33 aiasset set trong script Monster33BossPrefabBuilder. | - |
| `private static void ConfigureAttackAction( AICharacterAttackAction attackAction, string assetName, string animationName, bool isParryable, AttackType attackType, AICharacterAttackAction comboAction, int attackWeight, float minimumDistance, float maximumDistance)` | 1202 | Thực hiện logic configure attack action trong script Monster33BossPrefabBuilder. Liên kết trực tiếp: AttackType. | AttackType |
| `new SerializedObject(attackAction)` | 1215 | Thực hiện logic serialized object trong script Monster33BossPrefabBuilder. | - |
| `private static void ConfigureCombatStance( CombatStanceState combatStanceState, AICharacterAttackAction attack01, AICharacterAttackAction attack02, AICharacterAttackAction attack03, bool isPhase02, int comboChance, float maximumEngagementDistance)` | 1230 | Thực hiện logic configure combat stance trong script Monster33BossPrefabBuilder. | - |
| `new SerializedObject(combatStanceState)` | 1243 | Thực hiện logic serialized object trong script Monster33BossPrefabBuilder. | - |
| `private static void SetObjectArrayProperty(SerializedProperty arrayProperty, params UnityEngine.Object[] objects)` | 1258 | Thiết lập giá trị hoặc trạng thái object array property. | - |
| `private static void ApplyStateMotion(AnimatorController controller, string stateName, Motion motion)` | 1272 | Áp dụng state motion lên character/object mục tiêu. | - |
| `private static void EnsureComboState(AnimatorController controller, AnimationClip comboClip)` | 1284 | Thực hiện logic ensure combo state trong script Monster33BossPrefabBuilder. | - |
| `new Vector3(870f, -190f, 0f))` | 1294 | Thực hiện logic vector3 trong script Monster33BossPrefabBuilder. | - |
| `private static AnimatorState FindState(AnimatorController controller, string stateName)` | 1324 | Tìm state trong scene/danh sách dữ liệu. | - |
| `private static AnimatorStateMachine FindStateMachine(AnimatorController controller, string stateMachineName)` | 1338 | Tìm state machine trong scene/danh sách dữ liệu. | - |
| `private static AnimatorState FindStateRecursive(AnimatorStateMachine stateMachine, string stateName)` | 1352 | Tìm state recursive trong scene/danh sách dữ liệu. | - |
| `private static AnimatorStateMachine FindStateMachineRecursive(AnimatorStateMachine stateMachine, string stateMachineName)` | 1374 | Tìm state machine recursive trong scene/danh sách dữ liệu. | - |
| `private static AnimatorState FindStateInStateMachine(AnimatorStateMachine stateMachine, string stateName)` | 1393 | Tìm state in state machine trong scene/danh sách dữ liệu. | - |
| `private static void RemoveStateIfExists(AnimatorStateMachine stateMachine, string stateName)` | 1411 | Loại bỏ state if exists khỏi danh sách, trạng thái hoặc dữ liệu. | - |
| `private static void DeleteLegacyAnimatorController()` | 1429 | Thực hiện logic delete legacy animator controller trong script Monster33BossPrefabBuilder. | - |
| `private static void EnsureFolderExistsForAsset(string assetPath)` | 1440 | Thực hiện logic ensure folder exists for asset trong script Monster33BossPrefabBuilder. | - |
| `private static void EnsureLockOnTarget(Animator animator)` | 1463 | Thực hiện logic ensure lock on target trong script Monster33BossPrefabBuilder. Liên kết trực tiếp: LockOnTransform. | LockOnTransform |
| `private static void EnsureMainHurtbox(Animator animator)` | 1481 | Thực hiện logic ensure main hurtbox trong script Monster33BossPrefabBuilder. | - |
| `new Vector3(0f, 0.55f, 0f)` | 1495 | Thực hiện logic vector3 trong script Monster33BossPrefabBuilder. | - |
| `private static void EnsureWeaponDamageColliders(GameObject prefabRoot, Animator animator)` | 1498 | Thực hiện logic ensure weapon damage colliders trong script Monster33BossPrefabBuilder. Liên kết trực tiếp: AIMonster33CombatManager. | AIMonster33CombatManager |
| `new SerializedObject(combatManager)` | 1518 | Thực hiện logic serialized object trong script Monster33BossPrefabBuilder. | - |
| `private static void EnsureWeaponConstraints(Animator animator)` | 1525 | Thực hiện logic ensure weapon constraints trong script Monster33BossPrefabBuilder. | - |
| `private static void ConfigureWeaponConstraint(Transform weaponRoot, Transform hand)` | 1542 | Thực hiện logic configure weapon constraint trong script Monster33BossPrefabBuilder. | - |
| `private static void EnsureWeaponConstraintBootstrap(GameObject visualRoot)` | 1577 | Thực hiện logic ensure weapon constraint bootstrap trong script Monster33BossPrefabBuilder. Liên kết trực tiếp: Monster33WeaponConstraintBootstrap. | Monster33WeaponConstraintBootstrap |
| `private static void EnsureBodyColliders(Animator animator)` | 1600 | Thực hiện logic ensure body colliders trong script Monster33BossPrefabBuilder. Liên kết trực tiếp: BoneCapsuleDefinition. | BoneCapsuleDefinition |
| `private static ManualDamageCollider EnsureManualDamageCollider(Transform handBone, string hitboxName)` | 1638 | Thực hiện logic ensure manual damage collider trong script Monster33BossPrefabBuilder. Liên kết trực tiếp: ManualDamageCollider. | ManualDamageCollider |
| `private static ManualDamageCollider EnsureWeaponDamageCollider(Transform weaponBone, string hitboxName)` | 1658 | Thực hiện logic ensure weapon damage collider trong script Monster33BossPrefabBuilder. Liên kết trực tiếp: ManualDamageCollider, Monster33FireDamageCollider. | ManualDamageCollider, Monster33FireDamageCollider |
| `new Vector3(0f, 0f, -0.42f)` | 1671 | Thực hiện logic vector3 trong script Monster33BossPrefabBuilder. | - |
| `private static Transform FindOrCreateDirectChild(Transform parent, string childName, int layer)` | 1690 | Tìm or create direct child trong scene/danh sách dữ liệu. | - |
| `new GameObject(childName)` | 1701 | Thực hiện logic game object trong script Monster33BossPrefabBuilder. | - |
| `private static UnityEngine.Object GetSerializedReference(UnityEngine.Object target, string propertyName)` | 1710 | Lấy dữ liệu serialized reference cho hệ thống khác sử dụng. | - |
| `new SerializedObject(target)` | 1712 | Thực hiện logic serialized object trong script Monster33BossPrefabBuilder. | - |
| `private static Avatar LoadMonsterAvatar()` | 1716 | Nạp dữ liệu hoặc scene liên quan tới monster avatar. | - |
| `private static Transform FindBone(Animator animator, HumanBodyBones humanoidBone, params string[] fallbackNames)` | 1734 | Tìm bone trong scene/danh sách dữ liệu. | - |
| `private static Transform FindTransformByName(Transform root, string transformName)` | 1759 | Tìm transform by name trong scene/danh sách dữ liệu. | - |
| `private static void SetIntPropertyIfPresent(SerializedObject serializedObject, string propertyPath, int value)` | 1772 | Thiết lập giá trị hoặc trạng thái int property if present. | - |
| `private static void SetBoolPropertyIfPresent(SerializedObject serializedObject, string propertyPath, bool value)` | 1781 | Thiết lập giá trị hoặc trạng thái bool property if present. | - |
| `public Monster33AIAssetSet(AttackState attackState, CombatStanceState combatStanceState, CombatStanceState phase02CombatStanceState)` | 1822 | Thực hiện logic monster33 aiasset set trong script Monster33BossPrefabBuilder. Liên kết trực tiếp: AttackState, CombatStanceState. | AttackState, CombatStanceState |
| `public BoneCapsuleDefinition(HumanBodyBones bone, float radius, float height, int direction, Vector3 center, params string[] fallbackNames)` | 1843 | Thực hiện logic bone capsule definition trong script Monster33BossPrefabBuilder. | - |

#### PolygonBossZombiesUrpFixer

- **Đường dẫn:** `Assets/Game/Scripts/Editor/PolygonBossZombiesUrpFixer.cs`
- **Loại:** Editor tool
- **Vai trò dễ hiểu:** Công cụ chạy trong Unity Editor để dựng, sửa, kiểm tra hoặc tự động cấu hình scene/prefab/asset.
- **Kế thừa/cha:** -
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** GameAssetPaths

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public static void ConvertMaterialsToUrp()` | 10 | Thực hiện logic convert materials to urp trong script PolygonBossZombiesUrpFixer. | - |
| `private static Texture GetFirstTexture(Material material, params string[] propertyNames)` | 69 | Lấy dữ liệu first texture cho hệ thống khác sử dụng. | - |
| `private static Color GetFirstColor(Material material, params string[] propertyNames)` | 86 | Lấy dữ liệu first color cho hệ thống khác sử dụng. | - |
| `private static float GetFirstFloat(Material material, params string[] propertyNames)` | 99 | Lấy dữ liệu first float cho hệ thống khác sử dụng. | - |

#### PolygonZombiesUndeadBatchCreator

- **Đường dẫn:** `Assets/Game/Scripts/Editor/PolygonZombiesUndeadBatchCreator.cs`
- **Loại:** Editor tool
- **Vai trò dễ hiểu:** Công cụ chạy trong Unity Editor để dựng, sửa, kiểm tra hoặc tự động cấu hình scene/prefab/asset.
- **Kế thừa/cha:** -
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** AIUndeadCombatManager, CapsuleDefinition, CharacterUIManager, GameAssetPaths, LockOnTransform, ManualDamageCollider, UI_Character_HP_Bar

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `static PolygonZombiesUndeadBatchCreator()` | 18 | Thực hiện logic polygon zombies undead batch creator trong script PolygonZombiesUndeadBatchCreator. | - |
| `public static void CreateUndeadDummiesFromPolygonZombies()` | 24 | Tạo object/dữ liệu undead dummies from polygon zombies. | - |
| `private static void TryAutoCreateOnce()` | 65 | Thử thực hiện auto create once, thường có kiểm tra điều kiện trước khi chạy. | - |
| `private static void EnsureTargetPrefabExists(string targetPrefabPath)` | 78 | Thực hiện logic ensure target prefab exists trong script PolygonZombiesUndeadBatchCreator. | - |
| `private static void UpdateTargetPrefab(string targetPrefabPath, string targetPrefabName, string sourcePrefabPath)` | 91 | Cập nhật target prefab theo trạng thái mới. | - |
| `private static void RemoveExistingVisualRoots(Transform root)` | 134 | Loại bỏ existing visual roots khỏi danh sách, trạng thái hoặc dữ liệu. | - |
| `private static bool ShouldPreserveTopLevelChild(Transform child)` | 154 | Thực hiện logic should preserve top level child trong script PolygonZombiesUndeadBatchCreator. Liên kết trực tiếp: UI_Character_HP_Bar. | UI_Character_HP_Bar |
| `private static void SyncAnimatorAvatar(GameObject prefabRoot, GameObject visualRoot)` | 161 | Thực hiện logic sync animator avatar trong script PolygonZombiesUndeadBatchCreator. | - |
| `private static void RepairGameplayHooks(GameObject prefabRoot, Transform visualRoot)` | 177 | Thực hiện logic repair gameplay hooks trong script PolygonZombiesUndeadBatchCreator. | - |
| `private static void EnsureCharacterUiReferences(GameObject prefabRoot)` | 185 | Thực hiện logic ensure character ui references trong script PolygonZombiesUndeadBatchCreator. Liên kết trực tiếp: CharacterUIManager, UI_Character_HP_Bar. | CharacterUIManager, UI_Character_HP_Bar |
| `new SerializedObject(uiManager)` | 200 | Thực hiện logic serialized object trong script PolygonZombiesUndeadBatchCreator. | - |
| `private static void EnsureLockOnTarget(Transform visualRoot)` | 206 | Thực hiện logic ensure lock on target trong script PolygonZombiesUndeadBatchCreator. Liên kết trực tiếp: LockOnTransform. | LockOnTransform |
| `new GameObject("Lock on Target")` | 216 | Thực hiện logic game object trong script PolygonZombiesUndeadBatchCreator. | - |
| `private static void EnsureMainHurtbox(Transform visualRoot)` | 225 | Thực hiện logic ensure main hurtbox trong script PolygonZombiesUndeadBatchCreator. | - |
| `new GameObject("Undead_Main_Hurtbox")` | 232 | Thực hiện logic game object trong script PolygonZombiesUndeadBatchCreator. | - |
| `new Vector3(0f, 0.45f, 0f)` | 255 | Thực hiện logic vector3 trong script PolygonZombiesUndeadBatchCreator. | - |
| `private static void EnsureBodyColliders(Transform visualRoot)` | 258 | Thực hiện logic ensure body colliders trong script PolygonZombiesUndeadBatchCreator. Liên kết trực tiếp: CapsuleDefinition. | CapsuleDefinition |
| `private static void EnsureHandDamageColliders(GameObject prefabRoot, Transform visualRoot)` | 304 | Thực hiện logic ensure hand damage colliders trong script PolygonZombiesUndeadBatchCreator. Liên kết trực tiếp: AIUndeadCombatManager. | AIUndeadCombatManager |
| `new SerializedObject(combatManager)` | 324 | Thực hiện logic serialized object trong script PolygonZombiesUndeadBatchCreator. | - |
| `private static ManualDamageCollider EnsureManualDamageCollider(Transform handBone, string hitboxName)` | 331 | Thực hiện logic ensure manual damage collider trong script PolygonZombiesUndeadBatchCreator. Liên kết trực tiếp: ManualDamageCollider. | ManualDamageCollider |
| `new GameObject(hitboxName)` | 336 | Thực hiện logic game object trong script PolygonZombiesUndeadBatchCreator. | - |
| `private static void SetDamageableLayerRecursively(Transform root)` | 368 | Thiết lập giá trị hoặc trạng thái damageable layer recursively. Liên kết trực tiếp: ManualDamageCollider. | ManualDamageCollider |
| `private static void SetLayerRecursively(Transform root, int layer)` | 376 | Thiết lập giá trị hoặc trạng thái layer recursively. | - |
| `private static Transform FindDeepChild(Transform parent, string name)` | 386 | Tìm deep child trong scene/danh sách dữ liệu. | - |
| `private static Transform FindDirectChild(Transform parent, string name)` | 399 | Tìm direct child trong scene/danh sách dữ liệu. | - |
| `public CapsuleDefinition(string boneName, float radius, float height, int direction, Vector3 center)` | 414 | Thực hiện logic capsule definition trong script PolygonZombiesUndeadBatchCreator. | - |

#### PolygonZombiesUrpFixer

- **Đường dẫn:** `Assets/Game/Scripts/Editor/PolygonZombiesUrpFixer.cs`
- **Loại:** Editor tool
- **Vai trò dễ hiểu:** Công cụ chạy trong Unity Editor để dựng, sửa, kiểm tra hoặc tự động cấu hình scene/prefab/asset.
- **Kế thừa/cha:** -
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** GameAssetPaths

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `static PolygonZombiesUrpFixer()` | 10 | Thực hiện logic polygon zombies urp fixer trong script PolygonZombiesUrpFixer. | - |
| `public static void ConvertMaterialsToUrp()` | 16 | Thực hiện logic convert materials to urp trong script PolygonZombiesUrpFixer. | - |
| `private static void TryAutoFixOnce()` | 75 | Thử thực hiện auto fix once, thường có kiểm tra điều kiện trước khi chạy. | - |
| `private static Texture GetFirstTexture(Material material, params string[] propertyNames)` | 88 | Lấy dữ liệu first texture cho hệ thống khác sử dụng. | - |
| `private static Color GetFirstColor(Material material, params string[] propertyNames)` | 107 | Lấy dữ liệu first color cho hệ thống khác sử dụng. | - |
| `private static float GetFirstFloat(Material material, params string[] propertyNames)` | 120 | Lấy dữ liệu first float cho hệ thống khác sử dụng. | - |

#### RandomMapGeneratorEditor

- **Đường dẫn:** `Assets/Game/Scripts/Editor/RandomMapGeneratorEditor.cs`
- **Loại:** Editor tool
- **Vai trò dễ hiểu:** Công cụ chạy trong Unity Editor để dựng, sửa, kiểm tra hoặc tự động cấu hình scene/prefab/asset.
- **Kế thừa/cha:** UnityEditor.Editor
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** EventTriggerLoadScene, GameAssetPaths, GeneratedZoneInfo, RandomMapGenerator, SiteOfGraceInteractable, WorldAdditiveSceneBootstrap, WorldLocationRendererManager, WorldLocationSceneSet, WorldSceneManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `new Color(0.2f, 0.75f, 0.35f)` | 31 | Thực hiện logic color trong script RandomMapGeneratorEditor. | - |
| `new Color(0.85f, 0.3f, 0.25f)` | 32 | Thực hiện logic color trong script RandomMapGeneratorEditor. | - |
| `new Color(0.25f, 0.55f, 0.85f)` | 33 | Thực hiện logic color trong script RandomMapGeneratorEditor. | - |
| `new Color(0.18f, 0.18f, 0.22f)` | 34 | Thực hiện logic color trong script RandomMapGeneratorEditor. | - |
| `new Color(0.22f, 0.22f, 0.28f)` | 35 | Thực hiện logic color trong script RandomMapGeneratorEditor. | - |
| `private void OnEnable()` | 44 | Đăng ký event/callback hoặc bật trạng thái khi component được enable. | - |
| `public override void OnInspectorGUI()` | 53 | Thực hiện logic on inspector gui trong script RandomMapGeneratorEditor. Liên kết trực tiếp: RandomMapGenerator, SiteOfGraceInteractable. | RandomMapGenerator, SiteOfGraceInteractable |
| `Cấu trúc (Structure)", new Color(0.3f, 0.3f, 0.4f))` | 69 | Thực hiện logic trúc trong script RandomMapGeneratorEditor. | - |
| `Cầu thang (tuỳ chọn)")` | 81 | Thực hiện logic thang trong script RandomMapGeneratorEditor. | - |
| `new Color(0.3f, 0.3f, 0.4f))` | 85 | Thực hiện logic color trong script RandomMapGeneratorEditor. | - |
| `Đồ vật (bàn, hòm, thùng, …)")` | 89 | Thực hiện logic vật trong script RandomMapGeneratorEditor. | - |
| `Ánh sáng (Lights & Effects)", new Color(0.3f, 0.3f, 0.4f))` | 95 | Thực hiện logic sáng trong script RandomMapGeneratorEditor. | - |
| `new Color(0.3f, 0.3f, 0.4f))` | 105 | Thực hiện logic color trong script RandomMapGeneratorEditor. | - |
| `Spawner elite (phòng giữa)")` | 110 | Thực hiện logic elite trong script RandomMapGeneratorEditor. | - |
| `new GUIContent("Boss Prefab", "Boss đặt ở phòng cuối"))` | 111 | Thực hiện logic guicontent trong script RandomMapGeneratorEditor. | - |
| `new GUIContent("Site of Grace Prefab", "Checkpoint đặt ở phòng đầu"))` | 112 | Thực hiện logic guicontent trong script RandomMapGeneratorEditor. | - |
| `new GUIContent("Fog Wall Prefab", "Fog wall trước phòng boss"))` | 113 | Thực hiện logic guicontent trong script RandomMapGeneratorEditor. | - |
| `new GUIContent("Player Spawn Point Prefab", "Điểm spawn player (phòng đầu)"))` | 114 | Thực hiện logic guicontent trong script RandomMapGeneratorEditor. | - |
| `Tile Size (Tự động)", () =>` | 130 | Thực hiện logic size trong script RandomMapGeneratorEditor. | - |
| `new GUIContent("Wall Height (units)", "Chiều cao thực của prefab tường (phải khớp mesh). VD: tường cao 5m → nhập 5."))` | 141 | Thực hiện logic guicontent trong script RandomMapGeneratorEditor. | - |
| `new GUIContent("Wall Thickness (units)", "Độ dày prefab tường. VD: 0.5"))` | 143 | Thực hiện logic guicontent trong script RandomMapGeneratorEditor. | - |
| `Số ô (Tiles)", () =>` | 147 | Thực hiện logic ô trong script RandomMapGeneratorEditor. | - |
| `new GUIContent("Map Width (tiles)", "Số tile theo X"))` | 153 | Thực hiện logic guicontent trong script RandomMapGeneratorEditor. | - |
| `new GUIContent("Map Height (tiles)", "Số tile theo Z"))` | 154 | Thực hiện logic guicontent trong script RandomMapGeneratorEditor. | - |
| `new GUIContent("Min Room Size (tiles)"))` | 163 | Thực hiện logic guicontent trong script RandomMapGeneratorEditor. | - |
| `new GUIContent("Max Room Size (tiles)"))` | 164 | Thực hiện logic guicontent trong script RandomMapGeneratorEditor. | - |
| `new GUIContent("Max Rooms"))` | 165 | Thực hiện logic guicontent trong script RandomMapGeneratorEditor. | - |
| `chia Zone (Additive Scene)", () =>` | 168 | Thực hiện logic zone trong script RandomMapGeneratorEditor. | - |
| `new GUIContent("Zone Grid X", "Chia map thành N cột zone"))` | 172 | Thực hiện logic guicontent trong script RandomMapGeneratorEditor. | - |
| `new GUIContent("Zone Grid Z", "Chia map thành N hàng zone"))` | 173 | Thực hiện logic guicontent trong script RandomMapGeneratorEditor. | - |
| `new GUIContent("Prop Density", "Xác suất đặt prop mỗi tile bên trong phòng"))` | 190 | Thực hiện logic guicontent trong script RandomMapGeneratorEditor. | - |
| `new GUIContent("Decoration Density", "Xác suất đặt decoration tường"))` | 191 | Thực hiện logic guicontent trong script RandomMapGeneratorEditor. | - |
| `new GUIContent("Torch Density", "Xác suất đặt đuốc tường"))` | 192 | Thực hiện logic guicontent trong script RandomMapGeneratorEditor. | - |
| `new GUIContent("Torch Wall Spacing", "Place one torch every N wall tiles"))` | 197 | Thực hiện logic guicontent trong script RandomMapGeneratorEditor. | - |
| `new GUIContent("Torch Light Range", "Generated torch point light range"))` | 198 | Thực hiện logic guicontent trong script RandomMapGeneratorEditor. | - |
| `new GUIContent("Torch Light Intensity", "Generated torch point light intensity"))` | 199 | Thực hiện logic guicontent trong script RandomMapGeneratorEditor. | - |
| `new GUIContent("Torch Light Color", "Generated torch point light color"))` | 200 | Thực hiện logic guicontent trong script RandomMapGeneratorEditor. | - |
| `new GUIContent("Chandelier Light Range", "Generated chandelier point light range"))` | 201 | Thực hiện logic guicontent trong script RandomMapGeneratorEditor. | - |
| `new GUIContent("Chandelier Light Intensity", "Generated chandelier point light intensity"))` | 202 | Thực hiện logic guicontent trong script RandomMapGeneratorEditor. | - |
| `new GUIContent("Chandelier Light Color", "Generated chandelier point light color"))` | 203 | Thực hiện logic guicontent trong script RandomMapGeneratorEditor. | - |
| `new GUIContent("Use World_01 Lighting Mode", "Apply Game/System World Lighting Settings after generating"))` | 204 | Thực hiện logic guicontent trong script RandomMapGeneratorEditor. | - |
| `new GUIContent("Mark Generated Map For Bake", "Set generated renderers to Contribute GI and generated lights to Mixed"))` | 205 | Thực hiện logic guicontent trong script RandomMapGeneratorEditor. | - |
| `new GUIContent("Auto Bake NavMesh", "Bake NavMesh on generated Structure/Floors after generating"))` | 206 | Thực hiện logic guicontent trong script RandomMapGeneratorEditor. | - |
| `new GUIContent("Randomize Prefab Variants", "Bật để mỗi floor/wall/wall arch/ceiling chọn ngẫu nhiên prefab trong array. Tắt để luôn dùng prefab đầu tiên."))` | 212 | Thực hiện logic guicontent trong script RandomMapGeneratorEditor. | - |
| `new GUIContent("Use Random Seed"))` | 217 | Thực hiện logic guicontent trong script RandomMapGeneratorEditor. | - |
| `new GUIContent("Seed"))` | 220 | Thực hiện logic guicontent trong script RandomMapGeneratorEditor. | - |
| `new GUIContent("World Scene Name", "Tên scene thế giới (World_02, World_03…)"))` | 232 | Thực hiện logic guicontent trong script RandomMapGeneratorEditor. | - |
| `new GUIContent("Area Name", "Tên khu vực (Area_02, Area_03…)"))` | 233 | Thực hiện logic guicontent trong script RandomMapGeneratorEditor. | - |
| `new GUIContent("Generated Site Of Grace ID", "ID ghi vao SiteOfGraceInteractable khi generate"))` | 238 | Thực hiện logic guicontent trong script RandomMapGeneratorEditor. | - |
| `scene con (_Structure, _Props, _Effects, _Spawners)\n" + "3. Di chuyển object đúng loại vào đúng scene\n" + "4. Lưu tất cả scene\n" + "⚠️ Scene cần được thêm vào Build Settings thủ công (hoặc dùng script riêng)", MessageType.Info)` | 320 | Thực hiện logic con trong script RandomMapGeneratorEditor. | - |
| `new Color(0.25f, 0.25f, 0.32f))` | 351 | Thực hiện logic color trong script RandomMapGeneratorEditor. | - |
| `private void ExportZonesToScenes(RandomMapGenerator gen)` | 369 | Thực hiện logic export zones to scenes trong script RandomMapGeneratorEditor. Liên kết trực tiếp: GameAssetPaths, WorldSceneManager. | GameAssetPaths, WorldSceneManager |
| `root object(s) were exported. Generate the map again and export after fixing the hierarchy grouping.")` | 443 | Thực hiện logic object trong script RandomMapGeneratorEditor. | - |
| `private bool PrepareEditorForSubSceneExport()` | 475 | Thực hiện logic prepare editor for sub scene export trong script RandomMapGeneratorEditor. | - |
| `List<GameObject>>> BuildExportGroupsFromGeneratedHierarchy( RandomMapGenerator gen, Dictionary<string, string> categoryMap)` | 492 | Thực hiện logic build export groups from generated hierarchy trong script RandomMapGeneratorEditor. | - |
| `private List<GameObject> GetExportGroup(Dictionary<string, List<GameObject>> zoneGroups, string suffix)` | 538 | Lấy dữ liệu export group cho hệ thống khác sử dụng. | - |
| `private int MoveObjectsToSceneWithFolders(List<GameObject> objects, Scene scene)` | 546 | Thực hiện logic move objects to scene with folders trong script RandomMapGeneratorEditor. | - |
| `private Transform GetOrCreateExportFolder(Dictionary<string, Transform> folderRoots, Scene scene, string folderName)` | 576 | Lấy dữ liệu or create export folder cho hệ thống khác sử dụng. | - |
| `new GameObject(folderName)` | 584 | Thực hiện logic game object trong script RandomMapGeneratorEditor. | - |
| `private string GetExportFolderName(GameObject go)` | 591 | Lấy dữ liệu export folder name cho hệ thống khác sử dụng. | - |
| `private Transform GetGeneratedRoot(RandomMapGenerator gen)` | 630 | Lấy dữ liệu generated root cho hệ thống khác sử dụng. | - |
| `private void CollectChildren(Transform root, List<Transform> children)` | 644 | Thực hiện logic collect children trong script RandomMapGeneratorEditor. | - |
| `private string GetZoneNameForObject(RandomMapGenerator gen, GameObject go)` | 656 | Lấy dữ liệu zone name for object cho hệ thống khác sử dụng. Liên kết trực tiếp: GeneratedZoneInfo. | GeneratedZoneInfo |
| `private string GetZoneNameContainingPosition(RandomMapGenerator gen, Vector3 position)` | 737 | Lấy dữ liệu zone name containing position cho hệ thống khác sử dụng. Liên kết trực tiếp: GeneratedZoneInfo. | GeneratedZoneInfo |
| `private Bounds GetObjectBounds(GameObject go)` | 761 | Lấy dữ liệu object bounds cho hệ thống khác sử dụng. | - |
| `new Bounds(go.transform.position, Vector3.one)` | 766 | Thực hiện logic bounds trong script RandomMapGeneratorEditor. | - |
| `private Vector3 GetObjectCenter(GameObject go)` | 779 | Lấy dữ liệu object center cho hệ thống khác sử dụng. | - |
| `private void CleanupExportedMapFromMainScene(RandomMapGenerator gen)` | 797 | Thực hiện logic cleanup exported map from main scene trong script RandomMapGeneratorEditor. | - |
| `private void UpdateWorldAdditiveSceneBootstrap(RandomMapGenerator gen, List<string> sceneNames)` | 820 | Cập nhật world additive scene bootstrap theo trạng thái mới. Liên kết trực tiếp: WorldAdditiveSceneBootstrap. | WorldAdditiveSceneBootstrap |
| `private void DisableWorldAdditiveSceneBootstrap(RandomMapGenerator gen)` | 846 | Tắt world additive scene bootstrap. Liên kết trực tiếp: WorldAdditiveSceneBootstrap. | WorldAdditiveSceneBootstrap |
| `new SerializedObject(bootstrap)` | 856 | Thực hiện logic serialized object trong script RandomMapGeneratorEditor. | - |
| `private void AddScenesToBuildSettings(List<string> scenePaths)` | 872 | Thêm scenes to build settings vào danh sách, trạng thái hoặc dữ liệu. | - |
| `new EditorBuildSettingsScene(path, true)` | 890 | Thực hiện logic editor build settings scene trong script RandomMapGeneratorEditor. | - |
| `new EditorBuildSettingsScene(path, true))` | 894 | Thực hiện logic editor build settings scene trong script RandomMapGeneratorEditor. | - |
| `private void SetupWorldLocationSceneSetsAndTriggers(RandomMapGenerator gen)` | 900 | Thiết lập giá trị hoặc trạng thái up world location scene sets and triggers. Liên kết trực tiếp: WorldLocationSceneSet. | WorldLocationSceneSet |
| `WorldLocationSceneSet> CreateWorldLocationSceneSets(RandomMapGenerator gen)` | 910 | Tạo object/dữ liệu world location scene sets. Liên kết trực tiếp: GameAssetPaths, GeneratedZoneInfo, WorldLocationSceneSet. | GameAssetPaths, GeneratedZoneInfo, WorldLocationSceneSet |
| `private void AssignRequiredNeighborLocations(RandomMapGenerator gen, Dictionary<string, WorldLocationSceneSet> sceneSetsByZone)` | 951 | Thực hiện logic assign required neighbor locations trong script RandomMapGeneratorEditor. Liên kết trực tiếp: GeneratedZoneInfo, WorldLocationSceneSet. | GeneratedZoneInfo, WorldLocationSceneSet |
| `new SerializedObject(sceneSet)` | 989 | Thực hiện logic serialized object trong script RandomMapGeneratorEditor. | - |
| `private void CreateWorldLocationTriggers(RandomMapGenerator gen, Dictionary<string, WorldLocationSceneSet> sceneSetsByZone)` | 1005 | Tạo object/dữ liệu world location triggers. Liên kết trực tiếp: EventTriggerLoadScene, GeneratedZoneInfo, WorldLocationSceneSet. | EventTriggerLoadScene, GeneratedZoneInfo, WorldLocationSceneSet |
| `new SerializedObject(loadTrigger)` | 1035 | Thực hiện logic serialized object trong script RandomMapGeneratorEditor. | - |
| `private GameObject GetOrCreateSceneRoot(Scene scene, string rootName)` | 1045 | Lấy dữ liệu or create scene root cho hệ thống khác sử dụng. | - |
| `new GameObject(rootName)` | 1055 | Thực hiện logic game object trong script RandomMapGeneratorEditor. | - |
| `private void ClearAreaSceneTriggers(Transform triggerRoot, string areaName)` | 1060 | Thực hiện logic clear area scene triggers trong script RandomMapGeneratorEditor. | - |
| `private Bounds GetZoneTriggerBounds(GeneratedZoneInfo zone)` | 1080 | Lấy dữ liệu zone trigger bounds cho hệ thống khác sử dụng. | - |
| `new Vector3(SceneTriggerPaddingXZ, SceneTriggerPaddingY, SceneTriggerPaddingXZ))` | 1102 | Thực hiện logic vector3 trong script RandomMapGeneratorEditor. | - |
| `private bool TryCreateAdditiveExportScene(string sceneName, out Scene newScene)` | 1106 | Thử thực hiện create additive export scene, thường có kiểm tra điều kiện trước khi chạy. | - |
| `public static void NormalizeArea02SubScenes()` | 1128 | Thực hiện logic normalize area02 sub scenes trong script RandomMapGeneratorEditor. Liên kết trực tiếp: GameAssetPaths. | GameAssetPaths |
| `private static void NormalizeSubScenesInFolder(string folderPath)` | 1133 | Thực hiện logic normalize sub scenes in folder trong script RandomMapGeneratorEditor. | - |
| `private static string GetSceneCategorySuffix(string scenePath)` | 1167 | Lấy dữ liệu scene category suffix cho hệ thống khác sử dụng. | - |
| `private static void EnsureWorldLocationRenderer(Scene scene, string categorySuffix)` | 1183 | Thực hiện logic ensure world location renderer trong script RandomMapGeneratorEditor. Liên kết trực tiếp: WorldLocationRendererManager. | WorldLocationRendererManager |
| `new GameObject("World Location Renderer")` | 1217 | Thực hiện logic game object trong script RandomMapGeneratorEditor. | - |
| `private static bool ShouldCreateWorldLocationRenderer(string categorySuffix)` | 1241 | Thực hiện logic should create world location renderer trong script RandomMapGeneratorEditor. | - |
| `private string GetCategoryFromHierarchy(GameObject go)` | 1248 | Lấy dữ liệu category from hierarchy cho hệ thống khác sử dụng. | - |
| `private GameObject GetExportRootForSubScene(GameObject go)` | 1269 | Lấy dữ liệu export root for sub scene cho hệ thống khác sử dụng. | - |
| `private bool IsSceneVolumeNode(Transform t)` | 1295 | Kiểm tra điều kiện/trạng thái scene volume node. | - |
| `private bool IsGeneratedGroupingNode(string name)` | 1310 | Kiểm tra điều kiện/trạng thái generated grouping node. | - |
| `private void DrawBanner()` | 1332 | Thực hiện logic draw banner trong script RandomMapGeneratorEditor. | - |
| `new GUIStyle(EditorStyles.boldLabel)` | 1337 | Thực hiện logic guistyle trong script RandomMapGeneratorEditor. | - |
| `new GUIStyle(EditorStyles.miniLabel)` | 1344 | Thực hiện logic guistyle trong script RandomMapGeneratorEditor. | - |
| `new Rect(rect.x, rect.y + 6, rect.width, 24), "⚔ RANDOM MAP GENERATOR", titleStyle)` | 1350 | Thực hiện logic rect trong script RandomMapGeneratorEditor. | - |
| `new Rect(rect.x, rect.y + 28, rect.width, 18), "Tạo dungeon ngẫu nhiên – tự phân chia scene", subStyle)` | 1351 | Thực hiện logic rect trong script RandomMapGeneratorEditor. | - |
| `private bool DrawFoldout(bool state, string label, Color bgColor)` | 1354 | Thực hiện logic draw foldout trong script RandomMapGeneratorEditor. | - |
| `new GUIStyle(EditorStyles.foldout)` | 1358 | Thực hiện logic guistyle trong script RandomMapGeneratorEditor. | - |
| `new Rect(rect.x + 4, rect.y + 2, rect.width - 8, rect.height), state, label, true, style)` | 1364 | Thực hiện logic rect trong script RandomMapGeneratorEditor. | - |
| `private void DrawSectionHeader(string label)` | 1367 | Thực hiện logic draw section header trong script RandomMapGeneratorEditor. | - |
| `new GUIStyle(EditorStyles.boldLabel)` | 1371 | Thực hiện logic guistyle trong script RandomMapGeneratorEditor. | - |
| `private void DrawConfigSection(string label, System.Action drawContent)` | 1379 | Thực hiện logic draw config section trong script RandomMapGeneratorEditor. | - |
| `new GUIStyle(EditorStyles.boldLabel)` | 1382 | Thực hiện logic guistyle trong script RandomMapGeneratorEditor. | - |
| `private void DrawPrefabArray(SerializedProperty arrayProp, string label, string tooltip)` | 1394 | Thực hiện logic draw prefab array trong script RandomMapGeneratorEditor. | - |
| `new GUIContent(label, tooltip), true)` | 1396 | Thực hiện logic guicontent trong script RandomMapGeneratorEditor. | - |

#### ShopInventoryEditor

- **Đường dẫn:** `Assets/Game/Scripts/Editor/ShopInventoryEditor.cs`
- **Loại:** Editor tool
- **Vai trò dễ hiểu:** Công cụ chạy trong Unity Editor để dựng, sửa, kiểm tra hoặc tự động cấu hình scene/prefab/asset.
- **Kế thừa/cha:** UnityEditor.Editor
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** GameAssetPaths, GameProgressionManager, MerchantSetupChecklistGenerator, ShopInventory

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public override void OnInspectorGUI()` | 9 | Thực hiện logic on inspector gui trong script ShopInventoryEditor. Liên kết trực tiếp: GameAssetPaths, GameProgressionManager, MerchantSetupChecklistGenerator, ShopInventory. | GameAssetPaths, GameProgressionManager, MerchantSetupChecklistGenerator, ShopInventory |

#### Stylized3DMonsterUrpFixer

- **Đường dẫn:** `Assets/Game/Scripts/Editor/Stylized3DMonsterUrpFixer.cs`
- **Loại:** Editor tool
- **Vai trò dễ hiểu:** Công cụ chạy trong Unity Editor để dựng, sửa, kiểm tra hoặc tự động cấu hình scene/prefab/asset.
- **Kế thừa/cha:** -
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** GameAssetPaths

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `static Stylized3DMonsterUrpFixer()` | 10 | Thực hiện logic stylized3 dmonster urp fixer trong script Stylized3DMonsterUrpFixer. | - |
| `public static void ConvertMaterialsToUrp()` | 16 | Thực hiện logic convert materials to urp trong script Stylized3DMonsterUrpFixer. | - |
| `private static void TryAutoFixOnce()` | 76 | Thử thực hiện auto fix once, thường có kiểm tra điều kiện trước khi chạy. | - |
| `private static Texture GetFirstTexture(Material material, params string[] propertyNames)` | 89 | Lấy dữ liệu first texture cho hệ thống khác sử dụng. | - |
| `private static Color GetFirstColor(Material material, params string[] propertyNames)` | 108 | Lấy dữ liệu first color cho hệ thống khác sử dụng. | - |

#### TitleScreenLaunchModeSceneBuilder

- **Đường dẫn:** `Assets/Game/Scripts/Editor/TitleScreenLaunchModeSceneBuilder.cs`
- **Loại:** Editor tool
- **Vai trò dễ hiểu:** Công cụ chạy trong Unity Editor để dựng, sửa, kiểm tra hoặc tự động cấu hình scene/prefab/asset.
- **Kế thừa/cha:** -
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** -

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| - | - | Script này chủ yếu là dữ liệu/enum/struct, không có method rõ ràng. | - |

#### TitleScreenSettingsMenuSceneBuilder

- **Đường dẫn:** `Assets/Game/Scripts/Editor/TitleScreenSettingsMenuSceneBuilder.cs`
- **Loại:** Editor tool
- **Vai trò dễ hiểu:** Công cụ chạy trong Unity Editor để dựng, sửa, kiểm tra hoặc tự động cấu hình scene/prefab/asset.
- **Kế thừa/cha:** -
- **Script con:** -
- **Field public/serialized chính:** public readonly TextMeshProUGUI valueText, public readonly Button primaryButton, public readonly Button secondaryButton
- **Liên kết script:** GameAssetPaths, SelectionRow, TitleScreenManager, TitleScreenSettingsMenuView

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `static TitleScreenSettingsMenuSceneBuilder()` | 16 | Thực hiện logic title screen settings menu scene builder trong script TitleScreenSettingsMenuSceneBuilder. | - |
| `public static void RebuildViaMenu()` | 22 | Thực hiện logic rebuild via menu trong script TitleScreenSettingsMenuSceneBuilder. | - |
| `private static void EnsureSceneAuthoredSettingsMenuOnce()` | 27 | Thực hiện logic ensure scene authored settings menu once trong script TitleScreenSettingsMenuSceneBuilder. | - |
| `private static void BuildOrUpdateSceneMenu(bool forceRebuild)` | 36 | Thực hiện logic build or update scene menu trong script TitleScreenSettingsMenuSceneBuilder. Liên kết trực tiếp: TitleScreenManager, TitleScreenSettingsMenuView. | TitleScreenManager, TitleScreenSettingsMenuView |
| `new SerializedObject(manager)` | 54 | Thực hiện logic serialized object trong script TitleScreenSettingsMenuSceneBuilder. | - |
| `new SerializedObject(settingsView)` | 65 | Thực hiện logic serialized object trong script TitleScreenSettingsMenuSceneBuilder. | - |
| `private static void BuildAuthoredLayout(RectTransform contentRoot, SerializedObject viewSerializedObject)` | 97 | Thực hiện logic build authored layout trong script TitleScreenSettingsMenuSceneBuilder. Liên kết trực tiếp: SelectionRow. | SelectionRow |
| `new Vector2(120f, 70f))` | 114 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuSceneBuilder. | - |
| `private static void Assign(SerializedObject targetObject, string propertyName, Object value)` | 135 | Thực hiện logic assign trong script TitleScreenSettingsMenuSceneBuilder. | - |
| `private static void ClearChildren(RectTransform parent)` | 143 | Thực hiện logic clear children trong script TitleScreenSettingsMenuSceneBuilder. | - |
| `private static void CreatePanelBackground(RectTransform parent)` | 151 | Tạo object/dữ liệu panel background. | - |
| `new Vector2(0.5f, 0.5f)` | 157 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuSceneBuilder. | - |
| `new Vector2(0.5f, 0.5f)` | 158 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuSceneBuilder. | - |
| `new Vector2(0.5f, 0.5f)` | 159 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuSceneBuilder. | - |
| `new Vector2(120f, -20f)` | 160 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuSceneBuilder. | - |
| `new Vector2(1180f, 820f)` | 161 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuSceneBuilder. | - |
| `new Color(0f, 0f, 0f, 0.82f)` | 163 | Thực hiện logic color trong script TitleScreenSettingsMenuSceneBuilder. | - |
| `private static void CreateHeader(RectTransform parent)` | 167 | Tạo object/dữ liệu header. | - |
| `new Vector2(840f, 60f), new Vector2(120f, -55f), TextAlignmentOptions.Center, 40f)` | 169 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuSceneBuilder. | - |
| `new Vector2(0.5f, 1f)` | 171 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuSceneBuilder. | - |
| `new Vector2(0.5f, 1f)` | 172 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuSceneBuilder. | - |
| `new Vector2(0.5f, 1f)` | 173 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuSceneBuilder. | - |
| `private static Slider CreateSliderRow( RectTransform parent, string rowName, string label, float anchoredY, out TextMeshProUGUI valueText, float minValue = 0f, float maxValue = 1f)` | 176 | Tạo object/dữ liệu slider row. | - |
| `new Vector2(340f, 42f), new Vector2(-470f, -12f), TextAlignmentOptions.Left, 24f, new Vector2(0f, 1f))` | 186 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuSceneBuilder. | - |
| `new Vector2(450f, 30f), new Vector2(-60f, -8f), minValue, maxValue)` | 187 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuSceneBuilder. | - |
| `new Vector2(140f, 42f), new Vector2(415f, -12f), TextAlignmentOptions.Right, 22f, new Vector2(0f, 1f))` | 188 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuSceneBuilder. | - |
| `private static SelectionRow CreateSelectionRow(RectTransform parent, string rowName, string label, float anchoredY, bool singleButton)` | 192 | Tạo object/dữ liệu selection row. Liên kết trực tiếp: SelectionRow. | SelectionRow |
| `new Vector2(340f, 42f), new Vector2(-470f, -12f), TextAlignmentOptions.Left, 24f, new Vector2(0f, 1f))` | 195 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuSceneBuilder. | - |
| `new Vector2(160f, 42f), new Vector2(-40f, -12f), TextAlignmentOptions.Center, 22f, new Vector2(0f, 1f))` | 199 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuSceneBuilder. | - |
| `new Vector2(220f, 52f), new Vector2(210f, -4f), new Vector2(0f, 1f))` | 200 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuSceneBuilder. | - |
| `new SelectionRow(valueText, toggleButton, null)` | 201 | Thực hiện logic selection row trong script TitleScreenSettingsMenuSceneBuilder. | - |
| `new Vector2(200f, 42f), new Vector2(170f, -12f), TextAlignmentOptions.Center, 22f, new Vector2(0f, 1f))` | 204 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuSceneBuilder. | - |
| `new Vector2(82f, 52f), new Vector2(55f, -4f), new Vector2(0f, 1f))` | 205 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuSceneBuilder. | - |
| `new Vector2(82f, 52f), new Vector2(345f, -4f), new Vector2(0f, 1f))` | 206 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuSceneBuilder. | - |
| `new SelectionRow(centeredValueText, previousButton, nextButton)` | 207 | Thực hiện logic selection row trong script TitleScreenSettingsMenuSceneBuilder. | - |
| `private static Button CreateActionButton(RectTransform parent, string name, string label, Vector2 anchoredPosition)` | 210 | Tạo object/dữ liệu action button. | - |
| `return CreateButton(name, parent, label, new Vector2(340f, 62f), anchoredPosition, new Vector2(0.5f, 0f))` | 212 | Tạo object/dữ liệu button. | - |
| `private static RectTransform CreateRowRoot(RectTransform parent, string name, float anchoredY)` | 215 | Tạo object/dữ liệu row root. | - |
| `new Vector2(0.5f, 1f)` | 219 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuSceneBuilder. | - |
| `new Vector2(0.5f, 1f)` | 220 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuSceneBuilder. | - |
| `new Vector2(0.5f, 1f)` | 221 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuSceneBuilder. | - |
| `new Vector2(120f, anchoredY)` | 222 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuSceneBuilder. | - |
| `new Vector2(1060f, 78f)` | 223 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuSceneBuilder. | - |
| `private static Slider CreateSlider(string name, RectTransform parent, Vector2 size, Vector2 anchoredPosition, float minValue, float maxValue)` | 227 | Tạo object/dữ liệu slider. | - |
| `new Vector2(0f, 1f)` | 233 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuSceneBuilder. | - |
| `new Vector2(0f, 1f)` | 234 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuSceneBuilder. | - |
| `new Vector2(0f, 1f)` | 235 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuSceneBuilder. | - |
| `new Vector2(0f, 0.25f)` | 242 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuSceneBuilder. | - |
| `new Vector2(1f, 0.75f)` | 243 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuSceneBuilder. | - |
| `new Color(0.22f, 0.22f, 0.22f, 1f)` | 246 | Thực hiện logic color trong script TitleScreenSettingsMenuSceneBuilder. | - |
| `new Vector2(10f, 0f)` | 252 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuSceneBuilder. | - |
| `new Vector2(-10f, 0f)` | 253 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuSceneBuilder. | - |
| `new Vector2(10f, 0f)` | 268 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuSceneBuilder. | - |
| `new Vector2(-10f, 0f)` | 269 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuSceneBuilder. | - |
| `new Vector2(20f, 40f)` | 274 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuSceneBuilder. | - |
| `new Color(0.96f, 0.96f, 0.96f, 1f)` | 279 | Thực hiện logic color trong script TitleScreenSettingsMenuSceneBuilder. | - |
| `new Color(0.784f, 0.784f, 0.784f, 1f)` | 280 | Thực hiện logic color trong script TitleScreenSettingsMenuSceneBuilder. | - |
| `new Color(0.96f, 0.96f, 0.96f, 1f)` | 281 | Thực hiện logic color trong script TitleScreenSettingsMenuSceneBuilder. | - |
| `new Color(0.784f, 0.784f, 0.784f, 0.5f)` | 282 | Thực hiện logic color trong script TitleScreenSettingsMenuSceneBuilder. | - |
| `private static Button CreateButton(string name, RectTransform parent, string label, Vector2 size, Vector2 anchoredPosition, Vector2 anchor)` | 295 | Tạo object/dữ liệu button. | - |
| `new Color(0.22f, 0.22f, 0.22f, 1f)` | 308 | Thực hiện logic color trong script TitleScreenSettingsMenuSceneBuilder. | - |
| `new Color(0.21698111f, 0.21698111f, 0.21698111f, 1f)` | 311 | Thực hiện logic color trong script TitleScreenSettingsMenuSceneBuilder. | - |
| `new Color(0.9607843f, 0.9607843f, 0.9607843f, 1f)` | 312 | Thực hiện logic color trong script TitleScreenSettingsMenuSceneBuilder. | - |
| `new Color(0.9607843f, 0.9607843f, 0.9607843f, 1f)` | 314 | Thực hiện logic color trong script TitleScreenSettingsMenuSceneBuilder. | - |
| `new Color(0.78431374f, 0.78431374f, 0.78431374f, 0.5019608f)` | 315 | Thực hiện logic color trong script TitleScreenSettingsMenuSceneBuilder. | - |
| `new Vector2(10f, 5f)` | 323 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuSceneBuilder. | - |
| `new Vector2(-10f, -5f)` | 324 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuSceneBuilder. | - |
| `private static TextMeshProUGUI CreateText( string name, RectTransform parent, string value, Vector2 size, Vector2 anchoredPosition, TextAlignmentOptions alignment, float fontSize, Vector2? anchorOverride = null)` | 329 | Tạo object/dữ liệu text. | - |
| `new Vector2(0.5f, 0.5f)` | 343 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuSceneBuilder. | - |
| `new Color(1f, 0.8272578f, 0f, 1f)` | 354 | Thực hiện logic color trong script TitleScreenSettingsMenuSceneBuilder. | - |
| `private static GameObject CreateUIObject(string name, Transform parent)` | 361 | Tạo object/dữ liệu uiobject. | - |
| `new GameObject(name, typeof(RectTransform))` | 363 | Thực hiện logic game object trong script TitleScreenSettingsMenuSceneBuilder. | - |
| `public SelectionRow(TextMeshProUGUI valueText, Button primaryButton, Button secondaryButton)` | 371 | Thực hiện logic selection row trong script TitleScreenSettingsMenuSceneBuilder. | - |

#### UndeadDummyGameplayFixer

- **Đường dẫn:** `Assets/Game/Scripts/Editor/UndeadDummyGameplayFixer.cs`
- **Loại:** Editor tool
- **Vai trò dễ hiểu:** Công cụ chạy trong Unity Editor để dựng, sửa, kiểm tra hoặc tự động cấu hình scene/prefab/asset.
- **Kế thừa/cha:** -
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** AIUndeadCombatManager, CapsuleDefinition, GameAssetPaths, LockOnTransform, ManualDamageCollider

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `static UndeadDummyGameplayFixer()` | 15 | Thực hiện logic undead dummy gameplay fixer trong script UndeadDummyGameplayFixer. | - |
| `public static void FixUndeadDummyGameplayHooks()` | 21 | Thực hiện logic fix undead dummy gameplay hooks trong script UndeadDummyGameplayFixer. | - |
| `private static void TryAutoFixOnce()` | 48 | Thử thực hiện auto fix once, thường có kiểm tra điều kiện trước khi chạy. | - |
| `private static void EnsureLockOnTarget(Transform visualRoot)` | 59 | Thực hiện logic ensure lock on target trong script UndeadDummyGameplayFixer. Liên kết trực tiếp: LockOnTransform. | LockOnTransform |
| `new GameObject("Lock on Target")` | 69 | Thực hiện logic game object trong script UndeadDummyGameplayFixer. | - |
| `private static void EnsureHandDamageColliders(GameObject prefabRoot, Transform visualRoot)` | 78 | Thực hiện logic ensure hand damage colliders trong script UndeadDummyGameplayFixer. Liên kết trực tiếp: AIUndeadCombatManager. | AIUndeadCombatManager |
| `new SerializedObject(combatManager)` | 98 | Thực hiện logic serialized object trong script UndeadDummyGameplayFixer. | - |
| `private static void EnsureMainHurtbox(Transform visualRoot)` | 105 | Thực hiện logic ensure main hurtbox trong script UndeadDummyGameplayFixer. | - |
| `new GameObject("Undead_Main_Hurtbox")` | 112 | Thực hiện logic game object trong script UndeadDummyGameplayFixer. | - |
| `new Vector3(0f, 0.45f, 0f)` | 135 | Thực hiện logic vector3 trong script UndeadDummyGameplayFixer. | - |
| `private static ManualDamageCollider EnsureManualDamageCollider(Transform handBone, string hitboxName)` | 138 | Thực hiện logic ensure manual damage collider trong script UndeadDummyGameplayFixer. Liên kết trực tiếp: ManualDamageCollider. | ManualDamageCollider |
| `new GameObject(hitboxName)` | 143 | Thực hiện logic game object trong script UndeadDummyGameplayFixer. | - |
| `private static void EnsureBodyColliders(Transform visualRoot)` | 175 | Thực hiện logic ensure body colliders trong script UndeadDummyGameplayFixer. Liên kết trực tiếp: CapsuleDefinition. | CapsuleDefinition |
| `private static void SetDamageableLayerRecursively(Transform root)` | 220 | Thiết lập giá trị hoặc trạng thái damageable layer recursively. Liên kết trực tiếp: ManualDamageCollider. | ManualDamageCollider |
| `private static Transform FindDeepChild(Transform parent, string name)` | 234 | Tìm deep child trong scene/danh sách dữ liệu. | - |
| `private static Transform FindDirectChild(Transform parent, string name)` | 247 | Tìm direct child trong scene/danh sách dữ liệu. | - |
| `public CapsuleDefinition(string boneName, float radius, float height, int direction, Vector3 center)` | 262 | Thực hiện logic capsule definition trong script UndeadDummyGameplayFixer. | - |

#### UndeadDummyModelReplacer

- **Đường dẫn:** `Assets/Game/Scripts/Editor/UndeadDummyModelReplacer.cs`
- **Loại:** Editor tool
- **Vai trò dễ hiểu:** Công cụ chạy trong Unity Editor để dựng, sửa, kiểm tra hoặc tự động cấu hình scene/prefab/asset.
- **Kế thừa/cha:** -
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** GameAssetPaths, UndeadDummyGameplayFixer

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `static UndeadDummyModelReplacer()` | 12 | Thực hiện logic undead dummy model replacer trong script UndeadDummyModelReplacer. | - |
| `public static void ReplaceUndeadDummyModel()` | 18 | Thực hiện logic replace undead dummy model trong script UndeadDummyModelReplacer. Liên kết trực tiếp: UndeadDummyGameplayFixer. | UndeadDummyGameplayFixer |
| `private static void TryAutoReplaceOnce()` | 80 | Thử thực hiện auto replace once, thường có kiểm tra điều kiện trước khi chạy. | - |
| `private static bool HasReplacementAlready(Transform root, string replacementName)` | 91 | Thực hiện logic has replacement already trong script UndeadDummyModelReplacer. | - |
| `private static void DestroyImmediateChild(Transform root, string childName)` | 96 | Thực hiện logic destroy immediate child trong script UndeadDummyModelReplacer. | - |
| `private static void SetLayerRecursively(Transform root, int layer)` | 105 | Thiết lập giá trị hoặc trạng thái layer recursively. | - |

#### WeaponDamageColliderAlignmentUtility

- **Đường dẫn:** `Assets/Game/Scripts/Editor/WeaponDamageColliderAlignmentUtility.cs`
- **Loại:** Editor tool
- **Vai trò dễ hiểu:** Công cụ chạy trong Unity Editor để dựng, sửa, kiểm tra hoặc tự động cấu hình scene/prefab/asset.
- **Kế thừa/cha:** -
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** GameAssetPaths, MeleeWeaponDamageCollider, WeaponManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public static void AlignDamageCollidersToWeaponPivot()` | 13 | Thực hiện logic align damage colliders to weapon pivot trong script WeaponDamageColliderAlignmentUtility. Liên kết trực tiếp: MeleeWeaponDamageCollider, WeaponManager. | MeleeWeaponDamageCollider, WeaponManager |
| `private static bool ShouldSkip(string prefabPath)` | 67 | Thực hiện logic should skip trong script WeaponDamageColliderAlignmentUtility. | - |
| `private static Transform FindChildByName(Transform parent, string childName)` | 73 | Tìm child by name trong scene/danh sách dữ liệu. | - |

#### WeaponMaterialColorExporter

- **Đường dẫn:** `Assets/Game/Scripts/Editor/WeaponMaterialColorExporter.cs`
- **Loại:** Editor tool
- **Vai trò dễ hiểu:** Công cụ chạy trong Unity Editor để dựng, sửa, kiểm tra hoặc tự động cấu hình scene/prefab/asset.
- **Kế thừa/cha:** -
- **Script con:** -
- **Field public/serialized chính:** public List<WeaponMaterialRecord> records, public List<WeaponPromptRecord> records, public string assetPath, public string displayName, public string prefabName, public string category, public string role, public string rendererName, public string materialName, public string colorHex, public string colorName, public string colorNotes +3
- **Liên kết script:** ColorAnalysis, GameAssetPaths, MaterialPartRecord, WeaponCategory, WeaponItem, WeaponMaterialRecord, WeaponMaterialRecordCollection, WeaponPromptRecord, WeaponPromptRecordCollection

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public static void ExportWeaponMaterialColorsAndPrompts()` | 23 | Thực hiện logic export weapon material colors and prompts trong script WeaponMaterialColorExporter. Liên kết trực tiếp: MaterialPartRecord, WeaponCategory, WeaponItem, WeaponMaterialRecord, WeaponPromptRecord. | MaterialPartRecord, WeaponCategory, WeaponItem, WeaponMaterialRecord, WeaponPromptRecord |
| `private static List<WeaponItem> LoadAllWeaponItems()` | 75 | Nạp dữ liệu hoặc scene liên quan tới all weapon items. Liên kết trực tiếp: WeaponItem. | WeaponItem |
| `private static List<MaterialPartRecord> CollectMaterialParts(WeaponItem weapon)` | 93 | Thực hiện logic collect material parts trong script WeaponMaterialColorExporter. Liên kết trực tiếp: ColorAnalysis, MaterialPartRecord. | ColorAnalysis, MaterialPartRecord |
| `private static Mesh GetRendererMesh(Renderer renderer)` | 133 | Lấy dữ liệu renderer mesh cho hệ thống khác sử dụng. | - |
| `private static bool IsIgnorableRenderer(Renderer renderer)` | 142 | Kiểm tra điều kiện/trạng thái ignorable renderer. | - |
| `private static bool IsIgnorableMaterial(Material material)` | 150 | Kiểm tra điều kiện/trạng thái ignorable material. | - |
| `private static ColorAnalysis AnalyzeMaterialColor(Material material, Mesh mesh, int subMeshIndex)` | 157 | Thực hiện logic analyze material color trong script WeaponMaterialColorExporter. Liên kết trực tiếp: ColorAnalysis. | ColorAnalysis |
| `return BuildPaletteDescription(sampledColors)` | 164 | Thực hiện logic build palette description trong script WeaponMaterialColorExporter. | - |
| `private static Texture2D GetMainTexture(Material material)` | 175 | Lấy dữ liệu main texture cho hệ thống khác sử dụng. | - |
| `private static List<Color> SampleTextureColors(Texture2D sourceTexture, Mesh mesh, int subMeshIndex)` | 191 | Thực hiện logic sample texture colors trong script WeaponMaterialColorExporter. | - |
| `new Texture2D(64, 64, TextureFormat.RGBA32, false, true)` | 200 | Thực hiện logic texture2 d trong script WeaponMaterialColorExporter. | - |
| `new Rect(0, 0, 64, 64), 0, 0)` | 201 | Thực hiện logic rect trong script WeaponMaterialColorExporter. | - |
| `private static void SampleMeshUvColors(Texture2D readableTexture, Mesh mesh, int subMeshIndex, List<Color> sampledColors)` | 238 | Thực hiện logic sample mesh uv colors trong script WeaponMaterialColorExporter. | - |
| `private static void SampleUv(Texture2D texture, Vector2 uv, List<Color> sampledColors)` | 269 | Thực hiện logic sample uv trong script WeaponMaterialColorExporter. | - |
| `private static ColorAnalysis BuildPaletteDescription(List<Color> sampledColors)` | 287 | Thực hiện logic build palette description trong script WeaponMaterialColorExporter. Liên kết trực tiếp: ColorAnalysis. | ColorAnalysis |
| `private static Color ExtractRepresentativeFlatColor(Material material)` | 323 | Thực hiện logic extract representative flat color trong script WeaponMaterialColorExporter. | - |
| `private static string GuessRole(WeaponItem weapon, string rendererName, string materialName)` | 339 | Thực hiện logic guess role trong script WeaponMaterialColorExporter. Liên kết trực tiếp: WeaponCategory. | WeaponCategory |
| `private static string BuildColorNotes(WeaponCategory category, List<MaterialPartRecord> parts)` | 388 | Thực hiện logic build color notes trong script WeaponMaterialColorExporter. Liên kết trực tiếp: MaterialPartRecord. | MaterialPartRecord |
| `private static int GetRolePriority(WeaponCategory category, string role)` | 416 | Lấy dữ liệu role priority cho hệ thống khác sử dụng. Liên kết trực tiếp: WeaponCategory. | WeaponCategory |
| `private static string BuildPrompt(string displayName, WeaponCategory category, string colorNotes)` | 458 | Thực hiện logic build prompt trong script WeaponMaterialColorExporter. | - |
| `private static WeaponCategory ClassifyWeapon(WeaponItem weapon)` | 471 | Thực hiện logic classify weapon trong script WeaponMaterialColorExporter. Liên kết trực tiếp: WeaponCategory. | WeaponCategory |
| `private static string DescribeColor(Color color)` | 497 | Thực hiện logic describe color trong script WeaponMaterialColorExporter. | - |
| `private static string GetSafeObjectName(UnityEngine.Object unityObject)` | 536 | Lấy dữ liệu safe object name cho hệ thống khác sử dụng. | - |
| `private static void WriteMaterialManifestCsv(List<WeaponMaterialRecord> records, string outputPath)` | 541 | Thực hiện logic write material manifest csv trong script WeaponMaterialColorExporter. Liên kết trực tiếp: WeaponMaterialRecord. | WeaponMaterialRecord |
| `private static void WriteMaterialManifestJson(List<WeaponMaterialRecord> records, string outputPath)` | 565 | Thực hiện logic write material manifest json trong script WeaponMaterialColorExporter. Liên kết trực tiếp: WeaponMaterialRecordCollection. | WeaponMaterialRecordCollection |
| `private static void WritePromptCsv(List<WeaponPromptRecord> records, string outputPath)` | 570 | Thực hiện logic write prompt csv trong script WeaponMaterialColorExporter. Liên kết trực tiếp: WeaponPromptRecord. | WeaponPromptRecord |
| `private static void WritePromptJson(List<WeaponPromptRecord> records, string outputPath)` | 591 | Thực hiện logic write prompt json trong script WeaponMaterialColorExporter. Liên kết trực tiếp: WeaponPromptRecordCollection. | WeaponPromptRecordCollection |
| `private static string EscapeCsv(string value)` | 596 | Thực hiện logic escape csv trong script WeaponMaterialColorExporter. | - |
| `private static string GetExportAbsoluteRoot()` | 602 | Lấy dữ liệu export absolute root cho hệ thống khác sử dụng. | - |

#### WeaponPreviewExporter

- **Đường dẫn:** `Assets/Game/Scripts/Editor/WeaponPreviewExporter.cs`
- **Loại:** Editor tool
- **Vai trò dễ hiểu:** Công cụ chạy trong Unity Editor để dựng, sửa, kiểm tra hoặc tự động cấu hình scene/prefab/asset.
- **Kế thừa/cha:** -
- **Script con:** -
- **Field public/serialized chính:** public List<WeaponPreviewRecord> records, public string assetPath, public string itemType, public string currentObjectName, public string currentDisplayName, public string suggestedDisplayName, public string namingNotes, public string weaponClass, public string weaponModelType, public string modelPath, public string prefabName, public string meshNames +3
- **Liên kết script:** GameAssetPaths, WeaponClass, WeaponItem, WeaponNamingSuggestion, WeaponPreviewRecord, WeaponPreviewRecordCollection

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public static void ExportWeaponPreviewsAndManifest()` | 25 | Thực hiện logic export weapon previews and manifest trong script WeaponPreviewExporter. Liên kết trực tiếp: WeaponItem, WeaponPreviewRecord. | WeaponItem, WeaponPreviewRecord |
| `public static void ApplyWeaponNamesFromOverrideCsv()` | 61 | Áp dụng weapon names from override csv lên character/object mục tiêu. Liên kết trực tiếp: WeaponItem. | WeaponItem |
| `public static void ApplyAutoSuggestedWeaponNames()` | 95 | Áp dụng auto suggested weapon names lên character/object mục tiêu. Liên kết trực tiếp: WeaponItem, WeaponPreviewRecord. | WeaponItem, WeaponPreviewRecord |
| `public static void SyncWeaponAssetFileNames()` | 118 | Thực hiện logic sync weapon asset file names trong script WeaponPreviewExporter. Liên kết trực tiếp: WeaponItem. | WeaponItem |
| `private static List<WeaponItem> LoadAllWeaponItems()` | 135 | Nạp dữ liệu hoặc scene liên quan tới all weapon items. Liên kết trực tiếp: WeaponItem. | WeaponItem |
| `private static WeaponPreviewRecord BuildRecord(WeaponItem weapon, string assetPath, string previewRelativePath)` | 154 | Thực hiện logic build record trong script WeaponPreviewExporter. Liên kết trực tiếp: WeaponNamingSuggestion, WeaponPreviewRecord. | WeaponNamingSuggestion, WeaponPreviewRecord |
| `private static bool ApplyWeaponIdentity(WeaponItem weapon, string desiredName)` | 178 | Áp dụng weapon identity lên character/object mục tiêu. | - |
| `private static bool RenameWeaponAssetToMatchObjectName(WeaponItem weapon)` | 196 | Thực hiện logic rename weapon asset to match object name trong script WeaponPreviewExporter. | - |
| `private static List<string> CollectMeshNames(GameObject prefab)` | 220 | Thực hiện logic collect mesh names trong script WeaponPreviewExporter. | - |
| `private static void ExportPreviewTexture(GameObject prefab, string absoluteOutputPath)` | 252 | Thực hiện logic export preview texture trong script WeaponPreviewExporter. | - |
| `new PreviewRenderUtility()` | 264 | Thực hiện logic preview render utility trong script WeaponPreviewExporter. | - |
| `new Color(0.42f, 0.42f, 0.42f, 1f)` | 270 | Thực hiện logic color trong script WeaponPreviewExporter. | - |
| `new RenderTexture(PreviewSize, PreviewSize, 24, RenderTextureFormat.ARGB32)` | 276 | Thực hiện logic render texture trong script WeaponPreviewExporter. | - |
| `new Texture2D(PreviewSize, PreviewSize, TextureFormat.RGBA32, false)` | 285 | Thực hiện logic texture2 d trong script WeaponPreviewExporter. | - |
| `new Rect(0, 0, PreviewSize, PreviewSize), 0, 0)` | 286 | Thực hiện logic rect trong script WeaponPreviewExporter. | - |
| `private static Bounds CalculateRenderableBounds(GameObject instance)` | 313 | Tính toán renderable bounds từ chỉ số hoặc dữ liệu hiện có. | - |
| `new Bounds(instance.transform.position, Vector3.one * 0.5f)` | 316 | Thực hiện logic bounds trong script WeaponPreviewExporter. | - |
| `private static void SetupPreviewCamera(Camera camera, Bounds bounds)` | 333 | Thiết lập giá trị hoặc trạng thái up preview camera. | - |
| `new Color(0.11f, 0.11f, 0.115f, 0f)` | 336 | Thực hiện logic color trong script WeaponPreviewExporter. | - |
| `private static void WriteManifestCsv(List<WeaponPreviewRecord> records, string absolutePath)` | 350 | Thực hiện logic write manifest csv trong script WeaponPreviewExporter. Liên kết trực tiếp: WeaponPreviewRecord. | WeaponPreviewRecord |
| `new StringBuilder()` | 352 | Thực hiện logic string builder trong script WeaponPreviewExporter. | - |
| `private static void WriteManifestJson(List<WeaponPreviewRecord> records, string absolutePath)` | 374 | Thực hiện logic write manifest json trong script WeaponPreviewExporter. Liên kết trực tiếp: WeaponPreviewRecordCollection. | WeaponPreviewRecordCollection |
| `private static void EnsureOverrideCsv(List<WeaponPreviewRecord> records, string absolutePath)` | 385 | Thực hiện logic ensure override csv trong script WeaponPreviewExporter. Liên kết trực tiếp: WeaponPreviewRecord. | WeaponPreviewRecord |
| `new StringBuilder()` | 390 | Thực hiện logic string builder trong script WeaponPreviewExporter. | - |
| `string> ReadOverrideCsv(string absolutePath)` | 399 | Thực hiện logic read override csv trong script WeaponPreviewExporter. | - |
| `private static WeaponNamingSuggestion SuggestDisplayName( WeaponItem weapon, string assetPath, string prefabName, List<string> meshNames)` | 426 | Thực hiện logic suggest display name trong script WeaponPreviewExporter. Liên kết trực tiếp: WeaponNamingSuggestion. | WeaponNamingSuggestion |
| `private static string BuildDescriptor(string source)` | 462 | Thực hiện logic build descriptor trong script WeaponPreviewExporter. | - |
| `private static string BuildWeaponType(string source, WeaponItem weapon)` | 496 | Thực hiện logic build weapon type trong script WeaponPreviewExporter. Liên kết trực tiếp: WeaponClass. | WeaponClass |
| `private static bool RequiresVariantNumber(string baseName, string weaponType)` | 607 | Thực hiện logic requires variant number trong script WeaponPreviewExporter. | - |
| `private static int ExtractNumericSuffix(string baseName)` | 619 | Thực hiện logic extract numeric suffix trong script WeaponPreviewExporter. | - |
| `private static string CleanFallbackBaseName(string baseName)` | 631 | Thực hiện logic clean fallback base name trong script WeaponPreviewExporter. | - |
| `private static string NormalizeSourceText(string value)` | 643 | Thực hiện logic normalize source text trong script WeaponPreviewExporter. | - |
| `private static string ToTitleCase(string value)` | 661 | Thực hiện logic to title case trong script WeaponPreviewExporter. | - |
| `private static string EscapeCsv(string value)` | 669 | Thực hiện logic escape csv trong script WeaponPreviewExporter. | - |
| `private static string[] ParseCsvLine(string line)` | 678 | Thực hiện logic parse csv line trong script WeaponPreviewExporter. | - |
| `new StringBuilder()` | 681 | Thực hiện logic string builder trong script WeaponPreviewExporter. | - |
| `private static string SanitizeFileName(string value)` | 717 | Thực hiện logic sanitize file name trong script WeaponPreviewExporter. | - |
| `new StringBuilder(value.Length)` | 720 | Thực hiện logic string builder trong script WeaponPreviewExporter. | - |
| `private static string GetExportAbsoluteRoot()` | 733 | Lấy dữ liệu export absolute root cho hệ thống khác sử dụng. | - |

#### WorldLocationManagerEditor

- **Đường dẫn:** `Assets/Game/Scripts/Editor/WorldLocationManagerEditor.cs`
- **Loại:** Editor tool
- **Vai trò dễ hiểu:** Công cụ chạy trong Unity Editor để dựng, sửa, kiểm tra hoặc tự động cấu hình scene/prefab/asset.
- **Kế thừa/cha:** UnityEditor.Editor
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** WorldLocationManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public override void OnInspectorGUI()` | 9 | Thực hiện logic on inspector gui trong script WorldLocationManagerEditor. Liên kết trực tiếp: WorldLocationManager. | WorldLocationManager |

#### WorldLocationRendererManagerEditor

- **Đường dẫn:** `Assets/Game/Scripts/Editor/WorldLocationRendererManagerEditor.cs`
- **Loại:** Editor tool
- **Vai trò dễ hiểu:** Công cụ chạy trong Unity Editor để dựng, sửa, kiểm tra hoặc tự động cấu hình scene/prefab/asset.
- **Kế thừa/cha:** UnityEditor.Editor
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** WorldLocationRendererManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public override void OnInspectorGUI()` | 9 | Thực hiện logic on inspector gui trong script WorldLocationRendererManagerEditor. Liên kết trực tiếp: WorldLocationRendererManager. | WorldLocationRendererManager |

#### WorldMapTransitionSetupUtility

- **Đường dẫn:** `Assets/Game/Scripts/Editor/WorldMapTransitionSetupUtility.cs`
- **Loại:** Editor tool
- **Vai trò dễ hiểu:** Công cụ chạy trong Unity Editor để dựng, sửa, kiểm tra hoặc tự động cấu hình scene/prefab/asset.
- **Kế thừa/cha:** -
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** GameAssetPaths, WorldMapTransitionInteractable

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public static void CreateWorld01ToWorld02Connector()` | 12 | Tạo object/dữ liệu world01 to world02 connector. Liên kết trực tiếp: GameAssetPaths. | GameAssetPaths |
| `public static void CreateConnectorInCurrentScene()` | 21 | Tạo object/dữ liệu connector in current scene. | - |
| `private static void CreateConnectorInScene(Scene scene)` | 26 | Tạo object/dữ liệu connector in scene. Liên kết trực tiếp: WorldMapTransitionInteractable. | WorldMapTransitionInteractable |
| `new GameObject("Travel To World_02")` | 36 | Thực hiện logic game object trong script WorldMapTransitionSetupUtility. | - |
| `new Vector3(0f, 1f, 35f)` | 38 | Thực hiện logic vector3 trong script WorldMapTransitionSetupUtility. | - |
| `new Vector3(4f, 3f, 4f)` | 42 | Thực hiện logic vector3 trong script WorldMapTransitionSetupUtility. | - |
| `connector at (0, 1, 35). Move it to the desired door/exit if needed.")` | 50 | Thực hiện logic at trong script WorldMapTransitionSetupUtility. | - |

### Assets/Game/Scripts/Effects

#### InstantCharacterEffect

- **Đường dẫn:** `Assets/Game/Scripts/Effects/Base Classes/InstantCharacterEffect.cs`
- **Loại:** ScriptableObject
- **Vai trò dễ hiểu:** Hiệu ứng tác động lên character: damage tức thời, buff tạm thời, buildup hoặc trạng thái tĩnh.
- **Kế thừa/cha:** ScriptableObject
- **Script con:** BloodLossEffect, TakeBlockedDamageEffect, TakeBuildUpEffect, TakeDamageEffect, TakeStaminaDamageEffect
- **Field public/serialized chính:** public int instantEffectID
- **Liên kết script:** CharacterManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public virtual void ProcessEffect(CharacterManager character)` | 10 | Thực hiện logic process effect trong script InstantCharacterEffect. | - |

#### StaticCharacterEffect

- **Đường dẫn:** `Assets/Game/Scripts/Effects/Base Classes/StaticCharacterEffect.cs`
- **Loại:** ScriptableObject
- **Vai trò dễ hiểu:** Hiệu ứng tác động lên character: damage tức thời, buff tạm thời, buildup hoặc trạng thái tĩnh.
- **Kế thừa/cha:** ScriptableObject
- **Script con:** TwoHandingEffect
- **Field public/serialized chính:** public int staticEffectID
- **Liên kết script:** CharacterManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public virtual void ProcessStaticEffect(CharacterManager character)` | 11 | Thực hiện logic process static effect trong script StaticCharacterEffect. | - |
| `public virtual void RemoveStaticEffect(CharacterManager character)` | 16 | Loại bỏ static effect khỏi danh sách, trạng thái hoặc dữ liệu. | - |

#### TimedCharacterEffect

- **Đường dẫn:** `Assets/Game/Scripts/Effects/Base Classes/TimedCharacterEffect.cs`
- **Loại:** ScriptableObject
- **Vai trò dễ hiểu:** Hiệu ứng tác động lên character: damage tức thời, buff tạm thời, buildup hoặc trạng thái tĩnh.
- **Kế thừa/cha:** ScriptableObject
- **Script con:** BuildUpEffect, BurningEffect, FrostBiteEffect, ModifyStaminaRegenerationForATimeEffect, PlayerStatBuffTimedEffect, PoisonedEffect
- **Field public/serialized chính:** public int effectID, public float defaultLengthOfEffect, public float timeRemainingOnEffect
- **Liên kết script:** CharacterManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public virtual void ProcessEffect(CharacterManager character)` | 14 | Thực hiện logic process effect trong script TimedCharacterEffect. | - |
| `public virtual void RemoveEffect(CharacterManager character)` | 22 | Loại bỏ effect khỏi danh sách, trạng thái hoặc dữ liệu. | - |

#### BloodLossEffect

- **Đường dẫn:** `Assets/Game/Scripts/Effects/Instant/BloodLossEffect.cs`
- **Loại:** Effect
- **Vai trò dễ hiểu:** Hiệu ứng tác động lên character: damage tức thời, buff tạm thời, buildup hoặc trạng thái tĩnh.
- **Kế thừa/cha:** InstantCharacterEffect
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** CharacterManager, InstantCharacterEffect

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public override void ProcessEffect(CharacterManager character)` | 11 | Thực hiện logic process effect trong script BloodLossEffect. | - |
| `private void CalculateDamage(CharacterManager character)` | 19 | Tính toán damage từ chỉ số hoặc dữ liệu hiện có. | - |
| `private void CheckForDeath(CharacterManager character)` | 30 | Thực hiện logic check for death trong script BloodLossEffect. | - |

#### TakeBlockedDamageEffect

- **Đường dẫn:** `Assets/Game/Scripts/Effects/Instant/TakeBlockedDamageEffect.cs`
- **Loại:** Effect
- **Vai trò dễ hiểu:** Hiệu ứng tác động lên character: damage tức thời, buff tạm thời, buildup hoặc trạng thái tĩnh.
- **Kế thừa/cha:** InstantCharacterEffect
- **Script con:** -
- **Field public/serialized chính:** public CharacterManager characterCausingDamage, public float physicalDamage, public float magicDamage, public float fireDamage, public float lightningDamage, public float holyDamage, public float poiseDamage, public bool poiseIsBroken, public float staminaDamage, public float finalStaminaDamage, public bool playDamageAnimation, public bool manuallySelectDamageAnimation +5
- **Liên kết script:** CharacterManager, DamageIntensity, InstantCharacterEffect, WorldUtilityManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public override void ProcessEffect(CharacterManager character)` | 43 | Thực hiện logic process effect trong script TakeBlockedDamageEffect. | - |
| `private void CalculateDamage(CharacterManager character)` | 66 | Tính toán damage từ chỉ số hoặc dữ liệu hiện có. | - |
| `private void CalculateStaminaDamage(CharacterManager character)` | 96 | Tính toán stamina damage từ chỉ số hoặc dữ liệu hiện có. | - |
| `private void CheckForGuardBreak(CharacterManager character)` | 108 | Thực hiện logic check for guard break trong script TakeBlockedDamageEffect. | - |
| `private void PlayDamageVFX(CharacterManager character)` | 120 | Phát damage vfx, thường là animation, sound hoặc VFX. | - |
| `private void PlayDamageSFX(CharacterManager character)` | 125 | Phát damage sfx, thường là animation, sound hoặc VFX. | - |
| `private void PLayDirectionalBasedBlockingAnimation(CharacterManager character)` | 130 | Thực hiện logic play directional based blocking animation trong script TakeBlockedDamageEffect. Liên kết trực tiếp: DamageIntensity, WorldUtilityManager. | DamageIntensity, WorldUtilityManager |

#### TakeBuildUpEffect

- **Đường dẫn:** `Assets/Game/Scripts/Effects/Instant/TakeBuildUpEffect.cs`
- **Loại:** Effect
- **Vai trò dễ hiểu:** Hiệu ứng tác động lên character: damage tức thời, buff tạm thời, buildup hoặc trạng thái tĩnh.
- **Kế thừa/cha:** InstantCharacterEffect
- **Script con:** -
- **Field public/serialized chính:** public int buildUpAmount
- **Liên kết script:** BloodLossEffect, BuildUp, BuildUpEffect, BurningEffect, CharacterManager, FrostBiteEffect, InstantCharacterEffect, PlayerManager, PoisonedEffect, WorldCharacterEffectsManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public override void ProcessEffect(CharacterManager character)` | 12 | Thực hiện logic process effect trong script TakeBuildUpEffect. Liên kết trực tiếp: BuildUp. | BuildUp |
| `private void CheckForPoisonedStatus(CharacterManager character)` | 37 | Thực hiện logic check for poisoned status trong script TakeBuildUpEffect. Liên kết trực tiếp: BuildUpEffect, PlayerManager, PoisonedEffect, WorldCharacterEffectsManager. | BuildUpEffect, PlayerManager, PoisonedEffect, WorldCharacterEffectsManager |
| `private void CheckForBurningStatus(CharacterManager character)` | 72 | Thực hiện logic check for burning status trong script TakeBuildUpEffect. Liên kết trực tiếp: BuildUpEffect, BurningEffect, WorldCharacterEffectsManager. | BuildUpEffect, BurningEffect, WorldCharacterEffectsManager |
| `private void CheckForBloodLossStatus(CharacterManager character)` | 96 | Thực hiện logic check for blood loss status trong script TakeBuildUpEffect. Liên kết trực tiếp: BloodLossEffect, BuildUpEffect, PlayerManager, WorldCharacterEffectsManager. | BloodLossEffect, BuildUpEffect, PlayerManager, WorldCharacterEffectsManager |
| `private void CheckForFrostBiteStatus(CharacterManager character)` | 129 | Thực hiện logic check for frost bite status trong script TakeBuildUpEffect. Liên kết trực tiếp: BuildUpEffect, FrostBiteEffect, PlayerManager, WorldCharacterEffectsManager. | BuildUpEffect, FrostBiteEffect, PlayerManager, WorldCharacterEffectsManager |

#### TakeCriticalDamageEffect

- **Đường dẫn:** `Assets/Game/Scripts/Effects/Instant/TakeCriticalDamageEffect.cs`
- **Loại:** Effect
- **Vai trò dễ hiểu:** Hiệu ứng tác động lên character: damage tức thời, buff tạm thời, buildup hoặc trạng thái tĩnh.
- **Kế thừa/cha:** TakeDamageEffect
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** CharacterManager, TakeDamageEffect

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public override void ProcessEffect(CharacterManager character)` | 8 | Thực hiện logic process effect trong script TakeCriticalDamageEffect. | - |
| `protected override void CalculateDamage(CharacterManager character)` | 22 | Tính toán damage từ chỉ số hoặc dữ liệu hiện có. | - |

#### TakeDamageEffect

- **Đường dẫn:** `Assets/Game/Scripts/Effects/Instant/TakeDamageEffect.cs`
- **Loại:** Effect
- **Vai trò dễ hiểu:** Hiệu ứng tác động lên character: damage tức thời, buff tạm thời, buildup hoặc trạng thái tĩnh.
- **Kế thừa/cha:** InstantCharacterEffect
- **Script con:** TakeCriticalDamageEffect
- **Field public/serialized chính:** public CharacterManager characterCausingDamage, public float physicalDamage, public float magicDamage, public float fireDamage, public float lightningDamage, public float holyDamage, public float poiseDamage, public bool poiseIsBroken, public bool playDamageAnimation, public bool manuallySelectDamageAnimation, public string damageAnimation, public bool willPlayDamageSFX +3
- **Liên kết script:** AICharacterManager, AIMonster33BossCharacterNetworkManager, AIMonster33CombatManager, CharacterManager, InstantCharacterEffect, PlayerEffectsManager, PlayerManager, WorldSoundFXManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public override void ProcessEffect(CharacterManager character)` | 39 | Thực hiện logic process effect trong script TakeDamageEffect. | - |
| `protected void RegisterDamageDealer(CharacterManager character)` | 60 | Thực hiện logic register damage dealer trong script TakeDamageEffect. Liên kết trực tiếp: AICharacterManager, PlayerManager. | AICharacterManager, PlayerManager |
| `private void ApplyAttackerBuildUps(CharacterManager character)` | 69 | Áp dụng attacker build ups lên character/object mục tiêu. Liên kết trực tiếp: PlayerEffectsManager, PlayerManager. | PlayerEffectsManager, PlayerManager |
| `private bool ShouldApplyMonster33PowerUpFireBuildUp()` | 83 | Thực hiện logic should apply monster33 power up fire build up trong script TakeDamageEffect. Liên kết trực tiếp: AIMonster33BossCharacterNetworkManager, AIMonster33CombatManager. | AIMonster33BossCharacterNetworkManager, AIMonster33CombatManager |
| `protected virtual void CalculateDamage(CharacterManager character)` | 99 | Tính toán damage từ chỉ số hoặc dữ liệu hiện có. | - |
| `protected void CalculateStanceDamage(CharacterManager character)` | 132 | Tính toán stance damage từ chỉ số hoặc dữ liệu hiện có. Liên kết trực tiếp: AICharacterManager. | AICharacterManager |
| `protected void PlayDamageVFX(CharacterManager character)` | 145 | Phát damage vfx, thường là animation, sound hoặc VFX. | - |
| `protected void PlayDamageSFX(CharacterManager character)` | 150 | Phát damage sfx, thường là animation, sound hoặc VFX. Liên kết trực tiếp: WorldSoundFXManager. | WorldSoundFXManager |
| `protected void PLayDirectionalBasedDamageAnimation(CharacterManager character)` | 158 | Thực hiện logic play directional based damage animation trong script TakeDamageEffect. | - |

#### TakeStaminaDamageEffect

- **Đường dẫn:** `Assets/Game/Scripts/Effects/Instant/TakeStaminaDamageEffect.cs`
- **Loại:** Effect
- **Vai trò dễ hiểu:** Hiệu ứng tác động lên character: damage tức thời, buff tạm thời, buildup hoặc trạng thái tĩnh.
- **Kế thừa/cha:** InstantCharacterEffect
- **Script con:** -
- **Field public/serialized chính:** public float staminaDamage
- **Liên kết script:** CharacterManager, InstantCharacterEffect

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public override void ProcessEffect(CharacterManager character)` | 10 | Thực hiện logic process effect trong script TakeStaminaDamageEffect. | - |
| `private void CalculateStaminaDamage(CharacterManager character)` | 15 | Tính toán stamina damage từ chỉ số hoặc dữ liệu hiện có. | - |

#### TwoHandingEffect

- **Đường dẫn:** `Assets/Game/Scripts/Effects/Static/TwoHandingEffect.cs`
- **Loại:** Effect
- **Vai trò dễ hiểu:** Hiệu ứng tác động lên character: damage tức thời, buff tạm thời, buildup hoặc trạng thái tĩnh.
- **Kế thừa/cha:** StaticCharacterEffect
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** CharacterManager, StaticCharacterEffect

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public override void ProcessStaticEffect(CharacterManager character)` | 11 | Thực hiện logic process static effect trong script TwoHandingEffect. | - |
| `public override void RemoveStaticEffect(CharacterManager character)` | 23 | Loại bỏ static effect khỏi danh sách, trạng thái hoặc dữ liệu. | - |

#### BuildUpEffect

- **Đường dẫn:** `Assets/Game/Scripts/Effects/Timed/BuildUpEffect.cs`
- **Loại:** Effect
- **Vai trò dễ hiểu:** Hiệu ứng tác động lên character: damage tức thời, buff tạm thời, buildup hoặc trạng thái tĩnh.
- **Kế thừa/cha:** TimedCharacterEffect
- **Script con:** -
- **Field public/serialized chính:** public BuildUp buildUpType, public int buildUpAmountDegradation, public float buildUpRemaining
- **Liên kết script:** BuildUp, CharacterManager, TimedCharacterEffect

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public override void ProcessEffect(CharacterManager character)` | 15 | Thực hiện logic process effect trong script BuildUpEffect. | - |
| `public override void RemoveEffect(CharacterManager character)` | 33 | Loại bỏ effect khỏi danh sách, trạng thái hoặc dữ liệu. | - |
| `private void DegradeBuildUp(CharacterManager character)` | 38 | Thực hiện logic degrade build up trong script BuildUpEffect. | - |
| `private float GetCurrentBuildUpValue(CharacterManager character)` | 43 | Lấy dữ liệu current build up value cho hệ thống khác sử dụng. Liên kết trực tiếp: BuildUp. | BuildUp |

#### BurningEffect

- **Đường dẫn:** `Assets/Game/Scripts/Effects/Timed/BurningEffect.cs`
- **Loại:** Effect
- **Vai trò dễ hiểu:** Hiệu ứng tác động lên character: damage tức thời, buff tạm thời, buildup hoặc trạng thái tĩnh.
- **Kế thừa/cha:** TimedCharacterEffect
- **Script con:** -
- **Field public/serialized chính:** [SerializeField] private int burnDamage
- **Liên kết script:** CharacterManager, TimedCharacterEffect

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public override void ProcessEffect(CharacterManager character)` | 11 | Thực hiện logic process effect trong script BurningEffect. | - |
| `public override void RemoveEffect(CharacterManager character)` | 33 | Loại bỏ effect khỏi danh sách, trạng thái hoặc dữ liệu. | - |

#### FrostBiteEffect

- **Đường dẫn:** `Assets/Game/Scripts/Effects/Timed/FrostBiteEffect.cs`
- **Loại:** Effect
- **Vai trò dễ hiểu:** Hiệu ứng tác động lên character: damage tức thời, buff tạm thời, buildup hoặc trạng thái tĩnh.
- **Kế thừa/cha:** TimedCharacterEffect
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** CharacterManager, ModifyStaminaRegenerationForATimeEffect, TimedCharacterEffect, WorldCharacterEffectsManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public override void ProcessEffect(CharacterManager character)` | 14 | Thực hiện logic process effect trong script FrostBiteEffect. | - |
| `public override void RemoveEffect(CharacterManager character)` | 25 | Loại bỏ effect khỏi danh sách, trạng thái hoặc dữ liệu. | - |
| `private void InflictStaminaRegenerationDebuff(CharacterManager character)` | 33 | Thực hiện logic inflict stamina regeneration debuff trong script FrostBiteEffect. Liên kết trực tiếp: ModifyStaminaRegenerationForATimeEffect, WorldCharacterEffectsManager. | ModifyStaminaRegenerationForATimeEffect, WorldCharacterEffectsManager |

#### ModifyStaminaRegenerationForATimeEffect

- **Đường dẫn:** `Assets/Game/Scripts/Effects/Timed/ModifyStaminaRegenerationForATimeEffect.cs`
- **Loại:** Effect
- **Vai trò dễ hiểu:** Hiệu ứng tác động lên character: damage tức thời, buff tạm thời, buildup hoặc trạng thái tĩnh.
- **Kế thừa/cha:** TimedCharacterEffect
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** CharacterManager, TimedCharacterEffect

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public override void ProcessEffect(CharacterManager character)` | 14 | Thực hiện logic process effect trong script ModifyStaminaRegenerationForATimeEffect. | - |
| `public override void RemoveEffect(CharacterManager character)` | 28 | Loại bỏ effect khỏi danh sách, trạng thái hoặc dữ liệu. | - |

#### PlayerStatBuffTimedEffect

- **Đường dẫn:** `Assets/Game/Scripts/Effects/Timed/PlayerStatBuffTimedEffect.cs`
- **Loại:** Effect
- **Vai trò dễ hiểu:** Hiệu ứng tác động lên character: damage tức thời, buff tạm thời, buildup hoặc trạng thái tĩnh.
- **Kế thừa/cha:** TimedCharacterEffect
- **Script con:** -
- **Field public/serialized chính:** public int sourceItemID, public int maxHealthBonus, public int maxStaminaBonus, public int maxFocusPointsBonus, public float staminaRegenerationBonusPercentage, public float outgoingDamageBonusPercentage, [SerializeField] private bool effectHasBeenInitialized
- **Liên kết script:** CharacterManager, PlayerManager, PlayerUIManager, TimedCharacterEffect

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public override void ProcessEffect(CharacterManager character)` | 20 | Thực hiện logic process effect trong script PlayerStatBuffTimedEffect. | - |
| `public override void RemoveEffect(CharacterManager character)` | 28 | Loại bỏ effect khỏi danh sách, trạng thái hoặc dữ liệu. Liên kết trực tiếp: PlayerManager, PlayerUIManager. | PlayerManager, PlayerUIManager |
| `private void ApplyEffect(CharacterManager character)` | 56 | Áp dụng effect lên character/object mục tiêu. Liên kết trực tiếp: PlayerManager. | PlayerManager |

#### PoisonedEffect

- **Đường dẫn:** `Assets/Game/Scripts/Effects/Timed/PoisonedEffect.cs`
- **Loại:** Effect
- **Vai trò dễ hiểu:** Hiệu ứng tác động lên character: damage tức thời, buff tạm thời, buildup hoặc trạng thái tĩnh.
- **Kế thừa/cha:** TimedCharacterEffect
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** CharacterManager, TimedCharacterEffect

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public override void ProcessEffect(CharacterManager character)` | 11 | Thực hiện logic process effect trong script PoisonedEffect. | - |
| `private void CalculatePoisonDamage(CharacterManager character)` | 30 | Tính toán poison damage từ chỉ số hoặc dữ liệu hiện có. | - |
| `private void ProcessPoisonDamage(CharacterManager character)` | 35 | Thực hiện logic process poison damage trong script PoisonedEffect. | - |
| `public override void RemoveEffect(CharacterManager character)` | 40 | Loại bỏ effect khỏi danh sách, trạng thái hoặc dữ liệu. | - |

### Assets/Game/Scripts/Function

#### AnvilInteractable

- **Đường dẫn:** `Assets/Game/Scripts/Function/AnvilInteractable.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Script chức năng gắn vào object trong scene: interactable, trigger, pickup, elevator, fog wall, dialogue hoặc reset animation flag.
- **Kế thừa/cha:** Interactable
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** Interactable, PlayerManager, PlayerUIManager, WorldSaveGameManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public override void Interact(PlayerManager player)` | 7 | Thực hiện hành động tương tác khi player chọn object này. Liên kết trực tiếp: PlayerUIManager, WorldSaveGameManager. | PlayerUIManager, WorldSaveGameManager |
| `public override void OnTriggerExit(Collider other)` | 23 | Xử lý khi collider khác rời khỏi trigger của object này. Liên kết trực tiếp: PlayerManager, PlayerUIManager. | PlayerManager, PlayerUIManager |

#### BeaconDetector

- **Đường dẫn:** `Assets/Game/Scripts/Function/BeaconDetector.cs`
- **Loại:** MonoBehaviour
- **Vai trò dễ hiểu:** Script chức năng gắn vào object trong scene: interactable, trigger, pickup, elevator, fog wall, dialogue hoặc reset animation flag.
- **Kế thừa/cha:** MonoBehaviour
- **Script con:** -
- **Field public/serialized chính:** public PlayerManager player
- **Liên kết script:** AICharacterManager, PlayerManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `private void OnTriggerEnter(Collider other)` | 9 | Xử lý khi collider khác đi vào trigger của object này. | - |
| `private void OnTriggerExit(Collider other)` | 14 | Xử lý khi collider khác rời khỏi trigger của object này. Liên kết trực tiếp: AICharacterManager. | AICharacterManager |

#### BreakableObject

- **Đường dẫn:** `Assets/Game/Scripts/Function/BreakableObject.cs`
- **Loại:** NetworkBehaviour
- **Vai trò dễ hiểu:** Script chức năng gắn vào object trong scene: interactable, trigger, pickup, elevator, fog wall, dialogue hoặc reset animation flag.
- **Kế thừa/cha:** NetworkBehaviour
- **Script con:** -
- **Field public/serialized chính:** public NetworkVariable<Vector3> networkPosition, public NetworkVariable<Quaternion> networkRotation, public NetworkVariable<bool> isBroken, public bool isBrokenLocal, [SerializeField] private MeshRenderer[] meshRenderers, [SerializeField] private GameObject brokenObjectPrefab
- **Liên kết script:** AICharacterManager, DamageCollider, PlayerManager, WorldSoundFXManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `private void Awake()` | 38 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. | - |
| `private void Start()` | 45 | Chạy sau Awake để hoàn tất thiết lập ban đầu, nhất là khi cần các object khác đã sẵn sàng. | - |
| `public override void OnNetworkSpawn()` | 50 | Thiết lập dữ liệu và callback khi NetworkObject được spawn trong Netcode. | - |
| `public override void OnNetworkDespawn()` | 62 | Dọn dữ liệu/callback khi NetworkObject bị despawn khỏi Netcode. | - |
| `public override void OnDestroy()` | 74 | Dọn đăng ký/event/tài nguyên khi object bị hủy. | - |
| `private void OnTriggerEnter(Collider other)` | 79 | Xử lý khi collider khác đi vào trigger của object này. Liên kết trực tiếp: AICharacterManager, DamageCollider, PlayerManager. | AICharacterManager, DamageCollider, PlayerManager |
| `private void BreakObject()` | 101 | Thực hiện logic break object trong script BreakableObject. | - |
| `private void BreakObjectServerRpc()` | 111 | Gửi yêu cầu lên server trong Netcode để server xử lý break object. | - |
| `private void OnIsBrokenChanged(bool oldStatus, bool newStatus)` | 117 | Thực hiện logic on is broken changed trong script BreakableObject. | - |
| `private void PlayBreakFX()` | 126 | Phát break fx, thường là animation, sound hoặc VFX. Liên kết trực tiếp: WorldSoundFXManager. | WorldSoundFXManager |
| `private void OnNetworkPositionChanged(Vector3 oldPosition, Vector3 newPosition)` | 156 | Thực hiện logic on network position changed trong script BreakableObject. | - |
| `private void OnNetworkRotationChanged(Quaternion oldRotation, Quaternion newRotation)` | 161 | Thực hiện logic on network rotation changed trong script BreakableObject. | - |
| `private void ToggleMeshRenderers(bool status)` | 166 | Thực hiện logic toggle mesh renderers trong script BreakableObject. | - |
| `private void ToggleMeshColliders(bool status)` | 177 | Thực hiện logic toggle mesh colliders trong script BreakableObject. | - |

#### CallElevatorInteractable

- **Đường dẫn:** `Assets/Game/Scripts/Function/CallElevatorInteractable.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Script chức năng gắn vào object trong scene: interactable, trigger, pickup, elevator, fog wall, dialogue hoặc reset animation flag.
- **Kế thừa/cha:** Interactable
- **Script con:** CallElevatorLeverInteractable
- **Field public/serialized chính:** [SerializeField] protected ElevatorInteractable elevator, public List<PlayerManager> playersWithinInteractionTrigger
- **Liên kết script:** ElevatorInteractable, Interactable, PlayerManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public override void OnTriggerEnter(Collider other)` | 20 | Xử lý khi collider khác đi vào trigger của object này. Liên kết trực tiếp: PlayerManager. | PlayerManager |
| `public override void OnTriggerExit(Collider other)` | 28 | Xử lý khi collider khác rời khỏi trigger của object này. Liên kết trực tiếp: PlayerManager. | PlayerManager |
| `public override void Interact(PlayerManager player)` | 38 | Thực hiện hành động tương tác khi player chọn object này. | - |
| `public void AddCharacterToListOfCharactersOnElevator(PlayerManager player)` | 43 | Thêm character to list of characters on elevator vào danh sách, trạng thái hoặc dữ liệu. | - |
| `public void RemoveCharacterFromListOfCharactersOnElevator(PlayerManager player)` | 63 | Loại bỏ character from list of characters on elevator khỏi danh sách, trạng thái hoặc dữ liệu. | - |
| `private IEnumerator CheckForCharactersInTrigger()` | 77 | Thực hiện logic check for characters in trigger trong script CallElevatorInteractable. | - |
| `public void RemoveInteractionFromPlayers()` | 97 | Loại bỏ interaction from players khỏi danh sách, trạng thái hoặc dữ liệu. | - |
| `public void ReturnInteractionToPlayers()` | 111 | Thực hiện logic return interaction to players trong script CallElevatorInteractable. | - |

#### CallElevatorLeverInteractable

- **Đường dẫn:** `Assets/Game/Scripts/Function/CallElevatorLeverInteractable.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Script chức năng gắn vào object trong scene: interactable, trigger, pickup, elevator, fog wall, dialogue hoặc reset animation flag.
- **Kế thừa/cha:** CallElevatorInteractable
- **Script con:** -
- **Field public/serialized chính:** public NetworkVariable<bool> leverHasBeenPulled
- **Liên kết script:** CallElevatorInteractable, PlayerManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public override void Interact(PlayerManager player)` | 21 | Thực hiện hành động tương tác khi player chọn object này. | - |
| `private void ActivateElevatorWithLever()` | 26 | Thực hiện logic activate elevator with lever trong script CallElevatorLeverInteractable. | - |
| `private void PullLeverServerRpc()` | 54 | Gửi yêu cầu lên server trong Netcode để server xử lý pull lever. | - |
| `private void PullLeverClientRpc()` | 61 | Server gọi xuống client để đồng bộ hoặc phát hiệu ứng cho pull lever. | - |
| `private IEnumerator WaitForLeverAnimationThenMoveElevator()` | 66 | Thực hiện logic wait for lever animation then move elevator trong script CallElevatorLeverInteractable. | - |
| `new WaitForSeconds(timeToWaitAfterPullingLeverToMoveElevator)` | 74 | Thực hiện logic wait for seconds trong script CallElevatorLeverInteractable. | - |
| `private IEnumerator WaitForElevatorLeverToRelease()` | 86 | Thực hiện logic wait for elevator lever to release trong script CallElevatorLeverInteractable. | - |
| `new WaitForSeconds(minimumButtonReleaseTime)` | 93 | Thực hiện logic wait for seconds trong script CallElevatorLeverInteractable. | - |

#### CharacterClass

- **Đường dẫn:** `Assets/Game/Scripts/Function/CharacterClass.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Script chức năng gắn vào object trong scene: interactable, trigger, pickup, elevator, fog wall, dialogue hoặc reset animation flag.
- **Kế thừa/cha:** -
- **Script con:** -
- **Field public/serialized chính:** public string className, public int vitality, public int endurance, public int mind, public int strength, public int dexterity, public int intelligence, public int faith, public WeaponItem[] mainHandWeapons, public WeaponItem[] offHandWeapons, public HeadEquipmentItem headEquipment, public BodyEquipmentItem bodyEquipment +3
- **Liên kết script:** BodyEquipmentItem, HandEquipmentItem, HeadEquipmentItem, LegEquipmentItem, PlayerManager, QuickSlotItem, TitleScreenManager, WeaponItem

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public void SetClass(PlayerManager player)` | 33 | Thiết lập giá trị hoặc trạng thái class. Liên kết trực tiếp: TitleScreenManager. | TitleScreenManager |

#### CharacterDialogue

- **Đường dẫn:** `Assets/Game/Scripts/Function/CharacterDialogue.cs`
- **Loại:** ScriptableObject
- **Vai trò dễ hiểu:** Script chức năng gắn vào object trong scene: interactable, trigger, pickup, elevator, fog wall, dialogue hoặc reset animation flag.
- **Kế thừa/cha:** ScriptableObject
- **Script con:** -
- **Field public/serialized chính:** public int requiredStageID, public List<string> greetingDialogueString, public List<AudioClip> greetingDialogueAudio, public List<string> dialogueString, public List<AudioClip> dialogueAudio, public int dialogueIndex, public List<string> farewellDialogueString, public List<AudioClip> farewellDialogueAudio
- **Liên kết script:** AICharacterManager, DialogueEndEvents, PlayerUIManager, WorldSaveGameManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public void PlayDialogueEvent(AICharacterManager aiCharacter)` | 34 | Phát dialogue event, thường là animation, sound hoặc VFX. Liên kết trực tiếp: PlayerUIManager. | PlayerUIManager |
| `public IEnumerator PlayDialogueCoroutine(AICharacterManager aiCharacter)` | 46 | Phát dialogue coroutine, thường là animation, sound hoặc VFX. Liên kết trực tiếp: PlayerUIManager. | PlayerUIManager |
| `new WaitForSeconds(greetingDialogueAudio[randomGreetingDialogueIndex].length + 1)` | 55 | Thực hiện logic wait for seconds trong script CharacterDialogue. | - |
| `new WaitForSeconds(dialogueAudio[dialogueIndex].length + 1)` | 62 | Thực hiện logic wait for seconds trong script CharacterDialogue. | - |
| `new WaitForSeconds(farewellDialogueAudio[randomFarewellDialogueIndex].length + 1)` | 73 | Thực hiện logic wait for seconds trong script CharacterDialogue. | - |
| `public void OnDialogueEnded(AICharacterManager aiCharacter)` | 82 | Thực hiện logic on dialogue ended trong script CharacterDialogue. Liên kết trực tiếp: DialogueEndEvents, PlayerUIManager, WorldSaveGameManager. | DialogueEndEvents, PlayerUIManager, WorldSaveGameManager |
| `public void OnDialogueCancelled(AICharacterManager aiCharacter)` | 109 | Thực hiện logic on dialogue cancelled trong script CharacterDialogue. Liên kết trực tiếp: DialogueEndEvents, PlayerUIManager. | DialogueEndEvents, PlayerUIManager |

#### DialogueInteractable

- **Đường dẫn:** `Assets/Game/Scripts/Function/DialogueInteractable.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Script chức năng gắn vào object trong scene: interactable, trigger, pickup, elevator, fog wall, dialogue hoặc reset animation flag.
- **Kế thừa/cha:** Interactable
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** AICharacterManager, Interactable, PlayerManager, PlayerUIManager, WorldSaveGameManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `protected override void Awake()` | 10 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. Liên kết trực tiếp: AICharacterManager. | AICharacterManager |
| `public override void Interact(PlayerManager player)` | 17 | Thực hiện hành động tương tác khi player chọn object này. Liên kết trực tiếp: PlayerUIManager, WorldSaveGameManager. | PlayerUIManager, WorldSaveGameManager |
| `public override void OnTriggerEnter(Collider other)` | 34 | Xử lý khi collider khác đi vào trigger của object này. Liên kết trực tiếp: PlayerManager. | PlayerManager |
| `public override void OnTriggerExit(Collider other)` | 50 | Xử lý khi collider khác rời khỏi trigger của object này. Liên kết trực tiếp: PlayerManager. | PlayerManager |

#### ElevatorButtonTrigger

- **Đường dẫn:** `Assets/Game/Scripts/Function/ElevatorButtonTrigger.cs`
- **Loại:** MonoBehaviour
- **Vai trò dễ hiểu:** Script chức năng gắn vào object trong scene: interactable, trigger, pickup, elevator, fog wall, dialogue hoặc reset animation flag.
- **Kế thừa/cha:** MonoBehaviour
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** CharacterManager, ElevatorInteractable

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `private void OnTriggerEnter(Collider other)` | 23 | Xử lý khi collider khác đi vào trigger của object này. Liên kết trực tiếp: CharacterManager. | CharacterManager |
| `private void OnTriggerExit(Collider other)` | 31 | Xử lý khi collider khác rời khỏi trigger của object này. Liên kết trực tiếp: CharacterManager. | CharacterManager |
| `public void AddCharacterToListOfCharactersOnElevatorButton(CharacterManager character)` | 39 | Thêm character to list of characters on elevator button vào danh sách, trạng thái hoặc dữ liệu. | - |
| `public void RemoveCharacterFromListOfCharactersOnElevatorButton(CharacterManager character)` | 56 | Loại bỏ character from list of characters on elevator button khỏi danh sách, trạng thái hoặc dữ liệu. | - |
| `private void ActivateElevatorWithButton()` | 70 | Thực hiện logic activate elevator with button trong script ElevatorButtonTrigger. | - |
| `private IEnumerator WaitForElevatorButtonToRelease()` | 88 | Thực hiện logic wait for elevator button to release trong script ElevatorButtonTrigger. | - |
| `new WaitForSeconds(minimumButtonReleaseTime)` | 95 | Thực hiện logic wait for seconds trong script ElevatorButtonTrigger. | - |

#### ElevatorInteractable

- **Đường dẫn:** `Assets/Game/Scripts/Function/ElevatorInteractable.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Script chức năng gắn vào object trong scene: interactable, trigger, pickup, elevator, fog wall, dialogue hoặc reset animation flag.
- **Kế thừa/cha:** Interactable
- **Script con:** -
- **Field public/serialized chính:** public NetworkVariable<Vector3> networkPosition, public NetworkVariable<bool> elevatorIsRising, public NetworkVariable<bool> elevatorIsDescending, public Vector3 destinationHigh, public Vector3 destinationLow, public CallElevatorInteractable lowDestinationRecall, public CallElevatorInteractable highDestinationRecall, [SerializeField] protected List<CharacterManager> charactersOnElevator, [SerializeField] private AudioClip elevatorMovingSFX, [SerializeField] private AudioClip[] elevatorStoppingSFX
- **Liên kết script:** CallElevatorInteractable, CharacterManager, Interactable, PlayerManager, WorldSoundFXManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `protected override void Awake()` | 35 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. | - |
| `public override void OnTriggerEnter(Collider other)` | 42 | Xử lý khi collider khác đi vào trigger của object này. | - |
| `public override void Interact(PlayerManager player)` | 50 | Thực hiện hành động tương tác khi player chọn object này. | - |
| `public override void OnNetworkSpawn()` | 58 | Thiết lập dữ liệu và callback khi NetworkObject được spawn trong Netcode. | - |
| `public override void OnNetworkDespawn()` | 78 | Dọn dữ liệu/callback khi NetworkObject bị despawn khỏi Netcode. | - |
| `private void ActivateElevator(bool isRising)` | 83 | Thực hiện logic activate elevator trong script ElevatorInteractable. | - |
| `private IEnumerator MoveElevatorCoroutine(bool isRising)` | 88 | Thực hiện logic move elevator coroutine trong script ElevatorInteractable. Liên kết trực tiếp: PlayerManager, WorldSoundFXManager. | PlayerManager, WorldSoundFXManager |
| `new Vector3( charactersOnElevator[i].transform.position.x, velocityOfMovement.y + yMovementOffSet, charactersOnElevator[i].transform.position.z)` | 138 | Thực hiện logic vector3 trong script ElevatorInteractable. | - |
| `public void AddCharacterToListOfCharactersOnElevator(CharacterManager character)` | 169 | Thêm character to list of characters on elevator vào danh sách, trạng thái hoặc dữ liệu. | - |
| `public void RemoveCharacterFromListOfCharactersOnElevator(CharacterManager character)` | 178 | Loại bỏ character from list of characters on elevator khỏi danh sách, trạng thái hoặc dữ liệu. | - |
| `public void ActivateElevatorServerRpc()` | 189 | Gửi yêu cầu lên server trong Netcode để server xử lý activate elevator. | - |
| `private void ActivateElevatorClientRpc()` | 197 | Server gọi xuống client để đồng bộ hoặc phát hiệu ứng cho activate elevator. | - |

#### Enums

- **Đường dẫn:** `Assets/Game/Scripts/Function/Enums.cs`
- **Loại:** MonoBehaviour
- **Vai trò dễ hiểu:** Script chức năng gắn vào object trong scene: interactable, trigger, pickup, elevator, fog wall, dialogue hoặc reset animation flag.
- **Kế thừa/cha:** MonoBehaviour
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** AttackType, BuildUp, CharacterAttribute, CharacterDialogueID, CharacterGroup, CharacterSlot, DamageIntensity, DialogueEndEvents, EquipmentModelType, EquipmentType, HeadEquipmentType, IdleStateMode, ItemPickUpType, ProjectileClass, ProjectileSlot, SpellClass +6

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| - | - | Script này chủ yếu là dữ liệu/enum/struct, không có method rõ ràng. | - |

#### EventTriggerBossFight

- **Đường dẫn:** `Assets/Game/Scripts/Function/EventTriggerBossFight.cs`
- **Loại:** MonoBehaviour
- **Vai trò dễ hiểu:** Script chức năng gắn vào object trong scene: interactable, trigger, pickup, elevator, fog wall, dialogue hoặc reset animation flag.
- **Kế thừa/cha:** MonoBehaviour
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** AIBossCharacterManager, PlayerManager, WorldAIManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `private void Awake()` | 11 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. | - |
| `private void OnValidate()` | 19 | Tự kiểm tra/cập nhật giá trị trong Unity Editor khi inspector thay đổi. | - |
| `private void AutoAssignBossIDFromWorldScene()` | 24 | Thực hiện logic auto assign boss idfrom world scene trong script EventTriggerBossFight. | - |
| `private void Start()` | 34 | Chạy sau Awake để hoàn tất thiết lập ban đầu, nhất là khi cần các object khác đã sẵn sàng. | - |
| `private void OnTriggerEnter(Collider other)` | 39 | Xử lý khi collider khác đi vào trigger của object này. Liên kết trực tiếp: AIBossCharacterManager, PlayerManager, WorldAIManager. | AIBossCharacterManager, PlayerManager, WorldAIManager |
| `private IEnumerator SyncTriggerState()` | 73 | Thực hiện logic sync trigger state trong script EventTriggerBossFight. Liên kết trực tiếp: AIBossCharacterManager, WorldAIManager. | AIBossCharacterManager, WorldAIManager |
| `private IEnumerator TryWakeBossWhenAvailable()` | 89 | Thử thực hiện wake boss when available, thường có kiểm tra điều kiện trước khi chạy. Liên kết trực tiếp: AIBossCharacterManager, WorldAIManager. | AIBossCharacterManager, WorldAIManager |
| `private void DisableTrigger()` | 109 | Tắt trigger. | - |

#### EventTriggerWakeNearbyCharacters

- **Đường dẫn:** `Assets/Game/Scripts/Function/EventTriggerWakeNearbyCharacters.cs`
- **Loại:** MonoBehaviour
- **Vai trò dễ hiểu:** Script chức năng gắn vào object trong scene: interactable, trigger, pickup, elevator, fog wall, dialogue hoặc reset animation flag.
- **Kế thừa/cha:** MonoBehaviour
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** AICharacterManager, PlayerManager, WorldUtilityManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `private void OnTriggerEnter(Collider other)` | 11 | Xử lý khi collider khác đi vào trigger của object này. Liên kết trực tiếp: AICharacterManager, PlayerManager, WorldUtilityManager. | AICharacterManager, PlayerManager, WorldUtilityManager |
| `private void OnDrawGizmosSelected()` | 47 | Thực hiện logic on draw gizmos selected trong script EventTriggerWakeNearbyCharacters. | - |

#### FadeLoadingIcon

- **Đường dẫn:** `Assets/Game/Scripts/Function/FadeLoadingIcon.cs`
- **Loại:** MonoBehaviour
- **Vai trò dễ hiểu:** Script chức năng gắn vào object trong scene: interactable, trigger, pickup, elevator, fog wall, dialogue hoặc reset animation flag.
- **Kế thừa/cha:** MonoBehaviour
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** -

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `private void OnEnable()` | 12 | Đăng ký event/callback hoặc bật trạng thái khi component được enable. | - |
| `private void OnDisable()` | 17 | Gỡ event/callback hoặc dọn trạng thái khi component bị disable. | - |
| `public void FadeUIImage()` | 23 | Thực hiện logic fade uiimage trong script FadeLoadingIcon. | - |
| `private IEnumerator FadeCoroutine(bool fadeAway)` | 31 | Thực hiện logic fade coroutine trong script FadeLoadingIcon. | - |
| `new Color(1, 1, 1, i)` | 37 | Thực hiện logic color trong script FadeLoadingIcon. | - |
| `new Color(1, 1, 1, i)` | 47 | Thực hiện logic color trong script FadeLoadingIcon. | - |

#### FogWallInteractable

- **Đường dẫn:** `Assets/Game/Scripts/Function/FogWallInteractable.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Script chức năng gắn vào object trong scene: interactable, trigger, pickup, elevator, fog wall, dialogue hoặc reset animation flag.
- **Kế thừa/cha:** Interactable
- **Script con:** -
- **Field public/serialized chính:** public int fogWallID, public NetworkVariable<bool> isActive
- **Liên kết script:** Interactable, PlayerManager, WorldObjectManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `protected override void Awake()` | 28 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. | - |
| `private void OnValidate()` | 36 | Tự kiểm tra/cập nhật giá trị trong Unity Editor khi inspector thay đổi. | - |
| `private void AutoAssignFogWallIDFromWorldScene()` | 41 | Thực hiện logic auto assign fog wall idfrom world scene trong script FogWallInteractable. | - |
| `public override void Interact(PlayerManager player)` | 51 | Thực hiện hành động tương tác khi player chọn object này. | - |
| `public override void OnNetworkSpawn()` | 68 | Thiết lập dữ liệu và callback khi NetworkObject được spawn trong Netcode. Liên kết trực tiếp: WorldObjectManager. | WorldObjectManager |
| `public override void OnNetworkDespawn()` | 77 | Dọn dữ liệu/callback khi NetworkObject bị despawn khỏi Netcode. Liên kết trực tiếp: WorldObjectManager. | WorldObjectManager |
| `private void OnIsActiveChanged(bool oldStatus, bool newStatus)` | 85 | Thực hiện logic on is active changed trong script FogWallInteractable. | - |
| `private void AllowPlayerThroughFogWallCollidersServerRpc(ulong playerObjectID)` | 106 | Gửi yêu cầu lên server trong Netcode để server xử lý allow player through fog wall colliders. | - |
| `private void AllowPlayerThroughFogWallCollidersClientRpc(ulong playerObjectID)` | 115 | Server gọi xuống client để đồng bộ hoặc phát hiệu ứng cho allow player through fog wall colliders. Liên kết trực tiếp: PlayerManager. | PlayerManager |
| `private IEnumerator DisableCollisionForTime(PlayerManager player)` | 129 | Tắt collision for time. | - |
| `new WaitForSeconds(3)` | 133 | Thực hiện logic wait for seconds trong script FogWallInteractable. | - |
| `private IEnumerator ReEnableInteractionAfterPassThrough()` | 138 | Thực hiện logic re enable interaction after pass through trong script FogWallInteractable. | - |
| `new WaitForSeconds(3)` | 140 | Thực hiện logic wait for seconds trong script FogWallInteractable. | - |
| `private IEnumerator MovePlayerThroughFogWall(PlayerManager player, Vector3 passDirection)` | 148 | Thực hiện logic move player through fog wall trong script FogWallInteractable. | - |
| `private Quaternion GetPassThroughRotation(PlayerManager player)` | 175 | Lấy dữ liệu pass through rotation cho hệ thống khác sử dụng. | - |
| `private Vector3 GetPassDirection(PlayerManager player)` | 181 | Lấy dữ liệu pass direction cho hệ thống khác sử dụng. | - |

#### Interactable

- **Đường dẫn:** `Assets/Game/Scripts/Function/Interactable.cs`
- **Loại:** NetworkBehaviour
- **Vai trò dễ hiểu:** Script chức năng gắn vào object trong scene: interactable, trigger, pickup, elevator, fog wall, dialogue hoặc reset animation flag.
- **Kế thừa/cha:** NetworkBehaviour
- **Script con:** AnvilInteractable, CallElevatorInteractable, DialogueInteractable, ElevatorInteractable, FogWallInteractable, PickUpItemInteractable, PickUpRunesInteractable, ShopInteractable, SiteOfGraceInteractable, WorldMapTransitionInteractable
- **Field public/serialized chính:** public string interactableText, [SerializeField] protected Collider interactableCollider, [SerializeField] protected bool hostOnlyInteractable
- **Liên kết script:** PlayerManager, PlayerUIManager, WorldSaveGameManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `protected virtual void Awake()` | 12 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. | - |
| `protected virtual void Start()` | 18 | Chạy sau Awake để hoàn tất thiết lập ban đầu, nhất là khi cần các object khác đã sẵn sàng. | - |
| `public virtual void Interact(PlayerManager player)` | 23 | Thực hiện hành động tương tác khi player chọn object này. Liên kết trực tiếp: PlayerUIManager, WorldSaveGameManager. | PlayerUIManager, WorldSaveGameManager |
| `public virtual void OnTriggerEnter(Collider other)` | 37 | Xử lý khi collider khác đi vào trigger của object này. Liên kết trực tiếp: PlayerManager. | PlayerManager |
| `public virtual void OnTriggerExit(Collider other)` | 51 | Xử lý khi collider khác rời khỏi trigger của object này. Liên kết trực tiếp: PlayerManager, PlayerUIManager. | PlayerManager, PlayerUIManager |
| `public Collider GetInteractableCollider()` | 66 | Lấy dữ liệu interactable collider cho hệ thống khác sử dụng. | - |

#### IsOnElevatorTrigger

- **Đường dẫn:** `Assets/Game/Scripts/Function/IsOnElevatorTrigger.cs`
- **Loại:** MonoBehaviour
- **Vai trò dễ hiểu:** Script chức năng gắn vào object trong scene: interactable, trigger, pickup, elevator, fog wall, dialogue hoặc reset animation flag.
- **Kế thừa/cha:** MonoBehaviour
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** CharacterManager, ElevatorInteractable

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `private void OnTriggerEnter(Collider other)` | 9 | Xử lý khi collider khác đi vào trigger của object này. Liên kết trực tiếp: CharacterManager. | CharacterManager |
| `private void OnTriggerExit(Collider other)` | 17 | Xử lý khi collider khác rời khỏi trigger của object này. Liên kết trực tiếp: CharacterManager. | CharacterManager |

#### LockOnTransform

- **Đường dẫn:** `Assets/Game/Scripts/Function/LockOnTransform.cs`
- **Loại:** MonoBehaviour
- **Vai trò dễ hiểu:** Script chức năng gắn vào object trong scene: interactable, trigger, pickup, elevator, fog wall, dialogue hoặc reset animation flag.
- **Kế thừa/cha:** MonoBehaviour
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** -

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| - | - | Script này chủ yếu là dữ liệu/enum/struct, không có method rõ ràng. | - |

#### NetworkObjectSpawner

- **Đường dẫn:** `Assets/Game/Scripts/Function/NetworkObjectSpawner.cs`
- **Loại:** MonoBehaviour
- **Vai trò dễ hiểu:** Script chức năng gắn vào object trong scene: interactable, trigger, pickup, elevator, fog wall, dialogue hoặc reset animation flag.
- **Kế thừa/cha:** MonoBehaviour
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** WorldObjectManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `private void Awake()` | 13 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. | - |
| `private void Start()` | 17 | Chạy sau Awake để hoàn tất thiết lập ban đầu, nhất là khi cần các object khác đã sẵn sàng. Liên kết trực tiếp: WorldObjectManager. | WorldObjectManager |
| `public void AttemptToSpawnCharacter()` | 23 | Cố gắng kích hoạt to spawn character nếu trạng thái hiện tại cho phép. | - |

#### PickUpItemInteractable

- **Đường dẫn:** `Assets/Game/Scripts/Function/PickUpItemInteractable.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Script chức năng gắn vào object trong scene: interactable, trigger, pickup, elevator, fog wall, dialogue hoặc reset animation flag.
- **Kế thừa/cha:** Interactable
- **Script con:** -
- **Field public/serialized chính:** public ItemPickUpType pickUpType, public NetworkVariable<int> itemID, public NetworkVariable<Vector3> networkPosition, public NetworkVariable<ulong> droppingCreatureID, public NetworkVariable<ulong> allowedLooterClientId, public NetworkVariable<bool> isSharedLoot, public NetworkVariable<bool> isLooted, public bool trackDroppingCreaturesPosition
- **Liên kết script:** AICharacterManager, CharacterSlot, Interactable, Item, ItemPickUpType, PlayerManager, PlayerUIManager, WorldGameSessionManager, WorldItemDatabase, WorldSaveGameManager, WorldSoundFXManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `protected override void Awake()` | 31 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. | - |
| `protected override void Start()` | 38 | Chạy sau Awake để hoàn tất thiết lập ban đầu, nhất là khi cần các object khác đã sẵn sàng. Liên kết trực tiếp: ItemPickUpType. | ItemPickUpType |
| `public override void OnNetworkSpawn()` | 46 | Thiết lập dữ liệu và callback khi NetworkObject được spawn trong Netcode. Liên kết trực tiếp: ItemPickUpType. | ItemPickUpType |
| `public override void OnNetworkDespawn()` | 68 | Dọn dữ liệu/callback khi NetworkObject bị despawn khỏi Netcode. | - |
| `private void InitializeWorldSpawnLootState()` | 78 | Thực hiện logic initialize world spawn loot state trong script PickUpItemInteractable. Liên kết trực tiếp: WorldSaveGameManager. | WorldSaveGameManager |
| `public override void Interact(PlayerManager player)` | 92 | Thực hiện hành động tương tác khi player chọn object này. Liên kết trực tiếp: PlayerUIManager. | PlayerUIManager |
| `public override void OnTriggerEnter(Collider other)` | 116 | Xử lý khi collider khác đi vào trigger của object này. Liên kết trực tiếp: PlayerManager. | PlayerManager |
| `public override void OnTriggerExit(Collider other)` | 129 | Xử lý khi collider khác rời khỏi trigger của object này. Liên kết trực tiếp: PlayerManager, PlayerUIManager. | PlayerManager, PlayerUIManager |
| `protected void OnItemIDChanged(int oldValue, int newValue)` | 140 | Thực hiện logic on item idchanged trong script PickUpItemInteractable. Liên kết trực tiếp: ItemPickUpType, WorldItemDatabase. | ItemPickUpType, WorldItemDatabase |
| `protected void OnNetworkPositionChanged(Vector3 oldPosition, Vector3 newPosition)` | 148 | Thực hiện logic on network position changed trong script PickUpItemInteractable. Liên kết trực tiếp: ItemPickUpType. | ItemPickUpType |
| `protected void OnDroppingCreaturesIDChanged(ulong oldID, ulong newID)` | 156 | Thực hiện logic on dropping creatures idchanged trong script PickUpItemInteractable. Liên kết trực tiếp: ItemPickUpType. | ItemPickUpType |
| `protected IEnumerator TrackDroppingCreaturesPosition()` | 165 | Thực hiện logic track dropping creatures position trong script PickUpItemInteractable. Liên kết trực tiếp: AICharacterManager. | AICharacterManager |
| `private bool CanBeLootedBy(PlayerManager player)` | 185 | Kiểm tra có được phép be looted by hay không. Liên kết trực tiếp: ItemPickUpType. | ItemPickUpType |
| `private void OnIsLootedChanged(bool oldValue, bool newValue)` | 199 | Thực hiện logic on is looted changed trong script PickUpItemInteractable. Liên kết trực tiếp: ItemPickUpType. | ItemPickUpType |
| `private void RequestPickupServerRpc(ServerRpcParams serverRpcParams = default)` | 208 | Gửi yêu cầu lên server trong Netcode để server xử lý request pickup. | - |
| `private void CompletePickupOnServer(ulong looterClientId)` | 213 | Thực hiện logic complete pickup on server trong script PickUpItemInteractable. Liên kết trực tiếp: ItemPickUpType, PlayerManager, WorldGameSessionManager, WorldSaveGameManager. | ItemPickUpType, PlayerManager, WorldGameSessionManager, WorldSaveGameManager |
| `private void GrantPickedUpItemClientRpc(int grantedItemID, ClientRpcParams clientRpcParams = default)` | 250 | Server gọi xuống client để đồng bộ hoặc phát hiệu ứng cho grant picked up item. Liên kết trực tiếp: CharacterSlot, Item, ItemPickUpType, PlayerManager, PlayerUIManager +3. | CharacterSlot, Item, ItemPickUpType, PlayerManager, PlayerUIManager, WorldItemDatabase, WorldSaveGameManager, WorldSoundFXManager |
| `protected void DestroyThisNetworkObjectServerRpc()` | 289 | Gửi yêu cầu lên server trong Netcode để server xử lý destroy this network object. | - |

#### PickUpRunesInteractable

- **Đường dẫn:** `Assets/Game/Scripts/Function/PickUpRunesInteractable.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Script chức năng gắn vào object trong scene: interactable, trigger, pickup, elevator, fog wall, dialogue hoặc reset animation flag.
- **Kế thừa/cha:** Interactable
- **Script con:** -
- **Field public/serialized chính:** public NetworkVariable<int> runeCount, public NetworkVariable<ulong> runeOwnerClientId
- **Liên kết script:** CharacterSlot, Interactable, PlayerManager, PlayerUIManager, WorldSaveGameManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public override void OnTriggerEnter(Collider other)` | 17 | Xử lý khi collider khác đi vào trigger của object này. Liên kết trực tiếp: PlayerManager. | PlayerManager |
| `public override void OnTriggerExit(Collider other)` | 30 | Xử lý khi collider khác rời khỏi trigger của object này. Liên kết trực tiếp: PlayerManager. | PlayerManager |
| `public override void Interact(PlayerManager player)` | 43 | Thực hiện hành động tương tác khi player chọn object này. Liên kết trực tiếp: PlayerUIManager. | PlayerUIManager |
| `private void RequestRunePickupServerRpc(ServerRpcParams serverRpcParams = default)` | 65 | Gửi yêu cầu lên server trong Netcode để server xử lý request rune pickup. | - |
| `private void CompleteRunePickupOnServer(ulong looterClientId)` | 70 | Thực hiện logic complete rune pickup on server trong script PickUpRunesInteractable. | - |
| `private void GrantRunesClientRpc(int grantedRunes, ClientRpcParams clientRpcParams = default)` | 88 | Server gọi xuống client để đồng bộ hoặc phát hiệu ứng cho grant runes. Liên kết trực tiếp: CharacterSlot, PlayerManager, WorldSaveGameManager. | CharacterSlot, PlayerManager, WorldSaveGameManager |

#### ResetActionFlag

- **Đường dẫn:** `Assets/Game/Scripts/Function/ResetActionFlag.cs`
- **Loại:** Animator state
- **Vai trò dễ hiểu:** Script chức năng gắn vào object trong scene: interactable, trigger, pickup, elevator, fog wall, dialogue hoặc reset animation flag.
- **Kế thừa/cha:** StateMachineBehaviour
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** CharacterManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)` | 11 | Thực hiện logic on state enter trong script ResetActionFlag. Liên kết trực tiếp: CharacterManager. | CharacterManager |

#### ResetIsChugging

- **Đường dẫn:** `Assets/Game/Scripts/Function/ResetIsChugging.cs`
- **Loại:** Animator state
- **Vai trò dễ hiểu:** Script chức năng gắn vào object trong scene: interactable, trigger, pickup, elevator, fog wall, dialogue hoặc reset animation flag.
- **Kế thừa/cha:** StateMachineBehaviour
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** FlaskItem, PlayerManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)` | 10 | Thực hiện logic on state enter trong script ResetIsChugging. Liên kết trực tiếp: FlaskItem, PlayerManager. | FlaskItem, PlayerManager |

#### ResetUpperBodyAction

- **Đường dẫn:** `Assets/Game/Scripts/Function/ResetUpperBodyAction.cs`
- **Loại:** Animator state
- **Vai trò dễ hiểu:** Script chức năng gắn vào object trong scene: interactable, trigger, pickup, elevator, fog wall, dialogue hoặc reset animation flag.
- **Kế thừa/cha:** StateMachineBehaviour
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** PlayerManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)` | 9 | Thực hiện logic on state enter trong script ResetUpperBodyAction. Liên kết trực tiếp: PlayerManager. | PlayerManager |

#### SiteOfGraceInteractable

- **Đường dẫn:** `Assets/Game/Scripts/Function/SiteOfGraceInteractable.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Script chức năng gắn vào object trong scene: interactable, trigger, pickup, elevator, fog wall, dialogue hoặc reset animation flag.
- **Kế thừa/cha:** Interactable
- **Script con:** -
- **Field public/serialized chính:** public int siteOfGraceID, public NetworkVariable<bool> isActivated
- **Liên kết script:** CharacterSlot, Interactable, PlayerManager, PlayerUIManager, WorldAIManager, WorldGameSessionManager, WorldObjectManager, WorldSaveGameManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `protected override void Start()` | 26 | Chạy sau Awake để hoàn tất thiết lập ban đầu, nhất là khi cần các object khác đã sẵn sàng. Liên kết trực tiếp: WorldSaveGameManager. | WorldSaveGameManager |
| `public override void OnNetworkSpawn()` | 52 | Thiết lập dữ liệu và callback khi NetworkObject được spawn trong Netcode. Liên kết trực tiếp: WorldObjectManager. | WorldObjectManager |
| `public override void OnNetworkDespawn()` | 65 | Dọn dữ liệu/callback khi NetworkObject bị despawn khỏi Netcode. | - |
| `private void RestoreSiteOfGrace(PlayerManager player)` | 72 | Thực hiện logic restore site of grace trong script SiteOfGraceInteractable. Liên kết trực tiếp: WorldSaveGameManager. | WorldSaveGameManager |
| `private void RestAtSiteOfGrace(PlayerManager player)` | 85 | Thực hiện logic rest at site of grace trong script SiteOfGraceInteractable. Liên kết trực tiếp: WorldAIManager. | WorldAIManager |
| `private void CompleteGraceActivationLocally(PlayerManager player)` | 93 | Thực hiện logic complete grace activation locally trong script SiteOfGraceInteractable. Liên kết trực tiếp: CharacterSlot, PlayerUIManager, WorldSaveGameManager. | CharacterSlot, PlayerUIManager, WorldSaveGameManager |
| `private IEnumerator WaitForAnimationAndPopUpThenRestoreCollider()` | 116 | Thực hiện logic wait for animation and pop up then restore collider trong script SiteOfGraceInteractable. | - |
| `new WaitForSeconds(2)` | 118 | Thực hiện logic wait for seconds trong script SiteOfGraceInteractable. | - |
| `private void CompleteRestAtSiteOfGraceLocally(PlayerManager player)` | 122 | Thực hiện logic complete rest at site of grace locally trong script SiteOfGraceInteractable. Liên kết trực tiếp: CharacterSlot, PlayerUIManager, WorldSaveGameManager. | CharacterSlot, PlayerUIManager, WorldSaveGameManager |
| `private void OnIsActivatedChanged(bool oldStatus, bool newStatus)` | 146 | Thực hiện logic on is activated changed trong script SiteOfGraceInteractable. | - |
| `public override void Interact(PlayerManager player)` | 160 | Thực hiện hành động tương tác khi player chọn object này. Liên kết trực tiếp: PlayerUIManager, WorldSaveGameManager. | PlayerUIManager, WorldSaveGameManager |
| `private void ProcessGraceInteractionServerRpc(ServerRpcParams serverRpcParams = default)` | 186 | Gửi yêu cầu lên server trong Netcode để server xử lý process grace interaction. | - |
| `private void ProcessGraceInteractionOnServer(ulong playerClientId)` | 191 | Thực hiện logic process grace interaction on server trong script SiteOfGraceInteractable. Liên kết trực tiếp: PlayerManager, WorldAIManager, WorldGameSessionManager, WorldSaveGameManager. | PlayerManager, WorldAIManager, WorldGameSessionManager, WorldSaveGameManager |
| `private void CompleteGraceActivationClientRpc(int activatedSiteOfGraceID, ClientRpcParams clientRpcParams = default)` | 233 | Server gọi xuống client để đồng bộ hoặc phát hiệu ứng cho complete grace activation. Liên kết trực tiếp: PlayerManager. | PlayerManager |
| `private void CompleteGraceRestClientRpc(int targetSiteOfGraceID, ClientRpcParams clientRpcParams = default)` | 249 | Server gọi xuống client để đồng bộ hoặc phát hiệu ứng cho complete grace rest. Liên kết trực tiếp: PlayerManager. | PlayerManager |
| `private void playerNetworkManagerLastGrace(PlayerManager player, int targetSiteOfGraceID)` | 264 | Thực hiện logic player network manager last grace trong script SiteOfGraceInteractable. Liên kết trực tiếp: WorldSaveGameManager. | WorldSaveGameManager |
| `public void TeleportToSiteOfGrace()` | 272 | Thực hiện logic teleport to site of grace trong script SiteOfGraceInteractable. Liên kết trực tiếp: PlayerManager. | PlayerManager |
| `public void TeleportPlayerToSiteOfGrace(PlayerManager player, bool handleLoadingScreen = true)` | 279 | Thực hiện logic teleport player to site of grace trong script SiteOfGraceInteractable. Liên kết trực tiếp: PlayerUIManager. | PlayerUIManager |

#### StealthObject

- **Đường dẫn:** `Assets/Game/Scripts/Function/StealthObject.cs`
- **Loại:** MonoBehaviour
- **Vai trò dễ hiểu:** Script chức năng gắn vào object trong scene: interactable, trigger, pickup, elevator, fog wall, dialogue hoặc reset animation flag.
- **Kế thừa/cha:** MonoBehaviour
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** CharacterManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `private void OnTriggerEnter(Collider other)` | 12 | Xử lý khi collider khác đi vào trigger của object này. Liên kết trực tiếp: CharacterManager. | CharacterManager |
| `private void OnTriggerExit(Collider other)` | 22 | Xử lý khi collider khác rời khỏi trigger của object này. Liên kết trực tiếp: CharacterManager. | CharacterManager |
| `private void OnDisable()` | 32 | Gỡ event/callback hoặc dọn trạng thái khi component bị disable. | - |
| `private void AddCharacterToStealthObject(CharacterManager character)` | 43 | Thêm character to stealth object vào danh sách, trạng thái hoặc dữ liệu. | - |
| `private void RemoveCharacterFromStealthObject(CharacterManager character)` | 61 | Loại bỏ character from stealth object khỏi danh sách, trạng thái hoặc dữ liệu. | - |

#### TitleMenuPlayerPreviewRotator

- **Đường dẫn:** `Assets/Game/Scripts/Function/TitleMenuPlayerPreviewRotator.cs`
- **Loại:** MonoBehaviour
- **Vai trò dễ hiểu:** Script chức năng gắn vào object trong scene: interactable, trigger, pickup, elevator, fog wall, dialogue hoặc reset animation flag.
- **Kế thừa/cha:** MonoBehaviour
- **Script con:** -
- **Field public/serialized chính:** [SerializeField] private Vector2 cameraInput, [SerializeField] private float horizontalInput, [SerializeField] private float lookAngle, [SerializeField] private float rotationSpeed
- **Liên kết script:** PlayerCamera

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `private void OnEnable()` | 17 | Đăng ký event/callback hoặc bật trạng thái khi component được enable. Liên kết trực tiếp: PlayerCamera. | PlayerCamera |
| `new PlayerControls()` | 21 | Phát er controls, thường là animation, sound hoặc VFX. | - |
| `private void OnDisable()` | 29 | Gỡ event/callback hoặc dọn trạng thái khi component bị disable. | - |
| `private void Update()` | 34 | Cập nhật logic mỗi frame như input, timer, trạng thái chiến đấu hoặc UI. | - |

#### ToggleBlockingController

- **Đường dẫn:** `Assets/Game/Scripts/Function/ToggleBlockingController.cs`
- **Loại:** Animator state
- **Vai trò dễ hiểu:** Script chức năng gắn vào object trong scene: interactable, trigger, pickup, elevator, fog wall, dialogue hoặc reset animation flag.
- **Kế thừa/cha:** StateMachineBehaviour
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** PlayerManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)` | 9 | Thực hiện logic on state enter trong script ToggleBlockingController. Liên kết trực tiếp: PlayerManager. | PlayerManager |
| `public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)` | 34 | Thực hiện logic on state exit trong script ToggleBlockingController. Liên kết trực tiếp: PlayerManager. | PlayerManager |

#### ToggleNotchedArrowMovement

- **Đường dẫn:** `Assets/Game/Scripts/Function/ToggleNotchedArrowMovement.cs`
- **Loại:** Animator state
- **Vai trò dễ hiểu:** Script chức năng gắn vào object trong scene: interactable, trigger, pickup, elevator, fog wall, dialogue hoặc reset animation flag.
- **Kế thừa/cha:** StateMachineBehaviour
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** PlayerManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)` | 11 | Thực hiện logic on state enter trong script ToggleNotchedArrowMovement. Liên kết trực tiếp: PlayerManager. | PlayerManager |

#### WeaponModelInstantiationSlot

- **Đường dẫn:** `Assets/Game/Scripts/Function/WeaponModelInstantiationSlot.cs`
- **Loại:** MonoBehaviour
- **Vai trò dễ hiểu:** Script chức năng gắn vào object trong scene: interactable, trigger, pickup, elevator, fog wall, dialogue hoặc reset animation flag.
- **Kế thừa/cha:** MonoBehaviour
- **Script con:** -
- **Field public/serialized chính:** public WeaponModelSlot weaponSlot, public GameObject currentWeaponModel
- **Liên kết script:** PlayerManager, WeaponClass, WeaponModelSlot

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public void UnloadWeapon()` | 10 | Thực hiện logic unload weapon trong script WeaponModelInstantiationSlot. | - |
| `public void PlaceWeaponModelIntoSlot(GameObject weaponModel)` | 19 | Thực hiện logic place weapon model into slot trong script WeaponModelInstantiationSlot. | - |
| `public void PlaceWeaponModelInUnequippedSlot(GameObject weaponModel, WeaponClass weaponClass, PlayerManager player)` | 25 | Thực hiện logic place weapon model in unequipped slot trong script WeaponModelInstantiationSlot. Liên kết trực tiếp: WeaponClass. | WeaponClass |
| `new Vector3(0.064f, 0f, -0.06f)` | 33 | Thực hiện logic vector3 trong script WeaponModelInstantiationSlot. | - |
| `new Vector3(0.064f, 0f, -0.06f)` | 37 | Thực hiện logic vector3 trong script WeaponModelInstantiationSlot. | - |
| `new Vector3(0.074f, -0.002f, 0.069f)` | 41 | Thực hiện logic vector3 trong script WeaponModelInstantiationSlot. | - |

#### WorldMapTransitionInteractable

- **Đường dẫn:** `Assets/Game/Scripts/Function/WorldMapTransitionInteractable.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Script chức năng gắn vào object trong scene: interactable, trigger, pickup, elevator, fog wall, dialogue hoặc reset animation flag.
- **Kế thừa/cha:** Interactable
- **Script con:** -
- **Field public/serialized chính:** [SerializeField] private int targetMapIndex, [SerializeField] private string transitionText
- **Liên kết script:** GameProgressionManager, Interactable, PlayerManager, PlayerUIManager, WorldSaveGameManager, WorldSceneManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `protected override void Awake()` | 13 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. | - |
| `protected override void Start()` | 19 | Chạy sau Awake để hoàn tất thiết lập ban đầu, nhất là khi cần các object khác đã sẵn sàng. | - |
| `public override void Interact(PlayerManager player)` | 25 | Thực hiện hành động tương tác khi player chọn object này. Liên kết trực tiếp: GameProgressionManager, PlayerUIManager, WorldSaveGameManager, WorldSceneManager. | GameProgressionManager, PlayerUIManager, WorldSaveGameManager, WorldSceneManager |
| `private void RequestWorldSceneTransitionServerRpc(int sceneBuildIndex)` | 65 | Gửi yêu cầu lên server trong Netcode để server xử lý request world scene transition. Liên kết trực tiếp: WorldSceneManager. | WorldSceneManager |

### Assets/Game/Scripts/Game Saving

#### CharacterSaveData

- **Đường dẫn:** `Assets/Game/Scripts/Game Saving/CharacterSaveData.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Dữ liệu/helper phục vụ lưu và tải game.
- **Kế thừa/cha:** -
- **Script con:** -
- **Field public/serialized chính:** public int sceneIndex, public int startingClassID, public int currentMapIndex, public bool gameWon, public string characterName, public bool hasDeadSpot, public float deadSpotPositionX, public float deadSpotPositionY, public float deadSpotPositionZ, public int deadSpotRuneCount, public bool isMale, public int hairStyleID +50
- **Liên kết script:** SerializableActiveBuff, SerializableDictionary, SerializableQuickSlotItem, SerializableRangedProjectile, SerializableWeapon

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public CharacterSaveData()` | 119 | Thực hiện logic character save data trong script CharacterSaveData. | - |
| `public void EnsureCollectionsInitialized()` | 124 | Thực hiện logic ensure collections initialized trong script CharacterSaveData. Liên kết trực tiếp: SerializableActiveBuff, SerializableDictionary, SerializableQuickSlotItem, SerializableRangedProjectile, SerializableWeapon. | SerializableActiveBuff, SerializableDictionary, SerializableQuickSlotItem, SerializableRangedProjectile, SerializableWeapon |

#### SaveFileDataWriter

- **Đường dẫn:** `Assets/Game/Scripts/Game Saving/SaveFileDataWriter.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Dữ liệu/helper phục vụ lưu và tải game.
- **Kế thừa/cha:** -
- **Script con:** -
- **Field public/serialized chính:** public string saveDataDirectoryPath, public string saveFilename
- **Liên kết script:** CharacterSaveData

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public bool CheckToSeeIfFileExists()` | 13 | Thực hiện logic check to see if file exists trong script SaveFileDataWriter. | - |
| `public void DeleteSaveFile()` | 26 | Thực hiện logic delete save file trong script SaveFileDataWriter. | - |
| `public void CreateNewCharacterSaveFile(CharacterSaveData characterData)` | 33 | Tạo object/dữ liệu new character save file. | - |
| `new FileStream(savePath, FileMode.Create))` | 48 | Thực hiện logic file stream trong script SaveFileDataWriter. | - |
| `new StreamWriter(stream))` | 50 | Thực hiện logic stream writer trong script SaveFileDataWriter. | - |
| `public CharacterSaveData LoadSaveFile()` | 63 | Nạp dữ liệu hoặc scene liên quan tới save file. Liên kết trực tiếp: CharacterSaveData. | CharacterSaveData |
| `new FileStream(loadPath, FileMode.Open))` | 75 | Thực hiện logic file stream trong script SaveFileDataWriter. | - |
| `new StreamReader(stream))` | 77 | Thực hiện logic stream reader trong script SaveFileDataWriter. | - |

#### SerializableActiveBuff

- **Đường dẫn:** `Assets/Game/Scripts/Game Saving/SerializableActiveBuff.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Dữ liệu/helper phục vụ lưu và tải game.
- **Kế thừa/cha:** -
- **Script con:** -
- **Field public/serialized chính:** [SerializeField] public int sourceItemID, [SerializeField] public float timeRemaining
- **Liên kết script:** -

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| - | - | Script này chủ yếu là dữ liệu/enum/struct, không có method rõ ràng. | - |

#### SerializableDictionary

- **Đường dẫn:** `Assets/Game/Scripts/Game Saving/SerializableDictionary.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Dữ liệu/helper phục vụ lưu và tải game.
- **Kế thừa/cha:** -
- **Script con:** -
- **Field public/serialized chính:** [SerializeField] private List<Tkey> keys, [SerializeField] private List<TValue> values
- **Liên kết script:** -

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public void OnBeforeSerialize()` | 9 | Thực hiện logic on before serialize trong script SerializableDictionary. | - |
| `public void OnAfterDeserialize()` | 21 | Thực hiện logic on after deserialize trong script SerializableDictionary. | - |

#### SerializableFlask

- **Đường dẫn:** `Assets/Game/Scripts/Game Saving/SerializableFlask.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Dữ liệu/helper phục vụ lưu và tải game.
- **Kế thừa/cha:** ISerializationCallbackReceiver
- **Script con:** -
- **Field public/serialized chính:** [SerializeField] public int itemID
- **Liên kết script:** FlaskItem, WorldItemDatabase

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public FlaskItem GetFlask()` | 12 | Lấy dữ liệu flask cho hệ thống khác sử dụng. Liên kết trực tiếp: FlaskItem, WorldItemDatabase. | FlaskItem, WorldItemDatabase |
| `public void OnAfterDeserialize()` | 18 | Thực hiện logic on after deserialize trong script SerializableFlask. | - |
| `public void OnBeforeSerialize()` | 23 | Thực hiện logic on before serialize trong script SerializableFlask. | - |

#### SerializableQuickSlotItem

- **Đường dẫn:** `Assets/Game/Scripts/Game Saving/SerializableQuickSlotItem.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Dữ liệu/helper phục vụ lưu và tải game.
- **Kế thừa/cha:** ISerializationCallbackReceiver
- **Script con:** -
- **Field public/serialized chính:** [SerializeField] public int itemID, [SerializeField] public int itemAmount
- **Liên kết script:** QuickSlotItem, WorldItemDatabase

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public QuickSlotItem GetQuickSlotItem()` | 14 | Lấy dữ liệu quick slot item cho hệ thống khác sử dụng. Liên kết trực tiếp: QuickSlotItem, WorldItemDatabase. | QuickSlotItem, WorldItemDatabase |
| `public void OnAfterDeserialize()` | 20 | Thực hiện logic on after deserialize trong script SerializableQuickSlotItem. | - |
| `public void OnBeforeSerialize()` | 25 | Thực hiện logic on before serialize trong script SerializableQuickSlotItem. | - |

#### SerializableRangedProjectile

- **Đường dẫn:** `Assets/Game/Scripts/Game Saving/SerializableRangedProjectile.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Dữ liệu/helper phục vụ lưu và tải game.
- **Kế thừa/cha:** ISerializationCallbackReceiver
- **Script con:** -
- **Field public/serialized chính:** [SerializeField] public int itemID, [SerializeField] public int itemAmount
- **Liên kết script:** RangedProjectileItem, WorldItemDatabase

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public RangedProjectileItem GetProjectile()` | 11 | Lấy dữ liệu projectile cho hệ thống khác sử dụng. Liên kết trực tiếp: RangedProjectileItem, WorldItemDatabase. | RangedProjectileItem, WorldItemDatabase |
| `public void OnAfterDeserialize()` | 17 | Thực hiện logic on after deserialize trong script SerializableRangedProjectile. | - |
| `public void OnBeforeSerialize()` | 22 | Thực hiện logic on before serialize trong script SerializableRangedProjectile. | - |

#### SerializableWeapon

- **Đường dẫn:** `Assets/Game/Scripts/Game Saving/SerializableWeapon.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Dữ liệu/helper phục vụ lưu và tải game.
- **Kế thừa/cha:** ISerializationCallbackReceiver
- **Script con:** -
- **Field public/serialized chính:** [SerializeField] public int itemID, [SerializeField] public int upgradeLevel, [SerializeField] public int ashOfWarID
- **Liên kết script:** WeaponItem, WorldItemDatabase

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public WeaponItem GetWeapon()` | 12 | Lấy dữ liệu weapon cho hệ thống khác sử dụng. Liên kết trực tiếp: WeaponItem, WorldItemDatabase. | WeaponItem, WorldItemDatabase |
| `public void OnAfterDeserialize()` | 18 | Thực hiện logic on after deserialize trong script SerializableWeapon. | - |
| `public void OnBeforeSerialize()` | 23 | Thực hiện logic on before serialize trong script SerializableWeapon. | - |

### Assets/Game/Scripts/Items

#### AshOfWar

- **Đường dẫn:** `Assets/Game/Scripts/Items/Ashes Of War/AshOfWar.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Dữ liệu hoặc hành vi item: weapon, armor, spell, flask, quick slot, projectile hoặc material.
- **Kế thừa/cha:** Item
- **Script con:** ParryAshOfWar
- **Field public/serialized chính:** public WeaponClass[] usableWeaponClasses, public int focusPointCost, public int staminaCost
- **Liên kết script:** Item, PlayerManager, WeaponClass

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public virtual void AttemptToPerformAction(PlayerManager playerPerformingAction)` | 14 | Cố gắng kích hoạt to perform action nếu trạng thái hiện tại cho phép. | - |
| `public virtual bool CanIUseThisAbility(PlayerManager playerPerformingAbility)` | 19 | Kiểm tra có được phép iuse this ability hay không. | - |
| `protected virtual void DeductStaminaCost(PlayerManager playerPerformingAction)` | 24 | Thực hiện logic deduct stamina cost trong script AshOfWar. | - |
| `protected virtual void DeductFocusPointCost(PlayerManager playerPerformingAction)` | 29 | Thực hiện logic deduct focus point cost trong script AshOfWar. | - |

#### ParryAshOfWar

- **Đường dẫn:** `Assets/Game/Scripts/Items/Ashes Of War/ParryAshOfWar.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Dữ liệu hoặc hành vi item: weapon, armor, spell, flask, quick slot, projectile hoặc material.
- **Kế thừa/cha:** AshOfWar
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** AshOfWar, PlayerManager, WeaponClass, WeaponItem

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public override void AttemptToPerformAction(PlayerManager playerPerformingAction)` | 8 | Cố gắng kích hoạt to perform action nếu trạng thái hiện tại cho phép. | - |
| `public override bool CanIUseThisAbility(PlayerManager playerPerformingAction)` | 20 | Kiểm tra có được phép iuse this ability hay không. | - |
| `private void PerformParryTypeBasedOnWeapon(PlayerManager playerPerformingAction)` | 50 | Thực hiện logic perform parry type based on weapon trong script ParryAshOfWar. Liên kết trực tiếp: WeaponClass, WeaponItem. | WeaponClass, WeaponItem |

#### EquipmentModel

- **Đường dẫn:** `Assets/Game/Scripts/Items/Equipment Models/EquipmentModel.cs`
- **Loại:** ScriptableObject
- **Vai trò dễ hiểu:** Dữ liệu hoặc hành vi item: weapon, armor, spell, flask, quick slot, projectile hoặc material.
- **Kế thừa/cha:** ScriptableObject
- **Script con:** -
- **Field public/serialized chính:** public EquipmentModelType equipmentModelType, public string maleEquipmentName, public string femaleEquipmentName
- **Liên kết script:** EquipmentModelType, PlayerManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public void LoadModel(PlayerManager player, bool isMale)` | 12 | Nạp dữ liệu hoặc scene liên quan tới model. | - |
| `private void LoadMaleModel(PlayerManager player)` | 24 | Nạp dữ liệu hoặc scene liên quan tới male model. | - |
| `private void LoadFemaleModel(PlayerManager player)` | 29 | Nạp dữ liệu hoặc scene liên quan tới female model. | - |

#### ArmorItem

- **Đường dẫn:** `Assets/Game/Scripts/Items/Equipment/ArmorItem.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Dữ liệu hoặc hành vi item: weapon, armor, spell, flask, quick slot, projectile hoặc material.
- **Kế thừa/cha:** EquipmentItem
- **Script con:** BodyEquipmentItem, HandEquipmentItem, HeadEquipmentItem, LegEquipmentItem
- **Field public/serialized chính:** public float physicalDamageAbsorption, public float magicDamageAbsorption, public float fireDamageAbsorption, public float lightningDamageAbsorption, public float holyDamageAbsorption, public float immunity, public float robustness, public float focus, public float vitality, public float poise, public EquipmentModel[] equipmentModels
- **Liên kết script:** EquipmentItem, EquipmentModel

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| - | - | Script này chủ yếu là dữ liệu/enum/struct, không có method rõ ràng. | - |

#### BodyEquipmentItem

- **Đường dẫn:** `Assets/Game/Scripts/Items/Equipment/BodyEquipmentItem.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Dữ liệu hoặc hành vi item: weapon, armor, spell, flask, quick slot, projectile hoặc material.
- **Kế thừa/cha:** ArmorItem
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** ArmorItem

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| - | - | Script này chủ yếu là dữ liệu/enum/struct, không có method rõ ràng. | - |

#### EquipmentItem

- **Đường dẫn:** `Assets/Game/Scripts/Items/Equipment/EquipmentItem.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Dữ liệu hoặc hành vi item: weapon, armor, spell, flask, quick slot, projectile hoặc material.
- **Kế thừa/cha:** Item
- **Script con:** ArmorItem, WeaponItem
- **Field public/serialized chính:** public float itemWeight
- **Liên kết script:** Item

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| - | - | Script này chủ yếu là dữ liệu/enum/struct, không có method rõ ràng. | - |

#### HandEquipmentItem

- **Đường dẫn:** `Assets/Game/Scripts/Items/Equipment/HandEquipmentItem.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Dữ liệu hoặc hành vi item: weapon, armor, spell, flask, quick slot, projectile hoặc material.
- **Kế thừa/cha:** ArmorItem
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** ArmorItem

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| - | - | Script này chủ yếu là dữ liệu/enum/struct, không có method rõ ràng. | - |

#### HeadEquipmentItem

- **Đường dẫn:** `Assets/Game/Scripts/Items/Equipment/HeadEquipmentItem.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Dữ liệu hoặc hành vi item: weapon, armor, spell, flask, quick slot, projectile hoặc material.
- **Kế thừa/cha:** ArmorItem
- **Script con:** -
- **Field public/serialized chính:** public HeadEquipmentType headEquipmentType
- **Liên kết script:** ArmorItem, HeadEquipmentType

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| - | - | Script này chủ yếu là dữ liệu/enum/struct, không có method rõ ràng. | - |

#### LegEquipmentItem

- **Đường dẫn:** `Assets/Game/Scripts/Items/Equipment/LegEquipmentItem.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Dữ liệu hoặc hành vi item: weapon, armor, spell, flask, quick slot, projectile hoặc material.
- **Kế thừa/cha:** ArmorItem
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** ArmorItem

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| - | - | Script này chủ yếu là dữ liệu/enum/struct, không có method rõ ràng. | - |

#### RangedProjectileItem

- **Đường dẫn:** `Assets/Game/Scripts/Items/Equipment/RangedProjectileItem.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Dữ liệu hoặc hành vi item: weapon, armor, spell, flask, quick slot, projectile hoặc material.
- **Kế thừa/cha:** Item
- **Script con:** -
- **Field public/serialized chính:** public ProjectileClass projectileClass, public float forwardVelocity, public float upwardVelocity, public float ammoMass, public int maxAmmoAmount, public int currentAmmoAmount, public int physicalDamage, public int magicDamage, public int fireDamage, public int holyDamage, public int lightningDamage, public GameObject drawProjectileModel +1
- **Liên kết script:** Item, ProjectileClass

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| - | - | Script này chủ yếu là dữ liệu/enum/struct, không có method rõ ràng. | - |

#### FlaskItem

- **Đường dẫn:** `Assets/Game/Scripts/Items/Flask Items/FlaskItem.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Dữ liệu hoặc hành vi item: weapon, armor, spell, flask, quick slot, projectile hoặc material.
- **Kế thừa/cha:** QuickSlotItem
- **Script con:** -
- **Field public/serialized chính:** public bool healthFlask, public GameObject emptyFlaskItem, public string emptyFlaskAnimation
- **Liên kết script:** Item, PlayerManager, PlayerUIManager, QuickSlotItem, WorldCharacterEffectsManager, WorldSoundFXManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public override bool CanIUseThisItem(PlayerManager player)` | 19 | Kiểm tra có được phép iuse this item hay không. | - |
| `public override void AttemptToUseItem(PlayerManager player)` | 30 | Cố gắng kích hoạt to use item nếu trạng thái hiện tại cho phép. | - |
| `public override void SuccessfullyUseItem(PlayerManager player)` | 95 | Thực hiện logic successfully use item trong script FlaskItem. Liên kết trực tiếp: PlayerUIManager. | PlayerUIManager |
| `public override void PlayUseItemFX(PlayerManager player)` | 131 | Phát use item fx, thường là animation, sound hoặc VFX. Liên kết trực tiếp: WorldCharacterEffectsManager, WorldSoundFXManager. | WorldCharacterEffectsManager, WorldSoundFXManager |
| `public override int GetCurrentAmount(PlayerManager player)` | 137 | Lấy dữ liệu current amount cho hệ thống khác sử dụng. | - |

#### Item

- **Đường dẫn:** `Assets/Game/Scripts/Items/Item.cs`
- **Loại:** ScriptableObject
- **Vai trò dễ hiểu:** Dữ liệu hoặc hành vi item: weapon, armor, spell, flask, quick slot, projectile hoặc material.
- **Kế thừa/cha:** ScriptableObject
- **Script con:** AshOfWar, EquipmentItem, QuickSlotItem, RangedProjectileItem, SpellItem, UpgradeMaterial
- **Field public/serialized chính:** public string itemName, public Sprite itemIcon, public int maxItemAmount, public int currentItemAmount, public string itemDescription, public int itemID, public bool canBePurchased, public bool canBeSold, public int purchasePrice, public int sellPrice
- **Liên kết script:** -

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| - | - | Script này chủ yếu là dữ liệu/enum/struct, không có method rõ ràng. | - |

#### UpgradeMaterial

- **Đường dẫn:** `Assets/Game/Scripts/Items/Materials/UpgradeMaterial.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Dữ liệu hoặc hành vi item: weapon, armor, spell, flask, quick slot, projectile hoặc material.
- **Kế thừa/cha:** Item
- **Script con:** -
- **Field public/serialized chính:** public UpgradeStone upgradeStone
- **Liên kết script:** Item, UpgradeStone

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| - | - | Script này chủ yếu là dữ liệu/enum/struct, không có method rõ ràng. | - |

#### BuffCharmItem

- **Đường dẫn:** `Assets/Game/Scripts/Items/Quick Slot Items/BuffCharmItem.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Dữ liệu hoặc hành vi item: weapon, armor, spell, flask, quick slot, projectile hoặc material.
- **Kế thừa/cha:** QuickSlotItem
- **Script con:** -
- **Field public/serialized chính:** [SerializeField] private float buffDurationSeconds, [SerializeField] private int maxHealthBonus, [SerializeField] private int maxStaminaBonus, [SerializeField] private int maxFocusPointsBonus, [SerializeField] private float staminaRegenerationBonusPercentage, [SerializeField] private float outgoingDamageBonusPercentage, [SerializeField] private GameObject useItemVFX
- **Liên kết script:** PlayerManager, PlayerStatBuffTimedEffect, PlayerUIManager, QuickSlotItem, WorldCharacterEffectsManager, WorldSoundFXManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public void InitializeRuntimeBuff( string runtimeItemName, string runtimeItemDescription, Sprite runtimeIcon, float durationSeconds, int healthBonus, int staminaBonus, int focusBonus, float staminaRegenBonusPercent, float damageBonusPercent, int startingAmount, int maxAmount, int runtimePurchasePrice, int runtimeSellPrice, string runtimeAnimation = "Item_Flask_Drink_Start_01")` | 18 | Thực hiện logic initialize runtime buff trong script BuffCharmItem. | - |
| `public void SetRuntimeUseItemVFX(GameObject runtimeUseItemVFX)` | 55 | Thiết lập giá trị hoặc trạng thái runtime use item vfx. | - |
| `public override bool CanIUseThisItem(PlayerManager player)` | 60 | Kiểm tra có được phép iuse this item hay không. | - |
| `public override void AttemptToUseItem(PlayerManager player)` | 71 | Cố gắng kích hoạt to use item nếu trạng thái hiện tại cho phép. | - |
| `public override void SuccessfullyUseItem(PlayerManager player)` | 95 | Thực hiện logic successfully use item trong script BuffCharmItem. Liên kết trực tiếp: PlayerStatBuffTimedEffect, PlayerUIManager. | PlayerStatBuffTimedEffect, PlayerUIManager |
| `public PlayerStatBuffTimedEffect CreateEffectInstance()` | 142 | Tạo object/dữ liệu effect instance. Liên kết trực tiếp: PlayerStatBuffTimedEffect. | PlayerStatBuffTimedEffect |
| `private GameObject GetBuffUseVFXPrefab()` | 157 | Lấy dữ liệu buff use vfxprefab cho hệ thống khác sử dụng. Liên kết trực tiếp: WorldCharacterEffectsManager. | WorldCharacterEffectsManager |
| `public override void PlayUseItemFX(PlayerManager player)` | 185 | Phát use item fx, thường là animation, sound hoặc VFX. Liên kết trực tiếp: WorldCharacterEffectsManager, WorldSoundFXManager. | WorldCharacterEffectsManager, WorldSoundFXManager |

#### QuickSlotItem

- **Đường dẫn:** `Assets/Game/Scripts/Items/Quick Slot Items/QuickSlotItem.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Dữ liệu hoặc hành vi item: weapon, armor, spell, flask, quick slot, projectile hoặc material.
- **Kế thừa/cha:** Item
- **Script con:** BuffCharmItem, FlaskItem
- **Field public/serialized chính:** [SerializeField] protected GameObject itemModel, [SerializeField] protected string useItemAnimation, public bool isConsumable, public int itemAmount
- **Liên kết script:** Item, PlayerManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public virtual void AttemptToUseItem(PlayerManager player)` | 19 | Cố gắng kích hoạt to use item nếu trạng thái hiện tại cho phép. | - |
| `public virtual void SuccessfullyUseItem(PlayerManager player)` | 27 | Thực hiện logic successfully use item trong script QuickSlotItem. | - |
| `public virtual void PlayUseItemFX(PlayerManager player)` | 32 | Phát use item fx, thường là animation, sound hoặc VFX. | - |
| `public virtual bool CanIUseThisItem(PlayerManager player)` | 37 | Kiểm tra có được phép iuse this item hay không. | - |
| `public virtual int GetCurrentAmount(PlayerManager player)` | 42 | Lấy dữ liệu current amount cho hệ thống khác sử dụng. | - |
| `public void SetRuntimeItemModel(GameObject runtimeItemModel)` | 47 | Thiết lập giá trị hoặc trạng thái runtime item model. | - |

#### FireBallDamageCollider

- **Đường dẫn:** `Assets/Game/Scripts/Items/Spells/FireBallDamageCollider.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Dữ liệu hoặc hành vi item: weapon, armor, spell, flask, quick slot, projectile hoặc material.
- **Kế thừa/cha:** SpellProjectileDamageCollider
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** CharacterManager, FireBallManager, SpellProjectileDamageCollider, TakeDamageEffect, WorldCharacterEffectsManager, WorldUtilityManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `protected override void Awake()` | 9 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. Liên kết trực tiếp: FireBallManager. | FireBallManager |
| `protected override void OnTriggerEnter(Collider other)` | 16 | Xử lý khi collider khác đi vào trigger của object này. Liên kết trực tiếp: CharacterManager, WorldUtilityManager. | CharacterManager, WorldUtilityManager |
| `protected override void CheckForParry(CharacterManager damageTarget)` | 46 | Thực hiện logic check for parry trong script FireBallDamageCollider. | - |
| `protected override void GetBlockingDotValues(CharacterManager damageTarget)` | 51 | Lấy dữ liệu blocking dot values cho hệ thống khác sử dụng. | - |
| `protected override void DamageTarget(CharacterManager damageTarget)` | 57 | Gây hoặc xử lý sát thương cho target. Liên kết trực tiếp: TakeDamageEffect, WorldCharacterEffectsManager. | TakeDamageEffect, WorldCharacterEffectsManager |

#### FireBallManager

- **Đường dẫn:** `Assets/Game/Scripts/Items/Spells/FireBallManager.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Dữ liệu hoặc hành vi item: weapon, armor, spell, flask, quick slot, projectile hoặc material.
- **Kế thừa/cha:** SpellManager
- **Script con:** -
- **Field public/serialized chính:** public FireBallDamageCollider damageCollider, public bool isFullyCharged
- **Liên kết script:** CharacterManager, FireBallDamageCollider, SpellManager, WorldSoundFXManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `protected override void Awake()` | 19 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. | - |
| `protected override void Update()` | 27 | Cập nhật logic mỗi frame như input, timer, trạng thái chiến đấu hoặc UI. | - |
| `private void OnCollisionEnter(Collision collision)` | 35 | Xử lý va chạm vật lý khi object chạm object khác. | - |
| `public void InitializeFireBall(CharacterManager spellCaster)` | 48 | Thực hiện logic initialize fire ball trong script FireBallManager. | - |
| `public void InstantiateSpellDestructionFX()` | 58 | Thực hiện logic instantiate spell destruction fx trong script FireBallManager. Liên kết trực tiếp: WorldSoundFXManager. | WorldSoundFXManager |
| `public void WaitThenInstantiateSpellDestructionFX(float timeToWait)` | 74 | Thực hiện logic wait then instantiate spell destruction fx trong script FireBallManager. | - |
| `private IEnumerator WaitThenInstantiateFX(float timeToWait)` | 83 | Thực hiện logic wait then instantiate fx trong script FireBallManager. | - |
| `new WaitForSeconds(timeToWait)` | 85 | Thực hiện logic wait for seconds trong script FireBallManager. | - |

#### FireBallSpell

- **Đường dẫn:** `Assets/Game/Scripts/Items/Spells/FireBallSpell.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Dữ liệu hoặc hành vi item: weapon, armor, spell, flask, quick slot, projectile hoặc material.
- **Kế thừa/cha:** SpellItem
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** FireBallManager, PlayerManager, SpellInstantiationLocation, SpellItem

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public override void AttemptToCastSpell(PlayerManager player)` | 11 | Cố gắng kích hoạt to cast spell nếu trạng thái hiện tại cho phép. | - |
| `public override void InstantiateWarmUpSpellFX(PlayerManager player)` | 28 | Thực hiện logic instantiate warm up spell fx trong script FireBallSpell. Liên kết trực tiếp: SpellInstantiationLocation. | SpellInstantiationLocation |
| `public override void SuccessfullyCastSpell(PlayerManager player)` | 52 | Thực hiện logic successfully cast spell trong script FireBallSpell. Liên kết trực tiếp: FireBallManager, SpellInstantiationLocation. | FireBallManager, SpellInstantiationLocation |
| `public override void SuccessfullyChargeSpell(PlayerManager player)` | 113 | Thực hiện logic successfully charge spell trong script FireBallSpell. Liên kết trực tiếp: SpellInstantiationLocation. | SpellInstantiationLocation |
| `public override void SuccessfullyCastSpellFullCharge(PlayerManager player)` | 142 | Thực hiện logic successfully cast spell full charge trong script FireBallSpell. Liên kết trực tiếp: FireBallManager, SpellInstantiationLocation. | FireBallManager, SpellInstantiationLocation |

#### SpellItem

- **Đường dẫn:** `Assets/Game/Scripts/Items/Spells/SpellItem.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Dữ liệu hoặc hành vi item: weapon, armor, spell, flask, quick slot, projectile hoặc material.
- **Kế thừa/cha:** Item
- **Script con:** FireBallSpell, TestSpell
- **Field public/serialized chính:** public SpellClass spellClass, public float fullChargeEffectMultiplier, public int spellSlotUsed, public int staminaCost, public int focusPointCost, [SerializeField] protected GameObject spellCastWarmUpFX, [SerializeField] protected GameObject spellChargeFX, [SerializeField] protected GameObject spellCastReleaseFX, [SerializeField] protected GameObject spellCastReleaseFXFullCharge, [SerializeField] protected string mainHandSpellAnimation, [SerializeField] protected string offHandSpellAnimation, public AudioClip warmUpSoundFX +1
- **Liên kết script:** Item, PlayerManager, SpellClass

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public virtual void AttemptToCastSpell(PlayerManager player)` | 33 | Cố gắng kích hoạt to cast spell nếu trạng thái hiện tại cho phép. | - |
| `public virtual void InstantiateWarmUpSpellFX(PlayerManager player)` | 39 | Thực hiện logic instantiate warm up spell fx trong script SpellItem. | - |
| `public virtual void SuccessfullyCastSpell(PlayerManager player)` | 45 | Thực hiện logic successfully cast spell trong script SpellItem. | - |
| `public virtual void SuccessfullyChargeSpell(PlayerManager player)` | 54 | Thực hiện logic successfully charge spell trong script SpellItem. | - |
| `public virtual void SuccessfullyCastSpellFullCharge(PlayerManager player)` | 59 | Thực hiện logic successfully cast spell full charge trong script SpellItem. | - |
| `public virtual bool CanICastThisSpell(PlayerManager player)` | 69 | Kiểm tra có được phép icast this spell hay không. | - |

#### SpellManager

- **Đường dẫn:** `Assets/Game/Scripts/Items/Spells/SpellManager.cs`
- **Loại:** MonoBehaviour
- **Vai trò dễ hiểu:** Dữ liệu hoặc hành vi item: weapon, armor, spell, flask, quick slot, projectile hoặc material.
- **Kế thừa/cha:** MonoBehaviour
- **Script con:** FireBallManager
- **Field public/serialized chính:** [SerializeField] protected CharacterManager spellTarget, [SerializeField] protected GameObject impactParticle, [SerializeField] protected GameObject impactParticleFullCharge
- **Liên kết script:** CharacterManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `protected virtual void Awake()` | 14 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. | - |
| `protected virtual void Start()` | 19 | Chạy sau Awake để hoàn tất thiết lập ban đầu, nhất là khi cần các object khác đã sẵn sàng. | - |
| `protected virtual void Update()` | 24 | Cập nhật logic mỗi frame như input, timer, trạng thái chiến đấu hoặc UI. | - |

#### SpellProjectileDamageCollider

- **Đường dẫn:** `Assets/Game/Scripts/Items/Spells/SpellProjectileDamageCollider.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Dữ liệu hoặc hành vi item: weapon, armor, spell, flask, quick slot, projectile hoặc material.
- **Kế thừa/cha:** DamageCollider
- **Script con:** FireBallDamageCollider
- **Field public/serialized chính:** public CharacterManager spellCaster
- **Liên kết script:** CharacterManager, DamageCollider

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| - | - | Script này chủ yếu là dữ liệu/enum/struct, không có method rõ ràng. | - |

#### TestSpell

- **Đường dẫn:** `Assets/Game/Scripts/Items/Spells/TestSpell.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Dữ liệu hoặc hành vi item: weapon, armor, spell, flask, quick slot, projectile hoặc material.
- **Kế thừa/cha:** SpellItem
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** PlayerManager, SpellItem

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public override void AttemptToCastSpell(PlayerManager player)` | 8 | Cố gắng kích hoạt to cast spell nếu trạng thái hiện tại cho phép. | - |
| `public override void SuccessfullyCastSpell(PlayerManager player)` | 25 | Thực hiện logic successfully cast spell trong script TestSpell. | - |
| `public override void InstantiateWarmUpSpellFX(PlayerManager player)` | 32 | Thực hiện logic instantiate warm up spell fx trong script TestSpell. | - |
| `public override bool CanICastThisSpell(PlayerManager player)` | 39 | Kiểm tra có được phép icast this spell hay không. | - |

#### CasterWeaponItem

- **Đường dẫn:** `Assets/Game/Scripts/Items/Weapons/CasterWeaponItem.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Dữ liệu hoặc hành vi item: weapon, armor, spell, flask, quick slot, projectile hoặc material.
- **Kế thừa/cha:** WeaponItem
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** WeaponItem

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| - | - | Script này chủ yếu là dữ liệu/enum/struct, không có method rõ ràng. | - |

#### MeleeWeaponItem

- **Đường dẫn:** `Assets/Game/Scripts/Items/Weapons/MeleeWeaponItem.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Dữ liệu hoặc hành vi item: weapon, armor, spell, flask, quick slot, projectile hoặc material.
- **Kế thừa/cha:** WeaponItem
- **Script con:** -
- **Field public/serialized chính:** public float riposte_Attack_01_Modifier, public float backstab_Attack_01_Modifier
- **Liên kết script:** WeaponItem

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| - | - | Script này chủ yếu là dữ liệu/enum/struct, không có method rõ ràng. | - |

#### RangedWeaponItem

- **Đường dẫn:** `Assets/Game/Scripts/Items/Weapons/RangedWeaponItem.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Dữ liệu hoặc hành vi item: weapon, armor, spell, flask, quick slot, projectile hoặc material.
- **Kế thừa/cha:** WeaponItem
- **Script con:** -
- **Field public/serialized chính:** public AudioClip[] drawSounds, public AudioClip[] releaseSounds
- **Liên kết script:** WeaponItem

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| - | - | Script này chủ yếu là dữ liệu/enum/struct, không có method rõ ràng. | - |

#### WeaponItem

- **Đường dẫn:** `Assets/Game/Scripts/Items/Weapons/WeaponItem.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Dữ liệu hoặc hành vi item: weapon, armor, spell, flask, quick slot, projectile hoặc material.
- **Kế thừa/cha:** EquipmentItem
- **Script con:** CasterWeaponItem, MeleeWeaponItem, RangedWeaponItem
- **Field public/serialized chính:** public AnimatorOverrideController weaponAnimator, public WeaponModelType weaponModelType, public GameObject weaponModel, public WeaponClass weaponClass, public UpgradeLevel upgradeLevel, public int strengthREQ, public int dexREQ, public int intREQ, public int faithREQ, public int physicalDamage, public int magicDamage, public int fireDamage +39
- **Liên kết script:** AshOfWar, EquipmentItem, UpgradeLevel, WeaponClass, WeaponItemAction, WeaponModelType

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| - | - | Script này chủ yếu là dữ liệu/enum/struct, không có method rõ ràng. | - |

#### WeaponManager

- **Đường dẫn:** `Assets/Game/Scripts/Items/Weapons/WeaponManager.cs`
- **Loại:** MonoBehaviour
- **Vai trò dễ hiểu:** Dữ liệu hoặc hành vi item: weapon, armor, spell, flask, quick slot, projectile hoặc material.
- **Kế thừa/cha:** MonoBehaviour
- **Script con:** -
- **Field public/serialized chính:** public MeleeWeaponDamageCollider meleeDamageCollider
- **Liên kết script:** CharacterManager, MeleeWeaponDamageCollider, PlayerStatsManager, WeaponItem

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `private void Awake()` | 16 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. Liên kết trực tiếp: MeleeWeaponDamageCollider. | MeleeWeaponDamageCollider |
| `public void SetWeaponDamage(CharacterManager characterWieldingWeapon, WeaponItem weapon)` | 21 | Thiết lập giá trị hoặc trạng thái weapon damage. Liên kết trực tiếp: PlayerStatsManager. | PlayerStatsManager |
| `public void ToggleWeaponTrail(bool status)` | 100 | Thực hiện logic toggle weapon trail trong script WeaponManager. | - |

### Assets/Game/Scripts/Menu Scene

#### TitleScreenLoadMenuInputManager

- **Đường dẫn:** `Assets/Game/Scripts/Menu Scene/TitleScreenLoadMenuInputManager.cs`
- **Loại:** MonoBehaviour
- **Vai trò dễ hiểu:** Điều khiển title/menu scene: load slot, setting menu, preview nhân vật hoặc bắt input menu.
- **Kế thừa/cha:** MonoBehaviour
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** TitleScreenManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `private void Update()` | 13 | Cập nhật logic mỗi frame như input, timer, trạng thái chiến đấu hoặc UI. Liên kết trực tiếp: TitleScreenManager. | TitleScreenManager |
| `private void OnEnable()` | 22 | Đăng ký event/callback hoặc bật trạng thái khi component được enable. | - |
| `new PlayerControls()` | 26 | Phát er controls, thường là animation, sound hoặc VFX. | - |
| `private void OnDisable()` | 32 | Gỡ event/callback hoặc dọn trạng thái khi component bị disable. | - |

#### TitleScreenManager

- **Đường dẫn:** `Assets/Game/Scripts/Menu Scene/TitleScreenManager.cs`
- **Loại:** MonoBehaviour
- **Vai trò dễ hiểu:** Điều khiển title/menu scene: load slot, setting menu, preview nhân vật hoặc bắt input menu.
- **Kế thừa/cha:** MonoBehaviour
- **Script con:** -
- **Field public/serialized chính:** public static TitleScreenManager Instance, public CharacterSlot currentSelectedSlot, public CharacterClass[] startingClasses, [SerializeField] private int selectedStartingClassID, [SerializeField] private GameObject classReviewPanel, [SerializeField] private TextMeshProUGUI classReviewTitleText, [SerializeField] private TextMeshProUGUI classReviewSubtitleText, [SerializeField] private GameObject classReviewStatsInfoPanel, [SerializeField] private GameObject classReviewItemsInfoPanel, [SerializeField] private TextMeshProUGUI classReviewStatsInfoText, [SerializeField] private TextMeshProUGUI classReviewItemsInfoText, [SerializeField] private TextMeshProUGUI classReviewStatsText +4
- **Liên kết script:** BodyEquipmentItem, BuffCharmItem, BuildRuntimeLogger, CharacterClass, CharacterSlot, ClassReviewTab, GameSettingsManager, HandEquipmentItem, HeadEquipmentItem, Item, LegEquipmentItem, PlayerManager, PlayerUIManager, QuickSlotItem, SessionLaunchMode, TitleButtonClickSound +6

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `private void Awake()` | 113 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. Liên kết trực tiếp: BuildRuntimeLogger. | BuildRuntimeLogger |
| `private void OnEnable()` | 127 | Đăng ký event/callback hoặc bật trạng thái khi component được enable. Liên kết trực tiếp: GameSettingsManager. | GameSettingsManager |
| `private void OnDisable()` | 132 | Gỡ event/callback hoặc dọn trạng thái khi component bị disable. Liên kết trực tiếp: GameSettingsManager. | GameSettingsManager |
| `private void Start()` | 137 | Chạy sau Awake để hoàn tất thiết lập ban đầu, nhất là khi cần các object khác đã sẵn sàng. Liên kết trực tiếp: BuildRuntimeLogger. | BuildRuntimeLogger |
| `private void Update()` | 152 | Cập nhật logic mỗi frame như input, timer, trạng thái chiến đấu hoặc UI. | - |
| `private void LateUpdate()` | 158 | Cập nhật cuối frame, thường dùng cho camera, animation hoặc đồng bộ trạng thái sau movement. | - |
| `private void HideGameplayHUDOnTitleScreen()` | 163 | Thực hiện logic hide gameplay hudon title screen trong script TitleScreenManager. Liên kết trực tiếp: PlayerUIManager. | PlayerUIManager |
| `public void StartNetworkAsHost()` | 171 | Thực hiện logic start network as host trong script TitleScreenManager. | - |
| `public void PressStart()` | 176 | Thực hiện logic press start trong script TitleScreenManager. | - |
| `public void SelectSingleplayerMode()` | 193 | Thực hiện logic select singleplayer mode trong script TitleScreenManager. Liên kết trực tiếp: SessionLaunchMode. | SessionLaunchMode |
| `public void SelectMultiplayerMode()` | 203 | Thực hiện logic select multiplayer mode trong script TitleScreenManager. Liên kết trực tiếp: SessionLaunchMode. | SessionLaunchMode |
| `public void JoinOnlineGame()` | 213 | Thực hiện logic join online game trong script TitleScreenManager. | - |
| `public async void HostWorld()` | 218 | Thực hiện logic host world trong script TitleScreenManager. Liên kết trực tiếp: WorldGameSessionManager. | WorldGameSessionManager |
| `public void JoinWorld()` | 231 | Thực hiện logic join world trong script TitleScreenManager. Liên kết trực tiếp: WorldGameSessionManager. | WorldGameSessionManager |
| `private void HideLegacyNetworkControls()` | 240 | Thực hiện logic hide legacy network controls trong script TitleScreenManager. | - |
| `private void EnsureSettingsMenu()` | 269 | Thực hiện logic ensure settings menu trong script TitleScreenManager. Liên kết trực tiếp: TitleScreenSettingsMenuView. | TitleScreenSettingsMenuView |
| `public void OpenSettingsMenu()` | 288 | Mở UI/trạng thái/luồng settings menu. | - |
| `public void CloseSettingsMenu()` | 307 | Đóng UI/trạng thái/luồng settings menu. | - |
| `public async void AttemptToCreateNewCharacter()` | 319 | Cố gắng kích hoạt to create new character nếu trạng thái hiện tại cho phép. Liên kết trực tiếp: BuildRuntimeLogger, WorldSaveGameManager. | BuildRuntimeLogger, WorldSaveGameManager |
| `await EnsureHostSessionForSaveMenus())` | 323 | Thực hiện logic ensure host session for save menus trong script TitleScreenManager. Liên kết trực tiếp: BuildRuntimeLogger. | BuildRuntimeLogger |
| `public void StartNewGame()` | 342 | Thực hiện logic start new game trong script TitleScreenManager. Liên kết trực tiếp: BuildRuntimeLogger, WorldSaveGameManager. | BuildRuntimeLogger, WorldSaveGameManager |
| `private void EnsureEnterGameMusicPlayer()` | 352 | Thực hiện logic ensure enter game music player trong script TitleScreenManager. | - |
| `private void PlayEnterGameMusic()` | 365 | Phát enter game music, thường là animation, sound hoặc VFX. | - |
| `private void StopEnterGameMusic()` | 381 | Thực hiện logic stop enter game music trong script TitleScreenManager. | - |
| `private void ApplyTitleMusicVolume()` | 387 | Áp dụng title music volume lên character/object mục tiêu. Liên kết trực tiếp: GameSettingsManager. | GameSettingsManager |
| `private void EnsureButtonClickSFXPlayer()` | 398 | Thực hiện logic ensure button click sfxplayer trong script TitleScreenManager. | - |
| `private void InstallTitleButtonClickSFX()` | 410 | Thực hiện logic install title button click sfx trong script TitleScreenManager. Liên kết trực tiếp: TitleButtonClickSound. | TitleButtonClickSound |
| `public void PlayTitleButtonClickSFX()` | 428 | Phát title button click sfx, thường là animation, sound hoặc VFX. Liên kết trực tiếp: WorldSoundFXManager. | WorldSoundFXManager |
| `private void ApplyTitleButtonClickVolume()` | 444 | Áp dụng title button click volume lên character/object mục tiêu. Liên kết trực tiếp: GameSettingsManager. | GameSettingsManager |
| `public async void OpenLoadGameMenu()` | 452 | Mở UI/trạng thái/luồng load game menu. | - |
| `public void CloseLoadGameMenu()` | 467 | Đóng UI/trạng thái/luồng load game menu. | - |
| `public void ToggleBodyType()` | 488 | Thực hiện logic toggle body type trong script TitleScreenManager. Liên kết trực tiếp: PlayerManager. | PlayerManager |
| `public void OpenTitleScreenMainMenu()` | 504 | Mở UI/trạng thái/luồng title screen main menu. | - |
| `public void CloseTitleScreenMainMenu()` | 511 | Đóng UI/trạng thái/luồng title screen main menu. | - |
| `public void OpenLaunchModeMenu()` | 516 | Mở UI/trạng thái/luồng launch mode menu. | - |
| `public void CloseLaunchModeMenu()` | 527 | Đóng UI/trạng thái/luồng launch mode menu. | - |
| `private void EnsureLaunchModeMenu()` | 533 | Thực hiện logic ensure launch mode menu trong script TitleScreenManager. | - |
| `private void SetBannerActive(bool isActive)` | 551 | Thiết lập giá trị hoặc trạng thái banner active. | - |
| `public void OpenCharacterCreationMenu()` | 557 | Mở UI/trạng thái/luồng character creation menu. Liên kết trực tiếp: PlayerManager. | PlayerManager |
| `private async Task<bool> EnsureHostSessionForSaveMenus()` | 578 | Thực hiện logic ensure host session for save menus trong script TitleScreenManager. Liên kết trực tiếp: BuildRuntimeLogger, WorldGameSessionManager. | BuildRuntimeLogger, WorldGameSessionManager |
| `private void SetLaunchMode(SessionLaunchMode launchMode)` | 602 | Thiết lập giá trị hoặc trạng thái launch mode. Liên kết trực tiếp: WorldGameSessionManager. | WorldGameSessionManager |
| `private void RefreshSaveMenuButtons()` | 610 | Làm mới dữ liệu/hiển thị save menu buttons. Liên kết trực tiếp: WorldGameSessionManager. | WorldGameSessionManager |
| `private void CloseLoadGameMenuIfOpen()` | 621 | Đóng UI/trạng thái/luồng load game menu if open. | - |
| `private void CloseCharacterCreationMenuIfOpen()` | 627 | Đóng UI/trạng thái/luồng character creation menu if open. | - |
| `private void BuildNetworkMenuControls()` | 633 | Thực hiện logic build network menu controls trong script TitleScreenManager. | - |
| `private void SetupHostButton(RectTransform menuParent)` | 651 | Thiết lập giá trị hoặc trạng thái up host button. | - |
| `new Vector2(newGameRect.anchoredPosition.x, newGameRect.anchoredPosition.y + 90f)` | 667 | Thực hiện logic vector2 trong script TitleScreenManager. | - |
| `private void SetupJoinButton(RectTransform menuParent)` | 672 | Thiết lập giá trị hoặc trạng thái up join button. | - |
| `new Vector2(loadRect.anchoredPosition.x, loadRect.anchoredPosition.y + 90f)` | 696 | Thực hiện logic vector2 trong script TitleScreenManager. | - |
| `private void SetupNetworkAddressInput(RectTransform menuParent)` | 701 | Thiết lập giá trị hoặc trạng thái up network address input. | - |
| `new Vector2(hostRect.anchoredPosition.x, hostRect.anchoredPosition.y + 90f)` | 719 | Thực hiện logic vector2 trong script TitleScreenManager. | - |
| `new Vector2(420f, addressRect.sizeDelta.y)` | 720 | Thực hiện logic vector2 trong script TitleScreenManager. | - |
| `private void ConfigureButton(Button button, string objectName, string label, UnityEngine.Events.UnityAction callback)` | 737 | Thực hiện logic configure button trong script TitleScreenManager. | - |
| `private void PopulateDefaultNetworkAddressField()` | 754 | Thực hiện logic populate default network address field trong script TitleScreenManager. Liên kết trực tiếp: WorldGameSessionManager. | WorldGameSessionManager |
| `public void CloseCharacterCreationMenu()` | 767 | Đóng UI/trạng thái/luồng character creation menu. | - |
| `public void OpenChooseCharacterClassSubMenu()` | 774 | Mở UI/trạng thái/luồng choose character class sub menu. | - |
| `public void CloseChooseCharacterClassSubMenu()` | 790 | Đóng UI/trạng thái/luồng choose character class sub menu. | - |
| `public void OpenChooseHairStyleSubMenu()` | 800 | Mở UI/trạng thái/luồng choose hair style sub menu. Liên kết trực tiếp: PlayerManager. | PlayerManager |
| `public void CloseChooseHairStyleSubMenu()` | 823 | Đóng UI/trạng thái/luồng choose hair style sub menu. Liên kết trực tiếp: PlayerManager. | PlayerManager |
| `public void OpenChooseHairColorSubMenu()` | 838 | Mở UI/trạng thái/luồng choose hair color sub menu. Liên kết trực tiếp: PlayerManager. | PlayerManager |
| `public void CloseChooseHairColorSubMenu()` | 861 | Đóng UI/trạng thái/luồng choose hair color sub menu. Liên kết trực tiếp: PlayerManager. | PlayerManager |
| `public void OpenChooseNameSubMenu()` | 876 | Mở UI/trạng thái/luồng choose name sub menu. Liên kết trực tiếp: PlayerManager. | PlayerManager |
| `public void CloseChooseNameSubMenu()` | 887 | Đóng UI/trạng thái/luồng choose name sub menu. Liên kết trực tiếp: PlayerManager. | PlayerManager |
| `private void ToggleCharacterCreationScreenMainMenuButtons(bool status)` | 900 | Thực hiện logic toggle character creation screen main menu buttons trong script TitleScreenManager. | - |
| `public void DisplayNoFreeCharacterSlotPopUp()` | 910 | Thực hiện logic display no free character slot pop up trong script TitleScreenManager. | - |
| `public void CloseNoFreeCharacterSlotPopUp()` | 916 | Đóng UI/trạng thái/luồng no free character slot pop up. | - |
| `public void SelectCharacterSlot(CharacterSlot characterSlot)` | 922 | Thực hiện logic select character slot trong script TitleScreenManager. | - |
| `public void SelectNoSlot()` | 927 | Thực hiện logic select no slot trong script TitleScreenManager. Liên kết trực tiếp: CharacterSlot. | CharacterSlot |
| `public void AttemptToDeleteCharacterSlot()` | 932 | Cố gắng kích hoạt to delete character slot nếu trạng thái hiện tại cho phép. Liên kết trực tiếp: CharacterSlot. | CharacterSlot |
| `public void DeleteCharacterSlot()` | 941 | Thực hiện logic delete character slot trong script TitleScreenManager. Liên kết trực tiếp: WorldSaveGameManager. | WorldSaveGameManager |
| `public void CloseDeleteCharacterPopUp()` | 953 | Đóng UI/trạng thái/luồng delete character pop up. | - |
| `public void SelectClass(int classID)` | 961 | Thực hiện logic select class trong script TitleScreenManager. Liên kết trực tiếp: PlayerManager. | PlayerManager |
| `public void PreviewClass(int classID)` | 975 | Thực hiện logic preview class trong script TitleScreenManager. Liên kết trực tiếp: PlayerManager. | PlayerManager |
| `public int GetSelectedStartingClassID()` | 987 | Lấy dữ liệu selected starting class id cho hệ thống khác sử dụng. | - |
| `private void EnsureCharacterClassSelectionUI()` | 998 | Thực hiện logic ensure character class selection ui trong script TitleScreenManager. | - |
| `private void RefreshSerializedCharacterClassButtons()` | 1011 | Làm mới dữ liệu/hiển thị serialized character class buttons. | - |
| `private void ConfigureSerializedCharacterClassButton(Button button, int classID)` | 1033 | Thực hiện logic configure serialized character class button trong script TitleScreenManager. | - |
| `private void UpdateClassReviewPanel(int classID, bool isSelectedClass)` | 1048 | Cập nhật class review panel theo trạng thái mới. Liên kết trực tiếp: CharacterClass, ClassReviewTab. | CharacterClass, ClassReviewTab |
| `public void ShowClassReviewStats()` | 1083 | Thực hiện logic show class review stats trong script TitleScreenManager. Liên kết trực tiếp: ClassReviewTab. | ClassReviewTab |
| `public void ShowClassReviewItems()` | 1089 | Thực hiện logic show class review items trong script TitleScreenManager. Liên kết trực tiếp: ClassReviewTab. | ClassReviewTab |
| `private void RefreshClassReviewInfoPanel()` | 1095 | Làm mới dữ liệu/hiển thị class review info panel. Liên kết trực tiếp: ClassReviewTab. | ClassReviewTab |
| `private void PrepareClassReviewInfoText(TextMeshProUGUI infoText)` | 1120 | Thực hiện logic prepare class review info text trong script TitleScreenManager. | - |
| `private void UpdateClassReviewTabVisual(Button button, TextMeshProUGUI label, bool isActive)` | 1139 | Cập nhật class review tab visual theo trạng thái mới. | - |
| `new Color(1f, 1f, 1f, 0.16f) : new Color(1f, 1f, 1f, 0.08f)` | 1147 | Thực hiện logic color trong script TitleScreenManager. | - |
| `private void UpdateCharacterClassPrimaryButtonLabel()` | 1152 | Cập nhật character class primary button label theo trạng thái mới. | - |
| `private string GetFormattedClassButtonLabel(CharacterClass characterClass)` | 1167 | Lấy dữ liệu formatted class button label cho hệ thống khác sử dụng. | - |
| `private string GetClassSubtitle(CharacterClass characterClass)` | 1172 | Lấy dữ liệu class subtitle cho hệ thống khác sử dụng. | - |
| `return BuildStatArchetypeSummary(characterClass)` | 1189 | Thực hiện logic build stat archetype summary trong script TitleScreenManager. | - |
| `private string GetClassDescription(CharacterClass characterClass)` | 1193 | Lấy dữ liệu class description cho hệ thống khác sử dụng. | - |
| `private string BuildStatArchetypeSummary(CharacterClass characterClass)` | 1214 | Thực hiện logic build stat archetype summary trong script TitleScreenManager. | - |
| `private void UpdateHighestStat(int statValue, string statName, ref int highestStatValue, ref string highestStatName)` | 1229 | Cập nhật highest stat theo trạng thái mới. | - |
| `private string GetFormattedClassStats(CharacterClass characterClass)` | 1238 | Lấy dữ liệu formatted class stats cho hệ thống khác sử dụng. | - |
| `private string GetFormattedClassLoadout(CharacterClass characterClass)` | 1244 | Lấy dữ liệu formatted class loadout cho hệ thống khác sử dụng. | - |
| `private string GetWeaponListLabel(WeaponItem[] weapons)` | 1253 | Lấy dữ liệu weapon list label cho hệ thống khác sử dụng. Liên kết trực tiếp: WeaponItem. | WeaponItem |
| `private string GetQuickSlotListLabel(QuickSlotItem[] quickSlots)` | 1276 | Lấy dữ liệu quick slot list label cho hệ thống khác sử dụng. Liên kết trực tiếp: QuickSlotItem. | QuickSlotItem |
| `public void SetCharacterClass(PlayerManager player, int vitality, int endurance, int mind, int strength, int dexterity, int intelligence, int faith, WeaponItem[] mainHandWeapons, WeaponItem[] offHandWeapons, HeadEquipmentItem headEquipment, BodyEquipmentItem bodyEquipment, LegEquipmentItem legEquipment, HandEquipmentItem handEquipment, QuickSlotItem[] quickSlotItems)` | 1328 | Thiết lập giá trị hoặc trạng thái character class. Liên kết trực tiếp: BodyEquipmentItem, BuffCharmItem, HandEquipmentItem, HeadEquipmentItem, LegEquipmentItem +1. | BodyEquipmentItem, BuffCharmItem, HandEquipmentItem, HeadEquipmentItem, LegEquipmentItem, WorldItemDatabase |
| `private WeaponItem InstantiateWeaponOrFallback(WeaponItem sourceWeapon)` | 1446 | Thực hiện logic instantiate weapon or fallback trong script TitleScreenManager. Liên kết trực tiếp: WeaponItem, WorldItemDatabase. | WeaponItem, WorldItemDatabase |
| `private WeaponItem GetWeaponAtIndex(WeaponItem[] weapons, int index)` | 1456 | Lấy dữ liệu weapon at index cho hệ thống khác sử dụng. | - |
| `private QuickSlotItem GetQuickSlotItemAtIndex(QuickSlotItem[] quickSlotItems, int index)` | 1464 | Lấy dữ liệu quick slot item at index cho hệ thống khác sử dụng. | - |
| `public void SelectHair(int hairID)` | 1474 | Thực hiện logic select hair trong script TitleScreenManager. Liên kết trực tiếp: PlayerManager. | PlayerManager |
| `public void PreviewHair(int hairID)` | 1483 | Thực hiện logic preview hair trong script TitleScreenManager. Liên kết trực tiếp: PlayerManager. | PlayerManager |
| `public void SelectHairColor()` | 1490 | Thực hiện logic select hair color trong script TitleScreenManager. Liên kết trực tiếp: PlayerManager. | PlayerManager |
| `public void PreviewHairColor()` | 1501 | Thực hiện logic preview hair color trong script TitleScreenManager. Liên kết trực tiếp: PlayerManager. | PlayerManager |
| `public void SetRedColorSlider(float redValue)` | 1510 | Thiết lập giá trị hoặc trạng thái red color slider. | - |
| `public void SetGreenColorSlider(float greenValue)` | 1515 | Thiết lập giá trị hoặc trạng thái green color slider. | - |
| `public void SetBlueColorSlider(float blueValue)` | 1520 | Thiết lập giá trị hoặc trạng thái blue color slider. | - |
| `private void Awake()` | 1530 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. | - |
| `public void OnPointerDown(PointerEventData eventData)` | 1535 | Thực hiện logic on pointer down trong script TitleScreenManager. | - |
| `public void OnSubmit(BaseEventData eventData)` | 1540 | Thực hiện logic on submit trong script TitleScreenManager. | - |
| `private void PlayIfButtonCanClick()` | 1545 | Phát if button can click, thường là animation, sound hoặc VFX. | - |

#### TitleScreenSettingsMenuManager

- **Đường dẫn:** `Assets/Game/Scripts/Menu Scene/TitleScreenSettingsMenuManager.cs`
- **Loại:** MonoBehaviour
- **Vai trò dễ hiểu:** Điều khiển title/menu scene: load slot, setting menu, preview nhân vật hoặc bắt input menu.
- **Kế thừa/cha:** MonoBehaviour
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** GameSettingsManager, TitleScreenManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public void Initialize(TitleScreenManager owner, GameObject mainMenuRoot, Button loadButton, Button newGameButton, Button settingsMenuButton)` | 36 | Thực hiện logic initialize trong script TitleScreenSettingsMenuManager. Liên kết trực tiếp: GameSettingsManager. | GameSettingsManager |
| `private void Update()` | 53 | Cập nhật logic mỗi frame như input, timer, trạng thái chiến đấu hoặc UI. | - |
| `public void OpenSettingsMenu()` | 62 | Mở UI/trạng thái/luồng settings menu. | - |
| `public void CloseSettingsMenu()` | 73 | Đóng UI/trạng thái/luồng settings menu. | - |
| `private void BuildSettingsMenu()` | 85 | Thực hiện logic build settings menu trong script TitleScreenSettingsMenuManager. | - |
| `new GameObject("Title Screen Settings Menu", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image))` | 100 | Thực hiện logic game object trong script TitleScreenSettingsMenuManager. | - |
| `new Vector2(0.5f, 0.5f)` | 110 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuManager. | - |
| `new Color(0f, 0f, 0f, 0.98f)` | 111 | Thực hiện logic color trong script TitleScreenSettingsMenuManager. | - |
| `new GameObject("Settings Content", typeof(RectTransform))` | 113 | Thực hiện logic game object trong script TitleScreenSettingsMenuManager. | - |
| `new Vector2(0.5f, 0.5f)` | 116 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuManager. | - |
| `new Vector2(0.5f, 0.5f)` | 117 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuManager. | - |
| `new Vector2(0.5f, 0.5f)` | 118 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuManager. | - |
| `new Vector2(960f, 760f)` | 119 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuManager. | - |
| `new Vector2(0f, -40f), 38f)` | 122 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuManager. | - |
| `new Vector2(0f, 140f))` | 136 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuManager. | - |
| `private void HideSiblingMenus()` | 144 | Thực hiện logic hide sibling menus trong script TitleScreenSettingsMenuManager. | - |
| `private void RestoreSiblingMenus()` | 165 | Thực hiện logic restore sibling menus trong script TitleScreenSettingsMenuManager. | - |
| `private void CreateHeader(RectTransform parent, TextMeshProUGUI textTemplate, string label, Vector2 anchoredPosition, float fontSize)` | 176 | Tạo object/dữ liệu header. | - |
| `new Vector2(0.5f, 1f)` | 183 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuManager. | - |
| `new Vector2(0.5f, 1f)` | 184 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuManager. | - |
| `new Vector2(0.5f, 1f)` | 185 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuManager. | - |
| `new Vector2(840f, 60f)` | 187 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuManager. | - |
| `private void CreateSliderRow( RectTransform parent, TextMeshProUGUI textTemplate, Slider sliderTemplate, string label, float anchoredY, out Slider slider, out TextMeshProUGUI valueText, UnityEngine.Events.UnityAction<float> callback, float minValue = 0f, float maxValue = 1f)` | 195 | Tạo object/dữ liệu slider row. | - |
| `new Vector2(0.5f, 1f)` | 210 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuManager. | - |
| `new Vector2(0.5f, 1f)` | 211 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuManager. | - |
| `new Vector2(0.5f, 1f)` | 212 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuManager. | - |
| `new Vector2(0f, anchoredY)` | 213 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuManager. | - |
| `new Vector2(860f, 70f)` | 214 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuManager. | - |
| `new Vector2(-320f, -12f), 24f, TextAlignmentOptions.Left)` | 216 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuManager. | - |
| `new Vector2(300f, 42f)` | 217 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuManager. | - |
| `new Vector2(0.5f, 1f)` | 227 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuManager. | - |
| `new Vector2(0.5f, 1f)` | 228 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuManager. | - |
| `new Vector2(0.5f, 1f)` | 229 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuManager. | - |
| `new Vector2(40f, -8f)` | 230 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuManager. | - |
| `new Vector2(360f, 30f)` | 231 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuManager. | - |
| `new Vector2(330f, -12f), 22f, TextAlignmentOptions.Right)` | 238 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuManager. | - |
| `new Vector2(120f, 42f)` | 239 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuManager. | - |
| `private void CreateSelectionRow( RectTransform parent, TextMeshProUGUI textTemplate, Button buttonTemplate, string label, float anchoredY, out TextMeshProUGUI valueText, UnityEngine.Events.UnityAction primaryAction, UnityEngine.Events.UnityAction secondaryAction)` | 242 | Tạo object/dữ liệu selection row. | - |
| `new Vector2(0.5f, 1f)` | 255 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuManager. | - |
| `new Vector2(0.5f, 1f)` | 256 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuManager. | - |
| `new Vector2(0.5f, 1f)` | 257 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuManager. | - |
| `new Vector2(0f, anchoredY)` | 258 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuManager. | - |
| `new Vector2(860f, 70f)` | 259 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuManager. | - |
| `new Vector2(-320f, -12f), 24f, TextAlignmentOptions.Left)` | 261 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuManager. | - |
| `new Vector2(300f, 42f)` | 262 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuManager. | - |
| `new Vector2(215f, -4f), new Vector2(220f, 50f))` | 266 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuManager. | - |
| `new Vector2(10f, -12f), 22f, TextAlignmentOptions.Center)` | 270 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuManager. | - |
| `new Vector2(180f, 42f)` | 271 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuManager. | - |
| `new Vector2(120f, -4f), new Vector2(80f, 50f))` | 275 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuManager. | - |
| `new Vector2(340f, -4f), new Vector2(80f, 50f))` | 279 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuManager. | - |
| `new Vector2(230f, -12f), 22f, TextAlignmentOptions.Center)` | 283 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuManager. | - |
| `new Vector2(180f, 42f)` | 284 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuManager. | - |
| `private Button CreateActionButton(RectTransform parent, Button buttonTemplate, TextMeshProUGUI textTemplate, string label, Vector2 anchoredPosition)` | 287 | Tạo object/dữ liệu action button. | - |
| `new Vector2(0.5f, 0f)` | 294 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuManager. | - |
| `new Vector2(0.5f, 0f)` | 295 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuManager. | - |
| `new Vector2(0.5f, 0f)` | 296 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuManager. | - |
| `new Vector2(320f, 60f)` | 298 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuManager. | - |
| `private Button CreateMiniButton(RectTransform parent, Button buttonTemplate, TextMeshProUGUI textTemplate, string label, Vector2 anchoredPosition, Vector2 size)` | 312 | Tạo object/dữ liệu mini button. | - |
| `new Vector2(0f, 1f)` | 319 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuManager. | - |
| `new Vector2(0f, 1f)` | 320 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuManager. | - |
| `new Vector2(0f, 1f)` | 321 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuManager. | - |
| `private TextMeshProUGUI CreateRowLabel(RectTransform parent, TextMeshProUGUI textTemplate, string label, Vector2 anchoredPosition, float fontSize, TextAlignmentOptions alignment)` | 338 | Tạo object/dữ liệu row label. | - |
| `new Vector2(0f, 1f)` | 345 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuManager. | - |
| `new Vector2(0f, 1f)` | 346 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuManager. | - |
| `new Vector2(0f, 1f)` | 347 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuManager. | - |
| `new Vector2(240f, 42f)` | 349 | Thực hiện logic vector2 trong script TitleScreenSettingsMenuManager. | - |
| `private void RefreshSettingsDisplay()` | 359 | Làm mới dữ liệu/hiển thị settings display. Liên kết trực tiếp: GameSettingsManager. | GameSettingsManager |
| `masterVolumeValueText, GetPercentLabel(settings.masterVolume))` | 370 | Lấy dữ liệu percent label cho hệ thống khác sử dụng. | - |
| `musicVolumeValueText, GetPercentLabel(settings.musicVolume))` | 371 | Lấy dữ liệu percent label cho hệ thống khác sử dụng. | - |
| `sfxVolumeValueText, GetPercentLabel(settings.sfxVolume))` | 372 | Lấy dữ liệu percent label cho hệ thống khác sử dụng. | - |
| `private void OnMasterVolumeChanged(float value)` | 379 | Thực hiện logic on master volume changed trong script TitleScreenSettingsMenuManager. | - |
| `private void OnMusicVolumeChanged(float value)` | 384 | Thực hiện logic on music volume changed trong script TitleScreenSettingsMenuManager. | - |
| `private void OnSFXVolumeChanged(float value)` | 389 | Thực hiện logic on sfxvolume changed trong script TitleScreenSettingsMenuManager. | - |
| `private void OnCameraSensitivityChanged(float value)` | 394 | Thực hiện logic on camera sensitivity changed trong script TitleScreenSettingsMenuManager. | - |
| `private void ToggleFullscreen()` | 399 | Thực hiện logic toggle fullscreen trong script TitleScreenSettingsMenuManager. | - |
| `private void CycleResolution(int direction)` | 404 | Thực hiện logic cycle resolution trong script TitleScreenSettingsMenuManager. | - |
| `private void CycleQuality(int direction)` | 409 | Thực hiện logic cycle quality trong script TitleScreenSettingsMenuManager. | - |
| `private void CopyTextStyle(TextMeshProUGUI source, TextMeshProUGUI destination)` | 414 | Thực hiện logic copy text style trong script TitleScreenSettingsMenuManager. | - |
| `private void SetSliderWithoutNotify(Slider slider, float value)` | 423 | Thiết lập giá trị hoặc trạng thái slider without notify. | - |
| `private void SetText(TextMeshProUGUI text, string value)` | 429 | Thiết lập giá trị hoặc trạng thái text. | - |
| `private string GetPercentLabel(float value)` | 435 | Lấy dữ liệu percent label cho hệ thống khác sử dụng. | - |
| `private void ApplySettingsAndRefresh(Action<GameSettingsManager> applyAction)` | 440 | Áp dụng settings and refresh lên character/object mục tiêu. Liên kết trực tiếp: GameSettingsManager. | GameSettingsManager |

#### TitleScreenSettingsMenuView

- **Đường dẫn:** `Assets/Game/Scripts/Menu Scene/TitleScreenSettingsMenuView.cs`
- **Loại:** MonoBehaviour
- **Vai trò dễ hiểu:** Điều khiển title/menu scene: load slot, setting menu, preview nhân vật hoặc bắt input menu.
- **Kế thừa/cha:** MonoBehaviour
- **Script con:** -
- **Field public/serialized chính:** [SerializeField] private RectTransform contentRoot, [SerializeField] private Slider masterVolumeSlider, [SerializeField] private Slider musicVolumeSlider, [SerializeField] private Slider sfxVolumeSlider, [SerializeField] private Slider cameraSensitivitySlider, [SerializeField] private Button fullscreenToggleButton, [SerializeField] private Button resolutionPreviousButton, [SerializeField] private Button resolutionNextButton, [SerializeField] private Button qualityPreviousButton, [SerializeField] private Button qualityNextButton, [SerializeField] private Button closeButton, [SerializeField] private TextMeshProUGUI fullscreenValueText +6
- **Liên kết script:** GameSettingsManager, GameSettingsMenuViewUtility, TitleScreenManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public void Initialize(TitleScreenManager manager)` | 39 | Thực hiện logic initialize trong script TitleScreenSettingsMenuView. | - |
| `private void OnEnable()` | 47 | Đăng ký event/callback hoặc bật trạng thái khi component được enable. Liên kết trực tiếp: GameSettingsManager. | GameSettingsManager |
| `private void OnDisable()` | 52 | Gỡ event/callback hoặc dọn trạng thái khi component bị disable. Liên kết trực tiếp: GameSettingsManager. | GameSettingsManager |
| `public void Refresh()` | 57 | Làm mới dữ liệu/hiển thị . Liên kết trực tiếp: GameSettingsManager. | GameSettingsManager |
| `masterVolumeValueText, GetPercentLabel(settings.masterVolume))` | 72 | Lấy dữ liệu percent label cho hệ thống khác sử dụng. | - |
| `musicVolumeValueText, GetPercentLabel(settings.musicVolume))` | 73 | Lấy dữ liệu percent label cho hệ thống khác sử dụng. | - |
| `sfxVolumeValueText, GetPercentLabel(settings.sfxVolume))` | 74 | Lấy dữ liệu percent label cho hệ thống khác sử dụng. | - |
| `public void CloseMenu()` | 81 | Đóng UI/trạng thái/luồng menu. | - |
| `private void AutoBindSceneReferences()` | 87 | Thực hiện logic auto bind scene references trong script TitleScreenSettingsMenuView. | - |
| `private Slider FindSlider(string path) => GameSettingsMenuViewUtility.FindSlider(contentRoot, path)` | 116 | Tìm slider trong scene/danh sách dữ liệu. Liên kết trực tiếp: GameSettingsMenuViewUtility. | GameSettingsMenuViewUtility |
| `private Button FindButton(string path) => GameSettingsMenuViewUtility.FindButton(contentRoot, path)` | 117 | Tìm button trong scene/danh sách dữ liệu. Liên kết trực tiếp: GameSettingsMenuViewUtility. | GameSettingsMenuViewUtility |
| `private TextMeshProUGUI FindText(string path) => GameSettingsMenuViewUtility.FindText(contentRoot, path)` | 118 | Tìm text trong scene/danh sách dữ liệu. Liên kết trực tiếp: GameSettingsMenuViewUtility. | GameSettingsMenuViewUtility |
| `private void BindListeners()` | 120 | Thực hiện logic bind listeners trong script TitleScreenSettingsMenuView. | - |
| `private void BindSlider(Slider slider, UnityEngine.Events.UnityAction<float> callback, float minValue, float maxValue)` | 140 | Thực hiện logic bind slider trong script TitleScreenSettingsMenuView. | - |
| `private void BindButton(Button button, UnityEngine.Events.UnityAction callback)` | 152 | Thực hiện logic bind button trong script TitleScreenSettingsMenuView. | - |
| `private void SetSliderWithoutNotify(Slider slider, float value)` | 161 | Thiết lập giá trị hoặc trạng thái slider without notify. | - |
| `private void SetText(TextMeshProUGUI text, string value)` | 167 | Thiết lập giá trị hoặc trạng thái text. | - |
| `private string GetPercentLabel(float value)` | 173 | Lấy dữ liệu percent label cho hệ thống khác sử dụng. | - |
| `private void ApplySettingsAndRefresh(Action<GameSettingsManager> applyAction)` | 178 | Áp dụng settings and refresh lên character/object mục tiêu. Liên kết trực tiếp: GameSettingsManager. | GameSettingsManager |
| `private void OnMasterVolumeChanged(float value)` | 187 | Thực hiện logic on master volume changed trong script TitleScreenSettingsMenuView. | - |
| `private void OnMusicVolumeChanged(float value)` | 192 | Thực hiện logic on music volume changed trong script TitleScreenSettingsMenuView. | - |
| `private void OnSFXVolumeChanged(float value)` | 197 | Thực hiện logic on sfxvolume changed trong script TitleScreenSettingsMenuView. | - |
| `private void OnCameraSensitivityChanged(float value)` | 202 | Thực hiện logic on camera sensitivity changed trong script TitleScreenSettingsMenuView. | - |
| `private void ToggleFullscreen()` | 207 | Thực hiện logic toggle fullscreen trong script TitleScreenSettingsMenuView. | - |
| `private void CycleResolution(int direction)` | 212 | Thực hiện logic cycle resolution trong script TitleScreenSettingsMenuView. | - |
| `private void CycleQuality(int direction)` | 217 | Thực hiện logic cycle quality trong script TitleScreenSettingsMenuView. | - |
| `private void HandleSettingsChanged()` | 222 | Xử lý luồng settings changed. | - |

### Assets/Game/Scripts/Scenes

#### EventTriggerLoadScene

- **Đường dẫn:** `Assets/Game/Scripts/Scenes/EventTriggerLoadScene.cs`
- **Loại:** MonoBehaviour
- **Vai trò dễ hiểu:** Load/unload scene hoặc bootstrap các scene world theo location.
- **Kế thừa/cha:** MonoBehaviour
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** BuildRuntimeLogger, PlayerManager, WorldLocationManager, WorldLocationSceneSet, WorldSceneManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `private void OnEnable()` | 14 | Đăng ký event/callback hoặc bật trạng thái khi component được enable. Liên kết trực tiếp: BuildRuntimeLogger. | BuildRuntimeLogger |
| `private void OnDisable()` | 22 | Gỡ event/callback hoặc dọn trạng thái khi component bị disable. Liên kết trực tiếp: BuildRuntimeLogger. | BuildRuntimeLogger |
| `private void OnTriggerEnter(Collider other)` | 28 | Xử lý khi collider khác đi vào trigger của object này. Liên kết trực tiếp: PlayerManager. | PlayerManager |
| `private void AddPlayerToArea(PlayerManager player)` | 41 | Thêm player to area vào danh sách, trạng thái hoặc dữ liệu. Liên kết trực tiếp: BuildRuntimeLogger, WorldLocationManager, WorldSceneManager. | BuildRuntimeLogger, WorldLocationManager, WorldSceneManager |
| `public void ManualTriggerForPlayer(PlayerManager player)` | 59 | Thực hiện logic manual trigger for player trong script EventTriggerLoadScene. Liên kết trực tiếp: BuildRuntimeLogger. | BuildRuntimeLogger |
| `public WorldLocationSceneSet GetArea() =>` | 79 | Lấy dữ liệu area cho hệ thống khác sử dụng. Liên kết trực tiếp: WorldLocationSceneSet. | WorldLocationSceneSet |
| `public static List<EventTriggerLoadScene> GetRegisteredTriggersSnapshot()` | 81 | Lấy dữ liệu registered triggers snapshot cho hệ thống khác sử dụng. | - |

#### WorldAdditiveSceneBootstrap

- **Đường dẫn:** `Assets/Game/Scripts/Scenes/WorldAdditiveSceneBootstrap.cs`
- **Loại:** MonoBehaviour
- **Vai trò dễ hiểu:** Load/unload scene hoặc bootstrap các scene world theo location.
- **Kế thừa/cha:** MonoBehaviour
- **Script con:** -
- **Field public/serialized chính:** [SerializeField] private List<string> additiveScenesToLoad, [SerializeField] private bool loadOnStart
- **Liên kết script:** WorldSceneManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `private IEnumerator Start()` | 14 | Chạy sau Awake để hoàn tất thiết lập ban đầu, nhất là khi cần các object khác đã sẵn sàng. Liên kết trực tiếp: WorldSceneManager. | WorldSceneManager |
| `public void SetAdditiveScenes(IReadOnlyList<string> sceneNames)` | 58 | Thiết lập giá trị hoặc trạng thái additive scenes. | - |
| `private bool IsSceneLoaded(string sceneName)` | 74 | Kiểm tra điều kiện/trạng thái scene loaded. | - |

#### WorldLocationSceneSet

- **Đường dẫn:** `Assets/Game/Scripts/Scenes/WorldLocationSceneSet.cs`
- **Loại:** ScriptableObject
- **Vai trò dễ hiểu:** Load/unload scene hoặc bootstrap các scene world theo location.
- **Kế thừa/cha:** ScriptableObject
- **Script con:** -
- **Field public/serialized chính:** public List<string> scenesRequiredForThisLocation
- **Liên kết script:** -

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public List<string> GetRequiredSceneIDsForWorldLocation()` | 16 | Lấy dữ liệu required scene ids for world location cho hệ thống khác sử dụng. | - |
| `public List<string> GetDoNotUnloadListForWorldLocation()` | 47 | Lấy dữ liệu do not unload list for world location cho hệ thống khác sử dụng. | - |

### Assets/Game/Scripts/Settings

#### GameSettingsManager

- **Đường dẫn:** `Assets/Game/Scripts/Settings/GameSettingsManager.cs`
- **Loại:** MonoBehaviour
- **Vai trò dễ hiểu:** Quản lý cấu hình game do người chơi chọn.
- **Kế thừa/cha:** MonoBehaviour
- **Script con:** -
- **Field public/serialized chính:** public static bool HasInstance
- **Liên kết script:** CharacterSoundFXManager, GameSettingsMenuViewUtility, PlayerCamera, PlayerUIManager, WorldSoundFXManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `private static void Bootstrap()` | 37 | Thực hiện logic bootstrap trong script GameSettingsManager. | - |
| `new GameObject("Game Settings Manager")` | 42 | Thực hiện logic game object trong script GameSettingsManager. | - |
| `private void Awake()` | 47 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. | - |
| `private void OnEnable()` | 63 | Đăng ký event/callback hoặc bật trạng thái khi component được enable. | - |
| `private void OnDisable()` | 68 | Gỡ event/callback hoặc dọn trạng thái khi component bị disable. | - |
| `public void InitializeIfNeeded()` | 73 | Thực hiện logic initialize if needed trong script GameSettingsManager. | - |
| `public IReadOnlyList<Resolution> GetAvailableResolutions()` | 85 | Lấy dữ liệu available resolutions cho hệ thống khác sử dụng. | - |
| `public float GetEffectiveMusicVolume()` | 91 | Lấy dữ liệu effective music volume cho hệ thống khác sử dụng. | - |
| `public float GetEffectiveSFXVolume()` | 96 | Lấy dữ liệu effective sfxvolume cho hệ thống khác sử dụng. | - |
| `public string GetCurrentResolutionLabel()` | 101 | Lấy dữ liệu current resolution label cho hệ thống khác sử dụng. | - |
| `public string GetCurrentQualityLabel()` | 110 | Lấy dữ liệu current quality label cho hệ thống khác sử dụng. | - |
| `public void SetMasterVolume(float value)` | 123 | Thiết lập giá trị hoặc trạng thái master volume. | - |
| `public void SetMusicVolume(float value)` | 128 | Thiết lập giá trị hoặc trạng thái music volume. | - |
| `public void SetSFXVolume(float value)` | 133 | Thiết lập giá trị hoặc trạng thái sfxvolume. | - |
| `public void SetCameraSensitivity(float value)` | 138 | Thiết lập giá trị hoặc trạng thái camera sensitivity. | - |
| `public void ToggleFullscreen()` | 143 | Thực hiện logic toggle fullscreen trong script GameSettingsManager. | - |
| `public void CycleResolution(int direction)` | 151 | Thực hiện logic cycle resolution trong script GameSettingsManager. | - |
| `public void CycleQuality(int direction)` | 162 | Thực hiện logic cycle quality trong script GameSettingsManager. | - |
| `private void UpdateAudioSetting(float value, Func<float, float> clampFunc, Action<float> assignAction)` | 175 | Cập nhật audio setting theo trạng thái mới. | - |
| `private int CycleIndex(int currentIndex, int direction, int itemCount)` | 183 | Thực hiện logic cycle index trong script GameSettingsManager. | - |
| `private void OnSceneLoaded(Scene scene, LoadSceneMode mode)` | 196 | Thực hiện logic on scene loaded trong script GameSettingsManager. | - |
| `private void CacheAvailableResolutions()` | 202 | Thực hiện logic cache available resolutions trong script GameSettingsManager. | - |
| `private void LoadSettings()` | 226 | Nạp dữ liệu hoặc scene liên quan tới settings. | - |
| `private void SaveSettings()` | 250 | Lưu dữ liệu liên quan tới settings. | - |
| `private void ApplyAllSettings(bool saveSettings)` | 265 | Áp dụng all settings lên character/object mục tiêu. | - |
| `private void ApplyDisplaySettings()` | 274 | Áp dụng display settings lên character/object mục tiêu. | - |
| `private void ApplyAudioAndGameplaySettings()` | 286 | Áp dụng audio and gameplay settings lên character/object mục tiêu. Liên kết trực tiếp: CharacterSoundFXManager, PlayerCamera, PlayerUIManager, WorldSoundFXManager. | CharacterSoundFXManager, PlayerCamera, PlayerUIManager, WorldSoundFXManager |
| `private void NotifySettingsChanged()` | 308 | Thực hiện logic notify settings changed trong script GameSettingsManager. | - |
| `public static Slider FindSlider(RectTransform contentRoot, string path) => FindComponentByPath<Slider>(contentRoot, path)` | 316 | Tìm slider trong scene/danh sách dữ liệu. | - |
| `public static Button FindButton(RectTransform contentRoot, string path) => FindComponentByPath<Button>(contentRoot, path)` | 317 | Tìm button trong scene/danh sách dữ liệu. | - |
| `public static TextMeshProUGUI FindText(RectTransform contentRoot, string path) => FindComponentByPath<TextMeshProUGUI>(contentRoot, path)` | 318 | Tìm text trong scene/danh sách dữ liệu. | - |

### Assets/Game/Scripts/Shop

#### ShopInteractable

- **Đường dẫn:** `Assets/Game/Scripts/Shop/ShopInteractable.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Hệ thống shop: tồn kho, entry hàng bán và tương tác mua/bán với player.
- **Kế thừa/cha:** Interactable
- **Script con:** -
- **Field public/serialized chính:** [SerializeField] private ShopInventory shopInventory
- **Liên kết script:** Interactable, PlayerManager, PlayerUIManager, ShopInventory

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `protected override void Awake()` | 10 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. Liên kết trực tiếp: ShopInventory. | ShopInventory |
| `public override void Interact(PlayerManager player)` | 21 | Thực hiện hành động tương tác khi player chọn object này. Liên kết trực tiếp: PlayerUIManager. | PlayerUIManager |

#### ShopInventory

- **Đường dẫn:** `Assets/Game/Scripts/Shop/ShopInventory.cs`
- **Loại:** MonoBehaviour
- **Vai trò dễ hiểu:** Hệ thống shop: tồn kho, entry hàng bán và tương tác mua/bán với player.
- **Kế thừa/cha:** MonoBehaviour
- **Script con:** -
- **Field public/serialized chính:** public string shopName, [SerializeField] private string merchantID, [SerializeField] private int shopProgressionTier, [SerializeField] private bool autoScaleShopTierFromProgression, [SerializeField] private int shopTierOffset, [SerializeField] private bool useGlobalPurchasableItems, [SerializeField] private List<ShopStockEntry> customStock, [SerializeField] private float buyPriceIncreasePerTier, [SerializeField] private float sellPriceIncreasePerTier
- **Liên kết script:** CharacterSaveData, GameProgressionManager, Item, ShopStockEntry, WorldItemDatabase, WorldSaveGameManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public List<ShopStockEntry> GetStockEntries()` | 21 | Lấy dữ liệu stock entries cho hệ thống khác sử dụng. Liên kết trực tiếp: ShopStockEntry. | ShopStockEntry |
| `public int GetBuyPrice(Item item)` | 46 | Lấy dữ liệu buy price cho hệ thống khác sử dụng. Liên kết trực tiếp: ShopStockEntry. | ShopStockEntry |
| `public int GetSellPrice(Item item)` | 58 | Lấy dữ liệu sell price cho hệ thống khác sử dụng. Liên kết trực tiếp: ShopStockEntry. | ShopStockEntry |
| `public int GetEffectiveShopProgressionTier()` | 67 | Lấy dữ liệu effective shop progression tier cho hệ thống khác sử dụng. | - |
| `1, GetCurrentPlayerProgressionTier() + shopTierOffset)` | 72 | Lấy dữ liệu current player progression tier cho hệ thống khác sử dụng. | - |
| `public int GetRemainingQuantity(Item item)` | 75 | Lấy dữ liệu remaining quantity cho hệ thống khác sử dụng. Liên kết trực tiếp: ShopStockEntry. | ShopStockEntry |
| `return GetRemainingQuantity(entry)` | 78 | Lấy dữ liệu remaining quantity cho hệ thống khác sử dụng. | - |
| `public bool TryPurchaseItem(Item item)` | 81 | Thử thực hiện purchase item, thường có kiểm tra điều kiện trước khi chạy. Liên kết trực tiếp: ShopStockEntry. | ShopStockEntry |
| `private ShopStockEntry GetEntryForItem(Item item)` | 100 | Lấy dữ liệu entry for item cho hệ thống khác sử dụng. Liên kết trực tiếp: ShopStockEntry. | ShopStockEntry |
| `private List<ShopStockEntry> BuildGlobalStockEntries()` | 122 | Thực hiện logic build global stock entries trong script ShopInventory. Liên kết trực tiếp: Item, ShopStockEntry, WorldItemDatabase. | Item, ShopStockEntry, WorldItemDatabase |
| `new ShopStockEntry()` | 129 | Thực hiện logic shop stock entry trong script ShopInventory. | - |
| `private int GetRemainingQuantity(ShopStockEntry entry)` | 137 | Lấy dữ liệu remaining quantity cho hệ thống khác sử dụng. Liên kết trực tiếp: CharacterSaveData. | CharacterSaveData |
| `private void SetRemainingQuantity(ShopStockEntry entry, int remainingQuantity)` | 156 | Thiết lập giá trị hoặc trạng thái remaining quantity. Liên kết trực tiếp: CharacterSaveData. | CharacterSaveData |
| `private CharacterSaveData GetCurrentCharacterData()` | 176 | Lấy dữ liệu current character data cho hệ thống khác sử dụng. Liên kết trực tiếp: WorldSaveGameManager. | WorldSaveGameManager |
| `private string GetStockSaveKey(int itemID)` | 184 | Lấy dữ liệu stock save key cho hệ thống khác sử dụng. | - |
| `private string GetResolvedMerchantID()` | 189 | Lấy dữ liệu resolved merchant id cho hệ thống khác sử dụng. | - |
| `private int GetCurrentPlayerProgressionTier()` | 197 | Lấy dữ liệu current player progression tier cho hệ thống khác sử dụng. Liên kết trực tiếp: CharacterSaveData, GameProgressionManager. | CharacterSaveData, GameProgressionManager |

#### ShopStockEntry

- **Đường dẫn:** `Assets/Game/Scripts/Shop/ShopStockEntry.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Hệ thống shop: tồn kho, entry hàng bán và tương tác mua/bán với player.
- **Kế thừa/cha:** -
- **Script con:** -
- **Field public/serialized chính:** public Item item, public int buyPriceOverride, public int sellPriceOverride, public int requiredProgressionTier, public bool useLimitedQuantity, public int startingQuantity
- **Liên kết script:** Item

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public int GetBuyPrice()` | 15 | Lấy dữ liệu buy price cho hệ thống khác sử dụng. | - |
| `public int GetSellPrice()` | 26 | Lấy dữ liệu sell price cho hệ thống khác sử dụng. | - |
| `public ShopStockEntry GetRuntimeCopy()` | 37 | Lấy dữ liệu runtime copy cho hệ thống khác sử dụng. | - |

### Assets/Game/Scripts/UI

#### UIButtonClickSoundInstaller

- **Đường dẫn:** `Assets/Game/Scripts/UI/UIButtonClickSoundInstaller.cs`
- **Loại:** MonoBehaviour
- **Vai trò dễ hiểu:** UI component độc lập như thanh máu, build-up bar, button effect, slot UI hoặc warning.
- **Kế thừa/cha:** MonoBehaviour
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** GameSettingsManager, UIButtonClickSound, WorldSoundFXManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `private static void Bootstrap()` | 18 | Thực hiện logic bootstrap trong script UIButtonClickSoundInstaller. | - |
| `new GameObject("UI Button Click Sound Installer")` | 23 | Thực hiện logic game object trong script UIButtonClickSoundInstaller. | - |
| `private void Awake()` | 28 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. | - |
| `private void OnDestroy()` | 45 | Dọn đăng ký/event/tài nguyên khi object bị hủy. | - |
| `private void Update()` | 54 | Cập nhật logic mỗi frame như input, timer, trạng thái chiến đấu hoặc UI. | - |
| `private void OnSceneLoaded(Scene scene, LoadSceneMode mode)` | 63 | Thực hiện logic on scene loaded trong script UIButtonClickSoundInstaller. | - |
| `private void InstallButtonClickSounds()` | 68 | Thực hiện logic install button click sounds trong script UIButtonClickSoundInstaller. Liên kết trực tiếp: UIButtonClickSound. | UIButtonClickSound |
| `public static void PlayClickSound()` | 83 | Phát click sound, thường là animation, sound hoặc VFX. Liên kết trực tiếp: GameSettingsManager, WorldSoundFXManager. | GameSettingsManager, WorldSoundFXManager |
| `private void Awake()` | 111 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. | - |
| `public void OnPointerClick(PointerEventData eventData)` | 116 | Thực hiện logic on pointer click trong script UIButtonClickSoundInstaller. | - |
| `public void OnSubmit(BaseEventData eventData)` | 121 | Thực hiện logic on submit trong script UIButtonClickSoundInstaller. | - |
| `private void PlayIfButtonCanClick()` | 126 | Phát if button can click, thường là animation, sound hoặc VFX. | - |

#### UIButtonScaleInstaller

- **Đường dẫn:** `Assets/Game/Scripts/UI/UIButtonScaleInstaller.cs`
- **Loại:** MonoBehaviour
- **Vai trò dễ hiểu:** UI component độc lập như thanh máu, build-up bar, button effect, slot UI hoặc warning.
- **Kế thừa/cha:** MonoBehaviour
- **Script con:** -
- **Field public/serialized chính:** [SerializeField] private string[] targetSpriteNames
- **Liên kết script:** UIButtonScaleOnInteract

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `private void Awake()` | 10 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. | - |
| `private void OnEnable()` | 15 | Đăng ký event/callback hoặc bật trạng thái khi component được enable. | - |
| `public void Install()` | 20 | Thực hiện logic install trong script UIButtonScaleInstaller. Liên kết trực tiếp: UIButtonScaleOnInteract. | UIButtonScaleOnInteract |
| `private bool IsTargetSprite(string spriteName)` | 44 | Kiểm tra điều kiện/trạng thái target sprite. | - |

#### UIButtonScaleOnInteract

- **Đường dẫn:** `Assets/Game/Scripts/UI/UIButtonScaleOnInteract.cs`
- **Loại:** MonoBehaviour
- **Vai trò dễ hiểu:** UI component độc lập như thanh máu, build-up bar, button effect, slot UI hoặc warning.
- **Kế thừa/cha:** MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
- **Script con:** -
- **Field public/serialized chính:** [SerializeField] private Vector3 hoverScale, [SerializeField] private Vector3 pressedScale, [SerializeField] private float transitionSpeed, [SerializeField] private bool useUnscaledTime
- **Liên kết script:** -

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `new Vector3(0.98f, 0.98f, 1f)` | 8 | Thực hiện logic vector3 trong script UIButtonScaleOnInteract. | - |
| `new Vector3(0.95f, 0.95f, 1f)` | 9 | Thực hiện logic vector3 trong script UIButtonScaleOnInteract. | - |
| `private void Awake()` | 18 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. | - |
| `private void OnEnable()` | 24 | Đăng ký event/callback hoặc bật trạng thái khi component được enable. | - |
| `private void OnDisable()` | 29 | Gỡ event/callback hoặc dọn trạng thái khi component bị disable. | - |
| `private void Update()` | 34 | Cập nhật logic mỗi frame như input, timer, trạng thái chiến đấu hoặc UI. | - |
| `rectTransform.localScale, GetTargetScale(), lerpFactor)` | 41 | Lấy dữ liệu target scale cho hệ thống khác sử dụng. | - |
| `public void OnPointerEnter(PointerEventData eventData)` | 44 | Thực hiện logic on pointer enter trong script UIButtonScaleOnInteract. | - |
| `public void OnPointerExit(PointerEventData eventData)` | 49 | Thực hiện logic on pointer exit trong script UIButtonScaleOnInteract. | - |
| `public void OnPointerDown(PointerEventData eventData)` | 55 | Thực hiện logic on pointer down trong script UIButtonScaleOnInteract. | - |
| `public void OnPointerUp(PointerEventData eventData)` | 60 | Thực hiện logic on pointer up trong script UIButtonScaleOnInteract. | - |
| `private Vector3 GetTargetScale()` | 65 | Lấy dữ liệu target scale cho hệ thống khác sử dụng. | - |
| `private void ResetState()` | 76 | Đưa state về trạng thái mặc định. | - |

#### UI_Boss_HP_Bar

- **Đường dẫn:** `Assets/Game/Scripts/UI/UI_Boss_HP_Bar.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** UI component độc lập như thanh máu, build-up bar, button effect, slot UI hoặc warning.
- **Kế thừa/cha:** UI_StatBar
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** AIBossCharacterManager, UI_StatBar

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public void EnableBossHPBar(AIBossCharacterManager boss)` | 10 | Bật boss hpbar. | - |
| `private void OnDestroy()` | 19 | Dọn đăng ký/event/tài nguyên khi object bị hủy. | - |
| `private void OnBossHPChanged(int oldValue, int newValue)` | 24 | Thực hiện logic on boss hpchanged trong script UI_Boss_HP_Bar. | - |
| `public void RemoveHPBar(float time)` | 34 | Loại bỏ hpbar khỏi danh sách, trạng thái hoặc dữ liệu. | - |

#### UI_BuildUpBar

- **Đường dẫn:** `Assets/Game/Scripts/UI/UI_BuildUpBar.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** UI component độc lập như thanh máu, build-up bar, button effect, slot UI hoặc warning.
- **Kế thừa/cha:** UI_StatBar
- **Script con:** -
- **Field public/serialized chính:** public BuildUp buildUpType
- **Liên kết script:** BuildUp, PlayerUIManager, UI_StatBar

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public override void SetMaxStat(int maxValue)` | 10 | Thiết lập giá trị hoặc trạng thái max stat. Liên kết trực tiếp: PlayerUIManager. | PlayerUIManager |
| `new Vector2(maxValue * widthScaleMultiplier, rectTransform.sizeDelta.y)` | 16 | Thực hiện logic vector2 trong script UI_BuildUpBar. | - |
| `public override void SetStat(int newValue)` | 31 | Thiết lập giá trị hoặc trạng thái stat. | - |

#### UI_Character_Attribute_Slider

- **Đường dẫn:** `Assets/Game/Scripts/UI/UI_Character_Attribute_Slider.cs`
- **Loại:** MonoBehaviour
- **Vai trò dễ hiểu:** UI component độc lập như thanh máu, build-up bar, button effect, slot UI hoặc warning.
- **Kế thừa/cha:** MonoBehaviour
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** CharacterAttribute, PlayerUIManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public void SetCurrentSelectedAttribute()` | 9 | Thiết lập giá trị hoặc trạng thái current selected attribute. Liên kết trực tiếp: PlayerUIManager. | PlayerUIManager |

#### UI_Character_HP_Bar

- **Đường dẫn:** `Assets/Game/Scripts/UI/UI_Character_HP_Bar.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** UI component độc lập như thanh máu, build-up bar, button effect, slot UI hoặc warning.
- **Kế thừa/cha:** UI_StatBar
- **Script con:** -
- **Field public/serialized chính:** public int currentDamageTaken, public int oldHealthValue
- **Liên kết script:** AICharacterManager, CharacterManager, PlayerManager, UI_StatBar, WorldUtilityManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `protected override void Awake()` | 21 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. Liên kết trực tiếp: AICharacterManager, CharacterManager, PlayerManager. | AICharacterManager, CharacterManager, PlayerManager |
| `private bool EnsureUiReferences()` | 34 | Thực hiện logic ensure ui references trong script UI_Character_HP_Bar. | - |
| `protected override void Start()` | 48 | Chạy sau Awake để hoàn tất thiết lập ban đầu, nhất là khi cần các object khác đã sẵn sàng. | - |
| `public override void SetStat(int newValue)` | 55 | Thiết lập giá trị hoặc trạng thái stat. Liên kết trực tiếp: WorldUtilityManager. | WorldUtilityManager |
| `private void Update()` | 114 | Cập nhật logic mỗi frame như input, timer, trạng thái chiến đấu hoặc UI. | - |
| `private void OnDisable()` | 129 | Gỡ event/callback hoặc dọn trạng thái khi component bị disable. | - |

#### UI_Character_Save_Slot

- **Đường dẫn:** `Assets/Game/Scripts/UI/UI_Character_Save_Slot.cs`
- **Loại:** MonoBehaviour
- **Vai trò dễ hiểu:** UI component độc lập như thanh máu, build-up bar, button effect, slot UI hoặc warning.
- **Kế thừa/cha:** MonoBehaviour
- **Script con:** -
- **Field public/serialized chính:** public CharacterSlot characterSlot, public TextMeshProUGUI characterName, public TextMeshProUGUI timePlayed
- **Liên kết script:** CharacterSaveData, CharacterSlot, SaveFileDataWriter, TitleScreenManager, WorldSaveGameManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `private void OnEnable()` | 17 | Đăng ký event/callback hoặc bật trạng thái khi component được enable. | - |
| `private void LoadSaveSlots()` | 22 | Nạp dữ liệu hoặc scene liên quan tới save slots. Liên kết trực tiếp: CharacterSaveData, SaveFileDataWriter, WorldSaveGameManager. | CharacterSaveData, SaveFileDataWriter, WorldSaveGameManager |
| `new SaveFileDataWriter()` | 26 | Lưu dữ liệu liên quan tới file data writer. | - |
| `public void LoadGameFromCharacterSlot()` | 42 | Nạp dữ liệu hoặc scene liên quan tới game from character slot. Liên kết trực tiếp: WorldSaveGameManager. | WorldSaveGameManager |
| `public void SelectCurrentSlot()` | 48 | Thực hiện logic select current slot trong script UI_Character_Save_Slot. Liên kết trực tiếp: TitleScreenManager. | TitleScreenManager |

#### UI_Color_Button

- **Đường dẫn:** `Assets/Game/Scripts/UI/UI_Color_Button.cs`
- **Loại:** MonoBehaviour
- **Vai trò dễ hiểu:** UI component độc lập như thanh máu, build-up bar, button effect, slot UI hoặc warning.
- **Kế thừa/cha:** MonoBehaviour
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** TitleScreenManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `private void Awake()` | 15 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. | - |
| `public void SetSliderValuesToColor()` | 22 | Thiết lập giá trị hoặc trạng thái slider values to color. Liên kết trực tiếp: TitleScreenManager. | TitleScreenManager |
| `public void ConfirmColor()` | 30 | Thực hiện logic confirm color trong script UI_Color_Button. Liên kết trực tiếp: TitleScreenManager. | TitleScreenManager |

#### UI_EquipmentInventorySlot

- **Đường dẫn:** `Assets/Game/Scripts/UI/UI_EquipmentInventorySlot.cs`
- **Loại:** MonoBehaviour
- **Vai trò dễ hiểu:** UI component độc lập như thanh máu, build-up bar, button effect, slot UI hoặc warning.
- **Kế thừa/cha:** MonoBehaviour
- **Script con:** -
- **Field public/serialized chính:** public Image itemIcon, public Image highlightedIcon, [SerializeField] public Item currentItem
- **Liên kết script:** BodyEquipmentItem, EquipmentType, HandEquipmentItem, HeadEquipmentItem, Item, LegEquipmentItem, PlayerManager, PlayerUIManager, QuickSlotItem, RangedProjectileItem, WeaponItem, WorldItemDatabase

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public void AddItem(Item item)` | 13 | Thêm item vào danh sách, trạng thái hoặc dữ liệu. | - |
| `public void SelectSlot()` | 27 | Thực hiện logic select slot trong script UI_EquipmentInventorySlot. | - |
| `public void DeselectSlot()` | 32 | Thực hiện logic deselect slot trong script UI_EquipmentInventorySlot. | - |
| `public void EquipItem()` | 37 | Trang bị item và cập nhật model/chỉ số liên quan. Liên kết trực tiếp: BodyEquipmentItem, EquipmentType, HandEquipmentItem, HeadEquipmentItem, Item +7. | BodyEquipmentItem, EquipmentType, HandEquipmentItem, HeadEquipmentItem, Item, LegEquipmentItem, PlayerManager, PlayerUIManager, QuickSlotItem, RangedProjectileItem +2 |

#### UI_Match_Scroll_Wheel_To_Selected_Button

- **Đường dẫn:** `Assets/Game/Scripts/UI/UI_Match_Scroll_Wheel_To_Selected_Button.cs`
- **Loại:** MonoBehaviour
- **Vai trò dễ hiểu:** UI component độc lập như thanh máu, build-up bar, button effect, slot UI hoặc warning.
- **Kế thừa/cha:** MonoBehaviour
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** -

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `private void Update()` | 15 | Cập nhật logic mỗi frame như input, timer, trạng thái chiến đấu hoặc UI. | - |
| `private void SnapTo(RectTransform target)` | 30 | Thực hiện logic snap to trong script UI_Match_Scroll_Wheel_To_Selected_Button. | - |

#### UI_StatBar

- **Đường dẫn:** `Assets/Game/Scripts/UI/UI_StatBar.cs`
- **Loại:** MonoBehaviour
- **Vai trò dễ hiểu:** UI component độc lập như thanh máu, build-up bar, button effect, slot UI hoặc warning.
- **Kế thừa/cha:** MonoBehaviour
- **Script con:** UI_Boss_HP_Bar, UI_BuildUpBar, UI_Character_HP_Bar
- **Field public/serialized chính:** [SerializeField] protected bool scaleBarLengthWithStats, [SerializeField] protected float widthScaleMultiplier, [SerializeField] protected Image barFillImage, [SerializeField] protected Color barFillColor
- **Liên kết script:** PlayerUIManager, WorldUtilityManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `protected virtual void Awake()` | 20 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. | - |
| `protected virtual void Start()` | 27 | Chạy sau Awake để hoàn tất thiết lập ban đầu, nhất là khi cần các object khác đã sẵn sàng. | - |
| `public virtual void SetStat(int newValue)` | 32 | Thiết lập giá trị hoặc trạng thái stat. | - |
| `public virtual void SetMaxStat(int maxValue)` | 40 | Thiết lập giá trị hoặc trạng thái max stat. Liên kết trực tiếp: PlayerUIManager. | PlayerUIManager |
| `new Vector2(maxValue * widthScaleMultiplier, rectTransform.sizeDelta.y)` | 53 | Thực hiện logic vector2 trong script UI_StatBar. | - |
| `public void ToggleBarFillColor(bool isPoisoned)` | 59 | Thực hiện logic toggle bar fill color trong script UI_StatBar. Liên kết trực tiếp: WorldUtilityManager. | WorldUtilityManager |
| `public void SetBarFillColor(Color color)` | 77 | Thiết lập giá trị hoặc trạng thái bar fill color. | - |
| `public void ResetBarFillColor()` | 87 | Đưa bar fill color về trạng thái mặc định. | - |
| `protected void EnsureFillImageReference()` | 97 | Thực hiện logic ensure fill image reference trong script UI_StatBar. | - |

#### UI_StatusEffectWarning

- **Đường dẫn:** `Assets/Game/Scripts/UI/UI_StatusEffectWarning.cs`
- **Loại:** MonoBehaviour
- **Vai trò dễ hiểu:** UI component độc lập như thanh máu, build-up bar, button effect, slot UI hoặc warning.
- **Kế thừa/cha:** MonoBehaviour
- **Script con:** -
- **Field public/serialized chính:** public CanvasGroup canvas
- **Liên kết script:** BuildUp

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `new Color(1f, 0.45f, 0.1f, 1f)` | 13 | Thực hiện logic color trong script UI_StatusEffectWarning. | - |
| `public void SetWarningMessage(BuildUp status)` | 17 | Thiết lập giá trị hoặc trạng thái warning message. Liên kết trực tiếp: BuildUp. | BuildUp |
| `public void SetCustomMessage(string message, Color color)` | 38 | Thiết lập giá trị hoặc trạng thái custom message. | - |

### Assets/Game/Scripts/Utility

#### GameAssetPaths

- **Đường dẫn:** `Assets/Game/Scripts/Utility/GameAssetPaths.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Helper nhỏ dùng chung trong scene hoặc prefab.
- **Kế thừa/cha:** -
- **Script con:** -
- **Field public/serialized chính:** public const string AssetsRoot, public const string GameRoot, public const string AddonsRoot, public const string ArtRoot, public const string DataRoot, public const string DocsRoot, public const string PrefabsRoot, public const string MaterialsRoot, public const string ResourcesRoot, public const string ScenesRoot, public const string SystemRoot, public const string PolygonDungeonRoot +3
- **Liên kết script:** -

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| - | - | Script này chủ yếu là dữ liệu/enum/struct, không có method rõ ràng. | - |

#### SpellInstantiationLocation

- **Đường dẫn:** `Assets/Game/Scripts/Utility/SpellInstantiationLocation.cs`
- **Loại:** MonoBehaviour
- **Vai trò dễ hiểu:** Helper nhỏ dùng chung trong scene hoặc prefab.
- **Kế thừa/cha:** MonoBehaviour
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** -

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| - | - | Script này chủ yếu là dữ liệu/enum/struct, không có method rõ ràng. | - |

#### Utility_DestroyAfterTime

- **Đường dẫn:** `Assets/Game/Scripts/Utility/Utility_DestroyAfterTime.cs`
- **Loại:** MonoBehaviour
- **Vai trò dễ hiểu:** Helper nhỏ dùng chung trong scene hoặc prefab.
- **Kế thừa/cha:** MonoBehaviour
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** -

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public void SetLifetime(float lifetime)` | 9 | Thiết lập giá trị hoặc trạng thái lifetime. | - |
| `private void Awake()` | 14 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. | - |

### Assets/Game/Scripts/Weapon Actions

#### AimAction

- **Đường dẫn:** `Assets/Game/Scripts/Weapon Actions/AimAction.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Action ScriptableObject cho vũ khí/spell, được combat manager gọi khi player thực hiện hành động.
- **Kế thừa/cha:** WeaponItemAction
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** PlayerManager, WeaponItem, WeaponItemAction

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public override void AttemptToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)` | 8 | Cố gắng kích hoạt to perform action nếu trạng thái hiện tại cho phép. | - |

#### CastIncantationAction

- **Đường dẫn:** `Assets/Game/Scripts/Weapon Actions/CastIncantationAction.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Action ScriptableObject cho vũ khí/spell, được combat manager gọi khi player thực hiện hành động.
- **Kế thừa/cha:** WeaponItemAction
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** PlayerManager, SpellClass, WeaponItem, WeaponItemAction

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public override void AttemptToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)` | 8 | Cố gắng kích hoạt to perform action nếu trạng thái hiện tại cho phép. Liên kết trực tiếp: SpellClass. | SpellClass |
| `private void CastIncantation(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)` | 37 | Thực hiện logic cast incantation trong script CastIncantationAction. | - |

#### FireProjectileAction

- **Đường dẫn:** `Assets/Game/Scripts/Weapon Actions/FireProjectileAction.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Action ScriptableObject cho vũ khí/spell, được combat manager gọi khi player thực hiện hành động.
- **Kế thừa/cha:** WeaponItemAction
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** PlayerManager, ProjectileSlot, RangedProjectileItem, WeaponItem, WeaponItemAction

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public override void AttemptToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)` | 10 | Cố gắng kích hoạt to perform action nếu trạng thái hiện tại cho phép. Liên kết trực tiếp: ProjectileSlot, RangedProjectileItem. | ProjectileSlot, RangedProjectileItem |
| `private bool CanIFireThisProjectile(WeaponItem weaponPerformingAction, RangedProjectileItem projectileItem)` | 88 | Kiểm tra có được phép ifire this projectile hay không. | - |

#### HeavyAttackWeaponItemAction

- **Đường dẫn:** `Assets/Game/Scripts/Weapon Actions/HeavyAttackWeaponItemAction.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Action ScriptableObject cho vũ khí/spell, được combat manager gọi khi player thực hiện hành động.
- **Kế thừa/cha:** WeaponItemAction
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** AttackType, PlayerManager, WeaponItem, WeaponItemAction

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public override void AttemptToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)` | 16 | Cố gắng kích hoạt to perform action nếu trạng thái hiện tại cho phép. | - |
| `private void PerformHeavyAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)` | 43 | Thực hiện logic perform heavy attack trong script HeavyAttackWeaponItemAction. | - |
| `private void PerformMainHandHeavyAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)` | 56 | Thực hiện logic perform main hand heavy attack trong script HeavyAttackWeaponItemAction. Liên kết trực tiếp: AttackType. | AttackType |
| `private void PerformTwoHandHeavyAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)` | 78 | Thực hiện logic perform two hand heavy attack trong script HeavyAttackWeaponItemAction. Liên kết trực tiếp: AttackType. | AttackType |
| `private void PerformJumpingHeavyAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)` | 100 | Thực hiện logic perform jumping heavy attack trong script HeavyAttackWeaponItemAction. | - |
| `private void PerformMainHandJumpingAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)` | 112 | Thực hiện logic perform main hand jumping attack trong script HeavyAttackWeaponItemAction. Liên kết trực tiếp: AttackType. | AttackType |
| `private void PerformTwoHandJumpingAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)` | 120 | Thực hiện logic perform two hand jumping attack trong script HeavyAttackWeaponItemAction. Liên kết trực tiếp: AttackType. | AttackType |

#### LightAttackWeaponItemAction

- **Đường dẫn:** `Assets/Game/Scripts/Weapon Actions/LightAttackWeaponItemAction.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Action ScriptableObject cho vũ khí/spell, được combat manager gọi khi player thực hiện hành động.
- **Kế thừa/cha:** WeaponItemAction
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** AttackType, PlayerManager, WeaponItem, WeaponItemAction

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public override void AttemptToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)` | 38 | Cố gắng kích hoạt to perform action nếu trạng thái hiện tại cho phép. | - |
| `private void PerformLightAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)` | 87 | Thực hiện logic perform light attack trong script LightAttackWeaponItemAction. | - |
| `private void PerformMainHandLightAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)` | 99 | Thực hiện logic perform main hand light attack trong script LightAttackWeaponItemAction. Liên kết trực tiếp: AttackType. | AttackType |
| `private void PerformTwoHandLightAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)` | 122 | Thực hiện logic perform two hand light attack trong script LightAttackWeaponItemAction. Liên kết trực tiếp: AttackType. | AttackType |
| `private void PerformRunningAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)` | 145 | Thực hiện logic perform running attack trong script LightAttackWeaponItemAction. Liên kết trực tiếp: AttackType. | AttackType |
| `private void PerformRollingAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)` | 158 | Thực hiện logic perform rolling attack trong script LightAttackWeaponItemAction. Liên kết trực tiếp: AttackType. | AttackType |
| `private void PerformBackstepAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)` | 172 | Thực hiện logic perform backstep attack trong script LightAttackWeaponItemAction. Liên kết trực tiếp: AttackType. | AttackType |
| `private void PerformJumpingLightAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)` | 187 | Thực hiện logic perform jumping light attack trong script LightAttackWeaponItemAction. | - |
| `private void PerformMainHandJumpingLightAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)` | 199 | Thực hiện logic perform main hand jumping light attack trong script LightAttackWeaponItemAction. Liên kết trực tiếp: AttackType. | AttackType |
| `private void PerformTwoHandJumpingLightAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)` | 207 | Thực hiện logic perform two hand jumping light attack trong script LightAttackWeaponItemAction. Liên kết trực tiếp: AttackType. | AttackType |

#### OffHandMeleeAction

- **Đường dẫn:** `Assets/Game/Scripts/Weapon Actions/OffHandMeleeAction.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Action ScriptableObject cho vũ khí/spell, được combat manager gọi khi player thực hiện hành động.
- **Kế thừa/cha:** WeaponItemAction
- **Script con:** -
- **Field public/serialized chính:** -
- **Liên kết script:** AttackType, PlayerManager, WeaponItem, WeaponItemAction

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public override void AttemptToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)` | 16 | Cố gắng kích hoạt to perform action nếu trạng thái hiện tại cho phép. | - |
| `private void PerformPowerStanceLeftHandAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)` | 60 | Thực hiện logic perform power stance left hand action trong script OffHandMeleeAction. Liên kết trực tiếp: AttackType. | AttackType |

#### WeaponItemAction

- **Đường dẫn:** `Assets/Game/Scripts/Weapon Actions/WeaponItemAction.cs`
- **Loại:** ScriptableObject
- **Vai trò dễ hiểu:** Action ScriptableObject cho vũ khí/spell, được combat manager gọi khi player thực hiện hành động.
- **Kế thừa/cha:** ScriptableObject
- **Script con:** AimAction, CastIncantationAction, FireProjectileAction, HeavyAttackWeaponItemAction, LightAttackWeaponItemAction, OffHandMeleeAction
- **Field public/serialized chính:** public int actionID
- **Liên kết script:** PlayerManager, WeaponItem

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public virtual void AttemptToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)` | 10 | Cố gắng kích hoạt to perform action nếu trạng thái hiện tại cho phép. | - |

### Assets/Game/Scripts/World Managers

#### MapProgressionDefinition

- **Đường dẫn:** `Assets/Game/Scripts/World Managers/GameProgressionConfig.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Quản lý cấp world: điều phối hệ thống dùng chung như scene, save, object, AI, âm thanh, item hoặc progression.
- **Kế thừa/cha:** -
- **Script con:** -
- **Field public/serialized chính:** public string mapName, public int sceneBuildIndex, public int bossID, public int entrySiteOfGraceID, public float enemyHealthMultiplier, public float enemyDamageMultiplier, public MapProgressionDefinition[] mapDefinitions
- **Liên kết script:** GameProgressionConfig

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| - | - | Script này chủ yếu là dữ liệu/enum/struct, không có method rõ ràng. | - |

#### GameProgressionManager

- **Đường dẫn:** `Assets/Game/Scripts/World Managers/GameProgressionManager.cs`
- **Loại:** MonoBehaviour
- **Vai trò dễ hiểu:** Quản lý cấp world: điều phối hệ thống dùng chung như scene, save, object, AI, âm thanh, item hoặc progression.
- **Kế thừa/cha:** MonoBehaviour
- **Script con:** -
- **Field public/serialized chính:** public static GameProgressionManager instance, [SerializeField] private GameProgressionConfig progressionConfig, [SerializeField] private MapProgressionDefinition[] mapDefinitions, [SerializeField] private int startingClassID, [SerializeField] private int currentMapIndex, [SerializeField] private bool gameWon, [SerializeField] private int pendingTransitionSiteOfGraceID, public int StartingClassID, public int CurrentMapIndex, public bool GameWon
- **Liên kết script:** CharacterSaveData, GameProgressionConfig, MapProgressionDefinition, SerializableDictionary, WorldSaveGameManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public static void EnsureInstance()` | 48 | Thực hiện logic ensure instance trong script GameProgressionManager. | - |
| `new GameObject("Game Progression Manager")` | 58 | Thực hiện logic game object trong script GameProgressionManager. | - |
| `private void Awake()` | 62 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. | - |
| `private void OnValidate()` | 84 | Tự kiểm tra/cập nhật giá trị trong Unity Editor khi inspector thay đổi. | - |
| `public void ResetForNewGame(int selectedStartingClassID)` | 95 | Đưa for new game về trạng thái mặc định. | - |
| `public void LoadFromCharacterData(CharacterSaveData characterData)` | 108 | Nạp dữ liệu hoặc scene liên quan tới from character data. | - |
| `public void SaveToCharacterData(CharacterSaveData characterData)` | 142 | Lưu dữ liệu liên quan tới to character data. | - |
| `public bool RegisterBossDefeat(int bossID, out int nextSceneBuildIndex, out int unlockedMapIndex, out bool hasWonGame)` | 163 | Thực hiện logic register boss defeat trong script GameProgressionManager. | - |
| `public bool IsMapUnlocked(int mapIndex)` | 207 | Kiểm tra điều kiện/trạng thái map unlocked. | - |
| `public int GetSceneBuildIndexForCurrentMap(int fallbackSceneBuildIndex = DefaultWorldSceneBuildIndex)` | 217 | Lấy dữ liệu scene build index for current map cho hệ thống khác sử dụng. | - |
| `return GetSceneBuildIndexForMap(currentMapIndex, fallbackSceneBuildIndex)` | 219 | Lấy dữ liệu scene build index for map cho hệ thống khác sử dụng. | - |
| `public int GetMapIndexForSceneBuildIndex(int sceneBuildIndex)` | 222 | Lấy dữ liệu map index for scene build index cho hệ thống khác sử dụng. | - |
| `public string GetMapName(int mapIndex)` | 235 | Lấy dữ liệu map name cho hệ thống khác sử dụng. Liên kết trực tiếp: MapProgressionDefinition. | MapProgressionDefinition |
| `public int GetEntrySiteOfGraceIDForCurrentMap()` | 251 | Lấy dữ liệu entry site of grace idfor current map cho hệ thống khác sử dụng. | - |
| `return GetEntrySiteOfGraceIDForMap(currentMapIndex)` | 253 | Lấy dữ liệu entry site of grace idfor map cho hệ thống khác sử dụng. | - |
| `public float GetEnemyHealthMultiplierForCurrentMap()` | 256 | Lấy dữ liệu enemy health multiplier for current map cho hệ thống khác sử dụng. | - |
| `return GetEnemyHealthMultiplierForMap(currentMapIndex)` | 258 | Lấy dữ liệu enemy health multiplier for map cho hệ thống khác sử dụng. | - |
| `public float GetEnemyDamageMultiplierForCurrentMap()` | 261 | Lấy dữ liệu enemy damage multiplier for current map cho hệ thống khác sử dụng. | - |
| `return GetEnemyDamageMultiplierForMap(currentMapIndex)` | 263 | Lấy dữ liệu enemy damage multiplier for map cho hệ thống khác sử dụng. | - |
| `public int ConsumePendingTransitionSiteOfGraceID()` | 266 | Thực hiện logic consume pending transition site of grace id trong script GameProgressionManager. | - |
| `public bool HasPendingTransitionSiteOfGrace()` | 273 | Thực hiện logic has pending transition site of grace trong script GameProgressionManager. | - |
| `public void SetCurrentMapIndex(int mapIndex)` | 278 | Thiết lập giá trị hoặc trạng thái current map index. | - |
| `public void SetPendingTransitionSiteOfGraceID(int siteOfGraceID)` | 285 | Thiết lập giá trị hoặc trạng thái pending transition site of grace id. | - |
| `public bool PrepareTransitionToMap(int mapIndex, out int sceneBuildIndex)` | 290 | Thực hiện logic prepare transition to map trong script GameProgressionManager. | - |
| `private void UnlockMap(int mapIndex)` | 303 | Thực hiện logic unlock map trong script GameProgressionManager. | - |
| `private bool HaveAllConfiguredBossesBeenDefeated()` | 311 | Thực hiện logic have all configured bosses been defeated trong script GameProgressionManager. Liên kết trực tiếp: MapProgressionDefinition, SerializableDictionary, WorldSaveGameManager. | MapProgressionDefinition, SerializableDictionary, WorldSaveGameManager |
| `private int GetMapIndexForBossID(int bossID)` | 339 | Lấy dữ liệu map index for boss id cho hệ thống khác sử dụng. | - |
| `private int GetSceneBuildIndexForMap(int mapIndex, int fallbackSceneBuildIndex = DefaultWorldSceneBuildIndex)` | 350 | Lấy dữ liệu scene build index for map cho hệ thống khác sử dụng. | - |
| `private int GetEntrySiteOfGraceIDForMap(int mapIndex)` | 364 | Lấy dữ liệu entry site of grace idfor map cho hệ thống khác sử dụng. Liên kết trực tiếp: MapProgressionDefinition. | MapProgressionDefinition |
| `private void EnsureConfigurationIsValid()` | 378 | Thực hiện logic ensure configuration is valid trong script GameProgressionManager. Liên kết trực tiếp: MapProgressionDefinition, SerializableDictionary. | MapProgressionDefinition, SerializableDictionary |
| `private void SyncDefinitionsFromConfigIfPresent()` | 445 | Thực hiện logic sync definitions from config if present trong script GameProgressionManager. Liên kết trực tiếp: MapProgressionDefinition. | MapProgressionDefinition |
| `private float GetEnemyHealthMultiplierForMap(int mapIndex)` | 475 | Lấy dữ liệu enemy health multiplier for map cho hệ thống khác sử dụng. Liên kết trực tiếp: MapProgressionDefinition. | MapProgressionDefinition |
| `private float GetEnemyDamageMultiplierForMap(int mapIndex)` | 489 | Lấy dữ liệu enemy damage multiplier for map cho hệ thống khác sử dụng. Liên kết trực tiếp: MapProgressionDefinition. | MapProgressionDefinition |
| `private void ValidateConfigurationAndLogWarnings()` | 503 | Thực hiện logic validate configuration and log warnings trong script GameProgressionManager. Liên kết trực tiếp: MapProgressionDefinition. | MapProgressionDefinition |

#### MapTileset

- **Đường dẫn:** `Assets/Game/Scripts/World Managers/RandomMapGenerator.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Quản lý cấp world: điều phối hệ thống dùng chung như scene, save, object, AI, âm thanh, item hoặc progression.
- **Kế thừa/cha:** -
- **Script con:** -
- **Field public/serialized chính:** public GameObject[] floorPrefabs, public GameObject[] wallPrefabs, public GameObject[] wallArchPrefabs, public GameObject[] wallArchCornerPrefabs, public GameObject[] wallArchOuterCornerPrefabs, public GameObject[] ceilingPrefabs, public GameObject[] pillarPrefabs, public GameObject[] doorwayPrefabs, public GameObject[] stairPrefabs, public GameObject[] propPrefabs, public GameObject[] decorationPrefabs, public GameObject[] ruinPrefabs +46
- **Liên kết script:** GameAssetPaths, GeneratedZoneInfo, MapGenerationConfig, RandomMapGenerator, SiteOfGraceInteractable

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `new Color(0.8490566f, 0.657692f, 0.42052332f)` | 87 | Thực hiện logic color trong script MapTileset. | - |
| `new Color(0.8490566f, 0.657692f, 0.42052332f)` | 90 | Thực hiện logic color trong script MapTileset. | - |
| `public GeneratedZoneInfo(string name, Bounds bounds)` | 113 | Thực hiện logic generated zone info trong script MapTileset. | - |
| `public bool ContainsPosition(Vector3 position)` | 121 | Thực hiện logic contains position trong script MapTileset. | - |
| `public float SqrDistanceTo(Vector3 position)` | 138 | Thực hiện logic sqr distance to trong script MapTileset. | - |
| `public float SqrDistanceToCoverageCenter(Vector3 position)` | 153 | Thực hiện logic sqr distance to coverage center trong script MapTileset. | - |
| `public float GetMaxOverlapAreaXZ(Bounds bounds)` | 168 | Lấy dữ liệu max overlap area xz cho hệ thống khác sử dụng. | - |
| `bestArea, GetOverlapAreaXZ(coverageBounds[i], bounds))` | 177 | Lấy dữ liệu overlap area xz cho hệ thống khác sử dụng. | - |
| `public float GetRoomOverlapAreaXZ(Bounds bounds)` | 183 | Lấy dữ liệu room overlap area xz cho hệ thống khác sử dụng. | - |
| `return GetOverlapAreaXZ(zoneBounds, bounds)` | 185 | Lấy dữ liệu overlap area xz cho hệ thống khác sử dụng. | - |
| `private static float GetOverlapAreaXZ(Bounds a, Bounds b)` | 188 | Lấy dữ liệu overlap area xz cho hệ thống khác sử dụng. | - |
| `new MapTileset()` | 212 | Thực hiện logic map tileset trong script MapTileset. | - |
| `new MapGenerationConfig()` | 215 | Thực hiện logic map generation config trong script MapTileset. | - |
| `public void GenerateMap()` | 251 | Thực hiện logic generate map trong script MapTileset. Liên kết trực tiếp: RandomMapGenerator. | RandomMapGenerator |
| `public void ClearMap()` | 312 | Thực hiện logic clear map trong script MapTileset. | - |
| `UNITY_EDITOR DestroyImmediate(t.gameObject)` | 329 | Thực hiện logic destroy immediate trong script MapTileset. | - |
| `else Destroy(t.gameObject)` | 331 | Thực hiện logic destroy trong script MapTileset. | - |
| `private void PlaceRooms()` | 341 | Thực hiện logic place rooms trong script MapTileset. | - |
| `new RectInt(x, z, w, h)` | 353 | Thực hiện logic rect int trong script MapTileset. | - |
| `new RectInt(r.x - 1, r.y - 1, r.width + 2, r.height + 2)` | 358 | Thực hiện logic rect int trong script MapTileset. | - |
| `private void CarveRoom(RectInt room)` | 371 | Thực hiện logic carve room trong script MapTileset. | - |
| `private void ConnectRoomsWithCorridors()` | 378 | Thực hiện logic connect rooms with corridors trong script MapTileset. | - |
| `private void CarveHCorridor(int x1, int x2, int z)` | 389 | Thực hiện logic carve hcorridor trong script MapTileset. | - |
| `private void CarveVCorridor(int z1, int z2, int x)` | 397 | Thực hiện logic carve vcorridor trong script MapTileset. | - |
| `private void SetFloor(int x, int z, int halfWidth = 1)` | 405 | Thiết lập giá trị hoặc trạng thái floor. | - |
| `private Vector2Int RoomCenter(RectInt r) => new Vector2Int(r.x + r.width / 2, r.y + r.height / 2)` | 416 | Thực hiện logic room center trong script MapTileset. | - |
| `private void DetectTileSize()` | 423 | Thực hiện logic detect tile size trong script MapTileset. Liên kết trực tiếp: RandomMapGenerator. | RandomMapGenerator |
| `UNITY_EDITOR DestroyImmediate(temp)` | 447 | Thực hiện logic destroy immediate trong script MapTileset. | - |
| `else Destroy(temp)` | 449 | Thực hiện logic destroy trong script MapTileset. | - |
| `private void DetectWallYBase()` | 460 | Thực hiện logic detect wall ybase trong script MapTileset. Liên kết trực tiếp: RandomMapGenerator. | RandomMapGenerator |
| `UNITY_EDITOR DestroyImmediate(temp)` | 492 | Thực hiện logic destroy immediate trong script MapTileset. | - |
| `else Destroy(temp)` | 494 | Thực hiện logic destroy trong script MapTileset. | - |
| `private void DetectWallArchOffset()` | 501 | Thực hiện logic detect wall arch offset trong script MapTileset. Liên kết trực tiếp: RandomMapGenerator. | RandomMapGenerator |
| `UNITY_EDITOR DestroyImmediate(temp)` | 517 | Thực hiện logic destroy immediate trong script MapTileset. | - |
| `else Destroy(temp)` | 519 | Thực hiện logic destroy trong script MapTileset. | - |
| `private Vector3 T(int x, int z, float y = 0f) => new Vector3(x * detectedStepX, y, z * detectedStepZ)` | 529 | Thực hiện logic t trong script MapTileset. | - |
| `private Vector3 TEdge(int tileX, int tileZ, int dirX, int dirZ, float y = 0f) => new Vector3( tileX * detectedStepX + dirX * detectedStepX * 0.5f, y, tileZ * detectedStepZ + dirZ * detectedStepZ * 0.5f )` | 533 | Thực hiện logic tedge trong script MapTileset. | - |
| `private void BuildFloors()` | 545 | Thực hiện logic build floors trong script MapTileset. | - |
| `tileset.floorPrefabs, T(x, z, 0f), Quaternion.identity, parent)` | 558 | Thực hiện logic t trong script MapTileset. | - |
| `private void BuildWalls()` | 573 | Thực hiện logic build walls trong script MapTileset. | - |
| `new Vector3(wx, detectedWallYBase - WallYSink, wz)` | 643 | Thực hiện logic vector3 trong script MapTileset. | - |
| `private void BuildWallArches()` | 658 | Thực hiện logic build wall arches trong script MapTileset. | - |
| `new Vector3(-outward.x, 0f, -outward.y)` | 695 | Thực hiện logic vector3 trong script MapTileset. | - |
| `d, GetWallArchY(), rotation)` | 697 | Lấy dữ liệu wall arch y cho hệ thống khác sử dụng. | - |
| `private bool HasBoundaryWall(Vector2Int floorTile, Vector2Int outward)` | 707 | Thực hiện logic has boundary wall trong script MapTileset. | - |
| `private bool HasPerpendicularWallAtEitherEnd(Vector2Int floorTile, Vector2Int outward)` | 713 | Thực hiện logic has perpendicular wall at either end trong script MapTileset. | - |
| `new Vector2Int(floorTile.x + dx, floorTile.y + dz)` | 729 | Thực hiện logic vector2 int trong script MapTileset. | - |
| `private void TryPlaceWallArchCorner(Vector2Int endpoint, Vector2Int outward, Vector2Int[] dirs, Transform parent, HashSet<string> placedCorners)` | 748 | Thử thực hiện place wall arch corner, thường có kiểm tra điều kiện trước khi chạy. | - |
| `private bool HasPerpendicularBoundaryWallAtEndpoint(Vector2Int endpoint, Vector2Int outward, Vector2Int[] dirs)` | 780 | Thực hiện logic has perpendicular boundary wall at endpoint trong script MapTileset. | - |
| `new Vector2Int(endpoint.x / 2 + dx, endpoint.y / 2 + dz)` | 786 | Thực hiện logic vector2 int trong script MapTileset. | - |
| `private bool IsPerpendicular(Vector2Int a, Vector2Int b)` | 804 | Kiểm tra điều kiện/trạng thái perpendicular. | - |
| `private bool SharesEndpoint(Vector2Int endpoint, Vector2Int otherA, Vector2Int otherB)` | 809 | Thực hiện logic shares endpoint trong script MapTileset. | - |
| `private bool IsBlockedCornerEndpoint(Vector2Int endpoint, Vector2Int otherA, Vector2Int otherB)` | 814 | Kiểm tra điều kiện/trạng thái blocked corner endpoint. | - |
| `private int CountFloorTilesAroundEndpoint(Vector2Int endpoint2)` | 819 | Thực hiện logic count floor tiles around endpoint trong script MapTileset. | - |
| `new Vector2Int((endpoint2.x + xSign) / 2, (endpoint2.y + zSign) / 2)` | 827 | Thực hiện logic vector2 int trong script MapTileset. | - |
| `private bool TryGetCornerRotation(Vector2Int endpoint2, out Quaternion rotation)` | 836 | Thử thực hiện get corner rotation, thường có kiểm tra điều kiện trước khi chạy. | - |
| `new Vector2Int((endpoint2.x + xSign) / 2, (endpoint2.y + zSign) / 2)` | 844 | Thực hiện logic vector2 int trong script MapTileset. | - |
| `private bool TryGetOuterCornerRotation(Vector2Int endpoint2, out Quaternion rotation)` | 865 | Thử thực hiện get outer corner rotation, thường có kiểm tra điều kiện trước khi chạy. | - |
| `new Vector2Int((endpoint2.x + xSign) / 2, (endpoint2.y + zSign) / 2)` | 873 | Thực hiện logic vector2 int trong script MapTileset. | - |
| `private Vector3 GetLeftWallArchPivotPositionForCorner(Vector2Int endpoint, Vector2Int[] dirs)` | 894 | Lấy dữ liệu left wall arch pivot position for corner cho hệ thống khác sử dụng. | - |
| `new Vector2Int(endpoint.x / 2 + dx, endpoint.y / 2 + dz)` | 900 | Thực hiện logic vector2 int trong script MapTileset. | - |
| `new Vector3(-candidateOutward.x, 0f, -candidateOutward.y)` | 912 | Thực hiện logic vector3 trong script MapTileset. | - |
| `return GetWallArchEdgePosition(bounds, d, GetWallArchY(), straightArchRotation)` | 914 | Lấy dữ liệu wall arch edge position cho hệ thống khác sử dụng. | - |
| `return EndpointToWorld(endpoint, GetWallArchY())` | 919 | Thực hiện logic endpoint to world trong script MapTileset. | - |
| `private Vector3 GetLeftWallPivotPositionForCorner(Vector2Int endpoint, Vector2Int[] dirs)` | 922 | Lấy dữ liệu left wall pivot position for corner cho hệ thống khác sử dụng. | - |
| `return EndpointToWorld(endpoint, GetWallArchY())` | 925 | Thực hiện logic endpoint to world trong script MapTileset. | - |
| `return EndpointToWorld(endpoint, GetWallArchY())` | 928 | Thực hiện logic endpoint to world trong script MapTileset. | - |
| `new Vector2Int(0, 1)` | 932 | Thực hiện logic vector2 int trong script MapTileset. | - |
| `new Vector2Int(-1, 0)` | 938 | Thực hiện logic vector2 int trong script MapTileset. | - |
| `return GetWallEdgePosition(bounds, d, GetWallArchY()) + GetOuterCornerPivotOffset(xSign, zSign)` | 947 | Lấy dữ liệu wall edge position cho hệ thống khác sử dụng. | - |
| `return EndpointToWorld(endpoint, GetWallArchY())` | 950 | Thực hiện logic endpoint to world trong script MapTileset. | - |
| `private Vector3 GetOuterCornerPivotOffset(int xSign, int zSign)` | 953 | Lấy dữ liệu outer corner pivot offset cho hệ thống khác sử dụng. | - |
| `new Vector3(-detectedStepX, 0f, 0f)` | 956 | Thực hiện logic vector3 trong script MapTileset. | - |
| `new Vector3(detectedStepX, 0f, 0f)` | 958 | Thực hiện logic vector3 trong script MapTileset. | - |
| `new Vector3(0f, 0f, detectedStepZ)` | 960 | Thực hiện logic vector3 trong script MapTileset. | - |
| `new Vector3(0f, 0f, -detectedStepZ)` | 962 | Thực hiện logic vector3 trong script MapTileset. | - |
| `private bool TryGetSingleFloorAroundEndpoint(Vector2Int endpoint2, out Vector2Int floorTile, out int xSign, out int zSign)` | 965 | Thử thực hiện get single floor around endpoint, thường có kiểm tra điều kiện trước khi chạy. | - |
| `new Vector2Int((endpoint2.x + candidateXSign) / 2, (endpoint2.y + candidateZSign) / 2)` | 975 | Thực hiện logic vector2 int trong script MapTileset. | - |
| `private bool IsLeftEndpointWhenFacingOutward(Vector2Int endpoint, Vector2Int endA, Vector2Int endB, Vector2Int outward)` | 988 | Kiểm tra điều kiện/trạng thái left endpoint when facing outward. | - |
| `new Vector2Int((endA.x + endB.x) / 2, (endA.y + endB.y) / 2)` | 990 | Thực hiện logic vector2 int trong script MapTileset. | - |
| `new Vector2Int(-outward.y, outward.x)` | 991 | Thực hiện logic vector2 int trong script MapTileset. | - |
| `private Vector3 EndpointToWorld(Vector2Int endpoint2, float y)` | 995 | Thực hiện logic endpoint to world trong script MapTileset. | - |
| `new Vector3(endpoint2.x * detectedStepX * 0.5f, y, endpoint2.y * detectedStepZ * 0.5f)` | 997 | Thực hiện logic vector3 trong script MapTileset. | - |
| `private Vector3 GetAverageFloorDirectionFromEndpoint(Vector2Int endpoint2)` | 1000 | Lấy dữ liệu average floor direction from endpoint cho hệ thống khác sử dụng. | - |
| `new Vector2Int((endpoint2.x + xSign) / 2, (endpoint2.y + zSign) / 2)` | 1009 | Thực hiện logic vector2 int trong script MapTileset. | - |
| `private void GetWallEndpoints2(Vector2Int floorTile, Vector2Int outward, out Vector2Int a, out Vector2Int b)` | 1022 | Lấy dữ liệu wall endpoints2 cho hệ thống khác sử dụng. | - |
| `new Vector2Int(edgeX2, centerZ2 - 1)` | 1030 | Thực hiện logic vector2 int trong script MapTileset. | - |
| `new Vector2Int(edgeX2, centerZ2 + 1)` | 1031 | Thực hiện logic vector2 int trong script MapTileset. | - |
| `new Vector2Int(centerX2 - 1, edgeZ2)` | 1036 | Thực hiện logic vector2 int trong script MapTileset. | - |
| `new Vector2Int(centerX2 + 1, edgeZ2)` | 1037 | Thực hiện logic vector2 int trong script MapTileset. | - |
| `private Vector3 GetWallEdgePosition(Bounds bounds, int directionIndex, float y)` | 1041 | Lấy dữ liệu wall edge position cho hệ thống khác sử dụng. | - |
| `new Vector3(wx, y, wz)` | 1064 | Thực hiện logic vector3 trong script MapTileset. | - |
| `private Vector3 GetWallArchEdgePosition(Bounds bounds, int directionIndex, float y, Quaternion rotation)` | 1067 | Lấy dữ liệu wall arch edge position cho hệ thống khác sử dụng. | - |
| `new Vector3(bounds.max.x, y, bounds.center.z)` | 1073 | Thực hiện logic vector3 trong script MapTileset. | - |
| `new Vector3(bounds.min.x, y, bounds.center.z)` | 1076 | Thực hiện logic vector3 trong script MapTileset. | - |
| `new Vector3(bounds.center.x, y, bounds.max.z)` | 1079 | Thực hiện logic vector3 trong script MapTileset. | - |
| `new Vector3(bounds.center.x, y, bounds.min.z)` | 1082 | Thực hiện logic vector3 trong script MapTileset. | - |
| `private void BuildCeilings()` | 1092 | Thực hiện logic build ceilings trong script MapTileset. | - |
| `private void SpawnCeilingAtTile(Vector2Int tile, Transform parent, HashSet<Vector2Int> placedCeilings)` | 1125 | Spawn object/dữ liệu ceiling at tile. | - |
| `private bool TryGetThreeFloorCornerCeilingTile(Vector2Int endpoint2, out Vector2Int ceilingTile)` | 1134 | Thử thực hiện get three floor corner ceiling tile, thường có kiểm tra điều kiện trước khi chạy. | - |
| `new Vector2Int((endpoint2.x + xSign) / 2, (endpoint2.y + zSign) / 2)` | 1143 | Thực hiện logic vector2 int trong script MapTileset. | - |
| `new Vector2Int((endpoint2.x - xSign) / 2, (endpoint2.y - zSign) / 2)` | 1146 | Thực hiện logic vector2 int trong script MapTileset. | - |
| `private float GetWallArchY()` | 1154 | Lấy dữ liệu wall arch y cho hệ thống khác sử dụng. | - |
| `private bool IsInteriorFloorTile(Vector2Int tile)` | 1159 | Kiểm tra điều kiện/trạng thái interior floor tile. | - |
| `private void BuildPillars()` | 1171 | Thực hiện logic build pillars trong script MapTileset. | - |
| `private void PlacePillarAt(int x, int z, Transform parent)` | 1186 | Thực hiện logic place pillar at trong script MapTileset. | - |
| `tileset.pillarPrefabs, T(x, z, 0f), Quaternion.identity, parent)` | 1188 | Thực hiện logic t trong script MapTileset. | - |
| `private void PlaceDoorways()` | 1193 | Thực hiện logic place doorways trong script MapTileset. | - |
| `new Vector3((a.x + b.x) * 0.5f * config.tileSize, 0f, (a.y + b.y) * 0.5f * config.tileSize)` | 1204 | Thực hiện logic vector3 trong script MapTileset. | - |
| `private void PopulateProps()` | 1212 | Thực hiện logic populate props trong script MapTileset. | - |
| `new Vector3( x * config.tileSize + RandomOffset() * config.tileSize, 0f, z * config.tileSize + RandomOffset() * config.tileSize )` | 1230 | Thực hiện logic vector3 trong script MapTileset. | - |
| `private void PopulateDecorations()` | 1243 | Thực hiện logic populate decorations trong script MapTileset. | - |
| `new Vector3( x * config.tileSize + dx[d] * config.tileSize * 0.45f, config.wallHeight * 0.3f, z * config.tileSize + dz[d] * config.tileSize * 0.45f )` | 1266 | Thực hiện logic vector3 trong script MapTileset. | - |
| `private void PlaceLights()` | 1279 | Thực hiện logic place lights trong script MapTileset. | - |
| `new Vector3(-dx[d], 0f, -dz[d])` | 1306 | Thực hiện logic vector3 trong script MapTileset. | - |
| `tileset.ambientLightPrefabs, T(c.x, c.y, config.wallHeight - 0.5f), Quaternion.identity, parent)` | 1326 | Thực hiện logic t trong script MapTileset. | - |
| `private void PlaceRoomLanterns(Transform parent)` | 1333 | Thực hiện logic place room lanterns trong script MapTileset. | - |
| `private void SpawnChandelierLight(GameObject chandelier, Vector3 chandelierPosition, Transform parent)` | 1348 | Spawn object/dữ liệu chandelier light. | - |
| `new GameObject("Generated Chandelier Light")` | 1352 | Thực hiện logic game object trong script MapTileset. | - |
| `private void SpawnTorchLight(GameObject torch, Vector3 torchPosition, Vector3 inward, Transform parent)` | 1367 | Spawn object/dữ liệu torch light. | - |
| `new GameObject("Generated Torch Light")` | 1371 | Thực hiện logic game object trong script MapTileset. | - |
| `private Vector3 GetWallMountedTorchPosition(Bounds floorBoundsForTile, int directionIndex, Vector3 inward)` | 1386 | Lấy dữ liệu wall mounted torch position cho hệ thống khác sử dụng. | - |
| `private void PlacePlayerSpawn()` | 1404 | Thực hiện logic place player spawn trong script MapTileset. | - |
| `tileset.playerSpawnPointPrefab, T(c.x, c.y, 0f), Quaternion.identity, parent)` | 1411 | Thực hiện logic t trong script MapTileset. | - |
| `new GameObject("PlayerSpawnPoint")` | 1415 | Thực hiện logic game object trong script MapTileset. | - |
| `private void PlaceEnemySpawners()` | 1421 | Thực hiện logic place enemy spawners trong script MapTileset. | - |
| `spawnerPool, T(sx, sz, 0f), Quaternion.identity, parent)` | 1443 | Thực hiện logic t trong script MapTileset. | - |
| `private void PlaceSiteOfGrace()` | 1448 | Thực hiện logic place site of grace trong script MapTileset. Liên kết trực tiếp: SiteOfGraceInteractable. | SiteOfGraceInteractable |
| `new Vector3(config.tileSize * 1.5f, 0f, config.tileSize * 1.5f)` | 1454 | Thực hiện logic vector3 trong script MapTileset. | - |
| `private void PlaceBossRoom()` | 1461 | Thực hiện logic place boss room trong script MapTileset. | - |
| `new Vector3( (entry.x + bossCenter.x) * 0.5f * config.tileSize, 0f, (entry.y + bossCenter.y) * 0.5f * config.tileSize )` | 1472 | Thực hiện logic vector3 trong script MapTileset. | - |
| `tileset.bossPrefab, T(bossCenter.x, bossCenter.y, 0f), Quaternion.identity, parent)` | 1483 | Thực hiện logic t trong script MapTileset. | - |
| `private void BuildRoomZones()` | 1488 | Thực hiện logic build room zones trong script MapTileset. Liên kết trực tiếp: GeneratedZoneInfo, RandomMapGenerator. | GeneratedZoneInfo, RandomMapGenerator |
| `new GeneratedZoneInfo(zoneName, roomBounds)` | 1502 | Thực hiện logic generated zone info trong script MapTileset. | - |
| `zone, RoomCenter(rooms[i - 1]), RoomCenter(room))` | 1506 | Thực hiện logic room center trong script MapTileset. | - |
| `private Bounds ExpandRoomBoundsForStructure(Bounds roomFloorBounds)` | 1530 | Thực hiện logic expand room bounds for structure trong script MapTileset. | - |
| `new Vector3(expandX * 2f, 0f, expandZ * 2f))` | 1536 | Thực hiện logic vector3 trong script MapTileset. | - |
| `new Vector3(roomFloorBounds.center.x, height * 0.5f, roomFloorBounds.center.z)` | 1537 | Thực hiện logic vector3 trong script MapTileset. | - |
| `new Vector3(roomFloorBounds.size.x, height, roomFloorBounds.size.z)` | 1538 | Thực hiện logic vector3 trong script MapTileset. | - |
| `private void AddCorridorCoverageToZone(GeneratedZoneInfo zone, Vector2Int from, Vector2Int to)` | 1542 | Thêm corridor coverage to zone vào danh sách, trạng thái hoặc dữ liệu. | - |
| `private Bounds CreateRoomFloorBounds(RectInt room)` | 1551 | Tạo object/dữ liệu room floor bounds. | - |
| `return CreateTileCoverageBounds( room.x, room.y, room.x + room.width - 1, room.y + room.height - 1, 0)` | 1577 | Tạo object/dữ liệu tile coverage bounds. | - |
| `new Vector3(combined.center.x, height * 0.5f, combined.center.z)` | 1586 | Thực hiện logic vector3 trong script MapTileset. | - |
| `new Vector3(combined.size.x, height, combined.size.z)` | 1587 | Thực hiện logic vector3 trong script MapTileset. | - |
| `private GameObject CreateSceneVolumeCube(string zoneName, Bounds bounds)` | 1591 | Tạo object/dữ liệu scene volume cube. | - |
| `private bool IsSceneVolumeTransform(Transform t)` | 1611 | Kiểm tra điều kiện/trạng thái scene volume transform. | - |
| `private Bounds CreateTileCoverageBounds(int x1, int z1, int x2, int z2, int paddingTiles)` | 1626 | Tạo object/dữ liệu tile coverage bounds. | - |
| `new Bounds( new Vector3((minWorldX + maxWorldX) * 0.5f, height * 0.5f, (minWorldZ + maxWorldZ) * 0.5f), new Vector3(maxWorldX - minWorldX, height, maxWorldZ - minWorldZ))` | 1639 | Thực hiện logic bounds trong script MapTileset. | - |
| `private GeneratedZoneInfo GetBestZoneForBounds(Bounds bounds)` | 1644 | Lấy dữ liệu best zone for bounds cho hệ thống khác sử dụng. Liên kết trực tiếp: GeneratedZoneInfo. | GeneratedZoneInfo |
| `return GetBestZoneForPosition(bounds.center)` | 1695 | Lấy dữ liệu best zone for position cho hệ thống khác sử dụng. | - |
| `private GeneratedZoneInfo GetContainingZoneForPosition(Vector3 position)` | 1698 | Lấy dữ liệu containing zone for position cho hệ thống khác sử dụng. Liên kết trực tiếp: GeneratedZoneInfo. | GeneratedZoneInfo |
| `private GeneratedZoneInfo GetBestZoneForPosition(Vector3 position)` | 1722 | Lấy dữ liệu best zone for position cho hệ thống khác sử dụng. Liên kết trực tiếp: GeneratedZoneInfo. | GeneratedZoneInfo |
| `private void BuildZones()` | 1764 | Thực hiện logic build zones trong script MapTileset. Liên kết trực tiếp: GeneratedZoneInfo, RandomMapGenerator. | GeneratedZoneInfo, RandomMapGenerator |
| `new Bounds( new Vector3(minX + zoneW * 0.5f, config.wallHeight * 0.5f, minZ + zoneH * 0.5f), new Vector3(zoneW, config.wallHeight + 2f, zoneH) )` | 1784 | Thực hiện logic bounds trong script MapTileset. | - |
| `new GeneratedZoneInfo(zoneName, zoneBounds)` | 1790 | Thực hiện logic generated zone info trong script MapTileset. | - |
| `private void CollectAllChildren(Transform root, List<Transform> result)` | 1806 | Thực hiện logic collect all children trong script MapTileset. | - |
| `public void ApplyWorld01LightingMode()` | 1819 | Áp dụng world01 lighting mode lên character/object mục tiêu. Liên kết trực tiếp: GameAssetPaths. | GameAssetPaths |
| `private void ApplyWorld01RenderSettings()` | 1836 | Áp dụng world01 render settings lên character/object mục tiêu. | - |
| `new Color(0.5f, 0.5f, 0.5f, 1f)` | 1839 | Thực hiện logic color trong script MapTileset. | - |
| `new Color(0.212f, 0.227f, 0.259f, 1f)` | 1843 | Thực hiện logic color trong script MapTileset. | - |
| `new Color(0.114f, 0.125f, 0.133f, 1f)` | 1844 | Thực hiện logic color trong script MapTileset. | - |
| `new Color(0.047f, 0.043f, 0.035f, 1f)` | 1845 | Thực hiện logic color trong script MapTileset. | - |
| `private void ApplyWorld01PostProcessing()` | 1851 | Áp dụng world01 post processing lên character/object mục tiêu. Liên kết trực tiếp: GameAssetPaths. | GameAssetPaths |
| `new GameObject("Post Processing")` | 1861 | Thực hiện logic game object trong script MapTileset. | - |
| `private void ApplyWorld01GeneratedLightDefaults()` | 1876 | Áp dụng world01 generated light defaults lên character/object mục tiêu. | - |
| `new Color(0.8490566f, 0.657692f, 0.42052332f)` | 1878 | Thực hiện logic color trong script MapTileset. | - |
| `private void ApplyWorld01GeneratedLightValues()` | 1887 | Áp dụng world01 generated light values lên character/object mục tiêu. | - |
| `public void MarkGeneratedMapForBake()` | 1907 | Thực hiện logic mark generated map for bake trong script MapTileset. | - |
| `public void BakeGeneratedMapLighting()` | 1939 | Thực hiện logic bake generated map lighting trong script MapTileset. | - |
| `public void BakeGeneratedNavMesh()` | 1946 | Thực hiện logic bake generated nav mesh trong script MapTileset. Liên kết trực tiếp: RandomMapGenerator. | RandomMapGenerator |
| `private Transform GetGeneratedRoot()` | 1972 | Lấy dữ liệu generated root cho hệ thống khác sử dụng. | - |
| `private Transform GetOrCreateChild(string path)` | 1999 | Lấy dữ liệu or create child cho hệ thống khác sử dụng. | - |
| `private GameObject SpawnPrefab(GameObject[] pool, Vector3 position, Quaternion rotation, Transform parent)` | 2020 | Spawn object/dữ liệu prefab. | - |
| `return SpawnSingle(prefab, position, rotation, parent)` | 2027 | Spawn object/dữ liệu single. | - |
| `private GameObject SpawnSingle(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent)` | 2030 | Spawn object/dữ liệu single. | - |
| `return Instantiate(prefab, position, rotation, parent)` | 2038 | Thực hiện logic instantiate trong script MapTileset. | - |
| `private float RandomOffset() => (float)(rng.NextDouble() * 0.4 - 0.2)` | 2043 | Thực hiện logic random offset trong script MapTileset. | - |
| `private Bounds GetWorldBounds(GameObject go)` | 2049 | Lấy dữ liệu world bounds cho hệ thống khác sử dụng. | - |
| `new Bounds( go.transform.position, new Vector3(config.tileSize, 0.1f, config.tileSize) )` | 2057 | Thực hiện logic bounds trong script MapTileset. | - |

#### WorldAIManager

- **Đường dẫn:** `Assets/Game/Scripts/World Managers/WorldAIManager.cs`
- **Loại:** MonoBehaviour
- **Vai trò dễ hiểu:** Quản lý cấp world: điều phối hệ thống dùng chung như scene, save, object, AI, âm thanh, item hoặc progression.
- **Kế thừa/cha:** MonoBehaviour
- **Script con:** -
- **Field public/serialized chính:** public static WorldAIManager instance, public bool isPerformingLoadingOperation, public GameObject beaconGameObject, public GameObject dialogueInteractable
- **Liên kết script:** AIBossCharacterManager, AICharacterManager, AICharacterSpawner, AIPatrolPath, Interactable

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `private void Awake()` | 39 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. | - |
| `public void SpawnCharacter(AICharacterSpawner aiCharacterSpawner)` | 51 | Spawn object/dữ liệu character. | - |
| `public void AddCharacterToSpawnedCharacterList(AICharacterManager character)` | 60 | Thêm character to spawned character list vào danh sách, trạng thái hoặc dữ liệu. Liên kết trực tiếp: AIBossCharacterManager. | AIBossCharacterManager |
| `public AIBossCharacterManager GetBossCharacterByID(int ID)` | 78 | Lấy dữ liệu boss character by id cho hệ thống khác sử dụng. | - |
| `public void SpawnAllCharacters()` | 84 | Spawn object/dữ liệu all characters. | - |
| `private IEnumerator SpawnAllCharactersCoroutine()` | 94 | Spawn object/dữ liệu all characters coroutine. | - |
| `new WaitForFixedUpdate()` | 98 | Thực hiện logic wait for fixed update trong script WorldAIManager. | - |
| `public void ResetAllCharacters()` | 110 | Đưa all characters về trạng thái mặc định. | - |
| `private IEnumerator ResetAllCharactersCoroutine()` | 120 | Đưa all characters coroutine về trạng thái mặc định. | - |
| `new WaitForFixedUpdate()` | 124 | Thực hiện logic wait for fixed update trong script WorldAIManager. | - |
| `private void DespawnAllCharacters()` | 136 | Thực hiện logic despawn all characters trong script WorldAIManager. | - |
| `private IEnumerator DespawnAllCharactersCoroutine()` | 146 | Thực hiện logic despawn all characters coroutine trong script WorldAIManager. | - |
| `new WaitForFixedUpdate()` | 150 | Thực hiện logic wait for fixed update trong script WorldAIManager. | - |
| `private void DisableAllCharacters()` | 164 | Tắt all characters. | - |
| `public void PrepareForWorldSceneTransition()` | 169 | Thực hiện logic prepare for world scene transition trong script WorldAIManager. Liên kết trực tiếp: AICharacterManager. | AICharacterManager |
| `public void DespawnAllDeadCharacters()` | 199 | Thực hiện logic despawn all dead characters trong script WorldAIManager. | - |
| `private IEnumerator DespawnAllDeadCharactersCoroutine()` | 210 | Thực hiện logic despawn all dead characters coroutine trong script WorldAIManager. Liên kết trực tiếp: AIBossCharacterManager, AICharacterManager. | AIBossCharacterManager, AICharacterManager |
| `new WaitForFixedUpdate()` | 237 | Thực hiện logic wait for fixed update trong script WorldAIManager. | - |
| `public void DisableAllBossFights()` | 244 | Tắt all boss fights. | - |
| `public void AddPatrolPathToList(AIPatrolPath patrolPath)` | 256 | Thêm patrol path to list vào danh sách, trạng thái hoặc dữ liệu. | - |
| `public AIPatrolPath GetAIPatrolPathByID(int patrolPathID)` | 264 | Lấy dữ liệu aipatrol path by id cho hệ thống khác sử dụng. Liên kết trực tiếp: AIPatrolPath. | AIPatrolPath |
| `public void RemoveCharacterFromSpawnedCharacterList(AICharacterManager character)` | 277 | Loại bỏ character from spawned character list khỏi danh sách, trạng thái hoặc dữ liệu. Liên kết trực tiếp: AIBossCharacterManager. | AIBossCharacterManager |

#### WorldActionManager

- **Đường dẫn:** `Assets/Game/Scripts/World Managers/WorldActionManager.cs`
- **Loại:** MonoBehaviour
- **Vai trò dễ hiểu:** Quản lý cấp world: điều phối hệ thống dùng chung như scene, save, object, AI, âm thanh, item hoặc progression.
- **Kế thừa/cha:** MonoBehaviour
- **Script con:** -
- **Field public/serialized chính:** public static WorldActionManager instance, public WeaponItemAction[] weaponItemAction
- **Liên kết script:** Item, WeaponItemAction

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `private void Awake()` | 13 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. | - |
| `private void Start()` | 26 | Chạy sau Awake để hoàn tất thiết lập ban đầu, nhất là khi cần các object khác đã sẵn sàng. | - |
| `public WeaponItemAction GetWeaponItemActionByID(int ID)` | 34 | Lấy dữ liệu weapon item action by id cho hệ thống khác sử dụng. | - |

#### WorldBossDefinition

- **Đường dẫn:** `Assets/Game/Scripts/World Managers/WorldBossCatalog.cs`
- **Loại:** Data / plain C#
- **Vai trò dễ hiểu:** Quản lý cấp world: điều phối hệ thống dùng chung như scene, save, object, AI, âm thanh, item hoặc progression.
- **Kế thừa/cha:** -
- **Script con:** -
- **Field public/serialized chính:** public string worldName, public int sceneBuildIndex, public int bossID, public GameObject bossPrefab, [SerializeField] private WorldBossDefinition[] bosses
- **Liên kết script:** WorldBossCatalog

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `public static WorldBossCatalog LoadDefault()` | 21 | Nạp dữ liệu hoặc scene liên quan tới default. Liên kết trực tiếp: WorldBossCatalog. | WorldBossCatalog |
| `public GameObject GetBossPrefabForScene(int sceneBuildIndex)` | 26 | Lấy dữ liệu boss prefab for scene cho hệ thống khác sử dụng. | - |
| `public int GetBossIDForScene(int sceneBuildIndex)` | 37 | Lấy dữ liệu boss idfor scene cho hệ thống khác sử dụng. | - |

#### WorldCharacterEffectsManager

- **Đường dẫn:** `Assets/Game/Scripts/World Managers/WorldCharacterEffectsManager.cs`
- **Loại:** MonoBehaviour
- **Vai trò dễ hiểu:** Quản lý cấp world: điều phối hệ thống dùng chung như scene, save, object, AI, âm thanh, item hoặc progression.
- **Kế thừa/cha:** MonoBehaviour
- **Script con:** -
- **Field public/serialized chính:** public static WorldCharacterEffectsManager instance, public GameObject bloodSplatterVFX, public GameObject criticalBloodSplatterVFX, public GameObject healingFlaskVFX, public GameObject deadSpotVFX, public GameObject poisonedVFX, public GameObject burningVFX, public GameObject bloodLossVFX, public GameObject frostBiteVFX, public GameObject guardianBuffPotionVFX, public GameObject windBuffPotionVFX, public GameObject sageBuffPotionVFX +18
- **Liên kết script:** BloodLossEffect, BuildUpEffect, BurningEffect, FrostBiteEffect, InstantCharacterEffect, ModifyStaminaRegenerationForATimeEffect, PoisonedEffect, StaticCharacterEffect, TakeBlockedDamageEffect, TakeBuildUpEffect, TakeCriticalDamageEffect, TakeDamageEffect, TimedCharacterEffect, TwoHandingEffect

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `private void Awake()` | 63 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. | - |
| `private void GenerateEffectIDs()` | 77 | Thực hiện logic generate effect ids trong script WorldCharacterEffectsManager. | - |

#### WorldGameSessionManager

- **Đường dẫn:** `Assets/Game/Scripts/World Managers/WorldGameSessionManager.cs`
- **Loại:** MonoBehaviour
- **Vai trò dễ hiểu:** Quản lý cấp world: điều phối hệ thống dùng chung như scene, save, object, AI, âm thanh, item hoặc progression.
- **Kế thừa/cha:** MonoBehaviour
- **Script con:** -
- **Field public/serialized chính:** public static WorldGameSessionManager instance, public List<PlayerManager> players
- **Liên kết script:** BuildRuntimeLogger, EventTriggerLoadScene, GameProgressionManager, PlayerManager, PlayerUIManager, SessionEndGameActionType, SessionLaunchMode, SiteOfGraceInteractable, WorldAIManager, WorldLocationSceneSet, WorldObjectManager, WorldSaveGameManager, WorldSceneManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `private void Awake()` | 65 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. | - |
| `private void Start()` | 83 | Chạy sau Awake để hoàn tất thiết lập ban đầu, nhất là khi cần các object khác đã sẵn sàng. | - |
| `private void OnEnable()` | 88 | Đăng ký event/callback hoặc bật trạng thái khi component được enable. | - |
| `private void OnDisable()` | 93 | Gỡ event/callback hoặc dọn trạng thái khi component bị disable. | - |
| `private void OnSceneLoaded(Scene newScene, LoadSceneMode loadMode)` | 98 | Thực hiện logic on scene loaded trong script WorldGameSessionManager. Liên kết trực tiếp: GameProgressionManager. | GameProgressionManager |
| `public void ProcessPendingMapEntryWithoutSceneReload()` | 117 | Thực hiện logic process pending map entry without scene reload trong script WorldGameSessionManager. Liên kết trực tiếp: GameProgressionManager. | GameProgressionManager |
| `public void ReturnToTitleAfterVictory(float delay = 6f)` | 128 | Thực hiện logic return to title after victory trong script WorldGameSessionManager. | - |
| `public void ReturnToTitleAfterDefeat(float delay = 6f)` | 136 | Thực hiện logic return to title after defeat trong script WorldGameSessionManager. | - |
| `public void ScheduleMapTransition(bool shouldLoadNextScene, int nextSceneBuildIndex, bool gameWon, int unlockedMapIndex)` | 144 | Thực hiện logic schedule map transition trong script WorldGameSessionManager. | - |
| `public bool TryRegisterPlayerDeathForLose(ulong playerClientId, int mapIndex, out int deathCount)` | 151 | Thử thực hiện register player death for lose, thường có kiểm tra điều kiện trước khi chạy. | - |
| `public void HandleSessionLose(int mapIndex, ulong failedPlayerClientId, int deathCount)` | 175 | Xử lý luồng session lose. Liên kết trực tiếp: PlayerUIManager, WorldAIManager, WorldSaveGameManager. | PlayerUIManager, WorldAIManager, WorldSaveGameManager |
| `public void HandleSessionVictory(bool canContinueProgression, float popupDelay = 0f)` | 197 | Xử lý luồng session victory. Liên kết trực tiếp: PlayerUIManager, WorldSaveGameManager. | PlayerUIManager, WorldSaveGameManager |
| `public void ExecuteSynchronizedEndGameAction(SessionEndGameActionType action, bool performWorldTransition)` | 214 | Thực hiện logic execute synchronized end game action trong script WorldGameSessionManager. Liên kết trực tiếp: SessionEndGameActionType. | SessionEndGameActionType |
| `public void RetryCurrentMapFromStart()` | 232 | Thực hiện logic retry current map from start trong script WorldGameSessionManager. Liên kết trực tiếp: GameProgressionManager. | GameProgressionManager |
| `public void ContinuePendingVictoryFlow()` | 250 | Thực hiện logic continue pending victory flow trong script WorldGameSessionManager. | - |
| `public void AutoContinuePendingVictoryFlow(float delay = 3f)` | 272 | Thực hiện logic auto continue pending victory flow trong script WorldGameSessionManager. | - |
| `public void ReturnToTitleFromEndGame()` | 280 | Thực hiện logic return to title from end game trong script WorldGameSessionManager. | - |
| `public int GetDeathCountForPlayerThisMap(ulong playerClientId)` | 285 | Lấy dữ liệu death count for player this map cho hệ thống khác sử dụng. | - |
| `public int GetMaxDeathsPerMapBeforeLoseCount()` | 293 | Lấy dữ liệu max deaths per map before lose count cho hệ thống khác sử dụng. | - |
| `public bool CanRevivePlayers()` | 298 | Kiểm tra có được phép revive players hay không. | - |
| `private void LoadSceneForProgression(int nextSceneBuildIndex)` | 303 | Nạp dữ liệu hoặc scene liên quan tới scene for progression. Liên kết trực tiếp: WorldAIManager, WorldSceneManager. | WorldAIManager, WorldSceneManager |
| `private IEnumerator HandlePendingMapEntryCoroutine()` | 319 | Xử lý luồng pending map entry coroutine. Liên kết trực tiếp: BuildRuntimeLogger, GameProgressionManager, PlayerManager, PlayerUIManager, SiteOfGraceInteractable +4. | BuildRuntimeLogger, GameProgressionManager, PlayerManager, PlayerUIManager, SiteOfGraceInteractable, WorldLocationSceneSet, WorldObjectManager, WorldSaveGameManager, WorldSceneManager |
| `return WaitForRequiredAreaScenes(null, 30f, 0.05f, 0.75f)` | 363 | Thực hiện logic wait for required area scenes trong script WorldGameSessionManager. | - |
| `return WaitForRequiredAreaScenes(initialArea, 30f, 0.05f, 0.55f)` | 372 | Thực hiện logic wait for required area scenes trong script WorldGameSessionManager. | - |
| `return WaitForRequiredAreaScenes(entryArea, 30f, 0.55f, 0.95f)` | 436 | Thực hiện logic wait for required area scenes trong script WorldGameSessionManager. | - |
| `new WaitForSeconds(4f)` | 443 | Thực hiện logic wait for seconds trong script WorldGameSessionManager. | - |
| `private IEnumerator AutoContinuePendingVictoryFlowCoroutine(float delay)` | 456 | Thực hiện logic auto continue pending victory flow coroutine trong script WorldGameSessionManager. | - |
| `private WorldLocationSceneSet TriggerNearestAreaLoadForPlayer(PlayerManager player, Vector3 origin, float searchRadius = 120f)` | 481 | Thực hiện logic trigger nearest area load for player trong script WorldGameSessionManager. Liên kết trực tiếp: BuildRuntimeLogger, EventTriggerLoadScene. | BuildRuntimeLogger, EventTriggerLoadScene |
| `private WorldLocationSceneSet TriggerInitialAreaLoadForPlayer(PlayerManager player)` | 529 | Thực hiện logic trigger initial area load for player trong script WorldGameSessionManager. Liên kết trực tiếp: BuildRuntimeLogger, EventTriggerLoadScene. | BuildRuntimeLogger, EventTriggerLoadScene |
| `private IEnumerator WaitForRequiredAreaScenes(WorldLocationSceneSet area, float timeout, float startProgress = 0.1f, float endProgress = 0.95f)` | 562 | Thực hiện logic wait for required area scenes trong script WorldGameSessionManager. Liên kết trực tiếp: BuildRuntimeLogger, WorldSceneManager. | BuildRuntimeLogger, WorldSceneManager |
| `new WaitForSeconds(4f)` | 579 | Thực hiện logic wait for seconds trong script WorldGameSessionManager. | - |
| `private void SetLoadingProgress(float progress, string label)` | 639 | Thiết lập giá trị hoặc trạng thái loading progress. Liên kết trực tiếp: PlayerUIManager. | PlayerUIManager |
| `private IEnumerator ReturnToTitleAfterVictoryCoroutine(float delay)` | 647 | Thực hiện logic return to title after victory coroutine trong script WorldGameSessionManager. Liên kết trực tiếp: WorldSaveGameManager. | WorldSaveGameManager |
| `private void SetLastRestedSiteOfGrace(int siteOfGraceID)` | 673 | Thiết lập giá trị hoặc trạng thái last rested site of grace. Liên kết trực tiếp: PlayerUIManager, WorldSaveGameManager. | PlayerUIManager, WorldSaveGameManager |
| `public bool StartGameAsHost()` | 685 | Thực hiện logic start game as host trong script WorldGameSessionManager. | - |
| `public void SetLaunchMode(SessionLaunchMode launchMode)` | 709 | Thiết lập giá trị hoặc trạng thái launch mode. Liên kết trực tiếp: SessionLaunchMode. | SessionLaunchMode |
| `public SessionLaunchMode GetLaunchMode()` | 716 | Lấy dữ liệu launch mode cho hệ thống khác sử dụng. | - |
| `public bool RequiresRelayForCurrentMode()` | 721 | Thực hiện logic requires relay for current mode trong script WorldGameSessionManager. Liên kết trực tiếp: SessionLaunchMode. | SessionLaunchMode |
| `public bool AllowsDirectAddressForCurrentMode()` | 726 | Thực hiện logic allows direct address for current mode trong script WorldGameSessionManager. | - |
| `public async Task<bool> StartGameAsRelayHostAsync(int maxConnections = DefaultRelayMaxConnections)` | 731 | Thực hiện logic start game as relay host async trong script WorldGameSessionManager. | - |
| `await EnsureUnityServicesSignedInAsync()` | 761 | Thực hiện logic ensure unity services signed in async trong script WorldGameSessionManager. | - |
| `new RelayServerData(allocation, RelayConnectionType))` | 767 | Thực hiện logic relay server data trong script WorldGameSessionManager. | - |
| `private void ConfigureUnityTransportPortForCurrentProject()` | 794 | Thực hiện logic configure unity transport port for current project trong script WorldGameSessionManager. | - |
| `private ushort GetUnityTransportPortForCurrentProject()` | 803 | Lấy dữ liệu unity transport port for current project cho hệ thống khác sử dụng. | - |
| `public bool StartGameAsClient(string addressInput)` | 828 | Thực hiện logic start game as client trong script WorldGameSessionManager. | - |
| `public async Task<bool> StartGameAsClientAsync(string addressInput)` | 834 | Thực hiện logic start game as client async trong script WorldGameSessionManager. | - |
| `await StartGameAsRelayClientAsync(relayJoinCode)` | 841 | Thực hiện logic start game as relay client async trong script WorldGameSessionManager. | - |
| `private IEnumerator JoinAsClientCoroutine(string hostAddress, ushort port)` | 860 | Thực hiện logic join as client coroutine trong script WorldGameSessionManager. Liên kết trực tiếp: WorldSaveGameManager. | WorldSaveGameManager |
| `public async Task<bool> StartGameAsRelayClientAsync(string relayJoinCode)` | 891 | Thực hiện logic start game as relay client async trong script WorldGameSessionManager. Liên kết trực tiếp: WorldSaveGameManager. | WorldSaveGameManager |
| `await EnsureUnityServicesSignedInAsync()` | 907 | Thực hiện logic ensure unity services signed in async trong script WorldGameSessionManager. | - |
| `new RelayServerData(joinAllocation, RelayConnectionType))` | 925 | Thực hiện logic relay server data trong script WorldGameSessionManager. | - |
| `public async Task<bool> CheckRelayJoinCodeAsync(string relayJoinCode)` | 944 | Thực hiện logic check relay join code async trong script WorldGameSessionManager. | - |
| `await EnsureUnityServicesSignedInAsync()` | 955 | Thực hiện logic ensure unity services signed in async trong script WorldGameSessionManager. | - |
| `private async Task<bool> ShutdownNetworkSessionIfNeededAsync()` | 971 | Thực hiện logic shutdown network session if needed async trong script WorldGameSessionManager. | - |
| `public string GetSuggestedHostAddress()` | 1001 | Lấy dữ liệu suggested host address cho hệ thống khác sử dụng. | - |
| `public string GetCurrentConnectionAddress()` | 1009 | Lấy dữ liệu current connection address cho hệ thống khác sử dụng. | - |
| `public bool HasRelayJoinCode()` | 1018 | Thực hiện logic has relay join code trong script WorldGameSessionManager. | - |
| `public bool IsCurrentRelayJoinCode(string relayJoinCode)` | 1023 | Kiểm tra điều kiện/trạng thái current relay join code. | - |
| `public bool IsRelayJoinCodeChecked(string relayJoinCode)` | 1030 | Kiểm tra điều kiện/trạng thái relay join code checked. | - |
| `private JoinAllocation GetCheckedRelayJoinAllocation(string relayJoinCode)` | 1037 | Lấy dữ liệu checked relay join allocation cho hệ thống khác sử dụng. | - |
| `private void ClearCheckedRelayJoinCode()` | 1045 | Thực hiện logic clear checked relay join code trong script WorldGameSessionManager. | - |
| `private async Task EnsureUnityServicesSignedInAsync()` | 1051 | Thực hiện logic ensure unity services signed in async trong script WorldGameSessionManager. | - |
| `private bool TryNormalizeRelayJoinCode(string addressInput, out string relayJoinCode)` | 1064 | Thử thực hiện normalize relay join code, thường có kiểm tra điều kiện trước khi chạy. | - |
| `private bool TryParseAddressInput(string addressInput, out string hostAddress, out ushort port)` | 1083 | Thử thực hiện parse address input, thường có kiểm tra điều kiện trước khi chạy. | - |
| `public void WaitThenRevivePlayer(PlayerManager player)` | 1120 | Thực hiện logic wait then revive player trong script WorldGameSessionManager. | - |
| `private IEnumerator RevivePlayerCoroutine(PlayerManager player, float delay)` | 1134 | Thực hiện logic revive player coroutine trong script WorldGameSessionManager. Liên kết trực tiếp: PlayerUIManager, SiteOfGraceInteractable, WorldAIManager, WorldObjectManager, WorldSaveGameManager. | PlayerUIManager, SiteOfGraceInteractable, WorldAIManager, WorldObjectManager, WorldSaveGameManager |
| `new WaitForSeconds(delay)` | 1136 | Thực hiện logic wait for seconds trong script WorldGameSessionManager. | - |
| `private void ResetTransientSessionStateForCurrentMap()` | 1184 | Đưa transient session state for current map về trạng thái mặc định. Liên kết trực tiếp: GameProgressionManager, WorldSaveGameManager. | GameProgressionManager, WorldSaveGameManager |
| `private void ClearPendingVictoryTransition()` | 1197 | Thực hiện logic clear pending victory transition trong script WorldGameSessionManager. | - |
| `private void SyncDeathTrackingMap(int mapIndex)` | 1210 | Thực hiện logic sync death tracking map trong script WorldGameSessionManager. Liên kết trực tiếp: GameProgressionManager. | GameProgressionManager |
| `private void CancelPendingRevival()` | 1226 | Kiểm tra có được phép cel pending revival hay không. | - |
| `public void AddPlayerToActivePlayersList(PlayerManager player)` | 1235 | Thêm player to active players list vào danh sách, trạng thái hoặc dữ liệu. | - |
| `public void RemovePlayerFromActivePlayersList(PlayerManager player)` | 1251 | Loại bỏ player from active players list khỏi danh sách, trạng thái hoặc dữ liệu. | - |
| `public int GetActivePlayerCount()` | 1267 | Lấy dữ liệu active player count cho hệ thống khác sử dụng. | - |
| `public bool IsMultiplayerSessionActive()` | 1278 | Kiểm tra điều kiện/trạng thái multiplayer session active. | - |
| `public PlayerManager GetPlayerByClientId(ulong clientId)` | 1283 | Lấy dữ liệu player by client id cho hệ thống khác sử dụng. Liên kết trực tiếp: PlayerManager. | PlayerManager |

#### WorldItemDatabase

- **Đường dẫn:** `Assets/Game/Scripts/World Managers/WorldItemDatabase.cs`
- **Loại:** MonoBehaviour
- **Vai trò dễ hiểu:** Quản lý cấp world: điều phối hệ thống dùng chung như scene, save, object, AI, âm thanh, item hoặc progression.
- **Kế thừa/cha:** MonoBehaviour
- **Script con:** -
- **Field public/serialized chính:** public static WorldItemDatabase Instance, public WeaponItem unarmedWeapon, public GameObject pickUpItemPrefab, public UpgradeMaterial smallUpgradeStone, public UpgradeMaterial mediumUpgradeStone, public UpgradeMaterial largeUpgradeStone
- **Liên kết script:** AshOfWar, BodyEquipmentItem, BuffCharmItem, FlaskItem, HandEquipmentItem, HeadEquipmentItem, Item, LegEquipmentItem, QuickSlotItem, RangedProjectileItem, SerializableFlask, SerializableQuickSlotItem, SerializableRangedProjectile, SerializableWeapon, SpellItem, UpgradeLevel +2

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `private void Awake()` | 68 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. | - |
| `private void RegisterDefaultBuffCharms()` | 140 | Thực hiện logic register default buff charms trong script WorldItemDatabase. Liên kết trực tiếp: BuffCharmItem, QuickSlotItem. | BuffCharmItem, QuickSlotItem |
| `private BuffCharmItem CreateDefaultBuffCharm( string itemName, string itemDescription, Sprite icon, float durationSeconds, int maxHealthBonus = 0, int maxStaminaBonus = 0, int maxFocusPointsBonus = 0, float staminaRegenerationBonusPercentage = 0f, float outgoingDamageBonusPercentage = 0f, int purchasePrice = 100, int sellPrice = 50)` | 195 | Tạo object/dữ liệu default buff charm. Liên kết trực tiếp: BuffCharmItem. | BuffCharmItem |
| `private GameObject GetDefaultBuffFlaskPrefab(string itemName)` | 231 | Lấy dữ liệu default buff flask prefab cho hệ thống khác sử dụng. | - |
| `private GameObject GetDefaultBuffPotionVFXPrefab(string itemName)` | 253 | Lấy dữ liệu default buff potion vfxprefab cho hệ thống khác sử dụng. | - |
| `public List<BuffCharmItem> GetDefaultBuffCharms()` | 275 | Lấy dữ liệu default buff charms cho hệ thống khác sử dụng. | - |
| `public Item GetItemByID(int ID)` | 282 | Lấy dữ liệu item by id cho hệ thống khác sử dụng. | - |
| `public Item CreateItemInstance(int itemID)` | 287 | Tạo object/dữ liệu item instance. Liên kết trực tiếp: Item. | Item |
| `return Instantiate(item)` | 294 | Thực hiện logic instantiate trong script WorldItemDatabase. | - |
| `public List<Item> GetPurchasableItems()` | 297 | Lấy dữ liệu purchasable items cho hệ thống khác sử dụng. | - |
| `public WeaponItem GetWeaponByID(int ID)` | 302 | Lấy dữ liệu weapon by id cho hệ thống khác sử dụng. | - |
| `public HeadEquipmentItem GetHeadEquipmentByID(int ID)` | 307 | Lấy dữ liệu head equipment by id cho hệ thống khác sử dụng. | - |
| `public BodyEquipmentItem GetBodyEquipmentByID(int ID)` | 312 | Lấy dữ liệu body equipment by id cho hệ thống khác sử dụng. | - |
| `public LegEquipmentItem GetLegEquipmentByID(int ID)` | 317 | Lấy dữ liệu leg equipment by id cho hệ thống khác sử dụng. | - |
| `public HandEquipmentItem GetHandEquipmentByID(int ID)` | 322 | Lấy dữ liệu hand equipment by id cho hệ thống khác sử dụng. | - |
| `public AshOfWar GetAshOfWarByID(int ID)` | 327 | Lấy dữ liệu ash of war by id cho hệ thống khác sử dụng. | - |
| `public SpellItem GetSpellByID(int ID)` | 332 | Lấy dữ liệu spell by id cho hệ thống khác sử dụng. | - |
| `public RangedProjectileItem GetProjectileByID(int ID)` | 337 | Lấy dữ liệu projectile by id cho hệ thống khác sử dụng. | - |
| `public QuickSlotItem GetQuickSlotItemByID(int ID)` | 342 | Lấy dữ liệu quick slot item by id cho hệ thống khác sử dụng. | - |
| `public UpgradeMaterial GetUpgradeMaterialByID(int ID)` | 347 | Lấy dữ liệu upgrade material by id cho hệ thống khác sử dụng. | - |
| `public WeaponItem GetWeaponFromSerializedData(SerializableWeapon serializableWeapon)` | 354 | Lấy dữ liệu weapon from serialized data cho hệ thống khác sử dụng. Liên kết trực tiếp: AshOfWar, UpgradeLevel, WeaponItem. | AshOfWar, UpgradeLevel, WeaponItem |
| `return Instantiate(unarmedWeapon)` | 362 | Thực hiện logic instantiate trong script WorldItemDatabase. | - |
| `public RangedProjectileItem GetRangedProjectileFromSerializedData(SerializableRangedProjectile serializableProjectile)` | 375 | Lấy dữ liệu ranged projectile from serialized data cho hệ thống khác sử dụng. Liên kết trực tiếp: RangedProjectileItem. | RangedProjectileItem |
| `public FlaskItem GetFlaskFromSerializedData(SerializableFlask serializableFlask)` | 388 | Lấy dữ liệu flask from serialized data cho hệ thống khác sử dụng. Liên kết trực tiếp: FlaskItem. | FlaskItem |
| `public QuickSlotItem GetQuickSlotItemFromSerializedData(SerializableQuickSlotItem serializableQuickSlotItem)` | 398 | Lấy dữ liệu quick slot item from serialized data cho hệ thống khác sử dụng. Liên kết trực tiếp: QuickSlotItem. | QuickSlotItem |

#### WorldLocationManager

- **Đường dẫn:** `Assets/Game/Scripts/World Managers/WorldLocationManager.cs`
- **Loại:** MonoBehaviour
- **Vai trò dễ hiểu:** Quản lý cấp world: điều phối hệ thống dùng chung như scene, save, object, AI, âm thanh, item hoặc progression.
- **Kế thừa/cha:** MonoBehaviour
- **Script con:** -
- **Field public/serialized chính:** public static WorldLocationManager instance, public List<WorldLocationRendererManager> worldLocationRenderers, [SerializeField] private float recentlyVisitedLocationHoldTime
- **Liên kết script:** BuildRuntimeLogger, PlayerManager, WorldLocationRendererManager, WorldLocationSceneSet, WorldSceneManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `private void Awake()` | 27 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. | - |
| `public void ResetForWorldSceneTransition()` | 39 | Đưa for world scene transition về trạng thái mặc định. Liên kết trực tiếp: PlayerManager. | PlayerManager |
| `public List<string> GenerateDoNotUnloadListBasedOnPlayerLocations()` | 60 | Thực hiện logic generate do not unload list based on player locations trong script WorldLocationManager. Liên kết trực tiếp: PlayerManager, WorldLocationSceneSet, WorldSceneManager. | PlayerManager, WorldLocationSceneSet, WorldSceneManager |
| `public void LoadAreasBasedOnAreaCurrentIn(WorldLocationSceneSet areaCurrentlyIn, PlayerManager player)` | 132 | Nạp dữ liệu hoặc scene liên quan tới areas based on area current in. Liên kết trực tiếp: BuildRuntimeLogger, WorldSceneManager. | BuildRuntimeLogger, WorldSceneManager |
| `private bool IsPlayerAlreadyInArea(WorldLocationSceneSet area, PlayerManager player)` | 166 | Kiểm tra điều kiện/trạng thái player already in area. | - |
| `private void RemovePlayerFromPreviousLocation(PlayerManager player)` | 176 | Loại bỏ player from previous location khỏi danh sách, trạng thái hoặc dữ liệu. Liên kết trực tiếp: PlayerManager, WorldLocationSceneSet. | PlayerManager, WorldLocationSceneSet |
| `private void AddPlayerToNewLocation(WorldLocationSceneSet area, PlayerManager player)` | 206 | Thêm player to new location vào danh sách, trạng thái hoặc dữ liệu. Liên kết trực tiếp: PlayerManager, WorldLocationSceneSet. | PlayerManager, WorldLocationSceneSet |
| `private void MarkLocationAsRecentlyVisited(WorldLocationSceneSet location)` | 233 | Thực hiện logic mark location as recently visited trong script WorldLocationManager. | - |
| `private IEnumerator CleanupRecentlyVisitedLocationsCoroutine()` | 244 | Thực hiện logic cleanup recently visited locations coroutine trong script WorldLocationManager. Liên kết trực tiếp: WorldLocationSceneSet, WorldSceneManager. | WorldLocationSceneSet, WorldSceneManager |
| `new WaitForSeconds(waitTime)` | 278 | Thực hiện logic wait for seconds trong script WorldLocationManager. | - |
| `private void LoadAdditiveScenesAroundCurrentArea(WorldLocationSceneSet area)` | 284 | Nạp dữ liệu hoặc scene liên quan tới additive scenes around current area. Liên kết trực tiếp: BuildRuntimeLogger, WorldLocationSceneSet, WorldSceneManager. | BuildRuntimeLogger, WorldLocationSceneSet, WorldSceneManager |
| `private IEnumerator WaitThenSetActiveScene()` | 308 | Thực hiện logic wait then set active scene trong script WorldLocationManager. Liên kết trực tiếp: WorldSceneManager. | WorldSceneManager |
| `public void AddLocationRendererManagerToList(WorldLocationRendererManager worldLocationRendererManager)` | 331 | Thêm location renderer manager to list vào danh sách, trạng thái hoặc dữ liệu. | - |
| `public void ToggleGameMode()` | 345 | Thực hiện logic toggle game mode trong script WorldLocationManager. Liên kết trực tiếp: WorldLocationRendererManager. | WorldLocationRendererManager |
| `public void ToggleLightBakeMode()` | 363 | Thực hiện logic toggle light bake mode trong script WorldLocationManager. Liên kết trực tiếp: WorldLocationRendererManager. | WorldLocationRendererManager |

#### WorldLocationRendererManager

- **Đường dẫn:** `Assets/Game/Scripts/World Managers/WorldLocationRendererManager.cs`
- **Loại:** MonoBehaviour
- **Vai trò dễ hiểu:** Quản lý cấp world: điều phối hệ thống dùng chung như scene, save, object, AI, âm thanh, item hoặc progression.
- **Kế thừa/cha:** MonoBehaviour
- **Script con:** -
- **Field public/serialized chính:** public int renderSceneID, [SerializeField] public List<GameObject> rootGameObjects, [SerializeField] public List<MeshRenderer> meshRenderers
- **Liên kết script:** WorldLocationManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `private void Awake()` | 22 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. Liên kết trực tiếp: WorldLocationManager. | WorldLocationManager |
| `private void Start()` | 29 | Chạy sau Awake để hoàn tất thiết lập ban đầu, nhất là khi cần các object khác đã sẵn sàng. | - |
| `public void FindAllRootObjects()` | 38 | Tìm all root objects trong scene/danh sách dữ liệu. | - |
| `public void ToggleRootObjects(bool status)` | 61 | Thực hiện logic toggle root objects trong script WorldLocationRendererManager. | - |
| `public void FindAllMeshRenderers()` | 80 | Tìm all mesh renderers trong scene/danh sách dữ liệu. | - |
| `public void ToggleMeshRenderers(bool status)` | 100 | Thực hiện logic toggle mesh renderers trong script WorldLocationRendererManager. | - |
| `public void ToggleAllMeshRenderersOverTime(bool status)` | 116 | Thực hiện logic toggle all mesh renderers over time trong script WorldLocationRendererManager. | - |
| `private IEnumerator ToggleAllMeshRenderersOverTimeCoroutine(bool status)` | 124 | Thực hiện logic toggle all mesh renderers over time coroutine trong script WorldLocationRendererManager. | - |

#### WorldObjectManager

- **Đường dẫn:** `Assets/Game/Scripts/World Managers/WorldObjectManager.cs`
- **Loại:** MonoBehaviour
- **Vai trò dễ hiểu:** Quản lý cấp world: điều phối hệ thống dùng chung như scene, save, object, AI, âm thanh, item hoặc progression.
- **Kế thừa/cha:** MonoBehaviour
- **Script con:** -
- **Field public/serialized chính:** public static WorldObjectManager instance, public List<FogWallInteractable> fogWalls, public List<SiteOfGraceInteractable> sitesOfGrace
- **Liên kết script:** FogWallInteractable, NetworkObjectSpawner, SiteOfGraceInteractable

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `private void Awake()` | 21 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. | - |
| `public void SpawnObject(NetworkObjectSpawner networkObjectSpawner)` | 33 | Spawn object/dữ liệu object. | - |
| `public void AddFogWallToList(FogWallInteractable fogWall)` | 42 | Thêm fog wall to list vào danh sách, trạng thái hoặc dữ liệu. | - |
| `public void RemoveFogWallFromList(FogWallInteractable fogWall)` | 50 | Loại bỏ fog wall from list khỏi danh sách, trạng thái hoặc dữ liệu. | - |
| `public void AddSiteOfGraceToList(SiteOfGraceInteractable siteOfGrace)` | 58 | Thêm site of grace to list vào danh sách, trạng thái hoặc dữ liệu. | - |
| `public void RemoveSiteOfGraceFromList(SiteOfGraceInteractable siteOfGrace)` | 66 | Loại bỏ site of grace from list khỏi danh sách, trạng thái hoặc dữ liệu. | - |

#### WorldSaveGameManager

- **Đường dẫn:** `Assets/Game/Scripts/World Managers/WorldSaveGameManager.cs`
- **Loại:** MonoBehaviour
- **Vai trò dễ hiểu:** Quản lý cấp world: điều phối hệ thống dùng chung như scene, save, object, AI, âm thanh, item hoặc progression.
- **Kế thừa/cha:** MonoBehaviour
- **Script con:** -
- **Field public/serialized chính:** public static WorldSaveGameManager instance, public PlayerManager player, public CharacterSlot currentCharacterSlotBeingUsed, public CharacterSaveData currentCharacterData, public CharacterSaveData characterSlots01, public CharacterSaveData characterSlots02, public CharacterSaveData characterSlots03, public CharacterSaveData characterSlots04, public CharacterSaveData characterSlots05, public CharacterSaveData characterSlots06, public CharacterSaveData characterSlots07, public CharacterSaveData characterSlots08 +4
- **Liên kết script:** BuildRuntimeLogger, CharacterDialogue, CharacterDialogueID, CharacterSaveData, CharacterSlot, FlaskItem, GameProgressionManager, PlayerManager, QuickSlotItem, RangedProjectileItem, SaveFileDataWriter, SerializableFlask, SerializableQuickSlotItem, SerializableRangedProjectile, SerializableWeapon, TitleScreenManager +2

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `private void Awake()` | 51 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. Liên kết trực tiếp: BuildRuntimeLogger, GameProgressionManager. | BuildRuntimeLogger, GameProgressionManager |
| `private void Start()` | 67 | Chạy sau Awake để hoàn tất thiết lập ban đầu, nhất là khi cần các object khác đã sẵn sàng. Liên kết trực tiếp: BuildRuntimeLogger. | BuildRuntimeLogger |
| `private void Update()` | 76 | Cập nhật logic mỗi frame như input, timer, trạng thái chiến đấu hoặc UI. | - |
| `public static string FormatDuration(float totalSeconds)` | 93 | Thực hiện logic format duration trong script WorldSaveGameManager. | - |
| `public CharacterSaveData GetCharacterDataForSlot(CharacterSlot characterSlot)` | 102 | Lấy dữ liệu character data for slot cho hệ thống khác sử dụng. Liên kết trực tiếp: CharacterSlot. | CharacterSlot |
| `public float GetCurrentCharacterPlayedSeconds()` | 120 | Lấy dữ liệu current character played seconds cho hệ thống khác sử dụng. | - |
| `public void SetCurrentCharacterPlayTimeFrozen(bool isFrozen)` | 125 | Thiết lập giá trị hoặc trạng thái current character play time frozen. | - |
| `public bool HasFreeCharacterSlot()` | 130 | Thực hiện logic has free character slot trong script WorldSaveGameManager. Liên kết trực tiếp: CharacterSlot, SaveFileDataWriter. | CharacterSlot, SaveFileDataWriter |
| `new SaveFileDataWriter()` | 132 | Lưu dữ liệu liên quan tới file data writer. | - |
| `public string DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot characterSlot)` | 189 | Thực hiện logic decide character file name based on character slot being used trong script WorldSaveGameManager. Liên kết trực tiếp: CharacterSlot. | CharacterSlot |
| `public void AttemptToCreateNewGame()` | 230 | Cố gắng kích hoạt to create new game nếu trạng thái hiện tại cho phép. Liên kết trực tiếp: BuildRuntimeLogger, CharacterSaveData, CharacterSlot, SaveFileDataWriter, TitleScreenManager. | BuildRuntimeLogger, CharacterSaveData, CharacterSlot, SaveFileDataWriter, TitleScreenManager |
| `new SaveFileDataWriter()` | 233 | Lưu dữ liệu liên quan tới file data writer. | - |
| `new CharacterSaveData()` | 243 | Thực hiện logic character save data trong script WorldSaveGameManager. | - |
| `new CharacterSaveData()` | 255 | Thực hiện logic character save data trong script WorldSaveGameManager. | - |
| `new CharacterSaveData()` | 267 | Thực hiện logic character save data trong script WorldSaveGameManager. | - |
| `new CharacterSaveData()` | 279 | Thực hiện logic character save data trong script WorldSaveGameManager. | - |
| `new CharacterSaveData()` | 291 | Thực hiện logic character save data trong script WorldSaveGameManager. | - |
| `new CharacterSaveData()` | 303 | Thực hiện logic character save data trong script WorldSaveGameManager. | - |
| `new CharacterSaveData()` | 315 | Thực hiện logic character save data trong script WorldSaveGameManager. | - |
| `new CharacterSaveData()` | 327 | Thực hiện logic character save data trong script WorldSaveGameManager. | - |
| `new CharacterSaveData()` | 339 | Thực hiện logic character save data trong script WorldSaveGameManager. | - |
| `new CharacterSaveData()` | 351 | Thực hiện logic character save data trong script WorldSaveGameManager. | - |
| `private void NewGame()` | 362 | Thực hiện logic new game trong script WorldSaveGameManager. Liên kết trực tiếp: BuildRuntimeLogger, GameProgressionManager, TitleScreenManager, WorldSceneManager. | BuildRuntimeLogger, GameProgressionManager, TitleScreenManager, WorldSceneManager |
| `public void LoadGame()` | 384 | Nạp dữ liệu hoặc scene liên quan tới game. Liên kết trực tiếp: BuildRuntimeLogger, GameProgressionManager, SaveFileDataWriter, WorldSceneManager. | BuildRuntimeLogger, GameProgressionManager, SaveFileDataWriter, WorldSceneManager |
| `new SaveFileDataWriter()` | 390 | Lưu dữ liệu liên quan tới file data writer. | - |
| `public void SaveGame()` | 430 | Lưu dữ liệu liên quan tới game. Liên kết trực tiếp: GameProgressionManager, SaveFileDataWriter. | GameProgressionManager, SaveFileDataWriter |
| `new SaveFileDataWriter()` | 435 | Lưu dữ liệu liên quan tới file data writer. | - |
| `public void DeleteGame(CharacterSlot characterSlot)` | 452 | Thực hiện logic delete game trong script WorldSaveGameManager. Liên kết trực tiếp: SaveFileDataWriter. | SaveFileDataWriter |
| `new SaveFileDataWriter()` | 455 | Lưu dữ liệu liên quan tới file data writer. | - |
| `private void LoadAllCharacterProfiles()` | 463 | Nạp dữ liệu hoặc scene liên quan tới all character profiles. Liên kết trực tiếp: CharacterSlot, SaveFileDataWriter. | CharacterSlot, SaveFileDataWriter |
| `new SaveFileDataWriter()` | 465 | Lưu dữ liệu liên quan tới file data writer. | - |
| `public int GetWorldSceneIndex()` | 499 | Lấy dữ liệu world scene index cho hệ thống khác sử dụng. Liên kết trực tiếp: GameProgressionManager. | GameProgressionManager |
| `public SerializableWeapon GetSerializableWeaponFromWeaponItem(WeaponItem weapon)` | 504 | Lấy dữ liệu serializable weapon from weapon item cho hệ thống khác sử dụng. Liên kết trực tiếp: SerializableWeapon. | SerializableWeapon |
| `new SerializableWeapon()` | 506 | Thực hiện logic serializable weapon trong script WorldSaveGameManager. | - |
| `public SerializableRangedProjectile GetSerializableRangedProjectileFromRangedProjectileItem(RangedProjectileItem projectile)` | 526 | Lấy dữ liệu serializable ranged projectile from ranged projectile item cho hệ thống khác sử dụng. Liên kết trực tiếp: SerializableRangedProjectile. | SerializableRangedProjectile |
| `new SerializableRangedProjectile()` | 528 | Thực hiện logic serializable ranged projectile trong script WorldSaveGameManager. | - |
| `public SerializableFlask GetSerializableFlaskFromFlaskItem(FlaskItem flask)` | 544 | Lấy dữ liệu serializable flask from flask item cho hệ thống khác sử dụng. Liên kết trực tiếp: SerializableFlask. | SerializableFlask |
| `new SerializableFlask()` | 546 | Thực hiện logic serializable flask trong script WorldSaveGameManager. | - |
| `public SerializableQuickSlotItem GetSerializableQuickSlotItemFromQuickSlotItem(QuickSlotItem quickSlotItem)` | 561 | Lấy dữ liệu serializable quick slot item from quick slot item cho hệ thống khác sử dụng. Liên kết trực tiếp: SerializableQuickSlotItem. | SerializableQuickSlotItem |
| `new SerializableQuickSlotItem()` | 563 | Thực hiện logic serializable quick slot item trong script WorldSaveGameManager. | - |
| `public CharacterDialogue GetCharacterDialogueByEnum(CharacterDialogueID characterDialogueID)` | 580 | Lấy dữ liệu character dialogue by enum cho hệ thống khác sử dụng. Liên kết trực tiếp: CharacterDialogue, CharacterDialogueID. | CharacterDialogue, CharacterDialogueID |
| `private CharacterDialogue FindDialogueByStageID(int stageID, List<CharacterDialogue> dialogueList)` | 604 | Tìm dialogue by stage id trong scene/danh sách dữ liệu. Liên kết trực tiếp: CharacterDialogue. | CharacterDialogue |
| `public void SetStageOfDialogue(CharacterDialogueID characterDialogue, int stageIndex)` | 623 | Thiết lập giá trị hoặc trạng thái stage of dialogue. Liên kết trực tiếp: CharacterDialogueID. | CharacterDialogueID |
| `private void GetStageIDsOnLoad()` | 642 | Lấy dữ liệu stage ids on load cho hệ thống khác sử dụng. | - |
| `private void TickCurrentCharacterPlayTime()` | 647 | Thực hiện logic tick current character play time trong script WorldSaveGameManager. | - |
| `private bool CanAdvanceCurrentCharacterPlayTime()` | 655 | Kiểm tra có được phép advance current character play time hay không. | - |
| `private void SetCharacterDataForSlot(CharacterSlot characterSlot, CharacterSaveData characterData)` | 669 | Thiết lập giá trị hoặc trạng thái character data for slot. Liên kết trực tiếp: CharacterSlot. | CharacterSlot |

#### WorldSceneManager

- **Đường dẫn:** `Assets/Game/Scripts/World Managers/WorldSceneManager.cs`
- **Loại:** NetworkBehaviour
- **Vai trò dễ hiểu:** Quản lý cấp world: điều phối hệ thống dùng chung như scene, save, object, AI, âm thanh, item hoặc progression.
- **Kế thừa/cha:** NetworkBehaviour
- **Script con:** -
- **Field public/serialized chính:** public static WorldSceneManager instance, public List<Scene> loadedScenes, public List<string> doNotUnLoadList, [SerializeField] private float unrequiredSceneUnloadDelay, [SerializeField] private bool loadNonWorld01MapsAllAtOnce, [SerializeField] private string roomStreamingWorldSceneName, public string world, public string area_01_Subarea_00, public string area_01_Subarea_01, public string area_01_Subarea_02, public string area_01_Subarea_03, public string area_01_Subarea_04 +1
- **Liên kết script:** BuildRuntimeLogger, PlayerUIManager, WorldAIManager, WorldLocationManager, WorldLocationSceneSet, WorldSaveGameManager, WorldSceneLocation

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `private void Awake()` | 52 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. Liên kết trực tiếp: BuildRuntimeLogger. | BuildRuntimeLogger |
| `private void OnEnable()` | 69 | Đăng ký event/callback hoặc bật trạng thái khi component được enable. Liên kết trực tiếp: BuildRuntimeLogger. | BuildRuntimeLogger |
| `private void OnDisable()` | 75 | Gỡ event/callback hoặc dọn trạng thái khi component bị disable. Liên kết trực tiếp: BuildRuntimeLogger. | BuildRuntimeLogger |
| `public override void OnNetworkSpawn()` | 81 | Thiết lập dữ liệu và callback khi NetworkObject được spawn trong Netcode. | - |
| `public override void OnNetworkDespawn()` | 88 | Dọn dữ liệu/callback khi NetworkObject bị despawn khỏi Netcode. | - |
| `private void OnSceneEvent(SceneEvent sceneEvent)` | 98 | Thực hiện logic on scene event trong script WorldSceneManager. Liên kết trực tiếp: BuildRuntimeLogger. | BuildRuntimeLogger |
| `public void LoadWorldScene(int buildIndex)` | 170 | Nạp dữ liệu hoặc scene liên quan tới world scene. Liên kết trực tiếp: BuildRuntimeLogger, PlayerUIManager, WorldSaveGameManager. | BuildRuntimeLogger, PlayerUIManager, WorldSaveGameManager |
| `private void PrepareForSingleWorldSceneLoad()` | 225 | Thực hiện logic prepare for single world scene load trong script WorldSceneManager. Liên kết trực tiếp: BuildRuntimeLogger, WorldLocationManager. | BuildRuntimeLogger, WorldLocationManager |
| `private void OnUnitySceneLoaded(Scene scene, LoadSceneMode loadMode)` | 264 | Thực hiện logic on unity scene loaded trong script WorldSceneManager. Liên kết trực tiếp: BuildRuntimeLogger, WorldLocationManager. | BuildRuntimeLogger, WorldLocationManager |
| `public void LogLoadingStateSnapshot(string source)` | 280 | Thực hiện logic log loading state snapshot trong script WorldSceneManager. Liên kết trực tiếp: BuildRuntimeLogger, PlayerUIManager, WorldAIManager. | BuildRuntimeLogger, PlayerUIManager, WorldAIManager |
| `private void RefreshCurrentWorldSceneID(Scene scene)` | 300 | Làm mới dữ liệu/hiển thị current world scene id. | - |
| `public string GetCurrentWorldSceneID()` | 311 | Lấy dữ liệu current world scene id cho hệ thống khác sử dụng. | - |
| `public bool ShouldLoadGeneratedWorldAllAtOnce()` | 321 | Thực hiện logic should load generated world all at once trong script WorldSceneManager. | - |
| `public void LoadAllGeneratedWorldAreaScenes()` | 329 | Nạp dữ liệu hoặc scene liên quan tới all generated world area scenes. | - |
| `public List<string> GetGeneratedWorldAreaSceneNames()` | 346 | Lấy dữ liệu generated world area scene names cho hệ thống khác sử dụng. | - |
| `private string GetGeneratedWorldAreaScenePrefix()` | 373 | Lấy dữ liệu generated world area scene prefix cho hệ thống khác sử dụng. | - |
| `private void LoadAdditiveScene(string sceneName)` | 390 | Nạp dữ liệu hoặc scene liên quan tới additive scene. Liên kết trực tiếp: BuildRuntimeLogger. | BuildRuntimeLogger |
| `private IEnumerator LoadAdditiveSceneNonNetworkCoroutine(string sceneName)` | 431 | Nạp dữ liệu hoặc scene liên quan tới additive scene non network coroutine. Liên kết trực tiếp: BuildRuntimeLogger. | BuildRuntimeLogger |
| `public void LoadAdditiveScenes(List<string> scenesToLoad)` | 484 | Nạp dữ liệu hoặc scene liên quan tới additive scenes. Liên kết trực tiếp: BuildRuntimeLogger. | BuildRuntimeLogger |
| `private bool IsSceneLoadedOrQueued(string sceneName)` | 527 | Kiểm tra điều kiện/trạng thái scene loaded or queued. | - |
| `private IEnumerator LoadAdditiveScenesCoroutine()` | 546 | Nạp dữ liệu hoặc scene liên quan tới additive scenes coroutine. Liên kết trực tiếp: BuildRuntimeLogger, PlayerUIManager. | BuildRuntimeLogger, PlayerUIManager |
| `new WaitForSeconds(waitTime)` | 564 | Thực hiện logic wait for seconds trong script WorldSceneManager. | - |
| `new WaitForSeconds(waitTime)` | 579 | Thực hiện logic wait for seconds trong script WorldSceneManager. | - |
| `new WaitForFixedUpdate()` | 587 | Thực hiện logic wait for fixed update trong script WorldSceneManager. | - |
| `private void UnloadAdditiveScene(string sceneName)` | 599 | Thực hiện logic unload additive scene trong script WorldSceneManager. | - |
| `public void UnloadAdditiveScenes(List<string> sceneList)` | 625 | Thực hiện logic unload additive scenes trong script WorldSceneManager. | - |
| `private IEnumerator UnloadAdditiveScenesCoroutine()` | 643 | Thực hiện logic unload additive scenes coroutine trong script WorldSceneManager. Liên kết trực tiếp: PlayerUIManager. | PlayerUIManager |
| `new WaitForSeconds(waitTime)` | 654 | Thực hiện logic wait for seconds trong script WorldSceneManager. | - |
| `new WaitForSeconds(waitTime)` | 660 | Thực hiện logic wait for seconds trong script WorldSceneManager. | - |
| `new WaitForSeconds(waitTime)` | 673 | Thực hiện logic wait for seconds trong script WorldSceneManager. | - |
| `private IEnumerator UnloadAllAdditiveScenesNonNetwork()` | 687 | Thực hiện logic unload all additive scenes non network trong script WorldSceneManager. | - |
| `public string GetSceneIDFromWorldSceneLocation(WorldSceneLocation area)` | 711 | Lấy dữ liệu scene idfrom world scene location cho hệ thống khác sử dụng. Liên kết trực tiếp: WorldSceneLocation. | WorldSceneLocation |
| `public void CheckForUnrequiredScenes()` | 736 | Thực hiện logic check for unrequired scenes trong script WorldSceneManager. Liên kết trực tiếp: WorldLocationManager. | WorldLocationManager |
| `private void QueueUnrequiredScenesForDelayedUnload(List<string> scenesToUnload)` | 772 | Thực hiện logic queue unrequired scenes for delayed unload trong script WorldSceneManager. | - |
| `private IEnumerator DelayedUnrequiredSceneUnloadCoroutine()` | 805 | Thực hiện logic delayed unrequired scene unload coroutine trong script WorldSceneManager. | - |
| `new WaitForSeconds(Mathf.Max(0.25f, nextUnloadTime - Time.time))` | 817 | Thực hiện logic wait for seconds trong script WorldSceneManager. | - |
| `public void CheckForRequiredRenderers()` | 824 | Thực hiện logic check for required renderers trong script WorldSceneManager. Liên kết trực tiếp: BuildRuntimeLogger, PlayerUIManager, WorldLocationManager, WorldLocationSceneSet. | BuildRuntimeLogger, PlayerUIManager, WorldLocationManager, WorldLocationSceneSet |
| `private IEnumerator CheckForRequiredSceneRenderersCoroutine(WorldLocationSceneSet location)` | 850 | Thực hiện logic check for required scene renderers coroutine trong script WorldSceneManager. Liên kết trực tiếp: BuildRuntimeLogger, PlayerUIManager, WorldLocationManager. | BuildRuntimeLogger, PlayerUIManager, WorldLocationManager |
| `new WaitForEndOfFrame()` | 858 | Thực hiện logic wait for end of frame trong script WorldSceneManager. | - |
| `public int GetBuildIndexFromSceneID(string sceneID)` | 930 | Lấy dữ liệu build index from scene id cho hệ thống khác sử dụng. | - |
| `new object()` | 939 | Thực hiện logic object trong script WorldSceneManager. | - |
| `public static void Log(string message)` | 960 | Thực hiện logic log trong script WorldSceneManager. | - |
| `public static void Warning(string message)` | 966 | Thực hiện logic warning trong script WorldSceneManager. | - |
| `public static void Error(string message)` | 972 | Thực hiện logic error trong script WorldSceneManager. | - |
| `private static void Initialize()` | 978 | Thực hiện logic initialize trong script WorldSceneManager. Liên kết trực tiếp: BuildRuntimeLogger. | BuildRuntimeLogger |
| `public static void BeginLoadingWatch(string reason)` | 1018 | Thực hiện logic begin loading watch trong script WorldSceneManager. | - |
| `public static void EndLoadingWatch(string reason)` | 1030 | Thực hiện logic end loading watch trong script WorldSceneManager. | - |
| `public static void MainThreadHeartbeat(string context)` | 1037 | Thực hiện logic main thread heartbeat trong script WorldSceneManager. | - |
| `private static void WriteLoadingWatchdogSnapshot(object state)` | 1052 | Thực hiện logic write loading watchdog snapshot trong script WorldSceneManager. | - |
| `private static void HandleUnityLogMessage(string condition, string stackTrace, LogType type)` | 1065 | Xử lý luồng unity log message. | - |
| `private static void Write(string level, string message)` | 1076 | Thực hiện logic write trong script WorldSceneManager. | - |
| `private static void WriteRaw(string message)` | 1082 | Thực hiện logic write raw trong script WorldSceneManager. | - |

#### WorldSoundFXManager

- **Đường dẫn:** `Assets/Game/Scripts/World Managers/WorldSoundFXManager.cs`
- **Loại:** MonoBehaviour
- **Vai trò dễ hiểu:** Quản lý cấp world: điều phối hệ thống dùng chung như scene, save, object, AI, âm thanh, item hoặc progression.
- **Kế thừa/cha:** MonoBehaviour
- **Script con:** -
- **Field public/serialized chính:** public static WorldSoundFXManager instance, public AudioClip[] physicalDamageSFX, public AudioClip pickUpItemSFX, public AudioClip rollSFX, public AudioClip stanceBreakSFX, public AudioClip criticalStrikeSFX, public AudioClip[] releaseArrowSFX, public AudioClip[] notchArrowSFX, public AudioClip healingFlaskSFX, public AudioClip unableToContinueUISFX, public AudioClip hoverUISFX, public AudioClip confirmUISFX +1
- **Liên kết script:** AICharacterManager, GameSettingsManager

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `private void Awake()` | 34 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. | - |
| `private void Start()` | 46 | Chạy sau Awake để hoàn tất thiết lập ban đầu, nhất là khi cần các object khác đã sẵn sàng. | - |
| `public AudioClip ChooseRandomSFXFromArray(AudioClip[] array)` | 52 | Thực hiện logic choose random sfxfrom array trong script WorldSoundFXManager. | - |
| `public void PlayBossTrack(AudioClip introTrack, AudioClip loopTrack)` | 58 | Phát boss track, thường là animation, sound hoặc VFX. | - |
| `public void StopBossMusic()` | 84 | Thực hiện logic stop boss music trong script WorldSoundFXManager. | - |
| `private IEnumerator FadeOutBossMusicThenStop()` | 89 | Thực hiện logic fade out boss music then stop trong script WorldSoundFXManager. | - |
| `public void AlertNearbyCharactersToSound(Vector3 positionOfSound, float rangeOfSound)` | 102 | Thực hiện logic alert nearby characters to sound trong script WorldSoundFXManager. Liên kết trực tiếp: AICharacterManager. | AICharacterManager |
| `public void ApplyAudioSettings()` | 130 | Áp dụng audio settings lên character/object mục tiêu. Liên kết trực tiếp: GameSettingsManager. | GameSettingsManager |

#### WorldUtilityManager

- **Đường dẫn:** `Assets/Game/Scripts/World Managers/WorldUtilityManager.cs`
- **Loại:** MonoBehaviour
- **Vai trò dễ hiểu:** Quản lý cấp world: điều phối hệ thống dùng chung như scene, save, object, AI, âm thanh, item hoặc progression.
- **Kế thừa/cha:** MonoBehaviour
- **Script con:** -
- **Field public/serialized chính:** public static WorldUtilityManager Instance, public float slopeSlideForce, public float hiddenTargetDetectionRadiusPenalty
- **Liên kết script:** CharacterGroup, DamageIntensity, WeaponClass

| Hàm | Dòng | Ý nghĩa | Liên kết trong hàm |
|---|---:|---|---|
| `new Color(1f, 0.45f, 0.1f, 1f)` | 16 | Thực hiện logic color trong script WorldUtilityManager. | - |
| `private void Awake()` | 27 | Khởi tạo tham chiếu sớm khi object được tạo, thường lấy component con hoặc thiết lập singleton/local cache. | - |
| `public LayerMask GetCharacterLayers()` | 41 | Lấy dữ liệu character layers cho hệ thống khác sử dụng. | - |
| `public LayerMask GetEnviroLayers()` | 46 | Lấy dữ liệu enviro layers cho hệ thống khác sử dụng. | - |
| `public LayerMask GetSlipperyEnviroLayers()` | 51 | Lấy dữ liệu slippery enviro layers cho hệ thống khác sử dụng. | - |
| `public Color GetPoisonedColor()` | 56 | Lấy dữ liệu poisoned color cho hệ thống khác sử dụng. | - |
| `public Color GetBurningColor()` | 61 | Lấy dữ liệu burning color cho hệ thống khác sử dụng. | - |
| `public Material GetFrozenMaterial()` | 66 | Lấy dữ liệu frozen material cho hệ thống khác sử dụng. | - |
| `public bool CanIDamageThisTarget(CharacterGroup attackingCharacter, CharacterGroup targetCharacter)` | 71 | Kiểm tra có được phép idamage this target hay không. Liên kết trực tiếp: CharacterGroup. | CharacterGroup |
| `public float GetAngleOfTarget(Transform characterTransform, Vector3 targetsDirection)` | 97 | Lấy dữ liệu angle of target cho hệ thống khác sử dụng. | - |
| `public DamageIntensity GetDamageIntensityBasedOnPoiseDamage(float poiseDamage)` | 109 | Lấy dữ liệu damage intensity based on poise damage cho hệ thống khác sử dụng. Liên kết trực tiếp: DamageIntensity. | DamageIntensity |
| `public Vector3 GetRipostingPositionBasedOnWeaponClass(WeaponClass weaponClass)` | 133 | Lấy dữ liệu riposting position based on weapon class cho hệ thống khác sử dụng. Liên kết trực tiếp: WeaponClass. | WeaponClass |
| `new Vector3(0.11f, 0, 0.7f)` | 135 | Thực hiện logic vector3 trong script WorldUtilityManager. | - |
| `public Vector3 GetBackstabPositionBasedOnWeaponClass(WeaponClass weaponClass)` | 153 | Lấy dữ liệu backstab position based on weapon class cho hệ thống khác sử dụng. Liên kết trực tiếp: WeaponClass. | WeaponClass |
| `new Vector3(0.12f, 0, 0.74f)` | 155 | Thực hiện logic vector3 trong script WorldUtilityManager. | - |
