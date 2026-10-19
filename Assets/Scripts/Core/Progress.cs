using UnityEngine;

// Lưu số màn đã mở bằng PlayerPrefs. Màn đầu luôn mở.
public static class Progress
{
    const string Key = "unlocked";

    public static int Unlocked => PlayerPrefs.GetInt(Key, 1);

    public static bool IsUnlocked(int index) => index < Unlocked;

    // Gọi khi thắng màn index: mở màn kế tiếp
    public static void Complete(int index)
    {
        int count = index + 2;
        if (count > Unlocked)
        {
            PlayerPrefs.SetInt(Key, count);
            PlayerPrefs.Save();
        }
    }

    public static void Reset()
    {
        PlayerPrefs.DeleteKey(Key);
        PlayerPrefs.Save();
    }
}
