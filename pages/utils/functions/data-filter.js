export function filterData(
  data,
  serverName,
  applicationName,
  taskName,
  itemType
) {
  return data.filter((item) => {
    if (itemType == 'server') {
      const searchServerNameMatch = String(item.name)
        .toLowerCase()
        .includes(serverName.toLowerCase());
      return searchServerNameMatch;
    } else if (itemType == 'application') {
      const searchApplicationNameMatch = String(item.name)
        .toLowerCase()
        .includes(applicationName.toLowerCase());
      const searchServerNameMatch = String(item.server)
        .toLowerCase()
        .includes(serverName.toLowerCase());
      return searchApplicationNameMatch && searchServerNameMatch;
    } else {
      const searchApplicationNameMatch = String(item.application)
        .toLowerCase()
        .includes(applicationName.toLowerCase());
      const searchServerNameMatch = String(item.server)
        .toLowerCase()
        .includes(serverName.toLowerCase());
      const searchTaskNameMatch = item.name
        .toLowerCase()
        .includes(taskName.toLowerCase());
      return (
        searchApplicationNameMatch &&
        searchServerNameMatch &&
        searchTaskNameMatch
      );
    }
  });
}
