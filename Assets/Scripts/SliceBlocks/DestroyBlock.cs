
using UnityEngine;

public class DestroyBlock : MonoBehaviour
{
 private void OnCollisionEnter(Collision other)
 {
  Destroy(other.gameObject);
 }
}
