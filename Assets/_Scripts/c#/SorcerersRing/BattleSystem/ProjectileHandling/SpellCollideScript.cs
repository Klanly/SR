using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpellCollideScript : MonoBehaviour {
	
	public const string PLAYER = "Player";
	public const string ENEMY = "Enemy";
	public const string WARD = "Ward";
	public const string PLAYER_PROJECTILE = "Projectile";
	public const string ENEMY_PROJECTILE = "EnemyProjectile";
	public const string CHARGED_PROJECTILE = "Charged Projectile";
	
	public GameObject enemyHitExplosionPrefab;
	public GameObject wardHitExplosionPrefab;
	public GameObject projectileHitExplosionPrefab;
	
	GameObject enemyHitInstance;
	GameObject explosionInstance;
	//GameObject projectileHitInstance;
	static bool isNeutralCollission = false;
	
	BattleManager battleManager;
	
	ProjectileStatsComparator thisProjectileStatComparator;
	
	void Start () {
	
		battleManager = GameManager.instance._levelManager.battleManager;
		
		thisProjectileStatComparator = this.gameObject.GetComponent<ProjectileStatsComparator>();
	}
	
	protected void OnTriggerEnter(Collider other) 
	{	
		if(this.collider.CompareTag(PLAYER_PROJECTILE))
		{
			if(other.collider.CompareTag(ENEMY))
			{
				enemyHitInstance = GameObject.Instantiate(enemyHitExplosionPrefab, this.transform.position,other.transform.rotation) as GameObject;
			
				if(battleManager != null)
					battleManager.OnGameobjectCollided(other.gameObject, thisProjectileStatComparator, true);
				
				if(this.gameObject != null)
				{
					StartCoroutine(ExplodeAndDestroyProjectile(this.gameObject, null));
				}
				return;
			}
			else if(other.collider.CompareTag(ENEMY_PROJECTILE))
			{
				ProjectileStatsComparator thisProjectileStatComparator = this.gameObject.GetComponent<ProjectileStatsComparator>();
				ProjectileStatsComparator thatProjectileStatComparator = other.gameObject.GetComponent<ProjectileStatsComparator>();
				
				if(thisProjectileStatComparator.IsSame(thatProjectileStatComparator))
				{
					isNeutralCollission = false;
					if(battleManager != null)
						battleManager.OnGameobjectCollided(null, null);
					StartCoroutine(ExplodeAndDestroyProjectile(other.gameObject, null));
			
					if(this.gameObject != null)
					{
						StartCoroutine(ExplodeAndDestroyProjectile(this.gameObject, projectileHitExplosionPrefab));
					}
				}
				else if(thisProjectileStatComparator.IsNeutral(thatProjectileStatComparator))
				{
					isNeutralCollission = true;
					if(battleManager != null)
						battleManager.OnGameobjectCollided(null, null, false);
				}
				else if(thisProjectileStatComparator.IsCounter(thatProjectileStatComparator))
				{
					isNeutralCollission = false;
					if(this.gameObject != null)
					{
						StartCoroutine(ExplodeAndDestroyProjectile(this.gameObject, projectileHitExplosionPrefab));
					}
				}
				else if(thatProjectileStatComparator.IsCounter(thisProjectileStatComparator))
				{
					isNeutralCollission = false;
					if(other.gameObject != null)
					{
						StartCoroutine(ExplodeAndDestroyProjectile(other.gameObject, projectileHitExplosionPrefab));
					}
				}
			}
			else if(other.collider.CompareTag(WARD))
			{
				if(battleManager != null)
					battleManager.OnGameobjectCollided(other.gameObject, null);
				StartCoroutine(ExplodeAndDestroyProjectile(this.gameObject, projectileHitExplosionPrefab));
			}
		}
		else if(this.collider.CompareTag(CHARGED_PROJECTILE))
		{
			if(other.collider.CompareTag(ENEMY))
			{
				enemyHitInstance = GameObject.Instantiate(enemyHitExplosionPrefab, this.transform.position,other.transform.rotation) as GameObject;
			
				if(GameManager.PRINT_LOGS) Debug.Log("BM CALLED -------------------------------------------------------------------------------------- 3");
				if(battleManager != null)
					battleManager.OnGameobjectCollided(other.gameObject, thisProjectileStatComparator, true);
				
				if(this.gameObject != null)
				{
					StartCoroutine(ExplodeAndDestroyProjectile(this.gameObject, null));
				}
				return;
			}
			else if(other.collider.CompareTag(ENEMY_PROJECTILE))
			{
				///////////////////
				
				isNeutralCollission = false;
				if(other.gameObject != null)
				{
					StartCoroutine(ExplodeAndDestroyProjectile(other.gameObject, projectileHitExplosionPrefab));
				}
			}
			else if(other.collider.CompareTag(WARD))
			{
				StartCoroutine(ExplodeAndDestroyProjectile(other.gameObject, wardHitExplosionPrefab));
			}
		}
		else if(this.collider.CompareTag(ENEMY_PROJECTILE))
		{
			if(other.collider.CompareTag(PLAYER))
			{
				enemyHitInstance = GameObject.Instantiate(enemyHitExplosionPrefab, this.transform.position,other.transform.rotation) as GameObject;
				if(battleManager != null)
					battleManager.OnGameobjectCollided(other.gameObject, thisProjectileStatComparator, true);
				if(this.gameObject != null)
				{
					StartCoroutine(ExplodeAndDestroyProjectile(this.gameObject, null));
				}
				return;
				
			}
		}
	}
			
	private IEnumerator ExplodeAndDestroyProjectile(GameObject objectToDestroy, GameObject explosionPrefab, System.Action cb = null)
	{
		if(explosionPrefab != null)
			explosionInstance = GameObject.Instantiate(explosionPrefab, objectToDestroy.transform.position,objectToDestroy.transform.rotation) as GameObject;
		yield return null;
		if((battleManager != null) && battleManager._characterController.livingProjectiles.Contains(objectToDestroy))
			battleManager._characterController.livingProjectiles.Remove(objectToDestroy);
		GameObject.Destroy(objectToDestroy);
		//yield return new WaitForSeconds(1.0f);
		//if(explosionInstance!=null)
		//	Destroy(explosionInstance);
		
		//if(enemyHitInstance!=null)
		//	Destroy(enemyHitInstance);
		if(cb != null)
			cb();
	}
}
