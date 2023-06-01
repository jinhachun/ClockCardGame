using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class HpBar : MonoBehaviour
{
    [SerializeField] protected GameObject _hpBarPrefab;
    [SerializeField] protected GameObject _shieldPrefab;
    [SerializeField] protected TMP_Text _hptxtPrefab;
    [SerializeField] protected TMP_Text _shieldtxtPrefab;
    protected virtual int Hp => BattleManager.Instance.Hp;
    protected virtual int MHp => BattleManager.Instance.Mhp;
    protected virtual int Shield => BattleManager.Instance.Shield;
    public int MaxShield = 100;

    protected float HpScale => (float)Hp / (float)MHp;
    protected float ShieldScale =>((float) Shield / (float) MaxShield)> MaxShield ? MaxShield : ((float)Shield / (float)MaxShield); 
    [SerializeField] protected bool OnOff;

    protected void ScaleChange(GameObject g,float a)
    {
        g.transform.localScale = new Vector2(g.transform.localScale.x + a, g.transform.localScale.y);
    }
    public void ScaleSet(float a,GameObject b)
    {
        var localscalex = b.transform.localScale.x;
        if (a != localscalex)
        {
            if (Mathf.Abs(a - localscalex) <= 0.03f)
            {
                b.transform.localScale = new Vector2(a, b.transform.localScale.y);
                return;
            }
            float f;
            if (a > localscalex) f = 0.03f;
            else f = -0.03f;
            ScaleChange(b, f);
        }
    }
    public void textChange(TMP_Text txt, int a)
    {
        if (!txt.text.Equals(a.ToString())) txt.text = a.ToString();
    }
    
    public void FixedUpdate()
    {
        if (!OnOff) return;
        textChange(_hptxtPrefab, Hp);
        textChange(_shieldtxtPrefab, Shield);
        ScaleSet(HpScale, _hpBarPrefab);
        ScaleSet(ShieldScale, _shieldPrefab);
    }
}
