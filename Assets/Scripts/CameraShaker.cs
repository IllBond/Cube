using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    private Transform camTransform;
    public float shakeeDur = 2f; //время сколько будет камеря трястись
    public float shakeAmount= 0.5f; //Сила тряски камеры
    public float decreaserFactor = 25f; //Скорость уменьшения тряски камеры

    private Vector3 originPos; // Тут камера изначально
       
    private void Start() {
        camTransform = GetComponent<Transform>();
        originPos = camTransform.localPosition; // Первоначальное положение камеры
    }

    private void Update() {
        if (shakeeDur > 0)
        {
            //Заставляем камеру трястись

            //Random.insideUnitySphere рандомные значения внутри сферы с радиусом 1
            camTransform.localPosition = originPos + Random.insideUnitSphere * shakeAmount;
            
            shakeeDur -= Time.deltaTime * decreaserFactor; // время постепернно уменьшается
        }
        else {

            //как только время 0 камера возвращаетс в оригинальную позицию
            shakeeDur = 0;
            camTransform.localPosition = originPos;
        }
    }


}
