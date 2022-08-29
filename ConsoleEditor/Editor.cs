using System;
using System.Collections.Generic;

using PatientsInfo.Data;
using PatientsInfo.Entities;
using static ConsoleIO.Entering;

using ConsoleFormat;

using FileIO;

using ConsoleFormat.StylesEntities;

namespace ConsoleEditor {
	public partial class Editor : IConsoleEditor{
		DataContext dataContext { get; set; }

		IFileIoController fileIoController { get; set; }

		IEnumerable<Person>  sortingPeople { get; set; }
		IEnumerable<Patient> sortingPatients { get; set; }
		IEnumerable<Disease> sortingDiseases { get; set; }

		SPatient sPatient;
		SDiseases sDiseases;
		SPerson sPerson;

		XmlFileIoController xmlFileIoController;
		BinarySerializationController binarySerializationController;

		public Editor(DataContext dataContext, XmlFileIoController xmlFileIoController, BinarySerializationController binarySerializationController) {
			this.dataContext = dataContext;
			sortingDiseases = dataContext.Diseases;
			sortingPatients = dataContext.Patients;
			sortingPeople   = dataContext.People;

			this.xmlFileIoController = xmlFileIoController;
			this.binarySerializationController = binarySerializationController;

			this.fileIoController = xmlFileIoController;

			sPatient = new SPatient(sortingPatients);
			sDiseases = new SDiseases(sortingDiseases);
			sPerson = new SPerson(sortingPeople);

			Init();
		}

		private CommandInfo[] commandsInfo;

		private void Init() {
			commandsInfo = new CommandInfo[]{
				new CommandInfo("Вийти", null),
				new CommandInfo("Змінити редактор", SW),
				new CommandInfo("Створити тестові дані", dataContext.CreateTestingData),
				new CommandInfo("Додати запис про особу", AddPerson),
				new CommandInfo("Додати запис про пацієнта", AddPatient),
				new CommandInfo("Додати запис про захворювання", AddDiseases),
				new CommandInfo("Видалити запис про особу", RemovePerson),
				new CommandInfo("Видалити запис про пацієнта", RemovePatient),
				new CommandInfo("Видалити запис про хворобу", RemoveDisease),
				new CommandInfo("Видалити усі записи", dataContext.Clear),
				new CommandInfo("Сортувати пацієнтів за ID", SortPatientByID),
				new CommandInfo("Сортувати осіб за ПІБ", SortPeopleByFullName),
				new CommandInfo("Сортувати осіб за станом здоров'я", SortPeopleByHealthStatus),
				new CommandInfo("Сортувати осіб за персональним номером", SortPeopleByPersonalNumber),
				new CommandInfo("Сортувати хвороби за МКХ", SortDiseaseByCodeICD),
				new CommandInfo("Пошук особи за першої букви прізвища", SearchFullNameByLetter),
				new CommandInfo("Пошук осіб за типом крові", SearchTypeBloodWithStr),
				new CommandInfo("Показати хворих пацієнтів", ShowSickPatients),
				new CommandInfo("Показати пацієнтів, хвороби яких не підтвердженні", ShowPatientsWithoutPatient),
				new CommandInfo("Пошук осіб за віком", ShowPeopleBySpecifiedAge),
				new CommandInfo("Вивести інформацію про особу за ID", ShowInformationPerson),
				new CommandInfo("Вивести інформацію про хворобу за назвою", ShowInformationDisease),
				new CommandInfo("Зберегти дані", SaveData),
				new CommandInfo("Завантажити дані", LoadData),
				new CommandInfo("Тип збереження: XML", SetFIOXML),
				new CommandInfo("Тип збереження: BD", SetFIOBD),

			};
		}



		public int Run() {
			Command command;
			while (true) {
				Console.Clear();
				Console.WriteLine("Реалізація класів для предметної області");
				OutData();
				Console.WriteLine();
				ShowCommandsMenu();
				command = EnterCommand();
				if (command == null) return 0;
				if (command == SW) return 2;
				command();
			}
		}

		public void SW() { }

		private void ShowCommandsMenu() {
			Console.WriteLine("Список команд меню");
			for (int i = 0; i < commandsInfo.Length; i++) {
				Console.WriteLine("\t{0, 2} - {1}", i, commandsInfo[i].title);
			}
		}

		private Command EnterCommand() {
			Console.WriteLine();
			return commandsInfo[EnterULong("Введіть номер команди", 0, (ulong)commandsInfo.Length - 1)].command;
		}

	}
}
