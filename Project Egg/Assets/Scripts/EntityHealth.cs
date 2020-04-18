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
    private bool isDead;

    // Start is called before the first frame update
    void Start()
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

    // Update is called once per frame
    void Update()
    {
        if (entityCurrentHealth <= 0 && !isDead)
        {
            entityCurrentHealth = 0;
            isDead = true;
            Destroy(this.gameObject);
        }
    }

    void OnCollisionEnter(Collision col)
    {
        print("This " + this.transform.name + " has collided with Col " + col.transform.name);
        if (col.gameObject.GetComponent<Rigidbody>() != null)
        {
            if (col.gameObject.GetComponent<EntityHealth>() != null)
            {
                float kineticColDamage = KineticEnergy(col.gameObject.GetComponent<Rigidbody>());
                print("KineticColDamage is " + kineticColDamage);
                if (kineticColDamage > damageThreshold)
                {
                    entityCurrentHealth = entityCurrentHealth - (Mathf.RoundToInt(kineticColDamage - damageThreshold));
                    print(this.transform.name + " took physics damage and now is at " + entityCurrentHealth + " hp.");
                }
            }
        }

        float kineticDamage = KineticEnergy(this.transform.GetComponent<Rigidbody>());
        print("KineticDamage is " + kineticDamage);
        if (kineticDamage > damageThreshold)
        {
            entityCurrentHealth = entityCurrentHealth - (Mathf.RoundToInt(kineticDamage - damageThreshold));
            print(this.transform.name + " took physics damage and now is at " + entityCurrentHealth + " hp.");
        }
    }
    // CALCULATE PHYSIC DAMAGE
    public static float KineticEnergy(Rigidbody rb)
    {
        // mass in kg, velocity in meters per second, result is joules
        return 0.5f * rb.mass * Mathf.Pow(rb.velocity.magnitude, 2);
    }
}
