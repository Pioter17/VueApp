export const getServersHeaders = (i18n) => [
  { text: i18n.t('headers.name'), value: 'name' },
  { text: i18n.t('headers.creationDate'), value: 'date' },
  { text: i18n.t('headers.editionDate'), value: 'edition' },
  { text: i18n.t('headers.apps') },
  { text: i18n.t('headers.tasks') },
  { text: ' ', value: 'actions', sortable: false },
];

export const getApplicationsHeaders = (i18n) => [
  { text: i18n.t('headers.name'), value: 'name' },
  { text: i18n.t('headers.creationDate'), value: 'date' },
  { text: i18n.t('headers.editionDate'), value: 'edition' },
  { text: i18n.t('headers.server'), value: 'server' },
  { text: i18n.t('headers.tasks') },
  { text: ' ' },
];

export const getTasksHeaders = (i18n) => [
  { text: i18n.t('headers.name'), value: 'name' },
  { text: i18n.t('headers.creationDate'), value: 'date' },
  { text: i18n.t('headers.editionDate'), value: 'edition' },
  { text: i18n.t('headers.server'), value: 'server' },
  { text: i18n.t('headers.application'), value: 'application' },
  { text: ' ' },
];

export const getApplicationDetailsHeaders = (i18n) => [
  { text: i18n.t('headers.name'), value: 'name' },
  { text: i18n.t('headers.creationDate'), value: 'date' },
  { text: i18n.t('headers.editionDate'), value: 'edition' },
  { text: i18n.t('headers.server'), value: 'server' },
];

export const getServerDetailsTasksHeaders = (i18n) => [
  { text: i18n.t('headers.name'), value: 'name' },
  { text: i18n.t('headers.creationDate'), value: 'date' },
  { text: i18n.t('headers.editionDate'), value: 'edition' },
  { text: i18n.t('headers.application'), value: 'application' },
];

export const getServerDetailsApplicationsHeaders = (i18n) => [
  { text: i18n.t('headers.name'), value: 'name' },
  { text: i18n.t('headers.creationDate'), value: 'date' },
  { text: i18n.t('headers.editionDate'), value: 'edition' },
  { text: i18n.t('headers.tasks'), value: 'tasks' },
];
