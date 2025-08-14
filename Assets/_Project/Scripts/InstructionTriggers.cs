using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionTriggers : MonoBehaviour
{

    [SerializeField] GameObject[] instToActivate;
    private const string PLAYER_TAG = "Player";
    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject instruction in instToActivate)
        {
            instruction.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(PLAYER_TAG))
        {
            foreach (GameObject instruction in instToActivate)
            {
                instruction.SetActive(true);
            }
        }

        return;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(PLAYER_TAG))
        {
            foreach (GameObject instruction in instToActivate)
            {
                instruction.SetActive(false);
            }
        }

        return;
    }
}
