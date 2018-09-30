function dateString2Date(dateString) {
  var dt  = dateString.split(/\-|\s/);
  return new Date(dt.slice(0,3).reverse().join('-') + ' ' + dt[3]);
}

// multiple formats (e.g. yyyy/mm/dd or mm-dd-yyyy etc.)
function tryParseDateFromString(dateStringCandidateValue, format = "ymd") {
  if (!dateStringCandidateValue) { return null; }
  let mapFormat = format
          .split("")
          .reduce(function (a, b, i) { a[b] = i; return a;}, {});
  const dateStr2Array = dateStringCandidateValue.split(/[ :\-\/]/g);
  const datePart = dateStr2Array.slice(0, 3);
  let datePartFormatted = [
          +datePart[mapFormat.y],
          +datePart[mapFormat.m]-1,
          +datePart[mapFormat.d] ];
  if (dateStr2Array.length > 3) {
      dateStr2Array.slice(3).forEach(t => datePartFormatted.push(+t));
  }
  // test date validity according to given [format]
  const dateTrial = new Date(Date.UTC.apply(null, datePartFormatted));
  return dateTrial && dateTrial.getFullYear() === datePartFormatted[0] &&
         dateTrial.getMonth() === datePartFormatted[1] &&
         dateTrial.getDate() === datePartFormatted[2]
            ? dateTrial :
            null;
}