using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardBlocks : MonoBehaviour
{
    private string blockCode;

    public GameObject character;

    [SerializeField]
    private ChessBoardBlockCode chessBoardBlockCode = ChessBoardBlockCode.A_1;
    
    [SerializeField]
    private TextMesh blockTitle;

    public char columnCode;
    
    public char rowCode;

    void Start(){
        this.blockTitle.text = chessBoardBlockCode.ToString();
        string[] code = chessBoardBlockCode.ToString().Split('_');
        columnCode = char.Parse(code[0]);
        rowCode = char.Parse(code[1]);
    }

    public void SetBlockCode(string code){
        this.blockCode = code;
        this.blockTitle.text = code;
    }

    public string GetBlockCode(){
        return this.blockCode;
    }

    public ChessBoardBlockCode GetChessBoardBlockCode(){
        return chessBoardBlockCode;
    }

    public void SetBlockOwner(GameObject owner, bool init = false){

        if (character != null){
            //move character

            if(!init && character.GetComponent<ChessPiece>().chessPieceType == ChessPieceType.King){
                if (character.GetComponent<ChessPiece>().color > 0){
                    Debug.Log("White Win");
                    ChessSingleton.instance.chessBoard.gameOverUI.gameObject.SetActive(true);
                    ChessSingleton.instance.chessBoard.gameOverUI.SetTitleText("White Win");
                }
                else{
                    Debug.Log("Black Win");
                    ChessSingleton.instance.chessBoard.gameOverUI.gameObject.SetActive(true);
                    ChessSingleton.instance.chessBoard.gameOverUI.SetTitleText("Black Win");
                }


            }


            character.transform.position = new Vector3(10,-1,10);
        }

        character = owner;
    }

}
