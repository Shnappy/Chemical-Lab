using System.Collections.Generic;
using UnityEngine;

public class ChemicalReactions : MonoBehaviour
{
    private static readonly Dictionary<(ChemicalEnums.Chemical, ChemicalEnums.Chemical), ChemicalEnums.ChemicalSolution> ReactionsDictionary = new()
    {
        {(ChemicalEnums.Chemical.H2, ChemicalEnums.Chemical.O2), ChemicalEnums.ChemicalSolution.H2O},
        {(ChemicalEnums.Chemical.O2, ChemicalEnums.Chemical.H2), ChemicalEnums.ChemicalSolution.H2O}
    };

    public static ChemicalEnums.ChemicalSolution GetReactionResult((ChemicalEnums.Chemical, ChemicalEnums.Chemical) chemicalTuple)
    {
        return ReactionsDictionary[chemicalTuple];
    }
    
    public static bool CheckReactionResult((ChemicalEnums.Chemical, ChemicalEnums.Chemical) chemicalTuple)
    {
        return ReactionsDictionary.ContainsKey(chemicalTuple);
    }
}
