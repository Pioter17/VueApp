const pol = {
  welcome: 'Witamy w aplikacji Zarządcy',
  servers: 'Serwery',
  applications: 'Aplikacje',
  tasks: 'Zadania',
  mainPage: 'Strona główna',
  systems: 'Twoje systemy',
  serversPage: {
    warning:
      'UWAGA! Ta akcja USUNIE wszystkie aplikacje i zadania przypisane do tego serwera!\n Rozważ najpierw ich ponowne przypisanie. Czy na pewno chcesz usunąć ten serwer?',
  },
  applicationsPage: {
    warning:
      'UWAGA! Ta akcja odłączy wszystkie zadania przypisane do tej aplikacji!',
  },
  tasksPage: {
    taskNotAssigned: 'To zadanie nie jest przypisane do żadnej aplikacji',
  },
  itemType: {
    server: 'Serwer',
    application: 'Aplikację',
    task: 'Zadanie',
  },
  deleteConfirm: 'Czy na pewno chcesz usunąć',
  delete: 'Usuń',
  cancel: 'Anuluj',
  save: 'Zapisz',
  addNew: 'Dodaj',
  edit: 'Edytuj',
  search: 'Szukaj',
  forms: {
    appName: 'Nazwa aplikacji',
    serverName: 'Nazwa serwera',
    taskName: 'Nazwa zadania',
    attachToServer: 'Przypisz do serwera',
    attachToApplication: 'Przypisz do aplikacji',
    attachTasks: 'Przypisz zadania',
    required: 'jest wymagane',
    serverRequired: 'Wybór serwera jest wymagany',
    emptyServer: 'Ten serwer nie ma przypisanych żadnych zadań',
    emptyServerApp: 'Ten serwer nie ma przypisanych żadnych aplikacji',
    serverInfo: `Możesz ustawić tutaj tylko nazwę serwera. Aby przypisać aplikacje i/lub zadania, przejdź do stron aplikacji i/lub zadań.
                 To rozwiązanie ma na celu zapobieganie niezamierzonym działaniom, takim jak odłączenie wszystkich zadań od innego serwera
                 z powodu ponownego przypisania aplikacji, do której są one przypisane.`,
  },
  headers: {
    name: 'Nazwa',
    creationDate: 'Data utworzenia',
    editionDate: 'Data edycji',
    tasks: 'Zadania',
    server: 'Serwer',
    application: 'Aplikacja',
    apps: 'Aplikacje',
  },
  goBack: 'Wróć',
  goToTasks: 'Przejdź do zadań',
  goToApplications: 'Przejdź do aplikacji',
  goToServers: 'Przejdź do serwerów',
  creationDate: 'Data utworzenia',
  editionDate: 'Data edycji',
  server: 'Serwer',
  application: 'Aplikacja',
  updateTask: 'ZAKTUALIZUJ ZADANIE',
  deleteTask: 'USUŃ ZADANIE',
  updateApplication: 'ZAKTUALIZUJ APLIKACJĘ',
  deleteApplication: 'USUŃ APLIKACJĘ',
  updateServer: 'ZAKTUALIZUJ SERWER',
  deleteServer: 'USUŃ SERWER',
};

export default pol;
