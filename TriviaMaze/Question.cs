using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriviaMaze
{
	class Question
	{
		private string question, answer, opt1, opt2, opt3;

		public Question(int key)
		{
			question = "Default";
			answer = "Right";
			opt1 = "Wrong";
			opt2 = "Wrong";
			opt3 = "Wrong";
		}
	}
}
