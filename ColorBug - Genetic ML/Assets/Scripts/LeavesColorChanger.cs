using UnityEngine;

public class LeavesColorChanger : MonoBehaviour
{
    void Start()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
    }  
}
