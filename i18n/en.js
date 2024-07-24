const eng = {
  welcome: 'Welcome to the Manager App',
  servers: 'Servers',
  applications: 'Applications',
  tasks: 'Tasks',
  mainPage: 'Main page',
  systems: 'Your systems',
  serversPage: {
    warning: `WARNING! This action will DELETE all applications and tasks attached to this server!\n
              Consider reattaching them first.
              Are you sure to delete this server?`,
  },
  applicationsPage: {
    warning:
      'WARNING! This action will detach all tasks attached to this application!',
  },
  tasksPage: {
    taskNotAssigned: 'This task is not attached to any application',
  },
  itemType: {
    server: 'Server',
    application: 'Application',
    task: 'Task',
  },
  deleteConfirm: 'Are you sure you want to delete',
  delete: 'Delete',
  cancel: 'Cancel',
  save: 'Save',
  addNew: 'Add new',
  edit: 'Edit',
  search: 'Search',
  forms: {
    appName: 'Application name',
    serverName: 'Server name',
    taskName: 'Task name',
    attachToServer: 'Attach to server',
    attachToApplication: 'Attach to application',
    attachTasks: 'Attach tasks',
    required: 'is required',
    serverRequired: 'Server selection is required',
    emptyServer: "This server doesn't have any tasks attached",
    emptyServerApp: "This server doesn't have any applications attached",
    serverInfo: `You can only set the server name here. To attach applications and/or task
                to it, go to applications and/or tasks pages. This solution is to prevent
                some unintentional actions, like detaching all tasks from another server,
                because of reattaching the application they are connected to.`,
  },
  headers: {
    name: 'Name',
    creationDate: 'Creation date',
    editionDate: 'Edition date',
    tasks: 'Tasks',
    server: 'Server',
    application: 'Application',
    apps: 'Applications',
  },
  goBack: 'Go back',
  goToTasks: 'Go to tasks',
  goToApplications: 'Go to applications',
  goToServers: 'Go to servers',
  creationDate: 'Creation date',
  editionDate: 'Edition date',
  server: 'Server',
  application: 'Application',
  updateTask: 'UPDATE TASK',
  deleteTask: 'DELETE TASK',
  updateApplication: 'UPDATE APPLICATION',
  deleteApplication: 'DELETE APPLICATION',
  updateServer: 'UPDATE SERVER',
  deleteServer: 'DELETE SERVER',
};

export default eng;
