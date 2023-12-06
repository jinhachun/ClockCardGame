using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class DamagePopup : MonoBehaviour
{
     
    public static DamagePopup Create(Vector3 position,int damage,bool critical,bool lethal)
    {
        Vector3 pos = new Vector3(position.x + Random.Range(-2, 2), position.y + Random.Range(-1, 1), 0f);
        Transform damagePopupTransform = Instantiate(BattleManager.Instance._damagePopupPrefab, pos, Quaternion.identity);

        DamagePopup damagePopup = damagePopupTransform.GetComponent<DamagePopup>();
        damagePopup.Setup(damage,critical,lethal);

        return damagePopup;
    }
    public static DamagePopup Create(Vector3 position, string damage, Color color)
    {
        Vector3 pos = new Vector3(position.x + Random.Range(-2, 2), position.y + Random.Range(-1, 1), 0f);
        Transform damagePopupTransform = Instantiate(BattleManager.Instance._damagePopupPrefab, pos, Quaternion.identity);

        DamagePopup damagePopup = damagePopupTransform.GetComponent<DamagePopup>();
        damagePopup.Setup(damage, color);

        return damagePopup;
    }
    private TMP_Text textMesh;
    private float disappearTimer;
    private Color txtColor;
    // Start is called before the first frame update
    void Awake()
    {
        textMesh = GetComponent<TMP_Text>();    
    }

    // Update is called once per frame
    public void Setup(int dam,bool critical,bool lethal)
    {
        float randomScale = Random.Range(80, 120);
        if (textMesh == null) Debug.Log("??");
        textMesh.text = (dam+"");
        if (critical)
        {
            textMesh.fontSize = 10f*randomScale/100;
            txtColor = Color.red;
        }
        else if (lethal)
        {
            textMesh.fontSize = 5f * randomScale / 100; ;
            txtColor = Color.blue;
        }
        else
        {
            textMesh.fontSize = 5f * randomScale / 100; ;
            txtColor = Color.white;
        }
        textMesh.color = txtColor;
        disappearTimer = 0.5f;
    }
    public void Setup(string dam, Color color)
    {
        float randomScale = Random.Range(80, 120);
        if (textMesh == null) Debug.Log("??");
        textMesh.text = (dam);
        
        textMesh.fontSize = 5f * randomScale / 100; ;
        txtColor = color;
        textMesh.color = txtColor;
        disappearTimer = 0.5f;
    }
    public void Update()
    {
        float moveSpd = 2f;

        transform.position =(new Vector2(transform.position.x, transform.position.y+moveSpd * Time.deltaTime) );

        disappearTimer -= Time.deltaTime;
        if (disappearTimer < 0)
        {
            float disappearSpd = 2f;
            txtColor.a -= disappearSpd * Time.deltaTime;
            textMesh.color = txtColor;
            if (txtColor.a <= 0)
            {
                Destroy(gameObject);
            }
        }
        
    }
}
