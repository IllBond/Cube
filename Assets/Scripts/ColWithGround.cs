using System.Collections.Specialized;
using UnityEngine;

public class ColWithGround : MonoBehaviour
{

    public GameObject restartButton;
    public GameObject explosion; // тут лежит particle / частицы

    private void OnCollisionEnter(Collision collision) {
        
        if (collision.gameObject.tag == "cubes") {
            for (int i = collision.transform.childCount - 1; i >= 0; i--) {
                Transform child = collision.transform.GetChild(i);
                child.gameObject.AddComponent<Rigidbody>();
                
                child.gameObject.GetComponent<Rigidbody>().AddExplosionForce(150f, Vector3.up, 20f);
                child.SetParent(null);
            }
            restartButton.SetActive(true);
            Destroy(collision.gameObject);
            if (PlayerPrefs.GetString("music") == "on")
            {
                GetComponent<AudioSource>().Play(); // воспроизвести звук котоырй перенсли в обьект, если музыка в игре включена
            }
            // Это компонент сотрясения камеры
            Camera.main.gameObject.AddComponent<CameraShaker>();
            //Добавляем частицы
            
            Instantiate(
                explosion, // Говорим какую частицу добавить
                new Vector3(
                    collision.contacts[0].point.x, // [0] первая точка контакта по координате x
                    collision.contacts[0].point.y, // ...
                    collision.contacts[0].point.z // ...
                ),
                Quaternion.identity // Вроде бы делает что бы не вращалась ничего
                );
        }
    }

}
