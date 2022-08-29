using System;
using System.Text.RegularExpressions;

namespace ConsoleIO {
	public static class Entering {
		/// <summary>
		/// Для виведення у такому форматі
		/// </summary>
		public static string format = "{0,20}: ";


		/// <summary>
		/// Введення з консолі рядка
		/// </summary>
		/// <param name="title"></param>
		/// <returns></returns>
		public static string EnterString(string title) {
			Console.Write(format, title);
			return Console.ReadLine();
		}

		/// <summary>
		/// Введення з консолі рядка
		/// </summary>
		/// <param name="title"></param>
		/// <returns></returns>
		public static string InputString(string title) {
			Console.Write(title);
			return Console.ReadLine();
		}

		/// <summary>
		/// Введення з консолі рядка з заданою межею від <paramref name="min"/>
		/// </summary>
		/// <param name="title"></param>
		/// <param name="min"></param>
		/// <returns></returns>
		public static string EnterString(string title, int min) {
			string res;
			while (true) {
				Console.Write(format, title);
				res = Console.ReadLine();
				if (res.Length >= min) {
					Console.WriteLine($"Мінімальна довжина рядка становить {min}");
					continue;
				}
				return res;
			}
		}


		/// <summary>
		/// Введення з консолі рядка з заданою межею: від <paramref name="min"/> до <paramref name="max"/>
		/// </summary>
		/// <param name="title"></param>
		/// <param name="min"></param>
		/// <param name="max"></param>
		/// <returns></returns>
		public static string EnterString(string title, int min, int max) {
			string res;
			while (true) {
				Console.Write(format, title);
				res = Console.ReadLine();
				if (res.Length >= min && res.Length <= max) {
					Console.WriteLine($"Довжина рядка ({res.Length}) має бути у межах від {min} до {max}.");
					continue;
				}
				return res;
			}
		}

		/// <summary>
		/// Введення з консолі рядка з заданим регулярним виразом <paramref name="regex"/>
		/// </summary>
		/// <param name="title"></param>
		/// <param name="regex"></param>
		/// <returns></returns>
		public static string EnterString(string title, Regex regex) {
			string res;
			while (true) {
				Console.Write(format, title);
				res = Console.ReadLine();
				if (regex.IsMatch(res)) {
					return res;
				}
				Console.WriteLine("Неправильне форматування!");
			}
		}


		public static string EnterString(string title, Regex regex, string description) {
			string res;
			while (true) {
				Console.Write(format, title);
				res = Console.ReadLine();
				if (regex.IsMatch(res)) {
					return res;
				}
				Console.WriteLine("Неправильно введений формат. " + description);
			}
		}



		/// <summary>
		/// Введення з консолі дати.<br/>
		/// При введені пустого рядка повертає поточний час.
		/// </summary>
		/// <param name="title"></param>
		/// <returns></returns>
		public static DateTime EnterDateTime(string title) {
			string str;
			DateTime res;
			while (true) {
				Console.Write(format, title);
				str = Console.ReadLine();
				if (str.Length == 0) return DateTime.Now;
				if (DateTime.TryParse(str, out res)) {
					return res;
				}
				Console.WriteLine("Неправильно введена дата! Формат:\n" +
					"20.10\t -> 20.10.2022\n" +
					"06/02/22\t -> 06/02/22");
			}
		}


		/// <summary>
		/// Введення з консолі дати з границею до <paramref name="max"/>.<br/>
		/// При введені пустого рядка повертає поточний час.<br/>
		/// </summary>
		/// <param name="title"></param>
		/// <returns></returns>
		public static DateTime EnterDateTime(string title, DateTime max) {
			DateTime res;
			string str;
			while (true) {
				Console.Write(format, title);
				str = Console.ReadLine();
				if (str.Length == 0) return DateTime.Now;
				if (DateTime.TryParse(str, out res) == false) {
					Console.WriteLine("Неправильно введена дата! Формат:\n" +
						"20.10\t -> 20.10.2022\n" +
						"06/02/22\t -> 06/02/22");
					continue;
				}
				if (res.Date > max) {
					Console.WriteLine("Дата не може виходити за межі цієї дати: {0:d}", max);
					continue;
				}
				return res;
			}
		}

		/// <summary>
		/// Введення логічного виразу (булевого).<br/>
		/// Так, якщо + чи так<br/>
		/// Ні, якщо - чи ні
		/// </summary>
		/// <param name="title"></param>
		/// <returns></returns>
		public static bool EnterBool(string title) {
			string str;
			while (true) {
				Console.Write(format, title);
				str = Console.ReadLine();

				if (str == "+" || str == "Так" || str == "так") return true;
				if (str == "-" || str == "Ні"  || str == "ні")  return false;

				Console.WriteLine("Неправильно введено логічний вираз.\n" +
					"Для підтвердження: +, так;\n" +
					"Для заперечення: -, ні");
			}
		}


		public static bool? EnterBoolNull(string title) {
			string str;
			while (true) {
				Console.Write(format, title);
				str = Console.ReadLine();
				if (str.Length == 0) return null;
				if (str == "+" || str == "Так" || str == "так") return true;
				if (str == "-" || str == "Ні" || str == "ні") return false;

				Console.WriteLine("Неправильно введено логічний вираз.\n" +
					"Для підтвердження: +, так;\n" +
					"Для заперечення: -, ні");
			}
		}


		public static int EnterEnum(string title, string[] enums) {
			string str;
			while (true) {
				Console.Write(format, title);
				str = Console.ReadLine();
				for (int i = 0; i < enums.Length; i++)
					if (str == enums[i])
						return i;

				Console.Write("Такого перерахування нема. Наявні наступні: ");
				for (int i = 0; i < enums.Length; i++)
					Console.Write("{0}, ", enums[i]);
				Console.WriteLine();
			}
		}

		public static ulong EnterULong(string title) {
			ulong res;
			while (true) {
				Console.Write(format, title);
				if (ulong.TryParse(Console.ReadLine(), out res)) {
					return res;
				}
				Console.WriteLine("Неправильно введено число");
			}
		}



		public static ulong EnterULong(string title, ulong min, ulong max) {
			ulong res;
			while (true) {
				Console.Write(format, title);
				if (ulong.TryParse(Console.ReadLine(), out res) == false) {
					Console.WriteLine("Неправильно введено число");
					continue;
				}
				if (res >= min && res <= max) return res;
				Console.WriteLine($"Задвне число не входить межі від {min} до {max}");
			}
		}

		public static int EnterInt(string title) {
			int res;
			while (true) {
				Console.Write(format, title);
				if (int.TryParse(Console.ReadLine(), out res)) {
					return res;
				}
				Console.WriteLine("Неправильно введено число");
			}
		}

	}
}
