using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityHealth : MonoBehaviour
{
    public string entityName;

    public int entityMaxHealth;
    public int getEntityMaxHealth() { return entityMaxHealth; }

    public int entityCurrentHealth;
    public int getEntityCurrentHealth() { return entityCurrentHealth; }

    public float damageThreshold;
    public bool isPlayer;
    private bool isDead;

    // Start is called before the first frame update
    void Start()
    {
        if (!isPlayer)
        {
            if (entityName == null)
            {
                entityName = transform.gameObject.name;
            }
            else
            {
                transform.gameObject.name = entityName;
            }

            entityCurrentHealth = entityMaxHealth;
            isDead = false;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (entityCurrentHealth <= 0 && !isDead && !isPlayer)
        {
            entityCurrentHealth = 0;
            isDead = true;
            Destroy(this.gameObject);
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.GetComponent<Rigidbody>() != null)
        {
            float kineticColDamage = KineticEnergy(col.gameObject.GetComponent<Rigidbody>());
            print("KCE is " + kineticColDamage);
            if (col.gameObject.GetComponent<EntityHealth>() != null)
            {
                
                if (kineticColDamage > damageThreshold)
                {
                    entityCurrentHealth = entityCurrentHealth - (Mathf.RoundToInt(kineticColDamage - damageThreshold));
                }
            }
            else if(this.gameObject.GetComponent<EntityHealth>() != null)
            {
                if (isPlayer && PlayerSight.playerHoldingPosition.childCount > 0)
                {
                    if (kineticColDamage > damageThreshold)
                    {
                        print(" Player hit col object " + col.gameObject.name + " too hard for " + kineticColDamage + " vs " + damageThreshold + " has a rb");
                        PlayerSight.DropObject(PlayerSight.hit.transform.gameObject);
                    }
                }
            }
        }

        float kineticDamage = KineticEnergy(this.transform.GetComponent<Rigidbody>());
        print("KE is " + kineticDamage);
        if (kineticDamage > damageThreshold)
        {
            if (!isPlayer)
            {
                entityCurrentHealth = entityCurrentHealth - (Mathf.RoundToInt(kineticDamage - damageThreshold));
            }
            else if (isPlayer && PlayerSight.playerHoldingPosition.childCount > 0)
            {
                print(" Player hit the ground too hard for " + kineticDamage + " vs " + damageThreshold + " has a rb");
                PlayerSight.DropObject(PlayerSight.hit.transform.gameObject);
            }
        }
    }
    // CALCULATE PHYSIC DAMAGE
    public static float KineticEnergy(Rigidbody rb)
    {
        // mass in kg, velocity in meters per second, result is joules
        return 0.5f * rb.mass * Mathf.Pow(rb.velocity.magnitude, 2);
    }
}
