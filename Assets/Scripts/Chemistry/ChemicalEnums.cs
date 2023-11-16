using System.Collections;
using UnityEngine;

public class ChemicalEnums : MonoBehaviour
{
    public enum Chemical
    {
        H2,
        O2,
        HCl,
        NaOH,
        CuSO4,
        H2SO4,
        NaCl,
        Litmus,
        Phenolphthalein,
        MethylOrange
    }

    public enum ChemicalSolution
    {
        H2O,
        H2O_Cl2,
        H2O_NaCl,
        Cu_br_OH_br_2_Na2SO4,
        Na2SO4_HCl
    }

    public enum ChemicalIndicator
    {
        Litmus,
        Phenolphthalein,
        MethylOrange
    }

    public static Hashtable chemicalData = new()
    {
        { Chemical.H2, "H2" },
        { Chemical.O2, "O2" },
        { Chemical.HCl, "HCl" },
        { Chemical.NaOH, "NaOH" },
        { Chemical.CuSO4, "CuSO4" },
        { Chemical.H2SO4, "H2SO4" },
        { Chemical.NaCl, "NaCl" },
        { Chemical.Litmus, "Litmus" },
        { Chemical.Phenolphthalein, "Phenolphthalein" },
        { Chemical.MethylOrange, "MethylOrange" },
        { ChemicalIndicator.Litmus, "Litmus" },
        { ChemicalIndicator.Phenolphthalein, "Phenolphthalein" },
        { ChemicalIndicator.MethylOrange, "MethylOrange" },
        { ChemicalSolution.H2O, "H2O" },
        { ChemicalSolution.H2O_Cl2, "H2O + Cl2" },
        { ChemicalSolution.H2O_NaCl, "H2O + NaCl" },
        { ChemicalSolution.Cu_br_OH_br_2_Na2SO4, "Cu(OH)2 + Na2SO4" },
        { ChemicalSolution.Na2SO4_HCl, "Na2SO4 + HCl" }
    };
}