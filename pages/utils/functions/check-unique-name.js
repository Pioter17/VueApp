export function isUniqueName(value, items) {
  return !items.some((item) => item.name === value.trim());
}
