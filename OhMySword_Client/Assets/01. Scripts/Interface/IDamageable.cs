using UnityEngine;

public interface IDamageable
{
	public void OnDamage(int damage, GameObject performer, Vector3 point);
}
