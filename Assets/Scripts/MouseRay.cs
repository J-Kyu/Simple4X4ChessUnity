using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseRay : MonoBehaviour
{
    public static MouseRay instance;

    [SerializeField]
    private RectTransform testUI;

    private Transform targetPiece;

    public float pickUpHeight = 1.0f;

    private float theta = 0.0f;

    void Start(){
        theta += (ChessSingleton.color*90)*Mathf.Deg2Rad;
        Camera.main.transform.position = new Vector3 (0.3f*Mathf.Cos(theta),Camera.main.transform.position.y,0.3f*Mathf.Sin(theta));
        Camera.main.transform.LookAt(Vector3.zero);

    }

     void Update () {

        testUI.transform.position = Input.mousePosition;
        
        Vector3 mos = Input.mousePosition;
        mos.z = GetComponent<Camera>().farClipPlane;
        Vector3 dir = GetComponent<Camera>().ScreenToWorldPoint(mos);



        if( Input.GetMouseButton (0)) 
        {
        
            RaycastHit[] hits;
            hits = Physics.RaycastAll(transform.position,dir, mos.z);

            if (targetPiece != null){
                Vector3 targetPos = CalIntersectionPoint(transform.position,dir.normalized,0.0f,1.0f,0.0f,pickUpHeight);
                targetPiece.position = targetPos;
            }
            else{
                //picked

                 //find piece and chess block
                for (int i = 0; i < hits.Length; i++){
                    
                    if (hits[i].transform.tag == "Piece" && hits[i].transform.gameObject.GetComponent<ChessPiece>().color == ChessSingleton.color){
                        targetPiece = hits[i].transform;
                        targetPiece.gameObject.GetComponent<ChessPiece>().SetPieceState(ChessPieceState.picked);
                    }
                    else if (hits[i].transform.tag == "ChessBlock"){
                        // Debug.Log("ChessBlock");
                    }
                }
            }

            Debug.DrawRay(transform.position, dir * 1000, Color.blue);

        }
        //drop
        else{
            
            if (targetPiece != null){
                RaycastHit[] hits;
                hits = Physics.RaycastAll(transform.position,dir, mos.z);
                
                GameObject chessBlock = null;

                for (int i = 0; i < hits.Length; i++){    
                    if (hits[i].transform.tag == "ChessBlock"){
                            chessBlock = hits[i].transform.gameObject;
                            break;
                    }
                }

                // if move was made, we take turn
                if (targetPiece.GetComponent<ChessPiece>().DropPiece(chessBlock)){
                    ChessSingleton.TakeTurn();
                    StartCoroutine(MoveCamera());
                }

                targetPiece.gameObject.GetComponent<ChessPiece>().SetPieceState(ChessPieceState.idle);
                targetPiece = null;

            }
               

        }
    }


    public Vector3 CalIntersectionPoint(Vector3 sPoint,Vector3 dir, float a,float b, float c, float k ){

        float t = (k-(sPoint.x*a+sPoint.y*b+sPoint.z*c))/(a*dir.x+b*dir.y+c*dir.z);

        return new Vector3(sPoint.x+dir.x*t,sPoint.y+dir.y*t,sPoint.z+dir.z*t );

    }


    public IEnumerator MoveCamera()
    {
        float _timer = 1.0f;

        while (_timer > 0)
        {
            _timer -= Time.deltaTime;
            theta += (ChessSingleton.color*180*Time.deltaTime/1.0f)*Mathf.Deg2Rad;
            Camera.main.transform.position = new Vector3 (0.3f*Mathf.Cos(theta),Camera.main.transform.position.y,0.3f*Mathf.Sin(theta));
            Camera.main.transform.LookAt(Vector3.zero);

            yield return null;

            if (_timer <= 0)
            {
                //black
                if (ChessSingleton.color > 0){
                    theta = 90*Mathf.Deg2Rad;
                }
                else{
                    theta = 270*Mathf.Deg2Rad;
                }

                Camera.main.transform.position = new Vector3 (0.3f*Mathf.Cos(theta),Camera.main.transform.position.y,0.3f*Mathf.Sin(theta));
                Camera.main.transform.LookAt(Vector3.zero);
            }
        }
    }

     public IEnumerator ResetCamera()
    {
        float _timer = 1.0f;
        float targetAngle = 0.0f;

        if (theta == 90*Mathf.Deg2Rad){
            targetAngle = 0.0f;
        }
        else{
            targetAngle = -180.0f;
        }


        while (_timer > 0)
        {
            _timer -= Time.deltaTime;
            theta += (targetAngle*Time.deltaTime/1.0f)*Mathf.Deg2Rad;
            Camera.main.transform.position = new Vector3 (0.3f*Mathf.Cos(theta),Camera.main.transform.position.y,0.3f*Mathf.Sin(theta));
            Camera.main.transform.LookAt(Vector3.zero);

            yield return null;

            if (_timer <= 0)
            {
                //black
                if (ChessSingleton.color > 0){
                    theta = 90*Mathf.Deg2Rad;
                }
                else{
                    theta = 270*Mathf.Deg2Rad;
                }

                Camera.main.transform.position = new Vector3 (0.3f*Mathf.Cos(theta),Camera.main.transform.position.y,0.3f*Mathf.Sin(theta));
                Camera.main.transform.LookAt(Vector3.zero);
            }
        }
    }

}
