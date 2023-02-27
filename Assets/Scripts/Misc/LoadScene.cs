using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadScene : MonoBehaviour
{
    public void _LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void _LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }
}
