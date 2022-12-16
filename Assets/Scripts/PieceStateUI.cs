using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PieceStateUI : MonoBehaviour
{
    [SerializeField]
    private RectTransform panelTransform = null;

    [SerializeField]
    private Text statusText = null;

    [SerializeField]
    private Text titleText = null;

    [SerializeField]
    private Image colorImage = null;

    public void SetPanelPos(float x, float y){
        panelTransform.anchoredPosition = new Vector2(x,y);
    }

    public void SetStateText(ChessPieceState state){
        statusText.text = state.ToString();
    }

    public void SetTitleText(string chessType){
        titleText.text = chessType;
    }

    public void SetPieceColor(int color){
        

        //black
        if (color > 0){
            colorImage.color = new Color(0.0f,0.0f,0.0f);
        }
        //white
        else{
            colorImage.color = new Color(1.0f,1.0f,1.0f);            
        }
    }
}
