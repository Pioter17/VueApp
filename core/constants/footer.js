export const getFooter = (i18n) => {
  return {
    'items-per-page-options': [5, 10, 15, -1],
    'items-per-page-text': i18n.t('itemsPerPage'), // dynamiczne tłumaczenie
    'items-per-page-all-text': i18n.t('itemsAllText'),
    'page-text': i18n.t('{0}-{1} of {2}'),
  };
};
