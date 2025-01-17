using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendPanelManager : MonoBehaviour
{

 
    public GameObject FriendListPanel;
    public GameObject FriendAcceptPanel;



    private void Start()
    {
        FriendListPanel.SetActive(true);
        FriendAcceptPanel.SetActive(false);
    }


    public void FriendPanelChangeManager(string whichPanel)
    {
        if(whichPanel == "friendlist")
        {
            FriendListPanel.SetActive(true);
            FriendAcceptPanel.SetActive(false);
        }
        else 
        {
            FriendListPanel.SetActive(false);
            FriendAcceptPanel.SetActive(true);
        }
    }

}
