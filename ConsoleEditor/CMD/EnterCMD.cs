using System;
using System.Collections.Generic;

using PatientsInfo.Data;
using PatientsInfo.Entities;
using static ConsoleIO.Entering;

using ConsoleFormat;

using FileIO;
using ConsoleFormat.StylesEntities;

namespace ConsoleEditor.CMD {
	public partial class EnterCMD : IConsoleEditor {
		private DataContext dataContext;

		IFileIoController fileIoController;

		IEnumerable<Person>  sortingPeople;
		IEnumerable<Patient> sortingPatients;
		IEnumerable<Disease> sortingDiseases;

		public string[] strCMD;

		public SDiseases sDiseases;
		public SPatient sPatient;
		public SPerson sPerson;

		XmlFileIoController xmlFileIoController = new XmlFileIoController();
		BinarySerializationController binarySerializationController = new BinarySerializationController();

		public StringCommand[] Commands;

		public StringCommand lastCommand;

		public EnterCMD(DataContext dataContext, XmlFileIoController xmlFileIoController, BinarySerializationController binarySerializationController) {
			this.dataContext = dataContext;
			sortingDiseases = dataContext.Diseases;
			sortingPatients = dataContext.Patients;
			sortingPeople = dataContext.People;

			this.xmlFileIoController = xmlFileIoController;
			this.binarySerializationController = binarySerializationController;

			this.fileIoController = xmlFileIoController;

			this.sDiseases = new SDiseases(sortingDiseases);
			this.sPerson = new SPerson(sortingPeople);
			this.sPatient = new SPatient(sortingPatients);

			Init();
		}

		private void Init() {
			Commands = new StringCommand[]{
				new StringCommand("quit", Quit, "Вихід з програми"),
				new StringCommand("crttd", dataContext.CreateTestingData, "Створення тестових даних"),
				new StringCommand("clear", Console.Clear, "Очищення таблиць"),
				new StringCommand("help", Help, "Виведення наявних команд"),
				new StringCommand("lhelp", LHelp, "Повне виведення наявних команд"),
				new StringCommand("table", OutTable, "table [clear|person|patient|diseases] ; Виведення даних конкретної сутності у вигляді таблиці"),
				new StringCommand("ilu", ILU, ":-)"),
				new StringCommand("add", AddEntity, "add [person|patient|diseases]; додає певну сутність"),
				new StringCommand("rem", RemoveEntity, "rem [person|patient|diseases]; видаляє певну сутність"),
				new StringCommand("save", SaveData, "save [name]; збереження за вказаним setfio"),
				new StringCommand("load", LoadData, "load [name]; завантаження за вказаним setfio"),
				new StringCommand("xmlio", Xmlio, "xmlio [save|load|type] [name]; зберігання та завантаження у форматі XML)"),
				new StringCommand("bfio", Bfio, "bfio [save|load|type] [name]; зберігання та завантаження у форматі BF)"),
				new StringCommand("setfio", SetFIO, "setfio [xmlio|bfio]; Вказуємо тип зберігання"),
				new StringCommand("switch", SW, "Змінити редактор")

			};
		}

		public void SW() {}

		public int SwitchCommands(string strCMD) {

			this.strCMD = strCMD.Split(' ');
			if (this.strCMD.Length == 0) {
				return 1;
			}
			if (this.strCMD[0] == "switch") {
				return 2;
			}
			StringCommand strcommand;
			int i = 0;
			for (; i < Commands.Length; i++) {
				strcommand = Commands[i];
				if (strcommand.name == this.strCMD[0]) {
					strcommand.command();
					return 1;
				}
			}
			if (i == Commands.Length) {
				Console.WriteLine($"Команди {this.strCMD[0]} не існує. Введіть команду help чи lhelp, щоб вивести список всіх команд");
			}
			return 1;
		}

		public int Run() {
			string cmd;
			while (true) {
				cmd = InputString(" > ").ToLower();
				int res = SwitchCommands(cmd);
				if (res > 1) {
					return res;
				} 
			}
		}
	}
}
