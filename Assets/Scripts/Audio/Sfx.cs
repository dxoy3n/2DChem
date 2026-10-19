using UnityEngine;

public enum SfxType { Fire, Land, Precipitate, Drop, Win, Lose }

// Đặt trên object Sfx (scene Game)
public class Sfx : MonoBehaviour
{
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip fire, land, precipitate, drop, win, lose;

    static Sfx instance;

    void Awake() => instance = this;
    void OnDestroy() { if (instance == this) instance = null; }

    public static void Play(SfxType type)
    {
        if (instance == null) return;

        AudioClip clip = instance.Clip(type);
        if (clip != null) instance.source.PlayOneShot(clip);
    }

    AudioClip Clip(SfxType type) => type switch
    {
        SfxType.Fire => fire,
        SfxType.Land => land,
        SfxType.Precipitate => precipitate,
        SfxType.Drop => drop,
        SfxType.Win => win,
        _ => lose,
    };
}
