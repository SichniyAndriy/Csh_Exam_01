using System.Text;
namespace ConsoleApp1
{
	public class QuizSection
	{
		public string Question { get; set; }
		public string WrigthAnswer { get; set; }
		public string Description {get; set; }
		public List<string> VarAnswers { get; set; }

		public QuizSection() : this("", "", "", new List<string>())
		{ }
		public QuizSection(string question, string wrigthAnsw, string description) :
			this(question, wrigthAnsw, description, new List<string>())
		{ }
		public QuizSection(string question, string wrigthAnsw, string description, List<string> arr)
		{
			Question = question;
			WrigthAnswer = wrigthAnsw;
			Description = description;
			VarAnswers = arr;
		}
		public override string ToString()
		{
			StringBuilder tmp = new();
			_ = tmp.Append(Question).Append(":\n");
			int count = 0;
			foreach(string el in VarAnswers)
			{
				_ = tmp.Append(++count).Append(". ").Append(el).Append('\n');
			}
			return tmp.ToString();
		}

		public bool IsCorect(int x)
		{
			return x >= 0 && x <= VarAnswers.Count - 1 && 
				VarAnswers[x].Equals(WrigthAnswer, StringComparison.Ordinal);
		}
	}
}
