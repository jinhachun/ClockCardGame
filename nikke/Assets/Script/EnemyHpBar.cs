using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class EnemyHpBar : HpBar
{
    [SerializeField] TMPro.TMP_Text text_Name;
    [SerializeField] TMPro.TMP_Text text_Pattern;
    public void setText_Pattern(string a,Color color) { text_Pattern.text = a; text_Pattern.color = color; }

    public Enemy enemy;
    protected override int Hp => enemy._hp;
    protected override int MHp => enemy._mhp;
    protected override int Shield => enemy._shield;


    public void Set(Enemy enemy)
    {
        this.enemy = enemy;
        enemy._hpBar = this;
        text_Name.text = enemy._name;
        OnOff = true;
    }
    public void SetName(string name)
    {
        text_Name.text = name;
    }
}
