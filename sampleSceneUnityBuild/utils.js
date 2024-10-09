//Author of this file: https://github.com/Saberpeep/opecvjs-pupil-tracking
// The createFileFromUrl method was critical for the functionality of this application, without it i could not load the cascade xmls directly!
function Utils() {
  this.createFileFromUrl = function (path, url, callback) {
    let request = new XMLHttpRequest();
    request.open("GET", url, true);
    request.responseType = "arraybuffer";
    request.onload = function (ev) {
      if (request.readyState === 4) {
        if (request.status === 200) {
          let data = new Uint8Array(request.response);
          cv.FS_createDataFile("/", path, data, true, false, false);
          callback();
        } else {
          self.printError(
            "Failed to load " + url + " status: " + request.status
          );
        }
      }
    };
    request.send();
  };
}
