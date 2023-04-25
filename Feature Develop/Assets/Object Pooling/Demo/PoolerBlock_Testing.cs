using UnityEngine;

public class PoolerBlock_Testing : MonoBehaviour
{
	[SerializeField] float lifetime;

	void OnEnable() => Invoke("AutoDeactive", lifetime);

	void AutoDeactive() => gameObject.SetActive(false);
}
