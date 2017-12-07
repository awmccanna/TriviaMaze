using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriviaMaze
{
	class Question
	{
		private string theQuestion, answer, opt1, opt2, opt3;
		private bool correct, attempted;


		public Question()
		{
			TheQuestion = "Default";
			Answer = "Right";
			Opt1 = "Wrong";
			Opt2 = "Wrong";
			Opt3 = "Wrong";
			Correct = false;
			Attempted = false;
		}


		public Question(int key)
		{
			TheQuestion = "Default";
			Answer = "Right";
			Opt1 = "Wrong";
			Opt2 = "Wrong";
			Opt3 = "Wrong";
			Correct = false;
			Attempted = false;
		}

		public bool Correct { get => correct; set => correct = value; }
		public bool Attempted { get => attempted; set => attempted = value; }
		public string TheQuestion { get => theQuestion; set => theQuestion = value; }
		public string Answer { get => answer; set => answer = value; }
		public string Opt1 { get => opt1; set => opt1 = value; }
		public string Opt2 { get => opt2; set => opt2 = value; }
		public string Opt3 { get => opt3; set => opt3 = value; }
	}
}
