using UnityEngine;

[CreateAssetMenu(menuName = "Chem/Level", fileName = "Level")]
public class Level : ScriptableObject
{
    public int index;                  // 0 = Màn 1, 1 = Màn 2
    public ReactionTable table;
    public Level next;                 // Để trống nếu là màn cuối

    [Header("Luật")]
    public int startRows = 4;
    public int movesPerDrop = 3;       // Cứ N lượt thì bảng xuống 1 hàng
    public int maxMoves = 30;          // Hết lượt mà chưa clear = THUA
    public int totalBalls = 20;        // Tổng bóng của màn (rơi + bắn)
}