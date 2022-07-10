/*
 * Завдання 1
 * Створити додаток «Словники».
 * Основне завдання проєкту: зберігати словники різними мовами і дозволяти
 * користувачеві знаходити переклад потрібного слова або фрази. Інтерфейс 
 * додатку повинен надавати такі можливості:
 * ■ Створювати словник. Під час створення необхідно вказати тип словника.
 * Наприклад, англо-російський або російсько-англійський.
 * ■ Додавати слово і його переклад до вже існуючого словника. Оскільки слово 
 * може мати декілька перекладів, необхідно дотримуватися можливості створення
 * декількох варіантів перекладу.
 * ■ Замінювати слово або його переклад у словнику.
 * ■ Видаляти слово або переклад. Якщо слово видаляється, усі його переклади 
 * видаляються разом з ним. Не можна видалити переклад слова, якщо це останній 
 * варіант перекладу.
 * ■ Шукати переклад слова.
 * ■ Словники повинні зберігатися у файлах.
 * ■ Слово і варіанти його перекладів можна експортувати до окремого файлу результату.
 * ■ При старті програми потрібно показувати меню для роботи з програмою. 
 * Якщо вибір пункту меню відкриває підменю, тоді в ньому потрібно передбачити
 * можливість повернення до попереднього меню.
 */

using System.Collections;

namespace Dictionary
{
	internal class Vocabulary : IEnumerable<KeyValuePair<string, List<string>>>
	{
		public string? Name { get; set; }
		private SortedList<string, List<string>>? vocabulary;

		public Vocabulary() : this(""){ }
		public Vocabulary(string? name)
		{
			Name = name;
			vocabulary = new SortedList<string, List<string>>();
		}
#pragma warning disable CS8602 // Dereference of a possibly null reference.
		public List<string> this[string key] => vocabulary[key];

		public void AddTranslation(string key, string translation)
		{
			if (!vocabulary.ContainsKey(key)) _ = vocabulary.TryAdd(key, new List<string>());

			vocabulary[key].Add(translation);
		}

		public void AddTranslation(string key, string[] translation)
		{
			if (!vocabulary.ContainsKey(key)) _ = vocabulary.TryAdd(key, new List<string>());
			foreach (string el in translation)
			{
				vocabulary[key].Add(el);
			}
		}

		public void DeleteTranslation(string key, string translation)
		{
			if (vocabulary[key].Contains(translation) && vocabulary[key].Count > 1)
				vocabulary?[key].Remove(translation);
			else Console.WriteLine("Нема такого слова або варіантів перекладу один");
		}

		public void DeleteKey(string key)
		{
			if (vocabulary.ContainsKey(key))
				vocabulary.Remove(key);
			else Console.WriteLine("Нема такого ключа");
		}

		public void ReplaceTranslation(string key, string oldTr, string newTr)
		{
			if (vocabulary[key].Contains(oldTr))
			{
				vocabulary[key].Remove(oldTr);
				vocabulary[key].Add(newTr);
			}
			else Console.WriteLine("Нема такого слова");
		}
#pragma warning restore CS8602 // Dereference of a possibly null reference.
		public IEnumerator<KeyValuePair<string, List<string>>> GetEnumerator() => vocabulary.GetEnumerator();
		IEnumerator? IEnumerable.GetEnumerator() => vocabulary?.GetEnumerator();
	}
}