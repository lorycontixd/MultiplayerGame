using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    public List<Ability> abilities;

    private void Start()
    {
    }

    public void AssignAbility(int key, Ability ability)
    {
        if (key != 0 && key != 1)
        {
            return;
        }
        else
        {
            abilities[key] = ability;
        }
    } 

    public void Update()
    {
        if (abilities.Count > 2)
        {
            Debug.LogError($"Abilities cannot be more than 2, but are {abilities.Count}");
        }
        for (int i=0;i<abilities.Count;i++)
        {
            Ability ability;
            try
            {
                ability = abilities[i];
            }catch
            {
                Debug.Log("fail");
                continue;
            }
            switch (ability.state)
            {
                case AbilityState.READY:
                    {
                        if (Input.GetMouseButtonDown(i))
                        {
                            ability.Fire(gameObject);
                            ability.state = AbilityState.ACTIVE;
                        }
                    }
                    break;
                case AbilityState.ACTIVE:
                    {
                        if (ability.activeTimer > 0)
                        {
                            ability.activeTimer -= Time.deltaTime;
                        }
                        else
                        {
                            ability.state = AbilityState.COOLDOWN;
                            ability.cooldownTimer = ability.Cooldown.Value;
                        }
                    }
                    break;
                case AbilityState.COOLDOWN:
                    {
                        if (ability.cooldownTimer > 0)
                        {
                            ability.cooldownTimer -= Time.deltaTime;
                        }
                        else
                        {
                            ability.state = AbilityState.READY;
                        }
                    }
                    break;
            }
        }
    }
}
