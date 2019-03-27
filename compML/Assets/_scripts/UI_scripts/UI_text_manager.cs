using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_text_manager : MonoBehaviour
{
    public GameObject BondsLeftBalanceObj;
    public GameObject BondsRightBalanceObj;

    private AreasObjects areaobjects;
    private TextMeshProUGUI BondsLeftBalanceText;
    private TextMeshProUGUI BondsRightBalanceText;

    void Awake()
    {
        BondsLeftBalanceText = BondsLeftBalanceObj.GetComponent<TextMeshProUGUI>();
        BondsRightBalanceText = BondsRightBalanceObj.GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        areaobjects = Game_Manager.Instance.areasObjects;
        string leftWeigthtPerc = ((areaobjects.ObjectsLeftAreaPercentage) * 100).ToString();
        string RightWeigthPerc =((areaobjects.ObjectsRightAreaPercentage) * 100).ToString();
        BondsLeftBalanceText.text = leftWeigthtPerc;
        BondsRightBalanceText.text = RightWeigthPerc;
    }


}
