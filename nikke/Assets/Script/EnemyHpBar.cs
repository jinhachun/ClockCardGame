using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class EnemyHpBar : HpBar
{
    [SerializeField] TMPro.TMP_Text text_Name;
    public Enemy enemy;
    protected override int Hp => enemy._hp;
    protected override int MHp => enemy._mhp;
    protected override int Shield => enemy._shield;

    public void Set(Enemy enemy)
    {
        this.enemy = enemy;
        text_Name.text = enemy._name;
        OnOff = true;
    }
    
}
