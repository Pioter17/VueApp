export function isUniqueName(value, items, oldName) {
  if (oldName == '') {
    return !items.some((item) => item.name === value.trim());
  } else {
    return !items.some((item) => {
      return item.name === value.trim() && value.trim() !== oldName;
    });
  }
}
