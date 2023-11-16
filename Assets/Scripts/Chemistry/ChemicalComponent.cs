using UnityEngine;

public class ChemicalComponent : MonoBehaviour
{
    //TODO replace with setters and getters
    public string chemicalName;
    public Material _material;
    public ChemicalEnums.Chemical _enum;

    [SerializeField] private Material _rectionToLitmus;
    [SerializeField] private Material _rectionToPhenolphthalein;
    [SerializeField] private Material _reactionToMethylOrange;

    public Material getReactionToIndicator(ChemicalEnums.ChemicalIndicator indicatorEnum)
    {
        switch (indicatorEnum)
        {
            case ChemicalEnums.ChemicalIndicator.Litmus:
                return _rectionToLitmus;
            case ChemicalEnums.ChemicalIndicator.Phenolphthalein:
                return _rectionToPhenolphthalein;
            case ChemicalEnums.ChemicalIndicator.MethylOrange:
                return _reactionToMethylOrange;
        }

        return null; //TODO add exception handling
    }
}