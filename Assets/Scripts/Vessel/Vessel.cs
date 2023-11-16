using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Vessel : MonoBehaviour
{
    public List<Liquid> liquids;

    [SerializeField] private GameObject liquidPrefab;

    [SerializeField] private TMP_Text displayName;

    public void UpdateLiquids()
    {
        InstantiateListedLiquids();
        SetChemicalName();
    }

    private void Start()
    {
        GetStarterLiquids();
        SetChemicalName();
    }

    private void OnTriggerEnter(Collider othercollision)
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, -10), Time.deltaTime * 100f);
    }

    private void GetStarterLiquids()
    {
        var liquidsOnStart = GetComponentsInChildren<Liquid>();
        foreach (var liquid in liquidsOnStart)
        {
            liquids.Add(liquid);
        }
    }

    private void InstantiateListedLiquids()
    {
        var liquidComponents = GetComponentsInChildren<Liquid>();

        foreach (var liquid in liquids)
        {
            if (liquidComponents.Contains(liquid)) continue;
            var liquidInstance =
                Instantiate(liquidPrefab, new Vector3(0.0f, -40.0f, 0.0f),
                    Quaternion.identity); //TODO come back to this when multi-level liquids are implemented
            liquidInstance.transform.parent = transform;
            liquidInstance.GetComponent<ChemicalComponent>().chemicalName = liquid.chemicalComponent.chemicalName;
            liquidInstance.GetComponent<ChemicalComponent>()._enum = liquid.chemicalComponent._enum;
            liquidInstance.GetComponent<ChemicalComponent>()._material = liquid.chemicalComponent._material;
            liquidInstance.GetComponent<Liquid>().chemicalComponent = liquid.chemicalComponent;

            Debug.Log("new liquid in vessel is ", liquid.chemicalComponent);
        }
    }

    private void SetChemicalName()
    {
        var chemicalComponents = GetComponentsInChildren<ChemicalComponent>();

        var resultName = "";

        var chemicals = GetChemicalsInVessel(chemicalComponents);
        var chemicalCount = chemicals.Count;

        resultName = ComposeName(chemicalCount, chemicals, resultName);
        Debug.Log("Name set: " + resultName);
        displayName.GetComponent<TMP_Text>().text = resultName.Remove(resultName.Length - 2); //remove last coma ", "
    }

    private static string ComposeName(int chemicalCount, List<ChemicalEnums.Chemical> chemicals, string resultName)
    {
        for (var i = 0; i < chemicalCount; i++)
        {
            if (i < chemicalCount - 1)
            {
                if (ChemicalReactions.CheckReactionResult((chemicals[i], chemicals[i + 1])))
                {
                    var reactionResult = ChemicalReactions.GetReactionResult((chemicals[i], chemicals[i + 1]));
                    resultName += ChemicalEnums.chemicalData[reactionResult] + ", ";
                    i++;
                }
                else
                {
                    resultName += chemicals[i] + ", ";
                }
            }
            else
            {
                resultName += chemicals[i] + ", ";
            }
        }

        return resultName;
    }

    private static List<ChemicalEnums.Chemical> GetChemicalsInVessel(IEnumerable<ChemicalComponent> chemicalComponents)
    {
        return chemicalComponents.Select(chemicalComponent => chemicalComponent._enum).Distinct().ToList();
    }
}