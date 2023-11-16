using System.Collections.Generic;
using UnityEngine;

public class ChemicalReactions : MonoBehaviour
{
    private static Dictionary<(ChemicalEnums.Chemical, ChemicalEnums.Chemical), ChemicalEnums.ChemicalSolution> _reactionsDictionary = new()
    {
        {(ChemicalEnums.Chemical.H2, ChemicalEnums.Chemical.O2), ChemicalEnums.ChemicalSolution.H2O},
        {(ChemicalEnums.Chemical.O2, ChemicalEnums.Chemical.H2), ChemicalEnums.ChemicalSolution.H2O}
        
    };

    public static ChemicalEnums.ChemicalSolution GetReactionResult((ChemicalEnums.Chemical, ChemicalEnums.Chemical) chemicalTuple)
    {
        return _reactionsDictionary[chemicalTuple];
    }
    
    public static bool CheckReactionResult((ChemicalEnums.Chemical, ChemicalEnums.Chemical) chemicalTuple)
    {
        return _reactionsDictionary.ContainsKey(chemicalTuple);
    }
}
