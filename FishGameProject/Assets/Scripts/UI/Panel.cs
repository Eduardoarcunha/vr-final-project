using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Panel : MonoBehaviour
{
    virtual public void OpenPanel()
    {
        gameObject.SetActive(true);
    }
    virtual public void ClosePanel()
    {
        gameObject.SetActive(false);
    }
}
