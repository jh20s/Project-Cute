using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Item
{

    private static string TRIGGER_BOMB = "Boom";
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        
        anim = transform.GetComponent<Animator>();
    }


    private void OnEnable()
    {
        StartCoroutine(WaitBomb(1f));

   
    }

    private IEnumerator WaitBomb(float _time)
    {
        yield return new WaitForSeconds(_time);

        anim.SetTrigger(TRIGGER_BOMB);
        GameObject obj = ObjectPoolManager.Instance.EnableGameObject("s99");        
        obj.GetComponent<Transform>().localScale = new Vector3(Scale, Scale, Scale);
        obj.GetComponent<Projectile>().Damage = Damage;
        obj.GetComponent<Projectile>().setEnable(gameObject.transform.position, gameObject.transform.position, 0);
        MusicManager.Instance.OneShotProjectileSound("s99");
        yield return new WaitForSeconds(0.8f);
        ObjectPoolManager.Instance.DisableGameObject(gameObject);
        if (obj.activeInHierarchy)
        {
            obj.GetComponent<Projectile>().setDisable();
            ObjectPoolManager.Instance.DisableGameObject(obj);
        }
    }
}
