using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInterface : MonoBehaviour
{
    public void HideUi()
    {
        this.gameObject.SetActive(false);
    }

    public void ShowUi()
    {
        this.gameObject.SetActive(true);
    }
}
