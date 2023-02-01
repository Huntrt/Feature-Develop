using UnityEngine;

public class InGamePrint_Testing : MonoBehaviour
{
    public void Testing()
    {
        InGamePrint.i.Print(Random.Range(-100000,100000));
    }
}