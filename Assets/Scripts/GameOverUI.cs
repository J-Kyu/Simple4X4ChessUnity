using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField]
    private Text titleText = null;



    public void SetTitleText(string title){
        titleText.text = title;
    }
}
