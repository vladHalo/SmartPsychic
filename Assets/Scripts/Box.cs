using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
	class Box
	{
		public string name;
		public GameObject typeBox;
		public TypeFood typeFood;
		public Animator animator;
		public string nameAnim;

		public Box()
		{
			name = "";
			typeBox = null;
			typeFood = TypeFood.none;
			animator = null;
			nameAnim = "";
		}
	}

	enum TypeFood
	{
		banana,
		burger,
		orange,
		none
	}
}
