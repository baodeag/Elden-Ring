# Câu hỏi phản biện và câu trả lời mẫu cho đồ án Eternal Hollow

Tài liệu này đã được viết lại để mỗi câu hỏi có câu trả lời riêng, tránh lặp ý giữa
các câu. Mỗi đoạn được xuống dòng ngắn khoảng hai mươi chữ để dễ đọc khi ôn bảo vệ.

Cách dùng: đọc ý chính trước, nhớ class hoặc hàm liên quan, sau đó luyện nói lại bằng lời của
mình. Nếu thầy cô hỏi sâu, mở phần code tiêu biểu ở cuối nhóm để giải thích luồng hoạt động.

## 1. Tổng quan đề tài

### Câu 1. Vì sao em chọn đề tài game 3D hành động nhập vai nhiều người chơi?

**Trả lời mẫu:**

Em chọn đề tài này vì nó gom nhiều bài toán quan trọng của lập trình game 3D vào một
sản phẩm thống nhất: điều khiển nhân vật, camera, combat, AI, boss, UI, save/load, item, progression và multiplayer. Nhờ vậy
đồ án không chỉ là một scene demo, mà có vòng lặp chơi tương đối đầy đủ để thể hiện
khả năng thiết kế và triển khai bằng Unity/C#.

**Khi mở code:**

Mở `PROJECT_ROADMAP.md, các manager chính trong Assets/Game/Scripts`.

**Cách giải thích trên code:**

Giải thích theo hướng đồ án không chỉ có một cơ chế riêng lẻ, mà là nhiều hệ thống Unity/C#
nối thành vòng chơi hoàn chỉnh.

### Câu 2. Điểm khác biệt giữa game của em và các game Soulslike như Elden Ring/Dark Souls là gì?

**Trả lời mẫu:**

Game lấy cảm hứng Soulslike ở stamina, dodge, lock-on, checkpoint và boss fight, nhưng không sao chép quy mô Elden
Ring hay Dark Souls. Điểm khác là đồ án tập trung vào bản thu nhỏ có cấu trúc kỹ thuật
rõ: class khởi đầu, combat, AI/boss, save/load, progression 5 map và multiplayer nền tảng.

**Khi mở code:**

Mở `PROJECT_ROADMAP.md, các manager chính trong Assets/Game/Scripts`.

**Cách giải thích trên code:**

Giải thích theo hướng đồ án không chỉ có một cơ chế riêng lẻ, mà là nhiều hệ thống Unity/C#
nối thành vòng chơi hoàn chỉnh.

### Câu 3. Phạm vi đồ án của em gồm những chức năng nào?

**Trả lời mẫu:**

Phạm vi gồm title screen, chọn class, điều khiển player, combat cận chiến/cung/spell, AI quái, boss, Site of Grace, inventory,
shop, level up, weapon upgrade, save/load, progression 5 map, chuyển scene, HUD/settings và multiplayer bằng Netcode. Em giới hạn nội
dung để tập trung chứng minh luồng chức năng chính.

**Khi mở code:**

Mở `PROJECT_ROADMAP.md, các manager chính trong Assets/Game/Scripts`.

**Cách giải thích trên code:**

Giải thích theo hướng đồ án không chỉ có một cơ chế riêng lẻ, mà là nhiều hệ thống Unity/C#
nối thành vòng chơi hoàn chỉnh.

### Câu 4. Chức năng nào là trọng tâm kỹ thuật nhất trong đồ án?

**Trả lời mẫu:**

Trọng tâm kỹ thuật là combat kết hợp network và progression. Một đòn đánh đi từ input, weapon action, animation,
collider, RPC damage, cập nhật HP/UI và effect. Khi target là boss, kết quả còn mở map mới, lưu boss
defeated và kích hoạt luồng chiến thắng.

**Khi mở code:**

Mở `PROJECT_ROADMAP.md, các manager chính trong Assets/Game/Scripts`.

**Cách giải thích trên code:**

Giải thích theo hướng đồ án không chỉ có một cơ chế riêng lẻ, mà là nhiều hệ thống Unity/C#
nối thành vòng chơi hoàn chỉnh.

### Câu 5. Trong quá trình làm, phần nào em tự xây dựng, phần nào dùng asset/package có sẵn?

**Trả lời mẫu:**

Em dùng Unity, Netcode, Transport/Relay và một số asset model, animation, VFX, UI có sẵn. Phần tự xây dựng là
logic gameplay: controller, combat, AI state machine, boss flow, inventory, shop, level up, weapon upgrade, save/load, progression, UI flow và
đồng bộ multiplayer.

**Khi mở code:**

Mở `PROJECT_ROADMAP.md, các manager chính trong Assets/Game/Scripts`.

**Cách giải thích trên code:**

Giải thích theo hướng đồ án không chỉ có một cơ chế riêng lẻ, mà là nhiều hệ thống Unity/C#
nối thành vòng chơi hoàn chỉnh.

### Câu 6. Nếu phải rút gọn đồ án còn 3 chức năng quan trọng nhất, em chọn gì? Vì sao?

**Trả lời mẫu:**

Em chọn combat, AI/boss/progression và save-load/inventory. Combat là lõi trải nghiệm. AI, boss và progression tạo mục tiêu, thử thách
và cảm giác hoàn thành. Save-load cùng inventory giúp game có tính nhập vai, có tài nguyên, vật phẩm và
khả năng chơi tiếp nhiều phiên.

**Khi mở code:**

Mở `PROJECT_ROADMAP.md, các manager chính trong Assets/Game/Scripts`.

**Cách giải thích trên code:**

Giải thích theo hướng đồ án không chỉ có một cơ chế riêng lẻ, mà là nhiều hệ thống Unity/C#
nối thành vòng chơi hoàn chỉnh.

### Câu 7. Hạn chế lớn nhất của sản phẩm hiện tại là gì?

**Trả lời mẫu:**

Hạn chế lớn nhất là mức độ hoàn thiện chưa bằng game thương mại. Nội dung map, animation, cân bằng
chỉ số, phản hồi va chạm, tối ưu multiplayer khi mạng yếu và số lượng test case vẫn cần cải
thiện. Nếu phát triển tiếp, em ưu tiên polish combat, cân bằng boss và kiểm thử multiplayer.

**Khi mở code:**

Mở `PROJECT_ROADMAP.md, các manager chính trong Assets/Game/Scripts`.

**Cách giải thích trên code:**

Giải thích theo hướng đồ án không chỉ có một cơ chế riêng lẻ, mà là nhiều hệ thống Unity/C#
nối thành vòng chơi hoàn chỉnh.

## 2. Kiến trúc hệ thống

### Câu 1. Em mô tả kiến trúc tổng thể của game?

**Trả lời mẫu:**

Game được chia theo nhóm trách nhiệm. World manager quản lý scene, save, session và progression. CharacterManager là nền chung
cho nhân vật. PlayerManager gom component của player, còn AICharacterManager xử lý enemy. UI chỉ hiển thị và nhận thao
tác, còn network layer đồng bộ trạng thái qua NetworkVariable/RPC.

**Khi mở code:**

Mở `WorldSaveGameManager, WorldSceneManager, GameProgressionManager, PlayerManager`.

**Cách giải thích trên code:**

Giải thích kiến trúc theo trách nhiệm: world, player, AI, UI, network và save/load.

### Câu 2. Vì sao em chia hệ thống thành các manager như WorldSaveGameManager, WorldSceneManager, GameProgressionManager?

**Trả lời mẫu:**

Em chia thành nhiều manager để mỗi class có trách nhiệm rõ. WorldSaveGameManager chỉ lo lưu/nạp. WorldSceneManager lo chuyển scene.
GameProgressionManager lo boss nào đã chết và map nào được mở. Cách này giúp code dễ tìm, dễ debug và
tránh một class quá lớn.

**Khi mở code:**

Mở `WorldSaveGameManager, WorldSceneManager, GameProgressionManager, PlayerManager`.

**Cách giải thích trên code:**

Giải thích kiến trúc theo trách nhiệm: world, player, AI, UI, network và save/load.

### Câu 3. Vai trò của PlayerManager là gì?

**Trả lời mẫu:**

PlayerManager là điểm điều phối chính của nhân vật người chơi. Nó lấy các component như locomotion, combat, inventory, stats,
network và interaction, rồi gọi xử lý theo vòng đời Unity. PlayerManager không tự làm hết chi tiết, mà phân
việc cho từng manager con.

**Khi mở code:**

Mở `PlayerManager.Awake(), PlayerManager.OnNetworkSpawn()`.

**Cách giải thích trên code:**

Chỉ ra PlayerManager gom component con, còn logic chi tiết nằm ở từng manager riêng.

### Câu 4. Vai trò của CharacterManager khác gì với PlayerManager và AICharacterManager?

**Trả lời mẫu:**

CharacterManager chứa phần chung cho mọi nhân vật như trạng thái chết, animation, effects, combat manager và network manager cơ
bản. PlayerManager mở rộng cho nhân vật do người chơi điều khiển. AICharacterManager mở rộng cho enemy, thêm state machine,
NavMeshAgent và target detection.

**Khi mở code:**

Mở `PlayerManager.Awake(), PlayerManager.OnNetworkSpawn()`.

**Cách giải thích trên code:**

Chỉ ra PlayerManager gom component con, còn logic chi tiết nằm ở từng manager riêng.

### Câu 5. Vì sao nhiều hệ thống dùng Singleton?

**Trả lời mẫu:**

Singleton phù hợp với các hệ thống toàn cục chỉ nên có một instance như UI manager, save manager, scene
manager hoặc progression manager. Các script khác có thể gọi Instance để truy cập chức năng chung. Trong phạm vi
đồ án Unity nhỏ, cách này giúp kết nối hệ thống nhanh.

**Khi mở code:**

Mở `WorldSaveGameManager.instance, PlayerUIManager.instance, GameProgressionManager.Instance`.

**Cách giải thích trên code:**

Nói rõ singleton dùng cho manager toàn cục, tiện truy cập nhưng tạo phụ thuộc ẩn.

### Câu 6. Singleton có nhược điểm gì trong Unity?

**Trả lời mẫu:**

Singleton có nhược điểm là tạo phụ thuộc ẩn, khó test và dễ lỗi nếu scene load sai thứ tự.
Nếu có hai object cùng singleton, instance có thể trùng hoặc bị hủy sai. Vì vậy em chỉ dùng cho
manager thật sự toàn cục và kiểm soát Awake/Destroy rõ.

**Khi mở code:**

Mở `WorldSaveGameManager.instance, PlayerUIManager.instance, GameProgressionManager.Instance`.

**Cách giải thích trên code:**

Nói rõ singleton dùng cho manager toàn cục, tiện truy cập nhưng tạo phụ thuộc ẩn.

### Câu 7. Nếu dự án mở rộng lớn hơn, em sẽ cải tiến kiến trúc như thế nào?

**Trả lời mẫu:**

Nếu mở rộng, em sẽ giảm phụ thuộc trực tiếp vào singleton, dùng event bus hoặc service locator có kiểm
soát hơn, tách data bằng ScriptableObject rõ ràng hơn và chuẩn hóa interface cho combat, inventory, save. Với multiplayer lớn,
em chuyển logic quan trọng sang server authoritative.

**Khi mở code:**

Mở `WorldSaveGameManager, WorldSceneManager, GameProgressionManager, PlayerManager`.

**Cách giải thích trên code:**

Giải thích kiến trúc theo trách nhiệm: world, player, AI, UI, network và save/load.

### Câu 8. Các hệ thống UI, combat, save/load, AI liên kết với nhau ra sao?

**Trả lời mẫu:**

UI, combat, save/load và AI nối với nhau qua dữ liệu nhân vật và manager trung tâm. Combat làm đổi
HP/effect, UI lắng nghe để cập nhật. AI dùng state machine để chọn hành vi. Save/load gom dữ liệu từ
player, inventory, progression, Site of Grace và boss state để khôi phục.

**Khi mở code:**

Mở `WorldSaveGameManager, PlayerManager, CharacterNetworkManager, PlayerUIManager, AICharacterManager`.

**Cách giải thích trên code:**

Đi theo một ví dụ damage: combat làm đổi HP, network đồng bộ, UI cập nhật, AI phản ứng, save/load ghi trạng thái cần giữ.

**Code tiêu biểu để chỉ khi bảo vệ:**

Nguồn: `Assets/Game/Scripts/Character/Player/PlayerInputManager.cs`.

```csharp
private void Update()
{
    HandleAllInputs();
}

private void HandleAllInputs()
{
    if (IsGameplayInputLocked())
    {
        HandleCloseUIInputs();
        ClearGameplayInputsWhileMenuOpen();
        return;
    }

    HandleUseItemInput();
    HandleTwoHandInput();
    HandleLockOnInput();
    HandlePlayerMovementInput();
    HandleCameraMovementInput();
    HandleDodgeInput();
    HandleSprintInput();
    HandleJumpInput();
    HandleRBInput();
    HandleLBInput();
    HandleInteractionInput();
}
```

**Giải thích code:**

Update gọi HandleAllInputs mỗi frame. Nếu menu đang mở thì clear input gameplay. Nếu không, input được chia sang từng
nhóm hành động.
## 3. Điều khiển nhân vật

### Câu 1. Luồng xử lý input của người chơi diễn ra như thế nào?

**Trả lời mẫu:**

Input đi từ Unity Input System vào PlayerInputManager. Mỗi frame, PlayerManager của owner gọi HandleAllInputs để phân phối sang movement,
camera, dodge, attack, item, interaction và lock-on. Khi menu mở, gameplay input bị clear để tránh vừa thao tác UI
vừa đánh hoặc di chuyển.

**Khi mở code:**

Mở `PlayerInputManager.Update(), PlayerInputManager.HandleAllInputs()`.

**Cách giải thích trên code:**

Update gọi HandleAllInputs mỗi frame. Hàm này kiểm tra menu trước, rồi chia input sang từng nhóm hành động.

### Câu 2. PlayerInputManager.HandleAllInputs() có vai trò gì?

**Trả lời mẫu:**

HandleAllInputs là cổng phân phối input trong một frame. Hàm kiểm tra gameplay có bị khóa không, sau đó gọi
xử lý dùng item, lock-on, movement, camera, dodge, sprint, jump, attack, block và interaction. Khi debug lỗi input, đây là
hàm em kiểm tra đầu tiên.

**Khi mở code:**

Mở `PlayerInputManager.HandleAllInputs()`.

**Cách giải thích trên code:**

Chỉ vào thứ tự các hàm con để giải thích input nào được xử lý trước, input nào bị chặn
khi menu mở.

### Câu 3. Vì sao khi mở menu phải khóa input gameplay?

**Trả lời mẫu:**

Khi mở menu, người chơi đang chọn inventory, shop, level up hoặc settings. Nếu không khóa gameplay input, cùng một
nút có thể vừa chọn UI vừa khiến nhân vật đánh, né hoặc dùng item. Việc khóa input giúp tách
rõ trạng thái gameplay và trạng thái menu.

**Khi mở code:**

Mở `PlayerInputManager.IsGameplayInputLocked(), ClearGameplayInputsWhileMenuOpen()`.

**Cách giải thích trên code:**

Giải thích điều kiện menuWindowIsOpen khiến gameplay input bị clear và hàm return sớm.

### Câu 4. Cách game xử lý di chuyển theo hướng camera?

**Trả lời mẫu:**

Game lấy input ngang/dọc rồi quy đổi theo forward và right của camera. Vector kết quả được chuẩn hóa để
nhân vật đi đúng hướng nhìn. Nhờ vậy phím tiến luôn đi theo hướng camera, phù hợp game hành động
góc nhìn thứ ba.

**Khi mở code:**

Mở `PlayerInputManager.HandlePlayerMovementInput(), PlayerCamera`.

**Cách giải thích trên code:**

Nói input chỉ là giá trị 2D, còn hướng di chuyển thật được tính lại theo hướng camera.

### Câu 5. Cơ chế lock-on target hoạt động như thế nào?

**Trả lời mẫu:**

Khi bấm lock-on, hệ thống tìm mục tiêu còn sống trong vùng quanh player hoặc camera, lọc theo tầm nhìn
và chọn target phù hợp. Khi đã khóa, camera bám target, hướng di chuyển và projectile có thể ưu tiên
target đó. Người chơi có thể tắt hoặc đổi target.

**Khi mở code:**

Mở `HandleLockOnInput(), HandleLockOnSwitchTargetInput(), PlayerCamera`.

**Cách giải thích trên code:**

Giải thích lock-on là trạng thái chọn target, sau đó camera và một số hành động chiến đấu dùng target
này.

### Câu 6. Khi nhân vật sprint, dodge, jump thì stamina được trừ ở đâu?

**Trả lời mẫu:**

Stamina được kiểm tra trước khi sprint, dodge hoặc jump. Nếu đủ, manager vận động hoặc combat cho phép hành
động và trừ stamina qua PlayerNetworkManager hoặc StatsManager. Khi giá trị đổi, HUD nhận callback để cập nhật thanh stamina.

**Khi mở code:**

Mở `HandleSprintInput(), HandleDodgeInput(), HandleJumpInput(), PlayerNetworkManager.currentStamina`.

**Cách giải thích trên code:**

Chỉ ra input không chỉ phát animation, mà còn kiểm tra tài nguyên và cập nhật stamina.

### Câu 7. Làm sao để tránh người chơi vừa dùng item vừa né/nhảy/tấn công gây lỗi trạng thái?

**Trả lời mẫu:**

Game dùng các cờ trạng thái như isPerformingAction, isJumping, isDodging, isUsingItem và isDead. Trước khi nhận input mới, hệ thống
kiểm tra các cờ này. Khi animation quan trọng đang chạy, input xung đột bị chặn hoặc clear để tránh
hai hành động đè nhau.

**Khi mở code:**

Mở `PlayerManager.isPerformingAction, PlayerCombatManager.isUsingItem, PlayerInputManager.HandleAllInputs()`.

**Cách giải thích trên code:**

Giải thích đây là state gating: mỗi action quan trọng đặt cờ, action khác phải kiểm tra cờ trước khi
chạy.

### Câu 8. Vì sao chỉ owner mới được xử lý input trong multiplayer?

**Trả lời mẫu:**

Trong multiplayer, mỗi client chỉ xử lý input cho nhân vật thuộc quyền sở hữu của mình. Nếu mọi client
đều đọc input cho mọi player, người này có thể điều khiển nhầm nhân vật người khác. Vì vậy PlayerManager
kiểm tra IsOwner trước khi đọc input.

**Khi mở code:**

Mở `PlayerManager.Update(), PlayerInputManager, NetworkBehaviour.IsOwner`.

**Cách giải thích trên code:**

Nói owner đọc input để có cảm giác điều khiển, còn trạng thái quan trọng vẫn gửi lên server khi
cần.

**Code tiêu biểu để chỉ khi bảo vệ:**

Nguồn: `Assets/Game/Scripts/Character/Player/PlayerInputManager.cs`.

```csharp
private void Update()
{
    HandleAllInputs();
}

private void HandleAllInputs()
{
    if (IsGameplayInputLocked())
    {
        HandleCloseUIInputs();
        ClearGameplayInputsWhileMenuOpen();
        return;
    }

    HandleUseItemInput();
    HandleTwoHandInput();
    HandleLockOnInput();
    HandleLockOnSwitchTargetInput();
    HandlePlayerMovementInput();
    HandleCameraMovementInput();
    HandleDodgeInput();
    HandleSprintInput();
    HandleJumpInput();
    HandleRBInput();
    HandleLBInput();
    HandleInteractionInput();
}
```

**Giải thích code:**

Update gọi HandleAllInputs liên tục theo frame. Đầu tiên hàm kiểm tra menu có đang mở không.
Nếu có, gameplay input bị clear và hàm dừng. Nếu không, input được chia sang từng nhóm chức năng.

## 4. Combat

### Câu 1. Luồng từ khi người chơi bấm nút tấn công đến khi quái bị mất máu diễn ra như thế nào?

**Trả lời mẫu:**

Người chơi bấm attack, PlayerInputManager ghi nhận input, PlayerCombatManager chọn vũ khí và WeaponItemAction. Action phát animation, bật damage collider
ở frame hợp lệ. Khi collider chạm enemy, damage gửi lên server xử lý, HP enemy đổi và UI/effect/death flow
cập nhật.

**Khi mở code:**

Mở `PlayerCombatManager.PerformWeaponBasedAction(), WeaponItemAction.AttemptToPerformAction()`.

**Cách giải thích trên code:**

Giải thích combat tách input khỏi action cụ thể của từng vũ khí.

### Câu 2. Vai trò của PlayerCombatManager.PerformWeaponBasedAction() là gì?

**Trả lời mẫu:**

PerformWeaponBasedAction nối input với action của vũ khí. Hàm nhận WeaponItemAction và WeaponItem, kiểm tra điều kiện rồi gọi action
thực thi. Nhờ vậy PlayerCombatManager không hard-code từng đòn, mà để ScriptableObject action quyết định animation, cost, combo hoặc cast.

**Khi mở code:**

Mở `PlayerCombatManager.PerformWeaponBasedAction(), WeaponItemAction.AttemptToPerformAction()`.

**Cách giải thích trên code:**

Giải thích combat tách input khỏi action cụ thể của từng vũ khí.

### Câu 3. WeaponItemAction được dùng để làm gì?

**Trả lời mẫu:**

WeaponItemAction mô tả một hành động của vũ khí như light attack, heavy attack, block, shoot hoặc cast spell. Mỗi
action có logic riêng và có thể gắn cho nhiều vũ khí. Cách này làm hệ thống combat linh hoạt
hơn khi thêm vũ khí mới.

**Khi mở code:**

Mở `PlayerCombatManager.PerformWeaponBasedAction(), WeaponItemAction.AttemptToPerformAction()`.

**Cách giải thích trên code:**

Giải thích combat tách input khỏi action cụ thể của từng vũ khí.

### Câu 4. Vì sao em dùng ScriptableObject cho weapon action?

**Trả lời mẫu:**

ScriptableObject giúp tách dữ liệu/hành vi action khỏi PlayerCombatManager. Designer có thể tạo action asset mới, gán animation, stamina cost,
damage modifier hoặc projectile mà không sửa code player. Nó giảm if/else và giúp mở rộng weapon/spell nhanh hơn.

**Khi mở code:**

Mở `PlayerCombatManager.PerformWeaponBasedAction(), WeaponItemAction.AttemptToPerformAction()`.

**Cách giải thích trên code:**

Giải thích combat tách input khỏi action cụ thể của từng vũ khí.

### Câu 5. Damage collider được bật/tắt ở thời điểm nào?

**Trả lời mẫu:**

Damage collider được bật bằng animation event tại frame vung vũ khí có khả năng gây sát thương, rồi tắt
khi cửa sổ đánh kết thúc. Collider không bật liên tục, nên hitbox khớp animation hơn và tránh gây damage
khi vũ khí chưa thực sự chạm mục tiêu.

**Khi mở code:**

Mở `MeleeWeaponDamageCollider, CharacterNetworkManager.NotifyTheServerOfCharacterDamageServerRpc()`.

**Cách giải thích trên code:**

Giải thích collider chỉ bật theo animation frame, khi va chạm mới gửi damage.

### Câu 6. Damage được tính ở client hay server?

**Trả lời mẫu:**

Client có thể phát animation để phản hồi nhanh, nhưng damage quan trọng nên được server xác nhận. Client gửi
ServerRpc chứa hit/damage, server áp dụng vào HP NetworkVariable, sau đó các client nhận kết quả. Cách này giảm lệch
trạng thái và hạn chế client tự trừ máu.

**Khi mở code:**

Mở `CharacterNetworkManager.NotifyTheServerOfCharacterDamageServerRpc()`.

**Cách giải thích trên code:**

Nói rõ client gửi yêu cầu, server xác nhận, ClientRpc/NetworkVariable đồng bộ kết quả.

### Câu 7. Vì sao cần ServerRpc và ClientRpc khi xử lý sát thương?

**Trả lời mẫu:**

ServerRpc dùng để client gửi ý định lên server, ví dụ gây damage. ClientRpc dùng để server thông báo kết
quả hoặc hiệu ứng cho các client, ví dụ hit reaction hoặc boss defeated. Hai RPC tách rõ: client yêu
cầu, server quyết định, client hiển thị.

**Khi mở code:**

Mở `CharacterNetworkManager.NotifyTheServerOfCharacterDamageServerRpc()`.

**Cách giải thích trên code:**

Nói rõ client gửi yêu cầu, server xác nhận, ClientRpc/NetworkVariable đồng bộ kết quả.

### Câu 8. Cơ chế combo hoạt động như thế nào?

**Trả lời mẫu:**

Combo dựa trên trạng thái đang tấn công và cửa sổ cho phép queue input. Nếu người chơi bấm tiếp
trong thời điểm hợp lệ, PlayerCombatManager chuyển sang animation combo tiếp theo. Nếu bấm quá sớm hoặc quá muộn, combo
không nối.

**Khi mở code:**

Mở `PlayerCombatManager.PerformWeaponBasedAction(), WeaponItemAction.AttemptToPerformAction()`.

**Cách giải thích trên code:**

Giải thích combat tách input khỏi action cụ thể của từng vũ khí.

### Câu 9. Cơ chế block/parry/backstab/riposte hoạt động ra sao?

**Trả lời mẫu:**

Block giảm hoặc chặn damage khi người chơi đang phòng thủ đúng hướng và còn stamina. Parry cần đúng timing
để làm địch mất thế. Backstab/riposte yêu cầu vị trí hoặc trạng thái đặc biệt, sau đó phát animation critical
và áp dụng damage lớn qua hệ thống damage.

**Khi mở code:**

Mở `PlayerCombatManager.PerformWeaponBasedAction(), WeaponItemAction.AttemptToPerformAction()`.

**Cách giải thích trên code:**

Giải thích combat tách input khỏi action cụ thể của từng vũ khí.

### Câu 10. Nếu hai người chơi cùng đánh một enemy thì hệ thống xử lý thế nào?

**Trả lời mẫu:**

Nếu hai player cùng đánh một enemy, mỗi hit được gửi lên server. Server xử lý từng damage theo thứ
tự nhận được và cập nhật HP hiện tại. Vì HP là trạng thái network, mọi client cuối cùng nhìn
thấy cùng kết quả sau khi server đồng bộ.

**Khi mở code:**

Mở `CharacterNetworkManager.NotifyTheServerOfCharacterDamageServerRpc(), currentHealth NetworkVariable`.

**Cách giải thích trên code:**

Giải thích mỗi client gửi hit lên server, server xử lý theo thứ tự nhận được và HP network đảm bảo mọi client thấy cùng kết quả.

**Code tiêu biểu để chỉ khi bảo vệ:**

Nguồn: `Assets/Game/Scripts/Character/Player/PlayerCombatManager.cs và Assets/Game/Scripts/Character/CharacterNetworkManager.cs`.

```csharp
public void PerformWeaponBasedAction(WeaponItemAction weaponAction, WeaponItem weaponPerformingAction)
{
    if (player.IsOwner)
    {
        weaponAction.AttemptToPerformAction(player, weaponPerformingAction);
    }
}

[ServerRpc(RequireOwnership = false)]
public void NotifyTheServerOfCharacterDamageServerRpc(
    ulong damagedCharacterID,
    ulong characterCausingDamageID,
    float physicalDamage,
    float magicDamage,
    float fireDamage,
    float lightningDamage,
    float holyDamage,
    float poiseDamage,
    float angleHitFrom,
    float contactPointX,
    float contactPointY,
    float contactPointZ)
{
    if (IsServer)
    {
        NotifyTheServerOfCharacterDamageClientRpc(
            damagedCharacterID,
            characterCausingDamageID,
            physicalDamage,
            magicDamage,
            fireDamage,
            lightningDamage,
            holyDamage,
            poiseDamage,
            angleHitFrom,
            contactPointX,
            contactPointY,
            contactPointZ);
    }
}
```

**Giải thích code:**

PerformWeaponBasedAction chỉ cho owner thực hiện action. Damage collider/RPC đưa sát thương lên server, rồi server phát kết quả xuống
client.
## 5. Vũ khí, projectile và phép

### Câu 1. Vũ khí được lưu và load bằng cách nào?

**Trả lời mẫu:**

Vũ khí được lưu bằng dữ liệu tuần tự hóa, không lưu trực tiếp object Unity. Save data ghi weapon
ID, upgrade level và dữ liệu phụ. Khi load, WorldSaveGameManager dùng ID tra database để dựng lại WeaponItem runtime với
cấp nâng cấp đúng.

**Khi mở code:**

Mở `PlayerNetworkManager.OnCurrentRightHandWeaponIDChange(), WorldSaveGameManager`.

**Cách giải thích trên code:**

Nói ID là cầu nối giữa save, inventory, network và model trên tay.

### Câu 2. Weapon ID có vai trò gì?

**Trả lời mẫu:**

Weapon ID là khóa định danh ổn định cho từng vũ khí. Save/load, inventory, network sync và đổi model đều
dựa vào ID. Nếu lưu reference object trực tiếp, dữ liệu dễ hỏng khi đổi scene hoặc build. ID giúp
dữ liệu bền hơn.

**Khi mở code:**

Mở `PlayerNetworkManager.OnCurrentRightHandWeaponIDChange(), WorldSaveGameManager`.

**Cách giải thích trên code:**

Nói ID là cầu nối giữa save, inventory, network và model trên tay.

### Câu 3. Khi đổi vũ khí, model trên tay nhân vật được cập nhật như thế nào?

**Trả lời mẫu:**

Khi weapon ID trên tay thay đổi, callback trong PlayerNetworkManager chạy. Callback tìm WeaponItem tương ứng, cập nhật equipment/inventory và
gọi PlayerEquipmentManager load model vào slot tay. Vì ID đồng bộ qua network, client khác cũng thấy đúng model.

**Khi mở code:**

Mở `PlayerNetworkManager.OnCurrentRightHandWeaponIDChange(), WorldSaveGameManager`.

**Cách giải thích trên code:**

Nói ID là cầu nối giữa save, inventory, network và model trên tay.

### Câu 4. Cơ chế bắn cung hoạt động ra sao?

**Trả lời mẫu:**

Bắn cung là weapon action đặc biệt. Hệ thống kiểm tra vũ khí, đạn và tài nguyên, phát animation kéo
cung, rồi tạo projectile. Hướng bắn lấy theo camera hoặc lock-on target. Projectile bay bằng logic riêng và gây damage
khi va chạm hợp lệ.

**Khi mở code:**

Mở `Projectile prefab, damage collider, ServerRpc/ClientRpc`.

**Cách giải thích trên code:**

Giải thích projectile cần đồng bộ vị trí/hướng và damage nên để server xác nhận.

### Câu 5. Projectile được đồng bộ multiplayer như thế nào?

**Trả lời mẫu:**

Projectile trong multiplayer nên được server spawn hoặc server xác nhận qua RPC. Client owner gửi yêu cầu bắn, server
tạo projectile network hoặc gọi ClientRpc để mọi máy cùng thấy vị trí, hướng và tốc độ. Damage cuối cùng
vẫn nên do server áp dụng.

**Khi mở code:**

Mở `Projectile prefab, damage collider, ServerRpc/ClientRpc`.

**Cách giải thích trên code:**

Giải thích projectile cần đồng bộ vị trí/hướng và damage nên để server xác nhận.

### Câu 6. Khi người chơi lock-on và bắn projectile, hướng bay được tính thế nào?

**Trả lời mẫu:**

Khi lock-on, hướng bay ưu tiên vector từ điểm bắn tới target, thường thêm offset vào thân mục tiêu. Nếu
không lock-on, hướng dùng camera forward hoặc điểm ngắm. Cách này hỗ trợ bắn có mục tiêu nhưng vẫn cho
phép bắn tự do.

**Khi mở code:**

Mở `Projectile prefab, damage collider, ServerRpc/ClientRpc`.

**Cách giải thích trên code:**

Giải thích projectile cần đồng bộ vị trí/hướng và damage nên để server xác nhận.

### Câu 7. Spell khác gì so với weapon action thông thường?

**Trả lời mẫu:**

Spell vẫn dùng pipeline action, nhưng thường tiêu tốn FP, có cast animation, warm-up, VFX và projectile/effect riêng. Weapon action
cận chiến chủ yếu dùng hitbox vũ khí và stamina. Spell cần kiểm tra ô phép, đủ FP và prefab
hiệu ứng.

**Khi mở code:**

Mở `FireBallSpell, FireBallDamageCollider, WeaponItemAction`.

**Cách giải thích trên code:**

Giải thích spell vẫn là action nhưng tiêu FP, có cast animation, VFX và projectile riêng.

### Câu 8. FireBallSpell xử lý warm-up và cast như thế nào?

**Trả lời mẫu:**

FireBallSpell thường có pha warm-up và pha cast. Warm-up phát animation/VFX chuẩn bị và khóa hành động khác. Đến frame
cast, spell tạo fireball ở tay/catalyst, tính hướng theo target hoặc camera, rồi spawn projectile gây damage hoặc burning khi
va chạm.

**Khi mở code:**

Mở `FireBallSpell, FireBallDamageCollider, WeaponItemAction`.

**Cách giải thích trên code:**

Giải thích spell vẫn là action nhưng tiêu FP, có cast animation, VFX và projectile riêng.

### Câu 9. Nếu muốn thêm một spell mới, em cần tạo/sửa những gì?

**Trả lời mẫu:**

Để thêm spell mới, em tạo ScriptableObject spell, khai báo cost, animation, VFX, projectile hoặc effect. Nếu dùng projectile mới,
cần prefab và damage collider. Sau đó thêm spell vào database, shop/loot/inventory, trang bị thử và kiểm tra save/load.

**Khi mở code:**

Mở `FireBallSpell, FireBallDamageCollider, WeaponItemAction`.

**Cách giải thích trên code:**

Giải thích spell vẫn là action nhưng tiêu FP, có cast animation, VFX và projectile riêng.

## 6. Stat, damage và effect

### Câu 1. Các chỉ số chính của nhân vật gồm những gì?

**Trả lời mẫu:**

Các chỉ số chính gồm level, vigor, endurance, mind, strength, dexterity, intelligence/faith tùy thiết kế, cùng HP, stamina, FP, rune,
damage absorption, resistance và upgrade level vũ khí. Stat nền quyết định tài nguyên, damage và khả năng sống sót.

**Khi mở code:**

Mở `PlayerNetworkManager, CharacterStatsManager, PlayerStatsManager`.

**Cách giải thích trên code:**

Nói stat nền sinh ra tài nguyên và ảnh hưởng damage/effect.

### Câu 2. HP, stamina, FP được tính theo stat nào?

**Trả lời mẫu:**

HP thường tăng theo Vigor, stamina tăng theo Endurance, FP tăng theo Mind. Khi stat tăng, hệ thống tính lại
max value trong PlayerNetworkManager/StatsManager. UI không tự tính công thức, mà hiển thị giá trị đã được gameplay cập nhật.

**Khi mở code:**

Mở `PlayerNetworkManager.SetNewMaxHealthValue(), PlayerManager.OnNetworkSpawn()`.

**Cách giải thích trên code:**

Giải thích stat đổi làm max value đổi, UI nhận callback để cập nhật.

### Câu 3. CharacterStatsManager và PlayerStatsManager khác nhau thế nào?

**Trả lời mẫu:**

CharacterStatsManager chứa logic stat chung cho mọi nhân vật như nhận damage, chết hoặc resistance. PlayerStatsManager mở rộng cho người
chơi, thêm level, rune, level up và cập nhật tài nguyên theo stat. Enemy không cần toàn bộ logic riêng
của player.

**Khi mở code:**

Mở `PlayerNetworkManager, CharacterStatsManager, PlayerStatsManager`.

**Cách giải thích trên code:**

Nói stat nền sinh ra tài nguyên và ảnh hưởng damage/effect.

### Câu 4. Khi người chơi tăng Vigor thì max HP cập nhật ra sao?

**Trả lời mẫu:**

Khi confirm level up, Vigor mới được ghi vào dữ liệu player. Sau đó công thức max HP chạy lại,
NetworkVariable maxHealth thay đổi và current HP có thể được điều chỉnh. HUD nhận OnValueChanged để cập nhật thanh máu
theo giá trị mới.

**Khi mở code:**

Mở `PlayerNetworkManager.SetNewMaxHealthValue(), PlayerManager.OnNetworkSpawn()`.

**Cách giải thích trên code:**

Giải thích stat đổi làm max value đổi, UI nhận callback để cập nhật.

### Câu 5. Status effect như poison, burning, bleed, frost hoạt động thế nào?

**Trả lời mẫu:**

Status effect thường có buildup và ngưỡng kích hoạt. Poison/burning gây damage theo thời gian, bleed gây damage lớn tức
thời, frost có thể làm giảm khả năng chịu đòn hoặc stamina. CharacterEffectsManager quản lý thêm effect, chạy timer, tick
damage và gỡ effect.

**Khi mở code:**

Mở `CharacterEffectsManager, InstantCharacterEffect, TimedCharacterEffect`.

**Cách giải thích trên code:**

Giải thích effect được gom về manager để tránh item/spell tự sửa stat ở nhiều nơi.

### Câu 6. Instant effect và timed effect khác nhau thế nào?

**Trả lời mẫu:**

Instant effect áp dụng một lần ngay lập tức, ví dụ hồi máu hoặc gây bleed burst. Timed effect tồn
tại trong một khoảng thời gian, ví dụ poison tick hoặc buff tăng damage. Tách hai loại giúp item, spell
và status dễ mở rộng.

**Khi mở code:**

Mở `CharacterEffectsManager, InstantCharacterEffect, TimedCharacterEffect`.

**Cách giải thích trên code:**

Giải thích effect được gom về manager để tránh item/spell tự sửa stat ở nhiều nơi.

### Câu 7. CharacterEffectsManager có vai trò gì?

**Trả lời mẫu:**

CharacterEffectsManager là nơi nhận, chạy và gỡ hiệu ứng trên nhân vật. Nó kiểm tra effect có hợp lệ không,
chạy timer/coroutine, gọi tick damage hoặc buff stat. Nhờ vậy item, spell và weapon không tự sửa stat lung tung
ở nhiều nơi.

**Khi mở code:**

Mở `CharacterEffectsManager, InstantCharacterEffect, TimedCharacterEffect`.

**Cách giải thích trên code:**

Giải thích effect được gom về manager để tránh item/spell tự sửa stat ở nhiều nơi.

### Câu 8. Buff charm được áp dụng và gỡ bỏ như thế nào?

**Trả lời mẫu:**

Khi trang bị charm, equipment/inventory gọi logic áp buff lên player, ví dụ tăng HP, stamina, damage hoặc resistance. Khi
tháo charm, buff bị gỡ và stat được tính lại. Điểm quan trọng là không cộng dồn sai khi thay
charm nhiều lần.

**Khi mở code:**

Mở `CharacterEffectsManager, InstantCharacterEffect, TimedCharacterEffect`.

**Cách giải thích trên code:**

Giải thích effect được gom về manager để tránh item/spell tự sửa stat ở nhiều nơi.

### Câu 9. Làm sao để UI biết HP/stamina đã thay đổi?

**Trả lời mẫu:**

UI biết HP/stamina thay đổi nhờ callback từ NetworkVariable hoặc event dữ liệu. Khi giá trị đổi, HUD cập nhật
slider/bar. UI không polling và không tự đoán damage, mà phản ánh dữ liệu thật từ PlayerNetworkManager hoặc StatsManager.

**Khi mở code:**

Mở `PlayerNetworkManager.SetNewMaxHealthValue(), PlayerManager.OnNetworkSpawn()`.

**Cách giải thích trên code:**

Giải thích stat đổi làm max value đổi, UI nhận callback để cập nhật.

## 7. AI quái thường

### Câu 1. AI trong game dùng mô hình gì?

**Trả lời mẫu:**

AI dùng Finite State Machine. Enemy có currentState như idle, pursue target và attack. Mỗi tick, AICharacterManager gọi ProcessStateMachine, state
hiện tại đánh giá điều kiện và trả về state tiếp theo. Mô hình này rõ ràng, dễ debug và
phù hợp đồ án.

**Khi mở code:**

Mở `AICharacterManager.ProcessStateMachine(), IdleState, PursueTargetState, AttackState`.

**Cách giải thích trên code:**

Giải thích mỗi tick AI gọi state hiện tại, state trả về state tiếp theo nếu điều kiện thay đổi.

### Câu 2. Vì sao em dùng Finite State Machine cho AI?

**Trả lời mẫu:**

Finite State Machine phù hợp vì hành vi enemy có số trạng thái hữu hạn và chuyển đổi rõ: chờ,
phát hiện player, đuổi theo, tấn công, chết. So với behavior tree, FSM đơn giản hơn, dễ trình bày và
đủ đáp ứng enemy cơ bản.

**Khi mở code:**

Mở `AICharacterManager.ProcessStateMachine(), IdleState, PursueTargetState, AttackState`.

**Cách giải thích trên code:**

Giải thích mỗi tick AI gọi state hiện tại, state trả về state tiếp theo nếu điều kiện thay đổi.

### Câu 3. Các state chính của AI gồm những gì?

**Trả lời mẫu:**

Các state chính gồm IdleState để chờ hoặc tìm mục tiêu, PursueTargetState để chạy theo player bằng NavMeshAgent và AttackState
để chọn đòn đánh. Boss có thể thêm phase/combat state riêng, nhưng vẫn dựa trên nguyên tắc chuyển state.

**Khi mở code:**

Mở `AICharacterManager.ProcessStateMachine(), IdleState, PursueTargetState, AttackState`.

**Cách giải thích trên code:**

Giải thích mỗi tick AI gọi state hiện tại, state trả về state tiếp theo nếu điều kiện thay đổi.

### Câu 4. AICharacterManager.ProcessStateMachine() hoạt động như thế nào?

**Trả lời mẫu:**

ProcessStateMachine kiểm tra AI có chết hoặc bị khóa hành động không, rồi gọi Tick của currentState. State xử lý
logic riêng và trả về state mới nếu điều kiện đổi. Nếu có state mới, AI cập nhật currentState. Đây
là vòng lặp quyết định hành vi enemy.

**Khi mở code:**

Mở `AICharacterManager.ProcessStateMachine(), IdleState, PursueTargetState, AttackState`.

**Cách giải thích trên code:**

Giải thích mỗi tick AI gọi state hiện tại, state trả về state tiếp theo nếu điều kiện thay đổi.

### Câu 5. AI phát hiện người chơi bằng cách nào?

**Trả lời mẫu:**

AI phát hiện player bằng khoảng cách, góc nhìn, layer mask hoặc trigger vùng phát hiện. Khi player hợp lệ
trong detection radius, enemy gán currentTarget. Sau đó state chuyển từ idle sang pursue hoặc attack tùy khoảng cách.

**Khi mở code:**

Mở `AICharacterManager.ProcessStateMachine(), IdleState, PursueTargetState, AttackState`.

**Cách giải thích trên code:**

Giải thích mỗi tick AI gọi state hiện tại, state trả về state tiếp theo nếu điều kiện thay đổi.

### Câu 6. AI truy đuổi người chơi bằng gì?

**Trả lời mẫu:**

AI truy đuổi bằng NavMeshAgent. PursueTargetState đặt destination là vị trí player, đồng thời animator nhận vận tốc để phát
animation di chuyển. NavMeshAgent giúp enemy tìm đường trên NavMesh và tránh vật cản cơ bản.

**Khi mở code:**

Mở `AICharacterManager.ProcessStateMachine(), IdleState, PursueTargetState, AttackState`.

**Cách giải thích trên code:**

Giải thích mỗi tick AI gọi state hiện tại, state trả về state tiếp theo nếu điều kiện thay đổi.

### Câu 7. Vai trò của NavMeshAgent là gì?

**Trả lời mẫu:**

NavMeshAgent chịu trách nhiệm pathfinding và di chuyển trên NavMesh. AI chỉ cần cung cấp destination, speed và stopping distance.
Agent tính đường hợp lệ quanh vật cản, ổn định hơn so với tự cộng vector transform mỗi frame.

**Khi mở code:**

Mở `AICharacterManager.ProcessStateMachine(), IdleState, PursueTargetState, AttackState`.

**Cách giải thích trên code:**

Giải thích mỗi tick AI gọi state hiện tại, state trả về state tiếp theo nếu điều kiện thay đổi.

### Câu 8. AI chọn đòn đánh như thế nào?

**Trả lời mẫu:**

AI chọn đòn dựa trên khoảng cách tới target, góc đứng, cooldown, trọng số attack và trạng thái hiện tại.
Nếu player ở gần, nó chọn melee. Nếu ngoài tầm, nó tiếp tục pursue hoặc dùng attack xa nếu có.

**Khi mở code:**

Mở `AICharacterManager.ProcessStateMachine(), IdleState, PursueTargetState, AttackState`.

**Cách giải thích trên code:**

Giải thích mỗi tick AI gọi state hiện tại, state trả về state tiếp theo nếu điều kiện thay đổi.

### Câu 9. Nếu AI không tìm được đường đến người chơi thì xử lý ra sao?

**Trả lời mẫu:**

Nếu không có path hợp lệ, AI nên dừng truy đuổi, thử đặt lại destination, quay về idle hoặc reset
vị trí khi bị kẹt. Khi debug, em kiểm tra NavMesh đã bake chưa, player có nằm trên vùng reachable
không và obstacle có chặn đường không.

**Khi mở code:**

Mở `AICharacterManager.ProcessStateMachine(), IdleState, PursueTargetState, AttackState`.

**Cách giải thích trên code:**

Giải thích mỗi tick AI gọi state hiện tại, state trả về state tiếp theo nếu điều kiện thay đổi.

### Câu 10. Difficulty của enemy tăng theo map như thế nào?

**Trả lời mẫu:**

Difficulty enemy tăng theo map bằng cách nhân HP, damage, rune reward hoặc resistance theo tier/scene. Cách này tái sử
dụng prefab nhưng vẫn tạo độ khó tăng dần qua 5 map, phù hợp progression của đồ án.

**Khi mở code:**

Mở `AICharacterManager.ProcessStateMachine(), IdleState, PursueTargetState, AttackState`.

**Cách giải thích trên code:**

Giải thích mỗi tick AI gọi state hiện tại, state trả về state tiếp theo nếu điều kiện thay đổi.

**Code tiêu biểu để chỉ khi bảo vệ:**

Nguồn: `Assets/Game/Scripts/Character/AI Character/AICharacterManager.cs`.

```csharp
private void ProcessStateMachine()
{
    AIState nextState = currentState?.Tick(this);

    if (nextState != null && !hasManuallySwitchedState)
    {
        currentState = nextState;
    }

    hasManuallySwitchedState = false;
    navMeshAgent.transform.localPosition = Vector3.zero;
    navMeshAgent.transform.localRotation = Quaternion.identity;

    if (aiCharacterCombatManager.currentTarget != null)
    {
        aiCharacterCombatManager.targetsDirection =
            aiCharacterCombatManager.currentTarget.transform.position - transform.position;
        aiCharacterCombatManager.distanceFromTarget =
            Vector3.Distance(transform.position,
            aiCharacterCombatManager.currentTarget.transform.position);
    }
}
```

**Giải thích code:**

AI gọi Tick của state hiện tại. State có thể trả về state mới. Sau đó manager cập nhật hướng,
khoảng cách tới target và trạng thái di chuyển.
## 8. Boss

### Câu 1. Boss fight được kích hoạt khi nào?

**Trả lời mẫu:**

Boss fight kích hoạt khi player đi vào vùng EventTriggerBossFight hoặc thỏa điều kiện bắt đầu. Trigger gọi WakeBoss, bật
fog wall, hiển thị boss HP bar và chuyển boss từ trạng thái chờ sang combat. Từ đó boss bắt
đầu chọn target và xử lý AI.

**Khi mở code:**

Mở `EventTriggerBossFight.OnTriggerEnter(), AIBossCharacterManager.WakeBoss()`.

**Cách giải thích trên code:**

Giải thích trigger trong scene đánh thức boss và bật UI/fog wall.

### Câu 2. EventTriggerBossFight làm nhiệm vụ gì?

**Trả lời mẫu:**

EventTriggerBossFight là cầu nối giữa scene và boss logic. Nó phát hiện player vào vùng boss, kiểm tra boss đã
chết chưa, gọi boss thức dậy, bật UI boss, bật fog wall và khóa lối ra nếu cần. Nhờ vậy
boss không tự chạy khi scene vừa load.

**Khi mở code:**

Mở `EventTriggerBossFight.OnTriggerEnter(), AIBossCharacterManager.WakeBoss()`.

**Cách giải thích trên code:**

Giải thích trigger trong scene đánh thức boss và bật UI/fog wall.

### Câu 3. AIBossCharacterManager.WakeBoss() xử lý những gì?

**Trả lời mẫu:**

WakeBoss chuyển boss sang trạng thái active. Hàm bật AI/combat flag, đăng ký boss HP bar, có thể phát animation
xuất hiện hoặc nhạc boss. Nó cũng cần chống gọi lặp khi nhiều player cùng bước vào trigger.

**Khi mở code:**

Mở `EventTriggerBossFight.OnTriggerEnter(), AIBossCharacterManager.WakeBoss()`.

**Cách giải thích trên code:**

Giải thích trigger trong scene đánh thức boss và bật UI/fog wall.

### Câu 4. Fog wall được bật/tắt như thế nào?

**Trả lời mẫu:**

Fog wall bật khi boss fight bắt đầu để giới hạn khu vực chiến đấu, và tắt khi boss chết
hoặc trận kết thúc. Trong multiplayer, trạng thái này nên do server quyết định rồi đồng bộ để mọi client
thấy cùng cửa chắn.

**Khi mở code:**

Mở `AIBossCharacterManager.ProcessDeathEvent(), GameProgressionManager.RegisterBossDefeat()`.

**Cách giải thích trên code:**

Giải thích boss death flow chạy trên server, đặt cờ chết, tắt fog wall, thưởng rune và mở map.

### Câu 5. Boss HP bar xuất hiện ở đâu?

**Trả lời mẫu:**

Boss HP bar do UI/HUD bật khi boss fight bắt đầu. EventTriggerBossFight hoặc WakeBoss truyền thông tin boss cho UI,
UI đặt tên boss và theo dõi HP. Khi HP boss đổi, thanh máu cập nhật; khi boss chết, bar
bị ẩn.

**Khi mở code:**

Mở `AIBossCharacterManager.ProcessDeathEvent(), GameProgressionManager.RegisterBossDefeat()`.

**Cách giải thích trên code:**

Giải thích boss death flow chạy trên server, đặt cờ chết, tắt fog wall, thưởng rune và mở map.

### Câu 6. Boss chuyển phase dựa trên điều kiện nào?

**Trả lời mẫu:**

Boss chuyển phase dựa trên điều kiện như phần trăm HP, cờ phase hoặc event đặc biệt. Ví dụ dưới
50% HP, boss đổi moveset hoặc thêm đòn mới. Cần có cờ để phase transition không chạy nhiều lần.

**Khi mở code:**

Mở `AIBossCharacterManager.ProcessDeathEvent(), GameProgressionManager.RegisterBossDefeat()`.

**Cách giải thích trên code:**

Giải thích boss death flow chạy trên server, đặt cờ chết, tắt fog wall, thưởng rune và mở map.

### Câu 7. Khi boss chết, luồng xử lý diễn ra thế nào?

**Trả lời mẫu:**

Khi boss chết, ProcessDeathEvent dừng AI, phát animation chết, tắt fog wall, cập nhật boss HP bar, thưởng rune/item nếu
có, gọi RegisterBossDefeat và lưu progression. Nếu boss mở map mới, map kế tiếp được unlock sau bước này.

**Khi mở code:**

Mở `AIBossCharacterManager.ProcessDeathEvent(), GameProgressionManager.RegisterBossDefeat()`.

**Cách giải thích trên code:**

Giải thích boss death flow chạy trên server, đặt cờ chết, tắt fog wall, thưởng rune và mở map.

### Câu 8. AIBossCharacterManager.ProcessDeathEvent() vì sao là hàm quan trọng?

**Trả lời mẫu:**

ProcessDeathEvent quan trọng vì nó là điểm kết thúc boss fight và mở progression. Nếu hàm này lỗi, boss có
thể chết nhưng fog wall không tắt, UI không biến mất, map mới không mở hoặc save không ghi boss
defeated.

**Khi mở code:**

Mở `AIBossCharacterManager.ProcessDeathEvent(), GameProgressionManager.RegisterBossDefeat()`.

**Cách giải thích trên code:**

Giải thích boss death flow chạy trên server, đặt cờ chết, tắt fog wall, thưởng rune và mở map.

### Câu 9. Boss chết thì map mới được mở bằng cách nào?

**Trả lời mẫu:**

Boss chết thì AIBossCharacterManager gọi GameProgressionManager.RegisterBossDefeat với boss ID. Progression manager ghi nhận boss đã chết, tính map kế tiếp
được mở và lưu vào save. Sau đó transition hoặc Site of Grace có thể cho player đi tiếp.

**Khi mở code:**

Mở `AIBossCharacterManager.ProcessDeathEvent(), GameProgressionManager.RegisterBossDefeat()`.

**Cách giải thích trên code:**

Giải thích boss death flow chạy trên server, đặt cờ chết, tắt fog wall, thưởng rune và mở map.

### Câu 10. Làm sao để tránh boss death flow chạy nhiều lần?

**Trả lời mẫu:**

Để tránh death flow chạy nhiều lần, boss dùng cờ như hasBossBeenDefeated hoặc isDead. Khi ProcessDeathEvent chạy lần đầu, cờ
được bật trước khi phát thưởng và unlock map. Các lần gọi sau return ngay, đặc biệt quan trọng trong
multiplayer.

**Khi mở code:**

Mở `AIBossCharacterManager.ProcessDeathEvent(), bossDefeatFlowStarted, hasBeenDefeated`.

**Cách giải thích trên code:**

Chỉ ra cờ bossDefeatFlowStarted được bật ở đầu death flow, các lần gọi sau sẽ yield break để không thưởng hoặc unlock lặp.

**Code tiêu biểu để chỉ khi bảo vệ:**

Nguồn: `Assets/Game/Scripts/Character/AI Character/Boss Character/AIBossCharacterManager.cs`.

```csharp
public override IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false)
{
    if (bossDefeatFlowStarted)
    {
        yield break;
    }

    if (!IsServer)
    {
        RequestProcessBossDeathServerRpc(manuallySelectDeathAnimation);
        yield break;
    }

    bossDefeatFlowStarted = true;
    characterNetworkManager.currentHealth.Value = 0;
    isDead.Value = true;
    bossFightIsActive.Value = false;

    foreach (var fogWall in fogWalls)
    {
        if (fogWall != null)
            fogWall.isActive.Value = false;
    }

    hasBeenDefeated.Value = true;
}
```

**Giải thích code:**

Death flow có cờ chặn chạy lặp. Nếu không phải server thì gửi yêu cầu lên server. Server mới đặt
boss chết, tắt fog wall và đánh dấu defeated.
## 9. Progression và chuyển map

### Câu 1. Game có bao nhiêu map và quản lý progression như thế nào?

**Trả lời mẫu:**

Game có progression qua 5 map. GameProgressionManager lưu boss nào đã chết, map nào đã unlock và scene hiện tại.
Khi player thắng boss hoặc dùng transition hợp lệ, manager kiểm tra điều kiện rồi cho phép mở map tiếp
theo.

**Khi mở code:**

Mở `GameProgressionManager.RegisterBossDefeat(), WorldSceneManager.LoadWorldScene()`.

**Cách giải thích trên code:**

Nói bossID xác định map vừa hoàn thành, manager unlock map kế tiếp rồi trả scene build index để chuyển
scene.

### Câu 2. GameProgressionManager lưu những thông tin gì?

**Trả lời mẫu:**

GameProgressionManager lưu danh sách boss defeated, map/scene unlocked, current map, next map và các cờ hoàn thành quan trọng. Dữ
liệu này được WorldSaveGameManager ghi vào save để khi load lại, game biết player đã mở nội dung nào.

**Khi mở code:**

Mở `GameProgressionManager.RegisterBossDefeat(), WorldSceneManager.LoadWorldScene()`.

**Cách giải thích trên code:**

Nói bossID xác định map vừa hoàn thành, manager unlock map kế tiếp rồi trả scene build index để chuyển
scene.

### Câu 3. RegisterBossDefeat() làm gì?

**Trả lời mẫu:**

RegisterBossDefeat nhận boss ID vừa chết, đánh dấu boss đó đã defeated, cập nhật progression và mở khóa map tương
ứng. Hàm này là cầu nối trực tiếp giữa boss death và hệ thống mở map, nên nếu unlock lỗi
thì cần kiểm tra ở đây.

**Khi mở code:**

Mở `GameProgressionManager.RegisterBossDefeat(), WorldSceneManager.LoadWorldScene()`.

**Cách giải thích trên code:**

Nói bossID xác định map vừa hoàn thành, manager unlock map kế tiếp rồi trả scene build index để chuyển
scene.

### Câu 4. Khi đánh bại boss map 1, làm sao game biết mở map 2?

**Trả lời mẫu:**

Boss map 1 có ID hoặc scene index liên kết với progression. Khi boss chết, RegisterBossDefeat tra mapping boss-to-next-map. Nếu
ID đó là điều kiện của map 2, manager bật cờ map 2 unlocked và lưu lại.

**Khi mở code:**

Mở `GameProgressionManager.RegisterBossDefeat(), WorldSceneManager.LoadWorldScene()`.

**Cách giải thích trên code:**

Nói bossID xác định map vừa hoàn thành, manager unlock map kế tiếp rồi trả scene build index để chuyển
scene.

### Câu 5. Scene build index được dùng như thế nào?

**Trả lời mẫu:**

Scene build index là số định danh scene trong Build Settings. WorldSceneManager dùng index để load scene kế tiếp hoặc
scene đã lưu. Nếu index sai, game có thể load nhầm scene hoặc không load được.

**Khi mở code:**

Mở `GameProgressionManager.RegisterBossDefeat(), WorldSceneManager.LoadWorldScene()`.

**Cách giải thích trên code:**

Nói bossID xác định map vừa hoàn thành, manager unlock map kế tiếp rồi trả scene build index để chuyển
scene.

### Câu 6. Entry Site of Grace của từng map có vai trò gì?

**Trả lời mẫu:**

Entry Site of Grace là điểm xuất hiện an toàn khi vào map mới hoặc load map. Khi transition, player
được đặt tại Site tương ứng thay vì vị trí ngẫu nhiên. Nó cũng làm mốc checkpoint và hồi sinh.

**Khi mở code:**

Mở `GameProgressionManager.RegisterBossDefeat(), WorldSceneManager.LoadWorldScene()`.

**Cách giải thích trên code:**

Nói bossID xác định map vừa hoàn thành, manager unlock map kế tiếp rồi trả scene build index để chuyển
scene.

### Câu 7. Nếu map sau cùng được mở thì điều kiện thắng game được xử lý ra sao?

**Trả lời mẫu:**

Khi map cuối hoặc boss cuối hoàn thành, progression không mở map tiếp nữa mà bật điều kiện kết thúc.
Game có thể hiển thị victory popup, lưu trạng thái hoàn thành, quay về title hoặc cho phép tiếp tục
khám phá.

**Khi mở code:**

Mở `GameProgressionManager.RegisterBossDefeat(), WorldSceneManager.LoadWorldScene()`.

**Cách giải thích trên code:**

Nói bossID xác định map vừa hoàn thành, manager unlock map kế tiếp rồi trả scene build index để chuyển
scene.

### Câu 8. Chuyển map bằng interactable khác gì chuyển map sau khi thắng boss?

**Trả lời mẫu:**

Interactable chuyển map là hành động chủ động khi player đứng tại cổng đủ điều kiện. Chuyển map sau thắng
boss là event progression tự mở khóa. Hai luồng khác nguồn kích hoạt, nhưng cuối cùng đều gọi WorldSceneManager để
load scene.

**Khi mở code:**

Mở `GameProgressionManager.RegisterBossDefeat(), WorldSceneManager.LoadWorldScene()`.

**Cách giải thích trên code:**

Nói bossID xác định map vừa hoàn thành, manager unlock map kế tiếp rồi trả scene build index để chuyển
scene.

### Câu 9. Khi chuyển scene, player được đặt lại vị trí như thế nào?

**Trả lời mẫu:**

Sau khi scene load xong, WorldSceneManager hoặc session manager tìm điểm spawn phù hợp, thường là entry Site of Grace
hoặc saved position. Player transform được đặt lại, camera bám theo và dữ liệu session/save được khôi phục.

**Khi mở code:**

Mở `GameProgressionManager.RegisterBossDefeat(), WorldSceneManager.LoadWorldScene()`.

**Cách giải thích trên code:**

Nói bossID xác định map vừa hoàn thành, manager unlock map kế tiếp rồi trả scene build index để chuyển
scene.

### Câu 10. Nếu scene không có trong Build Settings thì chuyện gì xảy ra?

**Trả lời mẫu:**

Nếu scene không có trong Build Settings, Unity không thể load bằng build index hoặc scene name trong bản build.
Game sẽ báo lỗi hoặc đứng ở scene cũ. Khi debug, em kiểm tra Build Settings và đối chiếu index
với code progression.

**Khi mở code:**

Mở `GameProgressionManager.RegisterBossDefeat(), WorldSceneManager.LoadWorldScene()`.

**Cách giải thích trên code:**

Nói bossID xác định map vừa hoàn thành, manager unlock map kế tiếp rồi trả scene build index để chuyển
scene.

**Code tiêu biểu để chỉ khi bảo vệ:**

Nguồn: `Assets/Game/Scripts/World Managers/GameProgressionManager.cs`.

```csharp
public bool RegisterBossDefeat(int bossID,
    out int nextSceneBuildIndex,
    out int unlockedMapIndex,
    out bool hasWonGame)
{
    nextSceneBuildIndex = -1;
    unlockedMapIndex = -1;
    hasWonGame = false;

    int defeatedMapIndex = GetMapIndexForBossID(bossID);
    UnlockMap(defeatedMapIndex);

    if (HaveAllConfiguredBossesBeenDefeated())
    {
        gameWon = true;
        hasWonGame = true;
        return false;
    }

    unlockedMapIndex = defeatedMapIndex + 1;
    UnlockMap(unlockedMapIndex);
    currentMapIndex = unlockedMapIndex;
    pendingTransitionSiteOfGraceID = GetEntrySiteOfGraceIDForMap(currentMapIndex);
    nextSceneBuildIndex = GetSceneBuildIndexForMap(currentMapIndex);
    return nextSceneBuildIndex != SceneManager.GetActiveScene().buildIndex;
}
```

**Giải thích code:**

Hàm nhận bossID, suy ra map vừa hoàn thành, unlock map hiện tại và map kế tiếp. Nếu hết boss
thì bật trạng thái thắng game.
## 10. Save/Load

### Câu 1. Dữ liệu save game gồm những gì?

**Trả lời mẫu:**

Save game gồm dữ liệu nhân vật, vị trí, scene, stat, HP/stamina/FP, rune, inventory, equipment, weapon upgrade, spell/item, Site of
Grace đã kích hoạt, item world đã nhặt, boss đã chết và progression map.

**Khi mở code:**

Mở `WorldSaveGameManager.SaveGame(), WorldSaveGameManager.LoadGame(), CharacterSaveData`.

**Cách giải thích trên code:**

Giải thích save gom runtime data thành CharacterSaveData, load đọc file rồi khôi phục scene, stat, inventory và progression.

### Câu 2. CharacterSaveData lưu những nhóm dữ liệu nào?

**Trả lời mẫu:**

CharacterSaveData lưu dữ liệu có thể serialize của nhân vật như tên, class, level, stat, tài nguyên hiện tại, vị
trí, scene, rune, inventory, equipment và progression liên quan. Nó là object trung gian, thay vì lưu trực tiếp MonoBehaviour
runtime.

**Khi mở code:**

Mở `WorldSaveGameManager.SaveGame(), WorldSaveGameManager.LoadGame(), CharacterSaveData`.

**Cách giải thích trên code:**

Giải thích save gom runtime data thành CharacterSaveData, load đọc file rồi khôi phục scene, stat, inventory và progression.

### Câu 3. Save file được ghi ở đâu?

**Trả lời mẫu:**

Save file được ghi vào persistentDataPath hoặc thư mục save do SaveFileDataWriter quản lý. Đường dẫn này phù hợp vì
người chơi có quyền ghi và dữ liệu không phụ thuộc scene. File thường tách theo character slot hoặc save
slot.

**Khi mở code:**

Mở `WorldSaveGameManager.SaveGame(), WorldSaveGameManager.LoadGame(), CharacterSaveData`.

**Cách giải thích trên code:**

Giải thích save gom runtime data thành CharacterSaveData, load đọc file rồi khôi phục scene, stat, inventory và progression.

### Câu 4. Vì sao cần SaveFileDataWriter?

**Trả lời mẫu:**

SaveFileDataWriter tách thao tác file khỏi gameplay. WorldSaveGameManager chuẩn bị dữ liệu, còn writer tạo đường dẫn, kiểm tra file,
ghi và đọc dữ liệu. Nhờ vậy sau này đổi định dạng save sẽ ít ảnh hưởng logic gameplay.

**Khi mở code:**

Mở `WorldSaveGameManager.SaveGame(), WorldSaveGameManager.LoadGame(), CharacterSaveData`.

**Cách giải thích trên code:**

Giải thích save gom runtime data thành CharacterSaveData, load đọc file rồi khôi phục scene, stat, inventory và progression.

### Câu 5. Khi save game, dữ liệu player được lấy từ đâu?

**Trả lời mẫu:**

Khi save, WorldSaveGameManager lấy dữ liệu từ PlayerManager và các manager con: network, stats, inventory, equipment. Dữ liệu world như
boss defeated, Site of Grace và item đã nhặt cũng được lấy từ manager/interactable tương ứng.

**Khi mở code:**

Mở `WorldSaveGameManager.SaveGame(), WorldSaveGameManager.LoadGame(), CharacterSaveData`.

**Cách giải thích trên code:**

Giải thích save gom runtime data thành CharacterSaveData, load đọc file rồi khôi phục scene, stat, inventory và progression.

### Câu 6. Khi load game, dữ liệu được nạp lại vào player như thế nào?

**Trả lời mẫu:**

Khi load, WorldSaveGameManager đọc CharacterSaveData rồi gán lại stat, HP, stamina, FP, rune, inventory, equipment, scene và vị trí cho
player. Với item/vũ khí, hệ thống dùng ID tra database để dựng lại object runtime.

**Khi mở code:**

Mở `WorldSaveGameManager.SaveGame(), WorldSaveGameManager.LoadGame(), CharacterSaveData`.

**Cách giải thích trên code:**

Giải thích save gom runtime data thành CharacterSaveData, load đọc file rồi khôi phục scene, stat, inventory và progression.

### Câu 7. Dữ liệu boss đã chết được lưu ra sao?

**Trả lời mẫu:**

Mỗi boss có ID. Khi boss chết, ID được thêm vào danh sách defeated bosses trong save data. Khi scene
load, boss kiểm tra ID của mình; nếu đã defeated thì không spawn, bị disable hoặc đặt trạng thái đã
chết.

**Khi mở code:**

Mở `WorldSaveGameManager.SaveGame(), WorldSaveGameManager.LoadGame(), CharacterSaveData`.

**Cách giải thích trên code:**

Giải thích save gom runtime data thành CharacterSaveData, load đọc file rồi khôi phục scene, stat, inventory và progression.

### Câu 8. Dữ liệu item đã nhặt được lưu như thế nào?

**Trả lời mẫu:**

World item cần ID duy nhất. Khi player nhặt, ID được thêm vào danh sách picked up items. Khi load
scene, item kiểm tra danh sách này; nếu ID đã tồn tại, object bị ẩn hoặc không spawn nữa.

**Khi mở code:**

Mở `WorldSaveGameManager.SaveGame(), WorldSaveGameManager.LoadGame(), CharacterSaveData`.

**Cách giải thích trên code:**

Giải thích save gom runtime data thành CharacterSaveData, load đọc file rồi khôi phục scene, stat, inventory và progression.

### Câu 9. Dữ liệu Site of Grace được lưu như thế nào?

**Trả lời mẫu:**

Mỗi Site of Grace có ID. Khi kích hoạt, ID được lưu vào activated sites. Last Site cũng được lưu
để hồi sinh hoặc teleport. Khi load, Site đọc save data để hiển thị trạng thái đã kích hoạt.

**Khi mở code:**

Mở `WorldSaveGameManager.SaveGame(), WorldSaveGameManager.LoadGame(), CharacterSaveData`.

**Cách giải thích trên code:**

Giải thích save gom runtime data thành CharacterSaveData, load đọc file rồi khôi phục scene, stat, inventory và progression.

### Câu 10. Nếu save file bị thiếu trường dữ liệu mới, hệ thống xử lý ra sao?

**Trả lời mẫu:**

Nếu save file cũ thiếu trường mới, code load cần dùng giá trị mặc định và kiểm tra null. Cách
tốt hơn là version save data và migrate dữ liệu cũ. Như vậy cập nhật game không làm hỏng save
của người chơi.

**Khi mở code:**

Mở `CharacterSaveData.EnsureCollectionsInitialized(), WorldSaveGameManager.LoadGame()`.

**Cách giải thích trên code:**

Giải thích load phải khởi tạo collection mặc định và kiểm tra null để save cũ vẫn dùng được sau khi thêm field mới.

**Code tiêu biểu để chỉ khi bảo vệ:**

Nguồn: `Assets/Game/Scripts/World Managers/WorldSaveGameManager.cs`.

```csharp
public void LoadGame()
{
    saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(currentCharacterSlotBeingUsed);
    saveFileDataWriter = new SaveFileDataWriter();
    saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
    saveFileDataWriter.saveFilename = saveFileName;
    currentCharacterData = saveFileDataWriter.LoadSaveFile();

    GameProgressionManager.Instance.LoadFromCharacterData(currentCharacterData);
    WorldSceneManager.instance.LoadWorldScene(currentCharacterData.sceneIndex);
}

public void SaveGame()
{
    saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(currentCharacterSlotBeingUsed);
    saveFileDataWriter = new SaveFileDataWriter();
    saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
    saveFileDataWriter.saveFilename = saveFileName;

    player.SaveGameDataToCurrentCharacterData(ref currentCharacterData);
    GameProgressionManager.Instance.SaveToCharacterData(currentCharacterData);
    saveFileDataWriter.CreateNewCharacterSaveFile(currentCharacterData);
}
```

**Giải thích code:**

SaveGame gom dữ liệu player và progression rồi ghi file. LoadGame đọc file, nạp progression, sau đó load scene đã
lưu.
## 11. Inventory, item và loot

### Câu 1. Inventory của người chơi lưu item như thế nào?

**Trả lời mẫu:**

Inventory lưu item theo loại như weapon, armor, consumable, spell, material hoặc key item. Mỗi entry có item ID, quantity,
upgrade level và dữ liệu phụ. Runtime dùng Item object để hiển thị/sử dụng, còn save dùng ID và quantity.

**Khi mở code:**

Mở `PickUpItemInteractable.Interact(), CompletePickupOnServer(), GrantPickedUpItemClientRpc()`.

**Cách giải thích trên code:**

Giải thích pickup đi qua server để tránh nhiều người nhặt cùng một item.

### Câu 2. Khi nhặt item, luồng xử lý từ interact đến inventory ra sao?

**Trả lời mẫu:**

Player tương tác với PickUpItemInteractable. Interactable kiểm tra item còn tồn tại, gửi yêu cầu lên server nếu multiplayer, thêm
item vào inventory, hiển thị popup, đánh dấu item đã nhặt và disable object trong world.

**Khi mở code:**

Mở `PickUpItemInteractable.Interact(), CompletePickupOnServer(), GrantPickedUpItemClientRpc()`.

**Cách giải thích trên code:**

Giải thích pickup đi qua server để tránh nhiều người nhặt cùng một item.

### Câu 3. Vì sao pickup cần xử lý trên server?

**Trả lời mẫu:**

Pickup cần server xử lý vì nó ảnh hưởng tài nguyên thật và trạng thái world. Nếu client tự quyết
định, nhiều người có thể nhặt cùng một item. Server đảm bảo item chỉ cấp một lần và biến mất
đồng bộ.

**Khi mở code:**

Mở `PickUpItemInteractable.Interact(), CompletePickupOnServer(), GrantPickedUpItemClientRpc()`.

**Cách giải thích trên code:**

Giải thích pickup đi qua server để tránh nhiều người nhặt cùng một item.

### Câu 4. World spawn item và creature drop item khác nhau thế nào?

**Trả lời mẫu:**

World spawn item đặt sẵn trong map, có ID cố định và cần lưu đã nhặt. Creature drop item sinh
ra khi enemy chết, thường theo loot table và có thể không cần lưu lâu dài. Hai loại khác nhau
ở nguồn sinh và persistence.

**Khi mở code:**

Mở `PickUpItemInteractable.Interact(), CompletePickupOnServer(), GrantPickedUpItemClientRpc()`.

**Cách giải thích trên code:**

Giải thích pickup đi qua server để tránh nhiều người nhặt cùng một item.

### Câu 5. Làm sao game tránh việc nhặt lại item world spawn đã nhặt?

**Trả lời mẫu:**

Game tránh nhặt lại world item bằng ID duy nhất. Khi nhặt, ID được lưu vào save. Lúc scene load,
item kiểm tra danh sách picked up; nếu ID đã có thì object bị tắt, nên player không thể lấy
lại vật phẩm cố định.

**Khi mở code:**

Mở `PickUpItemInteractable.Interact(), CompletePickupOnServer(), GrantPickedUpItemClientRpc()`.

**Cách giải thích trên code:**

Giải thích pickup đi qua server để tránh nhiều người nhặt cùng một item.

### Câu 6. Rune hoạt động như tiền tệ như thế nào?

**Trả lời mẫu:**

Rune là tiền tệ và kinh nghiệm. Player nhận rune khi giết enemy, boss hoặc bán item. Rune dùng để
mua đồ và level up. Vì rune nằm trong player data/network data, shop, UI và level up cùng đọc một
nguồn.

**Khi mở code:**

Mở `PlayerStatsManager.runes, PickUpRunesInteractable, PlayerShopManager`.

**Cách giải thích trên code:**

Giải thích rune là tiền tệ dùng chung cho loot, shop và level up.

### Câu 7. Khi player chết, rune/dead spot được xử lý ra sao?

**Trả lời mẫu:**

Khi player chết, game có thể lưu rune hiện tại vào dead spot tại vị trí chết và đặt rune
về 0. Nếu player quay lại nhặt, rune được trả. Nếu chết lần nữa trước khi nhặt, dead spot cũ
có thể bị thay thế.

**Khi mở code:**

Mở `PlayerStatsManager.runes, PickUpRunesInteractable, PlayerShopManager`.

**Cách giải thích trên code:**

Giải thích rune là tiền tệ dùng chung cho loot, shop và level up.

### Câu 8. Stackable item được xử lý thế nào?

**Trả lời mẫu:**

Stackable item dùng cùng item ID và tăng quantity thay vì tạo nhiều entry. Khi nhặt thêm, inventory cộng số
lượng. Khi dùng hoặc bán, quantity giảm; nếu về 0 thì xóa entry. Save data ghi cả ID và quantity.

**Khi mở code:**

Mở `PickUpItemInteractable.Interact(), CompletePickupOnServer(), GrantPickedUpItemClientRpc()`.

**Cách giải thích trên code:**

Giải thích pickup đi qua server để tránh nhiều người nhặt cùng một item.

### Câu 9. Nếu nhiều người chơi cùng nhặt một item thì hệ thống xử lý ra sao?

**Trả lời mẫu:**

Nếu nhiều người cùng nhặt một item, server quyết định request nào hợp lệ trước. Request đầu tiên nhận item
và đánh dấu picked up. Request sau bị từ chối vì item đã mất. ClientRpc tắt item trên mọi client.

**Khi mở code:**

Mở `PickUpItemInteractable.Interact(), CompletePickupOnServer(), GrantPickedUpItemClientRpc()`.

**Cách giải thích trên code:**

Giải thích pickup đi qua server để tránh nhiều người nhặt cùng một item.

**Code tiêu biểu để chỉ khi bảo vệ:**

Nguồn: `Assets/Game/Scripts/Function/PickUpItemInteractable.cs`.

```csharp
public override void Interact(PlayerManager player)
{
    if (!CanBeLootedBy(player))
        return;

    if (player.isPerformingAction)
        return;

    if (player.playerCombatManager.isUsingItem)
        return;

    player.playerInteractionManager.RemoveInteractionFromList(this);
    PlayerUIManager.instance.playerUIPopUpManager.CloseAllPopUpWindows();

    if (IsServer)
    {
        CompletePickupOnServer(player.OwnerClientId);
    }
    else
    {
        RequestPickupServerRpc();
    }
}
```

**Giải thích code:**

Interact kiểm tra item còn nhặt được không và player không bận hành động. Server hoàn tất pickup, client thì
gửi ServerRpc.
## 12. Site of Grace

### Câu 1. Site of Grace có những chức năng gì?

**Trả lời mẫu:**

Site of Grace là checkpoint chính: kích hoạt điểm nghỉ, hồi HP/stamina/FP, reset AI nếu thiết kế, lưu game, làm
điểm hồi sinh, mở menu level up/upgrade/teleport và liên kết progression/chuyển map.

**Khi mở code:**

Mở `SiteOfGraceInteractable.isActivated, RestoreSiteOfGrace(), RestAtSiteOfGrace()`.

**Cách giải thích trên code:**

Giải thích NetworkVariable đồng bộ trạng thái kích hoạt, còn rest hồi phục/reset AI/lưu checkpoint.

### Câu 2. Tương tác Site of Grace lần đầu và các lần sau khác nhau thế nào?

**Trả lời mẫu:**

Lần đầu tương tác, Site được activate, lưu ID vào danh sách đã kích hoạt và phát hiệu ứng mở
khóa. Các lần sau, player rest để hồi phục, mở menu hoặc teleport. Vì vậy lần đầu là unlock, lần
sau là sử dụng.

**Khi mở code:**

Mở `SiteOfGraceInteractable.isActivated, RestoreSiteOfGrace(), RestAtSiteOfGrace()`.

**Cách giải thích trên code:**

Giải thích NetworkVariable đồng bộ trạng thái kích hoạt, còn rest hồi phục/reset AI/lưu checkpoint.

### Câu 3. Khi rest, game hồi phục những gì?

**Trả lời mẫu:**

Khi rest, game hồi HP, stamina, FP, reset một số status xấu nếu có, hồi bình máu/tài nguyên tiêu hao
và lưu game. Rest cũng có thể respawn enemy thường và cập nhật last Site of Grace.

**Khi mở code:**

Mở `SiteOfGraceInteractable.isActivated, RestoreSiteOfGrace(), RestAtSiteOfGrace()`.

**Cách giải thích trên code:**

Giải thích NetworkVariable đồng bộ trạng thái kích hoạt, còn rest hồi phục/reset AI/lưu checkpoint.

### Câu 4. Vì sao rest tại Site of Grace cần reset AI?

**Trả lời mẫu:**

Rest cần reset AI để thế giới trở lại trạng thái ổn định. Enemy đang đuổi player phải dừng, hồi
vị trí hoặc respawn. Nếu không, player có thể mở menu/rest trong khi enemy vẫn tấn công, gây lỗi gameplay.

**Khi mở code:**

Mở `SiteOfGraceInteractable.isActivated, RestoreSiteOfGrace(), RestAtSiteOfGrace()`.

**Cách giải thích trên code:**

Giải thích NetworkVariable đồng bộ trạng thái kích hoạt, còn rest hồi phục/reset AI/lưu checkpoint.

### Câu 5. Last Site of Grace được lưu để làm gì?

**Trả lời mẫu:**

Last Site of Grace là checkpoint gần nhất. Khi player chết hoặc load game, hệ thống có thể đưa player
về đây thay vì vị trí chết nguy hiểm. Nó cũng dùng cho teleport và xác định điểm bắt đầu
an toàn.

**Khi mở code:**

Mở `SiteOfGraceInteractable.isActivated, RestoreSiteOfGrace(), RestAtSiteOfGrace()`.

**Cách giải thích trên code:**

Giải thích NetworkVariable đồng bộ trạng thái kích hoạt, còn rest hồi phục/reset AI/lưu checkpoint.

### Câu 6. Teleport đến Site of Grace được xử lý thế nào?

**Trả lời mẫu:**

Teleport lấy danh sách Site đã kích hoạt, cho player chọn điểm đến, rồi chuyển scene nếu khác map hoặc
đặt lại vị trí nếu cùng scene. Site ID giúp tìm đúng spawn. Sau teleport, camera và player state được
reset.

**Khi mở code:**

Mở `SiteOfGraceInteractable.isActivated, RestoreSiteOfGrace(), RestAtSiteOfGrace()`.

**Cách giải thích trên code:**

Giải thích NetworkVariable đồng bộ trạng thái kích hoạt, còn rest hồi phục/reset AI/lưu checkpoint.

### Câu 7. Site of Grace liên quan thế nào đến map transition?

**Trả lời mẫu:**

Site of Grace liên quan map transition vì entry Site thường là điểm spawn của map mới. Khi map được
unlock hoặc load lại, hệ thống dựa vào Site để đặt nhân vật ở vị trí hợp lý và ổn
định.

**Khi mở code:**

Mở `SiteOfGraceInteractable.isActivated, RestoreSiteOfGrace(), RestAtSiteOfGrace()`.

**Cách giải thích trên code:**

Giải thích NetworkVariable đồng bộ trạng thái kích hoạt, còn rest hồi phục/reset AI/lưu checkpoint.

### Câu 8. Vì sao Site of Grace cần dùng NetworkVariable isActivated?

**Trả lời mẫu:**

isActivated cần là NetworkVariable để trạng thái Site giống nhau trên mọi client. Server đổi giá trị, client tự nhận
cập nhật. Nhờ vậy hiệu ứng, UI và khả năng rest không bị lệch giữa host và client.

**Khi mở code:**

Mở `SiteOfGraceInteractable.isActivated, RestoreSiteOfGrace(), RestAtSiteOfGrace()`.

**Cách giải thích trên code:**

Giải thích NetworkVariable đồng bộ trạng thái kích hoạt, còn rest hồi phục/reset AI/lưu checkpoint.

**Code tiêu biểu để chỉ khi bảo vệ:**

Nguồn: `Assets/Game/Scripts/Function/SiteOfGraceInteractable.cs`.

```csharp
public NetworkVariable<bool> isActivated = new NetworkVariable<bool>
    (false,
    NetworkVariableReadPermission.Everyone,
    NetworkVariableWritePermission.Server);

private void RestoreSiteOfGrace(PlayerManager player)
{
    isActivated.Value = true;

    WorldSaveGameManager.instance.currentCharacterData.sitesOfGrace[siteOfGraceID] = true;
    WorldSaveGameManager.instance.SaveGame();

    CompleteGraceActivationLocally(player);
}

private void RestAtSiteOfGrace(PlayerManager player)
{
    if (WorldAIManager.instance != null)
        WorldAIManager.instance.ResetAllCharacters();

    CompleteRestAtSiteOfGraceLocally(player);
}
```

**Giải thích code:**

isActivated do server ghi và mọi client đọc. Activate thì lưu vào save. Rest thì reset AI và chạy hồi
phục/checkpoint.
## 13. Level up và nâng cấp vũ khí

### Câu 1. Level nhân vật được tính như thế nào?

**Trả lời mẫu:**

Level nhân vật được tính từ currentLevel hoặc tổng điểm stat đã tăng. Khi player cộng một stat, level tăng
thêm một bậc và chi phí lần sau tăng. Level biểu thị sức mạnh tổng quát và liên quan progression
nhân vật.

**Khi mở code:**

Mở `PlayerUILevelUpManager.ConfirmLevels(), PlayerNetworkManager.vigor/mind/endurance`.

**Cách giải thích trên code:**

Giải thích confirm trừ rune, ghi stat mới vào NetworkVariable và gọi SaveGame.

### Câu 2. Chi phí level up được tính ra sao?

**Trả lời mẫu:**

Chi phí level up dựa trên level hiện tại và tăng dần theo công thức. UI tính rune cần dùng
trước khi confirm. Nếu đủ rune, stat được cộng; nếu thiếu, UI báo lỗi và dữ liệu player không đổi.

**Khi mở code:**

Mở `PlayerUILevelUpManager.ConfirmLevels(), PlayerNetworkManager.vigor/mind/endurance`.

**Cách giải thích trên code:**

Giải thích confirm trừ rune, ghi stat mới vào NetworkVariable và gọi SaveGame.

### Câu 3. Khi confirm level up, stat nào được cập nhật?

**Trả lời mẫu:**

Khi confirm, các stat đã cộng tạm trên UI được ghi vào dữ liệu thật. Rune bị trừ theo tổng
chi phí, current level tăng và các giá trị dẫn xuất như max HP, stamina, FP được tính lại.

**Khi mở code:**

Mở `PlayerUILevelUpManager.ConfirmLevels(), PlayerNetworkManager.vigor/mind/endurance`.

**Cách giải thích trên code:**

Giải thích confirm trừ rune, ghi stat mới vào NetworkVariable và gọi SaveGame.

### Câu 4. Vì sao cần cập nhật lại max HP/stamina/FP sau khi tăng stat?

**Trả lời mẫu:**

Cần cập nhật max HP/stamina/FP vì chúng phụ thuộc stat nền. Nếu tăng Vigor nhưng không tính lại HP, người
chơi không nhận lợi ích thật. Sau stat đổi, hệ thống cập nhật max value, current value và HUD.

**Khi mở code:**

Mở `PlayerUILevelUpManager.ConfirmLevels(), PlayerNetworkManager.vigor/mind/endurance`.

**Cách giải thích trên code:**

Giải thích confirm trừ rune, ghi stat mới vào NetworkVariable và gọi SaveGame.

### Câu 5. Weapon upgrade cần điều kiện gì?

**Trả lời mẫu:**

Weapon upgrade cần vũ khí hợp lệ, chưa đạt cấp tối đa, đủ rune và đủ nguyên liệu nâng cấp.
Nếu thiếu điều kiện, UI báo lỗi và không trừ tài nguyên. Nếu đủ, upgrade level tăng và damage được
tính lại.

**Khi mở code:**

Mở `PlayerUIWeaponUpgradeManager, PlayerInventoryManager, WorldSaveGameManager.SaveGame()`.

**Cách giải thích trên code:**

Giải thích upgrade kiểm tra nguyên liệu/rune, tăng level vũ khí, tính damage mới và lưu lại.

### Câu 6. Nguyên liệu nâng cấp được kiểm tra ở đâu?

**Trả lời mẫu:**

Nguyên liệu được kiểm tra trong inventory hoặc manager nâng cấp, không chỉ ở UI. Khi confirm, logic phải kiểm
tra lại rune/material để tránh trường hợp UI cũ hoặc inventory đã thay đổi trước khi bấm nâng cấp.

**Khi mở code:**

Mở `PlayerUILevelUpManager.ConfirmLevels(), PlayerNetworkManager.vigor/mind/endurance`.

**Cách giải thích trên code:**

Giải thích confirm trừ rune, ghi stat mới vào NetworkVariable và gọi SaveGame.

### Câu 7. Sau khi nâng cấp, damage vũ khí cập nhật thế nào?

**Trả lời mẫu:**

Sau nâng cấp, WeaponItem tính lại damage theo upgrade level. Nếu vũ khí đang trang bị, equipment/combat manager cần cập
nhật để hit tiếp theo dùng damage mới. Save data cũng phải ghi upgrade level.

**Khi mở code:**

Mở `PlayerUIWeaponUpgradeManager, PlayerInventoryManager, WorldSaveGameManager.SaveGame()`.

**Cách giải thích trên code:**

Giải thích upgrade kiểm tra nguyên liệu/rune, tăng level vũ khí, tính damage mới và lưu lại.

### Câu 8. Vì sao weapon upgrade cần sync bằng RPC?

**Trả lời mẫu:**

Upgrade cần sync bằng RPC vì nó thay đổi sức mạnh thật trong multiplayer. Client gửi yêu cầu, server kiểm
tra điều kiện, cập nhật dữ liệu và đồng bộ lại. Nếu chỉ client tự tăng, damage giữa các máy
có thể lệch.

**Khi mở code:**

Mở `PlayerUIWeaponUpgradeManager, PlayerInventoryManager, WorldSaveGameManager.SaveGame()`.

**Cách giải thích trên code:**

Giải thích upgrade kiểm tra nguyên liệu/rune, tăng level vũ khí, tính damage mới và lưu lại.

### Câu 9. Làm sao save trạng thái upgrade level của weapon?

**Trả lời mẫu:**

Để save upgrade, save data lưu weapon ID kèm upgrade level. Khi load, WorldSaveGameManager dùng ID tìm WeaponItem gốc, tạo
dữ liệu runtime và gán lại cấp nâng cấp. Nhờ vậy vũ khí giữ đúng sức mạnh sau khi load.

**Khi mở code:**

Mở `PlayerUIWeaponUpgradeManager, PlayerInventoryManager, WorldSaveGameManager.SaveGame()`.

**Cách giải thích trên code:**

Giải thích upgrade kiểm tra nguyên liệu/rune, tăng level vũ khí, tính damage mới và lưu lại.

**Code tiêu biểu để chỉ khi bảo vệ:**

Nguồn: `Assets/Game/Scripts/Character/Player/PlayerUI/PlayerUILevelUpManager.cs`.

```csharp
public void ConfirmLevels()
{
    PlayerManager player = PlayerUIManager.instance.localPlayer;

    player.playerStatsManager.runes -= totalLevelUpCost;

    player.playerNetworkManager.vigor.Value = Mathf.RoundToInt(vigorSlider.value);
    player.playerNetworkManager.mind.Value = Mathf.RoundToInt(mindSlider.value);
    player.playerNetworkManager.endurance.Value = Mathf.RoundToInt(enduranceSlider.value);
    player.playerNetworkManager.strength.Value = Mathf.RoundToInt(strengthSlider.value);
    player.playerNetworkManager.dexterity.Value = Mathf.RoundToInt(dexteritySlider.value);
    player.playerNetworkManager.intelligence.Value = Mathf.RoundToInt(intelligenceSlider.value);
    player.playerNetworkManager.faith.Value = Mathf.RoundToInt(faithSlider.value);

    SetCurrentStats();
    ChangeTextColorsDependingOnCosts();
    WorldSaveGameManager.instance.SaveGame();
}
```

**Giải thích code:**

Confirm trừ rune, ghi stat mới vào NetworkVariable, cập nhật UI dự đoán và lưu game. Các max value đổi
theo callback stat.
## 14. Shop

### Câu 1. Shop system gồm những class nào?

**Trả lời mẫu:**

Shop system gồm ShopInteractable, ShopInventory, PlayerShopManager và PlayerUIShopManager. Ngoài ra inventory, rune, save game và progression cũng tham gia vì
mua bán ảnh hưởng dữ liệu người chơi và stock của cửa hàng.

**Khi mở code:**

Mở `ShopInteractable, ShopInventory, PlayerShopManager.TryBuyItem(), PlayerShopManager.TrySellItem()`.

**Cách giải thích trên code:**

Giải thích UI chỉ chọn món, còn PlayerShopManager mới kiểm tra rune, stock, inventory và lưu giao dịch.

### Câu 2. ShopInteractable, ShopInventory, PlayerShopManager, PlayerUIShopManager khác nhau thế nào?

**Trả lời mẫu:**

ShopInteractable là điểm tương tác trong scene. ShopInventory chứa hàng hóa và giá. PlayerShopManager kiểm tra rune, inventory, stock và
thực hiện giao dịch. PlayerUIShopManager chỉ hiển thị danh sách, nút mua/bán và thông báo.

**Khi mở code:**

Mở `ShopInteractable, ShopInventory, PlayerShopManager.TryBuyItem(), PlayerShopManager.TrySellItem()`.

**Cách giải thích trên code:**

Giải thích UI chỉ chọn món, còn PlayerShopManager mới kiểm tra rune, stock, inventory và lưu giao dịch.

### Câu 3. Khi mua item, rune bị trừ ở đâu?

**Trả lời mẫu:**

Rune bị trừ trong PlayerShopManager hoặc transaction logic sau khi kiểm tra đủ điều kiện. UI không trực tiếp trừ
rune. Sau mua thành công, manager giảm rune, thêm item, giảm stock nếu có và refresh UI.

**Khi mở code:**

Mở `ShopInteractable, ShopInventory, PlayerShopManager.TryBuyItem(), PlayerShopManager.TrySellItem()`.

**Cách giải thích trên code:**

Giải thích UI chỉ chọn món, còn PlayerShopManager mới kiểm tra rune, stock, inventory và lưu giao dịch.

### Câu 4. Khi bán item, item bị xóa ở đâu?

**Trả lời mẫu:**

Khi bán, PlayerShopManager kiểm tra item có thể bán và số lượng hợp lệ. InventoryManager giảm quantity hoặc xóa entry,
rune tăng theo giá bán, rồi UI refresh. Shop manager điều phối, inventory manager sửa dữ liệu item.

**Khi mở code:**

Mở `ShopInteractable, ShopInventory, PlayerShopManager.TryBuyItem(), PlayerShopManager.TrySellItem()`.

**Cách giải thích trên code:**

Giải thích UI chỉ chọn món, còn PlayerShopManager mới kiểm tra rune, stock, inventory và lưu giao dịch.

### Câu 5. Limited stock được lưu như thế nào?

**Trả lời mẫu:**

Limited stock cần lưu số lượng còn lại theo shop ID và item ID. Khi mua, stock giảm và ghi
vào save. Khi load, ShopInventory đọc stock đã lưu để hàng giới hạn không reset về ban đầu.

**Khi mở code:**

Mở `ShopInteractable, ShopInventory, PlayerShopManager.TryBuyItem(), PlayerShopManager.TrySellItem()`.

**Cách giải thích trên code:**

Giải thích UI chỉ chọn món, còn PlayerShopManager mới kiểm tra rune, stock, inventory và lưu giao dịch.

### Câu 6. Giá shop thay đổi theo progression như thế nào?

**Trả lời mẫu:**

Giá shop có thể thay đổi theo map tier, boss đã đánh bại hoặc current progression. Shop lấy tier từ
GameProgressionManager rồi áp modifier giá. Map sau có thể bán đồ mạnh hơn và đắt hơn.

**Khi mở code:**

Mở `ShopInteractable, ShopInventory, PlayerShopManager.TryBuyItem(), PlayerShopManager.TrySellItem()`.

**Cách giải thích trên code:**

Giải thích UI chỉ chọn món, còn PlayerShopManager mới kiểm tra rune, stock, inventory và lưu giao dịch.

### Câu 7. Làm sao shop biết người chơi đang ở tier/map nào?

**Trả lời mẫu:**

Shop biết tier bằng current scene, shop ID hoặc dữ liệu progression. Khi mở shop, PlayerShopManager đọc GameProgressionManager để lọc
hàng và tính giá phù hợp với map hiện tại.

**Khi mở code:**

Mở `ShopInteractable, ShopInventory, PlayerShopManager.TryBuyItem(), PlayerShopManager.TrySellItem()`.

**Cách giải thích trên code:**

Giải thích UI chỉ chọn món, còn PlayerShopManager mới kiểm tra rune, stock, inventory và lưu giao dịch.

### Câu 8. Nếu người chơi không đủ rune thì UI xử lý ra sao?

**Trả lời mẫu:**

Nếu không đủ rune, manager từ chối giao dịch, không thêm item và không giảm stock. UI hiển thị thông
báo thiếu rune hoặc disable nút mua. Validation quan trọng phải nằm ở logic, không chỉ ở giao diện.

**Khi mở code:**

Mở `ShopInteractable, ShopInventory, PlayerShopManager.TryBuyItem(), PlayerShopManager.TrySellItem()`.

**Cách giải thích trên code:**

Giải thích UI chỉ chọn món, còn PlayerShopManager mới kiểm tra rune, stock, inventory và lưu giao dịch.

### Câu 9. Vì sao transaction cần save game?

**Trả lời mẫu:**

Transaction cần save vì nó thay đổi dữ liệu bền vững: rune, item và stock shop. Nếu không save, player
có thể mua xong thoát game rồi mất item hoặc stock reset. Vì vậy giao dịch quan trọng cần lưu
hoặc đánh dấu dirty.

**Khi mở code:**

Mở `ShopInteractable, ShopInventory, PlayerShopManager.TryBuyItem(), PlayerShopManager.TrySellItem()`.

**Cách giải thích trên code:**

Giải thích UI chỉ chọn món, còn PlayerShopManager mới kiểm tra rune, stock, inventory và lưu giao dịch.

**Code tiêu biểu để chỉ khi bảo vệ:**

Nguồn: `Assets/Game/Scripts/Character/Player/PlayerShopManager.cs`.

```csharp
public bool TryBuyItem(ShopStockEntry entry, ShopInventory shopInventory = null)
{
    if (player == null || entry == null || entry.item == null)
        return false;

    int price = shopInventory != null ? shopInventory.GetBuyPrice(entry.item) : entry.GetBuyPrice();

    if (price < 0 || player.playerStatsManager.runes < price)
        return false;

    if (shopInventory != null && !shopInventory.TryPurchaseItem(entry.item))
        return false;

    Item purchasedItem = CreatePurchasedItem(entry.item);
    player.playerInventoryManager.AddItemToInventory(purchasedItem);
    player.playerStatsManager.AddRunes(-price);

    TryAutoSave();
    RefreshOwnedPlayerUI();
    return true;
}
```

**Giải thích code:**

TryBuyItem kiểm tra dữ liệu, giá, rune và stock trước. Mua thành công thì thêm item, trừ rune, save và
refresh UI.
## 15. UI/HUD

### Câu 1. HUD hiển thị những thông tin gì?

**Trả lời mẫu:**

HUD hiển thị HP, stamina, FP, rune, item đang chọn, vũ khí, spell, prompt tương tác, popup item/status và boss
HP bar khi vào boss fight. HUD giúp player nắm tài nguyên và trạng thái combat liên tục.

**Khi mở code:**

Mở `PlayerManager.OnNetworkSpawn(), PlayerUIManager, PlayerUIPopUpManager`.

**Cách giải thích trên code:**

Giải thích UI lắng nghe OnValueChanged từ NetworkVariable, còn popup được gọi từ sự kiện gameplay.

### Câu 2. HP/stamina/FP bar được cập nhật bằng cách nào?

**Trả lời mẫu:**

HP/stamina/FP bar cập nhật bằng callback OnValueChanged từ NetworkVariable hoặc dữ liệu player. Khi giá trị đổi, UI slider nhận
giá trị mới. UI không tự đoán damage hay hồi phục, mà phản ánh dữ liệu thật.

**Khi mở code:**

Mở `PlayerManager.OnNetworkSpawn(), PlayerUIManager, PlayerUIPopUpManager`.

**Cách giải thích trên code:**

Giải thích UI lắng nghe OnValueChanged từ NetworkVariable, còn popup được gọi từ sự kiện gameplay.

### Câu 3. Boss HP bar được tạo khi nào?

**Trả lời mẫu:**

Boss HP bar được tạo hoặc bật khi boss fight bắt đầu. Trigger hoặc WakeBoss truyền boss cho UI, UI
đặt tên và đăng ký theo dõi HP. Khi boss chết, bar bị ẩn hoặc chuyển sang thông báo defeated.

**Khi mở code:**

Mở `PlayerManager.OnNetworkSpawn(), PlayerUIManager, PlayerUIPopUpManager`.

**Cách giải thích trên code:**

Giải thích UI lắng nghe OnValueChanged từ NetworkVariable, còn popup được gọi từ sự kiện gameplay.

### Câu 4. Popup item/status/boss defeated được gọi từ đâu?

**Trả lời mẫu:**

Popup item gọi từ PickUpItemInteractable sau khi cấp item. Popup status đến từ CharacterEffectsManager. Popup boss defeated đến từ boss
death flow. UI manager chỉ hiển thị, còn sự kiện thật nằm ở gameplay system.

**Khi mở code:**

Mở `PlayerManager.OnNetworkSpawn(), PlayerUIManager, PlayerUIPopUpManager`.

**Cách giải thích trên code:**

Giải thích UI lắng nghe OnValueChanged từ NetworkVariable, còn popup được gọi từ sự kiện gameplay.

### Câu 5. PlayerUIManager quản lý menu như thế nào?

**Trả lời mẫu:**

PlayerUIManager giữ trạng thái menu đang mở, bật/tắt panel inventory, equipment, level up, shop hoặc settings. Khi mở menu, nó
khóa gameplay input. Khi đóng menu, nó trả input về gameplay và refresh HUD nếu cần.

**Khi mở code:**

Mở `PlayerManager.OnNetworkSpawn(), PlayerUIManager, PlayerUIPopUpManager`.

**Cách giải thích trên code:**

Giải thích UI lắng nghe OnValueChanged từ NetworkVariable, còn popup được gọi từ sự kiện gameplay.

### Câu 6. Khi mở menu, vì sao phải khóa gameplay input?

**Trả lời mẫu:**

Mở menu phải khóa input để người chơi không vừa chọn UI vừa đánh, né, dùng item hoặc tương tác
trong world. Đây là kiểm soát trạng thái gameplay, giúp tránh lỗi animation và hành động bị chồng.

**Khi mở code:**

Mở `PlayerManager.OnNetworkSpawn(), PlayerUIManager, PlayerUIPopUpManager`.

**Cách giải thích trên code:**

Giải thích UI lắng nghe OnValueChanged từ NetworkVariable, còn popup được gọi từ sự kiện gameplay.

### Câu 7. UI level up, shop, equipment liên kết với player data như thế nào?

**Trả lời mẫu:**

UI level up, shop và equipment đọc dữ liệu từ player manager/inventory/stats để hiển thị. Khi confirm, UI gọi manager
gameplay xử lý thật. Nhờ vậy UI không tự quyết định dữ liệu, chỉ là lớp điều khiển và hiển
thị.

**Khi mở code:**

Mở `PlayerManager.OnNetworkSpawn(), PlayerUIManager, PlayerUIPopUpManager`.

**Cách giải thích trên code:**

Giải thích UI lắng nghe OnValueChanged từ NetworkVariable, còn popup được gọi từ sự kiện gameplay.

## 16. Multiplayer

### Câu 1. Game dùng công nghệ multiplayer nào?

**Trả lời mẫu:**

Game dùng Unity Netcode for GameObjects, kết hợp Unity Transport và có thể dùng Relay. Netcode cung cấp NetworkObject, NetworkBehaviour,
NetworkVariable, ServerRpc và ClientRpc, phù hợp đồ án vì tích hợp trực tiếp với Unity.

**Khi mở code:**

Mở `NetworkVariable, ServerRpc, ClientRpc, IsOwner, IsServer`.

**Cách giải thích trên code:**

Giải thích owner đọc input, server xử lý trạng thái quan trọng, client nhận kết quả đồng bộ.

### Câu 2. Netcode for GameObjects hoạt động theo mô hình gì?

**Trả lời mẫu:**

Netcode hoạt động theo mô hình host/server authority cơ bản. Host vừa là server vừa là client. Client sở hữu
player của mình và gửi yêu cầu lên server. Server cập nhật trạng thái quan trọng rồi đồng bộ lại
cho client.

**Khi mở code:**

Mở `NetworkVariable, ServerRpc, ClientRpc, IsOwner, IsServer`.

**Cách giải thích trên code:**

Giải thích owner đọc input, server xử lý trạng thái quan trọng, client nhận kết quả đồng bộ.

### Câu 3. ServerRpc và ClientRpc khác nhau thế nào?

**Trả lời mẫu:**

ServerRpc là lời gọi từ client lên server, dùng khi client muốn yêu cầu hành động quan trọng. ClientRpc là
lời gọi từ server xuống client, dùng để thông báo kết quả hoặc hiệu ứng. Một bên gửi ý định,
một bên phát kết quả.

**Khi mở code:**

Mở `NetworkVariable, ServerRpc, ClientRpc, IsOwner, IsServer`.

**Cách giải thích trên code:**

Giải thích owner đọc input, server xử lý trạng thái quan trọng, client nhận kết quả đồng bộ.

### Câu 4. NetworkVariable dùng để làm gì?

**Trả lời mẫu:**

NetworkVariable lưu trạng thái cần đồng bộ tự động như HP, stamina, weapon ID, isDead hoặc isActivated. Khi server đổi
giá trị, client nhận cập nhật và OnValueChanged có thể chạy. Nó phù hợp dữ liệu kéo dài.

**Khi mở code:**

Mở `NetworkVariable, ServerRpc, ClientRpc, IsOwner, IsServer`.

**Cách giải thích trên code:**

Giải thích owner đọc input, server xử lý trạng thái quan trọng, client nhận kết quả đồng bộ.

### Câu 5. Khi người chơi tấn công trong multiplayer, action được đồng bộ như thế nào?

**Trả lời mẫu:**

Owner đọc input và phát action cục bộ cho phản hồi nhanh. Với damage, client gửi ServerRpc lên server. Server
xác nhận, cập nhật HP và phát hiệu ứng/animation cho client khác bằng NetworkVariable, ClientRpc hoặc NetworkAnimator.

**Khi mở code:**

Mở `NetworkVariable, ServerRpc, ClientRpc, IsOwner, IsServer`.

**Cách giải thích trên code:**

Giải thích owner đọc input, server xử lý trạng thái quan trọng, client nhận kết quả đồng bộ.

### Câu 6. Khi projectile được bắn, các client khác thấy projectile bằng cách nào?

**Trả lời mẫu:**

Projectile được bắn bằng cách client gửi request lên server. Server spawn NetworkObject projectile hoặc ClientRpc để các client tạo
projectile cùng vị trí, hướng và tốc độ. Damage nên do server xác nhận khi projectile chạm mục tiêu.

**Khi mở code:**

Mở `NetworkVariable, ServerRpc, ClientRpc, IsOwner, IsServer`.

**Cách giải thích trên code:**

Giải thích owner đọc input, server xử lý trạng thái quan trọng, client nhận kết quả đồng bộ.

### Câu 7. Scene transition trong multiplayer được xử lý ra sao?

**Trả lời mẫu:**

Scene transition trong multiplayer nên do host/server điều phối. Khi đủ điều kiện, server dùng NetworkSceneManager hoặc cơ chế load
chung để các client cùng chuyển. Sau load, từng player được đặt tại spawn/entry Site phù hợp.

**Khi mở code:**

Mở `NetworkVariable, ServerRpc, ClientRpc, IsOwner, IsServer`.

**Cách giải thích trên code:**

Giải thích owner đọc input, server xử lý trạng thái quan trọng, client nhận kết quả đồng bộ.

### Câu 8. Host và client khác nhau thế nào trong game của em?

**Trả lời mẫu:**

Host vừa chạy logic server vừa có một player như client. Host có quyền spawn/despawn object, sửa NetworkVariable và xử
lý ServerRpc. Client chỉ điều khiển player của mình, gửi yêu cầu và nhận trạng thái đồng bộ.

**Khi mở code:**

Mở `NetworkVariable, ServerRpc, ClientRpc, IsOwner, IsServer`.

**Cách giải thích trên code:**

Giải thích owner đọc input, server xử lý trạng thái quan trọng, client nhận kết quả đồng bộ.

### Câu 9. Vì sao chỉ owner xử lý input nhưng server xử lý nhiều trạng thái quan trọng?

**Trả lời mẫu:**

Owner xử lý input để mỗi người chỉ điều khiển nhân vật của mình và có phản hồi nhanh. Server
xử lý trạng thái quan trọng như HP, item, boss defeated và progression để tránh gian lận và lệch dữ
liệu.

**Khi mở code:**

Mở `NetworkVariable, ServerRpc, ClientRpc, IsOwner, IsServer`.

**Cách giải thích trên code:**

Giải thích owner đọc input, server xử lý trạng thái quan trọng, client nhận kết quả đồng bộ.

### Câu 10. Nếu latency cao, combat có thể gặp vấn đề gì?

**Trả lời mẫu:**

Latency cao có thể làm hit đăng ký chậm, HP cập nhật trễ, projectile lệch hoặc animation không khớp. Người
chơi có thể thấy đã đánh trúng nhưng server chưa xác nhận. Cải thiện bằng prediction, lag compensation và tối
ưu gói tin.

**Khi mở code:**

Mở `ServerRpc, ClientRpc, NetworkVariable, NetworkAnimator`.

**Cách giải thích trên code:**

Giải thích latency làm request tới server chậm, nên animation có thể thấy trước nhưng HP/damage chỉ chắc chắn khi server đồng bộ.

**Code tiêu biểu để chỉ khi bảo vệ:**

Nguồn: `Assets/Game/Scripts/Settings/GameSettingsManager.cs`.

```csharp
private void LoadSettings()
{
    masterVolume = PlayerPrefs.GetFloat(MasterVolumeKey, 1f);
    musicVolume = PlayerPrefs.GetFloat(MusicVolumeKey, 1f);
    sfxVolume = PlayerPrefs.GetFloat(SfxVolumeKey, 1f);
    cameraSensitivity = PlayerPrefs.GetFloat(CameraSensitivityKey, 1f);
    isFullscreen = PlayerPrefs.GetInt(FullscreenKey, Screen.fullScreen ? 1 : 0) == 1;
}

private void SaveSettings()
{
    PlayerPrefs.SetFloat(MasterVolumeKey, masterVolume);
    PlayerPrefs.SetFloat(MusicVolumeKey, musicVolume);
    PlayerPrefs.SetFloat(SfxVolumeKey, sfxVolume);
    PlayerPrefs.SetFloat(CameraSensitivityKey, cameraSensitivity);
    PlayerPrefs.SetInt(FullscreenKey, isFullscreen ? 1 : 0);
    PlayerPrefs.Save();
}
```

**Giải thích code:**

Settings đọc/ghi PlayerPrefs vì đây là dữ liệu cục bộ của máy, không phải progression gameplay.
## 17. Settings

### Câu 1. Settings menu gồm những tùy chỉnh nào?

**Trả lời mẫu:**

Settings menu gồm âm lượng tổng, music, SFX, camera sensitivity, quality, resolution hoặc fullscreen tùy phần triển khai. Những tùy
chỉnh này ảnh hưởng trải nghiệm người chơi, không ảnh hưởng dữ liệu gameplay như stat, item hay progression.

**Khi mở code:**

Mở `GameSettingsManager, TitleScreenSettingsMenuManager`.

**Cách giải thích trên code:**

Giải thích settings là nhóm dữ liệu cấu hình trải nghiệm, không phải dữ liệu tiến trình nhân vật.

### Câu 2. Vì sao settings dùng PlayerPrefs?

**Trả lời mẫu:**

PlayerPrefs phù hợp cho dữ liệu cài đặt nhỏ và cục bộ như volume, sensitivity hoặc fullscreen. Nó dễ lưu,
dễ đọc và không cần save slot. Tuy nhiên không nên dùng PlayerPrefs cho dữ liệu gameplay quan trọng.

**Khi mở code:**

Mở `GameSettingsManager.LoadSettings(), GameSettingsManager.SaveSettings()`.

**Cách giải thích trên code:**

Chỉ ra PlayerPrefs lưu key-value đơn giản, phù hợp setting máy người chơi.

### Câu 3. Volume được áp vào những thành phần nào?

**Trả lời mẫu:**

Volume được áp vào AudioMixer hoặc các nhóm AudioSource như master, music và SFX. Khi slider đổi, GameSettingsManager cập nhật
giá trị, lưu PlayerPrefs và áp ngay. Nếu dùng AudioMixer, nên chuyển giá trị sang decibel.

**Khi mở code:**

Mở `GameSettingsManager.ApplyAudioAndGameplaySettings(), SaveSettings()`.

**Cách giải thích trên code:**

Giải thích slider thay đổi biến volume, SaveSettings lưu lại, ApplyAudioAndGameplaySettings áp vào hệ âm thanh.

### Câu 4. Camera sensitivity được cập nhật như thế nào?

**Trả lời mẫu:**

Camera sensitivity được đọc từ settings và gán vào camera manager hoặc multiplier của input look. Khi slider đổi, giá
trị mới được lưu và áp ngay. Khi load scene mới, settings đọc PlayerPrefs để giữ cảm giác camera.

**Khi mở code:**

Mở `GameSettingsManager.cameraSensitivity, PlayerCamera`.

**Cách giải thích trên code:**

Nói sensitivity là hệ số nhân cho input xoay camera, nên đổi giá trị sẽ làm camera nhanh hoặc chậm
hơn.

### Câu 5. Settings title menu và in-game menu dùng chung dữ liệu ra sao?

**Trả lời mẫu:**

Title menu và in-game menu dùng chung key PlayerPrefs. Vì vậy chỉnh volume ở title thì vào game vẫn giữ,
và chỉnh trong game cũng ảnh hưởng lần mở sau. GameSettingsManager gom logic đọc, lưu và áp dụng.

**Khi mở code:**

Mở `TitleScreenSettingsMenuManager, GameSettingsManager.LoadSettings()`.

**Cách giải thích trên code:**

Giải thích hai UI khác nhau nhưng cùng đọc ghi một nguồn dữ liệu PlayerPrefs.

### Câu 6. Khi load scene mới, settings có được giữ không? Vì sao?

**Trả lời mẫu:**

Settings được giữ sau khi load scene vì dữ liệu nằm trong PlayerPrefs hoặc manager persistent. Scene mới chỉ cần
đọc lại key đã lưu và áp dụng cho audio, camera, quality. Đây là dữ liệu cục bộ, không phụ
thuộc save slot.

**Khi mở code:**

Mở `GameSettingsManager.LoadSettings(), ApplyAllSettings()`.

**Cách giải thích trên code:**

Nói setting không mất theo scene vì được lưu ngoài scene, khác với object runtime trong Unity scene.

**Code tiêu biểu để chỉ khi bảo vệ:**

Nguồn: `Assets/Game/Scripts/Settings/GameSettingsManager.cs`.

```csharp
private void LoadSettings()
{
    masterVolume = PlayerPrefs.GetFloat(MasterVolumeKey, 1f);
    musicVolume = PlayerPrefs.GetFloat(MusicVolumeKey, 1f);
    sfxVolume = PlayerPrefs.GetFloat(SfxVolumeKey, 1f);
    cameraSensitivity = PlayerPrefs.GetFloat(CameraSensitivityKey, 1f);
    isFullscreen = PlayerPrefs.GetInt(FullscreenKey, Screen.fullScreen ? 1 : 0) == 1;
}

private void SaveSettings()
{
    PlayerPrefs.SetFloat(MasterVolumeKey, masterVolume);
    PlayerPrefs.SetFloat(MusicVolumeKey, musicVolume);
    PlayerPrefs.SetFloat(SfxVolumeKey, sfxVolume);
    PlayerPrefs.SetFloat(CameraSensitivityKey, cameraSensitivity);
    PlayerPrefs.SetInt(FullscreenKey, isFullscreen ? 1 : 0);
    PlayerPrefs.Save();
}
```

**Giải thích code:**

LoadSettings đọc các key đã lưu bằng PlayerPrefs. SaveSettings ghi lại volume, camera sensitivity và fullscreen.
Vì đây là dữ liệu cục bộ của máy, nó không cần nằm trong save slot gameplay.

## 18. Kiểm thử và đánh giá

### Câu 1. Em đã kiểm thử những chức năng nào?

**Trả lời mẫu:**

Em đã kiểm thử điều khiển player, dodge/sprint/jump, combat cận chiến, cung, spell, nhận damage, AI pursue/attack, boss fight, boss
death unlock map, Site of Grace, save/load, inventory, shop, level up, weapon upgrade, settings và multiplayer host-client.

**Khi mở code:**

Mở `Unity Console logs, BossFlow logs, ServerRpc/ClientRpc logs, save file`.

**Cách giải thích trên code:**

Khi debug, lần theo chuỗi input -> manager -> RPC/save -> UI thay vì đoán lỗi ở một chỗ.

### Câu 2. Chức năng nào khó kiểm thử nhất?

**Trả lời mẫu:**

Khó nhất là multiplayer combat và boss progression. Lỗi chỉ xuất hiện khi có nhiều client, network delay hoặc hai
sự kiện gần nhau. Ví dụ boss chết khi hai player cùng đánh, item bị nhặt cùng lúc hoặc scene
transition lệch.

**Khi mở code:**

Mở `Unity Console logs, BossFlow logs, ServerRpc/ClientRpc logs, save file`.

**Cách giải thích trên code:**

Khi debug, lần theo chuỗi input -> manager -> RPC/save -> UI thay vì đoán lỗi ở một chỗ.

### Câu 3. Em đã test multiplayer như thế nào?

**Trả lời mẫu:**

Em test multiplayer bằng host và client chạy song song, dùng editor kèm build hoặc nhiều instance. Các case gồm
join game, điều khiển đúng player, thấy animation của nhau, đánh enemy, projectile, nhặt item, boss fight và chuyển scene.

**Khi mở code:**

Mở `Unity Console logs, BossFlow logs, ServerRpc/ClientRpc logs, save file`.

**Cách giải thích trên code:**

Khi debug, lần theo chuỗi input -> manager -> RPC/save -> UI thay vì đoán lỗi ở một chỗ.

### Câu 4. Có bug nào nghiêm trọng từng gặp không? Em xử lý ra sao?

**Trả lời mẫu:**

Bug nghiêm trọng thường là trạng thái chạy nhiều lần trong multiplayer, ví dụ boss death gọi lặp hoặc item
bị nhặt hai lần. Em xử lý bằng cách đưa quyết định về server, thêm cờ chặn và kiểm tra
điều kiện trước khi cấp thưởng.

**Khi mở code:**

Mở `Unity Console logs, BossFlow logs, ServerRpc/ClientRpc logs, save file`.

**Cách giải thích trên code:**

Khi debug, lần theo chuỗi input -> manager -> RPC/save -> UI thay vì đoán lỗi ở một chỗ.

### Câu 5. Nếu boss không unlock map sau khi chết, em debug từ đâu?

**Trả lời mẫu:**

Em debug theo chuỗi: HP boss có về 0 không, ProcessDeathEvent có chạy không, cờ chống lặp có chặn nhầm
không, RegisterBossDefeat nhận đúng boss ID không, progression có mở đúng map không và save có ghi không.

**Khi mở code:**

Mở `Unity Console logs, BossFlow logs, ServerRpc/ClientRpc logs, save file`.

**Cách giải thích trên code:**

Khi debug, lần theo chuỗi input -> manager -> RPC/save -> UI thay vì đoán lỗi ở một chỗ.

### Câu 6. Nếu player load game sai vị trí, em kiểm tra class/hàm nào?

**Trả lời mẫu:**

Em kiểm tra WorldSaveGameManager phần ghi/đọc position, CharacterSaveData có lưu scene và tọa độ đúng không, WorldSceneManager có load đúng
scene không, và logic đặt player sau scene load có bị entry Site ghi đè không.

**Khi mở code:**

Mở `Unity Console logs, BossFlow logs, ServerRpc/ClientRpc logs, save file`.

**Cách giải thích trên code:**

Khi debug, lần theo chuỗi input -> manager -> RPC/save -> UI thay vì đoán lỗi ở một chỗ.

### Câu 7. Nếu damage không trừ máu trong multiplayer, em kiểm tra luồng nào?

**Trả lời mẫu:**

Em kiểm tra collider có bật đúng frame không, hit có detect target không, ServerRpc damage có được gọi không,
server có quyền sửa HP không, target NetworkObject có đúng không và UI có đăng ký OnValueChanged không.

**Khi mở code:**

Mở `Unity Console logs, BossFlow logs, ServerRpc/ClientRpc logs, save file`.

**Cách giải thích trên code:**

Khi debug, lần theo chuỗi input -> manager -> RPC/save -> UI thay vì đoán lỗi ở một chỗ.

### Câu 8. Nếu item nhặt rồi vẫn xuất hiện lại sau load, nguyên nhân có thể là gì?

**Trả lời mẫu:**

Nếu item nhặt rồi vẫn xuất hiện lại, nguyên nhân có thể là item không có ID duy nhất, ID
không lưu vào save, save chưa gọi, load scene không kiểm tra picked up list hoặc prefab spawn lại mà
không đọc trạng thái save.

**Khi mở code:**

Mở `Unity Console logs, BossFlow logs, ServerRpc/ClientRpc logs, save file`.

**Cách giải thích trên code:**

Khi debug, lần theo chuỗi input -> manager -> RPC/save -> UI thay vì đoán lỗi ở một chỗ.

## 19. Câu hỏi mở rộng

### Câu 1. Nếu có thêm thời gian, em sẽ phát triển thêm chức năng gì?

**Trả lời mẫu:**

Nếu có thêm thời gian, em sẽ phát triển lobby/matchmaking, dedicated server, moveset boss phong phú hơn, quest/NPC, minimap, tối
ưu animation và test tự động. Em cũng muốn cải thiện level design để mỗi map có bản sắc rõ
hơn.

**Khi mở code:**

Mở `Các manager hiện tại: progression, save, network, item database`.

**Cách giải thích trên code:**

Trả lời bằng cách nói điểm cần thêm, class bị ảnh hưởng và rủi ro khi mở rộng.

### Câu 2. Em sẽ tối ưu performance của game như thế nào?

**Trả lời mẫu:**

Em sẽ dùng Profiler để tìm bottleneck trước, sau đó tối ưu draw call, LOD, occlusion culling, pooling projectile/VFX, giảm
Update không cần thiết, tối ưu NavMeshAgent và giới hạn network sync. Với asset, em nén texture và kiểm soát
particle.

**Khi mở code:**

Mở `Các manager hiện tại: progression, save, network, item database`.

**Cách giải thích trên code:**

Trả lời bằng cách nói điểm cần thêm, class bị ảnh hưởng và rủi ro khi mở rộng.

### Câu 3. Em sẽ cải thiện bảo mật multiplayer như thế nào?

**Trả lời mẫu:**

Em sẽ tăng server authority, không tin damage hoặc item do client tự báo, validate khoảng cách tấn công, cooldown,
stamina, inventory và vị trí. Ngoài ra cần chống sửa save, giới hạn rate RPC và dùng dedicated server nếu
triển khai thật.

**Khi mở code:**

Mở `Các manager hiện tại: progression, save, network, item database`.

**Cách giải thích trên code:**

Trả lời bằng cách nói điểm cần thêm, class bị ảnh hưởng và rủi ro khi mở rộng.

### Câu 4. Nếu triển khai chính thức, em cần bổ sung hệ thống gì?

**Trả lời mẫu:**

Nếu triển khai chính thức, cần lobby/matchmaking, account, cloud save, anti-cheat, dedicated server, logging, analytics, crash reporting, tutorial, localization, accessibility,
QA plan và pipeline build. Ngoài ra cần polish art/audio và cân bằng progression.

**Khi mở code:**

Mở `Các manager hiện tại: progression, save, network, item database`.

**Cách giải thích trên code:**

Trả lời bằng cách nói điểm cần thêm, class bị ảnh hưởng và rủi ro khi mở rộng.

### Câu 5. Làm sao để thêm map thứ 6 vào game?

**Trả lời mẫu:**

Để thêm map 6, em tạo scene mới, thêm vào Build Settings, tạo entry Site of Grace, enemy/item/boss, cập nhật
GameProgressionManager để map 5 unlock map 6, thêm transition và kiểm tra save/load cùng difficulty tier.

**Khi mở code:**

Mở `Các manager hiện tại: progression, save, network, item database`.

**Cách giải thích trên code:**

Trả lời bằng cách nói điểm cần thêm, class bị ảnh hưởng và rủi ro khi mở rộng.

### Câu 6. Làm sao để thêm boss mới?

**Trả lời mẫu:**

Để thêm boss mới, em tạo prefab từ AIBossCharacterManager, gán stats, animation, collider, attack actions, boss ID, HP bar name
và EventTriggerBossFight. Sau đó cấu hình fog wall, progression unlock và test death flow.

**Khi mở code:**

Mở `Các manager hiện tại: progression, save, network, item database`.

**Cách giải thích trên code:**

Trả lời bằng cách nói điểm cần thêm, class bị ảnh hưởng và rủi ro khi mở rộng.

### Câu 7. Làm sao để thêm class nhân vật mới?

**Trả lời mẫu:**

Để thêm class mới, em tạo data gồm stat khởi đầu, vũ khí, armor, spell/item ban đầu và UI lựa
chọn ở title. Khi tạo nhân vật, class ID được lưu vào save, player spawn đọc ID để cấp equipment
và stat đúng.

**Khi mở code:**

Mở `Các manager hiện tại: progression, save, network, item database`.

**Cách giải thích trên code:**

Trả lời bằng cách nói điểm cần thêm, class bị ảnh hưởng và rủi ro khi mở rộng.

### Câu 8. Làm sao để thêm weapon/spell/item mới?

**Trả lời mẫu:**

Để thêm weapon/spell/item, em tạo ScriptableObject mới, gán ID duy nhất, icon, model/prefab, animation/action, damage hoặc effect. Sau đó thêm
vào database, loot/shop/inventory và kiểm tra save/load bằng ID.

**Khi mở code:**

Mở `Các manager hiện tại: progression, save, network, item database`.

**Cách giải thích trên code:**

Trả lời bằng cách nói điểm cần thêm, class bị ảnh hưởng và rủi ro khi mở rộng.

### Câu 9. Nếu chuyển từ host-client sang dedicated server thì cần thay đổi gì?

**Trả lời mẫu:**

Nếu chuyển sang dedicated server, cần tách server khỏi client hiển thị, đảm bảo logic quan trọng chạy headless, bỏ
phụ thuộc UI/camera ở server, chuyển scene/session sang server quản lý và validate toàn bộ RPC từ client.

**Khi mở code:**

Mở `Các manager hiện tại: progression, save, network, item database`.

**Cách giải thích trên code:**

Trả lời bằng cách nói điểm cần thêm, class bị ảnh hưởng và rủi ro khi mở rộng.

### Câu 10. Em rút ra bài học gì sau khi thực hiện đồ án này?

**Trả lời mẫu:**

Bài học lớn nhất là game 3D là tập hợp nhiều hệ thống nhỏ phải nối đúng thứ tự. Em
học được cách tách manager, dùng ScriptableObject, xử lý save/load, network sync, AI state machine và debug lỗi giữa gameplay,
UI và multiplayer.

**Khi mở code:**

Mở `Các manager hiện tại: progression, save, network, item database`.

**Cách giải thích trên code:**

Trả lời bằng cách nói điểm cần thêm, class bị ảnh hưởng và rủi ro khi mở rộng.

## 20. Cách dùng tài liệu khi ôn bảo vệ

### Ý 1

Không nên học thuộc nguyên văn. Hãy nắm ba ý cho mỗi câu: mục đích chức năng, class/hàm liên quan
và luồng hoạt động chính.

### Ý 2

Với câu hỏi về code, mở đúng nhóm chức năng rồi chỉ vào hàm trung tâm. Sau đó giải thích
input đi vào đâu, dữ liệu đổi ở đâu và kết quả hiển thị thế nào.

### Ý 3

Khi gặp câu hỏi chưa chuẩn bị, hãy trả lời theo cấu trúc: hiện tại đồ án làm gì, hạn
chế là gì và nếu phát triển tiếp em sẽ cải thiện ra sao.

## 21. Các hàm lõi nên nhớ

- PlayerInputManager.HandleAllInputs(): gom và phân phối input của người chơi.

- PlayerCombatManager.PerformWeaponBasedAction(): nối input tấn công với WeaponItemAction.

- CharacterNetworkManager.NotifyTheServerOfCharacterDamageServerRpc(): gửi yêu cầu xử lý damage lên server.

- AICharacterManager.ProcessStateMachine(): vòng lặp quyết định hành vi enemy.

- AIBossCharacterManager.ProcessDeathEvent(): kết thúc boss fight và kích hoạt progression.

- GameProgressionManager.RegisterBossDefeat(): lưu boss đã chết và mở map mới.

- WorldSaveGameManager.SaveGame()/LoadGame(): ghi và khôi phục dữ liệu chơi.

- SiteOfGraceInteractable.Interact(): kích hoạt/rest checkpoint.

- PlayerUILevelUpManager.ConfirmLevels(): xác nhận tăng stat và trừ rune.

- PlayerShopManager.TryBuyItem(): kiểm tra rune, stock và thêm item vào inventory.


