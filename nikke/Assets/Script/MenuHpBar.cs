using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuHpBar : HpBar
{
    protected override int Hp => Resource.Instance.Hp;
    protected override int MHp => Resource.Instance.mHp;
    protected override int Shield => 100;
}
