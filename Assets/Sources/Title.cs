using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public void Player1Button()
    {
        Manager.player[1] = Manager.PLAYTYPE.UsingHand;
    }
    public void Player2Button()
    {
        Manager.player[0] = Manager.PLAYTYPE.UsingHand;
    }
    // Update is called once per frame
    void Update()
    {
        if(Manager.player[0] != Manager.PLAYTYPE.Empty && Manager.player[1] != Manager.PLAYTYPE.Empty)
        {
            SceneManager.LoadScene("Main");
        }
    }
}
