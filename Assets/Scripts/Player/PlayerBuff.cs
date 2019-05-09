using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBuff : MonoBehaviour {

    public Image buffImage;
    public Text buffTime;
    public PlayerUI myUI;

    public void updateBuff(int input)
    {
        buffTime.text = "" + input;
        myUI.updateChildrenPosition();
    }

}