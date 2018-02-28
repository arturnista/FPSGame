using UnityEngine;

public class EnemyMovement : PhysicEntity {

	[SerializeField]
    protected bool m_IsFollowingPlayer = false;

    protected virtual void Awake() {
        base.Awake();
    }

    public virtual void TakeDamage(float damage, string name) {

    }
    
    public virtual void NoticePlayer() {
		m_IsFollowingPlayer = true;
	}
}
