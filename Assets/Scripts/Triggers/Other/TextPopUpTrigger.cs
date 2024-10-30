using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextPopUpTrigger : MonoBehaviour
{

    [Header("Text")]
    [SerializeField] private GameObject text;
    public string textToDisplay;

    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<SpriteRenderer>().enabled = false;
        text.SetActive(false);
        text.GetComponent<TMP_Text>().text = textToDisplay;   
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
            text.SetActive(true);   
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
            text.SetActive(false);
    }
}
