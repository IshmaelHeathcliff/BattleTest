using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadScene : MonoBehaviour
{
    public void OnClick()
    {
        SceneController.Instance.Reload();
    }
}
