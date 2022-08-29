using System;
using System.Linq;
using PatientsInfo.Entities;
using static ConsoleIO.Entering;
using static ConsoleIO.Setting;
using static PatientsInfo.Entities.Enum;
using static RegexExpressions.RegexExpressions;

namespace ConsoleEditor {
	public partial class Editor {
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

		public void AddDiseases() {
			Disease tmp = new Disease();

			tmp.Name = EnterString("Введіть назву захворювання");
			tmp.CodeICD = EnterString("Введіть МКХ", RCodeICD, "Латинська літера і дві цифри: A01, A04.1");
			tmp.Description = EnterString("Опишіть захворювання");

			dataContext.Diseases.Add(tmp);
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


		private void SortPatientByID() {
			sortingPatients = sortingPatients.OrderBy(e => e.Id);
		}
		private void SortPeopleByFullName() {
			sortingPeople = sortingPeople.OrderBy(e => e.FullName);
		}
		private void SortPeopleByHealthStatus() {
			sortingPeople = sortingPeople.OrderBy(e => e.HealthStatus);
		}
		private void SortPeopleByPersonalNumber() {
			sortingPeople = sortingPeople.OrderBy(e => e.Id);
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

		public void SaveData() {
			try {
				fileIoController.Save(dataContext.dataSet, "PatientInfo");
			}
			catch (Exception e) {
				Console.WriteLine(e.Message);
				Pause();
			}
			
		}
		public void LoadData() {
			try {
				fileIoController.Load(dataContext.dataSet, "PatientInfo");
			}
			catch (Exception e) {
				Console.WriteLine(e.Message);
				Pause();
			}
		}

		public void SetFIOXML() {
			fileIoController = xmlFileIoController;
		}

		public void SetFIOBD() {
			fileIoController = binarySerializationController;
		}


	}
}
