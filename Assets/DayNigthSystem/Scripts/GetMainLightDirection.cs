using UnityEngine;

[ExecuteInEditMode]
public class GetMainLightDirection : MonoBehaviour
{
    [SerializeField] private Material skyboxMat;

    void Update()
    {
        skyboxMat.SetVector(name = "_MainLightDirection", transform.forward);
    }
}
