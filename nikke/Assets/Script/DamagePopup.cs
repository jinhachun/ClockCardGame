using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class DamagePopup : MonoBehaviour
{
     
    public static DamagePopup Create(Vector3 position,int damage,bool critical)
    {
        Vector3 pos = new Vector3(position.x + Random.Range(-2, 2), position.y + Random.Range(-1, 1), 0f);
        Transform damagePopupTransform = Instantiate(BattleManager.Instance._damagePopupPrefab, pos, Quaternion.identity);

        DamagePopup damagePopup = damagePopupTransform.GetComponent<DamagePopup>();
        damagePopup.Setup(damage,critical);

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
    public void Setup(int dam,bool critical)
    {
        if (textMesh == null) Debug.Log("??");
        textMesh.text = (dam+"");
        if (critical)
        {
            textMesh.fontSize = 14f;
            txtColor = Color.red;
        }
        else
        {
            textMesh.fontSize = 7f;
            txtColor = Color.white;
        }
        textMesh.color = txtColor;
        disappearTimer = 0.5f;
    }
    public void Update()
    {
        float moveSpd = 3f;

        transform.position =(new Vector2(transform.position.x, transform.position.y+moveSpd * Time.deltaTime) );

        disappearTimer -= Time.deltaTime;
        if (disappearTimer < 0)
        {
            float disappearSpd = 3f;
            txtColor.a -= disappearSpd * Time.deltaTime;
            textMesh.color = txtColor;
            if (txtColor.a <= 0)
            {
                Destroy(gameObject);
            }
        }
        
    }
}
