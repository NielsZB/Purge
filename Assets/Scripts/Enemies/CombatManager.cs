using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [SerializeField] List<GameObject> combatEntities;
    [SerializeField] BoolVariable combatState;

    private void Start()
    {
        Cursor.visible = false;
        for (int i = 0; i < combatEntities.Count; i++)
        {
            combatEntities[i].SetActive(combatState.State);
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            combatState.SetState(!combatState.State);
            for (int i = 0; i < combatEntities.Count; i++)
            {
                combatEntities[i].SetActive(combatState.State);
            }
        }
    }
    public void AddEntity(GameObject entity)
    {
        combatEntities.Add(entity);
    }
}
