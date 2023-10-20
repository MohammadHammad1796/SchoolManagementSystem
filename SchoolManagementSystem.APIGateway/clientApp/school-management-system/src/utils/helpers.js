export function getDateTimeInUTC(clientDateTimeString) {
  return new Date(clientDateTimeString).toISOString();
}

export function getDateInCLientZone(utcDateTimeString) {
  const utcDate = new Date(utcDateTimeString);
  const clientTimeZoneOffset = new Date().getTimezoneOffset();
  const localDate = new Date(utcDate.getTime() - clientTimeZoneOffset * 60000);
  const formattedDate = localDate.toISOString().split("T")[0];
  return formattedDate;
}

export function getDateBeforeYears(before) {
  let date = new Date();
  let fullYear = date.getFullYear();
  date.setFullYear(fullYear - before);
  return date;
}

export function getFirstDateOfYear(date) {
  let firstDayDate = new Date(date.getFullYear(), 0, 1);
  return firstDayDate;
}

export function getLastDateOfYear(date) {
  let lastDayDate = new Date(date.getFullYear(), 11, 31);
  return lastDayDate;
}

export function isFunctionAsync(func) {
  let isAsync = func.constructor.name === "AsyncFunction";
  isAsync = isAsync || func instanceof (async () => {}).constructor === true;
  return isAsync;
}
