using System.Text.Json;
using System.Collections;
namespace ConsoleApp1
{
	public static class Methods
	{
		public static int MakeChoice(int max)
		{
			int countTry = 3;
			int choice;
			do {
				Console.Write("Зробіть свій вибір: ");
				_ = int.TryParse(Console.ReadLine(), out choice);
				--countTry;
			} while((choice < 0 || choice > max) && countTry > 0);
			return choice;
		}

		public static Hashtable FillTable(string path)
		{
			using StreamReader sr = new(path);
			Hashtable l_p = new();
			while (!sr.EndOfStream)
			{
				l_p.Add(sr.ReadLine() ?? "", sr.ReadLine() ?? "");
			}
			return l_p;
		}

		public static string LogIn(string path)
		{
			string login;
			try
			{
				Hashtable l_p = FillTable(path);
				if (0 == l_p.Count)
				{
					Console.WriteLine("Немає облікових записів");
					return "";
				}

				int count = 3;
				bool flag = false;
				do
				{
					if (flag) Console.WriteLine("Нема такого логіна");
					Console.Write("Введіть логін: ");
					login = Console.ReadLine() ?? "";
					--count;
					flag = true;
				} while (!l_p.ContainsKey(login) && count > 0);
				if (0 == count) return "";

				string password;
				string? tmp;
				count = 3;
				flag = false;
				do
				{
					if (flag) Console.WriteLine("\nПароль не вірний");
					Console.Write("Введіть пароль: ");
					password = InputPassword();
					--count;
					flag = true;
					tmp = l_p[login] as string;
				} while (!(tmp ?? "").Equals(password, StringComparison.Ordinal) && count > 0);
				if (0 == count) return "";
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return "";
			}
			return login;
		}

		public static string CreateLogin(string path)
		{
			Hashtable l_p = FillTable(path);
			string login;
			int count = 3;
			bool flag = false;
			do{
				if (flag) Console.WriteLine("Такий логін існує. Спробуйте інший");
				Console.Write("Створіть новий логін: ");
				login = Console.ReadLine() ?? "";
				--count;
				flag = true;
			} while (l_p.ContainsKey(login) && count > 0);
			if (0 == count) return ""; 
			Console.Write("Створіть новий пароль: ");
			string password = InputPassword();
			string passwordCheck;
			count = 3;
			flag = false;
			do{
				if (flag) Console.WriteLine("Паролі не співпадають");
				Console.Write("Введіть пароль ще раз: ");
				passwordCheck = InputPassword();
				--count;
				flag = true;
			} while(!password.Equals(passwordCheck, StringComparison.Ordinal) && count > 0);
			if (0 == count) return "";
			using (StreamWriter sw = new(path, true))
			{
				sw.WriteLine(login);
				sw.WriteLine(password);
			}
			return login;
		}

		public static List<string> GetListQuizes()
		{
			string[] arrTmp = Directory.GetFiles(Directory.GetCurrentDirectory());
			List<string> files = new();
			for(int i = 0; i< arrTmp.Length; ++i)
			{
				arrTmp[i] = arrTmp[i].Split('\\').Last();
				if (arrTmp[i].EndsWith("json") &&
					arrTmp[i].Split('.', StringSplitOptions.RemoveEmptyEntries).ToList().Count == 2)
					files.Add(arrTmp[i]);
			}
			return files;
		}

		public static string InputPassword()
		{
			string password = "";
			ConsoleKeyInfo key = Console.ReadKey(true);
			while (key.KeyChar != '\r')
			{
				Console.Write('*');
				password += key.KeyChar;
				key = Console.ReadKey(true);
			}
			Console.WriteLine();
			return password;
		}

		public static void ReadNewTextFiles(string path = @"..\..\..\files")
		{
			string name;
			string[] tmp = Directory.GetFiles(path);
			List<string> quiziesList = new();
			foreach (string file in tmp)
			{
				if (file.EndsWith("txt")) quiziesList.Add(file);
			}

			string? question;
			string? wrigthAnswer;
			string? description;
			string? var;
			List<string> variantsAnswer;
			List<QuizSection> quizSections = new();

			for (int i = 0; i < quiziesList.Count; ++i)
			{
				using (StreamReader sr = new(quiziesList[i]))
				{
					while (!sr?.EndOfStream ?? false)
					{
						question = sr?.ReadLine();
						wrigthAnswer = sr?.ReadLine();
						description = sr?.ReadLine();
						var = sr?.ReadLine();
						variantsAnswer = new();
						while (var != "" && var != null)
						{
							variantsAnswer.Add(var);
							var = sr?.ReadLine();
						}
						quizSections.Add(new(question ?? "", wrigthAnswer ?? "", description ?? "", variantsAnswer));
					}
				}
				name = quiziesList[i].Split('\\').Last();
				name = name.Split('.', StringSplitOptions.RemoveEmptyEntries)[0];
				using (FileStream fs = new(name + ".json", FileMode.OpenOrCreate, FileAccess.Write))
				{
					try
					{
						JsonSerializerOptions jso = new() { WriteIndented = true };
						JsonSerializer.Serialize(fs, quizSections, jso);
					} catch (Exception ex)
					{
						Console.WriteLine(ex.Message);
					}
				}
				quizSections.Clear();
			}
		}
	}
}