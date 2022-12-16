using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class ChessBoard : MonoBehaviour
{
    public static ChessBoard instance;

    [SerializeField]
    private GameObject boardBlock;

    [SerializeField]
    private Transform boardTrans;

    public GameOverUI gameOverUI;

    
    public List<BoardBlocks> blockList;

    public List<ChessPiece> chessPieceList;


    public void ResetChessPiece(){

        for(int i = 0; i < blockList.Count; i++){
            blockList[i].SetBlockOwner(null,true);
        }



        for(int i = 0; i < chessPieceList.Count; i++){
            chessPieceList[i].ResetPieceToOrigBlock();
        }

        ChessSingleton.ResetTurn();
        StartCoroutine(ChessSingleton.instance.mouseRay.ResetCamera());
        return;
    }

    public void TogglePieceUI(){
        for(int i = 0; i < chessPieceList.Count; i++){
            chessPieceList[i].ToggleStateUI();
        }
    }

}
