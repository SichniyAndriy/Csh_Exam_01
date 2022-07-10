using System.Text;
using ConsoleApp1;
Console.OutputEncoding = Encoding.UTF8;
string usersPath = "users.dat";

Console.WriteLine(@"Вас вітає програма ""Вікторина""");
Console.WriteLine("1 - Авторизуватись");
Console.WriteLine("2 - Створити новий логін");
Console.WriteLine("3 - Перевірити оновлення");
Console.WriteLine("0 - Вийти з програми");

string? login = "";
bool flag = false;
switch (Methods.MakeChoice(3))
{
	case 1: 
		login = Methods.LogIn(usersPath);
		if (login != "") flag = true;
		break;
	case 2:
		login = Methods.CreateLogin(usersPath);
		if (login != "") flag = true;
		break;
	case 3: 
		Methods.ReadNewTextFiles();
		break;
	case 0:
		return;
}
int choice;
if (flag)
{
	do{
		Console.Clear();
		Console.WriteLine("\n1 - Почати вікторину");
		Console.WriteLine("2 - Переглянути результати");
		Console.WriteLine("3 - Видалити аккаунт");
		Console.WriteLine("0 - Вийти з програми");
		choice = Methods.MakeChoice(3);
		switch (choice)
		{
			case 1:
				Methods1.ShowBegin(login);
				break;
			case 2:
				Methods1.ShowResults(login);
				break;
			case 3:
				Methods1.DeleteAccount(login, usersPath);
				return;
		}
		Console.WriteLine("Натисніь будь-яку кнопку");
		Console.ReadKey();
	} while (choice is > 0 and < 4);
}
Console.WriteLine("Вихід з програми");