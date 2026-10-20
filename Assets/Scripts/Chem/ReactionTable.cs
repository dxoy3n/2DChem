using UnityEngine;

[System.Serializable]
public class ReactionRule
{
    public Ion ionA;
    public Ion ionB;
    public string precipitate;        // Tên hiển thị, hỗ trợ rich text: BaSO<sub>4</sub>
    public string equation;          
    [TextArea] public string fact;   
    public int points = 10;
}

[CreateAssetMenu(menuName = "Chem/Reaction Table", fileName = "ReactionTable")]
public class ReactionTable : ScriptableObject
{
    public Ion[] ions;
    public ReactionRule[] rules;

    [Header("Ion trung hòa (chỉ rơi)")]
    public Ion[] fillers;
    [Range(0, 1)] public float fillerChance = 0.25f;

    public Ion RandomIon() => ions[Random.Range(0, ions.Length)];
    public Ion RandomFiller() => fillers[Random.Range(0, fillers.Length)];

    public ReactionRule Find(Ion a, Ion b)
    {
        foreach (var r in rules)
            if ((r.ionA == a && r.ionB == b) || (r.ionA == b && r.ionB == a))
                return r;
        return null;
    }
}