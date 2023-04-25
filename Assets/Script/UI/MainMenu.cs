using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void PlayGameButton()
    {
        UIManager.Ins.CloseUI(UIID.MainMenu,0.5f);
    }
}
