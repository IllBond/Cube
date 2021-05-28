using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public class GameController : MonoBehaviour
{
    private CubePos nowCube = new CubePos(0,1,0); //Координаты начального блока
    public float cubeChangePlaceSpeed = 0.5f; //Скорость

    public Transform cubeToPlace; // Сюда перетащим игровой обьект CubeToPlace То что присвоили в Game Controller  

    public GameObject[] cubeToCreator; // Сюда перетащим игровой обьект Cub
    public GameObject allCubes; // Сюда перетащим игровой обьект AllCubes
    public GameObject vfx; // ??? обьект нужный для эффектов с частицами

    private Rigidbody allCubesRB; // Вспомогательная переменная. Позже сдесь будем перезагружать значение InCenematic у обьекта котоырй хранит в себе все кубы

    private bool isLose = false; // Вспомогательная переменная проверки проигрыша
    private Coroutine showCubePlace; // Переменная для записи в себя куратины и для последующей остановки
    public Transform mainCamera; //Сюда перемещаем обьект камеры // Так же есть переменная mainCame это альтернативынй вариант

    private bool hiddenMenu = false;
    public GameObject[] menuItems;
    public Text scoreText;


    public GameObject buttons1; 
    public GameObject buttons2; 
    public GameObject buttons3; 
    public GameObject buttons4; 
    public GameObject buttons5;
    public GameObject buttons6;

    // Тут добавили цвета для заднего фона
    public Color[] bgColors;
    // Цвет в который перекраситься камера
    private Color toCameraColor;
    private List<GameObject> cubeToCreatorList = new List<GameObject>();
    private float camMoveToYposition; // Переменная содержит знание на сколько поднять камеру, что бы быть с блоком на одном уровне
    private float camMoveSpeed = 2f; // Переменная содержит знание на сколько поднять камеру, что бы быть с блоком на одном уровне
    private Transform mainCam;
    // Координаты места в пространстве которые заняты. Сюда же будут добавляться коордидинаты кубов которые мы установили
    private List<Vector3> allCubesPositions = new List<Vector3> {
        new Vector3(0,0,0),
        new Vector3(1,0,0),
        new Vector3(-1,0,0),
        new Vector3(0,1,0),
        new Vector3(0,0,1),
        new Vector3(0,0,-1),
        new Vector3(1,0,1),
        new Vector3(1,0,-1),
        new Vector3(-1,0,-1),
        new Vector3(-1,0,1),
        };


    private int prevCountMaxHirizontal;

    private void Start()
    {
        scoreText.text = "Лучший: " + PlayerPrefs.GetInt("score") + " \n Текущий: 0";
        // Задаем текущее значение для камеры. При старте
        toCameraColor = Camera.main.backgroundColor;

        mainCam = Camera.main.transform; //Находим обьект камеры. Есть алтернатива mainCamera
        camMoveToYposition = 4 + nowCube.y - 1; // первая цйифра это первоночальное положене камеры //Вторая цифра координата кубика по y //3 координата это зарезервированное положение т.к по умолчанию значение уже еденица
            //Альтернативный способ
            //camMoveToYposition = nowCube.y - 1;
            //mainCamera.position = new Vector3(mainCamera.position.x, camMoveToYposition, mainCamera.position.z);


        // При старте программы в allCubesRB добавиться Rigidbody из allCubes (Обьект который мы перетащили в скрипт)
        allCubesRB = allCubes.GetComponent<Rigidbody>();
        // Стартуем куратину. Через функциию 
        // Куранкцию которые будет делать перебор значений
        showCubePlace = StartCoroutine(ShowCubePlace());
       
    }
        
    private void Update()
    {
        if (
            (Input.GetMouseButtonDown(0) || Input.touchCount > 0) //Если кликнули левой кнопкой мышки или количество касаний по экрану больше одного
            && cubeToPlace != null //Если cubeToPlace существует
            && allCubes != null
            ) 
        {
                  
        //Проверка нажата ли кнопка меню или нет (если нажата не ставить блок)
        if (EventSystem.current.IsPointerOverGameObject()) {
                return;
            }

        //Если меню еще не скрыто, сработает
        if (hiddenMenu==false) {
                hiddenMenu = true; //Сделает что бы больше не проверяло скрыто ли меню
                foreach (GameObject item in menuItems) {
                    Destroy(item); // уничтожает обьекты меню. Их так же можно скрыть
                }
            }



        //Какая то проверка связаная с Юнити. Примерно: Если не считалось никакое нажатие то завершаем функцию
        #if !UNITY_EDITOR
                    if (Input.GetTouch(0).phase != TouchPhase.Began)
                        return;
#endif
            if (PlayerPrefs.GetInt("score") < 2) {
                    isShowencube(0);
            } else if (PlayerPrefs.GetInt("score") < 4) {
                isShowencube(1);
            } else if (PlayerPrefs.GetInt("score") < 8) {
                isShowencube(2);
            } else if (PlayerPrefs.GetInt("score") < 16) {
                isShowencube(3);
            } else if (PlayerPrefs.GetInt("score") < 20) {
                isShowencube(4);
            } else if (PlayerPrefs.GetInt("score") < 24) {
                isShowencube(5);
            } else if (PlayerPrefs.GetInt("score") < 36) {
                isShowencube(6);
            } else if (PlayerPrefs.GetInt("score") < 54) {
                isShowencube(7);
            } else {
                isShowencube(8);
            }

            //Создаем экземпляр 
            GameObject newCube = Instantiate(
                cubeToCreatorList[UnityEngine.Random.Range(0, cubeToCreatorList.Count)], //Какой обьект из unity
                cubeToPlace.position,  //Какиекоординаты
                Quaternion.identity // что то не понятное
                ) as GameObject;

            newCube.transform.SetParent(allCubes.transform); //Добавляем экземпляру родииеля
            nowCube.setVector(cubeToPlace.position); //Добавляем экземпляру родииеля

            if (PlayerPrefs.GetString("music") == "on")
            {
                GetComponent<AudioSource>().Play(); // воспроизвести звук котоырй перенсли в обьект, если музыка в игре включена
            }

            Instantiate(
                vfx, //Какой обьект в данном случае частица которую мы передали через публичную перменную
                newCube.transform.position, //поцзиция где создать
                Quaternion.identity); // что то не понятное

            allCubesPositions.Add(nowCube.getVector());  // Добавляем координаты текущего куба в список зщапрещеных позиций
            allCubesRB.isKinematic = true; //затычка что бы появилась физика
            allCubesRB.isKinematic = false; //затычка что бы появилась физика
            SpawnPositions(); // перезапускем функцию спавна 
            moveCameraChaneBg(); //Находим максимальный элкмент по всем трем координатам
        }


        //allCubesRB.velocity.magnitude > 0.1f если обьект с кубами начинает движение (падение) и переменная проигрыша false
        if ( !isLose && (allCubesRB.velocity.magnitude > 0.05f))
        {
            isLose = true; // Вспомогательный эллемент
            mainCamera.localScale += new Vector3(0, 0, 2f); //Отдалить камеру 
            Destroy(cubeToPlace.gameObject); // Уничтожаем полупрозрачный элемент, что бы не получалось установить другие кубы
            StopCoroutine(showCubePlace); // остановочка
            // UnityEngine.Debug.Write("Invalid object. ");
        }

        mainCam.localPosition = Vector3.MoveTowards(
            mainCam.localPosition,
            new Vector3(mainCam.localPosition.x, camMoveToYposition, mainCam.localPosition.z),
            camMoveSpeed * Time.deltaTime);

        // При клике по экрану, сразу ничего не покраситься. 
        // Если текущий цвет камеры такой же каким должен стать ничего не сработает
        // Иначе цвет камеры плавно поменяется с цвета текузего в цвет который мы задали в update
        if (Camera.main.backgroundColor != toCameraColor)
        {
            Camera.main.backgroundColor = Color.Lerp(Camera.main.backgroundColor, toCameraColor, Time.deltaTime / 1.5f);
        }
    }


    //Куратина представляет собой функцию которая будет повторяться раз в заданый отрезок времени
    IEnumerator ShowCubePlace()
    {
        while (true)
        {
            //1 раз в 0.5 сек вызывает ф-цию SpawnPositions
            SpawnPositions();
            yield return new WaitForSeconds(cubeChangePlaceSpeed);
        }
    }


    private void isShowencube(int amount) {
        if (amount <= 1) {
            cubeToCreatorList.Add(cubeToCreator[0]);
            cubeToCreatorList.Add(cubeToCreator[1]);
        } else {
            for (int i = 0; i < amount; i++)
            {
                cubeToCreatorList.Add(cubeToCreator[i]);
            }
        }
        
    }

    private void SpawnPositions()
    {

        //Пустой массив, позже сюда будут добавлены координаты потенциально пригодные для размещения блока
        //При каждом вызове функции SpawnPositions, positions будет становится пустым
        List<Vector3> positions = new List<Vector3>();


        //Проверяем доступна ли координата, если да то добавляем ее 
        if (IsPositionEmpty(new Vector3(nowCube.x + 1, nowCube.y, nowCube.z)) && (nowCube.x + 1 != cubeToPlace.position.x))
        {
            positions.Add(new Vector3(nowCube.x + 1, nowCube.y, nowCube.z));
        }
        if (IsPositionEmpty(new Vector3(nowCube.x - 1, nowCube.y, nowCube.z)) && (nowCube.x - 1 != cubeToPlace.position.x))
        {
            positions.Add(new Vector3(nowCube.x - 1, nowCube.y, nowCube.z));
        }
        if (IsPositionEmpty(new Vector3(nowCube.x, nowCube.y + 1, nowCube.z)) && (nowCube.y + 1 != cubeToPlace.position.y))
        {
            positions.Add(new Vector3(nowCube.x, nowCube.y + 1, nowCube.z));
        }
        if (IsPositionEmpty(new Vector3(nowCube.x, nowCube.y - 1, nowCube.z)) && (nowCube.y - 1 != cubeToPlace.position.y))
        {
            positions.Add(new Vector3(nowCube.x, nowCube.y - 1, nowCube.z));
        }
        if (IsPositionEmpty(new Vector3(nowCube.x, nowCube.y, nowCube.z + 1)) && (nowCube.z + 1 != cubeToPlace.position.z))
        {
            positions.Add(new Vector3(nowCube.x, nowCube.y, nowCube.z + 1));
        }
        if (IsPositionEmpty(new Vector3(nowCube.x, nowCube.y, nowCube.z - 1)) && (nowCube.z - 1 != cubeToPlace.position.z))
        {
            positions.Add(new Vector3(nowCube.x, nowCube.y, nowCube.z - 1));
        }


        if (positions.Count > 1)
        {
            cubeToPlace.position = positions[UnityEngine.Random.Range(0, positions.Count)];
        }
        else if (positions.Count == 0)
        {
            isLose = true;
        }
        else {
            cubeToPlace.position = positions[0];
        }
        

    }


    //Тут проверка с возвращением false / true на то занятя ли уже координата другим кубом
    private bool IsPositionEmpty(Vector3 targetPos)
    {

        
        if (targetPos.y == 0)
            return false;
        foreach (Vector3 pos in allCubesPositions)
        {

            
            if (pos.x == targetPos.x && pos.y == targetPos.y && pos.z == targetPos.z)
            {
                return false;
            }
        }
        return true;
    }


    //Находим максимальный элкмент по всем трем координатам
    private void moveCameraChaneBg() {
        int maxX = 0;
        int maxY = 0;
        int maxZ = 0;
        int maxHorizontal;


        foreach (Vector3 pos in allCubesPositions) {
            if (Mathf.Abs(Convert.ToInt32(pos.x)) > maxX) {
                maxX = Mathf.Abs(Convert.ToInt32(pos.x));
            } 
            if (Convert.ToInt32(pos.y) > maxY) {
                maxY = Convert.ToInt32(pos.y);
            }
            if (Mathf.Abs(Convert.ToInt32(pos.z)) > maxZ) {
                maxZ = Mathf.Abs(Convert.ToInt32(pos.z));
            }
        }

        //Записываем в очки максимальное значение котороые существовало когда либо
        if (PlayerPrefs.GetInt("score") < maxY) {
            PlayerPrefs.SetInt("score", maxY);
        }

        scoreText.text = "Лучший: " + PlayerPrefs.GetInt("score") + " \n Текущий: " + maxY;

        maxHorizontal = maxX > maxZ ? maxX : maxZ;

        camMoveToYposition = 4 + nowCube.y - 1; // первая цйифра это первоночальное положене камеры //Вторая цифра координата кубика по y //3 координата это зарезервированное положение т.к по умолчанию значение уже еденица
                                                // mainCamera.position = new Vector3(mainCamera.position.x, camMoveToYposition, mainCamera.position.z);

        //Если максимальная длина 3 то пора отдалить камеры.
        //Если предыдущий prevCountMaxHirizontal не такой же как текущий maxHorizontal то сработает. Иначе при достижении длины 3 камеру бесконечно будет отдаляться. А так она будет отдаляться только если 3 6 9 ...
        if (maxHorizontal % 3 == 0 && prevCountMaxHirizontal != maxHorizontal)
            {
            //меняем предыдущий prevCountMaxHorizontal
                prevCountMaxHirizontal = maxHorizontal;
            //Отдаляем камеру
                mainCam.localPosition -= new Vector3(0, 0, 2.5f);
            }

        // Зпаписывает в toCameraColor рандомный цвет
        toCameraColor = bgColors[UnityEngine.Random.Range(0, bgColors.Length)];

    }
}

struct CubePos
{
    public int x, y, z;

    public CubePos(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public Vector3 getVector() {
        return new Vector3(x, y, z);
    }

    public void setVector(Vector3 pos) {
        x = Convert.ToInt32(pos.x);
        y = Convert.ToInt32(pos.y);
        z = Convert.ToInt32(pos.z);
    }
}