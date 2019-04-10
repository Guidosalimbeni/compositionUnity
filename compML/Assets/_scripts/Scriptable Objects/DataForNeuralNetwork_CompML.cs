using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "new compMLData", menuName = "CompML Neural Network Data", order = 51)]

public class DataForNeuralNetwork_CompML : ScriptableObject
{

    [SerializeField]
    private List<string> names;
    [SerializeField]
    private List<double> g0s;
    [SerializeField]
    private List<double> g1s;
    [SerializeField]
    private List<double> g2s;
    [SerializeField]
    private List<double> g3s;
    [SerializeField]
    private List<double> g4s;
    [SerializeField]
    private List<double> g5s;
    [SerializeField]
    private List<double> judges;


    public List<string> Names { get { return names; } set { names = value; } }
    public List<double> G0 { get { return g0s; } set { g0s = value; } }
    public List<double> G1 { get { return g1s; } set { g1s = value; } }
    public List<double> G2 { get { return g2s; } set { g2s = value; } }
    public List<double> G3 { get { return g3s; } set { g3s = value; } }
    public List<double> G4 { get { return g4s; } set { g4s = value; } }
    public List<double> G5 { get { return g5s; } set { g5s = value; } }
    public List<double> JUDGE { get { return judges; } set { judges = value; } }

}

