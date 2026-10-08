using UnityEngine;

[System.Serializable]
public class ReactionRule
{
    public Ion ionA;
    public Ion ionB;
    public string precipitate;
    public int points = 10;
}

[CreateAssetMenu(menuName = "Chem/Reaction Table", fileName = "ReactionTable")]
public class ReactionTable : ScriptableObject
{
    public Ion[] ions;
    public ReactionRule[] rules;

    public Ion RandomIon() => ions[Random.Range(0, ions.Length)];

    public ReactionRule Find(Ion a, Ion b)
    {
        foreach (var r in rules)
            if ((r.ionA == a && r.ionB == b) || (r.ionA == b && r.ionB == a))
                return r;
        return null;
    }
}