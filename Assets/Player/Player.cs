using UnityEngine;

public class Player : MonoBehaviour {
    
    public static PlayerHealth health;
    public static PlayerMovement movement;
    public static PlayerGun gun;
    public static PlayerLook look;
    public static PlayerWeapon weapon;

    void Awake() {
        health = GetComponent<PlayerHealth>();
        movement = GetComponent<PlayerMovement>();
        gun = GetComponent<PlayerGun>();
        look = GetComponent<PlayerLook>();
        weapon = GetComponent<PlayerWeapon>();
    }

    public bool IsPlayer(GameObject go) {
        return gameObject == go;
    }

}