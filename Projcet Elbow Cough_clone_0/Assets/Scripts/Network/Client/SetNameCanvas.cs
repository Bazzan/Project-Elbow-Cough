using Mirror;
using TMPro;
using UnityEngine;

public class SetNameCanvas : MonoBehaviour
{
    [SerializeField] private TMP_InputField input;

    private string lastValue = string.Empty;

    private void Update()
    {
        CheckSetName();
    }

    private void CheckSetName()
    {
        if (!NetworkClient.active) return;

        ClientInstance ci = ClientInstance.ReturnClientInstance();
       
        if (ci == null)
        {
            lastValue = string.Empty;
            return;
        }

        if (input.text != lastValue)
        {
            lastValue = input.text;
            
            Debug.Log(ci.playerPrefab.name);
            // Debug.Log(input.text);
            ci.SetName(input.text);

        }
    }
}