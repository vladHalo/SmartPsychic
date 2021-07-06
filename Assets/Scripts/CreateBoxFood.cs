using Assets.Scripts;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class CreateBoxFood : MonoBehaviour
{
	public GameObject timeLose;
	public Text lvlNumber;
	public Text quetion;
	public Camera camera;
	public float posBetweenObj;
	public GameObject btn;
	public Sprite[] sprites;

	public GameObject[] confetti;

	byte lvl;
	int boxWithFood;
	bool verifyOrange;
	byte[,] boxType;
	bool checkLvl2;
	TimeLose time;
	List<Box> listBox;
	GameObject smoke;

	void Start()
	{
		lvl = 0;
		checkLvl2 = false;
		verifyOrange = true;
		lvlNumber.text = $"Lvl {lvl + 1}";
		boxType = new byte[,] { { 0, 1, 3 }, { 0, 1, 2 }, { 0, 4, 3 } };
		listBox = new List<Box>();
		time = timeLose.GetComponent<TimeLose>();

		for (int i = 0; i < 5; i++)
		{
			Box box = new Box();
			box.name = "Cube";
			box.typeBox = transform.GetChild(i).gameObject;
			box.animator = box.typeBox.GetComponent<Animator>();
			listBox.Add(box);
		}
		listBox[3].name = "Rectangle";
		listBox[4].name = "Sphere";

		CreateBox();
	}

	void LateUpdate()
	{
		if (smoke != null)
			smoke.transform.rotation = Quaternion.identity;
	}

	void Update()
	{
		if (!time.Timer())
		{	
			listBox[boxWithFood].typeBox.SetActive(true);
			listBox[boxWithFood].animator.SetBool("check", true);
			listBox[0].typeBox.transform.GetChild(0).gameObject.SetActive(true);
			if (checkLvl2)
			{
				listBox[0].typeBox.transform.GetChild(1).gameObject.SetActive(true);
				listBox[0].typeBox.transform.GetChild(0).gameObject.SetActive(false);
			}	

			if (isPlaying(listBox[boxWithFood]))
				btn.SetActive(true);
		}
		else
		{
			if (OpenBox() == 1)
			{
				foreach (var i in confetti)
					i.SetActive(true);
					
				time.timeChange = 0;
				btn.GetComponent<Image>().sprite = sprites[1];
				lvl++;
			}
			else if (OpenBox() == 0)
				time.timeChange = 0;
			if (lvl > 2) lvl = 0;
		}
	}

	public void Refresh()
	{
		foreach (var i in confetti)
			i.SetActive(false);

		listBox[boxWithFood].animator.SetBool("check", false);
		time.timeChange = 5;
		btn.GetComponent<Image>().sprite = sprites[0];
		btn.SetActive(false);
		CreateBox();
	}

	bool isPlaying(Box box) => box.animator.GetCurrentAnimatorStateInfo(0).IsName(box.nameAnim)
		&& box.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1;

	public static void Shuffle(int[] arr)
	{
		for (int i = arr.Length - 1; i >= 1; i--)
		{
			int j = Random.Range(0,i + 1);

			int tmp = arr[j];
			arr[j] = arr[i];
			arr[i] = tmp;
		}
	}

	void CreateBox()
	{
		listBox[0].typeBox.transform.GetChild(1).gameObject.SetActive(false);
		checkLvl2 = false;
		foreach (var i in listBox)
			i.typeBox.SetActive(false);

		if (smoke != null)
		{
			smoke.SetActive(false);
			smoke = null;
		}
		
		int[] arr = { boxType[lvl, 0], boxType[lvl, 1], boxType[lvl, 2] };
		Shuffle(arr);

		float[] vecX = { 0, 0, 0 };
		if (arr[1] == 3 || lvl == 1)
			vecX[1] = 0;
		else
		{
			vecX[1] = 6;
			if (arr[0] != 3)
				vecX[1] *= -1;
		}
		vecX[0] = vecX[1] - listBox[arr[0]].typeBox.transform.localScale.x / 2 - listBox[arr[1]].typeBox.transform.localScale.x / 2 - posBetweenObj;
		vecX[2] = vecX[1] + listBox[arr[2]].typeBox.transform.localScale.x / 2 + listBox[arr[1]].typeBox.transform.localScale.x / 2 + posBetweenObj;

		if (arr[2] == 3)
			vecX[0] += 6;
		else if(arr[0] == 3)
			vecX[2] -= 6;
		for (int i = 0; i < 3; i++)
		{
			listBox[arr[i]].typeFood = TypeFood.none;
			listBox[arr[i]].nameAnim = "";
			listBox[arr[i]].typeBox.transform.position = new Vector3(vecX[i], listBox[arr[i]].typeBox.transform.position.y,listBox[arr[i]].typeBox.transform.position.z);
			listBox[arr[i]].typeBox.SetActive(true);
		}

		if (lvl == 0) boxWithFood = 3;
		else if (lvl == 1)
		{
			boxWithFood = Random.Range(0, 3);
			smoke = listBox[boxWithFood].typeBox.transform.GetChild(0).transform.GetChild(0).gameObject;
			smoke.SetActive(true);
			Vector2 vector = listBox[boxWithFood].typeBox.transform.position;
		}
		else
		{
			listBox[0].typeBox.transform.GetChild(0).gameObject.SetActive(false);
			boxWithFood = Random.Range(0, 10);
			if (boxWithFood > 3) boxWithFood = 4;
			else boxWithFood = 0;
		}
		if (verifyOrange && lvl == 2)
		{
			verifyOrange = false;
			boxWithFood = 0;
		}
		listBox[boxWithFood].typeFood = (TypeFood)lvl;
		listBox[boxWithFood].nameAnim = $"True{listBox[boxWithFood].name}";

		lvlNumber.text = $"Lvl {lvl + 1}";
		string line="";
		if (lvl == 2)
		{
			checkLvl2 = true;
			line = "n";
		}
		quetion.text = $"Can you guess where is a{line} {(TypeFood)lvl} ?";
	}

	int OpenBox()
	{
		if (Input.GetKeyDown(KeyCode.Mouse0))
		{
			RaycastHit hit;
			Ray ray = camera.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(ray, out hit))
			{
				int number = int.Parse(hit.transform.name);
				if (listBox[number].typeFood != TypeFood.none)
					return 1;
				else	
					return 0;
			}
		}
		return 2;
	}
}
