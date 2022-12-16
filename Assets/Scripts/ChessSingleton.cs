using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessSingleton : MonoBehaviour
{
    public static ChessSingleton instance;

    [SerializeField]
    public ChessBoard chessBoard =  null;

    [SerializeField]
    public MouseRay mouseRay =  null;


    public static int color = 1; // 1 -> blakc , -1 -> white
 
    void Awake () {
        instance = this;
        ChessBoard.instance = chessBoard;
        MouseRay.instance = mouseRay;

    }


    public static void TakeTurn(){
        color = -1*color;
        // Camera.main.transform.position = new Vector3 (Camera.main.transform.position.x,Camera.main.transform.position.y,-1*Camera.main.transform.position.z);
        // Camera.main.transform.LookAt(Vector3.zero);
    }

     public static void ResetTurn(){
        color = 1;
     }
}
