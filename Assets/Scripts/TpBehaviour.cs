using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TpBehaviour : MonoBehaviour
{
    [SerializeField] private string changeTo;
    // Lv1-2

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider tp)
    {
        if (tp.gameObject.tag == "Player")
            changeScene(changeTo);
    }

    void changeScene(string name)
    {
        SceneManager.LoadScene(name);
    }
}
