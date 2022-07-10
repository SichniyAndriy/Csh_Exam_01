using System.Text.Json;
using System.Collections;

namespace ConsoleApp1
{
	public static class Methods1
	{
		public static void ShowBegin(string login)
		{
			List<string> files = Methods.GetListQuizes();
			Console.Clear();
			Console.WriteLine("Оберіть вікторину:");

			List<QuizSection>? quizSections = new();
			int count = 0;
			foreach(string file in files)
			{
				Console.WriteLine(++count + ") " + 
					file.Split('.', StringSplitOptions.RemoveEmptyEntries).First());
			}
			int index = Methods.MakeChoice(files.Count);
			using (FileStream fs = new(files[index - 1], FileMode.Open))
			{
				quizSections = JsonSerializer.Deserialize<List<QuizSection>>(fs);
			}
			int result = StartQuiz(quizSections);
			using (StreamWriter fs = new(login + ".txt", true))
			{
				fs.WriteLine(files[index - 1].Split('.').First() + " - " + result +
							 ": " + DateTime.Now.ToString());
			}
		}

		public static int StartQuiz(List<QuizSection>? quizSections)
		{
			Console.Clear();
			List<int> numQuestion = new();
			int num;
			int result = 0;
			bool flag;
			Random rand = new();
			for (int i = 0; i < 4; ++i)
			{
				do
				{
					num = rand.Next(0, quizSections?.Count ?? 0);
				} while (numQuestion.Contains(num));
				numQuestion.Add(num);
				Console.Write("\n" + (i + 1) + ") " + quizSections?[num].ToString() + "Ваша відповідь: ");
				if (int.TryParse(Console.ReadLine(), out int var))
				{
					flag = quizSections?[num].IsCorect(var - 1) ?? false;
					Console.WriteLine(flag ? "Вірно.\n" + quizSections?[num].Description :
									  "Невірно.\n" + quizSections?[num].WrigthAnswer +
									  ": " + quizSections?[num].Description);
					if (flag) ++result;
				}
				else 
					Console.WriteLine("Неправильний ввід");
			}
			Console.WriteLine($"Ваш результат: {result}");
			return result;
		}

		internal static void ShowResults(string login)
		{
			string tmp = "";
			try
			{
				using StreamReader sr = new(login + ".txt");
				tmp = sr.ReadToEnd();
			}
			catch (Exception)
			{
				Console.Write("Статистика недоступна");
				return;
			}
			Console.Write(tmp);
		}

		public static void DeleteAccount(string login, string usersPath)
		{
			File.Delete(login + ".txt");
			Hashtable l_p = Methods.FillTable(usersPath);
			if (l_p.ContainsKey(login))
			{
				l_p.Remove(login);
				Console.WriteLine("Акаунт видалено");
			}
			else 
				Console.WriteLine("Нема такого акаунта");
			List<string>? lk = l_p.Keys as List<string>;
			List<string>? lp = l_p.Values as List<string>;
			using StreamWriter sw = new(usersPath);
			for (int i = 0; i < l_p.Count; ++i)
			{
				sw.WriteLine(lk?[i]);
				sw.WriteLine(lp?[i]);
			}
		}
	}
}
