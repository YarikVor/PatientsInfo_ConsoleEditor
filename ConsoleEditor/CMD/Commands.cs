using System;
using System.Linq;
using ConsoleFormat.StylesEntities;
using PatientsInfo.Entities;
using static ConsoleIO.Entering;
using static ConsoleIO.Setting;
using static PatientsInfo.Entities.Enum;
using static RegexExpressions.RegexExpressions;
using ConsoleFormat;

namespace ConsoleEditor.CMD {
	public partial class EnterCMD {
		public const string formatERROR = "Не правильно введено значення чи не всі аргументи були записані. ";

		private void outError() => Console.WriteLine(formatERROR);

		public void Quit() {
			Pause();
			Environment.Exit(0);
		}

		public void Help() {
			int i = 0;
			int max = 0;
			while (max < Commands.Length) {
				max += 7;
				if (max > Commands.Length) {
					max = Commands.Length;
				}
				for (; i < max; i++) {
					Console.Write("{0,-7} ", Commands[i].name);
				}
				Console.WriteLine();
			}
		}

		public void LHelp() {
			for (int i = 0; i < Commands.Length; i++) {
				Console.WriteLine(" {0,-9} {1}", Commands[i].name, Commands[i].textHelp);
			}
		}

		public void OutTable() {
			if (strCMD.Length >= 2) {
				switch (strCMD[1]) {
					case "clear": {
						TableClear();
						break;
					}
					case "disease": {
						RunUniversalOutTable(sDiseases);
						break;
					}
					case "person": {
						RunUniversalOutTable(sPerson);
						break;
					}
					case "patient": {
						RunUniversalOutTable(sPatient);
						break;
					}
					default: {
						outError();
						break;
					}
				}
			} else {
				outError();
			}
		}

		public void RunUniversalOutTable(IStyleEntity styleEntity) {
			ConsoleKeyInfo consoleKeyInfo;
			bool isLongTable = false;
			do {
				Console.Clear();

				styleEntity.OutTable(true, isLongTable);

				Console.WriteLine("\t{0} - {1} з {2}", styleEntity.consoleTable.pos + 1, (styleEntity.consoleTable.pos + styleEntity.consoleTable.countView >= styleEntity.consoleTable.countData) ? styleEntity.consoleTable.countData : styleEntity.consoleTable.pos + styleEntity.consoleTable.countView, styleEntity.consoleTable.countData);

				Console.WriteLine("\t﻿﻿◄ - назад; ﻿﻿► - вперед;\nesc - вийти з таблиці;\nx - змінити вид таблиці;\nu/d - збільшити/зменшити к-ть елементів");

			l1:

				consoleKeyInfo = Console.ReadKey(true);

				switch (consoleKeyInfo.Key) {
					case ConsoleKey.LeftArrow: {
						styleEntity.consoleTable.Back();
						break;
					}
					case ConsoleKey.RightArrow: {
						styleEntity.consoleTable.Next();
						break;
					}
					case ConsoleKey.Escape: {
						break;
					}
					case ConsoleKey.X: {
						isLongTable = !isLongTable;
						break;
					}
					case ConsoleKey.U: {
						styleEntity.consoleTable.AddCountView();
						break;
					}
					case ConsoleKey.D: {
						styleEntity.consoleTable.RemCountView();
						break;
					}
					default: {
						goto l1;
					}
				}
			} while (consoleKeyInfo.Key != ConsoleKey.Escape);
			Console.Clear();
		}
		public void TableClear() {
			dataContext.Clear();
		}

		public void ILU() {
			Console.WriteLine("I love you Vika <3");
		}

		private void OutData() {
			OutPeopleData();
			OutPatientsData();
			OutDiseasesShortData();
		}

		private void OutPeopleData() {
			sPerson.OutTable();
			Console.WriteLine();
		}

		private void OutDiseasesShortData() {
			sDiseases.OutTable();
			Console.WriteLine();
		}

		private void OutPatientsData() {
			sPatient.OutTable();
			Console.WriteLine();
		}


		private void AddEntity() {
			if (strCMD.Length >= 2) {
				string arg = strCMD[1];
				switch (arg) {
					case "disease": {
						AddDiseases();
						break;
					}
					case "person": {
						AddPerson();
						break;
					}
					case "patient": {
						AddPatient();
						break;
					}
					default: {
						outError();
						break;
					}
				}

			} else {
				outError();
			}
		}
		public void AddDiseases() {
			Disease tmp = new Disease();

			tmp.Name = EnterString("Введіть назву захворювання");
			tmp.CodeICD = EnterString("Введіть МКХ", RCodeICD, "Латинська літера і дві цифри: A01, A04.1");
			tmp.Description = EnterString("Опишіть захворювання");

			dataContext.Diseases.Add(tmp);
		}
		public void AddPatient() {
			Patient inst = new Patient();

			inst.Person = SelectPatientByID();
			if (inst.Person == null) {
				Console.WriteLine("Такого пацієнта нема");
				Pause();
				return;
			}
			inst.disease = SelectDisease();
			if (inst.disease == null) {
				Console.WriteLine("Такої хвороби нема");
				Pause();
				return;
			}

			inst.IsSick = EnterBoolNull("На даний час хворіє нею?(пропуск, якщо не відомо");

			dataContext.Patients.Add(inst);
			Pause();
		}

		private Person SelectPatientByID() {
			ulong id = EnterULong("Введіть персональний номер особи");
			return dataContext.People.FirstOrDefault(e => e.Id == id);
		}

		private Disease SelectDisease() {
			Disease patient;
			string str = EnterString("Введіть назву хвороби");

			patient = dataContext.Diseases.FirstOrDefault(e => e.Name == str);
			return patient;
		}

		public void AddPerson() {
			Person inst = new Person();

			inst.FullName = EnterString("Введіть ПІБ", RFullName);
			inst.DiseaseOnsetDate = EnterDateTime("Введіть дату захворювання", DateTime.Now);
			inst.DateBirth = EnterDateTime("Введіть дата народження:", DateTime.Now);
			inst.Gender = (Gender)EnterEnum("Введіть стать (Ч/Ж)", GenderShortNameUA);
			inst.PhoneNumber = EnterString("Введіть номер телефону (не обов.)", RPhoneNumber);
			inst.ResidenceAddress = EnterString("Введіть адресу", 1);
			inst.HealthStatus = (HealthStatus)EnterEnum("Введіть стан здоров'я", HealthStatusNameUA);
			inst.TypeBlood = EnterString("Введіть тип крові", RTypeBlood);
			inst.Comment = EnterString("Опишіть пацієнта");

			dataContext.People.Add(inst);
		}

		public void RemoveEntity() {
			if (strCMD.Length >= 2) {
				string arg = strCMD[1];
				switch (arg) {
					case "disease": {
						RemoveDisease();
						break;
					}
					case "person": {
						RemovePerson();
						break;
					}
					case "patient": {
						RemovePatient();
						break;
					}
					default: {
						outError();
						break;
					}
				}

			} else {
				outError();
			}
		}

		public void RemovePatient() {
			ulong id = EnterULong("Введіть ID пацієнта");
			dataContext.Patients.Remove(dataContext.Patients.FirstOrDefault(e => e.Id == id));
		}

		public void RemoveDisease() {
			string str = EnterString("Введіть назву хвороби");
			dataContext.Diseases.Remove(dataContext.Diseases.FirstOrDefault(e => e.Name == str));
		}

		public void RemovePerson() {
			ulong id = EnterULong("Введіть персональний номер особи");
			dataContext.People.Remove(dataContext.People.FirstOrDefault(e => e.Id == id));
		}

		public void SaveData() {
			if (strCMD.Length >= 2) {
				try {
					fileIoController.Save(dataContext.dataSet, strCMD[1]);
				}
				catch (Exception e) {
					Console.WriteLine(e.Message);
				}
			} else {
				outError();
			}
		}
		public void LoadData() {
			if (strCMD.Length >= 2) {
				try {
					fileIoController.Load(dataContext.dataSet, strCMD[1]);
				}
				catch (Exception e) {
					Console.WriteLine(e.Message);
				}
			} else {
				outError();
			}
		}

		private void Xmlio() {
			if (strCMD.Length >= 3) {
				switch (strCMD[1]) {
					case "save": {
						xmlFileIoController.Save(dataContext.dataSet, strCMD[2]);
						break;
					}
					case "load": {
						try {
							xmlFileIoController.Load(dataContext.dataSet, strCMD[2]);
						}
						catch (Exception e) {
							Console.WriteLine(e.Message);
						}
						break;
					}
					case "type": {
						try {
							xmlFileIoController.FileExtension = strCMD[2];
						}
						catch (Exception e) {
							Console.WriteLine(e.Message);
						}
						break;
					}
					default: {
						Console.WriteLine($"Format type: \"{xmlFileIoController.FileExtension}\"");
						break;
					}
				}
			} else {
				outError();
			}
		}

		private void Bfio() {
			if (strCMD.Length >= 3) {
				switch (strCMD[1]) {
					case "save": {
						binarySerializationController.Save(dataContext.dataSet, strCMD[2]);
						break;
					}
					case "load": {
						try {
							binarySerializationController.Load(dataContext.dataSet, strCMD[2]);
						}
						catch (Exception e) {
							Console.WriteLine(e.Message);
						}
						break;
					}
					case "type": {
						try {
							binarySerializationController.FileExtension = strCMD[2];
						}
						catch (Exception e) {
							Console.WriteLine(e.Message);
						}
						break;
					}
					default: {
						Console.WriteLine($"Format type: \"{binarySerializationController.FileExtension}\"");
						break;
					}
				}

			} else {
				outError();
			}
		}

		private void SetFIO() {
			if (strCMD.Length >= 2) {
				switch (strCMD[1]) {
					case "xmlio": {
						fileIoController = xmlFileIoController;
						break;
					}
					case "bfio": {
						fileIoController = binarySerializationController;
						break;
					}
					default: {
						Console.WriteLine($"Format type: \"{fileIoController.FileExtension}\"");
						break;
					}
				}
			} else {
				outError();
			}
		}

		/*
				





				private void SortPatientByID() {
					sortingPatients = sortingPatients.OrderBy(e => e.Key);
				}
				private void SortPeopleByFullName() {
					sortingPeople = sortingPeople.OrderBy(e => e.FullName);
				}
				private void SortPeopleByHealthStatus() {
					sortingPeople = sortingPeople.OrderBy(e => e.HealthStatus);
				}
				private void SortPeopleByPersonalNumber() {
					sortingPeople = sortingPeople.OrderBy(e => e.PersonalNumber);
				}
				private void SortDiseaseByCodeICD() {
					sortingDiseases = sortingDiseases.OrderBy(e => e.CodeICD);
				}

				//My function
				public void SearchFullNameByLetter() {
					string symb = EnterString("Введіть перші букви прізвища");
					Console.WriteLine($"Люди з початком прізвища на \"{symb}\":");
					foreach (var obj in sortingPeople) {
						if (obj.FullName.StartsWith(symb)) {
							Console.WriteLine(obj);
						}
					}
					Pause();
				}

				public void SearchTypeBloodWithStr() {
					string str = EnterString("Введіть тип крові", RTypeBlood);
					Console.WriteLine($"Люди з {str} групою крові:");
					var init =
						from el in sortingPeople
						where el.TypeBlood == str
						select el;

					foreach (var obj in init) {
						Console.WriteLine(obj);
					}

					Pause();
				}

				public void ShowSickPatients() {
					Console.WriteLine($"Хворі пацієнти:");
					var init =
						from el in sortingPatients
						where el.IsSick == true
						select el;

					foreach (var obj in init) {
						Console.WriteLine(obj);
					}

					Pause();
				}

				public void ShowPatientsWithoutPatient() {
					Console.WriteLine($"Хвороби пацієнтів, що не підтверджені: ");
					var init =
						from el in sortingPatients
						where el.IsSick == null
						select el;

					foreach (var obj in init) {
						Console.WriteLine(obj);
					}

					Pause();
				}

				public void ShowPeopleBySpecifiedAge() {
					int
						age_min = EnterInt("Введіть мінімальне значення віку"),
						age_max = EnterInt("Введіть максимальне значення віку");
					Console.WriteLine();
					int year;
					foreach (var obj in sortingPeople) {
						year = Methods.Date.GetAge(obj.DateBirth);
						if (year >= age_min && year <= age_max) {
							Console.WriteLine(obj);
						}
					}
					Pause();
				}

				public void ShowInformationPerson() {
					Person inst = SelectPatientByID();
					if (inst == null) {
						Console.WriteLine("Такої особи нема");
						Pause();
						return;
					}
					Console.WriteLine(inst);
					Console.WriteLine("Хвороби");
					foreach (var el in sortingPatients) {
						if (el.Person == inst) {
							Console.WriteLine(el.ToShortString());
						}
					}
					Pause();
				}

				public void ShowInformationDisease() {
					Disease inst = SelectDisease();
					if (inst == null) {
						Console.WriteLine("Такої хвороби нема");
						Pause();
						return;
					}
					Console.WriteLine(inst);
					Pause();
				}

				public void SaveDataInXML() {
					xmlFileIoController.Save(dataContext.dataSet, "PatientsInfo");
				}

				public void LoadDataFromXML() {

				}
				public void AddDataFromXML() {

				}*/
	}
}
