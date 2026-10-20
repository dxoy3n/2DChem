# ChemistryBlast – Game bắn bóng hóa học (2D)

Game puzzle 2D theo phong cách bubble shoot. Người chơi bắn bóng ion vào bảng, khi bóng ion gặp đúng ion trong bảng sẽ tạo **kết tủa** và được cộng điểm. Mục tiêu là đạt đủ số phản ứng trong giới hạn lượt đi.

## Thông tin
- **Engine:** Unity 6000.0.81f1 (Unity 6), URP 2D
- **Ngôn ngữ:** C#
- **Nền tảng:** Android (APK nộp riêng), chạy thử trên Standalone
- **GitHub:** https://github.com/dxoy3n/2DChem

## Cách mở dự án
1. Cài Unity Hub và Unity 6000.0.81f1 (hoặc phiên bản Unity 6 tương thích).
2. Clone repo:
   ```
   git clone https://github.com/dxoy3n/2DChem.git
   ```
3. Mở Unity Hub → Add project from disk → chọn thư mục dự án.
4. Mở scene `Assets/Scenes/MainMenu.unity` và nhấn Play.

## Luật chơi
- Bóng bắn ra lấy từ hàng chờ; có thể đổi bóng trên súng với bóng kế tiếp.
- Bảng bóng ion rơi xuống theo số lượt quy định; mỗi lần rơi xuất hiện thêm một hàng bóng ngẫu nhiên.
- Khi bóng mới chạm bóng ion có thể tạo kết tủa: bóng bị loại và cộng điểm.
- **Thắng:** đạt đủ số phản ứng yêu cầu của màn.
- **Thua:** bóng chạm vạch đỏ, hết lượt, hoặc hết bóng để bắn.

## Cấu trúc thư mục chính
```
Assets/
├── Scenes/        MainMenu, Game
├── Scripts/
│   ├── Core/      GameManager, Level, HUD, Session, Progress, Pause
│   ├── Board/     Board, Ball, DeadlineWarn
│   ├── Shot/      Shooter, ShotQueue, Projectile, Trajectory
│   ├── Chem/      Ion, ReactionTable
│   ├── Audio/     Sfx, Music
│   └── Menu/      Menu
├── Data/
│   ├── Ions/      Dữ liệu từng ion (ScriptableObject)
│   ├── Table/     Bảng phản ứng (Table_01, Table_02)
│   └── Levels/    Cấu hình màn (Level_01, Level_02)
├── Prefabs/       Ball
└── Sprites/       Hình ảnh
```

## Thành viên thực hiện
- Đào Thị Thúy Yên

## Ghi chú
- Các thư mục `Library/`, `Temp/`, `Obj/`, `Logs/`, `UserSettings/` không có trong repo vì Unity tự tạo lại.
