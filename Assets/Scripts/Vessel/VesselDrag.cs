using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VesselDrag : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private bool _isHeld;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnMouseDrag()
    {
        if (Camera.main != null)
        {
            var mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                -Camera.main.transform.position.z + transform.position.z);
            var objPosition = Camera.main.ScreenToWorldPoint(mousePosition);

            transform.position = objPosition;
        }

        _rigidbody.isKinematic = true;
    }


    private void OnTriggerEnter(Collider other)
    {
        
        var thisVessel = GetComponent<Vessel>();
        var otherVessel = other.gameObject.GetComponent<Vessel>();

        var thisTopMostLiquid = GetTopMostLiquid(thisVessel.liquids);
        var otherTopMostLiquid = GetTopMostLiquid(otherVessel.liquids);

        var thisChemicalIndicator = GetChemicalIndicator(thisTopMostLiquid);
        var otherChemicalIndicator = GetChemicalIndicator(otherTopMostLiquid);

        var thisChemicalComponent = GetChemicalComponent(thisTopMostLiquid);
        var otherChemicalComponent = GetChemicalComponent(otherTopMostLiquid);


        ApplyIndicator(otherTopMostLiquid, otherChemicalComponent, thisChemicalIndicator);
        if (_isHeld) return;
        AddLiquid(thisChemicalComponent, otherChemicalComponent, thisVessel, otherTopMostLiquid);
    }

    private static void ApplyIndicator(Liquid otherTopMostLiquid, ChemicalComponent otherChemicalComponent,
        ChemicalIndicator thisChemicalIndicator)
    {
        if (thisChemicalIndicator == null) return;
        otherTopMostLiquid.SetChemicalMaterial(
            otherChemicalComponent.getReactionToIndicator(thisChemicalIndicator._enum));
        Debug.Log("indicator applied");
    }

    private void OnMouseUp()
    {
        _rigidbody.isKinematic = false;
        _isHeld = false;
    }

    private void OnMouseDown()
    {
        _isHeld = true;
    }

    private static Liquid GetTopMostLiquid(List<Liquid> liquids)
    {
        return liquids.Any() ? liquids[0] : null;
    }

    private static ChemicalComponent GetChemicalComponent(Liquid liquid)
    {
        try
        {
            return liquid.chemicalComponent;
        }
        catch (NullReferenceException e)
        {
            Debug.Log("Empty vessel detected: " + e);
        }

        return null;
    }

    private static ChemicalIndicator GetChemicalIndicator(Liquid liquid)
    {
        try
        {
            return liquid.ChemicalIndicator;
        }
        catch (NullReferenceException e)
        {
            Debug.Log("Vessel contains no indicator on top: " + e);
        }

        return null;
    }

    private static void AddLiquid(ChemicalComponent thisChemicalComponent, ChemicalComponent otherChemicalComponent,
        Vessel thisVessel, Liquid otherTopMostLiquid)
    {
        if (thisChemicalComponent == otherChemicalComponent) return;
        thisVessel.liquids.Add(otherTopMostLiquid);
        thisVessel.UpdateLiquids();
        Debug.Log("liquid added");
    }
}