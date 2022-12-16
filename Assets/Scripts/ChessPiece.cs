using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessPiece : MonoBehaviour
{

    public int color = 1;
    private string blockCode;

    public ChessPieceType chessPieceType = ChessPieceType.Pawn;
    private ChessPieceState state = ChessPieceState.idle;
    private ChessBoardBlockCode chessBoardBlockCode = ChessBoardBlockCode.A_1;

    public GameObject previousBlock = null;
    private GameObject origBlock = null;

    [SerializeField]
    private PieceStateUI stateUI = null;

    void Start(){

        //set UI
        stateUI.SetTitleText(chessPieceType.ToString());
        stateUI.SetPieceColor(color);

        if (previousBlock != null){
            origBlock = previousBlock;
            previousBlock.GetComponent<BoardBlocks>().SetBlockOwner(this.gameObject);
            this.transform.position = previousBlock.transform.position;
            chessBoardBlockCode = previousBlock.GetComponent<BoardBlocks>().GetChessBoardBlockCode();
        }
    }

    void Update(){
        Vector2 uiPos = Camera.main.WorldToScreenPoint(transform.position);
        stateUI.SetPanelPos(uiPos.x,uiPos.y);

        //relativePosition = cameraTransform.InverseTransformDirection(transform.position - cameraTransform.position);
    }

    public void ResetPieceToOrigBlock(){
        previousBlock = origBlock;
        this.transform.position = origBlock.transform.position;
        origBlock.GetComponent<BoardBlocks>().SetBlockOwner(this.gameObject);
        chessBoardBlockCode = previousBlock.GetComponent<BoardBlocks>().GetChessBoardBlockCode();
    }

    public void SetBlockCode(string code){
        this.blockCode = code+"_Piece";
    }

    public string GetBlockCode(){
        return this.blockCode;
    }

    public void SetPieceState(ChessPieceState state){
        this.state = state;
        stateUI.SetStateText(state);
    }

    public ChessPieceState GetPieceState(){
        return this.state;
    }

    public void ToggleStateUI(){

        if(stateUI.gameObject.activeSelf == true)  { 
            stateUI.gameObject.SetActive(false);
        }
        else if(stateUI.gameObject.activeSelf == false){ 
            stateUI.gameObject.SetActive(true);
        }
    }


    public bool DropPiece(GameObject chessBlock){

        if (chessBlock != null && IsValidBoardBlock(chessBlock.GetComponent<BoardBlocks>())){
                
                if (chessBlock.GetComponent<BoardBlocks>().character != null && chessBlock.GetComponent<BoardBlocks>().character.GetComponent<ChessPiece>().color == color ){
                    this.transform.position = previousBlock.transform.position;
                    return false;
                }


                // Set BoardBlock Owner...if already exist, it will move previous owner
                chessBlock.GetComponent<BoardBlocks>().SetBlockOwner(this.gameObject);

                //set position
                previousBlock.GetComponent<BoardBlocks>().character = null;
                this.transform.position = chessBlock.transform.position;
                previousBlock = chessBlock;
                return true; 
         
        }
        else{
            //if chess Block is none valid location or block
            this.transform.position = previousBlock.transform.position;
        }

        return false;
    }



    private bool IsValidBoardBlock(BoardBlocks boardBlock){
        string[] code = boardBlock.GetChessBoardBlockCode().ToString().Split('_');
        char columCode = boardBlock.columnCode;
        char rowCode = boardBlock.rowCode;

        switch(chessPieceType){
            case ChessPieceType.Pawn:{
                return PawnLogic(boardBlock.columnCode,boardBlock.rowCode);
            }
            case ChessPieceType.Rook:{
                return RookLogic(boardBlock.columnCode,boardBlock.rowCode);
            }

            case ChessPieceType.Bishop:{
                 return BishopLogic(boardBlock.columnCode,boardBlock.rowCode);
            }
            case ChessPieceType.Queen:{
                return QueenLogic(boardBlock.columnCode,boardBlock.rowCode);
            }
            case ChessPieceType.King:{
                return KingLogic(boardBlock.columnCode,boardBlock.rowCode);
            }
            default:{
                return PawnLogic(boardBlock.columnCode,boardBlock.rowCode);
            }
        }


        //return PawnLogic(char.Parse(columCode),char.Parse(rowCode));

    }
    
    
    private bool PawnLogic(char columnCode, char rowCode){

        string[] prevCode = previousBlock.GetComponent<BoardBlocks>().GetChessBoardBlockCode().ToString().Split('_');

        char preColumnCode = char.Parse(prevCode[0]);
        char preRowCode = char.Parse(prevCode[1]);

        //check if any character is on the path
        List<BoardBlocks> blocks = ChessSingleton.instance.chessBoard.blockList;
        int index =  (int)columnCode-65 + ((int)rowCode-49)*4;
        //black
        if (color > 0){
            //front

            if((int)preRowCode+1 == (int)rowCode &&  (int)preColumnCode == (int)columnCode && blocks[index].character == null ){
                return true;
            }


            //side
            if ( (int)preRowCode + 1 == (int)rowCode && ( (int)preColumnCode-1 == columnCode || preColumnCode+1 == columnCode ) && (blocks[index].character != null) ){
                return true;
            }

        }
        else{
            
            if((int)preRowCode-1 == (int)rowCode &&  (int)preColumnCode == (int)columnCode && blocks[index].character == null ){
                return true;
            }



            if ( (int)preRowCode - 1 == (int)rowCode && ( (int)preColumnCode-1 == columnCode || preColumnCode+1 == columnCode) && (blocks[index].character != null) ){
                return true;
            }
        }

        return false;



    }

    private bool RookLogic(char columnCode, char rowCode){
        string[] prevCode = previousBlock.GetComponent<BoardBlocks>().GetChessBoardBlockCode().ToString().Split('_');

        char preColumnCode = char.Parse(prevCode[0]);
        char preRowCode = char.Parse(prevCode[1]);

        //check if any character is on the path
        List<BoardBlocks> blocks = ChessSingleton.instance.chessBoard.blockList;

        //check if given block is valid block
        if ((int)columnCode == (int)preColumnCode){


            for (int i = 0 ; i < blocks.Count; i++){

                if(blocks[i].columnCode == columnCode){

                    //case 1  prev  target block --> pass
                    //case 2 target prev block --> pass

                    //case 3 (prev blokc target) (target block prev) --> false 
                    

                    if ( ((int)preRowCode < (int)blocks[i].rowCode && (int)blocks[i].rowCode < (int)rowCode ) || (  ((int)rowCode < (int)blocks[i].rowCode && (int)blocks[i].rowCode < (int)preRowCode )  )  ){
                        if (blocks[i].character != null){
                            return false;
                        }
                    }

                }
            }
            return true;

        } 
        //row
        else if ((int)rowCode == (int)preRowCode ) {

            for (int i = 0 ; i < blocks.Count; i++){

                if((int)blocks[i].rowCode == (int)rowCode){

                    //case 1  prev  target block --> pass

                    //case 2 prev blokc target --> false

                    //case 3 target prev block --> pass

                    if ( ( (int)preColumnCode < (int)blocks[i].columnCode && (int)blocks[i].columnCode < (int)columnCode ) || ( (int)columnCode < (int)blocks[i].columnCode && (int)blocks[i].columnCode < (int)preColumnCode ) ){
                        if (blocks[i].character != null){
                            return false;
                        }
                    }

                }
            }
            return true;

        }

        return false;

        
    }

    private bool BishopLogic(char columnCode, char rowCode){
        string[] prevCode = previousBlock.GetComponent<BoardBlocks>().GetChessBoardBlockCode().ToString().Split('_');

        char preColumnCode = char.Parse(prevCode[0]);
        char preRowCode = char.Parse(prevCode[1]);

        //check if any character is on the path
        List<BoardBlocks> blocks = ChessSingleton.instance.chessBoard.blockList;

        //on the same diagonal
        int columnDiff = Mathf.Abs((int)columnCode - (int)preColumnCode);
        int rowDiff = Mathf.Abs((int)rowCode - (int)preRowCode);

        //if not diagonal
        if (columnDiff != rowDiff){
            return false;
        }

        if (columnDiff == 1){
            return true;
        }

        int xDir = (int)columnCode > (int)preColumnCode ? 1 : -1;
        int yDir = (int)rowCode > (int)preRowCode ? 1 : -1;
        int index = 0;
        if (xDir > 0 && yDir > 0){
            //+ +
            // + 5

            for (int i = 1; i < columnDiff; i++){

                index = (int)preColumnCode-65 + ((int)preRowCode-49)*4 + i*5;

                //out of range
                if (index >= blocks.Count){
                    break;
                }

                if(blocks[index].character != null){
                    return false;
                }
            }
            return true;



        }
        else if (xDir < 0 && yDir > 0){
            // - +

            //+3
            for (int i = 1; i < columnDiff; i++){

                index = (int)preColumnCode-65 + ((int)preRowCode-49)*4 + i*3;

                //out of range
                if (index >= blocks.Count){
                    break;
                }

                if(blocks[index].character != null){
                    return false;
                }
            }
            return true;


        
        }
        else if (xDir < 0 && yDir < 0){
            // - - 
            //-5
            for (int i = 1; i < columnDiff; i++){

                index = (int)preColumnCode-65 + ((int)preRowCode-49)*4 - i*5;

                //out of range
                if (index < 0){
                    break;
                }

                if(blocks[index].character != null){
                    return false;
                }
            }
            return true;

        
        }
        else if (xDir > 0 && yDir < 0){
            // + -

            //-3
            for (int i = 1; i < columnDiff; i++){

                index = (int)preColumnCode-65 + ((int)preRowCode-49)*4 - i*3;
                Debug.Log(index);
                //out of range
                if (index < 0){
                    break;
                }

                if(blocks[index].character != null){
                    return false;
                }
            }
            return true;
        
        }

        
    

        return false;

        
    }

    private bool QueenLogic(char columnCode, char rowCode){
        return ( RookLogic(columnCode,rowCode) || BishopLogic(columnCode,rowCode));
    }
    private bool KingLogic(char columnCode, char rowCode){

        string[] prevCode = previousBlock.GetComponent<BoardBlocks>().GetChessBoardBlockCode().ToString().Split('_');

        char preColumnCode = char.Parse(prevCode[0]);
        char preRowCode = char.Parse(prevCode[1]);

        //check if any character is on the path
        List<BoardBlocks> blocks = ChessSingleton.instance.chessBoard.blockList;

        int preX = (int)preColumnCode-65;
        int preY = ((int)preRowCode-49);
        int index = 0;

        for(int i = -1; i < 2; i++){
            for (int j = -1 ; j < 2; j ++){
                if( preX+i >= 4 || preX+i < 0 || preY+j >= 4 || preY +j < 0  ){
                    continue;
                }

                index = (preX+i)+(preY+j)*4;


                if (blocks[index].columnCode == columnCode && blocks[index].rowCode == rowCode){
                    return true;
                }
                
            }
        }


        
        return false;
    }

}
