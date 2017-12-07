using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriviaMaze
{
	class QuestionFactory
	{
		
		private Question[] bank = new Question[40];
		private int num;
		public QuestionFactory()
		{
			num = 0;
			
			for(int i = 0; i < 40; i++)
			{
				bank[i] = new Question(i);
			}
		}

		public Question GetQuestion()
		{
			if (num >= 40)
				throw new IndexOutOfRangeException("Ran out of questions!");
			return bank[num++];
		}

	}
}
