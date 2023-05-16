using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class HpBar : MonoBehaviour
{
    [SerializeField] GameObject _hpBarPrefab;
    [SerializeField] GameObject _shieldPrefab;
    [SerializeField] TMP_Text _hptxtPrefab;
    [SerializeField] TMP_Text _shieldtxtPrefab;
    private int Hp => BattleManager.Instance.Hp;
    private int MHp => BattleManager.Instance.Mhp;
    private int Shield=>BattleManager.Instance.Shield;
    public int MaxShield = 100;

    private float HpScale => (float)Hp / (float)MHp;
    private float ShieldScale => (float)Shield / (float)MaxShield;

    private void ScaleChange(GameObject g,float a)
    {
        g.transform.localScale = new Vector2(g.transform.localScale.x + a, g.transform.localScale.y);
    }
    public void ScaleSet(float a,GameObject b)
    {
        var localscalex = b.transform.localScale.x;
        if (a != localscalex)
        {
            Debug.Log(a + "," + localscalex);
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
        textChange(_hptxtPrefab, Hp);
        textChange(_shieldtxtPrefab, Shield);
        ScaleSet(HpScale, _hpBarPrefab);
        ScaleSet(ShieldScale, _shieldPrefab);
    }
}
