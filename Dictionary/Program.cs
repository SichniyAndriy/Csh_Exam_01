using Dictionary;
using System.Text;
Console.OutputEncoding = Encoding.UTF8;

Console.WriteLine("Вас вітає програма \"Словники\"");
Console.WriteLine("Цо ви хочете зробити:");
Console.WriteLine("1 - Вибрати існуючий словник");
Console.WriteLine("2 - Додати новий словник");
Console.WriteLine("3 - Редагувати існуючий словник");
Console.WriteLine("4 - Видалити існуючий словник");
Console.WriteLine("0 - Вийти з програми");

int choice = MakeChoice(4);
string tmpS;
List<string> listDict = new();
Vocabulary? voc;
switch(choice)
{
	case 1:
		voc = GetVocabulary();
		if (voc is not null)
		{
			Console.Write("Введіть ключ пошуку: ");
			listDict = SearchInDict(Console.ReadLine() ?? "", voc);
			foreach(string el in listDict)
			{
				Console.WriteLine(el);
			}
		}
		break;
	case 2:
		Console.Write("Назва нового словника: ");
		tmpS = Console.ReadLine() ?? "";
		CreateDict(tmpS);
		break;
	case 3:
		voc = GetVocabulary();
		if (voc is not null) EditDict(voc);
		break;
	case 4:
		DeleteDict();
		break;
	case 0:
		return;
}

int MakeChoice(int max)
{
	int countChoice = 3;
	do{
		Console.Write("Зробіть ваш вибір: ");
		_ = int.TryParse(Console.ReadLine(), out choice);
		--countChoice;
	} while ((choice < 0 || choice > max) && countChoice != 0);
	return choice;
}

List<string> GetListDict()
{
	string[] tmpSA = Directory.GetFiles(Directory.GetCurrentDirectory());
	for(int i = 0; i< tmpSA.Length; ++i)
	{
		tmpSA[i] = tmpSA[i].Split('\\').Last();
	}
	IEnumerable<string> newList = from el in tmpSA where el.EndsWith(".txt") select el;
	int count = 0;
	Console.WriteLine("Список словників:");
	foreach (string s in newList)
	{
		listDict.Add(s);
		Console.WriteLine($"{++count} {listDict[count - 1].Split('.').First()}");
	}
	if (!listDict.Any()) Console.WriteLine("Словників немає");
	return listDict;
}

Vocabulary? GetVocabulary()
{
	listDict = GetListDict();
	choice = MakeChoice(listDict.Count);
	if (choice >= 1 && choice <= listDict.Count)
	{
		voc = ReadDictFromFile(--choice);
		return voc;
	}
	else 
	{
		Console.WriteLine("Помилковий номер");
		return null;
	}
}

void CreateDict(string path)
{
	path += ".txt";
	if (File.Exists(path))
		Console.WriteLine("Такий словник існує");
	else
	{
		File.Create(path);
		Console.WriteLine($"{path[..path.LastIndexOf('.')]} словник створено");
	}
}

void DeleteDict()
{
	listDict = GetListDict();
	if (listDict.Any())
	{
		choice = MakeChoice(listDict.Count);
		try { 
			File.Delete(listDict[choice - 1]); 
			Console.WriteLine("Сдовник видалено");
		}
		catch (ArgumentOutOfRangeException) { Console.WriteLine("Помилковий номер словника"); }
	}
}

Vocabulary ReadDictFromFile(int choice)
{
	voc = new(listDict[choice].Split('.').First());
	using StreamReader reader = new(listDict[choice]);
	string key;
	string[] value;
	while (!reader.EndOfStream)
	{
		key = reader.ReadLine() ?? "";
#pragma warning disable CS8602 // Dereference of a possibly null reference.
		value = reader.ReadLine().Split(',', StringSplitOptions.RemoveEmptyEntries);
		for(int i = 0; i < value.Length; i++)
		{
			value[i] = value[i].Trim();
		}
#pragma warning restore CS8602 // Dereference of a possibly null reference.
		voc.AddTranslation(key, value);
	}
	return voc;
}

void WriteDictToFile(Vocabulary voc)
{
	using StreamWriter writer = new(voc?.Name + ".txt");
	foreach (KeyValuePair<string, List<string>> el in voc)
	{
		writer.WriteLine(el.Key);
		for (int i = 0; i < el.Value.Count; ++i)
		{
			writer.Write(el.Value[i]);
			if (i != el.Value.Count - 1)Console.Write(", ");
		}
		writer.WriteLine();
	}
}

List<string> SearchInDict(string word, Vocabulary v)
{
	return v[word];
}

void EditDict(Vocabulary voc)
{
	Console.Clear();
	Console.WriteLine("Що ви хочете зробити?");
	Console.WriteLine("1 - Додати переклад");
	Console.WriteLine("2 - Видалити ключове слово");
	Console.WriteLine("3 - Видалити переклад");
	Console.WriteLine("4 - Замінити переклад");
	Console.WriteLine("0 - Вийти з програми");
	string key;
	switch (MakeChoice(3))
	{
		case 1: 
			Console.Write("Введіть ключове слово: ");
			key = Console.ReadLine() ?? "";
			Console.Write("Введіть переклад(через Enter): ");
			List<string> tmpL = new();
			string tmp;
			tmp = Console.ReadLine() ?? "";
			while(tmp != "")
			{
				tmpL.Add(tmp);
				tmp = Console.ReadLine() ?? "";
			}
			voc.AddTranslation(key, tmpL.ToArray());
			break;
		case 2:
			Console.Write("Ведіть ключове слово: ");
			voc.DeleteKey(Console.ReadLine() ?? "");
			break;
		case 3:
			Console.Write("Ведіть ключове слово: ");
			key = Console.ReadLine() ?? "";
			Console.Write("Ведіть переклад, який треба видалити: ");
			string value = Console.ReadLine() ?? "";
			voc.DeleteTranslation(key, value);
			break;
		case 4:
			Console.WriteLine("Ведіть ключове слово: ");
			key = Console.ReadLine() ?? "";
			Console.WriteLine("Ведіть переклад, який треба замінити: ");
			string oldTr = Console.ReadLine() ?? "";
			Console.WriteLine("Ведіть новий варіант перекладу: ");
			string newTr = Console.ReadLine() ?? "";
			voc.ReplaceTranslation(key, oldTr, newTr);
			break;
		case 0: 
			return;
	}
	WriteDictToFile(voc);
}