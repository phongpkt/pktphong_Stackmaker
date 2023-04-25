using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class BrickBar1 : MonoBehaviour
{
    public TextMeshProUGUI CountOfBrick;
    public Player numberOfBrick;

    // Update is called once per frame
    void Update()
    {
        CountOfBrick = GetComponent<TextMeshProUGUI>();
        CountOfBrick.text = "Brick = " +  numberOfBrick.numberofbrick;
    }
}
